using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comfile.ComfilePi
{
    public class Input
    {
        internal Input(int pi, uint number)
        {
            _pi = pi;
            _number = number;
            pigpio.set_mode(_pi, _number, pigpio.PI_INPUT);
        }

        readonly int _pi;
        readonly uint _number;

        public uint Number
        {
            get { return _number; }
        }

        public bool IsOn
        {
            get { return pigpio.gpio_read(_pi, _number) == 1; }
        }

        public bool IsOff
        {
            get { return !IsOff; }
        }
    }
}
