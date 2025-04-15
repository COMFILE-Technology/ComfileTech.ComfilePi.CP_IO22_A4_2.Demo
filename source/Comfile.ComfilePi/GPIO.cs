using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comfile.ComfilePi
{
    public static class GPIO
    {
        static bool _isInitialized;
        static int __pi;
        static int _pi
        {
            get
            {
                if (!_isInitialized)
                {
                    AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

                    int result = pigpio.pigpio_start(IntPtr.Zero, IntPtr.Zero);
                    if (result < 0)
                    {
                        throw new Exception("GPIO.Open failed");
                    }

                    _isInitialized = true;

                    __pi = result;
                }

                return __pi;
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            // Dispose of all I2C devices
            _adcs = null;
            _adc4 = null;
            _adc3 = null;
            _adc2 = null;
            _adc1 = null;
            _adc = null;
            _dacs = null;
            _dac2 = null;
            _dac1 = null;
            GC.Collect();

            pigpio.pigpio_stop(_pi);
        }

        #region Inputs
        static readonly Input _input04 = new Input(_pi, 4);
        public static Input Input04
        {
            get { return _input04; }
        }

        static readonly Input _input05 = new Input(_pi, 5);
        public static Input Input05
        {
            get { return _input05; }
        }

        static readonly Input _input06 = new Input(_pi, 6);
        public static Input Input06
        {
            get { return _input06; }
        }

        static readonly Input _input07 = new Input(_pi, 7);
        public static Input Input07
        {
            get { return _input07; }
        }

        static readonly Input _input08 = new Input(_pi, 8);
        public static Input Input08
        {
            get { return _input08; }
        }

        static readonly Input _input09 = new Input(_pi, 9);
        public static Input Input09
        {
            get { return _input09; }
        }

        static readonly Input _input10 = new Input(_pi, 10);
        public static Input Input10
        {
            get { return _input10; }
        }

        static readonly Input _input11 = new Input(_pi, 11);
        public static Input Input11
        {
            get { return _input11; }
        }

        static readonly Input _input12 = new Input(_pi, 12);
        public static Input Input12
        {
            get { return _input12; }
        }

        static readonly Input _input13 = new Input(_pi, 13);
        public static Input Input13
        {
            get { return _input13; }
        }

        static readonly Input _input16 = new Input(_pi, 16);
        public static Input Input16
        {
            get { return _input16; }
        }

        static readonly Input[] _inputs = new Input[]
        {
            _input04,
            _input05,
            _input06,
            _input07,
            _input08,
            _input09,
            _input10,
            _input11,
            _input12,
            _input13,
            _input16
        };
        public static ReadOnlyCollection<Input> Inputs
        {
            get { return Array.AsReadOnly(_inputs); }
        }

        #endregion Inputs

        #region Outputs
        static readonly Output _output17 = new Output(_pi, 17);
        public static Output Output17
        {
            get { return _output17; }
        }

        static readonly Output _output18 = new Output(_pi, 18);
        public static Output Output18
        {
            get { return _output18; }
        }

        static readonly Output _output19 = new Output(_pi, 19);
        public static Output Output19
        {
            get { return _output19; }
        }

        static readonly Output _output20 = new Output(_pi, 20);
        public static Output Output20
        {
            get { return _output20; }
        }

        static readonly Output _output21 = new Output(_pi, 21);
        public static Output Output21
        {
            get { return _output21; }
        }

        static readonly Output _output22 = new Output(_pi, 22);
        public static Output Output22
        {
            get { return _output22; }
        }

        static readonly Output _output23 = new Output(_pi, 23);
        public static Output Output23
        {
            get { return _output23; }
        }

        static readonly Output _output24 = new Output(_pi, 24);
        public static Output Output24
        {
            get { return _output24; }
        }

        static readonly Output _output25 = new Output(_pi, 25);
        public static Output Output25
        {
            get { return _output25; }
        }

        static readonly Output _output26 = new Output(_pi, 26);
        public static Output Output26
        {
            get { return _output26; }
        }

        static readonly Output _output27 = new Output(_pi, 27);
        public static Output Output27
        {
            get { return _output27; }
        }

        static readonly Output[] _outputs = new Output[] 
        {
            _output17,
            _output18,
            _output19,
            _output20,
            _output21,
            _output22,
            _output23,
            _output24,
            _output25,
            _output26,
            _output27,
        };
        public static ReadOnlyCollection<Output> Outputs
        {
            get { return Array.AsReadOnly(_outputs); }
        }
        #endregion Outputs

        #region ADC
        static ADS1115 _adc = new ADS1115(_pi, 1);

        static ADCChannel _adc1 = new ADCChannel(_adc, 1);
        public static ADCChannel ADC1
        {
            get { return _adc1; }
        }

        static ADCChannel _adc2 = new ADCChannel(_adc, 2);
        public static ADCChannel ADC2
        {
            get { return _adc2; }
        }

        static ADCChannel _adc3 = new ADCChannel(_adc, 3);
        public static ADCChannel ADC3
        {
            get { return _adc3; }
        }

        static ADCChannel _adc4 = new ADCChannel(_adc, 4);
        public static ADCChannel ADC4
        {
            get { return _adc4; }
        }

        public static ADCChannel[] _adcs = new ADCChannel[]
        {
            _adc1,
            _adc2,
            _adc3,
            _adc4
        };
        public static ReadOnlyCollection<ADCChannel> ADCs
        {
            get { return Array.AsReadOnly(_adcs); }
        }
        #endregion ADC

        #region DAC
        static MCP4725 _dacDev1 = new MCP4725(_pi, 1, 0x62);
        static MCP4725 _dacDev2 = new MCP4725(_pi, 1, 0x63);

        static DACChannel _dac1 = new DACChannel(_dacDev1, 1);
        public static DACChannel DAC1
        {
            get { return _dac1; }
        }

        static DACChannel _dac2 = new DACChannel(_dacDev2, 2);
        public static DACChannel DAC2
        {
            get { return _dac2; }
        }

        public static DACChannel[] _dacs = new DACChannel[]
        {
            _dac1,
            _dac2,
        };
        public static ReadOnlyCollection<DACChannel> DACs
        {
            get { return Array.AsReadOnly(_dacs); }
        }
        #endregion
    }
}
