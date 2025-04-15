using Iot.Device.Ads1115;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ComfileTech.ComfilePi.CP_IO22_A4_2
{
    /// <summary>
    /// An analog input pin on the IO board.
    /// </summary>
    public class AnalogInput
    {
        static readonly Ads1115 _ads1115;
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
                _ads1115 = new Ads1115(I2cDevice.Create(new I2cConnectionSettings(1, 0x48)));
                _ads1115.DataRate = DataRate.SPS128;
                _ads1115.MeasuringRange = MeasuringRange.FS6144;
                _ads1115.InputMultiplexer = InputMultiplexer.AIN0;
                _ads1115.DeviceMode = DeviceMode.PowerDown;

                _thread = new Thread(Read);
                _thread.IsBackground = true;
                _thread.Start();
            }
        }

        static void Read()
        {
            while(true)
            {
                var index = (int)_ads1115.InputMultiplexer - 4;
                var input = AnalogInputs[index];
                input.Voltage = _ads1115.ReadVoltage().Volts;

                // Advance to the next channel, and start conversion.
                index++;
                _ads1115.InputMultiplexer = InputMultiplexer.AIN0 + (index % 4);

                Thread.Yield();
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
