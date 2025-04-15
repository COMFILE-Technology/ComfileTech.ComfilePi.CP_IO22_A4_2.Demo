using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comfile.ComfilePi
{
    public class ADCChannel
    {
        internal ADCChannel(ADS1115 adc, byte channel)
        {
            _adc = adc;
            _channel = channel;
        }

        readonly ADS1115 _adc;
        readonly byte _channel;

        public byte Channel
        {
            get { return _channel; }
        }

        public short Read()
        {
            return _adc.ReadValue((byte)(_channel - 1));
        }
    }
}
