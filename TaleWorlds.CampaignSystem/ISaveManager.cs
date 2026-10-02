using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200009F RID: 159
	public interface ISaveManager
	{
		// Token: 0x06001347 RID: 4935
		int GetAutoSaveInterval();

		// Token: 0x06001348 RID: 4936
		bool IsAutoSaveDisabled();

		// Token: 0x06001349 RID: 4937
		void OnSaveOver(bool isSuccessful, string newSaveGameName);
	}
}
