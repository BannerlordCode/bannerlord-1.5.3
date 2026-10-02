using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.Library
{
	// Token: 0x02000080 RID: 128
	public struct PlatformFilePath
	{
		// Token: 0x06000488 RID: 1160 RVA: 0x0001011C File Offset: 0x0000E31C
		public PlatformFilePath(PlatformDirectoryPath folderPath, string fileName)
		{
			this.FolderPath = folderPath;
			this.FileName = fileName;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001012C File Offset: 0x0000E32C
		public static PlatformFilePath operator +(PlatformFilePath path, string str)
		{
			return new PlatformFilePath(path.FolderPath, path.FileName + str);
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x00010145 File Offset: 0x0000E345
		public string FileFullPath
		{
			get
			{
				return Common.PlatformFileHelper.GetFileFullPath(this);
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00010158 File Offset: 0x0000E358
		public string GetFileNameWithoutExtension()
		{
			int num = this.FileName.LastIndexOf('.');
			if (num == -1)
			{
				return this.FileName;
			}
			return this.FileName.Substring(0, num);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0001018B File Offset: 0x0000E38B
		public override string ToString()
		{
			return this.FolderPath.ToString() + " - " + this.FileName;
		}

		// Token: 0x0400016D RID: 365
		public PlatformDirectoryPath FolderPath;

		// Token: 0x0400016E RID: 366
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string FileName;
	}
}
