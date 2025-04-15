using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comfile.ComfilePi
{
    public class Output
    {
        internal Output(int pi, uint number)
        {
            _pi = pi;
            _number = number;
            pigpio.set_mode(_pi, _number, pigpio.PI_OUTPUT);
            pigpio.set_pull_up_down(_pi, _number, pigpio.PI_PUD_OFF);
        }

        readonly int _pi;
        readonly uint _number;

        public uint Number
        {
            get { return _number; }
        }

        public void On()
        {
            pigpio.gpio_write(_pi, _number, 1);
        }

        public void Off()
        {
            pigpio.gpio_write(_pi, _number, 0);
        }

        public bool IsOn
        {
            get
            {
                return pigpio.gpio_read(_pi, _number) == 1;
            }
            set
            {
                if (value)
                {
                    On();
                }
                else
                {
                    Off();
                }
            }
        }

        public bool IsOff
        {
            get { return !IsOff; }
            set { IsOn = !value; }
        }
    }
}
