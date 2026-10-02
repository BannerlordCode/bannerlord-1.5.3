using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000025 RID: 37
	public static class ConfigurationManager
	{
		// Token: 0x060000EA RID: 234 RVA: 0x00005418 File Offset: 0x00003618
		public static void SetConfigurationManager(IConfigurationManager configurationManager)
		{
			ConfigurationManager._configurationManager = configurationManager;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005420 File Offset: 0x00003620
		public static string GetAppSettings(string name)
		{
			if (ConfigurationManager._configurationManager != null)
			{
				return ConfigurationManager._configurationManager.GetAppSettings(name);
			}
			return null;
		}

		// Token: 0x04000078 RID: 120
		private static IConfigurationManager _configurationManager;
	}
}
