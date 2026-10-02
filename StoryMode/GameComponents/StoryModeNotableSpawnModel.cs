using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;

namespace StoryMode.GameComponents
{
	// Token: 0x02000046 RID: 70
	public class StoryModeNotableSpawnModel : NotableSpawnModel
	{
		// Token: 0x0600045D RID: 1117 RVA: 0x00019619 File Offset: 0x00017819
		public override int GetTargetNotableCountForSettlement(Settlement settlement, Occupation occupation)
		{
			if (!StoryModeManager.Current.MainStoryLine.TutorialPhase.IsCompleted && settlement.StringId == "village_ES3_2")
			{
				return 0;
			}
			return base.BaseModel.GetTargetNotableCountForSettlement(settlement, occupation);
		}
	}
}
