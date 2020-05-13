using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MCore.Comp.SMLib.SMFlowChart
{
    public class CustomCursor
    {

        [DllImport("user32.dll")]
        private static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        private static Bitmap testSizeBMP = new Bitmap(1000, 1000);
        /// <summary>
        /// Create a text based cursor
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Cursor CreateText(string text)
        {
            Graphics g = Graphics.FromImage(testSizeBMP);
            Font f = new Font(FontFamily.GenericMonospace, 10);
            SizeF size = g.MeasureString(text, f);


            Bitmap bitmap = new Bitmap((int)(size.Width + 100.0F), (int)(size.Height + 100.0F));
            g = Graphics.FromImage(bitmap);
            g.DrawString(text, f, Brushes.Blue, 0, 0);

            Cursor cursor = Create(bitmap, 3, 3);

            bitmap.Dispose();
            return cursor;
        }

        /// <summary>
        /// Create a bmp-based cursor with hotspot in the middle
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Cursor Create(Bitmap bmp)
        {
            int xHotSpot = bmp.Size.Width / 2;
            int yHotSpot = bmp.Size.Height / 2;
            return Create(bmp, xHotSpot, yHotSpot);
        }
        /// <summary>
        /// Create a cursor with a specified hot spot
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="xHotSpot"></param>
        /// <param name="yHotSpot"></param>
        /// <returns></returns>
        public static Cursor Create(Bitmap bmp, int xHotSpot, int yHotSpot)
        {
            IconInfo tmp = new IconInfo();
            GetIconInfo(bmp.GetHicon(), ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            return new Cursor(CreateIconIndirect(ref tmp));
        }
    }
    struct IconInfo
    {
        public bool fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }
}