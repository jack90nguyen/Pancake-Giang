using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Onetez.Core.Libs
{
    public class Arrays
    {
        #region Array Int

        /// <summary>
        /// Convert String thành Array
        /// </summary>
        /// <returns></returns>
        public static List<long> StringToArrayInt(string str)
        {
            var list = new List<long>();
            string[] array = str.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                if (!string.IsNullOrEmpty(array[i]))
                    list.Add(Convert.ToInt32(array[i]));
            }
            return list;
        }

        /// <summary>
        /// Convert Array thành String
        /// </summary>
        /// <returns></returns>
        public static string ArrayToStringInt(List<long> list)
        {
            string str = list.Count > 0 ? "|" : string.Empty;
            foreach (int i in list)
            {
                str += i + "|";
            }
            return str;
        }

        /// <summary>
        /// Delete phần tử Array trong String
        /// </summary>
        /// <returns></returns>
        public static string DeleteArrayInt(string str, long value)
        {
            List<long> list = StringToArrayInt(str);
            list.Remove(value);
            return ArrayToStringInt(list);
        }

        /// <summary>
        /// Add phần tử Array vào String
        /// </summary>
        /// <returns></returns>
        public static string AddArrayInt(string str, long value)
        {
            List<long> list = StringToArrayInt(str);
            if (!list.Contains(value))
                list.Add(value);
            return ArrayToStringInt(list);
        }

        #endregion

        #region Array String

        /// <summary>
        /// Convert String thành Array
        /// </summary>
        /// <returns></returns>
        public static List<string> StringToArrayString(string str)
        {
            List<string> list = new List<string>();
            string[] array = str.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                if (!string.IsNullOrEmpty(array[i]))
                    list.Add(array[i]);
            }
            return list;
        }

        /// <summary>
        /// Convert String thành Array, bao gồm rỗng
        /// </summary>
        /// <returns></returns>
        public static List<string> StringToArrayStringAll(string str)
        {
            List<string> list = new List<string>();
            string[] array = str.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                if (!string.IsNullOrEmpty(array[i]))
                    list.Add(array[i]);
            }
            return list;
        }

        /// <summary>
        /// Convert Array thành String
        /// </summary>
        /// <returns></returns>
        public static string ArrayToStringString(List<string> list)
        {
            string str = list.Count > 0 ? "|" : string.Empty;
            foreach (string i in list)
            {
                str += i + "|";
            }
            return str;
        }

        /// <summary>
        /// Delete phần tử Array trong String
        /// </summary>
        /// <returns></returns>
        public static string DeleteArrayString(string str, string value)
        {
            List<string> list = StringToArrayString(str);
            if(!string.IsNullOrEmpty(value))
                list.Remove(value);
            return ArrayToStringString(list);
        }

        /// <summary>
        /// Add phần tử Array vào String
        /// </summary>
        /// <returns></returns>
        public static string AddArrayString(string str, string value)
        {
            List<string> list = StringToArrayString(str);
            if (!list.Contains(value))
                list.Add(value);
            return ArrayToStringString(list);
        }

        #endregion
    }
}
