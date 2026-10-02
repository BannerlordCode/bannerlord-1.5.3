using System;
using System.IO;
using System.IO.Compression;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000025 RID: 37
	internal static class ZipExtensions
	{
		// Token: 0x06000148 RID: 328 RVA: 0x00006C68 File Offset: 0x00004E68
		public static void FillFrom(this ZipArchiveEntry entry, byte[] data)
		{
			using (Stream stream = entry.Open())
			{
				stream.Write(data, 0, data.Length);
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00006CA4 File Offset: 0x00004EA4
		public static void FillFrom(this ZipArchiveEntry entry, BinaryWriter writer)
		{
			using (Stream stream = entry.Open())
			{
				byte[] finalData = writer.GetFinalData();
				stream.Write(finalData, 0, finalData.Length);
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00006CE8 File Offset: 0x00004EE8
		public static BinaryReader GetBinaryReader(this ZipArchiveEntry entry)
		{
			BinaryReader binaryReader = null;
			using (Stream stream = entry.Open())
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					stream.CopyTo(memoryStream);
					binaryReader = new BinaryReader(memoryStream.ToArray());
				}
			}
			return binaryReader;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00006D4C File Offset: 0x00004F4C
		public static byte[] GetBinaryData(this ZipArchiveEntry entry)
		{
			byte[] array = null;
			using (Stream stream = entry.Open())
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					stream.CopyTo(memoryStream);
					array = memoryStream.ToArray();
				}
			}
			return array;
		}
	}
}
