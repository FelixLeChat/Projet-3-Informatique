using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HttpHelper.Image
{
    public class ScreenShotHelper
    {
        /// <summary>
        /// Capture all the screen
        /// </summary>
        /// <param name="name"></param>
        public static void CaptureAllScreen(string name = "default")
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                           Screen.PrimaryScreen.Bounds.Height,
                                           PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            var defaultPath = Path.GetTempPath();
            bmpScreenshot.Save(defaultPath + name +".png", ImageFormat.Png);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public static System.Drawing.Image CaptureDesktop()
        {
            return CaptureWindow(GetDesktopWindow());
        }

        public static Bitmap CaptureActiveWindow()
        {
            return CaptureWindow(GetForegroundWindow());
        }

        public static void SaveActiveWindow(string name = "default")
        {
            var image = CaptureActiveWindow();
            var defaultPath = Path.GetTempPath();

            image.Save(defaultPath+name+".png", ImageFormat.Png);
        }

        public static void SaveActiveMapHack(string path)
        {
            var image = CaptureMapHack(GetForegroundWindow());
            image.Save(path, ImageFormat.Png);
        }

        public static Bitmap CaptureWindow(IntPtr handle)
        {
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return result;
        }

        public static Bitmap CaptureMapHack(IntPtr handle)
        {
            var modifLeft = 0;
            var modifRight = 0;
            var modifTop = 30;
            var modifBottom = 0;

            var width = 300;
            var height = 533;

            var rect = new Rect();
            GetWindowRect(handle, ref rect);


            var realWidth = rect.Right - rect.Left - modifLeft - modifRight;
            var realHeight = rect.Bottom - rect.Top - modifTop - modifBottom;

            var horizontalOffset = realWidth/2 + rect.Left + modifLeft - width/2;
            var verticalOffset = realHeight/2 + rect.Top + modifTop - height/2;
            
            var bounds = new Rectangle(horizontalOffset, verticalOffset, width, height);
            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return result;
        }
    }
}
