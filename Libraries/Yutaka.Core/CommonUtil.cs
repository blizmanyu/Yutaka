using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace Yutaka.Core
{
	/// <summary>
	/// Represents a common util
	/// </summary>
	public partial class CommonUtil
	{
		private static readonly Regex RegexWhitespace = new Regex(@"\s+", RegexOptions.Compiled);

		/// <summary>
		/// Indicates whether the specified strings are null or empty strings
		/// </summary>
		/// <param name="stringsToValidate">Array of strings to validate</param>
		/// <returns>Boolean</returns>
		public static bool AreNullOrEmpty(params string[] stringsToValidate)
		{
			return stringsToValidate.Any(p => string.IsNullOrEmpty(p));
		}

		/// <summary>
		/// Compare two arrasy
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="a1">Array 1</param>
		/// <param name="a2">Array 2</param>
		/// <returns>Result</returns>
		public static bool ArraysEqual<T>(T[] a1, T[] a2)
		{
			//also see Enumerable.SequenceEqual(a1, a2);
			if (ReferenceEquals(a1, a2))
				return true;

			if (a1 == null || a2 == null)
				return false;

			if (a1.Length != a2.Length)
				return false;

			var comparer = EqualityComparer<T>.Default;

			for (int i = 0; i < a1.Length; i++) {
				if (!comparer.Equals(a1[i], a2[i]))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Convert enum for front-end
		/// </summary>
		/// <param name="str">Input string</param>
		/// <returns>Converted string</returns>
		public static string ConvertEnum(string str)
		{
			if (string.IsNullOrEmpty(str))
				return string.Empty;

			var result = string.Empty;

			foreach (var c in str) {
				if (c.ToString() != c.ToString().ToLower())
					result += " " + c.ToString();
				else
					result += c.ToString();
			}

			//ensure no spaces (e.g. when the first letter is upper case)
			result = result.TrimStart();
			return result;
		}

		/// <summary>
		/// Ensure that a string doesn't exceed maximum allowed length
		/// </summary>
		/// <param name="str">Input string</param>
		/// <param name="maxLength">Maximum length</param>
		/// <param name="postfix">A string to add to the end if the original string was shorten</param>
		/// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
		public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
		{
			if (String.IsNullOrEmpty(str))
				return str;

			if (str.Length > maxLength) {
				var pLen = postfix == null ? 0 : postfix.Length;
				var result = str.Substring(0, maxLength - pLen);

				if (!String.IsNullOrEmpty(postfix))
					result += postfix;

				return result;
			}

			return str;
		}

		/// <summary>
		/// Ensure that a string is not null
		/// </summary>
		/// <param name="str">Input string</param>
		/// <returns>Result</returns>
		public static string EnsureNotNull(string str)
		{
			return str ?? string.Empty;
		}

		/// <summary>
		/// Ensures that a string only contains numeric values
		/// </summary>
		/// <param name="str">Input string</param>
		/// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
		public static string EnsureNumericOnly(string str)
		{
			return string.IsNullOrEmpty(str) ? string.Empty : new string(str.Where(p => char.IsDigit(p)).ToArray());
		}

		/// <summary>
		/// Ensures the subscriber email or throw.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <returns></returns>
		public static string EnsureSubscriberEmailOrThrow(string email)
		{
			var output = EnsureNotNull(email);
			output = output.Trim();
			output = EnsureMaximumLength(output, 255);

			if (!IsValidEmail(output))
				throw new YuException("Email is not valid.");

			return output;
		}

		/// <summary>
		/// Generate random digit code
		/// </summary>
		/// <param name="length">Length</param>
		/// <returns>Result string</returns>
		public static string GenerateRandomDigitCode(int length)
		{
			var random = new Random();
			var str = string.Empty;
			for (int i = 0; i < length; i++)
				str = String.Concat(str, random.Next(10).ToString());
			return str;
		}

		/// <summary>
		/// Returns an random interger number within a specified rage
		/// </summary>
		/// <param name="min">Minimum number</param>
		/// <param name="max">Maximum number</param>
		/// <returns>Result</returns>
		public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
		{
			var randomNumberBuffer = new byte[10];
			new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
			return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
		}

		/// <summary>
		/// Gets a person's current age.
		/// </summary>
		/// <param name="birthdate">The person's date of birth.</param>
		/// <returns></returns>
		public static int GetCurrentAge(DateTime birthdate)
		{
			return GetDifferenceInYears(birthdate.Date, DateTime.Today);
		}

		/// <summary>
		/// Get difference in years
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
		{
			//source: https://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
			//this assumes you are looking for the western idea of age and not using East Asian reckoning.
			var age = endDate.Year - startDate.Year;

			if (startDate > endDate.AddYears(-age))
				--age;

			return age;
		}

		private static AspNetHostingPermissionLevel? _trustLevel;
		/// <summary>
		/// Finds the trust level of the running application (https://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
		/// </summary>
		/// <returns>The current trust level.</returns>
		public static AspNetHostingPermissionLevel GetTrustLevel()
		{
			if (!_trustLevel.HasValue) {
				//set minimum
				_trustLevel = AspNetHostingPermissionLevel.None;

				//determine maximum
				foreach (AspNetHostingPermissionLevel trustLevel in new[] {
								AspNetHostingPermissionLevel.Unrestricted,
								AspNetHostingPermissionLevel.High,
								AspNetHostingPermissionLevel.Medium,
								AspNetHostingPermissionLevel.Low,
								AspNetHostingPermissionLevel.Minimal
							}) {
					try {
						new AspNetHostingPermission(trustLevel).Demand();
						_trustLevel = trustLevel;
						break; //we've set the highest permission we can
					}
					catch (System.Security.SecurityException) {
						continue;
					}
				}
			}
			return _trustLevel.Value;
		}

		/// <summary>
		/// Verifies that a string is in valid e-mail format
		/// </summary>
		/// <param name="email">Email to verify</param>
		/// <returns>true if the string is a valid e-mail address and false if it's not</returns>
		public static bool IsValidEmail(string email)
		{
			if (String.IsNullOrEmpty(email))
				return false;

			email = email.Trim();
			var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
			return result;
		}

		/// <summary>
		/// Verifies that string is an valid IP-Address
		/// </summary>
		/// <param name="ipAddress">IPAddress to verify</param>
		/// <returns>true if the string is a valid IpAddress and false if it's not</returns>
		public static bool IsValidIpAddress(string ipAddress)
		{
			return IPAddress.TryParse(ipAddress, out var ip);
		}

		/// <summary>
		/// Maps a virtual path to a physical disk path.
		/// </summary>
		/// <param name="path">The path to map. E.g. "~/bin"</param>
		/// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
		public static string MapPath(string path)
		{
			if (HostingEnvironment.IsHosted) {
				//hosted
				return HostingEnvironment.MapPath(path);
			}

			//not hosted. For example, run in unit tests
			var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
			return Path.Combine(baseDirectory, path);
		}

		/// <summary>
		/// Masks a credit card number except for the first 4 and last 4 digits. If a card is less than 12 digits, its invalid, so just return.
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static string MaskCreditCardNumber(string card)
		{
			if (String.IsNullOrWhiteSpace(card))
				return "";

			card = RemoveAllWhitespaces(card);
			var cardLength = card.Length;

			if (cardLength < 12)
				return card;

			return String.Format("{0} **** {1}", card.Substring(0, 4), card.Substring(cardLength - 4));
		}

		/// <summary>
		/// Reduces excessive amounts of whitespaces.
		/// </summary>
		/// <param name="input">The input string to reduce.</param>
		/// <returns></returns>
		public static string ReduceExcessiveWhitespace(string input)
		{
			if (String.IsNullOrWhiteSpace(input))
				return "";

			input = input.Trim();

			while (input.Contains("\r\n\r\n\r\n"))
				input = input.Replace("\r\n\r\n\r\n", "\r\n\r\n");

			while (input.Contains("\r\r\r"))
				input = input.Replace("\r\r\r", "\r\r");

			while (input.Contains("\n\n\n"))
				input = input.Replace("\n\n\n", "\n\n");

			while (input.Contains("\t\t"))
				input = input.Replace("\t\t", "\t");

			while (input.Contains("  "))
				input = input.Replace("  ", " ");

			return input.Trim();
		}

		/// <summary>
		/// Removes all whitespaces.
		/// </summary>
		/// <param name="input">The input string to remove whitespaces from.</param>
		/// <returns></returns>
		public static string RemoveAllWhitespaces(string input)
		{
			return RegexWhitespace.Replace(input, "");
		}

		/// <summary>
		/// Replaces all whitespace characters with the specified replacement.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <param name="replacement">The replacement string.</param>
		/// <returns></returns>
		public static string ReplaceWhitespace(string input, string replacement)
		{
			return RegexWhitespace.Replace(input, replacement);
		}

		/// <summary>
		/// Replaces all whitespaces with a single space.
		/// </summary>
		/// <param name="input">The input string to strip.</param>
		/// <returns></returns>
		public static string ReplaceWhitespaceWithSpace(string input)
		{
			if (String.IsNullOrWhiteSpace(input))
				return "";

			input = input.Trim().Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");

			while (input.Contains("  "))
				input = input.Replace("  ", " ");

			return input.Trim();
		}

		/// <summary>
		/// Sets a property on an object to a valuae.
		/// </summary>
		/// <param name="instance">The object whose property to set.</param>
		/// <param name="propertyName">The name of the property to set.</param>
		/// <param name="value">The value to set the property to.</param>
		public static void SetProperty(object instance, string propertyName, object value)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");
			if (propertyName == null)
				throw new ArgumentNullException("propertyName");

			var instanceType = instance.GetType();
			var pi = instanceType.GetProperty(propertyName);
			if (pi == null)
				throw new YuException("No property '{0}' found on the instance of type '{1}'.", propertyName, instanceType);
			if (!pi.CanWrite)
				throw new YuException("The property '{0}' on the instance of type '{1}' does not have a setter.", propertyName, instanceType);
			if (value != null && !value.GetType().IsAssignableFrom(pi.PropertyType))
				value = To(value, pi.PropertyType);
			pi.SetValue(instance, value, new object[0]);
		}

		/// <summary>
		/// Set Telerik (Kendo UI) culture
		/// </summary>
		public static void SetTelerikCulture()
		{
			//little hack here
			//always set culture to 'en-US' (Kendo UI has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs

			var culture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
		}

		/// <summary>
		/// Converts a value to a destination type.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="destinationType">The type to convert the value to.</param>
		/// <returns>The converted value.</returns>
		public static object To(object value, Type destinationType)
		{
			return To(value, destinationType, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Converts a value to a destination type.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="destinationType">The type to convert the value to.</param>
		/// <param name="culture">Culture</param>
		/// <returns>The converted value.</returns>
		public static object To(object value, Type destinationType, CultureInfo culture)
		{
			if (value != null) {
				var sourceType = value.GetType();
				var destinationConverter = TypeDescriptor.GetConverter(destinationType);

				if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
					return destinationConverter.ConvertFrom(null, culture, value);

				var sourceConverter = TypeDescriptor.GetConverter(sourceType);

				if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
					return sourceConverter.ConvertTo(null, culture, value, destinationType);

				if (destinationType.IsEnum && value is int)
					return Enum.ToObject(destinationType, (int) value);

				if (!destinationType.IsInstanceOfType(value))
					return Convert.ChangeType(value, destinationType, culture);
			}
			return value;
		}

		/// <summary>
		/// Converts a value to a destination type.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <typeparam name="T">The type to convert the value to.</typeparam>
		/// <returns>The converted value.</returns>
		public static T To<T>(object value)
		{
			//return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
			return (T) To(value, typeof(T));
		}
	}
}