using System;
using System.Text;

namespace Hotkeys_Editor_App
{
    /* RTS text resourse structure with value:
        4  bytes   always CONST (LBL_IDENTIFER)
        4  bytes   equals 0x0 means resourse with value
        4  bytes   length of ascii string (n)
        n  bytes   ascii string (Label)
        4  bytes   always CONST (RTS_IDENTIFIER)
        4  bytes   length of unicode string (k)
        2k bytes   unicode string: XOR'ed by 255 (Value)
       
       RTS text resourse structure without value:
        4  bytes   always const (LBL_IDENTIFER)
        4  bytes   equals 0x1 means resourse without value
        4  bytes   length of ascii string (n)
        n  bytes   ascii string (Label)

       WRTS text resourse structure:
        4  bytes   always CONST (LBL_IDENTIFER)
        4  bytes   equals 0x0 means resourse with value
        4  bytes   length of ascii string (n)
        n  bytes   ascii string (label)
        4  bytes   always CONST (WRTS_IDENTIFIER)
        4  bytes   length of unicode string (k)
        2k bytes   unicode string: XOR'ed by 255 (value)
        4  bytes   length of ascii string (j)
        j  bytes   ascii string (description)
    */

    internal class TextResourse
    {
        public static readonly byte[] LBL_IDENTIFER = new byte[4] { 0x20, 0x4c, 0x42, 0x4c };
        public static readonly byte[] VALUE_EXISTS = new byte[4] { 0x01, 0x00, 0x00, 0x00 };
        public static readonly byte[] VALUE_IS_NOT_EXISTS = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
        public static readonly byte[] RTS_IDENTIFIER = new byte[4] { 0x20, 0x52, 0x54, 0x53 };
        public static readonly byte[] WRTS_IDENTIFIER = new byte[4] { 0x57, 0x52, 0x54, 0x53 };

        public string Label;
        public string Value;
        private bool IsWTS;
        private bool IsValueExists;
        private string Description;


        public TextResourse(string label, string value)
        {
            this.Label = label;
            this.Value = value;
            this.IsWTS = false;
            this.Description = null;
            IsValueExists = true;
        }

        public TextResourse(string label)
        {
            this.Label = label;
            this.IsValueExists = false;
        }

        public TextResourse(string label, string value, string desciption) : this(label, value)
        {
            this.IsWTS = true;
            this.Description = desciption;
        }

        public byte[] ToByteArray()
        {

            if (!IsValueExists)
                return WithoutValueToByteArray();
            if (IsWTS)
                return WRTSToByteArray();
            return RTSToByteArray();
        }

        private byte[] WithoutValueToByteArray()
        {
            byte[] arr = new byte[12 + Label.Length];
            LBL_IDENTIFER.CopyTo(arr, 0);
            VALUE_IS_NOT_EXISTS.CopyTo(arr, 4);
            BitConverter.GetBytes(Label.Length).CopyTo(arr, 8);
            Encoding.ASCII.GetBytes(Label).CopyTo(arr, 12);
            return arr;
        }

        private byte[] WRTSToByteArray()
        {
            byte[] arr = new byte[24 + Label.Length + Value.Length * 2 + Description.Length];
            LBL_IDENTIFER.CopyTo(arr, 0);
            VALUE_EXISTS.CopyTo(arr, 4);
            BitConverter.GetBytes(Label.Length).CopyTo(arr, 8);
            Encoding.ASCII.GetBytes(Label).CopyTo(arr, 12);
            WRTS_IDENTIFIER.CopyTo(arr, Label.Length + 12);
            BitConverter.GetBytes(Value.Length).CopyTo(arr, Label.Length + 16);
            GetEncodedValue().CopyTo(arr, Label.Length + 20);
            BitConverter.GetBytes(Description.Length).CopyTo(arr, Label.Length + 20 + Value.Length * 2);
            Encoding.ASCII.GetBytes(Description).CopyTo(arr, arr.Length - Description.Length);
            return arr;
        }

        private byte[] RTSToByteArray()
        {
            byte[] arr = new byte[20 + Label.Length + Value.Length * 2];
            LBL_IDENTIFER.CopyTo(arr, 0);
            VALUE_EXISTS.CopyTo(arr, 4);
            BitConverter.GetBytes(Label.Length).CopyTo(arr, 8);
            Encoding.ASCII.GetBytes(Label).CopyTo(arr, 12);
            RTS_IDENTIFIER.CopyTo(arr, Label.Length + 12);
            BitConverter.GetBytes(Value.Length).CopyTo(arr, Label.Length + 16);
            GetEncodedValue().CopyTo(arr, Label.Length + 20);
            return arr;
        }

        private byte[] GetEncodedValue()
        {
            byte[] arr = Encoding.Unicode.GetBytes(Value);
            for (int i = 0; i < arr.Length; ++i)
                arr[i] = (byte)(255 - arr[i]);
            return arr;
        }

    }
}
