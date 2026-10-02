using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Encounters
{
	// Token: 0x020002FE RID: 766
	public class CastleEncounter : LocationEncounter
	{
		// Token: 0x06002AD4 RID: 10964 RVA: 0x000B183A File Offset: 0x000AFA3A
		public CastleEncounter(Settlement settlement)
			: base(settlement)
		{
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x000B1844 File Offset: 0x000AFA44
		public override IMission CreateAndOpenMissionController(Location nextLocation, Location previousLocation = null, CharacterObject talkToChar = null, string playerSpecialSpawnTag = null)
		{
			int num = base.Settlement.Town.GetWallLevel();
			IMission mission;
			if (nextLocation.StringId == "center")
			{
				mission = CampaignMission.OpenCastleCourtyardMission(nextLocation.GetSceneName(num), nextLocation, talkToChar, num);
			}
			else if (nextLocation.StringId == "lordshall")
			{
				nextLocation.GetSceneName(num);
				mission = CampaignMission.OpenIndoorMission(nextLocation.GetSceneName(num), num, nextLocation, talkToChar);
			}
			else
			{
				num = Campaign.Current.Models.LocationModel.GetSettlementUpgradeLevel(PlayerEncounter.LocationEncounter);
				nextLocation.GetSceneName(num);
				mission = CampaignMission.OpenIndoorMission(nextLocation.GetSceneName(num), num, nextLocation, talkToChar);
			}
			return mission;
		}
	}
}
