using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comfile.ComfilePi
{
    public class MCP4725 : I2C
    {
        internal MCP4725(int pi, uint bus, byte address)
            : base(pi, bus, address)
        {

        }

        public void Init(ushort value)
        {
            uint address = (0b011 << 5) | (0b00 << 3) | (0b00 << 2) | 0;
            value = (ushort)(value << 4);  // only 12 bits
            value = BitConverter.ToUInt16(BitConverter.GetBytes(value).Reverse().ToArray(), 0);
            base.Write(address, value);
        }

        public void Write(ushort value)
        {
            uint address = (0b010 << 5) | (0b00 << 3) | (0b00 << 2) | 0;
            value = (ushort)(value << 4);  // only 12 bits
            value = BitConverter.ToUInt16(BitConverter.GetBytes(value).Reverse().ToArray(), 0);
            base.Write(address, value);
        }
    }
}
