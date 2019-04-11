using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace FlashCards.Custom
{
    public class ImagesConverter
    {
        public static string ImageToStringBase64(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return Convert.ToBase64String(ms.ToArray());

            }
        }

        public static MemoryStream StringBase64ToImage(string data)
        {
            byte[] stream = Convert.FromBase64String(data);
            return new MemoryStream(stream);
        }

    }
}
