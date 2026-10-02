using System;
using System.Security.Cryptography;

namespace Jose
{
	// Token: 0x02000028 RID: 40
	public class RsaPssUsingSha : IJwsAlgorithm
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x0000513F File Offset: 0x0000333F
		public RsaPssUsingSha(int saltSize)
		{
			this.saltSize = saltSize;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000514E File Offset: 0x0000334E
		public byte[] Sign(byte[] securedInput, object key)
		{
			return Ensure.Type<RSA>(key, "RsaUsingSha with PSS padding alg expects key to be of RSA type.", Array.Empty<object>()).SignData(securedInput, this.HashAlgorithm, RSASignaturePadding.Pss);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005171 File Offset: 0x00003371
		public bool Verify(byte[] signature, byte[] securedInput, object key)
		{
			return Ensure.Type<RSA>(key, "RsaUsingSha with PSS padding alg expects key to be of RSA type.", Array.Empty<object>()).VerifyData(securedInput, signature, this.HashAlgorithm, RSASignaturePadding.Pss);
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00005198 File Offset: 0x00003398
		private HashAlgorithmName HashAlgorithm
		{
			get
			{
				if (this.saltSize == 32)
				{
					return HashAlgorithmName.SHA256;
				}
				if (this.saltSize == 48)
				{
					return HashAlgorithmName.SHA384;
				}
				if (this.saltSize == 64)
				{
					return HashAlgorithmName.SHA512;
				}
				throw new ArgumentException(string.Format("Unsupported salt size: '{0} bytes'", this.saltSize));
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000051F0 File Offset: 0x000033F0
		private CngAlgorithm Hash
		{
			get
			{
				if (this.saltSize == 32)
				{
					return CngAlgorithm.Sha256;
				}
				if (this.saltSize == 48)
				{
					return CngAlgorithm.Sha384;
				}
				if (this.saltSize == 64)
				{
					return CngAlgorithm.Sha512;
				}
				throw new ArgumentException(string.Format("Unsupported salt size: '{0} bytes'", this.saltSize));
			}
		}

		// Token: 0x0400005A RID: 90
		private int saltSize;
	}
}
