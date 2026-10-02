using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Jose.native
{
	// Token: 0x02000031 RID: 49
	public static class NCrypt
	{
		// Token: 0x060000E3 RID: 227
		[DllImport("ncrypt.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern uint NCryptSecretAgreement(SafeNCryptKeyHandle hPrivKey, SafeNCryptKeyHandle hPublicKey, out SafeNCryptSecretHandle phSecret, uint flags);

		// Token: 0x060000E4 RID: 228
		[DllImport("ncrypt.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern uint NCryptDeriveKey(SafeNCryptSecretHandle hSharedSecret, string kdf, NCrypt.NCryptBufferDesc parameterList, byte[] derivedKey, uint derivedKeyByteSize, out uint result, uint flags);

		// Token: 0x060000E5 RID: 229
		[DllImport("ncrypt.dll")]
		internal static extern uint NCryptSignHash(SafeNCryptKeyHandle hKey, ref BCrypt.BCRYPT_PSS_PADDING_INFO pPaddingInfo, byte[] pbHashValue, int cbHashValue, byte[] pbSignature, int cbSignature, out uint pcbResult, uint dwFlags);

		// Token: 0x060000E6 RID: 230
		[DllImport("ncrypt.dll")]
		internal static extern uint NCryptVerifySignature(SafeNCryptKeyHandle hKey, ref BCrypt.BCRYPT_PSS_PADDING_INFO pPaddingInfo, byte[] pbHashValue, int cbHashValue, byte[] pbSignature, int cbSignature, uint dwFlags);

		// Token: 0x060000E7 RID: 231
		[DllImport("ncrypt.dll")]
		internal static extern uint NCryptDecrypt(SafeNCryptKeyHandle hKey, byte[] pbInput, int cbInput, ref BCrypt.BCRYPT_OAEP_PADDING_INFO pvPadding, byte[] pbOutput, uint cbOutput, out uint pcbResult, uint dwFlags);

		// Token: 0x060000E8 RID: 232
		[DllImport("ncrypt.dll")]
		internal static extern uint NCryptEncrypt(SafeNCryptKeyHandle hKey, byte[] pbInput, int cbInput, ref BCrypt.BCRYPT_OAEP_PADDING_INFO pvPadding, byte[] pbOutput, uint cbOutput, out uint pcbResult, uint dwFlags);

		// Token: 0x0400006D RID: 109
		public const uint NTE_BAD_SIGNATURE = 2148073478U;

		// Token: 0x0400006E RID: 110
		public const uint KDF_ALGORITHMID = 8U;

		// Token: 0x0400006F RID: 111
		public const uint KDF_PARTYUINFO = 9U;

		// Token: 0x04000070 RID: 112
		public const uint KDF_PARTYVINFO = 10U;

		// Token: 0x04000071 RID: 113
		public const uint KDF_SUPPPUBINFO = 11U;

		// Token: 0x04000072 RID: 114
		public const uint KDF_SUPPPRIVINFO = 12U;

		// Token: 0x0200003D RID: 61
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public class NCryptBufferDesc : IDisposable
		{
			// Token: 0x060000FE RID: 254 RVA: 0x0000621C File Offset: 0x0000441C
			public NCryptBufferDesc(params NCrypt.NCryptBuffer[] buffers)
			{
				this.cBuffers = (uint)buffers.Length;
				this.ulVersion = 0U;
				this.pBuffers = Marshal.AllocHGlobal(buffers.Sum<NCrypt.NCryptBuffer>((NCrypt.NCryptBuffer buf) => Marshal.SizeOf<NCrypt.NCryptBuffer>(buf)));
				int num = 0;
				foreach (NCrypt.NCryptBuffer ncryptBuffer in buffers)
				{
					Marshal.StructureToPtr<NCrypt.NCryptBuffer>(ncryptBuffer, this.pBuffers + num, false);
					num += Marshal.SizeOf<NCrypt.NCryptBuffer>(ncryptBuffer);
				}
			}

			// Token: 0x060000FF RID: 255 RVA: 0x000062A1 File Offset: 0x000044A1
			public void Dispose()
			{
				Marshal.FreeHGlobal(this.pBuffers);
			}

			// Token: 0x04000091 RID: 145
			public uint ulVersion;

			// Token: 0x04000092 RID: 146
			public uint cBuffers;

			// Token: 0x04000093 RID: 147
			public IntPtr pBuffers;
		}

		// Token: 0x0200003E RID: 62
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public class NCryptBuffer : IDisposable
		{
			// Token: 0x06000100 RID: 256 RVA: 0x000062B0 File Offset: 0x000044B0
			public NCryptBuffer(uint bufferType, string data)
			{
				this.BufferType = bufferType;
				this.cbBuffer = (uint)(data.Length * 2 + 2);
				this.pvBuffer = Marshal.AllocHGlobal(data.Length * 2);
				Marshal.Copy(data.ToCharArray(), 0, this.pvBuffer, data.Length);
			}

			// Token: 0x06000101 RID: 257 RVA: 0x00006305 File Offset: 0x00004505
			public NCryptBuffer(uint bufferType, byte[] data)
			{
				this.BufferType = bufferType;
				this.cbBuffer = (uint)data.Length;
				this.pvBuffer = Marshal.AllocHGlobal(data.Length);
				Marshal.Copy(data, 0, this.pvBuffer, data.Length);
			}

			// Token: 0x06000102 RID: 258 RVA: 0x0000633B File Offset: 0x0000453B
			public void Dispose()
			{
				Marshal.FreeHGlobal(this.pvBuffer);
			}

			// Token: 0x04000094 RID: 148
			public uint cbBuffer;

			// Token: 0x04000095 RID: 149
			public uint BufferType;

			// Token: 0x04000096 RID: 150
			public IntPtr pvBuffer;
		}
	}
}
