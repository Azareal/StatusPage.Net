using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPage.Net.Misc.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToPrettyFormatShort(this TimeSpan span)
        {

            if (span == TimeSpan.Zero) return "0 minutes";

            var years = (int)span.TotalDays / 365;
            if (years > 1)
                return $"{years} years ";
            var months = (int)(span.TotalDays / 30.436875);
            if (months > 0)
                return $"{months} month{(months > 1 ? "s" : String.Empty)} ";
            if (span.Days > 0)
                return $"{span.Days % 365} day{(span.Days % 365 > 1 ? "s" : String.Empty)} ";
            if (span.Hours > 0)
                return $"{span.Hours} hour{(span.Hours > 1 ? "s" : String.Empty)} ";
            if (span.Minutes > 0)
                return $"{span.Minutes} minute{(span.Minutes > 1 ? "s" : String.Empty)} ";
            if (span.Seconds > 0)
                return $"{span.Seconds} second{(span.Seconds > 1 ? "s" : String.Empty)} ";
            return "0 seconds";
        }
        public static string ToPrettyFormatVeryShort(this TimeSpan span)
        {

            if (span == TimeSpan.Zero) return "0 m";

            var years = (int)span.TotalDays / 365;
            if (years > 1)
                return $"{years}y ";
            var months = (int)(span.TotalDays / 30.436875);
            if (months > 0)
                return $"{months}mo ";
            if (span.Days > 0)
                return $"{span.Days % 365}d ";
            if (span.Hours > 0)
                return $"{span.Hours}h ";
            if (span.Minutes > 0)
                return $"{span.Minutes}m ";
            if (span.Seconds > 0)
                return $"{span.Seconds}s ";
            return "0 seconds";
        }
        public static string ToPrettyFormat(this TimeSpan span, bool showYear = false)
        {

            if (span == TimeSpan.Zero) return "0 minutes";

            var sb = new StringBuilder();
            if (span.Days > 365 && showYear)
                sb.AppendFormat("{0} year{1} ", span.Days / 365, span.Days / 365 > 1 ? "s" : String.Empty);
            if (span.Days > 0)
                sb.AppendFormat("{0} day{1} ", span.Days % 365, span.Days % 365 > 1 ? "s" : String.Empty);
            if (span.Hours > 0)
                sb.AppendFormat("{0} hour{1} ", span.Hours, span.Hours > 1 ? "s" : String.Empty);
            if (span.Minutes > 0)
                sb.AppendFormat("{0} minute{1} ", span.Minutes, span.Minutes > 1 ? "s" : String.Empty);
            if (span.Seconds > 0)
                sb.AppendFormat("{0} second{1} ", span.Seconds, span.Seconds > 1 ? "s" : String.Empty);
            return sb.ToString();

        }
    }
}
