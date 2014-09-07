using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NewBeanfunStarter
{
    public class ImageMarquee
    {
        public uint index = 0;
        private Image[] imgs;
        public ImageMarquee(Image dark, Image light, Color bgcolor, int width, int height, int overlap = 0)
        {
            // n * img.width - (n - 1) * overlap < width
            int n = (width - overlap) / (light.Width - overlap),
                total_width = n * light.Width - (n - 1) * overlap,
                x0 = (width - total_width) / 2 + overlap,
                y0 = (height - light.Height) / 2;
            int[,] state = new int[n + 3, n];
            this.imgs = new Image[n + 3];
            for (int i = 1; i < n + 3; i++)
                for (int j = i - 3; j < i; j++)
                    if (j >= 0 && j < n)
                        state[i, j] = 1;
            for (int i = 0; i < n + 3; i++)
            {
                Image img = (Image)new Bitmap(width, height);
                Graphics g = Graphics.FromImage(img);
                g.Clear(bgcolor);
                for (int j = 0; j < n; j++)
                {
                    g.DrawImage((state[i, j] == 0) ? dark : light, x0 + (dark.Width - overlap) * j, y0, dark.Width, dark.Height);
                }
                this.imgs[i] = img;
            }
        }

        public Image Next()
        {
            if (this.index >= this.imgs.Length)
                this.index = 0;
            return this.imgs[this.index++];
        }
    }
}
