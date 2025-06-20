using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Globalization;

namespace WebAPISalesManagement.Helpers
{
    public class StringConvert
    {
        public StringConvert()
        {

        }
        public static string DateTimeConvert(DateTime datetime, bool isOnlyDate) {
            if (!isOnlyDate) {
                return Convert.ToDateTime(datetime.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
            }
            else {
                return Convert.ToDateTime(datetime.ToString()).ToString("dd/MM/yyyy");
            }
           
        }
        public string ConvertToUnSign(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "";
            }
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2.ToLower();
        }
        public static string stringCutter(string text, int length = 30)
        {
            text = GetPlainText(text);
            return text;
            //return (text.Length > length ? text.Substring(0, length).Trim() + "..." : text);
        }
        public static string GetPlainText(string text)
        {
            text = HttpUtility.HtmlDecode(text);
            return Regex.Replace(text, @"<(.|\n)*?>", "");
        }
    }
}
