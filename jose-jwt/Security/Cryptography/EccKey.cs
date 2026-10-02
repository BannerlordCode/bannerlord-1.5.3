using System;
using System.Security.Cryptography;
using Jose;

namespace Security.Cryptography
{
	// Token: 0x02000002 RID: 2
	public class EccKey
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public byte[] X
		{
			get
			{
				if (this.x == null)
				{
					this.ExportKey();
				}
				return this.x;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002066 File Offset: 0x00000266
		public byte[] Y
		{
			get
			{
				if (this.y == null)
				{
					this.ExportKey();
				}
				return this.y;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000207C File Offset: 0x0000027C
		public byte[] D
		{
			get
			{
				if (this.d == null)
				{
					this.ExportKey();
				}
				return this.d;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002092 File Offset: 0x00000292
		public CngKey Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000209C File Offset: 0x0000029C
		public static CngKey New(byte[] x, byte[] y, byte[] d = null, CngKeyUsages usage = 2)
		{
			if (x.Length != y.Length)
			{
				throw new ArgumentException("X,Y and D must be same size");
			}
			if (d != null && x.Length != d.Length)
			{
				throw new ArgumentException("X,Y and D must be same size");
			}
			if (usage != 2 && usage != 4)
			{
				throw new ArgumentException("Usage parameter expected to be set either 'CngKeyUsages.Signing' or 'CngKeyUsages.KeyAgreement");
			}
			bool flag = usage == 2;
			int num = x.Length;
			byte[] array;
			if (num == 32)
			{
				array = ((d == null) ? (flag ? EccKey.BCRYPT_ECDSA_PUBLIC_P256_MAGIC : EccKey.BCRYPT_ECDH_PUBLIC_P256_MAGIC) : (flag ? EccKey.BCRYPT_ECDSA_PRIVATE_P256_MAGIC : EccKey.BCRYPT_ECDH_PRIVATE_P256_MAGIC));
			}
			else if (num == 48)
			{
				array = ((d == null) ? (flag ? EccKey.BCRYPT_ECDSA_PUBLIC_P384_MAGIC : EccKey.BCRYPT_ECDH_PUBLIC_P384_MAGIC) : (flag ? EccKey.BCRYPT_ECDSA_PRIVATE_P384_MAGIC : EccKey.BCRYPT_ECDH_PRIVATE_P384_MAGIC));
			}
			else
			{
				if (num != 66)
				{
					throw new ArgumentException("Size of X,Y or D must equal to 32, 48 or 66 bytes");
				}
				array = ((d == null) ? (flag ? EccKey.BCRYPT_ECDSA_PUBLIC_P521_MAGIC : EccKey.BCRYPT_ECDH_PUBLIC_P521_MAGIC) : (flag ? EccKey.BCRYPT_ECDSA_PRIVATE_P521_MAGIC : EccKey.BCRYPT_ECDH_PRIVATE_P521_MAGIC));
			}
			byte[] bytes = BitConverter.GetBytes(num);
			byte[] array2;
			CngKeyBlobFormat cngKeyBlobFormat;
			if (d == null)
			{
				array2 = Arrays.Concat(new byte[][] { array, bytes, x, y });
				cngKeyBlobFormat = CngKeyBlobFormat.EccPublicBlob;
			}
			else
			{
				array2 = Arrays.Concat(new byte[][] { array, bytes, x, y, d });
				cngKeyBlobFormat = CngKeyBlobFormat.EccPrivateBlob;
			}
			return CngKey.Import(array2, cngKeyBlobFormat);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021DC File Offset: 0x000003DC
		public static EccKey Generate(CngKey receiverPubKey)
		{
			CngKey cngKey = CngKey.Create(receiverPubKey.Algorithm, null, new CngKeyCreationParameters
			{
				ExportPolicy = new CngExportPolicies?(2)
			});
			return new EccKey
			{
				key = cngKey
			};
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002213 File Offset: 0x00000413
		public static EccKey Export(CngKey _key)
		{
			return new EccKey
			{
				key = _key
			};
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002224 File Offset: 0x00000424
		private void ExportKey()
		{
			byte[] array = this.key.Export(CngKeyBlobFormat.EccPrivateBlob);
			int num = BitConverter.ToInt32(new byte[]
			{
				array[4],
				array[5],
				array[6],
				array[7]
			}, 0);
			byte[][] array2 = Arrays.Slice(Arrays.RightmostBits(array, num * 24), num);
			this.x = array2[0];
			this.y = array2[1];
			this.d = array2[2];
		}

		// Token: 0x04000001 RID: 1
		public static readonly byte[] BCRYPT_ECDSA_PUBLIC_P256_MAGIC = BitConverter.GetBytes(827540293);

		// Token: 0x04000002 RID: 2
		public static readonly byte[] BCRYPT_ECDSA_PRIVATE_P256_MAGIC = BitConverter.GetBytes(844317509);

		// Token: 0x04000003 RID: 3
		public static readonly byte[] BCRYPT_ECDSA_PUBLIC_P384_MAGIC = BitConverter.GetBytes(861094725);

		// Token: 0x04000004 RID: 4
		public static readonly byte[] BCRYPT_ECDSA_PRIVATE_P384_MAGIC = BitConverter.GetBytes(877871941);

		// Token: 0x04000005 RID: 5
		public static readonly byte[] BCRYPT_ECDSA_PUBLIC_P521_MAGIC = BitConverter.GetBytes(894649157);

		// Token: 0x04000006 RID: 6
		public static readonly byte[] BCRYPT_ECDSA_PRIVATE_P521_MAGIC = BitConverter.GetBytes(911426373);

		// Token: 0x04000007 RID: 7
		public static readonly byte[] BCRYPT_ECDH_PUBLIC_P256_MAGIC = BitConverter.GetBytes(827016005);

		// Token: 0x04000008 RID: 8
		public static readonly byte[] BCRYPT_ECDH_PRIVATE_P256_MAGIC = BitConverter.GetBytes(843793221);

		// Token: 0x04000009 RID: 9
		public static readonly byte[] BCRYPT_ECDH_PUBLIC_P384_MAGIC = BitConverter.GetBytes(860570437);

		// Token: 0x0400000A RID: 10
		public static readonly byte[] BCRYPT_ECDH_PRIVATE_P384_MAGIC = BitConverter.GetBytes(877347653);

		// Token: 0x0400000B RID: 11
		public static readonly byte[] BCRYPT_ECDH_PUBLIC_P521_MAGIC = BitConverter.GetBytes(894124869);

		// Token: 0x0400000C RID: 12
		public static readonly byte[] BCRYPT_ECDH_PRIVATE_P521_MAGIC = BitConverter.GetBytes(910902085);

		// Token: 0x0400000D RID: 13
		private CngKey key;

		// Token: 0x0400000E RID: 14
		private byte[] x;

		// Token: 0x0400000F RID: 15
		private byte[] y;

		// Token: 0x04000010 RID: 16
		private byte[] d;
	}
}
