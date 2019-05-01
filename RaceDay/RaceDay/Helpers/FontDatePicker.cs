using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RaceDay.Helpers
{
    public class FontDatePicker : DatePicker
    {
        public new static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
            propertyName: "FontSize",
            returnType: typeof(int),
            declaringType: typeof(int),
            defaultValue: 18);

        public new int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
    }
}
