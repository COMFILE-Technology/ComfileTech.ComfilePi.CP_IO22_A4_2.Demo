using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Runtime.InteropServices;
using System.Threading;

namespace ComfileTech.ComfilePi.CP_IO22_A4_2
{
    /// <summary>
    /// An analog input pin on the IO board.
    /// </summary>
    public class AnalogInput
    {
        static readonly object _syncRoot = new object();
        static readonly IReadOnlyList<AnalogInput> _analogInputs;
        static Ads1115Device _ads1115;
        static Thread _thread;
        static bool _started;
        static volatile bool _disposed;

        static AnalogInput()
        {
            // Create each channel
            var list = new List<AnalogInput>();
            for (int i = 0; i < 4; i++)
            {
                list.Add(new AnalogInput(i + 1));
            }
            _analogInputs = list.AsReadOnly();
        }

        static void EnsureStarted()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_started || _disposed)
                {
                    return;
                }

                _ads1115 = new Ads1115Device(I2cDevice.Create(new I2cConnectionSettings(1, 0x48)));
                AppDomain.CurrentDomain.ProcessExit += (s, e) => DisposeAds1115Device();

                _thread = new Thread(Read)
                {
                    IsBackground = true
                };
                _thread.Start();

                _started = true;
            }
        }

        static void Read()
        {
            try
            {
                while (!_disposed)
                {
                    for (int index = 0; index < _analogInputs.Count && !_disposed; index++)
                    {
                        var input = _analogInputs[index];
                        input.Voltage = _ads1115.ReadVoltage(index);
                        Thread.Yield();
                    }
                }
            }
            catch (Exception) when (_disposed)
            { }
        }

        internal static IReadOnlyList<AnalogInput> AnalogInputs
        {
            get
            {
                EnsureStarted();
                return _analogInputs;
            }
        }

        internal static void DisposeAds1115Device()
        {
            Thread thread;
            Ads1115Device ads1115;

            lock (_syncRoot)
            {
                if (_disposed)
                {
                    return;
                }

                _disposed = true;
                thread = _thread;
                ads1115 = _ads1115;
            }

            if (thread != null && thread != Thread.CurrentThread && thread.IsAlive)
            {
                thread.Join(1000);
            }

            ads1115?.Dispose();

            if (thread != null && thread != Thread.CurrentThread && thread.IsAlive)
            {
                thread.Join(1000);
            }
        }

        private AnalogInput(int channel)
        {
            Channel = channel;
        }

        /// <summary>
        /// The channel number of this analog input.
        /// </summary>
        public int Channel { get; }

        double _voltage;

        /// <summary>
        /// The voltage as read from the analog input.
        /// </summary>
        public double Voltage
        {
            get => _voltage;
            private set
            {
                if (_voltage != value)
                {
                    _voltage = value;
                    VoltageChanged?.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Fires when the voltage of this analog input changes.
        /// </summary>
        public event Action<AnalogInput> VoltageChanged;
    }
}
