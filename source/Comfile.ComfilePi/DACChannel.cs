using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comfile.ComfilePi
{
    public class DACChannel
    {
        internal DACChannel(MCP4725 dac, byte channel)
        {
            _dac = dac;
            _channel = channel;
        }

        readonly MCP4725 _dac;
        readonly byte _channel;

        public byte Channel
        {
            get { return _channel; }
        }

        public void Init(ushort value)
        {
            _dac.Init(value);
        }

        public void Write(ushort value)
        {
            _dac.Write(value);
        }
    }
}
