using System.Collections.Generic;
using FuiEditor.Utils;

namespace FuiEditor
{
    class FuiImageInfo
    {
        public static readonly int NativeSize = 32;

        private int descriptor;
        private int attribute;
        private int imageWidth;
        private int imageHeight;
        private int imageOffset;
        private int imageSize;
        private int unkOffset;
        private int unk_0x1C;

        public FuiImageInfo()
        {

        }

        public void Read(byte[] filedata, int startIndex)
        {
            descriptor = FuiUtils.ToInt32(filedata, startIndex + 0);
            attribute = FuiUtils.ToInt32(filedata, startIndex + 4);
            imageWidth = FuiUtils.ToInt32(filedata, startIndex + 8);
            imageHeight = FuiUtils.ToInt32(filedata, startIndex + 12);
            imageOffset = FuiUtils.ToInt32(filedata, startIndex + 16);
            imageSize = FuiUtils.ToInt32(filedata, startIndex + 20);
            unkOffset = FuiUtils.ToInt32(filedata, startIndex + 24);
            unk_0x1C = FuiUtils.ToInt32(filedata, startIndex + 28);
        }

        public byte[] ToByteArray()
        {
            List<byte> byteList = new List<byte>();

            byteList.AddRange(FuiUtils.GetBytes(descriptor));
            byteList.AddRange(FuiUtils.GetBytes(attribute));
            byteList.AddRange(FuiUtils.GetBytes(imageWidth));
            byteList.AddRange(FuiUtils.GetBytes(imageHeight));
            byteList.AddRange(FuiUtils.GetBytes(imageOffset));
            byteList.AddRange(FuiUtils.GetBytes(ImageSize));
            byteList.AddRange(FuiUtils.GetBytes(unkOffset));
            byteList.AddRange(FuiUtils.GetBytes(unk_0x1C));

            return byteList.ToArray();
        }

        public int ImageOffset
        {
            get => imageOffset;
            set => imageOffset = value;
        }

        public int ImageSize
        {
            get => imageSize;
            set => imageSize = value;
        }

        public int Width
        {
            get => imageWidth;
            set => imageWidth = value;
        }

        public int Height
        {
            get => imageHeight;
            set => imageHeight = value;
        }

        public int Descriptor
        {
            get => descriptor;
            set => descriptor = value;
        }

        public int Attribute
        {
            get => attribute;
            set => attribute = value;
        }
    }
}
