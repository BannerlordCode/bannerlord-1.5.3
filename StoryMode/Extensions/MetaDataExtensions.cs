using System;
using TaleWorlds.SaveSystem;

namespace StoryMode.Extensions
{
	// Token: 0x02000059 RID: 89
	public static class MetaDataExtensions
	{
		// Token: 0x0600058D RID: 1421 RVA: 0x000201AC File Offset: 0x0001E3AC
		public static bool HasStoryMode(this MetaData metaData)
		{
			bool flag = false;
			string text;
			if (metaData != null && metaData.TryGetValue("Modules", out text))
			{
				string[] array = text.Split(new char[] { ';' });
				for (int i = 0; i < array.Length; i++)
				{
					if (string.Equals(array[i], "StoryMode", StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00020204 File Offset: 0x0001E404
		public static bool AreAchievementsDisabled(this MetaData metaData)
		{
			string text;
			int num;
			return metaData != null && metaData.TryGetValue("AchievementsDisabled", out text) && int.TryParse(text, out num) && num == 1;
		}
	}
}
