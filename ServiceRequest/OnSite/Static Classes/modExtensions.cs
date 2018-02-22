using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Idox.LGDP.Apps.Common.OnSite
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name			modExtensions
    /// 
    /// <summary>		All common extensions for Idox On-Site project components.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public static class modExtensions
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Static Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string[] Alphabet;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Static Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        static modExtensions()
        {
            Alphabet = new string[26] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetDescription
        /// 
        /// <summary>	Gets the textual description for an OnSiteEnvironment.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string GetDescription(this OnSiteEnvironments environment)
        {
            switch (environment)
            {
                case OnSiteEnvironments.Production:
                    return "Live";
                case OnSiteEnvironments.Staging:
                    return "Test";
                case OnSiteEnvironments.Sales:
                    return "Demo";
                default:
                    return environment.ToString();
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		LongishDateTimeFormat
        /// 
        /// <summary>	Returns an app friendly string representation of a DateTime?
        /// </summary>
        /// <param name="dateTime">			The DateTime nullable value.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string LongishDateTimeFormat(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
                return dateTime.Value.ToString("ddd dd MMM HH:mm");
            else
                return "";
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ToAlphaIndex
        /// 
        /// <summary>	Converts a zero index integer to an alphabetic index.
        /// </summary>
        /// <param name="index">		The index to convert.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string ToAlphaIndex(this int index)
        {
            string alphaIndex;
            //
            if (index >= Alphabet.Length)
            {
                var f = (int)Math.Floor((double)index / Alphabet.Length);
                var r = index - (Alphabet.Length * f);
                alphaIndex = string.Format("{0}{1}",
                                           Alphabet[f - 1],
                                           Alphabet[r]);
            }
            else if (index >= 0)
                alphaIndex = Alphabet[index];
            else
                alphaIndex = "?";
            //
            return alphaIndex;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ToBase64Encode
        /// 
        /// <summary>	Encodes the supplied text to Base64Url encoding.
        /// </summary>
        /// <param name="toEncode">		The text to encode.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string ToBase64Url(this string toEncode)
        {
            string encoded;
            //
            encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(toEncode.ToUpper()))
                             .TrimEnd('=')
                             .Replace('+', '-')
                             .Replace('/', '_');
            //
            return encoded;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ToBase64Url
        /// 
        /// <summary>	Joins all the fields but first encodes them to Base64Url and uses the seperator
        /// 			string to join.
        /// </summary>
        /// <param name="fieldsToEncode">	The fields to encode and join together.</param>
        /// <param name="seperator">		The string to use in the Join.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string ToBase64Url(this string[] fieldsToEncode, string seperator)
        {
            for (int i = 0; i < fieldsToEncode.Length; i++)
                fieldsToEncode[i] = fieldsToEncode[i].ToBase64Url();
            //
            return string.Join(seperator, fieldsToEncode);
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ToByteSizeString
        /// 
        /// <summary>	Converts number of bytes to an appropriate binary string representation.
        /// </summary>
        /// <param name="bytes">		The number of bytes to represent.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string ToByteSizeString(this int bytes)
        {
            string size;
            //
            // Don't measure less than a kilobyte (kibi).
            if (bytes < 1024)
                size = "1 KB";
            else if (bytes < 1048576)
                size = string.Format("{0} KB", bytes / 1024);
            else
                size = string.Format("{0} MB", Math.Round((double)bytes / 1048576, 1));
            //
            return size;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ToDateString
        /// 
        /// <summary>	Returns the date value of the param name="dateTime" as string.
        /// </summary>
        /// <param name="dateTime">		The DateTime value.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string ToDateString(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
                return dateTime.Value.ToString("dd MMM yyyy");
            else
                return "No date";
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ToDueDateString
        /// 
        /// <summary>	Returns an app friendly string representation of a due date or scheduled date.
        /// </summary>
        /// <param name="dateTime">			The DateTime nullable value.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string ToDueDateString(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
                return string.Format("Due {0}", dateTime.LongishDateTimeFormat());
            else
                return "No date";
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ToMilesText
        /// 
        /// <summary>	Converts the value to a miles string.
        /// </summary>
        /// <param name="miles">		The miles value.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string ToMilesText(this double miles)
        {
            if (miles == 0)
                return "No miles";
            else
                return string.Format("{0} miles", miles);
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ToMilesText
        /// 
        /// <summary>	Converts the value to a miles string.
        /// </summary>
        /// <param name="miles">		The miles value.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string ToMilesText(this float miles)
        {
            return ToMilesText((double)miles);
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ToString
        /// 
        /// <summary>	Converts a nullable DateTime value to string. If the DateTime has a value then the
        /// 			prefix will be used, otherwise only the noValue parameter will be returned.
        /// </summary>
        /// <param name="dateTime">		The DateTime? value.</param>
        /// <param name="format">		The date format to use.</param>
        /// <param name="valuePrefix">	The prefix to use when there is a value.</param>
        /// <param name="noValue">		The text to use if there is no value.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string ToString(this DateTime? dateTime, string format, string valuePrefix, string noValue)
        {
            if (dateTime.HasValue)
                return string.Format("{0}{1}", valuePrefix, dateTime.Value.ToString(format));
            else
                return noValue;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ToTimeString
        /// 
        /// <summary>	Converts the float value to hours and minutes where each integer value represents
        /// 			a single hour.
        /// </summary>
        /// <param name="value">		The value to convert.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string ToTimeString(this float? value)
        {
            if (value.HasValue)
                return value.Value.ToTimeString();
            else
                return "None";
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ToTimeString
        /// 
        /// <summary>	Converts the float value to hours and minutes where each integer value represents
        /// 			a single hour.
        /// </summary>
        /// <param name="value">		The value to convert.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string ToTimeString(this float value)
        {
            StringBuilder oTime;
            int nHours;
            int nMin;
            //
            // Make sure there is a value to calculate.
            if (value > 0)
            {
                // Calculate the hours and minutes.
                nHours = (int)Math.Floor(value);
                nMin = (int)((value % 1) * 60);
                //
                oTime = new StringBuilder();
                if (nHours > 0)
                    oTime.Append(string.Format("{0} hour{1}", nHours, nHours > 1 ? "s" : ""));
                if (nMin > 0)
                    oTime.Append(string.Format(" {0} minutes", nMin));
            }
            else
                oTime = new StringBuilder("None");
            //
            return oTime.ToString();
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ToUrlEncoded
        /// 
        /// <summary>	Url encodes the text.
        /// </summary>
        /// <param name="text">			The text to encode.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string ToUrlEncoded(this string text)
        {
            string encoded;
            //
            encoded = System.Net.WebUtility.UrlEncode(text);
            return encoded;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RTF2PlainText
        /// 
        /// <summary>	
        /// 			Attempts a very naive RTF to plain text convserion. Microsoft's recommended
        /// 			way is to use a RichTextBox, but that doesn't seem to be available. I think it should
        /// 			be possible using NSTextStorage and NSDocumentType.RTF, but this left residule RTF
        /// 			special characters. I don't know if this is because the RTF in Uniform is non-standard
        /// 			or what. Another option might be NSDocument, but that doesn't seem available in iOS.
        /// 			As implied above this may need to become an OS specific bit of code, but it also needs
        /// 			to be accessable from the CPi Models.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static string ToRTF2PlainText(this string rtf)
        {
            try
            {
                if (rtf == null) return null;
                var res = Regex.Replace(rtf, @"\{\*?\\[^{}]+;}|[{}]|\\[A-Za-z]+\n?(?:-?\d+)?[ ]?", " ", RegexOptions.None, TimeSpan.FromSeconds(1));
                return Regex.Replace(res, @"\s+", " ", RegexOptions.None, TimeSpan.FromSeconds(1)).Trim();
            }
            catch
            {
                return rtf;
            }
        }
        /// 
        public static float NextFloat(this Random rand)
        {
            return (float)rand.NextDouble();
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Override
        /// 
        /// <summary>	Overrides the current status with the new one if it has a higher value.
        /// </summary>
        /// <param name="s">		The current status.</param>
        /// <param name="status">	The new status.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static SyncStatus Override(this SyncStatus s, SyncStatus status)
        {
            if ((int)status > (int)s)
                s = status;
            //
            return s;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}

