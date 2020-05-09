using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FuiEditor.Utils
{
    static class FuiUtils
    {
        public static FuiImageInfo[] GetImageInfo(byte[] filedata, int imageIndex)
        {
            Stack<FuiImageInfo> imageInfoStack = new Stack<FuiImageInfo>();
            int currentOffset = imageIndex - FuiImageInfo.NativeSize;
            while(true)
            {
                FuiImageInfo imageInfo = new FuiImageInfo();
                imageInfo.Read(filedata, currentOffset);
                imageInfoStack.Push(imageInfo);
                if(imageInfo.ImageOffset == 0)
                {
                    break;
                }
                currentOffset -= FuiImageInfo.NativeSize;
            }

            return imageInfoStack.ToArray();
        }

        public static List<byte[]> GetImagesData(byte[] filedata, int imageIndex, FuiImageInfo[] imageInfoNeeded)
        {
            List<byte[]> imageList = new List<byte[]>(imageInfoNeeded.Length);
            int bytesRead = 0;
            foreach(FuiImageInfo imageInfo in imageInfoNeeded)
            {
                /*using (MemoryStream stream = new MemoryStream(filedata, imageIndex + imageInfo.ImageOffset, imageInfo.ImageSize))
                {
                    //for error when save image
                    Image image = Image.FromStream(stream);
                    imageList.Add(new Bitmap(image));
                }*/

                imageList.Add(filedata.Skip(imageIndex + imageInfo.ImageOffset).Take(imageInfo.ImageSize).ToArray());

                bytesRead += imageInfo.ImageSize;
            }
            if(filedata.Length != (imageIndex + bytesRead))
            {
                MessageBox.Show("Oops, maybe not supported.", "Warning");
            }
            return imageList;
        }

        public static byte[] ProcessHeader(byte[] filedata)
        {
            byte[] processed = (byte[])filedata.Clone();
            byte[] sizeBytes = GetBytes(filedata.Length - 152);
            Array.Copy(sizeBytes, 0, processed, 8, sizeBytes.Length);
            return processed;
        }

        public static int ToInt32(byte[] filedata, int startIndex)
        {
            byte[] data = filedata.Skip(startIndex).Take(4).ToArray();
            if(BitConverter.IsLittleEndian)
            {
                return BitConverter.ToInt32(data, 0);
            }
            Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }

        public static byte[] GetBytes(int value)
        {
            byte[] data = BitConverter.GetBytes(value);
            if(BitConverter.IsLittleEndian)
            {
                return data;
            }
            Array.Reverse(data);
            return data;
        }
    }
}
