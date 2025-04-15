using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Comfile.ComfilePi
{
    internal class ADS1115 : I2C
    {
        const uint CONVERSION = 0x00;
        const uint CONFIG = 0x01;

        internal ADS1115(int pi, uint bus)
            : base(pi, bus, 0x48)
        {
            
        }

        public short ReadValue(byte channel)
        {
            ushort config = (ushort)
            (
                (1 << 15)                 // Start conversion
                | (1 << 14)               // non-differential
                | (channel << 12)         // channel
                | (0b000 << 9)            // 6.144V
                | (1 << 8)                // Single-shot
                | (0b100 << 5)            // 128SPS
                | (0 << 4)
                | (0 << 3)
                | (0 << 2)
                | (0b11)                  // Disable comparator
            );
            WriteBE(CONFIG, config);

            Thread.Sleep(10);

            return ReadInt16BE(CONVERSION);
        }
    }
}
