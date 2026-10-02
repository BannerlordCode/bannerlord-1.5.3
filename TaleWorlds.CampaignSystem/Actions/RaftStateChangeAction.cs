using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004E9 RID: 1257
	public class RaftStateChangeAction
	{
		// Token: 0x06004DD2 RID: 19922 RVA: 0x00189990 File Offset: 0x00187B90
		private static void ApplyInternal(MobileParty mobileParty, bool isRaftState)
		{
			mobileParty.IsInRaftState = isRaftState;
			if (mobileParty.Army != null)
			{
				mobileParty.Army = null;
			}
			if (isRaftState)
			{
				mobileParty.MovePartyToTheClosestLand();
				mobileParty.Ai.DisableAi();
				if (mobileParty.Party.PrisonRoster.TotalManCount > 0)
				{
					if (mobileParty.Party.PrisonRoster.TotalHeroes > 0)
					{
						foreach (TroopRosterElement troopRosterElement in mobileParty.PrisonRoster.GetTroopRoster())
						{
							if (troopRosterElement.Character.IsHero)
							{
								EndCaptivityAction.ApplyByEscape(troopRosterElement.Character.HeroObject, null, true);
							}
						}
					}
					mobileParty.PrisonRoster.Clear();
				}
			}
			else
			{
				mobileParty.Ai.EnableAi();
				mobileParty.RecalculateShortTermBehavior();
				mobileParty.Ai.DefaultBehaviorNeedsUpdate = true;
				mobileParty.Ai.RethinkAtNextHourlyTick = true;
			}
			CampaignEventDispatcher.Instance.OnMobilePartyRaftStateChanged(mobileParty);
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x00189A98 File Offset: 0x00187C98
		public static void ActivateRaftStateForParty(MobileParty mobileParty)
		{
			RaftStateChangeAction.ApplyInternal(mobileParty, true);
		}

		// Token: 0x06004DD4 RID: 19924 RVA: 0x00189AA1 File Offset: 0x00187CA1
		public static void DeactivateRaftStateForParty(MobileParty mobileParty)
		{
			RaftStateChangeAction.ApplyInternal(mobileParty, false);
		}
	}
}
