using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Encounters
{
	// Token: 0x02000304 RID: 772
	public class VillageEncounter : LocationEncounter
	{
		// Token: 0x06002AE9 RID: 10985 RVA: 0x000B1C0E File Offset: 0x000AFE0E
		public VillageEncounter(Settlement settlement)
			: base(settlement)
		{
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x000B1C18 File Offset: 0x000AFE18
		public override IMission CreateAndOpenMissionController(Location nextLocation, Location previousLocation = null, CharacterObject talkToChar = null, string playerSpecialSpawnTag = null)
		{
			IMission mission = null;
			if (nextLocation.StringId == "village_center")
			{
				mission = CampaignMission.OpenVillageMission(nextLocation.GetSceneName(1), nextLocation, talkToChar);
			}
			return mission;
		}
	}
}
