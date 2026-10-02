using System;
using System.Security.Cryptography;

namespace Jose
{
	// Token: 0x02000029 RID: 41
	public class RsaUsingSha : IJwsAlgorithm
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00005247 File Offset: 0x00003447
		public RsaUsingSha(string hashMethod)
		{
			this.hashMethod = hashMethod;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005256 File Offset: 0x00003456
		public byte[] Sign(byte[] securedInput, object key)
		{
			return Ensure.Type<RSA>(key, "RsaUsingSha alg expects key to be of RSA type.", Array.Empty<object>()).SignData(securedInput, this.HashAlgorithm, RSASignaturePadding.Pkcs1);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005279 File Offset: 0x00003479
		public bool Verify(byte[] signature, byte[] securedInput, object key)
		{
			return Ensure.Type<RSA>(key, "RsaUsingSha alg expects key to be of RSA type.", Array.Empty<object>()).VerifyData(securedInput, signature, this.HashAlgorithm, RSASignaturePadding.Pkcs1);
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000052A0 File Offset: 0x000034A0
		private HashAlgorithmName HashAlgorithm
		{
			get
			{
				if (this.hashMethod.Equals("SHA256"))
				{
					return HashAlgorithmName.SHA256;
				}
				if (this.hashMethod.Equals("SHA384"))
				{
					return HashAlgorithmName.SHA384;
				}
				if (this.hashMethod.Equals("SHA512"))
				{
					return HashAlgorithmName.SHA512;
				}
				throw new ArgumentException("Unsupported hashing algorithm: '{0}'", this.hashMethod);
			}
		}

		// Token: 0x0400005B RID: 91
		private string hashMethod;
	}
}
