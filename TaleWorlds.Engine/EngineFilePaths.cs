using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000044 RID: 68
	public static class EngineFilePaths
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x00003F67 File Offset: 0x00002167
		public static PlatformDirectoryPath ConfigsPath
		{
			get
			{
				return new PlatformDirectoryPath(PlatformFileType.User, "Configs");
			}
		}

		// Token: 0x04000056 RID: 86
		public const string ConfigsDirectoryName = "Configs";
	}
}
