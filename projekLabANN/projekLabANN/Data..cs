using Accord.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekLabANN
{
    class Data
    {
        private static Data instance;

        public List<Bitmap> images;
        public List<String> classes;
        public List<String> filenames;
        public List<int> classindex;

        public Data()
        {
            images = new List<Bitmap>();
            classes = new List<string>();
            filenames = new List<string>();
            classindex = new List<int>();
        }

        public static Data getInstance()
        {
            if (instance == null)
            {
                instance = new Data();
            }
            return instance;
        }

        public Bitmap preprocessing(Bitmap image)
        {
            image = image.Clone(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            image = Grayscale.CommonAlgorithms.BT709.Apply(image);
            image = new Threshold(127).Apply(image);
            image = new CannyEdgeDetector().Apply(image);

            int x = image.Width;
            int y = image.Height;
            int width = 0;
            int height = 0;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    if (image.GetPixel(j, i).R > 127)
                    {
                        if (x > j) x = j;
                        if (y > i) y = i;
                        if (width < j) width = j;
                        if (height < i) height = i;
                    }
                }
            }
            image = image.Clone(new Rectangle(x, y, width - x, height - y), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            image = new ResizeBilinear(10 , 10).Apply(image);

            return image;
        }

    }
}
