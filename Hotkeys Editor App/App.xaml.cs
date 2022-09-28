using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Hotkeys_Editor_App.Navigation;

namespace Hotkeys_Editor_App
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class Singleton : System.Windows.Application
    {
        public Singleton()
        {
            InitializeComponent();
            Resourses = new Resourses();
            Resourses.GameInfo = new Data.GameInfo(Resourses.GAME_INFO_PATH);
            Resourses.LabelsInfo = new Model.LabelsInfo(Resourses.HOTKEYS_INFO);
            Navigation = new NavigationService();
        }
        public static NavigationService Navigation;
        public static Resourses Resourses;
        public static string Side;
        public static string Category;
        public static string Unit;
    }
}
