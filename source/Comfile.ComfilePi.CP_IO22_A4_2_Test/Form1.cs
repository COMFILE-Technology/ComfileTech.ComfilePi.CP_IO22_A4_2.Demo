using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Comfile.ComfilePi;

namespace Comfile.ComfilePi.CP_IO22_A4_2_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var asm = Assembly.GetExecutingAssembly();
            Cursor = new Cursor(asm.GetManifestResourceStream("Comfile.ComfilePi.CP_IO22_A4_2_Test.Resources.hiddencursor.cur"));
        }

        Dictionary<uint, Lamp> _lamps;
        Dictionary<uint, Button> _buttons;
        Dictionary<byte, Label> _adcLabels;

        Thread _pollingThread;
        volatile bool _stopThread = false;

        volatile bool _dac1Init = false;
        volatile bool _dac2Init = false;

        void PollGPIO()
        {
            while (!_stopThread)
            {
                short[] adcValues = new short[4];
                foreach (var adc in GPIO.ADCs)
                {
                    adcValues[adc.Channel - 1] = adc.Read();
                }

                var result = BeginInvoke(new Action(() =>
                {
                    foreach (var input in GPIO.Inputs)
                    {
                        _lamps[input.Number].IsOn = input.IsOn;
                    }

                    foreach (var output in GPIO.Outputs)
                    {
                        output.IsOn = _buttons[output.Number].IsOn;
                    }

                    foreach (var adc in GPIO.ADCs)
                    {
                        _adcLabels[adc.Channel].Text = adcValues[adc.Channel - 1].ToString();
                    }

                    if (_dac1Init)
                    {
                        GPIO.DAC1.Init((ushort)_dac1Progress.Value);
                        _dac1Init = false;
                    }
                    else
                    {
                        GPIO.DAC1.Write((ushort)_dac1Progress.Value);
                    }

                    if (_dac2Init)
                    {
                        GPIO.DAC2.Init((ushort)_dac2Progress.Value);
                        _dac2Init = false;
                    }
                    else
                    {
                        GPIO.DAC2.Write((ushort)_dac2Progress.Value);
                    }
                }));

                while (!_stopThread && !result.IsCompleted)
                {
                    Thread.Sleep(1);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _lamps = new Dictionary<uint, Lamp>();
            _lamps.Add(GPIO.Input04.Number, lamp1);
            _lamps.Add(GPIO.Input05.Number, lamp2);
            _lamps.Add(GPIO.Input06.Number, lamp3);
            _lamps.Add(GPIO.Input07.Number, lamp4);
            _lamps.Add(GPIO.Input08.Number, lamp5);
            _lamps.Add(GPIO.Input09.Number, lamp6);
            _lamps.Add(GPIO.Input10.Number, lamp7);
            _lamps.Add(GPIO.Input11.Number, lamp8);
            _lamps.Add(GPIO.Input12.Number, lamp9);
            _lamps.Add(GPIO.Input13.Number, lamp10);
            _lamps.Add(GPIO.Input16.Number, lamp11);

            _buttons = new Dictionary<uint, Button>();
            _buttons.Add(GPIO.Output17.Number, button1);
            _buttons.Add(GPIO.Output18.Number, button2);
            _buttons.Add(GPIO.Output19.Number, button3);
            _buttons.Add(GPIO.Output20.Number, button4);
            _buttons.Add(GPIO.Output21.Number, button5);
            _buttons.Add(GPIO.Output22.Number, button6);
            _buttons.Add(GPIO.Output23.Number, button7);
            _buttons.Add(GPIO.Output24.Number, button8);
            _buttons.Add(GPIO.Output25.Number, button9);
            _buttons.Add(GPIO.Output26.Number, button10);
            _buttons.Add(GPIO.Output27.Number, button11);

            _adcLabels = new Dictionary<byte, Label>();
            _adcLabels.Add(GPIO.ADC1.Channel, adinLabel1);
            _adcLabels.Add(GPIO.ADC2.Channel, adinLabel2);
            _adcLabels.Add(GPIO.ADC3.Channel, adinLabel3);
            _adcLabels.Add(GPIO.ADC4.Channel, adinLabel4);

            _dac1Progress.Value = 0;
            _dac1Label.Text = _dac1Progress.Value.ToString();

            _dac1Progress.Value = 0;
            _dac1Label.Text = _dac1Progress.Value.ToString();

            _pollingThread = new Thread(PollGPIO);
            _pollingThread.IsBackground = true;
            _pollingThread.Start();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _stopThread = true;
            _pollingThread.Join();
        }

        private void _dac2Progress_MouseDown(object sender, MouseEventArgs e)
        {
            _dac2Progress.Value = e.X * 4095 / _dac2Progress.Width;
            _dac2Label.Text =  _dac2Progress.Value.ToString();
        }

        private void _dac1Progress_MouseDown(object sender, MouseEventArgs e)
        {
            _dac1Progress.Value = e.X * 4095 / _dac1Progress.Width;
            _dac1Label.Text = _dac1Progress.Value.ToString();
        }

        private void _dac1MinButton_Click(object sender, EventArgs e)
        {
            _dac1Progress.Value = 0;
            _dac1Label.Text = _dac1Progress.Value.ToString();
        }

        private void _dac1HalfButton_Click(object sender, EventArgs e)
        {
            _dac1Progress.Value = 4096 / 2;
            _dac1Label.Text = _dac1Progress.Value.ToString();
        }

        private void _dac1MaxButton_Click(object sender, EventArgs e)
        {
            _dac1Progress.Value = 4095;
            _dac1Label.Text = _dac1Progress.Value.ToString();
        }

        private void _dac2MinButton_Click(object sender, EventArgs e)
        {
            _dac2Progress.Value = 0;
            _dac2Label.Text = _dac2Progress.Value.ToString();
        }

        private void _dac2HalfButton_Click(object sender, EventArgs e)
        {
            _dac2Progress.Value = 4096 / 2;
            _dac2Label.Text = _dac2Progress.Value.ToString();
        }

        private void _dac2MaxButton_Click(object sender, EventArgs e)
        {
            _dac2Progress.Value = 4095;
            _dac2Label.Text = _dac2Progress.Value.ToString();
        }

        private void _dac1InitButton_Click(object sender, EventArgs e)
        {
            _dac1Progress.Value = 0;
            _dac1Init = true;
            _dac1Label.Text = _dac1Progress.Value.ToString();
        }

        private void _dac2InitButton_Click(object sender, EventArgs e)
        {
            _dac2Progress.Value = 0;
            _dac2Init = true;
            _dac2Label.Text = _dac2Progress.Value.ToString();
        }
    }
}
