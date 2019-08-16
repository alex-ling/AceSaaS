using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Acesoft.Util
{
    public static class RegexHelper
    {
        const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled;
        
        // 含字母、下划线、数字
        const string REGEX_Variable = "^[a-zA-Z_][a-zA-Z0-9_]*$";
        // 邮件
        const string REGEX_Mail = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        // 含字母、数字、特殊字符
        const string REGEX_Password = @"^(?i).+(?<=\d.*)(?<=[a-z].*)(?<=[^a-z\s\d].*)$";
        // 不含空格、回车、换行符
        const string REGEX_NotEmpty = @"^[\S]*$";

        public static bool IsMatch(string str, string pattern)
        {
            if (str == null) return false;
            return Regex.IsMatch(str, pattern, options);
        }

        public static bool IsMatchVariable(string name)
        {
            return IsMatch(name, REGEX_Variable);
        }

        public static bool IsMatchMail(string mail)
        {
            return IsMatch(mail, REGEX_Mail);
        }

        public static string GetMatchValue(string str, string pattern)
        {
            if (!str.HasValue()) return string.Empty;
            Check.Require(IsMatch(str, pattern), $"字符串未匹配表达式{pattern}");

            return Regex.Match(str, pattern, options).Value;
        }

        public static MatchCollection GetMatchValues(string str, string pattern)
        {
            return Regex.Matches(str, pattern, options);
        }

        public static void Matchs(string str, string pattern, Action<Match> action)
        {
            var matchs = GetMatchValues(str, pattern);
            foreach (Match m in matchs)
            {
                action(m);
            }
        }

        public static string Replace(string str, string pattern, string replace)
        {
            return Regex.Replace(str, pattern, replace, options);
        }

        public static string Replace(string str, string pattern, Func<Match, string> func)
        {
            return Regex.Replace(str, pattern, new MatchEvaluator(func), options);
        }
    }
}
