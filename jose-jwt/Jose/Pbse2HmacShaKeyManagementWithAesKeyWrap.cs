using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Jose
{
	// Token: 0x02000020 RID: 32
	public class Pbse2HmacShaKeyManagementWithAesKeyWrap : IKeyManagement
	{
		// Token: 0x0600008B RID: 139 RVA: 0x000048BE File Offset: 0x00002ABE
		public Pbse2HmacShaKeyManagementWithAesKeyWrap(int keyLengthBits, AesKeyWrapManagement aesKw)
		{
			this.aesKW = aesKw;
			this.keyLengthBits = keyLengthBits;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000048D4 File Offset: 0x00002AD4
		public byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header)
		{
			string text = Ensure.Type<string>(key, "Pbse2HmacShaKeyManagementWithAesKeyWrap management algorithm expectes key to be string.", Array.Empty<object>());
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			byte[] bytes2 = Encoding.UTF8.GetBytes((string)header["alg"]);
			int num = 8192;
			byte[] array = Arrays.Random(96);
			header["p2c"] = num;
			header["p2s"] = Base64Url.Encode(array);
			byte[] array2 = Arrays.Concat(new byte[][]
			{
				bytes2,
				Arrays.Zero,
				array
			});
			byte[] array3;
			using (HMAC prf = this.PRF)
			{
				array3 = PBKDF2.DeriveKey(bytes, array2, num, this.keyLengthBits, prf);
			}
			return this.aesKW.WrapNewKey(cekSizeBits, array3, header);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000049B4 File Offset: 0x00002BB4
		public byte[] Unwrap(byte[] encryptedCek, object key, int cekSizeBits, IDictionary<string, object> header)
		{
			string text = Ensure.Type<string>(key, "Pbse2HmacShaKeyManagementWithAesKeyWrap management algorithm expectes key to be string.", Array.Empty<object>());
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			Ensure.Contains(header, new string[] { "p2c" }, "Pbse2HmacShaKeyManagementWithAesKeyWrap algorithm expects 'p2c' param in JWT header, but was not found", Array.Empty<object>());
			Ensure.Contains(header, new string[] { "p2s" }, "Pbse2HmacShaKeyManagementWithAesKeyWrap algorithm expects 'p2s' param in JWT header, but was not found", Array.Empty<object>());
			byte[] bytes2 = Encoding.UTF8.GetBytes((string)header["alg"]);
			int num = Convert.ToInt32(header["p2c"]);
			byte[] array = Base64Url.Decode((string)header["p2s"]);
			byte[] array2 = Arrays.Concat(new byte[][]
			{
				bytes2,
				Arrays.Zero,
				array
			});
			byte[] array3;
			using (HMAC prf = this.PRF)
			{
				array3 = PBKDF2.DeriveKey(bytes, array2, num, this.keyLengthBits, prf);
			}
			return this.aesKW.Unwrap(encryptedCek, array3, cekSizeBits, header);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00004ACC File Offset: 0x00002CCC
		private HMAC PRF
		{
			get
			{
				if (this.keyLengthBits == 128)
				{
					return new HMACSHA256();
				}
				if (this.keyLengthBits == 192)
				{
					return new HMACSHA384();
				}
				if (this.keyLengthBits == 256)
				{
					return new HMACSHA512();
				}
				throw new ArgumentException(string.Format("Unsupported key size: '{0}'", this.keyLengthBits));
			}
		}

		// Token: 0x04000053 RID: 83
		private AesKeyWrapManagement aesKW;

		// Token: 0x04000054 RID: 84
		private int keyLengthBits;
	}
}
