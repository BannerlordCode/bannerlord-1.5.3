using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000062 RID: 98
	public static class ManagedDllFolder
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x00008810 File Offset: 0x00006A10
		public static string Name
		{
			get
			{
				if (!string.IsNullOrEmpty(ManagedDllFolder._overridenFolder))
				{
					return ManagedDllFolder._overridenFolder;
				}
				if (ApplicationPlatform.CurrentPlatform == Platform.Orbis)
				{
					return "/app0/";
				}
				if (ApplicationPlatform.CurrentPlatform == Platform.Durango)
				{
					return "/";
				}
				return "";
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00008845 File Offset: 0x00006A45
		public static void OverrideManagedDllFolder(string overridenFolder)
		{
			ManagedDllFolder._overridenFolder = overridenFolder;
		}

		// Token: 0x04000124 RID: 292
		private static string _overridenFolder;
	}
}
