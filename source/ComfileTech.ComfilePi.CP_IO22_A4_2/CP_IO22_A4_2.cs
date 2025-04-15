using Iot.Device.Ads1115;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.I2c;
using System.Runtime.InteropServices;
using System.Threading;

namespace ComfileTech.ComfilePi.CP_IO22_A4_2
{
    /// <summary>
    /// Represents the CP-IO22-A4-2 IO board connected to the ComfilePi.
    /// </summary>
    public class CP_IO22_A4_2
    {
        static CP_IO22_A4_2()
        {
            Instance = new CP_IO22_A4_2();
        }

        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        public static CP_IO22_A4_2 Instance
        {
            get; private set;
        }

        private CP_IO22_A4_2()
        {
            // Create the digital inputs
            {
                var list = new List<DigitalInput>();
                for (int i = 4; i <= 13; i++)
                {
                    list.Add(new DigitalInput(i));
                }
                list.Add(new DigitalInput(16));
                DigitalInputs = list.AsReadOnly();
            }

            // Create the digital outputs
            {
                var list = new List<DigitalOutput>();
                for (int i = 17; i <= 27; i++)
                {
                    list.Add(new DigitalOutput(i));
                }
                DigitalOutputs = list.AsReadOnly();
            }

            // Create the analog outputs
            {
                var list = new List<AnalogOutput>();
                for (int i = 1; i <= 2; i++)
                {
                    list.Add(new AnalogOutput(i));
                }
                AnalogOutputs = list.AsReadOnly();
            }

        }

        /// <summary>
        /// The digital inputs for the IO board.
        /// </summary>
        public IReadOnlyList<DigitalInput> DigitalInputs
        {
            get;
        }

        /// <summary>
        /// The digital outputs for the IO board.
        /// </summary>
        public IReadOnlyList<DigitalOutput> DigitalOutputs
        {
            get;
        }

        /// <summary>
        /// The analog inputs for the IO board.
        /// </summary>
        public IReadOnlyList<AnalogInput> AnalogInputs
        {
            get => AnalogInput.AnalogInputs;
        }

        /// <summary>
        /// The analog outputs for the IO board.
        /// </summary>
        public IReadOnlyList<AnalogOutput> AnalogOutputs
        {
            get;
        }
    }
}
