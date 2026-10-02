using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200045B RID: 1115
	public class RansomOfferCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x060047DA RID: 18394 RVA: 0x00161A78 File Offset: 0x0015FC78
		private static TextObject RansomPanelTitleText
		{
			get
			{
				return new TextObject("{=ho5EndaV}Decision", null);
			}
		}

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x060047DB RID: 18395 RVA: 0x00161A85 File Offset: 0x0015FC85
		private static TextObject RansomPanelAffirmativeText
		{
			get
			{
				return new TextObject("{=Y94H6XnK}Accept", null);
			}
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x060047DC RID: 18396 RVA: 0x00161A92 File Offset: 0x0015FC92
		private static TextObject RansomPanelNegativeText
		{
			get
			{
				return new TextObject("{=cOgmdp9e}Decline", null);
			}
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x00161AA0 File Offset: 0x0015FCA0
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.OnRansomOfferedToPlayerEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnRansomOffered));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, new Action<Hero, PartyBase, IFaction, EndCaptivityDetail, bool>(this.OnHeroPrisonerReleased));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.PrisonersChangeInSettlement.AddNonSerializedListener(this, new Action<Settlement, FlattenedTroopRoster, Hero, bool>(this.OnPrisonersChangeInSettlement));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroPrisonerTaken));
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x00161B4E File Offset: 0x0015FD4E
		private void OnHeroPrisonerTaken(PartyBase party, Hero hero)
		{
			this.HandleDeclineRansomOffer(hero);
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x00161B58 File Offset: 0x0015FD58
		private void DailyTickHero(Hero hero)
		{
			if (hero.IsPrisoner && hero.Clan != null && hero.PartyBelongedToAsPrisoner != null && hero.PartyBelongedToAsPrisoner.MapFaction != null && !hero.PartyBelongedToAsPrisoner.MapFaction.IsBanditFaction && hero != Hero.MainHero && hero.Clan.AliveLords.Count > 1 && hero.MapFaction != null)
			{
				this.ConsiderRansomPrisoner(hero);
			}
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x00161BC8 File Offset: 0x0015FDC8
		private void ConsiderRansomPrisoner(Hero hero)
		{
			Clan captorClanOfPrisoner = this.GetCaptorClanOfPrisoner(hero);
			if (captorClanOfPrisoner != null)
			{
				bool flag = true;
				CampaignEventDispatcher.Instance.CanHeroBeReleased(hero, ref flag);
				if (!flag)
				{
					return;
				}
				Hero hero2 = ((hero.Clan.Leader != hero) ? hero.Clan.Leader : hero.Clan.AliveLords.Where<Hero>((Hero t) => t != hero.Clan.Leader).GetRandomElementInefficiently<Hero>());
				if (hero2 != Hero.MainHero || !hero2.IsPrisoner)
				{
					if (captorClanOfPrisoner == Clan.PlayerClan || hero.Clan == Clan.PlayerClan)
					{
						if (this._currentRansomHero == null && !MobileParty.MainParty.IsInNavalAutoTravel)
						{
							float num = ((!this._heroesWithDeclinedRansomOffers.Contains(hero)) ? 0.2f : 0.12f);
							if (MBRandom.RandomFloat < num)
							{
								float num2 = (float)new SetPrisonerFreeBarterable(hero, captorClanOfPrisoner.Leader, hero.PartyBelongedToAsPrisoner, hero2).GetUnitValueForFaction(hero.Clan) * 1.1f;
								if (num2 > 1E-05f && (float)(hero2.Gold + 1000) >= num2)
								{
									this.SetCurrentRansomHero(hero, hero2);
									StringHelpers.SetCharacterProperties("CAPTIVE_HERO", hero.CharacterObject, RansomOfferCampaignBehavior.RansomOfferDescriptionText, false);
									Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new RansomOfferMapNotification(hero, RansomOfferCampaignBehavior.RansomOfferDescriptionText));
									return;
								}
							}
						}
					}
					else if (MBRandom.RandomFloat < 0.1f)
					{
						SetPrisonerFreeBarterable setPrisonerFreeBarterable = new SetPrisonerFreeBarterable(hero, captorClanOfPrisoner.Leader, hero.PartyBelongedToAsPrisoner, hero2);
						if (setPrisonerFreeBarterable.GetValueForFaction(captorClanOfPrisoner) + setPrisonerFreeBarterable.GetValueForFaction(hero.Clan) > 0)
						{
							Campaign.Current.BarterManager.ExecuteAiBarter(captorClanOfPrisoner, hero.Clan, captorClanOfPrisoner.Leader, hero2, setPrisonerFreeBarterable);
						}
					}
				}
			}
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x00161DE4 File Offset: 0x0015FFE4
		private Clan GetCaptorClanOfPrisoner(Hero hero)
		{
			Clan clan;
			if (hero.PartyBelongedToAsPrisoner.IsMobile)
			{
				if ((hero.PartyBelongedToAsPrisoner.MobileParty.IsMilitia || hero.PartyBelongedToAsPrisoner.MobileParty.IsGarrison || hero.PartyBelongedToAsPrisoner.MobileParty.IsCaravan || hero.PartyBelongedToAsPrisoner.MobileParty.IsVillager) && hero.PartyBelongedToAsPrisoner.Owner != null)
				{
					if (hero.PartyBelongedToAsPrisoner.Owner.IsNotable)
					{
						clan = hero.PartyBelongedToAsPrisoner.Owner.CurrentSettlement.OwnerClan;
					}
					else
					{
						clan = hero.PartyBelongedToAsPrisoner.Owner.Clan;
					}
				}
				else if (hero.PartyBelongedToAsPrisoner.MobileParty.IsPatrolParty)
				{
					clan = hero.PartyBelongedToAsPrisoner.MobileParty.HomeSettlement.OwnerClan;
				}
				else
				{
					clan = hero.PartyBelongedToAsPrisoner.MobileParty.ActualClan;
				}
			}
			else
			{
				clan = hero.PartyBelongedToAsPrisoner.Settlement.OwnerClan;
			}
			return clan;
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x00161EE4 File Offset: 0x001600E4
		public void SetCurrentRansomHero(Hero hero, Hero ransomPayer = null)
		{
			this._currentRansomHero = hero;
			this._currentRansomPayer = ransomPayer;
			this._currentRansomOfferDate = ((hero != null) ? CampaignTime.Now : CampaignTime.Never);
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x00161F0C File Offset: 0x0016010C
		private void OnRansomOffered(Hero captiveHero)
		{
			Clan captorClanOfPrisoner = this.GetCaptorClanOfPrisoner(captiveHero);
			Clan clan = ((captiveHero.Clan == Clan.PlayerClan) ? captorClanOfPrisoner : captiveHero.Clan);
			Hero ransomPayer = ((captiveHero.Clan.Leader != captiveHero) ? captiveHero.Clan.Leader : captiveHero.Clan.AliveLords.Where<Hero>((Hero t) => t != captiveHero.Clan.Leader).GetRandomElementInefficiently<Hero>());
			int ransomPrice = (int)((float)new SetPrisonerFreeBarterable(captiveHero, captorClanOfPrisoner.Leader, captiveHero.PartyBelongedToAsPrisoner, ransomPayer).GetUnitValueForFaction(captiveHero.Clan) * 1.1f);
			TextObject textObject = ((captorClanOfPrisoner == Clan.PlayerClan) ? RansomOfferCampaignBehavior.RansomPanelDescriptionPlayerHeldPrisonerText : RansomOfferCampaignBehavior.RansomPanelDescriptionNpcHeldPrisonerText);
			textObject.SetTextVariable("CLAN_NAME", clan.Name);
			textObject.SetTextVariable("GOLD_AMOUNT", ransomPrice);
			textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			StringHelpers.SetCharacterProperties("CAPTIVE_HERO", captiveHero.CharacterObject, textObject, false);
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			InformationManager.ShowInquiry(new InquiryData(RansomOfferCampaignBehavior.RansomPanelTitleText.ToString(), textObject.ToString(), true, true, RansomOfferCampaignBehavior.RansomPanelAffirmativeText.ToString(), RansomOfferCampaignBehavior.RansomPanelNegativeText.ToString(), delegate
			{
				this.AcceptRansomOffer(ransomPrice);
			}, new Action(this.DeclineRansomOffer), "", 0f, null, () => this.IsAffirmativeOptionEnabled(ransomPayer, ransomPrice), null), true, false);
		}

		// Token: 0x060047E4 RID: 18404 RVA: 0x001620C0 File Offset: 0x001602C0
		private ValueTuple<bool, string> IsAffirmativeOptionEnabled(Hero ransomPayer, int ransomPrice)
		{
			if (ransomPayer == Hero.MainHero && ransomPayer.Gold < ransomPrice)
			{
				return new ValueTuple<bool, string>(false, "{=d0kbtGYn}You don't have enough gold.");
			}
			return new ValueTuple<bool, string>(true, string.Empty);
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x001620EC File Offset: 0x001602EC
		private void AcceptRansomOffer(int ransomPrice)
		{
			if (this._heroesWithDeclinedRansomOffers.Contains(this._currentRansomHero))
			{
				this._heroesWithDeclinedRansomOffers.Remove(this._currentRansomHero);
			}
			if (this._currentRansomPayer.Gold < ransomPrice + 1000 && this._currentRansomPayer != Hero.MainHero)
			{
				this._currentRansomPayer.Gold = ransomPrice + 1000;
			}
			GiveGoldAction.ApplyBetweenCharacters(this._currentRansomPayer, this.GetCaptorClanOfPrisoner(this._currentRansomHero).Leader, ransomPrice, false);
			EndCaptivityAction.ApplyByRansom(this._currentRansomHero, this._currentRansomHero.Clan.Leader);
			IStatisticsCampaignBehavior behavior = Campaign.Current.CampaignBehaviorManager.GetBehavior<IStatisticsCampaignBehavior>();
			if (behavior != null)
			{
				behavior.OnPlayerAcceptedRansomOffer(ransomPrice);
			}
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x001621A4 File Offset: 0x001603A4
		private void DeclineRansomOffer()
		{
			if (this._currentRansomHero.IsPrisoner && this._currentRansomHero.IsAlive && !this._heroesWithDeclinedRansomOffers.Contains(this._currentRansomHero))
			{
				this._heroesWithDeclinedRansomOffers.Add(this._currentRansomHero);
			}
			this.SetCurrentRansomHero(null, null);
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x001621F7 File Offset: 0x001603F7
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			this.HandleDeclineRansomOffer(victim);
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x00162200 File Offset: 0x00160400
		private void HandleDeclineRansomOffer(Hero victim)
		{
			if (this._currentRansomHero != null && (victim == this._currentRansomHero || victim == Hero.MainHero))
			{
				CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._currentRansomHero);
				this.DeclineRansomOffer();
			}
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x00162234 File Offset: 0x00160434
		private void OnPrisonersChangeInSettlement(Settlement settlement, FlattenedTroopRoster roster, Hero prisoner, bool takenFromDungeon)
		{
			if (!takenFromDungeon && this._currentRansomHero != null)
			{
				if (prisoner == this._currentRansomHero)
				{
					CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._currentRansomHero);
					this.DeclineRansomOffer();
					return;
				}
				if (roster != null)
				{
					foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in roster)
					{
						if (flattenedTroopRosterElement.Troop.IsHero && flattenedTroopRosterElement.Troop.HeroObject == this._currentRansomHero)
						{
							CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._currentRansomHero);
							this.DeclineRansomOffer();
							break;
						}
					}
				}
			}
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x001622E4 File Offset: 0x001604E4
		private void OnHeroPrisonerReleased(Hero prisoner, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail, bool showNotification)
		{
			this.HandleDeclineRansomOffer(prisoner);
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x001622ED File Offset: 0x001604ED
		private void HourlyTick()
		{
			if (this._currentRansomHero != null && this._currentRansomOfferDate.ElapsedDaysUntilNow >= 2f)
			{
				CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._currentRansomHero);
				this.DeclineRansomOffer();
			}
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x00162320 File Offset: 0x00160520
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<Hero>>("_heroesWithDeclinedRansomOffers", ref this._heroesWithDeclinedRansomOffers);
			dataStore.SyncData<Hero>("_currentRansomHero", ref this._currentRansomHero);
			dataStore.SyncData<Hero>("_currentRansomPayer", ref this._currentRansomPayer);
			dataStore.SyncData<CampaignTime>("_currentRansomOfferDate", ref this._currentRansomOfferDate);
		}

		// Token: 0x04001457 RID: 5207
		private const float RansomOfferInitialChance = 0.2f;

		// Token: 0x04001458 RID: 5208
		private const float RansomOfferChanceAfterRefusal = 0.12f;

		// Token: 0x04001459 RID: 5209
		private const float RansomOfferChanceForPrisonersKeptByAI = 0.1f;

		// Token: 0x0400145A RID: 5210
		private const float MapNotificationAutoDeclineDurationInDays = 2f;

		// Token: 0x0400145B RID: 5211
		private const int AmountOfGoldLeftAfterRansom = 1000;

		// Token: 0x0400145C RID: 5212
		private static TextObject RansomOfferDescriptionText = new TextObject("{=ZqJ92UN4}A courier with a ransom offer for the freedom of {CAPTIVE_HERO.NAME} has arrived.", null);

		// Token: 0x0400145D RID: 5213
		private static TextObject RansomPanelDescriptionNpcHeldPrisonerText = new TextObject("{=4fXpOe4N}A courier arrives from the {CLAN_NAME}. They hold {CAPTIVE_HERO.NAME} and are demanding {GOLD_AMOUNT}{GOLD_ICON} in ransom.", null);

		// Token: 0x0400145E RID: 5214
		private static TextObject RansomPanelDescriptionPlayerHeldPrisonerText = new TextObject("{=PutoRsWp}A courier arrives from the {CLAN_NAME}. They offer you {GOLD_AMOUNT}{GOLD_ICON} in ransom if you will free {CAPTIVE_HERO.NAME}.", null);

		// Token: 0x0400145F RID: 5215
		private List<Hero> _heroesWithDeclinedRansomOffers = new List<Hero>();

		// Token: 0x04001460 RID: 5216
		private Hero _currentRansomHero;

		// Token: 0x04001461 RID: 5217
		private Hero _currentRansomPayer;

		// Token: 0x04001462 RID: 5218
		private CampaignTime _currentRansomOfferDate;
	}
}
