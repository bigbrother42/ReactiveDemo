namespace ReactiveDemo.Util
{
    public static class ConvertUtil
    {
        public static int ToInt(this string strVal)
        {
            if (string.IsNullOrEmpty(strVal)) return 0;

            var isInt = int.TryParse(strVal.Trim(), out var result);

            return isInt ? result : 0;
        }
    }
}