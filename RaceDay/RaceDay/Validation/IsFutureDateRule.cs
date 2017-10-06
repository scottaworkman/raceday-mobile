using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceDay.Validation
{
    public class IsFutureDate<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            DateTime date = DateTime.MinValue;
            if (value is string str)
            {
                DateTime.TryParse(str, out date);
            }
            else if (value is DateTime dt)
            {
                date = dt;
            }

            if (date != DateTime.MinValue)
                return date >= DateTime.Now.Date;

            return false;
        }
    }
}
