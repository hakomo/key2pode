using Hakomo.Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Key2pode {

    class Key2pode : LayeredForm {

        private const int MN = 8;

        private readonly Stack<Rectangle> boundss = new Stack<Rectangle>();

        public Key2pode() {
            Text = "key2pode";
            Rectangle r = Screen.PrimaryScreen.Bounds;
            using(Bitmap b = new Bitmap(r.Width, r.Height))
            using(Graphics g = Graphics.FromImage(b)) {
                g.Clear(Color.FromArgb(128, 191, 255, 128));
                SetLayeredBitmap(r.Location, b);
            }
        }

        private void Update(Rectangle r) {
            r.Size = new Size(Math.Max(2, r.Width), Math.Max(2, r.Height));
            using(Bitmap b = Create(r.Size))
                SetLayeredBitmap(r.Location, b);
            Mouse.Location = new Point((r.Left + r.Right) / 2, (r.Top + r.Bottom) / 2);
        }

        private static Bitmap Create(Size s) {
            int i;
            Bitmap bmp = new Bitmap(s.Width, s.Height);
            using(Graphics g = Graphics.FromImage(bmp))
            using(SolidBrush sb = new SolidBrush(Color.FromArgb(128, 191, 255, 128))) {
                for(i = 0; i < 2; ++i) {
                    g.FillRectangle(sb, 0, (bmp.Height / 2 + 1) * i, bmp.Width, (bmp.Height - i) / 2);
                    g.FillRectangle(sb, (bmp.Width / 2 + 1) * i, bmp.Height / 2, (bmp.Width - i) / 2, 1);
                }
            }
            return bmp;
        }

        private void Operate(Action a) {
            Neutralize();
            ++Left;
            a();
            Close();
        }

        private void Neutralize() {
            using(Bitmap b = new Bitmap(2, 2))
                SetLayeredBitmap(Mouse.Location, b);
            Mouse.Up();
        }

        protected override bool ProcessDialogKey(Keys k) {
            if(k == Keys.J) {
                boundss.Push(Bounds);
                if(Width == 2) {
                    Update(new Rectangle(Left - MN, Top, Width, Height));
                } else {
                    Update(new Rectangle(Left, Top, Width / 2, Height));
                }
            } else if(k == Keys.K) {
                boundss.Push(Bounds);
                if(Height == 2) {
                    Update(new Rectangle(Left, Top + MN, Width, Height));
                } else {
                    Update(new Rectangle(Left, (Top + Bottom) / 2, Width, (Height + 1) / 2));
                }
            } else if(k == Keys.L) {
                boundss.Push(Bounds);
                if(Height == 2) {
                    Update(new Rectangle(Left, Top - MN, Width, Height));
                } else {
                    Update(new Rectangle(Left, Top, Width, Height / 2));
                }
            } else if(k == Keys.OemSemicolon) {
                boundss.Push(Bounds);
                if(Width == 2) {
                    Update(new Rectangle(Left + MN, Top, Width, Height));
                } else {
                    Update(new Rectangle((Left + Right) / 2, Top, (Width + 1) / 2, Height));
                }
            } else if(k == Keys.H && boundss.Count > 0) {
                Update(boundss.Pop());
            } else if(k == Keys.C) {
                Operate(Mouse.Click);
            } else if(k == Keys.R) {
                Operate(Mouse.RightClick);
            } else if(k == Keys.M) {
                Operate(Mouse.MiddleClick);
            } else if(k == Keys.B) {
                Operate(Mouse.DoubleClick);
            } else if(k == Keys.D) {
                if(Mouse.IsDown) {
                    Mouse.Up();
                    Close();
                } else {
                    Mouse.Down();
                    boundss.Clear();
                    Update(Screen.PrimaryScreen.Bounds);
                    WinAPI.Activate(Handle);
                }
            } else if(k == Keys.G) {
                Neutralize();
                Close();
            }
            return base.ProcessDialogKey(k);
        }

        protected override void OnFormClosed(FormClosedEventArgs e) {
            base.OnFormClosed(e);
            Mouse.Up();
        }

        [STAThread]
        private static void Main() {
            Util.Run<Key2pode>("63F880FB-11F6-40B0-9BD7-9C37A87B72DC");
        }
    }
}
