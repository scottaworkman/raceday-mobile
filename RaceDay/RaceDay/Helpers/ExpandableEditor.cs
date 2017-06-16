using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RaceDay.Helpers
{
    public class ExpandableEditor : Editor
    {
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
             propertyName: "Placeholder",
             returnType: typeof(string),
             declaringType: typeof(string),
             defaultValue: string.Empty);

        public string Placeholder
        {
            get { return GetValue(PlaceholderProperty).ToString(); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public ExpandableEditor()
        {
            this.TextChanged += (sender, e) => { this.InvalidateMeasure(); };
        }
    }
}
