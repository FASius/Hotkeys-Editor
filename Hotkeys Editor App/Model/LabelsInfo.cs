using System.IO;
using System.Text.Json;

namespace Hotkeys_Editor_App.Model
{
    public class LabelsInfo
    {
        private class LabelInfo
        {
            public string label { get; set; }
            public string name { get; set; }
            public int group { get; set; }
        }

        private LabelInfo[] labelsInfo { get; set; }

        public LabelsInfo(string path)
        {
            string file = File.ReadAllText(path);
            labelsInfo = JsonSerializer.Deserialize<LabelInfo[]>(file);
        }

        public int GetHotkeyGroup(string labelName)
        {
            for (int i = 0; i < labelsInfo.Length; ++i)
                if (labelsInfo[i].label.ToLower() == labelName.ToLower())
                    return labelsInfo[i].group;
            return -1;
        }

        public string GetUnitName(string label)
        {
            for (int i = 0; i < labelsInfo.Length; ++i)
                if (labelsInfo[i].label == label)
                    return labelsInfo[i].name;
            return null;
        }
    }
}
