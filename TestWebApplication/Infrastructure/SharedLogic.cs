using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApplication.WebUI.Infrastructure
{
    public static class SharedLogic
    {
        public static decimal DisplaySizeFromStr(string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            string digits = "0123456789";
            int intPart = 0; bool flagInt = false;
            int decPart = 0;
            int tempNumber = 0;
            string sInt = string.Empty;
            string sDec = string.Empty;
            decimal num = 0;
            foreach (char ch in str)
            {
                if (!flagInt)
                {
                    if (ch == '.' || ch == ' ')
                    {
                        while (tempNumber < digits.Length)
                        {
                            if (sInt[0] == digits[tempNumber])
                            {
                                flagInt = true;
                                intPart = tempNumber;
                                tempNumber = 0;
                                break;
                            }
                            tempNumber++;
                        }
                    }
                    sInt += ch;
                }
                else
                {
                    sDec += ch;
                }
                if (ch == ' ')
                {
                    for (int i = 0; i < sDec.Length - 1; i++)
                    {
                        while (tempNumber < digits.Length)
                        {
                            if (sDec[i] == digits[tempNumber])
                            {
                                decPart = decPart * 10 + tempNumber;
                                tempNumber = 0;
                                break;
                            }
                            tempNumber++;
                        }
                    }

                    num = sDec.Length > 0 ? intPart + (decPart / (decimal)(Math.Pow(10, (sDec.Length - 1)))) : intPart;
                    break;
                }
            }
            return num;
        }
     
        public static bool IsDecimal(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return false;

            bool pointFlag = false;
            int i = 0;
            while (i < value.Length)
            {
                if(i == 0 && !char.IsDigit(value[i]))
                {
                    return false;
                }
                if (char.IsDigit(value[i]))
                {
                    i++;
                    continue;
                }
                else if (value[i] == '.' || value[i] == ',')
                {
                    if (pointFlag == true)
                        return false;
                    pointFlag = true;
                }
                else
                {
                    return false;
                }
                i++;
            }

            return true;
        }

        public static string TranslateCategory(string category)
        {
            switch (category)
            {
                case "Мобильные телефоны": return "MobilePhones";
                case "Чехлы для мобильных телефонов": return "MobilePhoneCases";
            }
            return category;
        }

        public static string DecodeUrl(string url)
        {
            string newUrl;
            while ((newUrl = HttpUtility.UrlDecode(url)) != url)
                url = newUrl;
            return newUrl;
        }
    }
}