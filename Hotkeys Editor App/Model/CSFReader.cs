using System;
using System.IO;
using System.Text;
using static Hotkeys_Editor_App.TextResourse;

namespace Hotkeys_Editor_App.Model
{
    public static class CSFReader
    {

        public static HotkeysEditor Open(string path)
        {
            byte[] file = File.ReadAllBytes(path);
            Header header = ParseHeader(file);
            TextResourse[] textAssets = ParseAssets(file, header.labelsCount);
            return new HotkeysEditor(header, textAssets);
        }

        public static void Save(HotkeysEditor file, string path)
        {
            FileStream f = File.OpenWrite(path);
            f.Write(file.Header.ToByteArray(), 0, 24);
            for (int i = 0; i < file.Header.labelsCount; ++i)
            {
                byte[] csfString = file.TextResourses[i].ToByteArray();
                f.Write(csfString, 0, csfString.Length);
            }
            f.Close();
        }

        private static Header ParseHeader(byte[] file)
        {
            if (!Header.IsHeaderCorrect(file))
                throw new FileIsNotCorrect();
            int labelsCount = BitConverter.ToInt32(file, 8);
            int valuesCount = BitConverter.ToInt32(file, 12);
            return new Header(labelsCount, valuesCount);
        }

        private static TextResourse[] ParseAssets(byte[] file, int stringCounts, int startIndex = 24)
        {
            TextResourse[] result = new TextResourse[stringCounts];
            for (int i = 0, index = startIndex; i < stringCounts; ++i)
            {
                (result[i], index) = ParseAsset(file, index);
                if (index == -1)
                    throw new FileIsNotCorrect();
            }
            return result;
        }

        private static (TextResourse, int) ParseAsset(byte[] arr, int index)
        {
            if (!Algos.IsByteArraysEquals(arr, LBL_IDENTIFER, index, LBL_IDENTIFER.Length))
                throw new FileIsNotCorrect();
            index += LBL_IDENTIFER.Length;

            bool valueIsExists = Algos.IsByteArraysEquals(arr, VALUE_EXISTS, index, VALUE_EXISTS.Length);
            index += 4;

            int labelLength = BitConverter.ToInt32(arr, index);
            index += 4;

            string label = Encoding.ASCII.GetString(arr, index, labelLength);
            index += labelLength;

            if (!valueIsExists)
                return (new TextResourse(label), index);

            bool isRTS = false;
            if (Algos.IsByteArraysEquals(arr, RTS_IDENTIFIER, index, RTS_IDENTIFIER.Length))
                isRTS = true;
            else if (!Algos.IsByteArraysEquals(arr, WRTS_IDENTIFIER, index, WRTS_IDENTIFIER.Length))
                throw new FileIsNotCorrect();
            index += 4;

            int valueLength = BitConverter.ToInt32(arr, index) * 2;
            index += 4;

            byte[] valueArr = new byte[valueLength];
            for (int i = 0, valueEnd = index + valueLength; index < valueEnd; ++index, ++i)
                valueArr[i] = arr[index];
            string value = GetDecodedValue(valueArr);

            if (isRTS)
                return (new TextResourse(label, value), index);

            int descriptionLength = BitConverter.ToInt32(arr, index);
            index += 4;

            string description = Encoding.ASCII.GetString(arr, index, descriptionLength);
            index += descriptionLength;
            return (new TextResourse(label, value, description), index);
        }

        private static string GetDecodedValue(byte[] descriptionArr)
        {
            for (int i = 0; i < descriptionArr.Length; ++i)
                descriptionArr[i] = (byte)(255 - descriptionArr[i]);
            return Encoding.Unicode.GetString(descriptionArr);

        }
    }
}
