using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ColorSwap
{
    class Program
    {
        static void Main(string[] args)
        {
            string imagePath;
            int photosNeeded;
            List<string> originalColors;

            Console.WriteLine("***********************************");
            Console.WriteLine("************ ColorSwap ************");
            Console.WriteLine("***********************************");
            Console.WriteLine("Please drag the photo into the cmd and press [ENTER] ..");
            imagePath = Console.ReadLine();
            Console.WriteLine("Please enter the colors to change (e.g. #00000;#000001) and press [ENTER] ..");
            originalColors = Console.ReadLine().Split(";").ToList();
            Console.WriteLine("Please enter the number of photos to be created and press [ENTER] ..");
            photosNeeded = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("Creating a list of random colors ...");
            List<string> colors = CreateColorArray(photosNeeded * originalColors.Count);
            Console.WriteLine("Creating OUTPUT folder ..");
            
            System.IO.Directory.CreateDirectory(@".\output");

            Bitmap bmp = new Bitmap(Image.FromFile(imagePath));
            for (int i = 1; i <= photosNeeded; i++)
            {
                Bitmap newPhoto = bmp;
                foreach (string oldColor in originalColors)
                {
                    newPhoto = ChangeColor(newPhoto, oldColor, colors[0]);
                    if (colors.Count >= 1)
                        colors.RemoveAt(0);
                }
                Console.WriteLine($"Saving photo number { i } ..");
                bmp.Save(@$".\output\img{ i }.png", System.Drawing.Imaging.ImageFormat.Png);
            }

            Console.WriteLine("*******************************");
            Console.WriteLine("All photos created");
            Console.WriteLine("Press any key to exit ..");
            Console.ReadLine();
        }

        public static List<string> CreateColorArray(int colorCount)
        {
            List<string> colors = new List<string>();
            do
            {
                Random r = new Random();
                string newColor = r.Next(0, 1000000).ToString("000000");
                if (!colors.Contains(newColor))
                    colors.Add(newColor);
            } while (colors.Count < colorCount);
            return colors;
        }

        public static Bitmap ChangeColor(Bitmap img, string oldCode, string newCode)
        {
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    Color currentColor = img.GetPixel(x, y);
                    if (ColorTranslator.ToHtml(currentColor).Equals(oldCode))
                        currentColor = ColorTranslator.FromHtml(newCode);
                    img.SetPixel(x, y, currentColor);
                }
            }
            return img;
        }
    }
}
