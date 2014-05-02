using System;
using System.IO;
using System.Security.Cryptography;

namespace LeagueSoldierDeathTeam.BusinessLogic.Classes.Security
{
	public static class Symmetric
	{
		public const string Rijndael = "Rijndael";

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		public static byte[] Encrypt(string symmetricAlgorithm, string input, byte[] key, byte[] iv)
		{
			if (input == null || input.Length <= 0)
				return new byte[0];
			if (key == null || key.Length <= 0)
				throw new ArgumentNullException("key");
			if (iv == null || iv.Length <= 0)
				throw new ArgumentNullException("iv");

			MemoryStream msEncrypt;
			StreamWriter swEncrypt = null;

			try
			{
				using (var rijn = SymmetricAlgorithm.Create(symmetricAlgorithm))
				{
					msEncrypt = new MemoryStream();
					swEncrypt = new StreamWriter(new CryptoStream(msEncrypt, rijn.CreateEncryptor(key, iv), CryptoStreamMode.Write));
					swEncrypt.Write(input);
				}
			}
			finally
			{
				if (swEncrypt != null)
					swEncrypt.Close();
			}

			return msEncrypt.ToArray();
		}

		public static string Decrypt(string symmetricAlgorithm, byte[] cipherText, byte[] key, byte[] iv)
		{
			if (cipherText == null || cipherText.Length <= 0)
				throw new ArgumentNullException("cipherText");
			if (key == null || key.Length <= 0)
				throw new ArgumentNullException("key");
			if (iv == null || iv.Length <= 0)
				throw new ArgumentNullException("iv");

			MemoryStream msDecrypt = null;
			string plaintext;

			try
			{
				// Create the streams used for decryption.
				using (var rijn = SymmetricAlgorithm.Create(symmetricAlgorithm))
				{
					msDecrypt = new MemoryStream(cipherText);
					var csDecrypt = new CryptoStream(msDecrypt, rijn.CreateDecryptor(key, iv), CryptoStreamMode.Read);
					var srDecrypt = new StreamReader(csDecrypt);
					plaintext = srDecrypt.ReadToEnd();
				}
			}
			finally
			{
				if (msDecrypt != null)
					msDecrypt.Close();
			}

			return plaintext;
		}
	}
}
