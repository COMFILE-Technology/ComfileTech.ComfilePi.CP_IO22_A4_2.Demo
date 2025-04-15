using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comfile.ComfilePi.CP_IO22_A4_2_Test
{
    public partial class Button : Control
    {
        public Button()
        {
            InitializeComponent();

            this.SetStyle(
             ControlStyles.AllPaintingInWmPaint |
             ControlStyles.UserPaint |
             ControlStyles.DoubleBuffer,
             true);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            _pressImage = _pressImage.Resize(Size);
            _releaseImage = _releaseImage.Resize(Size);
        }

        private Image _pressImage;
        public Image PressImage
        {
            get { return _pressImage; }
            set
            {
                if (_pressImage != value)
                {
                    _pressImage = value;
                    _pressImage = _pressImage.Resize(Size);
                    Invalidate();
                }
            }
        }

        private Image _releaseImage;
        public Image ReleaseImage
        {
            get { return _releaseImage; }
            set
            {
                if (_releaseImage != value)
                {
                    _releaseImage = value;
                    _releaseImage = _releaseImage.Resize(Size);
                    Invalidate();
                }
            }
        }

        private bool _isOn;
        public bool IsOn
        {
            get { return _isOn; }
            set
            {
                if (_isOn != value)
                {
                    _isOn = value;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _isOn = !_isOn;

            base.OnMouseDown(e);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            
            if (_isOn)
            {
                if (_pressImage != null)
                {
                    pe.Graphics.DrawImage(_pressImage, ClientRectangle);
                }
            }
            else
            {
                if (_releaseImage != null)
                {
                    pe.Graphics.DrawImage(_releaseImage, ClientRectangle);
                }
            }

            base.OnPaint(pe);
        }
    }
}
