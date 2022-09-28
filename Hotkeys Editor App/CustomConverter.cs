using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Hotkeys_Editor_App
{
    public class CustomConverter : IMultiValueConverter
    {
        public object Convert(object[] Values, Type Target_Type, object Parameter, CultureInfo culture)
        {
            var findCommandParameters = new FindCommandParameters();
            findCommandParameters.Property1 = (string)Values[0];
            findCommandParameters.Property2 = (string)Values[1];
            findCommandParameters.Property3 = (Values[2] as TextBox).CaretIndex;
            return findCommandParameters;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FindCommandParameters
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
        public int Property3 { get; set; }
    }
}
