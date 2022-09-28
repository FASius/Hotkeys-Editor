using System;
using System.Linq;

namespace Hotkeys_Editor_App
{
    public class FileIsNotCorrect : Exception {
        public FileIsNotCorrect() { }
        public FileIsNotCorrect(string message) : base(message) { }
        public FileIsNotCorrect(string message, Exception innerException) : base(message, innerException) { }
    }

    
    public class HotkeysEditor
    {
        internal Header Header;
        internal TextResourse[] TextResourses;

        internal HotkeysEditor(Header header, TextResourse[] textResourses)
        {
            this.Header = header;
            this.TextResourses = textResourses;
        }

        public string GetHotkey(string label)
        {
            for (int i = 0; i < TextResourses.Length; ++i)
                if (TextResourses[i].Label.ToLower() == label.ToLower())
                    for (int j = 1; j < TextResourses[i].Value.Length; ++j)
                        if (TextResourses[i].Value[j - 1] == '&' && char.IsLetter(TextResourses[i].Value[j])) 
                            return TextResourses[i].Value[j].ToString();
            return null;
        }

        public void SetHotkey(string label, string hotkey)
        {
            for (int i = 0; i < TextResourses.Length; ++i)
                if (TextResourses[i].Label.ToLower() == label.ToLower())
                {
                    string value = TextResourses[i].Value;
                    if (hotkey == "")
                    {
                        TextResourses[i].Value = value.Replace("&", "").Replace("[", "").Replace("]", "");
                        return;
                    }
                    if (value.Contains('&'))
                    {
                        if (value.Contains("[&"))
                        {
                            value = value.Remove(value.IndexOf('&') + 1, 1);  
                            TextResourses[i].Value = value.Insert(value.IndexOf('&'), hotkey);
                        }
                        else
                        {
                            value = value.Remove(value.IndexOf('&'), 1);
                            TextResourses[i].Value = $"[&{hotkey}] {value}";
                        }
                        return;
                    }
                    else
                    {
                        TextResourses[i].Value = $"[&{hotkey}] {value}";
                        return;
                    }
                }
        }
    }
}
