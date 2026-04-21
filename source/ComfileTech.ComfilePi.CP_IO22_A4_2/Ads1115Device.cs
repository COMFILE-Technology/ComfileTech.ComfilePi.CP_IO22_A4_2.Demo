using System;
using System.Device.I2c;
using System.Threading;

namespace ComfileTech.ComfilePi.CP_IO22_A4_2
{
    /// <summary>
    /// Minimal ADS1115 single-ended reader for channels AIN0-AIN3.
    /// </summary>
    internal sealed class Ads1115Device : IDisposable
    {
        const byte ConversionRegister = 0x00;
        const byte ConfigRegister = 0x01;

        const ushort StartSingleConversion = 0x8000;
        const ushort RangeFs6144 = 0x0000;
        const ushort SingleShotMode = 0x0100;
        const ushort DataRate128Sps = 0x0080;
        const ushort ComparatorDisabled = 0x0003;

        const double FullScaleVoltage = 6.144;
        const double VoltagePerBit = FullScaleVoltage / 32768.0;

        readonly I2cDevice _device;

        internal Ads1115Device(I2cDevice device)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
        }

        internal double ReadVoltage(int channel)
        {
            WriteConfig((ushort)(
                StartSingleConversion |
                GetMultiplexerBits(channel) |
                RangeFs6144 |
                SingleShotMode |
                DataRate128Sps |
                ComparatorDisabled));

            WaitForConversion();

            short rawValue = unchecked((short)ReadRegister(ConversionRegister));

            // Single-ended measurements should not be negative. Clamp any
            // transient invalid value so the UI never displays a bogus voltage.
            if (rawValue < 0)
            {
                rawValue = 0;
            }

            return rawValue * VoltagePerBit;
        }

        static ushort GetMultiplexerBits(int channel)
        {
            return channel switch
            {
                0 => 0x4000,
                1 => 0x5000,
                2 => 0x6000,
                3 => 0x7000,
                _ => throw new ArgumentOutOfRangeException(nameof(channel))
            };
        }

        void WaitForConversion()
        {
            while ((ReadRegister(ConfigRegister) & StartSingleConversion) == 0)
            {
                Thread.Sleep(1);
            }
        }

        ushort ReadRegister(byte register)
        {
            Span<byte> registerAddress = stackalloc byte[] { register };
            Span<byte> buffer = stackalloc byte[2];

            _device.WriteRead(registerAddress, buffer);
            return (ushort)((buffer[0] << 8) | buffer[1]);
        }

        void WriteConfig(ushort value)
        {
            Span<byte> buffer = stackalloc byte[3];
            buffer[0] = ConfigRegister;
            buffer[1] = (byte)(value >> 8);
            buffer[2] = (byte)(value & 0xFF);

            _device.Write(buffer);
        }

        public void Dispose()
        {
            _device.Dispose();
        }
    }
}
