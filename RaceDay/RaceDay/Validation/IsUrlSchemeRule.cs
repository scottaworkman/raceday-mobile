using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceDay.Validation
{
    public class IsUrlSchemeRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return true;
            }

            var str = value as string;

            if (!string.IsNullOrEmpty(str) && str.ToLower().StartsWith("http://") == false && str.ToLower().StartsWith("https://") == false)
                return false;

            return true;
        }
    }
}
