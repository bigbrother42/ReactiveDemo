using System;
using System.ComponentModel;
using System.Linq;

namespace BaseDemo.Data.Constants
{
    public static class EnumExtension
    {
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class CodeAttribute : Attribute
        {
            public CodeAttribute(string code)
            {
                Code = code;
            }

            public string Code = "";
        }

        public static string GetCode(this System.Enum value)
        {
            return value.GetAttribute<CodeAttribute>()?.Code
               ?? value.ToString();
        }

        public static string GetDescription(this System.Enum value)
        {
            return value.GetAttribute<DescriptionAttribute>()?.Description
               ?? value.ToString();
        }

        private static TAttribute GetAttribute<TAttribute>(this System.Enum value) where TAttribute : Attribute
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = fieldInfo.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>();

            if ((attributes?.Count() ?? 0) <= 0)
                return null;

            return attributes.First();
        }

        public static System.Enum ConvertCodeToEnum<T>(string value)
        {
            foreach (System.Enum item in System.Enum.GetValues(typeof(T)))
            {
                string code = item.GetCode();

                if (code == value)
                {
                    return item;
                }
            }

            return null;
        }

        public static System.Enum ConvertNameToEnum<T>(string value)
        {
            foreach (System.Enum item in System.Enum.GetValues(typeof(T)))
            {
                string code = item.ToString();

                if (code == value)
                {
                    return item;
                }
            }

            return null;
        }

        public static System.Enum ConvertDescriptionToEnum<T>(string value)
        {
            foreach (System.Enum item in System.Enum.GetValues(typeof(T)))
            {
                string code = item.GetDescription();

                if (code == value)
                {
                    return item;
                }
            }

            return null;
        }

        public static System.Enum ConvertValueToEnum<T>(int value)
        {
            foreach (System.Enum item in System.Enum.GetValues(typeof(T)))
            {
                if (item.GetHashCode() == value)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
