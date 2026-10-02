using System;
using System.Security.Cryptography;
using Jose;

namespace Security.Cryptography
{
	// Token: 0x02000003 RID: 3
	public class RsaKey
	{
		// Token: 0x0600000B RID: 11 RVA: 0x0000235D File Offset: 0x0000055D
		public static CngKey New(RSAParameters parameters)
		{
			return RsaKey.New(parameters.Exponent, parameters.Modulus, parameters.P, parameters.Q);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000237C File Offset: 0x0000057C
		public static CngKey New(byte[] exponent, byte[] modulus, byte[] p = null, byte[] q = null)
		{
			bool flag = p == null || q == null;
			byte[] array = (flag ? RsaKey.BCRYPT_RSAPUBLIC_MAGIC : RsaKey.BCRYPT_RSAPRIVATE_MAGIC);
			byte[] bytes = BitConverter.GetBytes(modulus.Length * 8);
			byte[] bytes2 = BitConverter.GetBytes(exponent.Length);
			byte[] bytes3 = BitConverter.GetBytes(modulus.Length);
			byte[] array2 = (flag ? BitConverter.GetBytes(0) : BitConverter.GetBytes(p.Length));
			byte[] array3 = (flag ? BitConverter.GetBytes(0) : BitConverter.GetBytes(q.Length));
			byte[] array4 = Arrays.Concat(new byte[][] { array, bytes, bytes2, bytes3, array2, array3, exponent, modulus, p, q });
			CngKeyBlobFormat cngKeyBlobFormat = (flag ? CngKeyBlobFormat.GenericPublicBlob : CngKeyBlobFormat.GenericPrivateBlob);
			return CngKey.Import(array4, cngKeyBlobFormat);
		}

		// Token: 0x04000011 RID: 17
		public static readonly byte[] BCRYPT_RSAPUBLIC_MAGIC = BitConverter.GetBytes(826364754);

		// Token: 0x04000012 RID: 18
		public static readonly byte[] BCRYPT_RSAPRIVATE_MAGIC = BitConverter.GetBytes(843141970);
	}
}
