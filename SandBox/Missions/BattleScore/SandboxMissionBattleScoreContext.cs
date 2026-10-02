using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.BattleScore;

namespace SandBox.Missions.BattleScore
{
	// Token: 0x020000A1 RID: 161
	public class SandboxMissionBattleScoreContext : BattleScoreContext
	{
		// Token: 0x060006AA RID: 1706 RVA: 0x0002CE7B File Offset: 0x0002B07B
		public SandboxMissionBattleScoreContext(Mission mission)
		{
			this._mission = mission;
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x0002CE8A File Offset: 0x0002B08A
		public override bool IsPowerComparisonRelevant
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0002CE90 File Offset: 0x0002B090
		public override Banner GetAttackerBanner()
		{
			if (Campaign.Current == null)
			{
				return null;
			}
			MapEvent battle = PlayerEncounter.Battle;
			Banner banner;
			if (battle == null)
			{
				banner = null;
			}
			else
			{
				MapEventSide attackerSide = battle.AttackerSide;
				banner = ((attackerSide != null) ? attackerSide.LeaderParty.Banner : null);
			}
			Banner banner2;
			if ((banner2 = banner) == null)
			{
				Mission mission = this._mission;
				if (mission == null)
				{
					return null;
				}
				banner2 = mission.Teams.Attacker.Banner;
			}
			return banner2;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002CEE8 File Offset: 0x0002B0E8
		public override Banner GetDefenderBanner()
		{
			if (Campaign.Current == null)
			{
				return null;
			}
			MapEvent battle = PlayerEncounter.Battle;
			Banner banner;
			if (battle == null)
			{
				banner = null;
			}
			else
			{
				MapEventSide defenderSide = battle.DefenderSide;
				banner = ((defenderSide != null) ? defenderSide.LeaderParty.Banner : null);
			}
			Banner banner2;
			if ((banner2 = banner) == null)
			{
				Mission mission = this._mission;
				if (mission == null)
				{
					return null;
				}
				banner2 = mission.Teams.Defender.Banner;
			}
			return banner2;
		}

		// Token: 0x04000392 RID: 914
		private readonly Mission _mission;
	}
}
