using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Util
{
    public static class ChsHelper
    {
        public static string GetChsNum(char c)
        {
            switch (c)
            {
                case '0': return "〇";
                case '1': return "一";
                case '2': return "二";
                case '3': return "三";
                case '4': return "四";
                case '5': return "五";
                case '6': return "六";
                case '7': return "七";
                case '8': return "八";
                case '9': return "九";
            }
            throw new Exception("非正常的数字转换！");
        }

        private static string GetScalar(int pos)
        {
            switch (pos)
            {
                case 0: return "";
                case 1: return "十";
                case 2: return "百";
                case 3: return "千";
                case 4: return "万";
                case 5: return "十";
                case 6: return "百";
                case 7: return "千";
                case 8: return "亿";
            }
            throw new AceException("数字太大，请输入亿以内的数字");
        }

        public static string GetChs(string mzs)
        {
            int length = mzs.Length;
            if (length > 8)
            {
                throw new AceException("数字太大，请输入亿以内的数字");
            }

            var builder = new StringBuilder();
            for (var i = length - 1; i >= 0; i--)
            {
                builder.Append(GetChsNum(mzs[(length - i) - 1]));
                builder.Append(GetScalar(i));
            }
            return builder.ToString();
        }

        public static string GetChsMoney(decimal count)
        {
            if (count > 9999999999999999.99M)
            {
                throw new AceException("数字太大，请输入千万亿元以内的金额");
            }
            if (count < 0M)
            {
                throw new AceException("货币只能是正的数值");
            }

            string mzs = Convert.ToString(count);
            string[] strArray = mzs.Split(new char[] { '.' });
            if (strArray.Length > 2)
            {
                throw new AceException("表示货币金额的浮点数格式错误！");
            }
            if (strArray.Length == 1)
            {
                return (ConvertZhengShu(mzs) + "整");
            }
            return (ConvertZhengShu(strArray[0]) + ConvertXiaoShu(strArray[1]));
        }

        public static string ConvertToChinese(string count)
        {
            if ((count == null) || (string.Empty == count))
            {
                return null;
            }
            if (!count.IsFloat())
            {
                throw new Exception("货币只能是正的数字！");
            }
            return GetChsMoney(decimal.Parse(count));
        }

        private static string GetScalarMoney(int pos)
        {
            switch (pos)
            {
                case 0: return "圆";
                case 1: return "拾";
                case 2: return "佰";
                case 3: return "仟";
                case 4: return "萬";
                case 5: return "拾";
                case 6: return "佰";
                case 7: return "仟";
                case 8: return "億";
                case 9: return "拾";
                case 10: return "佰";
                case 11: return "仟";
                case 12: return "萬";
                case 13: return "拾";
                case 14: return "佰";
                case 15: return "仟";
            }
            throw new AceException("数字太大，请输入千万亿元以内的金额");
        }

        private static string ConvertXiaoShu(string mxs)
        {
            if (mxs.Length == 1)
            {
                return (GetChsMoney(mxs[0]) + "角" + GetChsMoney((byte)0) + "分");
            }
            return (GetChsMoney(mxs[0]) + "角" + GetChsMoney(mxs[1]) + "分");
        }

        private static string ConvertZhengShu(string mzs)
        {
            int length = mzs.Length;
            if (length > 0x10)
            {
                throw new AceException("数字太大，请输入千万亿元以内的金额");
            }
            StringBuilder builder = new StringBuilder(0x80);
            for (int i = length - 1; i >= 0; i--)
            {
                builder.Append(GetChsMoney(mzs[(length - i) - 1]));
                builder.Append(GetScalarMoney(i));
            }
            return builder.ToString();
        }

        public static string GetChsMoney(char c)
        {
            switch (c)
            {
                case '0': return "零";
                case '1': return "壹";
                case '2': return "贰";
                case '3': return "叁";
                case '4': return "肆";
                case '5': return "伍";
                case '6': return "陆";
                case '7': return "柒";
                case '8': return "捌";
                case '9': return "玖";
            }
            throw new Exception("非正常的数字转换！");
        }
    }
}
