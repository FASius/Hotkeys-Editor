using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Hotkeys_Editor_App.ViewModel
{
    public class CategoriesViewModel : BaseViewModel
    {
        private List<string> _Categories;
        public List<string> Categories { get { return _Categories; } 
            set { _Categories = value; OnPropertyChanged(nameof(Categories)); } }


        public CategoriesViewModel()
        {
            string sideName = Singleton.Side;
            Categories = Singleton.Resourses.GameInfo.GetCategories(sideName).ToList();
        }

        public ICommand OnCategoryClick
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Singleton.Category = obj.ToString();
                    Singleton.Navigation.CurrentPage = new UnitsPage();
                });
            }
        }

        public ICommand OnBackClick
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Singleton.Category = null;
                    Singleton.Navigation.CurrentPage = new SidesPage();
                });
            }
        }
    }
}
