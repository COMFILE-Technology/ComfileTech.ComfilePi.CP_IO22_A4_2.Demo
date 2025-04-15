using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnitsNet;

namespace ComfileTech.ComfilePi.CP_IO22_A4_2
{
    /// <summary>
    /// An analog output on the IO board.
    /// </summary>
    public class AnalogOutput
    {
        internal AnalogOutput(int channel)
        {
            Channel = channel;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _i2cDevice = I2cDevice.Create(new I2cConnectionSettings(1, 0x62 + (Channel - 1)));

                Init(0);
            }
        }

        readonly I2cDevice _i2cDevice;

        void Init(ushort value)
        {
            byte address = (0b011 << 5) | (0b00 << 3) | (0b00 << 2) | 0;
            Write(address, value);
        }

        void Write(ushort value)
        {
            byte address = (0b010 << 5) | (0b00 << 3) | (0b00 << 2) | 0;
            Write(address, value);
        }

        void Write(double voltage)
        {
            Write((ushort)Math.Round(voltage * 4095 / 5.0)); // 12 bits, 0 ~ 5V
        }

        void Write(byte address, ushort value)
        {
            value = (ushort)(value << 4);  // only 12 bits
            _i2cDevice.Write(new byte[] { address, (byte)((value >> 8) & 0xFF), (byte)(value & 0xFF) });
        }

        /// <summary>
        /// The channel number of this analog output.
        /// </summary>
        public int Channel { get; }

        private double _voltage;
        /// <summary>
        /// The raw binary value from 0 ~ 5.0V (12 bits) that is written to the analog output.
        /// </summary>
        public double Voltage
        {
            get => _voltage;
            set
            {
                // Coerce the value to the correct range
                if (value >= 5.0)
                {
                    value = 5.0;
                }
                else if (value < 0.0)
                {
                    value = 0.0;
                }

                // Set the value
                if (_voltage != value)
                {
                    _voltage = value;

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        Write(_voltage);
                    }
                }
            }
        }
    }
}
