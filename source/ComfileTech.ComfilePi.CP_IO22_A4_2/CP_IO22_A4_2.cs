using System.Collections.Generic;

namespace ComfileTech.ComfilePi.CP_IO22_A4_2
{
    /// <summary>
    /// Represents the CP-IO22-A4-2 IO board connected to the ComfilePi.
    /// </summary>
    public class CP_IO22_A4_2 : System.IDisposable
    {
        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        public static CP_IO22_A4_2 Instance { get; } = new CP_IO22_A4_2();

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
        public IReadOnlyList<DigitalInput> DigitalInputs { get; }

        /// <summary>
        /// The digital outputs for the IO board.
        /// </summary>
        public IReadOnlyList<DigitalOutput> DigitalOutputs { get; }

        /// <summary>
        /// The analog inputs for the IO board.
        /// </summary>
        public IReadOnlyList<AnalogInput> AnalogInputs => AnalogInput.AnalogInputs;

        /// <summary>
        /// The analog outputs for the IO board.
        /// </summary>
        public IReadOnlyList<AnalogOutput> AnalogOutputs { get; }

        bool _disposed;

        /// <summary>
        /// Releases the GPIO and I2C resources used by the IO board.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            DigitalInput.DisposeGpioController();
            DigitalOutput.DisposeGpioController();
            AnalogInput.DisposeAds1115Device();

            foreach (var analogOutput in AnalogOutputs)
            {
                analogOutput.Dispose();
            }

            _disposed = true;
            System.GC.SuppressFinalize(this);
        }
    }
}
