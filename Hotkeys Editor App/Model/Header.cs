using System;

namespace Hotkeys_Editor_App
{
    /* HEADER:
        8 bytes   allways const
        4 bytes   labels count
        4 bytes   values count
        8 bytes   allways const 
    */
    internal class Header
    {
        public static readonly int HEADER_LENGTH = 24;
        public static readonly byte[] HEADER_START = new byte[8] { 0x20, 0x46, 0x53, 0x43, 0x03, 0x00, 0x00, 0x00 };
        public static readonly byte[] HEADER_END = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

        public int labelsCount;
        public int valueCount;


        public Header (int labelsCount, int valueCount)
        {
            this.labelsCount = labelsCount;
            this.valueCount = valueCount;
        }

        public byte[] ToByteArray()
        {
            byte[] arr = new byte[HEADER_LENGTH];
            HEADER_START.CopyTo(arr, 0);
            BitConverter.GetBytes(labelsCount).CopyTo(arr, 8);
            BitConverter.GetBytes(valueCount).CopyTo(arr, 12);
            HEADER_END.CopyTo(arr, 16);
            return arr;
        }

        public static bool IsHeaderCorrect(byte[] header)
        {
            return Algos.IsByteArraysEquals(header, HEADER_START, 0, 8) &&
                Algos.IsByteArraysEquals(header, HEADER_END, 16, 8);
        }
    }
}
