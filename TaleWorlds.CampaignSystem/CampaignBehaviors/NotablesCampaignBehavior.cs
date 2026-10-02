using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200043F RID: 1087
	public class NotablesCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060045EF RID: 17903 RVA: 0x00153AE4 File Offset: 0x00151CE4
		public NotablesCampaignBehavior()
		{
			this._settlementPassedDaysForWeeklyTick = new Dictionary<Settlement, int>();
		}

		// Token: 0x060045F0 RID: 17904 RVA: 0x00153AF8 File Offset: 0x00151CF8
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(this.WeeklyTick));
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.CanHeroDieEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.CanHeroDie));
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x00153BD4 File Offset: 0x00151DD4
		private void OnHeroCreated(Hero hero, bool isBornNaturally)
		{
			if (hero.Occupation == Occupation.GangLeader || hero.Occupation == Occupation.Artisan || hero.Occupation == Occupation.RuralNotable || hero.Occupation == Occupation.Merchant || hero.Occupation == Occupation.Headman)
			{
				hero.ChangeState(Hero.CharacterStates.Active);
				EnterSettlementAction.ApplyForCharacterOnly(hero, hero.HomeSettlement);
				GiveGoldAction.ApplyBetweenCharacters(null, hero, 10000, true);
				CharacterObject template = hero.Template;
				bool flag;
				if (template == null)
				{
					flag = null != null;
				}
				else
				{
					Hero heroObject = template.HeroObject;
					flag = ((heroObject != null) ? heroObject.Clan : null) != null;
				}
				if (flag && hero.Template.HeroObject.Clan.IsMinorFaction)
				{
					hero.SupporterOf = hero.Template.HeroObject.Clan;
					return;
				}
				hero.SupporterOf = HeroHelper.GetRandomClanForNotable(hero);
			}
		}

		// Token: 0x060045F2 RID: 17906 RVA: 0x00153C90 File Offset: 0x00151E90
		private void WeeklyTick()
		{
			foreach (Hero hero in Hero.DeadOrDisabledHeroes.ToList<Hero>())
			{
				if (hero.IsDead && hero.IsNotable && hero.DeathDay.ElapsedDaysUntilNow >= 7f)
				{
					Campaign.Current.CampaignObjectManager.UnregisterDeadHero(hero);
				}
			}
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x00153D18 File Offset: 0x00151F18
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.WeeklyTick();
		}

		// Token: 0x060045F4 RID: 17908 RVA: 0x00153D20 File Offset: 0x00151F20
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Settlement, int>>("_settlementPassedDaysForWeeklyTick", ref this._settlementPassedDaysForWeeklyTick);
		}

		// Token: 0x060045F5 RID: 17909 RVA: 0x00153D34 File Offset: 0x00151F34
		public void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
			this.SpawnNotablesAtGameStart();
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x00153D3C File Offset: 0x00151F3C
		private void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
		{
			if (hero.IsNotable)
			{
				using (List<CaravanPartyComponent>.Enumerator enumerator = hero.OwnedCaravans.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Party.MapEvent != null)
						{
							result = false;
							break;
						}
					}
				}
			}
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x00153DA0 File Offset: 0x00151FA0
		private void DetermineRelation(Hero hero1, Hero hero2, float randomValue, float chanceOfConflict)
		{
			float num = 0.3f;
			if (randomValue < num)
			{
				int num2 = (int)((num - randomValue) * (num - randomValue) / (num * num) * 100f);
				if (num2 > 0)
				{
					hero1.SetPersonalRelation(hero2, num2);
					return;
				}
			}
			else if (randomValue > 1f - chanceOfConflict)
			{
				int num3 = -(int)((randomValue - (1f - chanceOfConflict)) * (randomValue - (1f - chanceOfConflict)) / (chanceOfConflict * chanceOfConflict) * 100f);
				if (num3 < 0)
				{
					hero1.SetPersonalRelation(hero2, num3);
				}
			}
		}

		// Token: 0x060045F8 RID: 17912 RVA: 0x00153E14 File Offset: 0x00152014
		private void SetInitialRelationsBetweenNotablesAndLords()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				for (int i = 0; i < settlement.Notables.Count; i++)
				{
					Hero hero = settlement.Notables[i];
					foreach (Hero hero2 in settlement.MapFaction.AliveLords.Union<Hero>(settlement.MapFaction.DeadLords))
					{
						if ((!hero2.IsDead || !(hero2.DeathDay < hero.BirthDay)) && (!hero.IsDead || !(hero.DeathDay < hero2.BirthDay)) && hero2 != hero && hero2 == hero2.Clan.Leader && hero2.MapFaction == settlement.MapFaction)
						{
							float num = 0f;
							for (int j = 0; j < 4; j++)
							{
								num += MBRandom.RandomFloat * 2f - 1f;
							}
							num = MBMath.ClampFloat(num * 30f, -100f, 100f);
							int num2 = HeroHelper.NPCPersonalityClashWithNPC(hero, hero2);
							if (num2 == 0)
							{
								hero.SetPersonalRelation(hero2, MathF.Round(num));
							}
							else if (num2 < 0)
							{
								hero.SetPersonalRelation(hero2, MathF.Abs(MathF.Round(num)) * -1);
							}
							else
							{
								hero.SetPersonalRelation(hero2, MathF.Abs(MathF.Round(num)));
							}
						}
					}
					for (int k = i + 1; k < settlement.Notables.Count; k++)
					{
						Hero hero3 = settlement.Notables[k];
						float num3 = 0f;
						for (int l = 0; l < 4; l++)
						{
							num3 += MBRandom.RandomFloat * 2f - 1f;
						}
						num3 = MBMath.ClampFloat(num3 * 30f, -100f, 100f);
						int num4 = HeroHelper.NPCPersonalityClashWithNPC(hero, hero3);
						if (num4 == 0)
						{
							hero.SetPersonalRelation(hero3, MathF.Round(num3));
						}
						else if (num4 < 0)
						{
							hero.SetPersonalRelation(hero3, MathF.Abs(MathF.Round(num3)) * -1);
						}
						else
						{
							hero.SetPersonalRelation(hero3, MathF.Abs(MathF.Round(num3)));
						}
					}
				}
			}
		}

		// Token: 0x060045F9 RID: 17913 RVA: 0x001540C8 File Offset: 0x001522C8
		public void OnNewGameCreatedPartialFollowUp(CampaignGameStarter starter, int i)
		{
			if (i == 1)
			{
				this.SetInitialRelationsBetweenNotablesAndLords();
				int num = 50;
				for (int j = 0; j < num; j++)
				{
					foreach (Hero hero in Hero.AllAliveHeroes)
					{
						if (hero.IsNotable)
						{
							this.UpdateNotableSupport(hero);
						}
					}
				}
			}
		}

		// Token: 0x060045FA RID: 17914 RVA: 0x0015413C File Offset: 0x0015233C
		private void DailyTickSettlement(Settlement settlement)
		{
			if (this._settlementPassedDaysForWeeklyTick.ContainsKey(settlement))
			{
				Dictionary<Settlement, int> settlementPassedDaysForWeeklyTick = this._settlementPassedDaysForWeeklyTick;
				int num = settlementPassedDaysForWeeklyTick[settlement];
				settlementPassedDaysForWeeklyTick[settlement] = num + 1;
				if (this._settlementPassedDaysForWeeklyTick[settlement] == CampaignTime.DaysInWeek)
				{
					SettlementHelper.SpawnNotablesIfNeeded(settlement);
					this._settlementPassedDaysForWeeklyTick[settlement] = 0;
					return;
				}
			}
			else
			{
				this._settlementPassedDaysForWeeklyTick.Add(settlement, 0);
			}
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x001541A4 File Offset: 0x001523A4
		private void UpdateNotableRelations(Hero notable)
		{
			foreach (Clan clan in Clan.All)
			{
				if (clan != Clan.PlayerClan && clan.Leader != null && !clan.IsEliminated)
				{
					int relation = notable.GetRelation(clan.Leader);
					if (relation > 0)
					{
						float num = (float)relation / 1000f;
						if (MBRandom.RandomFloat < num)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(notable, clan.Leader, -20, true);
						}
					}
					else if (relation < 0)
					{
						float num2 = (float)(-(float)relation) / 1000f;
						if (MBRandom.RandomFloat < num2)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(notable, clan.Leader, 20, true);
						}
					}
				}
			}
		}

		// Token: 0x060045FC RID: 17916 RVA: 0x00154264 File Offset: 0x00152464
		private void UpdateNotableSupport(Hero notable)
		{
			if (notable.SupporterOf == null)
			{
				using (IEnumerator<Clan> enumerator = Clan.NonBanditFactions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Clan clan = enumerator.Current;
						if (clan.Leader != null && clan != Clan.PlayerClan)
						{
							int relation = notable.GetRelation(clan.Leader);
							if (relation > 50)
							{
								float num = (float)(relation - 50) / 2000f;
								if (MBRandom.RandomFloat < num)
								{
									notable.SupporterOf = clan;
								}
							}
						}
					}
					return;
				}
			}
			int relation2 = notable.GetRelation(notable.SupporterOf.Leader);
			if (relation2 < 0 || MBRandom.RandomFloat < (50f - (float)relation2) / 500f)
			{
				bool flag = notable.SupporterOf == Clan.PlayerClan;
				notable.SupporterOf = null;
				if (flag)
				{
					TextObject textObject = new TextObject("{=aaOIjHeP}{NOTABLE.NAME} no longer supports your clan as your relationship deteriorated too much.", null);
					textObject.SetCharacterProperties("NOTABLE", notable.CharacterObject, false);
					InformationManager.DisplayMessage(new InformationMessage(textObject.ToString(), new Color(0f, 1f, 0f, 1f)));
				}
			}
		}

		// Token: 0x060045FD RID: 17917 RVA: 0x0015437C File Offset: 0x0015257C
		private void DailyTickHero(Hero hero)
		{
			if (hero.IsNotable && hero.CurrentSettlement != null)
			{
				if (MBRandom.RandomFloat < 0.01f)
				{
					this.UpdateNotableRelations(hero);
				}
				this.UpdateNotableSupport(hero);
				this.ManageCaravanExpensesOfNotable(hero);
				this.CheckAndMakeNotableDisappear(hero);
			}
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x001543B8 File Offset: 0x001525B8
		private void CheckAndMakeNotableDisappear(Hero notable)
		{
			if (notable.OwnedWorkshops.IsEmpty<Workshop>() && notable.OwnedCaravans.IsEmpty<CaravanPartyComponent>() && notable.OwnedAlleys.IsEmpty<Alley>() && notable.CanDie(KillCharacterAction.KillCharacterActionDetail.Lost) && notable.CanHaveCampaignIssues() && notable.Power < (float)Campaign.Current.Models.NotablePowerModel.NotableDisappearPowerLimit)
			{
				float randomFloat = MBRandom.RandomFloat;
				float notableDisappearProbability = this.GetNotableDisappearProbability(notable);
				if (randomFloat < notableDisappearProbability)
				{
					KillCharacterAction.ApplyByRemove(notable, false, true);
					IssueBase issue = notable.Issue;
					if (issue == null)
					{
						return;
					}
					issue.CompleteIssueWithAiLord(notable.CurrentSettlement.OwnerClan.Leader);
				}
			}
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x00154458 File Offset: 0x00152658
		private void ManageCaravanExpensesOfNotable(Hero notable)
		{
			for (int i = notable.OwnedCaravans.Count - 1; i >= 0; i--)
			{
				CaravanPartyComponent caravanPartyComponent = notable.OwnedCaravans[i];
				int totalWage = caravanPartyComponent.MobileParty.TotalWage;
				if (caravanPartyComponent.MobileParty.PartyTradeGold >= totalWage)
				{
					caravanPartyComponent.MobileParty.PartyTradeGold -= totalWage;
				}
				else
				{
					int num = MathF.Min(totalWage, notable.Gold);
					notable.Gold -= num;
				}
				if (caravanPartyComponent.MobileParty.PartyTradeGold < 5000)
				{
					int num2 = MathF.Min(5000 - caravanPartyComponent.MobileParty.PartyTradeGold, notable.Gold);
					caravanPartyComponent.MobileParty.PartyTradeGold += num2;
					notable.Gold -= num2;
				}
			}
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x00154532 File Offset: 0x00152732
		private float GetNotableDisappearProbability(Hero hero)
		{
			return ((float)Campaign.Current.Models.NotablePowerModel.NotableDisappearPowerLimit - hero.Power) / (float)Campaign.Current.Models.NotablePowerModel.NotableDisappearPowerLimit * 0.02f;
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x0015456C File Offset: 0x0015276C
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (victim.IsNotable)
			{
				if (victim.Power >= (float)Campaign.Current.Models.NotablePowerModel.NotableDisappearPowerLimit)
				{
					Hero hero = HeroCreator.CreateRelativeNotableHero(victim);
					if (victim.CurrentSettlement != null)
					{
						this.ChangeDeadNotable(victim, hero, victim.CurrentSettlement);
					}
					using (List<CaravanPartyComponent>.Enumerator enumerator = victim.OwnedCaravans.ToList<CaravanPartyComponent>().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CaravanPartyComponent caravanPartyComponent = enumerator.Current;
							CaravanPartyComponent.TransferCaravanOwnership(caravanPartyComponent.MobileParty, hero, hero.CurrentSettlement);
						}
						return;
					}
				}
				foreach (CaravanPartyComponent caravanPartyComponent2 in victim.OwnedCaravans.ToList<CaravanPartyComponent>())
				{
					DestroyPartyAction.Apply(null, caravanPartyComponent2.MobileParty);
				}
			}
		}

		// Token: 0x06004602 RID: 17922 RVA: 0x00154660 File Offset: 0x00152860
		private void ChangeDeadNotable(Hero deadNotable, Hero newNotable, Settlement notableSettlement)
		{
			EnterSettlementAction.ApplyForCharacterOnly(newNotable, notableSettlement);
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (newNotable != hero)
				{
					int relation = deadNotable.GetRelation(hero);
					if (Math.Abs(relation) >= 20 || (relation != 0 && hero.CurrentSettlement == notableSettlement))
					{
						newNotable.SetPersonalRelation(hero, relation);
					}
				}
			}
			if (deadNotable.Issue != null)
			{
				Campaign.Current.IssueManager.ChangeIssueOwner(deadNotable.Issue, newNotable);
			}
		}

		// Token: 0x06004603 RID: 17923 RVA: 0x001546FC File Offset: 0x001528FC
		private void SpawnNotablesAtGameStart()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsTown)
				{
					int targetNotableCountForSettlement = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.Artisan);
					for (int i = 0; i < targetNotableCountForSettlement; i++)
					{
						HeroCreator.CreateNotable(Occupation.Artisan, settlement);
					}
					int targetNotableCountForSettlement2 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.Merchant);
					for (int j = 0; j < targetNotableCountForSettlement2; j++)
					{
						HeroCreator.CreateNotable(Occupation.Merchant, settlement);
					}
					int targetNotableCountForSettlement3 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.GangLeader);
					for (int k = 0; k < targetNotableCountForSettlement3; k++)
					{
						HeroCreator.CreateNotable(Occupation.GangLeader, settlement);
					}
				}
				else if (settlement.IsVillage)
				{
					int targetNotableCountForSettlement4 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.RuralNotable);
					for (int l = 0; l < targetNotableCountForSettlement4; l++)
					{
						HeroCreator.CreateNotable(Occupation.RuralNotable, settlement);
					}
					int targetNotableCountForSettlement5 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.Headman);
					for (int m = 0; m < targetNotableCountForSettlement5; m++)
					{
						HeroCreator.CreateNotable(Occupation.Headman, settlement);
					}
				}
			}
		}

		// Token: 0x04001424 RID: 5156
		private const int CaravanGoldLowLimit = 5000;

		// Token: 0x04001425 RID: 5157
		private const int RemoveNotableCharacterAfterDays = 7;

		// Token: 0x04001426 RID: 5158
		private Dictionary<Settlement, int> _settlementPassedDaysForWeeklyTick;
	}
}
