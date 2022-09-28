using System;
using System.Windows.Controls;

namespace Hotkeys_Editor_App.Navigation
{
    public class NavigationService
    {
        private Page _currentPage;
        public Page CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnCurrentPageChanged();
            }
        }

        public event Action CurrentPageChanged;

        private void OnCurrentPageChanged()
        {
            CurrentPageChanged?.Invoke();
        }
    }
}
