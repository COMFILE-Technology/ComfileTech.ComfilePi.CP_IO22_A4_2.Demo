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
        static readonly Ads1115Device _ads1115;
        static readonly Thread _thread;

        static AnalogInput()
        {
            // Create each channel
            var list = new List<AnalogInput>();
            for (int i = 0; i < 4; i++)
            {
                list.Add(new AnalogInput(i + 1));
            }
            AnalogInputs = list.AsReadOnly();

            // Begin reading the analog inputs
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _ads1115 = new Ads1115Device(I2cDevice.Create(new I2cConnectionSettings(1, 0x48)));
                AppDomain.CurrentDomain.ProcessExit += (s, e) => _ads1115.Dispose();

                _thread = new Thread(Read);
                _thread.IsBackground = true;
                _thread.Start();
            }
        }

        static void Read()
        {
            while (true)
            {
                for (int index = 0; index < AnalogInputs.Count; index++)
                {
                    var input = AnalogInputs[index];
                    input.Voltage = _ads1115.ReadVoltage(index);
                    Thread.Yield();
                }
            }
        }

        internal static IReadOnlyList<AnalogInput> AnalogInputs { get; }

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
