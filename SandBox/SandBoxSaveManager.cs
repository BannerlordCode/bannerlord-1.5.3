using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;

namespace SandBox
{
	// Token: 0x02000027 RID: 39
	public class SandBoxSaveManager : ISaveManager
	{
		// Token: 0x06000127 RID: 295 RVA: 0x00007FE8 File Offset: 0x000061E8
		public int GetAutoSaveInterval()
		{
			return BannerlordConfig.AutoSaveInterval;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007FEF File Offset: 0x000061EF
		public bool IsAutoSaveDisabled()
		{
			return BannerlordConfig.AutoSaveInterval == -1;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00007FF9 File Offset: 0x000061F9
		public void OnSaveOver(bool isSuccessful, string newSaveGameName)
		{
			if (isSuccessful)
			{
				BannerlordConfig.LatestSaveGameName = newSaveGameName;
				BannerlordConfig.Save();
			}
		}
	}
}
