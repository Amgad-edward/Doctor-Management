using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Doctor_Management.Controllers;
using System.Threading;
using Doctor_Management.Models_View;
using Doctor_Management.Models;

namespace Doctor_Management
{
    public static class Hex
    {

        public static string Treal = $"15{DateTime.Now.Year}{DateTime.Now.Hour}";

        private const string Code = "001D0F1F90CAED110727010C";

        public const int MaxAllowImage = 1048576;

        //public static string User { get; set; } = "";

        //public static bool Login { get; set; } = false;

        //public static bool Admin { get; set; } = false;

        public static bool IsTry { get; set; }

        public static string NameOwner { get; set; } 

        public static byte[] Logo { get; set; }

        public static string ToDate(this DateTime dateTime)
        {
            return dateTime.ToString("dddd - dd-MM-yyyy");
        }

        public static string GetSpan(this DateTime date)
        {
            TimeSpan ts = DateTime.Now - date;
            double days = ts.TotalDays;
            double Hours = ts.TotalHours;
            double Minutes = ts.TotalMinutes;
            if(days >= 1)
            {
                if (days < 30 && days >= 1)
                    return $" {Math.Round(days)} days ago";
                else if (days >= 30 && days < 365)
                    return $" {Math.Round(days / 30)} Month ago";
                else if (days >= 365)
                    return $" {Math.Round(days / 365)} years ago";
            }
            else if(Hours < 24 && Hours >= 1)
            {
                return $" {Math.Round(Hours)} Hour ago";
            }

            return $" {Math.Round(Minutes)} Minutes ago";
        }

        public static string showimage(this byte[] bytes , string type = "*")
        {
            if (bytes is not null)
                return $"data:image/{type};base64,{Convert.ToBase64String(bytes)}";

            return string.Empty;
        } 

        public static byte[] setimage(this IFormFile formFile)
        {
            if(formFile != null)
            {
                using var stream = new MemoryStream();
                formFile.CopyTo(stream);
                return stream.ToArray();
            }
            return null;
        }

        public static byte[] setimage(this IFormFileCollection formFile)
        {
            if (formFile != null)
            {
                using var stream = new MemoryStream();
                formFile[0].CopyTo(stream);
                return stream.ToArray();
            }
            return null;
        }

        public static int Age(this DateTime dateTime)
        {
            return DateTime.Now.Year - dateTime.Year;
        }

        public static string GetAge(this DateTime dateTime)
        {
            var ts = DateTime.Now - dateTime;
            var days = ts.TotalDays;
            if (days < 30 && days > 0)
                return $" {Math.Round(days)} days";

            else if (days >= 30 && days < 365)
                return $" {Math.Round(days / 30)} Month";

            else if(days >= 365)
                return $" {Math.Round(days / 365)} years";

            return "";
        }

        public static IEnumerable<CustomerInfo> ToInfo(this IEnumerable<Customer> customers)
        {
            foreach (var item in customers)
            {
                yield return item;
            }
        }

        public static string ConvertName(this string Name)
        {
            var name = "";
            try
            {
                foreach (var C in Name)
                    name += GetChar(C);
            }
            catch 
            {
                return Name;
            }

            return name;
        }

        public static string TextToNumber(this int number , string gender = "")
        {
            return Create(number) + $" {gender}";
        }

        public static string TextToNumber(this decimal number, string gender = "")
        {
            return Create(number) + $" {gender}";
        }

        public static string TextToNumber(this double number, string gender = "")
        {
            return Create(number) + $" {gender}";
        }

        public static string TextToNumber(this string number, string gender = "")
        {
            var Numberonly = NumberOnly(number);
            return Create(Numberonly) + $" {gender}";
        }

        public static bool GetActive()
        {
            ManagementObjectCollection collection;
            using var difine = new ManagementObjectSearcher("SELECT * FROM Win32_USBHub");
            collection = difine.Get();
            foreach (var item in collection)
            {
                var Arr = item.GetPropertyValue("DeviceID").ToString().Split('\\');
                var N = Arr[Arr.Length - 1];
                if (N == Code)
                    return true;
            }

            return false;
        }

