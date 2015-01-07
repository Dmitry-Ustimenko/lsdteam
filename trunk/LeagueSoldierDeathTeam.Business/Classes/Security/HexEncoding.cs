using System;
using System.Globalization;
using System.Linq;

namespace LeagueSoldierDeathTeam.Business.Classes.Security
{
	public static class HexEncoding
	{
		public static int GetByteCount(string value)
		{
			var numHexChars = value.Count(IsHexDigit);
			if (numHexChars % 2 != 0)
				numHexChars--;
			return numHexChars / 2;
		}

		public static byte[] GetBytes(string value)
		{
			var newString = value.Where(IsHexDigit).Aggregate(string.Empty, (current, c) => current + c);

			// remove all none A-F, 0-9, characters

			if (newString.Length % 2 != 0)
			{
				newString = newString.Substring(0, newString.Length - 1);
			}

			var byteLength = newString.Length / 2;
			var bytes = new byte[byteLength];
			var j = 0;

			for (var i = 0; i < bytes.Length; i++)
			{
				var hex = new String(new[] { newString[j], newString[j + 1] });
				bytes[i] = HexToByte(hex);
				j = j + 2;
			}

			return bytes;
		}

		public static string ToString(byte[] data)
		{
			return data.Aggregate(string.Empty, (current, t) => current + t.ToString("X2", CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Determines if given string is in proper hexadecimal string format
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool InHexFormat(string value)
		{
			return value.All(IsHexDigit);
		}

		/// <summary>
		/// Returns true is c is a hexadecimal digit (A-F, a-f, 0-9)
		/// </summary>
		/// <param name="c">Character to test</param>
		/// <returns>true if hex digit, false if not</returns>
		public static bool IsHexDigit(Char c)
		{
			var numA = Convert.ToInt32('A');
			var num1 = Convert.ToInt32('0');
			c = Char.ToUpper(c, CultureInfo.InvariantCulture);
			var numChar = Convert.ToInt32(c);
			if (numChar >= numA && numChar < (numA + 6))
				return true;
			return numChar >= num1 && numChar < (num1 + 10);
		}

		/// <summary>
		/// Converts 1 or 2 character string into equivalant byte value
		/// </summary>
		/// <param name="value">1 or 2 character string</param>
		/// <returns>byte</returns>
		public static byte HexToByte(string value)
		{
			if (value.Length > 2 || value.Length <= 0)
				throw new ArgumentException("hex must be 1 or 2 characters in length");
			var newByte = byte.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			return newByte;
		}
	}
}
