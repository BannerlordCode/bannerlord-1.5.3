using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x0200005F RID: 95
	public static class FilePaths
	{
		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x00018F23 File Offset: 0x00017123
		public static PlatformDirectoryPath SavePath
		{
			get
			{
				return new PlatformDirectoryPath(PlatformFileType.User, "Game Saves");
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x00018F30 File Offset: 0x00017130
		public static PlatformDirectoryPath RecordingsPath
		{
			get
			{
				return new PlatformDirectoryPath(PlatformFileType.User, "Recordings");
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x00018F3D File Offset: 0x0001713D
		public static PlatformDirectoryPath StatisticsPath
		{
			get
			{
				return new PlatformDirectoryPath(PlatformFileType.User, "Statistics");
			}
		}

		// Token: 0x040003A5 RID: 933
		public const string SaveDirectoryName = "Game Saves";

		// Token: 0x040003A6 RID: 934
		public const string RecordingsDirectoryName = "Recordings";

		// Token: 0x040003A7 RID: 935
		public const string StatisticsDirectoryName = "Statistics";
	}
}
