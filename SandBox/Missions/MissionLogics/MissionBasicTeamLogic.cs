using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000076 RID: 118
	public class MissionBasicTeamLogic : MissionLogic
	{
		// Token: 0x060004CF RID: 1231 RVA: 0x0001EDA9 File Offset: 0x0001CFA9
		public override void EarlyStart()
		{
			base.EarlyStart();
			this.InitializeTeams(true);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001EDB8 File Offset: 0x0001CFB8
		private void GetTeamColor(BattleSideEnum side, bool isPlayerAttacker, out uint teamColor1, out uint teamColor2)
		{
			teamColor1 = uint.MaxValue;
			teamColor2 = uint.MaxValue;
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				if ((isPlayerAttacker && side == BattleSideEnum.Attacker) || (!isPlayerAttacker && side == BattleSideEnum.Defender))
				{
					teamColor1 = Hero.MainHero.MapFaction.Color;
					teamColor2 = Hero.MainHero.MapFaction.Color2;
					return;
				}
				if (MobileParty.MainParty.MapEvent != null)
				{
					if (MobileParty.MainParty.MapEvent.MapEventSettlement != null)
					{
						teamColor1 = MobileParty.MainParty.MapEvent.MapEventSettlement.MapFaction.Color;
						teamColor2 = MobileParty.MainParty.MapEvent.MapEventSettlement.MapFaction.Color2;
						return;
					}
					teamColor1 = MobileParty.MainParty.MapEvent.GetLeaderParty(side).MapFaction.Color;
					teamColor2 = MobileParty.MainParty.MapEvent.GetLeaderParty(side).MapFaction.Color2;
				}
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001EEA0 File Offset: 0x0001D0A0
		private void InitializeTeams(bool isPlayerAttacker = true)
		{
			if (!base.Mission.Teams.IsEmpty<Team>())
			{
				throw new MBIllegalValueException("Number of teams is not 0.");
			}
			uint num;
			uint num2;
			this.GetTeamColor(BattleSideEnum.Defender, isPlayerAttacker, out num, out num2);
			uint num3;
			uint num4;
			this.GetTeamColor(BattleSideEnum.Attacker, isPlayerAttacker, out num3, out num4);
			base.Mission.Teams.Add(BattleSideEnum.Defender, num, num2, null, true, false, true);
			base.Mission.Teams.Add(BattleSideEnum.Attacker, num3, num4, null, true, false, true);
			if (isPlayerAttacker)
			{
				base.Mission.Teams.Add(BattleSideEnum.Attacker, uint.MaxValue, uint.MaxValue, null, true, false, true);
				base.Mission.PlayerTeam = base.Mission.AttackerTeam;
				return;
			}
			base.Mission.Teams.Add(BattleSideEnum.Defender, uint.MaxValue, uint.MaxValue, null, true, false, true);
			base.Mission.PlayerTeam = base.Mission.DefenderTeam;
		}
	}
}
