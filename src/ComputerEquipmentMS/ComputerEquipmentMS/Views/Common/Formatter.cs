using System.Globalization;
using System.Text;
using NodaTime;

namespace ComputerEquipmentMS.Views.Common;

public static class Formatter
{
    public static string FormatInstantToTime(Instant i) =>
        i.ToString("HH:mm", new CultureInfo("ru-RU"));
    
    public static string FormatPeriodToString(Period p)
    {
        var str = new StringBuilder();
        if (p.Years > 0)
            AddYears();
        if (p.Months > 0)
            AddMonths();
        if (p.Weeks > 0 || p.Days > 0)
            AddDays();

        return str.ToString();
            
        void AddYears()
        {
            str.Append(p.Years);
            str.Append(p.Years switch
            {
                1 => " год",
                >= 2 and <= 4 => " года",
                _ => " лет"
            });
        }
        
        void AddMonths()
        {
            str.Append(p.Months);
            str.Append(p.Months switch
            {
                1 => " месяц",
                _ => " месяцев"
            });
        }
        
        void AddDays()
        {
            var days = p.Weeks * 7 + p.Days;
            str.Append(days);
            str.Append(days switch
            {
                1 => " день",
                >= 2 and <= 4 => " дня",
                _ => " дней"
            });
        }
    }
}