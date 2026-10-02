using System;
using System.Linq;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000077 RID: 119
	public class MissionCaravanOrVillagerTacticsHandler : MissionLogic
	{
		// Token: 0x060004D3 RID: 1235 RVA: 0x0001EF7C File Offset: 0x0001D17C
		public override void EarlyStart()
		{
			foreach (Team team in Mission.Current.Teams)
			{
				if (team.HasTeamAi)
				{
					if (!MapEvent.PlayerMapEvent.PartiesOnSide(team.Side).Any<MapEventParty>((MapEventParty p) => p.Party.IsMobile && p.Party.MobileParty.IsCaravan))
					{
						if (MapEvent.PlayerMapEvent.MapEventSettlement != null)
						{
							continue;
						}
						if (!MapEvent.PlayerMapEvent.PartiesOnSide(team.Side).Any<MapEventParty>((MapEventParty p) => p.Party.IsMobile && p.Party.MobileParty.IsVillager))
						{
							continue;
						}
					}
					team.AddTacticOption(new TacticDefensiveLine(team));
				}
			}
		}
	}
}
