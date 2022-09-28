using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Hotkeys_Editor_App.ViewModel
{
    public class UnitsViewModel : BaseViewModel
    {
        private List<string> _Units;
        public List<string> Units
        {
            get { return _Units; }
            set { _Units = value; OnPropertyChanged(nameof(Units)); }
        }

        public UnitsViewModel()
        {
            string sideName = Singleton.Side;
            string categoryName = Singleton.Category;
            Units = Singleton.Resourses.GameInfo.GetUnits(sideName, categoryName).ToList();
        }

        public ICommand OnUnitClick
        {
            get
            {
                return new DelegateCommand((obj) =>
            {
                Singleton.Unit = obj.ToString();
                Singleton.Navigation.CurrentPage = new HotkeysPage();
            });
            }
        }

        public ICommand OnBackClick
        {
            get
            {
                return new DelegateCommand((obj) => {
                    Singleton.Unit = null;
                    Singleton.Navigation.CurrentPage = new CategoriesPage();
                });
            }
        }
    }
}
