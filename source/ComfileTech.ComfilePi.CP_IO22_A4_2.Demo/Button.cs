using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComfileTech.ComfilePi.CP_IO22_A4_2.Demo
{
    /// <summary>
    /// A button with two images: one for the pressed state and one for the released state.
    /// </summary>
    internal class Button : Control
    {
        /// <summary>
        /// Instantiates a new instance of this class.
        /// </summary>
        public Button()
        { }

        /// <summary>
        /// The image to display when the button is pressed.
        /// </summary>
        [Description("The image to display when the button is pressed.")]
        public Image PressedImage
        {
            get; set;
        }

        /// <summary>
        /// The image to display when the button is released.
        /// </summary>
        [Description("The image to display when the button is released.")]
        public Image ReleasedImage
        {
            get; set;
        }

        private bool _state;
        /// <summary>
        /// The current state of the button.  `true` means the button is pressed, `false` means the button is released.
        /// </summary>
        [Description("The current state of the button.  `true` means the button is pressed, `false` means the button is released.")]
        public bool State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;

                    StateChanged?.Invoke(this, EventArgs.Empty);

                    Invalidate();
                    Update(); 
                }
            }
        }

        /// <summary>
        /// Fires when the state of the button changes.
        /// </summary>
        public event EventHandler StateChanged;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            State = !State;

            base.OnMouseDown(e);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {

            if (State)
            {
                if (PressedImage != null)
                {
                    pe.Graphics.DrawImage(PressedImage, ClientRectangle);
                }
            }
            else
            {
                if (ReleasedImage != null)
                {
                    pe.Graphics.DrawImage(ReleasedImage, ClientRectangle);
                }
            }

            using (var b = new SolidBrush(ForeColor))
            {
                using (var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                })
                {
                    pe.Graphics.DrawString(Text, Font, b, ClientRectangle, sf);
                }
            }

            base.OnPaint(pe);
        }
    }
}
