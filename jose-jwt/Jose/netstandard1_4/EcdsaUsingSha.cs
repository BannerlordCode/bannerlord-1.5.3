using System;
using System.Security.Cryptography;

namespace Jose.netstandard1_4
{
	// Token: 0x02000032 RID: 50
	public class EcdsaUsingSha : IJwsAlgorithm
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x00005D88 File Offset: 0x00003F88
		public EcdsaUsingSha(int keySize)
		{
			this.keySize = keySize;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00005D98 File Offset: 0x00003F98
		public byte[] Sign(byte[] securedInput, object key)
		{
			byte[] array;
			try
			{
				if (key is CngKey)
				{
					CngKey cngKey = (CngKey)key;
					Ensure.BitSize(cngKey.KeySize, this.keySize, string.Format("EcdsaUsingSha algorithm expected key of size {0} bits, but was given {1} bits", this.keySize, cngKey.KeySize));
					using (ECDsaCng ecdsaCng = new ECDsaCng(cngKey))
					{
						return ecdsaCng.SignData(securedInput, this.Hash);
					}
				}
				if (!(key is ECDsa))
				{
					throw new ArgumentException("EcdsaUsingSha algorithm expects key to be of either CngKey or ECDsa types.");
				}
				ECDsa ecdsa = (ECDsa)key;
				Ensure.BitSize(ecdsa.KeySize, this.keySize, string.Format("EcdsaUsingSha algorithm expected key of size {0} bits, but was given {1} bits", this.keySize, ecdsa.KeySize));
				array = ecdsa.SignData(securedInput, this.Hash);
			}
			catch (CryptographicException ex)
			{
				throw new JoseException("Unable to sign content.", ex);
			}
			return array;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005E90 File Offset: 0x00004090
		public bool Verify(byte[] signature, byte[] securedInput, object key)
		{
			bool flag;
			try
			{
				if (key is CngKey)
				{
					CngKey cngKey = (CngKey)key;
					Ensure.BitSize(cngKey.KeySize, this.keySize, string.Format("EcdsaUsingSha algorithm expected key of size {0} bits, but was given {1} bits", this.keySize, cngKey.KeySize));
					using (ECDsaCng ecdsaCng = new ECDsaCng(cngKey))
					{
						return ecdsaCng.VerifyData(securedInput, signature, this.Hash);
					}
				}
				if (!(key is ECDsa))
				{
					throw new ArgumentException("EcdsaUsingSha algorithm expects key to be of either CngKey or ECDsa types.");
				}
				ECDsa ecdsa = (ECDsa)key;
				Ensure.BitSize(ecdsa.KeySize, this.keySize, string.Format("EcdsaUsingSha algorithm expected key of size {0} bits, but was given {1} bits", this.keySize, ecdsa.KeySize));
				flag = ecdsa.VerifyData(securedInput, signature, this.Hash);
			}
			catch (CryptographicException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00005F80 File Offset: 0x00004180
		protected HashAlgorithmName Hash
		{
			get
			{
				if (this.keySize == 256)
				{
					return HashAlgorithmName.SHA256;
				}
				if (this.keySize == 384)
				{
					return HashAlgorithmName.SHA384;
				}
				if (this.keySize == 521)
				{
					return HashAlgorithmName.SHA512;
				}
				throw new ArgumentException(string.Format("Unsupported key size: '{0} bytes'", this.keySize));
			}
		}

		// Token: 0x04000073 RID: 115
		private int keySize;
	}
}
