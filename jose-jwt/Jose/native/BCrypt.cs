using System;
using System.Runtime.InteropServices;

namespace Jose.native
{
	// Token: 0x02000030 RID: 48
	public static class BCrypt
	{
		// Token: 0x060000DA RID: 218
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptOpenAlgorithmProvider(out IntPtr phAlgorithm, [MarshalAs(UnmanagedType.LPWStr)] string pszAlgId, [MarshalAs(UnmanagedType.LPWStr)] string pszImplementation, uint dwFlags);

		// Token: 0x060000DB RID: 219
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, uint flags);

		// Token: 0x060000DC RID: 220
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptGetProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbOutput, int cbOutput, ref int pcbResult, uint flags);

		// Token: 0x060000DD RID: 221
		[DllImport("bcrypt.dll", EntryPoint = "BCryptSetProperty")]
		internal static extern uint BCryptSetAlgorithmProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbInput, int cbInput, int dwFlags);

		// Token: 0x060000DE RID: 222
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptImportKey(IntPtr hAlgorithm, IntPtr hImportKey, [MarshalAs(UnmanagedType.LPWStr)] string pszBlobType, out IntPtr phKey, IntPtr pbKeyObject, int cbKeyObject, byte[] pbInput, int cbInput, uint dwFlags);

		// Token: 0x060000DF RID: 223
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptDestroyKey(IntPtr hKey);

		// Token: 0x060000E0 RID: 224
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptEncrypt(IntPtr hKey, byte[] pbInput, int cbInput, ref BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo, byte[] pbIV, int cbIV, byte[] pbOutput, int cbOutput, ref int pcbResult, uint dwFlags);

		// Token: 0x060000E1 RID: 225
		[DllImport("bcrypt.dll")]
		internal static extern uint BCryptDecrypt(IntPtr hKey, byte[] pbInput, int cbInput, ref BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo, byte[] pbIV, int cbIV, byte[] pbOutput, int cbOutput, ref int pcbResult, int dwFlags);

		// Token: 0x0400005F RID: 95
		public const uint ERROR_SUCCESS = 0U;

		// Token: 0x04000060 RID: 96
		public const uint BCRYPT_PAD_PSS = 8U;

		// Token: 0x04000061 RID: 97
		public const uint BCRYPT_PAD_OAEP = 4U;

		// Token: 0x04000062 RID: 98
		public static readonly byte[] BCRYPT_KEY_DATA_BLOB_MAGIC = BitConverter.GetBytes(1296188491);

		// Token: 0x04000063 RID: 99
		public static readonly string BCRYPT_OBJECT_LENGTH = "ObjectLength";

		// Token: 0x04000064 RID: 100
		public static readonly string BCRYPT_CHAIN_MODE_GCM = "ChainingModeGCM";

		// Token: 0x04000065 RID: 101
		public static readonly string BCRYPT_AUTH_TAG_LENGTH = "AuthTagLength";

		// Token: 0x04000066 RID: 102
		public static readonly string BCRYPT_CHAINING_MODE = "ChainingMode";

		// Token: 0x04000067 RID: 103
		public static readonly string BCRYPT_KEY_DATA_BLOB = "KeyDataBlob";

		// Token: 0x04000068 RID: 104
		public static readonly string BCRYPT_AES_ALGORITHM = "AES";

		// Token: 0x04000069 RID: 105
		public static readonly string MS_PRIMITIVE_PROVIDER = "Microsoft Primitive Provider";

		// Token: 0x0400006A RID: 106
		public static readonly int BCRYPT_AUTH_MODE_CHAIN_CALLS_FLAG = 1;

		// Token: 0x0400006B RID: 107
		public static readonly int BCRYPT_INIT_AUTH_MODE_INFO_VERSION = 1;

		// Token: 0x0400006C RID: 108
		public static readonly uint STATUS_AUTH_TAG_MISMATCH = 3221266434U;

		// Token: 0x02000039 RID: 57
		public struct BCRYPT_PSS_PADDING_INFO
		{
			// Token: 0x060000FA RID: 250 RVA: 0x0000608D File Offset: 0x0000428D
			public BCRYPT_PSS_PADDING_INFO(string pszAlgId, int cbSalt)
			{
				this.pszAlgId = pszAlgId;
				this.cbSalt = cbSalt;
			}

			// Token: 0x0400007C RID: 124
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszAlgId;

			// Token: 0x0400007D RID: 125
			public int cbSalt;
		}

		// Token: 0x0200003A RID: 58
		public struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO : IDisposable
		{
			// Token: 0x060000FB RID: 251 RVA: 0x000060A0 File Offset: 0x000042A0
			public BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(byte[] iv, byte[] aad, byte[] tag)
			{
				this = default(BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO);
				this.dwInfoVersion = BCrypt.BCRYPT_INIT_AUTH_MODE_INFO_VERSION;
				this.cbSize = Marshal.SizeOf(typeof(BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
				if (iv != null)
				{
					this.cbNonce = iv.Length;
					this.pbNonce = Marshal.AllocHGlobal(this.cbNonce);
					Marshal.Copy(iv, 0, this.pbNonce, this.cbNonce);
				}
				if (aad != null)
				{
					this.cbAuthData = aad.Length;
					this.pbAuthData = Marshal.AllocHGlobal(this.cbAuthData);
					Marshal.Copy(aad, 0, this.pbAuthData, this.cbAuthData);
				}
				if (tag != null)
				{
					this.cbTag = tag.Length;
					this.pbTag = Marshal.AllocHGlobal(this.cbTag);
					Marshal.Copy(tag, 0, this.pbTag, this.cbTag);
					this.cbMacContext = tag.Length;
					this.pbMacContext = Marshal.AllocHGlobal(this.cbMacContext);
				}
			}

			// Token: 0x060000FC RID: 252 RVA: 0x00006180 File Offset: 0x00004380
			public void Dispose()
			{
				if (this.pbNonce != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbNonce);
				}
				if (this.pbTag != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbTag);
				}
				if (this.pbAuthData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbAuthData);
				}
				if (this.pbMacContext != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbMacContext);
				}
			}

			// Token: 0x0400007E RID: 126
			public int cbSize;

			// Token: 0x0400007F RID: 127
			public int dwInfoVersion;

			// Token: 0x04000080 RID: 128
			public IntPtr pbNonce;

			// Token: 0x04000081 RID: 129
			public int cbNonce;

			// Token: 0x04000082 RID: 130
			public IntPtr pbAuthData;

			// Token: 0x04000083 RID: 131
			public int cbAuthData;

			// Token: 0x04000084 RID: 132
			public IntPtr pbTag;

			// Token: 0x04000085 RID: 133
			public int cbTag;

			// Token: 0x04000086 RID: 134
			public IntPtr pbMacContext;

			// Token: 0x04000087 RID: 135
			public int cbMacContext;

			// Token: 0x04000088 RID: 136
			public int cbAAD;

			// Token: 0x04000089 RID: 137
			public long cbData;

			// Token: 0x0400008A RID: 138
			public int dwFlags;
		}

		// Token: 0x0200003B RID: 59
		public struct BCRYPT_KEY_LENGTHS_STRUCT
		{
			// Token: 0x0400008B RID: 139
			public int dwMinLength;

			// Token: 0x0400008C RID: 140
			public int dwMaxLength;

			// Token: 0x0400008D RID: 141
			public int dwIncrement;
		}

		// Token: 0x0200003C RID: 60
		public struct BCRYPT_OAEP_PADDING_INFO
		{
			// Token: 0x060000FD RID: 253 RVA: 0x00006201 File Offset: 0x00004401
			public BCRYPT_OAEP_PADDING_INFO(string alg)
			{
				this.pszAlgId = alg;
				this.pbLabel = IntPtr.Zero;
				this.cbLabel = 0;
			}

			// Token: 0x0400008E RID: 142
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszAlgId;

			// Token: 0x0400008F RID: 143
			public IntPtr pbLabel;

			// Token: 0x04000090 RID: 144
			public int cbLabel;
		}
	}
}
