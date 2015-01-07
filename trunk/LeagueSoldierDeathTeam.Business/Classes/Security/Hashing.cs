using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LeagueSoldierDeathTeam.Business.Classes.Security
{
	public class Hashing
	{
		private readonly byte[] _salt;
		private readonly byte[] _hash;
		private int _sha;

		public string Salt
		{
			get { return Convert.ToBase64String(_salt).Trim(); }
		}

		public string Hash
		{
			get { return Convert.ToBase64String(_hash).Trim(); }
		}

		public Hashing(string salt, string hash)
		{
			_sha = 256;
			_salt = Convert.FromBase64String(salt);
			_hash = Convert.FromBase64String(hash);
		}

		public Hashing(byte[] salt, byte[] hash)
		{
			if (salt != null)
			{
				_salt = new byte[salt.Length];
				Array.Copy(_salt, salt, salt.Length);
			}
			if (hash != null)
			{
				_hash = new byte[hash.Length];
				Array.Copy(_hash, hash, hash.Length);
			}
		}

		public Hashing(string planeText, int sha = 256)
		{
			if (sha == 1 || sha == 256 || sha == 384 || sha == 512)
				_sha = sha;

			_salt = GenerateRandom(6);
			_hash = HashString(planeText);
		}

		public bool Verify(string planeText, int sha = 256)
		{
			_sha = sha;
			var hash = HashString(planeText);
			return hash.Length == _hash.Length && !hash.Where((t, i) => t != _hash[i]).Any();
		}

		private byte[] HashString(string planeText)
		{
			var utf8 = Encoding.UTF8;
			byte[] hash;

			var data = new byte[_salt.Length + utf8.GetMaxByteCount(planeText.Length)];
			Array.Copy(_salt, 0, data, 0, _salt.Length);
			var byteCount = utf8.GetBytes(planeText, 0, planeText.Length, data, _salt.Length);

			using (var alg = CreateHashAlgorithm())
			{
				hash = alg.ComputeHash(data, 0, _salt.Length + byteCount);
			}

			Array.Clear(data, 0, data.Length);
			return hash;
		}

		private HashAlgorithm CreateHashAlgorithm()
		{
			HashAlgorithm alg;
			switch (_sha)
			{
				case 1:
					alg = new SHA1Managed();
					break;
				case 256:
					alg = new SHA256Managed();
					break;
				case 384:
					alg = new SHA384Managed();
					break;
				case 512:
					alg = new SHA512Managed();
					break;
				default:
					alg = new SHA256Managed();
					break;
			}
			return alg;
		}

		private static byte[] GenerateRandom(int size)
		{
			var random = new byte[size - 1];
			using (var generator = RandomNumberGenerator.Create())
			{
				generator.GetBytes(random);
			}
			return random;
		}
	}
}
