using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C3 RID: 1219
	public static class ChangeKingdomAction
	{
		// Token: 0x06004D1D RID: 19741 RVA: 0x00186084 File Offset: 0x00184284
		private static void ApplyInternal(Clan clan, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, CampaignTime shouldStayInKingdomUntil, int awardMultiplier = 0, bool byRebellion = false, bool showNotification = true)
		{
			Kingdom kingdom = clan.Kingdom;
			clan.DebtToKingdom = 0;
			if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdom || detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinAsMercenary || detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdomByDefection)
			{
				clan.ShouldStayInKingdomUntil = shouldStayInKingdomUntil;
				FactionHelper.AdjustFactionStancesForClanJoiningKingdom(clan, newKingdom);
			}
			else
			{
				clan.ShouldStayInKingdomUntil = CampaignTime.Zero;
			}
			if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdom || detail == ChangeKingdomAction.ChangeKingdomActionDetail.CreateKingdom || detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdomByDefection)
			{
				if (clan.IsUnderMercenaryService)
				{
					EndMercenaryServiceAction.EndByDefault(clan);
				}
				if (kingdom != null)
				{
					clan.ClanLeaveKingdom(!byRebellion);
				}
				if (newKingdom != null && detail == ChangeKingdomAction.ChangeKingdomActionDetail.CreateKingdom)
				{
					ChangeRulingClanAction.Apply(newKingdom, clan);
				}
				clan.Kingdom = newKingdom;
			}
			else if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinAsMercenary)
			{
				StartMercenaryServiceAction.ApplyByDefault(clan, newKingdom, awardMultiplier);
			}
			else if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveKingdom || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveAsMercenary || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveByClanDestruction || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveByKingdomDestruction)
			{
				clan.Kingdom = null;
				bool flag = false;
				if (clan.IsUnderMercenaryService)
				{
					flag = true;
					EndMercenaryServiceAction.EndByLeavingKingdom(clan);
				}
				if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion)
				{
					DeclareWarAction.ApplyByRebellion(kingdom, clan);
					using (List<IFaction>.Enumerator enumerator = kingdom.FactionsAtWarWith.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IFaction faction = enumerator.Current;
							if (faction != clan && !clan.IsAtWarWith(faction))
							{
								DeclareWarAction.ApplyByDefault(clan, faction);
							}
						}
						goto IL_0298;
					}
				}
				if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveKingdom)
				{
					using (List<Settlement>.Enumerator enumerator2 = new List<Settlement>(clan.Settlements).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Settlement settlement = enumerator2.Current;
							ChangeOwnerOfSettlementAction.ApplyByLeaveFaction(kingdom.Leader, settlement);
							foreach (Hero hero in new List<Hero>(settlement.HeroesWithoutParty))
							{
								if (hero.CurrentSettlement != null && hero.Clan == clan)
								{
									if (hero.PartyBelongedTo != null)
									{
										LeaveSettlementAction.ApplyForParty(hero.PartyBelongedTo);
										EnterSettlementAction.ApplyForParty(hero.PartyBelongedTo, clan.Leader.HomeSettlement);
									}
									else
									{
										LeaveSettlementAction.ApplyForCharacterOnly(hero);
										EnterSettlementAction.ApplyForCharacterOnly(hero, clan.Leader.HomeSettlement);
									}
								}
							}
						}
						goto IL_0298;
					}
				}
				if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveByKingdomDestruction)
				{
					if (flag)
					{
						using (List<IFaction>.Enumerator enumerator = kingdom.FactionsAtWarWith.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								IFaction faction2 = enumerator.Current;
								if (clan != faction2 && !Campaign.Current.Models.DiplomacyModel.IsAtConstantWar(clan, faction2))
								{
									MakePeaceAction.Apply(clan, faction2);
								}
							}
							goto IL_0298;
						}
					}
					foreach (IFaction faction3 in kingdom.FactionsAtWarWith)
					{
						if (clan != faction3 && !clan.GetStanceWith(faction3).IsAtWar)
						{
							DeclareWarAction.ApplyByDefault(clan, faction3);
						}
					}
				}
			}
			IL_0298:
			if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveAsMercenary || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveKingdom)
			{
				foreach (IFaction faction4 in clan.FactionsAtWarWith.ToList<IFaction>())
				{
					if (clan != faction4 && !Campaign.Current.Models.DiplomacyModel.IsAtConstantWar(clan, faction4))
					{
						MakePeaceAction.Apply(clan, faction4);
						FactionHelper.FinishAllRelatedHostileActionsOfFactionToFaction(clan, faction4);
						FactionHelper.FinishAllRelatedHostileActionsOfFactionToFaction(faction4, clan);
					}
				}
				ChangeKingdomAction.CheckIfPartyIconIsDirty(clan, kingdom);
			}
			foreach (WarPartyComponent warPartyComponent in clan.WarPartyComponents)
			{
				if (warPartyComponent.MobileParty.MapEvent == null)
				{
					warPartyComponent.MobileParty.SetMoveModeHold();
				}
			}
			CampaignEventDispatcher.Instance.OnClanChangedKingdom(clan, kingdom, newKingdom, detail, showNotification);
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x00186458 File Offset: 0x00184658
		public static void ApplyByJoinToKingdom(Clan clan, Kingdom newKingdom, CampaignTime shouldStayInKingdomUntil = default(CampaignTime), bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdom, shouldStayInKingdomUntil, 0, false, showNotification);
		}

		// Token: 0x06004D1F RID: 19743 RVA: 0x00186466 File Offset: 0x00184666
		public static void ApplyByJoinToKingdomByDefection(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, CampaignTime shouldStayInKingdomUntil = default(CampaignTime), bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdomByDefection, shouldStayInKingdomUntil, 0, false, showNotification);
			CampaignEventDispatcher.Instance.OnClanDefected(clan, oldKingdom, newKingdom);
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x00186482 File Offset: 0x00184682
		public static void ApplyByCreateKingdom(Clan clan, Kingdom newKingdom, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail.CreateKingdom, CampaignTime.Zero, 0, false, showNotification);
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x00186494 File Offset: 0x00184694
		public static void ApplyByLeaveByKingdomDestruction(Clan clan, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, null, ChangeKingdomAction.ChangeKingdomActionDetail.LeaveByKingdomDestruction, CampaignTime.Zero, 0, false, showNotification);
		}

		// Token: 0x06004D22 RID: 19746 RVA: 0x001864A6 File Offset: 0x001846A6
		public static void ApplyByLeaveKingdom(Clan clan, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, null, ChangeKingdomAction.ChangeKingdomActionDetail.LeaveKingdom, CampaignTime.Zero, 0, false, showNotification);
		}

		// Token: 0x06004D23 RID: 19747 RVA: 0x001864B8 File Offset: 0x001846B8
		public static void ApplyByLeaveWithRebellionAgainstKingdom(Clan clan, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, null, ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion, CampaignTime.Zero, 0, false, showNotification);
		}

		// Token: 0x06004D24 RID: 19748 RVA: 0x001864CA File Offset: 0x001846CA
		public static void ApplyByJoinFactionAsMercenary(Clan clan, Kingdom newKingdom, CampaignTime shouldStayInKingdomUntil = default(CampaignTime), int awardMultiplier = 50, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail.JoinAsMercenary, shouldStayInKingdomUntil, awardMultiplier, false, showNotification);
		}

		// Token: 0x06004D25 RID: 19749 RVA: 0x001864D9 File Offset: 0x001846D9
		public static void ApplyByLeaveKingdomAsMercenary(Clan mercenaryClan, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(mercenaryClan, null, ChangeKingdomAction.ChangeKingdomActionDetail.LeaveAsMercenary, CampaignTime.Zero, 0, false, showNotification);
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x001864EB File Offset: 0x001846EB
		public static void ApplyByLeaveKingdomByClanDestruction(Clan clan, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, null, ChangeKingdomAction.ChangeKingdomActionDetail.LeaveByClanDestruction, CampaignTime.Zero, 0, false, showNotification);
		}

		// Token: 0x06004D27 RID: 19751 RVA: 0x00186500 File Offset: 0x00184700
		private static void CheckIfPartyIconIsDirty(Clan clan, Kingdom oldKingdom)
		{
			IFaction faction;
			if (clan.Kingdom == null)
			{
				faction = clan;
			}
			else
			{
				IFaction kingdom = clan.Kingdom;
				faction = kingdom;
			}
			IFaction faction2 = faction;
			IFaction faction3 = oldKingdom ?? clan;
			foreach (MobileParty mobileParty in MobileParty.All)
			{
				if (mobileParty.IsVisible && ((mobileParty.Party.Owner != null && mobileParty.Party.Owner.Clan == clan) || (clan == Clan.PlayerClan && ((!FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, faction2) && FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, faction3)) || (FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, faction2) && !FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, faction3))))))
				{
					mobileParty.Party.SetVisualAsDirty();
				}
			}
			foreach (Settlement settlement in clan.Settlements)
			{
				settlement.Party.SetVisualAsDirty();
			}
		}

		// Token: 0x04001594 RID: 5524
		public const float PotentialSettlementsPerNobleEffect = 0.2f;

		// Token: 0x04001595 RID: 5525
		public const float NewGainedFiefsValueForKingdomConstant = 0.1f;

		// Token: 0x04001596 RID: 5526
		public const float LordsUnitStrengthValue = 20f;

		// Token: 0x04001597 RID: 5527
		public const float MercenaryUnitStrengthValue = 5f;

		// Token: 0x04001598 RID: 5528
		public const float MinimumNeededGoldForRecruitingMercenaries = 20000f;

		// Token: 0x020008DA RID: 2266
		public enum ChangeKingdomActionDetail
		{
			// Token: 0x0400264B RID: 9803
			JoinAsMercenary,
			// Token: 0x0400264C RID: 9804
			JoinKingdom,
			// Token: 0x0400264D RID: 9805
			JoinKingdomByDefection,
			// Token: 0x0400264E RID: 9806
			LeaveKingdom,
			// Token: 0x0400264F RID: 9807
			LeaveWithRebellion,
			// Token: 0x04002650 RID: 9808
			LeaveAsMercenary,
			// Token: 0x04002651 RID: 9809
			LeaveByClanDestruction,
			// Token: 0x04002652 RID: 9810
			CreateKingdom,
			// Token: 0x04002653 RID: 9811
			LeaveByKingdomDestruction
		}
	}
}
