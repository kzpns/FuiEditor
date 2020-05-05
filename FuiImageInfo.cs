using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuiEditor
{
    class FuiImageInfo
    {
        public static readonly int NativeSize = 32;

        private int unk_0x0;
        private int unk_0x4;
        private int unk_0x8;
        private int unk_0xC;
        private int imageOffset;
        private int imageSize;
        private int unk_0x18;
        private int unk_0x1C;

        public FuiImageInfo()
        {

        }

        public void Read(byte[] filedata, int startIndex)
        {
            unk_0x0 = FuiUtils.ToInt32(filedata, startIndex + 0);
            unk_0x4 = FuiUtils.ToInt32(filedata, startIndex + 4);
            unk_0x8 = FuiUtils.ToInt32(filedata, startIndex + 8);
            unk_0xC = FuiUtils.ToInt32(filedata, startIndex + 12);
            imageOffset = FuiUtils.ToInt32(filedata, startIndex + 16);
            imageSize = FuiUtils.ToInt32(filedata, startIndex + 20);
            unk_0x18 = FuiUtils.ToInt32(filedata, startIndex + 24);
            unk_0x1C = FuiUtils.ToInt32(filedata, startIndex + 28);
        }

        public byte[] ToByteArray()
        {
            List<byte> byteList = new List<byte>();

            byteList.AddRange(FuiUtils.GetBytes(unk_0x0));
            byteList.AddRange(FuiUtils.GetBytes(unk_0x4));
            byteList.AddRange(FuiUtils.GetBytes(unk_0x8));
            byteList.AddRange(FuiUtils.GetBytes(unk_0xC));
            byteList.AddRange(FuiUtils.GetBytes(imageOffset));
            byteList.AddRange(FuiUtils.GetBytes(ImageSize));
            byteList.AddRange(FuiUtils.GetBytes(unk_0x18));
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
    }
}
