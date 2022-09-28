using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Hotkeys_Editor_App.ViewModel
{
    public class SidesViewModel : BaseViewModel
    {
        private List<string> _SidesList;
        public List<string> SidesList
        {
            get { return _SidesList; }
            set { _SidesList = value; OnPropertyChanged(nameof(SidesList)); }
        }

        public SidesViewModel()
        {
            SidesList = Singleton.Resourses.GameInfo.GetSides().ToList();
        }

        public ICommand OnSideClick
        {
            get
            {
                return new DelegateCommand((obj) =>
            {
                Singleton.Side = obj.ToString();
                Singleton.Navigation.CurrentPage = new CategoriesPage();
            });
            }
        }
    }
}
