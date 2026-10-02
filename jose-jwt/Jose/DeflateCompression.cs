using System;
using System.IO;
using System.IO.Compression;

namespace Jose
{
	// Token: 0x0200000F RID: 15
	public class DeflateCompression : ICompression
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00003658 File Offset: 0x00001858
		public byte[] Compress(byte[] plainText)
		{
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
				{
					deflateStream.Write(plainText, 0, plainText.Length);
				}
				array = memoryStream.ToArray();
			}
			return array;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000036BC File Offset: 0x000018BC
		public byte[] Decompress(byte[] compressedText)
		{
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (MemoryStream memoryStream2 = new MemoryStream(compressedText))
				{
					using (DeflateStream deflateStream = new DeflateStream(memoryStream2, CompressionMode.Decompress))
					{
						deflateStream.CopyTo(memoryStream);
					}
				}
				array = memoryStream.ToArray();
			}
			return array;
		}
	}
}
