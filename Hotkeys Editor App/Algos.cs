using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotkeys_Editor_App
{
    internal class Algos
    {
        public static bool IsByteArraysEquals(byte[] arr, byte[] cmpArr, int startIndex, int length)
        {
            for (int i = 0; i < length; ++i)
                if (arr[startIndex + i] != cmpArr[i])
                    return false;
            return true;
        }
    }
}
