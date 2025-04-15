using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Runtime.InteropServices;
using System.Text;

namespace ComfileTech.ComfilePi.CP_IO22_A4_2
{
    /// <summary>
    /// A base class for digital inputs and outputs
    /// </summary>
    public abstract class DigitalInputOutput<T>
        where T : DigitalInputOutput<T>
    {
        static readonly GpioController _gpio;

        static DigitalInputOutput()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _gpio = new GpioController();

                // Without this application will take a long tme to close
                AppDomain.CurrentDomain.ProcessExit += (s, e) =>
                {
                    _gpio.Dispose();
                };
            }
        }

        private protected DigitalInputOutput(int number, PinMode mode)
        {
            Number = number;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _pin = _gpio.OpenPin(number, mode);

                // ValueChanged events only work for inputs
                if (mode != PinMode.Output)
                {
                    _pin.ValueChanged += (sender, args) =>
                    {
                        StateChanged?.Invoke((T)this);
                    };
                }
            }
        }

        private readonly GpioPin _pin;

        /// <summary>
        /// The GPIO pin number associated with this digital output.
        /// </summary>
        public int Number { get; }

        bool _state;
        /// <summary>
        /// Gets the current state of the digital output.  `true` means the pin is high, `false` means the pin is low.
        /// </summary>
        public bool State
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    _state = _pin.Read() == PinValue.High;
                }

                return _state;
            }
            private protected set
            {
                if (_state != value)
                {
                    _state = value;

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        _pin.Write(_state ? PinValue.High : PinValue.Low);

                        // Since ValueChanged events only work for inputs, we need to fire the event manually for outputs
                        if (_pin.GetPinMode() != PinMode.Output)
                        {
                            StateChanged?.Invoke((T)this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fires when the state of an input changes;
        /// </summary>
        public event Action<T> StateChanged;
    }
}
