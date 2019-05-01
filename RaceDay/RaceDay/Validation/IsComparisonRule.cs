using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceDay.Validation
{
    public class IsComparisonRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public IComparisonValue CompareValue { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;
            return !string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(CompareValue.ValueToCompare()) &&  (str == CompareValue.ValueToCompare());
        }
    }

    public interface IComparisonValue
    {
        string ValueToCompare();
    }
}
