using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 原神_启动_
{
    internal static class ScreenUtils
    {
        public static int ScreensCount { get => Screen.AllScreens.Length; }
        public static Size GetScreenSize(int screen) => Screen.AllScreens[screen].Bounds.Size;

        public static void GetScreenShot(int screen, out Image image)
        {
            image = new Bitmap(GetScreenSize(screen).Width, GetScreenSize(screen).Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.CopyFromScreen(new Point(0, 0), new Point(0, 0), GetScreenSize(screen));
            graphics.Dispose();
        }
    }
}
