using Hakomo.Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Key2pode {

    class Key2pode : LayeredForm {

        private readonly Stack<Rectangle> boundss = new Stack<Rectangle>();

        public Key2pode() {
            AllowDrop = true;
            DragEnter += (s, e) => e.Effect = DragDropEffects.All;
            Text = "key2pode";
            Update(Screen.PrimaryScreen.Bounds);
        }

        private void Update(Rectangle r) {
            Mouse.Location = new Point((r.Left + r.Right) / 2, (r.Top + r.Bottom) / 2);
            Location = r.Location;
            using(Bitmap b = Create(r.Size))
                LayeredBitmap = b;
        }

        private Bitmap Create(Size s) {
            Bitmap bmp = new Bitmap(s.Width, s.Height);
            using(Graphics g = Graphics.FromImage(bmp))
                g.Clear(Color.FromArgb(128, 191, 255, 128));
            return bmp;
        }

        private void OperateMouse(Action a, bool isRepeat = false) {
            using(Bitmap b = new Bitmap(1, 1))
                LayeredBitmap = b;
            a();
            if(isRepeat) {
                boundss.Clear();
                Update(Screen.PrimaryScreen.Bounds);
                WinAPI.Activate(Handle);
            } else {
                Application.Exit();
            }
        }

        protected override bool ProcessDialogKey(Keys k) {
            if(k == Keys.J) {
                boundss.Push(Bounds);
                if(Width == 2) {
                    Update(new Rectangle(Left - 4, Top, Width, Height));
                } else {
                    Update(new Rectangle(Left, Top, Width / 2, Height));
                }
            } else if(k == Keys.K) {
                boundss.Push(Bounds);
                if(Height == 2) {
                    Update(new Rectangle(Left, Top + 4, Width, Height));
                } else {
                    Update(new Rectangle(Left, (Top + Bottom) / 2, Width, (Height + 1) / 2));
                }
            } else if(k == Keys.L) {
                boundss.Push(Bounds);
                if(Height == 2) {
                    Update(new Rectangle(Left, Top - 4, Width, Height));
                } else {
                    Update(new Rectangle(Left, Top, Width, Height / 2));
                }
            } else if(k == Keys.OemSemicolon) {
                boundss.Push(Bounds);
                if(Width == 2) {
                    Update(new Rectangle(Left + 4, Top, Width, Height));
                } else {
                    Update(new Rectangle((Left + Right) / 2, Top, (Width + 1) / 2, Height));
                }
            } else if(k == Keys.H && boundss.Count > 0) {
                Update(boundss.Pop());
            } else if(k == Keys.C) {
                Mouse.Up();
                OperateMouse(Mouse.Click);
            } else if(k == Keys.R) {
                Mouse.Up();
                OperateMouse(Mouse.RightClick);
            } else if(k == Keys.M) {
                Mouse.Up();
                OperateMouse(Mouse.MiddleClick);
            } else if(k == Keys.B) {
                Mouse.Up();
                OperateMouse(Mouse.DoubleClick);
            } else if(k == Keys.D) {
                if(Mouse.IsDown) {
                    OperateMouse(Mouse.Up);
                } else {
                    OperateMouse(Mouse.Down, true);
                }
            } else if(k == Keys.G) {
                Mouse.Up();
                Application.Exit();
            }
            return base.ProcessDialogKey(k);
        }

        [STAThread]
        private static void Main() {
            Util.Run<Key2pode>("63F880FB-11F6-40B0-9BD7-9C37A87B72DC");
        }
    }
}
