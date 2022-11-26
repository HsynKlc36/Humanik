using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.BUSINESS.Concrete
{
    public static class TextCases
    {
        public static string ToTitleCase(this string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }

        public static string ToGlobal(this string str)
        {
            string result = String.Join("", str.Normalize(NormalizationForm.FormD)
               .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
            return result;
        }
    }
}
