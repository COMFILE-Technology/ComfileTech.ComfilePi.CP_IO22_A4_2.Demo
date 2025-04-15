using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Runtime.InteropServices;
using System.Text;

namespace ComfileTech.ComfilePi.CP_IO22_A4_2
{
    /// <summary>
    /// A digital output pin on the IO board.
    /// </summary>
    public class DigitalOutput : DigitalInputOutput<DigitalOutput>
    {
        internal DigitalOutput(int number)
            : base(number, PinMode.Output)
        { }

        /// <summary>
        /// Gets or sets the current state of the digital output.  `true` means the pin is high, `false` means the pin is low.
        /// </summary>
        public new bool State
        {
            get => base.State;
            set => base.State = value;
        }
    }
}
