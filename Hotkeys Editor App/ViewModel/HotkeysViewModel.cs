using Hotkeys_Editor_App.Data;
using Hotkeys_Editor_App.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Hotkeys_Editor_App.ViewModel
{
    public class UIHotkey : BaseViewModel
    {
        public string Name { get; set; }
        public string[] Hotkeys { get; set; }
        public string[] Labels { get; set; }
        public Brush[] Colors { get; set; }
        public float[] Opacities { get; set; }
        public Visibility[] visibilities { get; set; }
        public int Group { get; set; }

        public UIHotkey(string name, string[] hotkeys, string[] labels, Brush[] colors, float[] opacities, Visibility[] visibilits, int group)
        {
            this.Name = name;
            this.Hotkeys = hotkeys;
            this.Labels = labels;
            this.Colors = colors;
            this.Opacities = opacities;
            this.visibilities = visibilities;
            this.Group = group;
        }

        public void ChangeHotkey(string label, string hotkey)
        {
            for (int i = 0; i < Hotkeys.Length; ++i)
            {
                if (Labels[i] == label)
                    Hotkeys[i] = hotkey;
            }
            OnPropertyChanged(nameof(Hotkeys));
        }

        public void ChangeColor(int index, Brush color)
        {
            Colors[index] = color;
            OnPropertyChanged(nameof(Colors)); 
        }
    }

    public class HotkeysViewModel : BaseViewModel
    {
        private HotkeysEditor CSF { get; set; }
        private GameInfo GameInfo { get; set; }
        private LabelsInfo LabelsInfo { get; set; }
        public List<string> Armies { get; set; }

        private ObservableCollection<UIHotkey> _Hotkeys = null;
        public ObservableCollection<UIHotkey> Hotkeys
        {
            get { return _Hotkeys; }
            set { _Hotkeys = value; OnPropertyChanged(nameof(Hotkeys)); }
        }
        private string LastHotkey = null;


        public HotkeysViewModel()
        {
            CSF = Singleton.Resourses.HotkeysEditor;
            GameInfo = Singleton.Resourses.GameInfo;
            LabelsInfo = Singleton.Resourses.LabelsInfo;
            Hotkeys = GetHotkeys();
            Armies = GameInfo.GetArmies(Singleton.Side).ToList();
            CheckForMatches();
        }

        private void CheckForMatches()
        {
            for (int i = 0; i < Hotkeys.Count; ++i)
            {
                for (int a = 0; a < Hotkeys[i].Hotkeys.Length; ++a)
                {
                    if (Hotkeys[i].Hotkeys[a] == null)
                        continue;
                    bool searchMatches = false;
                    if (Hotkeys[i].Colors[a] == Brushes.Red)
                        searchMatches = true;
                    bool found = false;
                    for (int s = 0; s < Hotkeys.Count; ++s)
                    {
                        if (Hotkeys[s].Hotkeys[a] == null)
                            continue;
                        if (Hotkeys[s].Hotkeys[a].ToLower() == Hotkeys[i].Hotkeys[a].ToLower() && s != i && Hotkeys[s].Group == Hotkeys[i].Group)
                        {
                            found = true;
                            Hotkeys[s].ChangeColor(a, Brushes.Red);
                            Hotkeys[i].ChangeColor(a, Brushes.Red);
                        }
                    }
                    if (searchMatches && !found)
                        Hotkeys[i].ChangeColor(a, Brushes.LightGreen);

                }
            }
        }

        public ObservableCollection<UIHotkey> GetHotkeys()
        {
            string sideName = Singleton.Side;
            string categoryName = Singleton.Category;
            string unitName = Singleton.Unit;
            string[][] labels = GameInfo.GetHotkeys(sideName, categoryName, unitName);
            ObservableCollection<UIHotkey> result = new ObservableCollection<UIHotkey>();
            for (int i = 0; i < labels.Length; ++i)
            {
                string name = null;
                string[] hotkeys = new string[labels[i].Length];
                Brush[] colors = new Brush[labels[i].Length];
                float[] opacities = new float[labels[i].Length];
                Visibility[] visibilities = new Visibility[labels[i].Length];
                int group = 0;
                for (int j = 0; j < labels[i].Length; ++j)
                {
                    if (labels[i][j] == null)
                        visibilities[j] = Visibility.Hidden;
                    else
                    {
                        if (name == null)
                        {
                            name = LabelsInfo.GetUnitName(labels[i][j]);
                            group = LabelsInfo.GetHotkeyGroup(labels[i][j]);
                        }
                        hotkeys[j] = CSF.GetHotkey(labels[i][j]);
                        colors[j] = Brushes.LightGreen;
                        opacities[j] = 1.0f;
                        for (int l = 0; l < j; ++l)
                            if (labels[i][j] == labels[i][l])
                                opacities[j] = 0.25f;
                        visibilities[j] = Visibility.Visible;
                    }
                }
                UIHotkey uIHotkeyInfo = new UIHotkey(name, hotkeys, labels[i], colors, opacities, visibilities, group);
                result.Add(uIHotkeyInfo);
            }
            return result;

        }

        public void UpdateUI(string label, string newHotkey)
        {
            for (int i = 0; i < Hotkeys.Count; ++i)
            {
                for (int j = 0; j < Hotkeys[i].Hotkeys.Length; ++j)
                {
                    if (Hotkeys[i].Labels[j] == label)
                    {
                        Hotkeys[i].ChangeHotkey(label, newHotkey);
                        CheckForMatches();
                        return;
                    }
                }
            }
        }

        public bool HotkeyValidate(string hotkey)
        {
            return (hotkey.First() >= 'a' && hotkey.First() <= 'z') ||
                (hotkey.First() >= 'A' && hotkey.First() <= 'Z') || hotkey == "/";
        }

        public ICommand OnBackClick
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Singleton.Unit = null;
                    Singleton.Navigation.CurrentPage = new UnitsPage();
                });
            }
        }

        public ICommand OnGotFocus
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    FindCommandParameters parameters = obj as FindCommandParameters;
                    string label = parameters.Property1;
                    LastHotkey = parameters.Property2;
                });
            }
        }

        public ICommand OnLostFocus
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    FindCommandParameters parameters = obj as FindCommandParameters;
                    string label = parameters.Property1;
                    string text = parameters.Property2;
                    if (text.Length == 0)
                    {
                        UpdateUI(label, text);
                        CSF.SetHotkey(label, text);
                    }
                    else
                    {
                        UpdateUI(label, text);
                        CSF.SetHotkey(label, text);
                    }
                });
            }
        }

        public ICommand OnTextChanged
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    FindCommandParameters parameters = obj as FindCommandParameters;
                    string label = parameters.Property1;
                    string text = parameters.Property2;
                    int CaretIndex = parameters.Property3;
                    switch (text.Length)
                    {
                        case 1:
                            if (!HotkeyValidate(text))
                                text = LastHotkey;
                            LastHotkey = text;
                            break;
                        case 0:
                            break;
                        default:
                            if (CaretIndex != 1)
                            {
                                if (!HotkeyValidate(text.Last().ToString()))
                                    text = text.First().ToString();
                                else
                                    text = text.Last().ToString();
                            }
                            else
                            {
                                if (!HotkeyValidate(text.Last().ToString()))
                                    text = text.Last().ToString();
                                else
                                    text = text.First().ToString();

                            }
                            LastHotkey = text;
                            break;
                    }
                    if (text.Length != 0)
                    {
                        UpdateUI(label, text);
                    }
                });
            }
        }
    }
}
