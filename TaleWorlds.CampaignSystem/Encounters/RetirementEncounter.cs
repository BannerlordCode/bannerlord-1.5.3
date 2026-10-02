using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Encounters
{
	// Token: 0x02000302 RID: 770
	public class RetirementEncounter : LocationEncounter
	{
		// Token: 0x06002AE5 RID: 10981 RVA: 0x000B1AEE File Offset: 0x000AFCEE
		public RetirementEncounter(Settlement settlement)
			: base(settlement)
		{
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x000B1AF8 File Offset: 0x000AFCF8
		public override IMission CreateAndOpenMissionController(Location nextLocation, Location previousLocation = null, CharacterObject talkToChar = null, string playerSpecialSpawnTag = null)
		{
			IMission mission = null;
			if (Settlement.CurrentSettlement.SettlementComponent is RetirementSettlementComponent)
			{
				int num = (Settlement.CurrentSettlement.IsTown ? Settlement.CurrentSettlement.Town.GetWallLevel() : 1);
				mission = CampaignMission.OpenRetirementMission(nextLocation.GetSceneName(num), nextLocation, null, null, "retirement_after_player_knockedout");
			}
			return mission;
		}

		// Token: 0x04000C5D RID: 3165
		private const string UnconsciousGameMenuID = "retirement_after_player_knockedout";
	}
}
