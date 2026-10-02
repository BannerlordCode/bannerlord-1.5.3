using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.Library
{
	// Token: 0x0200007D RID: 125
	public struct PlatformDirectoryPath
	{
		// Token: 0x06000471 RID: 1137 RVA: 0x0000FB9C File Offset: 0x0000DD9C
		public PlatformDirectoryPath(PlatformFileType type, string path)
		{
			this.Type = type;
			this.Path = path;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000FBAC File Offset: 0x0000DDAC
		public static PlatformDirectoryPath operator +(PlatformDirectoryPath path, string str)
		{
			return new PlatformDirectoryPath(path.Type, path.Path + str);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000FBC5 File Offset: 0x0000DDC5
		public override string ToString()
		{
			return this.Type + " " + this.Path;
		}

		// Token: 0x04000165 RID: 357
		public PlatformFileType Type;

		// Token: 0x04000166 RID: 358
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string Path;
	}
}
