using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GameZone.TOOLS
{
    public class RegexUtilities
    {
        bool invalid = false;
        /// <summary>
        /// Validate Email Address format
        /// </summary>
        /// <param name="emailToVerify"></param>
        /// <returns></returns>
        public bool IsValidEmail(string emailToVerify)
        {
            invalid = false;
            if (String.IsNullOrEmpty(emailToVerify))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                emailToVerify = Regex.Replace(emailToVerify, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid email format.
            try
            {
                return Regex.IsMatch(emailToVerify,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
        /// <summary>
        /// Check if string contains upper or lower case alphabet
        /// </summary>
        /// <param name="stringToCheck"></param>
        /// <returns></returns>
        public bool ContainsAlphabet(string stringToCheck)
        {
            return Regex.IsMatch(stringToCheck, "[a-zA-Z]");
        }
        
    }
}
