using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityHelper
{
    public class DateTimeHelper
    {
        public static DateTime TimeStampParse(string inputText, string format = "yyyy-MM-dd'T'HH:mm:ss")
        {
            var regex = new System.Text.RegularExpressions.Regex(@"\d{4}-\d{2}-\d{2}T\d{2}-\d{2}-\d{2}");
            foreach (System.Text.RegularExpressions.Match m in regex.Matches(inputText))
            {
                DateTime dt;
                if (DateTime.TryParseExact(m.Value,
                                      format,
                                      CultureInfo.InvariantCulture,
                                      DateTimeStyles.AssumeUniversal |
                                      DateTimeStyles.AdjustToUniversal, out dt))

                    return dt;
            }
            return default(DateTime);
        }
    }
}
