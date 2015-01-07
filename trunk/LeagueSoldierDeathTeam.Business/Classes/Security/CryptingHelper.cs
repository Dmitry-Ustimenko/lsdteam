using System.Security.Cryptography;
using System.Web;

namespace LeagueSoldierDeathTeam.Business.Classes.Security
{
	public static class CryptingHelper
	{
		private static class Key
		{
			#region Keys

			public const string GoldKey = "F7CC67B54D54C2A96729D29204ECA6D1EFE5D17EC2927080715D467C3CA94B9D";
			public const string GoldIv = "938329794A029D51DD4177C827A1AC03";

			#endregion
		}

		public static string GenerateEncodedUniqueToken()
		{
			var randomSequence = new byte[12];
			using (var cryptoProvider = new RNGCryptoServiceProvider())
			{
				cryptoProvider.GetBytes(randomSequence);
			}
			return HttpServerUtility.UrlTokenEncode(randomSequence);
		}

		public static string CryptRijndael(object value)
		{
			var key = HexEncoding.GetBytes(Key.GoldKey);
			var iv = HexEncoding.GetBytes(Key.GoldIv);
			return HexEncoding.ToString(Symmetric.Encrypt(Symmetric.Rijndael, value.ToString(), key, iv));
		}

		public static string DecryptRijndael(string value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;

			var key = HexEncoding.GetBytes(Key.GoldKey);
			var iv = HexEncoding.GetBytes(Key.GoldIv);
			return Symmetric.Decrypt(Symmetric.Rijndael, HexEncoding.GetBytes(value), key, iv);
		}

	}
}
