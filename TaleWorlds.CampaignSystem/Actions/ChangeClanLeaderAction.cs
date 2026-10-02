using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C0 RID: 1216
	public static class ChangeClanLeaderAction
	{
		// Token: 0x06004D13 RID: 19731 RVA: 0x00185CDC File Offset: 0x00183EDC
		private static void ApplyInternal(Clan clan, Hero newLeader = null)
		{
			Hero leader = clan.Leader;
			if (newLeader == null)
			{
				Dictionary<Hero, int> heirApparents = clan.GetHeirApparents();
				if (heirApparents.Count == 0)
				{
					return;
				}
				int highestPoint = heirApparents.OrderByDescending<KeyValuePair<Hero, int>, int>((KeyValuePair<Hero, int> h) => h.Value).FirstOrDefault<KeyValuePair<Hero, int>>().Value;
				newLeader = heirApparents.Where<KeyValuePair<Hero, int>>((KeyValuePair<Hero, int> h) => h.Value.Equals(highestPoint)).GetRandomElementInefficiently<KeyValuePair<Hero, int>>().Key;
			}
			GiveGoldAction.ApplyBetweenCharacters(leader, newLeader, leader.Gold, true);
			if (newLeader.GovernorOf != null)
			{
				ChangeGovernorAction.RemoveGovernorOf(newLeader);
			}
			if (!newLeader.IsPrisoner && !newLeader.IsFugitive && !newLeader.IsReleased && !newLeader.IsTraveling)
			{
				MobileParty mobileParty = newLeader.PartyBelongedTo;
				if (mobileParty == null)
				{
					mobileParty = MobilePartyHelper.CreateNewClanMobileParty(newLeader, clan);
				}
				if (mobileParty.LeaderHero != newLeader)
				{
					mobileParty.ChangePartyLeader(newLeader);
				}
			}
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero != newLeader)
				{
					int relationChangeAfterClanLeaderIsDead = Campaign.Current.Models.DiplomacyModel.GetRelationChangeAfterClanLeaderIsDead(leader, hero);
					int heroRelation = CharacterRelationManager.GetHeroRelation(newLeader, hero);
					newLeader.SetPersonalRelation(hero, heroRelation + relationChangeAfterClanLeaderIsDead);
				}
			}
			clan.SetLeader(newLeader);
			CampaignEventDispatcher.Instance.OnClanLeaderChanged(leader, newLeader);
		}

		// Token: 0x06004D14 RID: 19732 RVA: 0x00185E50 File Offset: 0x00184050
		public static void ApplyWithSelectedNewLeader(Clan clan, Hero newLeader)
		{
			ChangeClanLeaderAction.ApplyInternal(clan, newLeader);
		}

		// Token: 0x06004D15 RID: 19733 RVA: 0x00185E59 File Offset: 0x00184059
		public static void ApplyWithoutSelectedNewLeader(Clan clan)
		{
			ChangeClanLeaderAction.ApplyInternal(clan, null);
		}
	}
}
