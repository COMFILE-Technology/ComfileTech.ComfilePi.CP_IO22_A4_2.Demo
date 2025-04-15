using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ComfileTech.ComfilePi.CP_IO22_A4_2.Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var model = File.ReadAllText("/proc/device-tree/model").Trim();
                if (!model.Contains("Compute Module 4S") && !model.Contains("Compute Module 3"))
                {
                    MessageBox.Show("This application should only be run on a CPi-A, CPi-B, or CPi-S series panel PC.", Text);
                    Environment.Exit(0);
                }

                // Workaround for fullscreen on Raspberry Pi OS Bookworm with Wayland and LabWC
                LocationChanged += (s, e) => { Location = new Point(0, 0); };
            }

            // On Linux, bind the digital input lamps to the IO board's digital inputs
            var lamps = _digitalInputPanel.Controls.OfType<Lamp>().ToArray();

            int index = 0;
            foreach (var input in CP_IO22_A4_2.Instance.DigitalInputs)
            {
                int i = index;

                var lamp = lamps[i];
                lamp.State = input.State;
                lamp.Text = input.Number.ToString();

                input.StateChanged += (di) =>
                {
                    lamp.State = di.State;
                };

                index++;
            }

            // On Linux, bind the digital output buttons to the IO board's digital outputs
            var buttons = _digitalOutputPanel.Controls.OfType<Button>().ToArray();

            index = 0;
            foreach (var output in CP_IO22_A4_2.Instance.DigitalOutputs)
            {
                int i = index;

                var button = buttons[i];
                button.State = output.State;
                button.Text = output.Number.ToString();

                button.StateChanged += (s, e) =>
                {
                    output.State = button.State;
                };

                index++;
            }

            // On Linux, bind the analog input labels to the IO board's analog inputs
            var labels = _analogInputPanel.Controls.OfType<Label>().ToArray();

            index = 0;
            foreach (var input in CP_IO22_A4_2.Instance.AnalogInputs)
            {
                int i = index;

                var label = labels[i];
                label.Text = $"{input.Voltage:0.000}V";

                input.VoltageChanged += (ai) =>
                {
                    label.Text = $"{input.Voltage:0.000}V";
                    label.Update();
                };

                index++;
            }

            // On Linux, bind the digital output buttons to the IO board's digital outputs
            var trackBars = _analogOutputPanel.Controls.OfType<TrackBar>().ToArray();
            labels = _analogOutputPanel.Controls.OfType<Label>().ToArray();

            index = 0;
            foreach (var output in CP_IO22_A4_2.Instance.AnalogOutputs)
            {
                int i = index;

                var trackBar = trackBars[i];
                var label = labels[i];
                trackBar.Value = (int)Math.Round(output.Voltage * 1000);
                label.Text = $"{output.Voltage:0.000}V";
                trackBar.Text = output.Channel.ToString();

                trackBar.ValueChanged += (s, e) =>
                {
                    output.Voltage = trackBar.Value / 1000.0;
                    label.Text = $"{output.Voltage:0.000}V";
                };

                index++;
            }
        }

        private void _repositoryUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var linkLabel = sender as LinkLabel;
            System.Diagnostics.Process.Start(linkLabel.Text);
        }

        private void _closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
