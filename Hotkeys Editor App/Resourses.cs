using Hotkeys_Editor_App.Data;
using Hotkeys_Editor_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotkeys_Editor_App
{
    public class Resourses
    {
        public const string DEFAULT_CSF_FILE_PATH = "Data/generals.csf";
        public const string GAME_INFO_PATH = "Data/UnitsInfo.json";
        public const string HOTKEYS_INFO = "Data/HotkeysInfo.json";

        public HotkeysEditor HotkeysEditor;
        public GameInfo GameInfo;
        public LabelsInfo LabelsInfo;
    }
}
