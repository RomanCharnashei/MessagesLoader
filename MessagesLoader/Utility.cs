using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesLoader
{
    internal class Utility
    {
        public static CultureInfo CreateRusCulture()
        {
            var originCulture = CultureInfo.CreateSpecificCulture("ru-RU");

            originCulture.DateTimeFormat.AbbreviatedMonthNames = new[] { "янв", "фев", "мар", "апр", "мая", "июн", "июл", "авг", "сен", "окт", "ноя", "дек", "" };
            originCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames = originCulture.DateTimeFormat.AbbreviatedMonthNames;

            return originCulture;
        }
    }
}
