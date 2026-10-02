using System;
using System.IO;
using System.Threading.Tasks;

namespace TaleWorlds.Library
{
	// Token: 0x02000031 RID: 49
	public static class FileHelper
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x00006DC3 File Offset: 0x00004FC3
		public static SaveResult SaveFile(PlatformFilePath path, byte[] data)
		{
			return Common.PlatformFileHelper.SaveFile(path, data);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00006DD1 File Offset: 0x00004FD1
		public static SaveResult SaveFileString(PlatformFilePath path, string data)
		{
			return Common.PlatformFileHelper.SaveFileString(path, data);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00006DDF File Offset: 0x00004FDF
		public static string GetFileFullPath(PlatformFilePath path)
		{
			return Common.PlatformFileHelper.GetFileFullPath(path);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00006DEC File Offset: 0x00004FEC
		public static SaveResult AppendLineToFileString(PlatformFilePath path, string data)
		{
			return Common.PlatformFileHelper.AppendLineToFileString(path, data);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00006DFA File Offset: 0x00004FFA
		public static Task<SaveResult> SaveFileAsync(PlatformFilePath path, byte[] data)
		{
			return Common.PlatformFileHelper.SaveFileAsync(path, data);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00006E08 File Offset: 0x00005008
		public static Task<SaveResult> SaveFileStringAsync(PlatformFilePath path, string data)
		{
			return Common.PlatformFileHelper.SaveFileStringAsync(path, data);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00006E16 File Offset: 0x00005016
		public static string GetError()
		{
			return Common.PlatformFileHelper.GetError();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00006E22 File Offset: 0x00005022
		public static bool FileExists(PlatformFilePath path)
		{
			return Common.PlatformFileHelper.FileExists(path);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00006E2F File Offset: 0x0000502F
		public static Task<string> GetFileContentStringAsync(PlatformFilePath path)
		{
			return Common.PlatformFileHelper.GetFileContentStringAsync(path);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00006E3C File Offset: 0x0000503C
		public static string GetFileContentString(PlatformFilePath path)
		{
			return Common.PlatformFileHelper.GetFileContentString(path);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00006E49 File Offset: 0x00005049
		public static void DeleteFile(PlatformFilePath path)
		{
			Common.PlatformFileHelper.DeleteFile(path);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00006E57 File Offset: 0x00005057
		public static PlatformFilePath[] GetFiles(PlatformDirectoryPath path, string searchPattern, SearchOption searchOption)
		{
			return Common.PlatformFileHelper.GetFiles(path, searchPattern, searchOption);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00006E66 File Offset: 0x00005066
		public static byte[] GetFileContent(PlatformFilePath filePath)
		{
			return Common.PlatformFileHelper.GetFileContent(filePath);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00006E73 File Offset: 0x00005073
		public static byte[] GetMetaDataContent(PlatformFilePath filePath)
		{
			return Common.PlatformFileHelper.GetMetaDataContent(filePath);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00006E80 File Offset: 0x00005080
		public static void CopyFile(PlatformFilePath source, PlatformFilePath target)
		{
			byte[] fileContent = FileHelper.GetFileContent(source);
			FileHelper.SaveFile(target, fileContent);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00006E9C File Offset: 0x0000509C
		public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDir);
			if (!directoryInfo.Exists)
			{
				return;
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			Directory.CreateDirectory(destinationDir);
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				string text = Path.Combine(destinationDir, fileInfo.Name);
				fileInfo.CopyTo(text);
			}
			if (recursive)
			{
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					string text2 = Path.Combine(destinationDir, directoryInfo2.Name);
					FileHelper.CopyDirectory(directoryInfo2.FullName, text2, true);
				}
			}
		}
	}
}
