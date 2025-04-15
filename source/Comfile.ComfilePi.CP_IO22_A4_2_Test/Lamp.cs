using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Comfile.ComfilePi.CP_IO22_A4_2_Test
{
    public partial class Lamp : Control
    {
        public Lamp()
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
            _onImage = _onImage.Resize(Size);
            _offImage = _offImage.Resize(Size);
        }

        private Image _onImage;
        public Image OnImage
        {
            get { return _onImage; }
            set
            {
                if (_onImage != value)
                {
                    _onImage = value;
                    _onImage = _onImage.Resize(Size);
                    Invalidate();
                }
            }
        }

        private Image _offImage;
        public Image OffImage
        {
            get { return _offImage; }
            set
            {
                if (_offImage != value)
                {
                    _offImage = value;
                    _offImage = _offImage.Resize(Size);
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

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (IsOn)
            {
                if (_onImage != null)
                {
                    pe.Graphics.DrawImage(_onImage, ClientRectangle);
                }
            }
            else
            {
                if (_offImage != null)
                {
                    pe.Graphics.DrawImage(_offImage, ClientRectangle);
                }
            }

            base.OnPaint(pe);
        }
    }
}
