using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo.Constants.Enum
{
    public static class EnumExtension
    {
        #region 属性

        [AttributeUsage(AttributeTargets.Enum)]
        public sealed class KomokuNameAttribute : Attribute
        {
            public KomokuNameAttribute(string name)
            {
                KomokuName = name;
            }

            public string KomokuName;
        }

        /// <summary>
        /// コード値に関する属性
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class CodeAttribute : Attribute
        {
            public CodeAttribute(string code)
            {
                Code = code;
            }

            public string Code = "";
        }

        /// <summary>
        /// Description値に関する属性
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class DescriptionEnAttribute : Attribute
        {
            public DescriptionEnAttribute(string descriptionEn)
            {
                DescriptionEn = descriptionEn;
            }

            public string DescriptionEn = "";
        }

        /// <summary>
        /// リソースに関する属性
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class ResourceAttribute : Attribute
        {
            public ResourceAttribute(string resource)
            {
                Resource = resource;
            }

            public string Resource = "";
        }

        /// <summary>
        /// デート値に関する属性
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class DateValueAttribute : Attribute
        {
            public DateValueAttribute(int dateValue)
            {
                DateValue = dateValue;
            }

            public int DateValue;
        }

        /// <summary>
        /// メッセージ
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class MessageAttribute : Attribute
        {
            public MessageAttribute(string message)
            {
                Message = message;
            }

            public string Message;
        }

        /// <summary>
        /// タイトル
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class CaptionAttribute : Attribute
        {
            public CaptionAttribute(string caption)
            {
                Caption = caption;
            }

            public string Caption = "確認";
        }

        /// <summary>
        /// タイトル
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class MessageBoxTitle : Attribute
        {
            public MessageBoxTitle(string title)
            {
                Title = title;
            }

            public string Title = "";
        }


        /// <summary>
        /// タイトル色 特殊指定
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class MessageBoxTitleColorAttribute : Attribute
        {
            public MessageBoxTitleColorAttribute(string titleColor)
            {
                TitleColor = titleColor;
            }

            public string TitleColor;
        }

        /// <summary>
        /// 付加文字 Yes
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class MessageBoxYesDescriptionAttribute : Attribute
        {
            public MessageBoxYesDescriptionAttribute(string yesDescription)
            {
                YesDescription = yesDescription;
            }

            public string YesDescription;
        }

        /// <summary>
        /// 付加文字 No
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class MessageBoxNoDescriptionAttribute : Attribute
        {
            public MessageBoxNoDescriptionAttribute(string noDescription)
            {
                NoDescription = noDescription;
            }

            public string NoDescription;
        }

        /// <summary>
        /// 付加文字 Cancel
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class MessageBoxCancelDescriptionAttribute : Attribute
        {
            public MessageBoxCancelDescriptionAttribute(string cancelDescription)
            {
                CancelDescription = cancelDescription;
            }

            public string CancelDescription;
        }

        /// <summary>
        /// 切替表示文字 Cancel
        /// 20221223 #70239 caixiaopeng add
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class MessageBoxCancelBtnNewTextAttribute : Attribute
        {
            public MessageBoxCancelBtnNewTextAttribute(string cancelBtnNewText)
            {
                CancelBtnNewText = cancelBtnNewText;
            }

            public string CancelBtnNewText;
        }

        /// <summary>
        /// プリンタ設定タイプ
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class PrintDisPlayTypeAttribute : Attribute
        {
            public PrintDisPlayTypeAttribute(string printDisPlayType)
            {
                PrintDisPlayType = printDisPlayType;
            }

            public string PrintDisPlayType;
        }


        /// <summary>
        /// プリンタカテゴリタイトル
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class PrintCategoryTitleAttribute : Attribute
        {
            public PrintCategoryTitleAttribute(bool printCategoryTitle)
            {
                PrintCategoryTitle = printCategoryTitle;
            }

            public bool PrintCategoryTitle;
        }


        /// <summary>
        /// ガイド文言
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class GuideTextAttribute : Attribute
        {
            public GuideTextAttribute(string guideText)
            {
                GuideText = guideText;
            }

            public string GuideText;
        }


        /// <summary>
        /// カルテ列
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class IsKarteAttribute : Attribute
        {
            public IsKarteAttribute(bool isKarte)
            {
                IsKarte = isKarte;
            }

            public bool IsKarte;
        }


        /// <summary>
        /// 医事列
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public sealed class IsIjiAttribute : Attribute
        {
            public IsIjiAttribute(bool isIji)
            {
                IsIji = isIji;
            }

            public bool IsIji;
        }

        // 20230613 #78168 guobin add start
        /// <summary>
        ///  システムカラー設定
        /// </summary>
        public static string SystemColorSetting { get; set; } = string.Empty;
        // 20230613 #78168 guobin add end

        #endregion

        #region 拡張メソッド

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetKomokuName(this System.Enum value)
        {
            var typeInfo = value.GetType();
            //指定した属性のリスト
            var attributes = typeInfo.GetCustomAttributes(typeof(KomokuNameAttribute), false)
                .Cast<KomokuNameAttribute>();
            //属性がなかった場合、空を返す
            var komokuNameAttributes = attributes.ToArray();
            if ((komokuNameAttributes?.Count() ?? 0) <= 0)
                return string.Empty;
            //同じ属性が複数含まれていても、最初のみ返す
            return komokuNameAttributes.FirstOrDefault()?.KomokuName ?? value.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum value)
        {
            if (value == null) return string.Empty;
            return value.GetAttribute<DescriptionAttribute>()?.Description
               //属性が未定義だったら列挙値の文字列を返す
               ?? value.ToString();
        }

        /// <summary>
        /// Get Dscription of given enum value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="setDefaultValue"></param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum value, Func<string> setDefaultValue)
        {
            if (value == null) return setDefaultValue?.Invoke() ?? string.Empty;
            return value.GetAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriptionEn(this System.Enum value)
        {
            if (value == null) return string.Empty;
            return value.GetAttribute<DescriptionEnAttribute>()?.DescriptionEn
               //属性が未定義だったら列挙値の文字列を返す
               ?? value.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCode(this System.Enum value)
        {
            if (value == null) return string.Empty;
            return value.GetAttribute<CodeAttribute>()?.Code
               //属性が未定義だったら列挙値の文字列を返す
               ?? value.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetResource(this System.Enum value)
        {
            if (value == null) return string.Empty;
            return value.GetAttribute<ResourceAttribute>()?.Resource
                   //属性が未定義だったら列挙値の文字列を返す
                   ?? value.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMessage(this System.Enum value)
        {
            if (value == null) return string.Empty;
            return value.GetAttribute<MessageAttribute>()?.Message
                   //属性が未定義だったら列挙値の文字列を返す
                   ?? value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMessage(this System.Enum value, params object[] args)
        {
            return string.Format(GetMessage(value), args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetTitle(this System.Enum value)
        {
            return value.GetAttribute<MessageBoxTitle>()?.Title
                   //属性が未定義だったら列挙値の文字列を返す
                   ?? string.Empty;
        }

        /// <summary>
        /// タイトル色指定
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMessageBoxTitleColor(this System.Enum value)
        {
            return value.GetAttribute<MessageBoxTitleColorAttribute>()?.TitleColor;
        }

        /// <summary>
        /// 付加文字 Yes
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetYesDescription(this System.Enum value)
        {
            return value.GetAttribute<MessageBoxYesDescriptionAttribute>()?.YesDescription;
        }

        /// <summary>
        /// 付加文字 No
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetNoDescription(this System.Enum value)
        {
            return value.GetAttribute<MessageBoxNoDescriptionAttribute>()?.NoDescription;
        }

        /// <summary>
        /// 付加文字 Cancel
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCancelDescription(this System.Enum value)
        {
            return value.GetAttribute<MessageBoxCancelDescriptionAttribute>()?.CancelDescription;
        }


        /// <summary>
        /// 切替表示文字 Cancel
        /// 20221223 #70239 caixiaopeng add
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCancelBtnNewText(this System.Enum value)
        {
            return value.GetAttribute<MessageBoxCancelBtnNewTextAttribute>()?.CancelBtnNewText;
        }

        /// <summary>
        /// プリンタ設定タイプ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetPrintDisPlayType(this System.Enum value)
        {
            return value.GetAttribute<PrintDisPlayTypeAttribute>()?.PrintDisPlayType;
        }


        /// <summary>
        /// プリンタ設定タイプ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetPrintCategoryTitle(this System.Enum value)
        {
            return value.GetAttribute<PrintCategoryTitleAttribute>()?.PrintCategoryTitle == true;
        }

        /// <summary>
        /// ガイド文言
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetGuideText(this System.Enum value)
        {
            return value.GetAttribute<GuideTextAttribute>()?.GuideText;
        }



        /// <summary>
        /// カルテ列
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetIskarte(this System.Enum value)
        {
            return value.GetAttribute<IsKarteAttribute>()?.IsKarte == true;
        }


        /// <summary>
        /// 医事列
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetIsIji(this System.Enum value)
        {
            return value.GetAttribute<IsIjiAttribute>()?.IsIji == true;
        }



        /// <summary>
        /// Enumの次の値を取得
        /// 前提：enumの値は1づつ増加
        /// (Flagsの場合等は正しく取得できない)
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="firstValue">The first value.</param>
        /// <returns></returns>
        public static TEnum GetNextEnum<TEnum>(this TEnum value, int firstValue = 0) where TEnum : struct
        {
            var valueInt = Convert.ToInt32(value);
            var enumCount = System.Enum.GetValues(typeof(TEnum)).Length;
            var lastValue = enumCount - 1 - firstValue;
            var nextValueInt = valueInt == lastValue ? firstValue : valueInt + 1;
            return (TEnum)System.Enum.ToObject(typeof(TEnum), nextValueInt);
        }

        #endregion

        #region privateメソッド

        /// <summary>
        /// 特定の属性を取得する
        /// </summary>
        /// <typeparam name="TAttribute">属性型</typeparam>
        private static TAttribute GetAttribute<TAttribute>(this System.Enum value) where TAttribute : Attribute
        {
            if (value == null) return null;
            //リフレクションを用いて列挙体の型から情報を取得
            var fieldInfo = value.GetType().GetField(value.ToString());
            //指定した属性のリスト
            var attributes
                = fieldInfo.GetCustomAttributes(typeof(TAttribute), false)
                    .Cast<TAttribute>();
            //属性がなかった場合、空を返す
            if ((attributes?.Count() ?? 0) <= 0)
                return null;
            //同じ属性が複数含まれていても、最初のみ返す
            return attributes.First();
        }

        #endregion

        #region publicメソッド

        public static List<ComboBoxSourceItem> EnumToList(Type enumType)
        {
            return EnumToList<ComboBoxSourceItem>(enumType);
        }

        public static List<T> EnumToList<T>(Type enumType) where T : ComboBoxSourceItem, new()
        {
            var result = new List<T>();

            if (enumType.BaseType == typeof(System.Enum))
            {

                foreach (var enumValue in System.Enum.GetValues(enumType))
                {
                    // 値の説明を取得する
                    FieldInfo fi = enumType.GetField(System.Enum.GetName(enumType, enumValue));
                    if (fi != null)
                    {
                        var description =
                            (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                        var content =
                            (DefaultValueAttribute)Attribute.GetCustomAttribute(fi, typeof(DefaultValueAttribute));
                        var code = (CodeAttribute)Attribute.GetCustomAttribute(fi, typeof(CodeAttribute));
                        var descriptionEn =
                            (DescriptionEnAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionEnAttribute));
                        var isKarte =
                            (IsKarteAttribute)Attribute.GetCustomAttribute(fi, typeof(IsKarteAttribute));
                        var isIji =
                            (IsIjiAttribute)Attribute.GetCustomAttribute(fi, typeof(IsIjiAttribute));

                        T obj = new T();
                        obj.Content = content?.Value.ToString() ?? string.Empty;
                        obj.Value = Convert.ToInt32(enumValue);
                        obj.Description = description?.Description ?? string.Empty;
                        obj.DescriptionEn = descriptionEn?.DescriptionEn ?? string.Empty;
                        obj.Code = code?.Code ?? string.Empty;
                        obj.IsKarte = isKarte?.IsKarte ?? false;
                        obj.IsIji = isIji?.IsIji ?? false;

                        result.Add(obj);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// int→enum
        /// 変換できない場合defaultValue
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToEnumOr<T>(this int value, T defaultValue) where T : struct =>
            ToEnumOr(value.ToString(), defaultValue);

        /// <summary>
        /// string→enum
        /// 変換できない場合defaultValue
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToEnumOr<T>(this string value, T defaultValue)
            where T : struct
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return System.Enum.TryParse<T>(value, out var ret) ? ret : defaultValue;
        }

        /// <summary>Gets the values.</summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <returns></returns>
        public static TEnum[] GetValues<TEnum>()
                where TEnum : struct =>
                GetValuesExcept<TEnum>();

        /// <summary>Gets the values.</summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="exceptValues">除外したい項目</param>
        /// <returns></returns>
        public static TEnum[] GetValuesExcept<TEnum>(params TEnum[] exceptValues)
                where TEnum : struct
        {
            var ret = (TEnum[])System.Enum.GetValues(typeof(TEnum));
            return ret.Except(exceptValues).ToArray();
        }

        #endregion
    }
}
