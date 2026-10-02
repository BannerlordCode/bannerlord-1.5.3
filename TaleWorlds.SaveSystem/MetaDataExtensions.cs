using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000013 RID: 19
	public static class MetaDataExtensions
	{
		// Token: 0x0600005D RID: 93 RVA: 0x000034F0 File Offset: 0x000016F0
		public static ApplicationVersion GetApplicationVersion(this MetaData metaData)
		{
			string text = ((metaData != null) ? metaData["ApplicationVersion"] : null);
			if (text == null)
			{
				return ApplicationVersion.Empty;
			}
			return ApplicationVersion.FromString(text, 0);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003520 File Offset: 0x00001720
		public static ApplicationVersion GetNewGameVersion(this MetaData metaData)
		{
			string text = ((metaData != null) ? metaData["NewGameVersion"] : null);
			if (string.IsNullOrEmpty(text))
			{
				return ApplicationVersion.Empty;
			}
			return ApplicationVersion.FromString(text, 0);
		}
	}
}