        private static string GetChar(char Char)
        {
            Dictionary<char, string> keys = new Dictionary<char, string>();
            keys.Add(' ', " ");
            keys.Add('ا', "A");
            keys.Add('أ', "A");
            keys.Add('إ', "E");
            keys.Add('آ', "AE");
            keys.Add('ب', "B");
            keys.Add('ت', "T");
            keys.Add('ث', "TH");
            keys.Add('ج', "GA");
            keys.Add('ح', "HA");
            keys.Add('خ', "KH");
            keys.Add('د', "D");
            keys.Add('ذ', "Z");
            keys.Add('ر', "R");
            keys.Add('ز', "ZE");
            keys.Add('س', "S");
            keys.Add('ش', "SH");
            keys.Add('ص', "SHA");
            keys.Add('ض', "DA");
            keys.Add('ط', "TA");
            keys.Add('ظ', "ZAH");
            keys.Add('ع', "A");
            keys.Add('غ', "GH");
            keys.Add('ف', "F");
            keys.Add('ق', "KA");
            keys.Add('ك', "K");
            keys.Add('ل', "L");
            keys.Add('م', "M");
            keys.Add('ن', "N");
            keys.Add('ه', "HA");
            keys.Add('و', "W");
            keys.Add('ؤ', "WI");
            keys.Add('ى', "I");
            keys.Add('ي', "EE");
            keys.Add('ء', "EE");
            keys.Add('ئ', "E");
            keys.Add('-', "-");
            keys.Add('ة', "H");

            return keys[Char];

        }
        private static string Create<Ty>(Ty obj)
        {
            var Result = "";
            char stop = ' ';
            string Numberstr = "";
            double Number = 0;//32 ,  256 ,302
            var plus = 0;
            switch (obj)
            {
                case string s:
                    s = NumberOnly(s);
                    if (s.Length > 0)
                    {
                        Numberstr = s;
                        Number = Convert.ToDouble(s);
                    }
                    else
                        throw new InvalidOperationException("لا توجد قيمة");
                    break;
                case double d:
                    Numberstr = d.ToString();
                    Number = d;
                    break;
                case long L:
                    Numberstr = L.ToString();
                    Number = L;
                    break;
                case float f:
                    Numberstr = f.ToString();
                    Number = f;
                    break;
                case decimal m:
                    if (m < decimal.MaxValue)
                    {
                        Numberstr = m.ToString();
                        Number = Convert.ToDouble(m);
                    }
                    else
                        throw new InvalidOperationException("The Number Of Decimal Can Not Be Convert");
                    break;
                case int n:
                    Numberstr = n.ToString();
                    Number = Convert.ToDouble(n);
                    break;
                default:
                    string st = NumberOnly(obj.ToString());
                    if (st.Length > 0)
                    {
                        Numberstr = st;
                        Number = Convert.ToDouble(st);
                    }
                    else
                        throw new InvalidOperationException("There are no numeric values, you must enter numbers to change them ");
                    break;
            }

            foreach (var N in Numberstr)
            {
                if (Number >= 1000000 && Number <= 99000000)
                {
                    if (Number >= 1000000 && Number <= 9999999)
                    {
                        Result = Values_1(N) + " مليون";
                        Number -= Convert.ToDouble(N.ToString()) * 1000000;
                    }
                    else
                    {
                        if (stop == ' ')
                        {
                            stop = (N != '0') ? N : ' ';
                            continue;
                        }
                        else
                        {
                            Result = Values_1(N) + ((stop != '1') ? " و " : " ") + Values_10(stop) + " مليون";
                            var str = stop.ToString() + N.ToString();
                            stop = ' ';
                            Number -= Convert.ToDouble(str) * 1000000;
                        }
                    }
                }
                else if (Number < 1000000 && Number >= 100000)
                {
                    plus += 1;
                    Result += (Result != "" && N != '0' ? " و " : "") + Values_100(N) + " الف";
                    Number -= Convert.ToDouble(N.ToString()) * 100000;
                }

                else if (Number < 100000 && Number >= 10000)
                {
                    if (stop == ' ')
                    {
                        stop = (N != '0') ? N : ' ';
                        continue;
                    }
                    else
                    {
                        Result += (Result != "" ? " و " : "") + Values_1(N) + $" {((N != '0') ? "و" : "")} {Values_10(stop)}";
                        var str = stop.ToString() + N.ToString();
                        Number -= Convert.ToDouble(str) * 1000;
                        Result = Result.Replace(" الف", "");
                        Result += " الف";
                        stop = ' ';
                    }

                }
                else if (Number < 10000 && Number >= 1000)
                {
                    Result += (Result != "" && N != '0' ? " و " : "") + Values_1000(N);
                    Number -= Convert.ToDouble(N.ToString()) * 1000;
                    Result = Result.Replace(" الف", "");

                }

                else if (Number < 1000 && Number >= 100)
                {
                    if (Number >= 100)
                    {
                        Result += (Result != "" && N != '0' ? " و " : "") + Values_100(N);
                        Number -= Convert.ToDouble(N.ToString()) * 100;
                    }

                }
                else if (Number < 100 && Number >= 10)
                {
                    if (stop == ' ')
                    {
                        stop = (N != '0') ? N : ' ';
                        continue;
                    }
                    else
                    {
                        Result += (Result != "" ? " و " : "") + Values_1(N) + $" {((int.Parse(stop.ToString()) >= 2 && N != '0') ? "و" : "")} {Values_10(stop)}";
                        var str = stop.ToString() + N.ToString();
                        Number -= Convert.ToDouble(str);
                        stop = ' ';
                    }
                }
                else if (Number < 10 && Number >= 1)
                {
                    Result += (Result != "" && N != '0' ? " و " : "") + Values_1(N);
                    Number -= Convert.ToDouble(N.ToString());
                }
            }
            return Result;
        }
        private static string NumberOnly(string Number)
        {
            var number = "";
            foreach (var N in Number)
            {
                if ((N == '1') || (N == '2') || (N == '3') || (N == '4') || (N == '5') || (N == '6') || (N == '7') || (N == '8') || (N == '9') || (N == '0'))
                    number += N;
            }

            return number;
        }
        private static string Values_1(char Key)
        {
            Dictionary<char, string> keys = new Dictionary<char, string>();
            keys.Add('1', "واحد");
            keys.Add('2', "اثنان");
            keys.Add('3', "ثلاثة");
            keys.Add('4', "اربعة");
            keys.Add('5', "خمسة");
            keys.Add('6', "ستة");
            keys.Add('7', "سبعة");
            keys.Add('8', "ثمانية");
            keys.Add('9', "تسعة");
            keys.Add('0', "");
            return keys[Key];
        }
        private static string Values_1000(char Key)
        {
            Dictionary<char, string> keys = new Dictionary<char, string>();
            keys.Add('1', "الف");
            keys.Add('2', "الفان");
            keys.Add('3', "ثلاث الالف");
            keys.Add('4', "اربع الالف");
            keys.Add('5', "خمس الالف");
            keys.Add('6', "ستة الالف");
            keys.Add('7', "سبع الالف");
            keys.Add('8', "ثمانية الالف");
            keys.Add('9', "تسع الالف");
            keys.Add('0', "");
            return keys[Key];
        }
        private static string Values_100(char Key)
        {
            Dictionary<char, string> keys = new Dictionary<char, string>();
            keys.Add('1', "مائة");
            keys.Add('2', "مئتان");
            keys.Add('3', "ثلاثمائة");
            keys.Add('4', "اربعومائة");
            keys.Add('5', "خمسمائة");
            keys.Add('6', "ستمائة");
            keys.Add('7', "سبعومائة");
            keys.Add('8', "ثمانمائة");
            keys.Add('9', "تسعمائة");
            keys.Add('0', "");
            return keys[Key];
        }
        private static string Values_10(char Key)
        {
            Dictionary<char, string> keys = new Dictionary<char, string>();
            keys.Add('1', "عشر");
            keys.Add('2', "عشرون");
            keys.Add('3', "ثلاثون");
            keys.Add('4', "اربعون");
            keys.Add('5', "خمسون");
            keys.Add('6', "ستين");
            keys.Add('7', "سبعون");
            keys.Add('8', "ثمانين");
            keys.Add('9', "تسعون");
            keys.Add('0', "");
            return keys[Key];
        }
    }
}
