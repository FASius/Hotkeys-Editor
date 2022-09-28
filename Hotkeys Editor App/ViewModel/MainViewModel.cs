using Hotkeys_Editor_App.Model;
using Hotkeys_Editor_App.Navigation;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hotkeys_Editor_App.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private NavigationService _Navigation;
        public Page CurrentPage => _Navigation.CurrentPage; 

        private string Path;
        private Resourses Resourses;

        private Page WelcomePage;

        public MainViewModel()
        {
            WelcomePage = new WelcomePage();
            Resourses = Singleton.Resourses;
            _Navigation = Singleton.Navigation;
            _Navigation.CurrentPageChanged += OnCurrentPageChanged;
            _Navigation.CurrentPage = WelcomePage;
        }

        private void OnCurrentPageChanged()
        {
            OnPropertyChanged(nameof(CurrentPage));
        }
        public ICommand OnOpenClick
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    var dialog = new OpenFileDialog();
                    bool? result = dialog.ShowDialog();
                    if (result == true)
                    {
                        Path = dialog.FileName;
                        Resourses.HotkeysEditor = CSFReader.Open(Path);
                        _Navigation.CurrentPage = new SidesPage();
                    }
                });
            }
        }

        public ICommand OnCloseClick
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Resourses.HotkeysEditor = null;
                    Path = null;
                    _Navigation.CurrentPage = WelcomePage;
                }, (obj) => { return Resourses.HotkeysEditor != null; });
            }
        }

        public ICommand OnSaveClick
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    CSFReader.Save(Resourses.HotkeysEditor, Path);
                }, (obj) => { return Resourses.HotkeysEditor != null 
                    && Path != null; });
            }
        }

        public ICommand OnSaveAsClick
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = "generals.csf";
                    saveFileDialog.ShowDialog();
                    Path = saveFileDialog.FileName;
                    CSFReader.Save(Resourses.HotkeysEditor, Path);
                }, (obj) => { return Resourses.HotkeysEditor != null; });
            }
        }

        public ICommand OnDefualtOpenClick
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    try
                    {
                        Resourses.HotkeysEditor = CSFReader.Open(Resourses.DEFAULT_CSF_FILE_PATH);
                        _Navigation.CurrentPage = new SidesPage();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                });
            }
        }
    }
}
