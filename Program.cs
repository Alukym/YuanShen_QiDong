using System;
using System.IO;
using System.Media;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace 原神_启动_
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string game_path;
            try 
            { 
                game_path = File.ReadAllText("config.txt");
                if (string.IsNullOrEmpty(game_path))
                    throw new Exception();
                if (!File.Exists(game_path))
                    throw new Exception();
            } 
            catch (Exception)
            {
                var ofd = new OpenFileDialog
                {
                    Title = "Select game path",
                    Filter = "Game executable|*.exe"
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    game_path = ofd.FileName;
                    File.WriteAllText("config.txt", game_path);
                }
                else return;
            }

            while (true)
            {
                Console.WriteLine($"Info: Screens count: {ScreenUtils.ScreensCount}");
                for (int i = 0; i < ScreenUtils.ScreensCount; i++)
                {
                    ScreenUtils.GetScreenShot(i, out var screenshot);
                    if (screenshot == null)
                    {
                        Console.WriteLine($"Warn: Failed to get the ScreenShot in screen {i}");
                        continue;
                    }

                    var bmp = new Bitmap(screenshot);
                    screenshot.Dispose();
                    if (bmp == null)
                    {
                        Console.WriteLine("Warn: Failed to convert a Image object into a Bitmap object");
                        continue;
                    }

                    int whiteBitsCount = 0;

                    for (int x = 0; x < bmp.Width; x++)
                    {
                        for (int y = 0; y < bmp.Height; y++)
                        {
                            var color = bmp.GetPixel(x, y);
                            bool check(int val) => val >= 230 && val <= 255;
                            if (check(color.R) && check(color.G) && check(color.B))
                                whiteBitsCount++;
                        }
                    }

                    // Console.WriteLine($"{whiteBitsCount} {ScreenUtils.GetScreenSize(i).Width * ScreenUtils.GetScreenSize(i).Height * 0.9}");

                    if (whiteBitsCount >= ScreenUtils.GetScreenSize(i).Width * ScreenUtils.GetScreenSize(i).Height * 0.9)
                    {
                        var soundPlayer = new SoundPlayer(Properties.Resources.music);
                        soundPlayer.Play();

                        Process.Start(game_path);

                        MessageBox.Show("原神，启动！");
                    }
                }

                // Code above are slow enough
                // Thread.Sleep(100);
            }
        }
    }
}
