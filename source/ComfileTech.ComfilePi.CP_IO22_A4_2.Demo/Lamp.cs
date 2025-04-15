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
    /// A lamp with two images: one for the on state and one for the off state.
    /// </summary>
    internal class Lamp : Control
    {
        /// <summary>
        /// Instantiates a new instance of this class.
        /// </summary>
        public Lamp()
        { }

        /// <summary>
        /// The image to display when the lamp is on.
        /// </summary>
        [Description("The image to display when the lamp is on.")]
        public Image OnImage
        {
            get; set;
        }

        /// <summary>
        /// The image to display when the lamp is off.
        /// </summary>
        [Description("The image to display when the lamp is off.")]
        public Image OffImage
        {
            get; set;
        }

        private bool _state;

        /// <summary>
        /// The current state of the lamp.  `true` means the lamp is on, `false` means the lamp is off.
        /// </summary>
        [Description("The current state of the lamp.  `true` means the lamp is on, `false` means the lamp is off.")]
        public bool State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    Invalidate();
                    Update();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (State)
            {
                if (OnImage != null)
                {
                    pe.Graphics.DrawImage(OnImage, ClientRectangle);
                }
            }
            else
            {
                if (OffImage != null)
                {
                    pe.Graphics.DrawImage(OffImage, ClientRectangle);
                }
            }

            using(var b = new SolidBrush(ForeColor))
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
