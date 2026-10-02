using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200043A RID: 1082
	public class MarriageOfferCampaignBehavior : CampaignBehaviorBase, IMarriageOfferCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x060045B8 RID: 17848 RVA: 0x0015205C File Offset: 0x0015025C
		internal bool IsThereActiveMarriageOffer
		{
			get
			{
				return this._currentOfferedPlayerClanHero != null && this._currentOfferedOtherClanHero != null;
			}
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x00152074 File Offset: 0x00150274
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.DailyTickClan));
			CampaignEvents.OnMarriageOfferedToPlayerEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnMarriageOfferedToPlayer));
			CampaignEvents.OnMarriageOfferCanceledEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnMarriageOfferCanceled));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroPrisonerTaken));
			CampaignEvents.BeforeHeroesMarried.AddNonSerializedListener(this, new Action<Hero, Hero, bool>(this.OnHeroesMarried));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.ArmyCreated.AddNonSerializedListener(this, new Action<Army>(this.OnArmyCreated));
			CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
			CampaignEvents.CharacterBecameFugitiveEvent.AddNonSerializedListener(this, new Action<Hero, bool>(this.CharacterBecameFugitive));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.HeroRelationChanged.AddNonSerializedListener(this, new Action<Hero, Hero, int, bool, ChangeRelationAction.ChangeRelationDetail, Hero, Hero>(this.OnHeroRelationChanged));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x001521B0 File Offset: 0x001503B0
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Hero>("_currentOfferedPlayerClanHero", ref this._currentOfferedPlayerClanHero);
			dataStore.SyncData<Hero>("_currentOfferedOtherClanHero", ref this._currentOfferedOtherClanHero);
			dataStore.SyncData<CampaignTime>("_lastMarriageOfferTime", ref this._lastMarriageOfferTime);
			dataStore.SyncData<Dictionary<Hero, Hero>>("_acceptedMarriageOffersThatWaitingForAvailability", ref this._acceptedMarriageOffersThatWaitingForAvailability);
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x00152208 File Offset: 0x00150408
		public void CreateMarriageOffer(Hero currentOfferedPlayerClanHero, Hero currentOfferedOtherClanHero)
		{
			this._currentOfferedPlayerClanHero = currentOfferedPlayerClanHero;
			this._currentOfferedOtherClanHero = currentOfferedOtherClanHero;
			this._lastMarriageOfferTime = CampaignTime.Now;
			this.MarriageOfferPanelExplanationText.SetCharacterProperties("CLAN_MEMBER", this._currentOfferedPlayerClanHero.CharacterObject, false);
			this.MarriageOfferPanelExplanationText.SetTextVariable("OFFERING_CLAN_NAME", this._currentOfferedOtherClanHero.Clan.Name);
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new MarriageOfferMapNotification(this._currentOfferedPlayerClanHero, this._currentOfferedOtherClanHero, this.MarriageOfferPanelExplanationText));
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x00152294 File Offset: 0x00150494
		public MBBindingList<TextObject> GetMarriageAcceptedConsequences()
		{
			MBBindingList<TextObject> mbbindingList = new MBBindingList<TextObject>();
			TextObject textObject = GameTexts.FindText("str_marriage_consequence_hero_join_clan", null);
			if (Campaign.Current.Models.MarriageModel.GetClanAfterMarriage(this._currentOfferedPlayerClanHero, this._currentOfferedOtherClanHero) == this._currentOfferedPlayerClanHero.Clan)
			{
				textObject.SetCharacterProperties("HERO", this._currentOfferedOtherClanHero.CharacterObject, false);
				textObject.SetTextVariable("CLAN_NAME", this._currentOfferedPlayerClanHero.Clan.Name);
			}
			else
			{
				textObject.SetCharacterProperties("HERO", this._currentOfferedPlayerClanHero.CharacterObject, false);
				textObject.SetTextVariable("CLAN_NAME", this._currentOfferedOtherClanHero.Clan.Name);
			}
			mbbindingList.Add(textObject);
			TextObject textObject2 = GameTexts.FindText("str_marriage_consequence_clan_relation", null);
			textObject2.SetTextVariable("CLAN_NAME", this._currentOfferedOtherClanHero.Clan.Name);
			textObject2.SetTextVariable("AMOUNT", 10.ToString("+0;-#"));
			mbbindingList.Add(textObject2);
			return mbbindingList;
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x00152398 File Offset: 0x00150598
		private void DailyTickClan(Clan consideringClan)
		{
			if (this.CanOfferMarriageForClan(consideringClan))
			{
				MobileParty.NavigationType navigationType = (consideringClan.HasNavalNavigationCapability ? MobileParty.NavigationType.All : MobileParty.NavigationType.Default);
				float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(Clan.PlayerClan.FactionMidSettlement, consideringClan.FactionMidSettlement, false, false, navigationType);
				if (MBRandom.RandomFloat >= distance / Campaign.Current.Models.MapDistanceModel.GetMaximumDistanceBetweenTwoConnectedSettlements(navigationType) - 0.5f)
				{
					foreach (Hero hero in Clan.PlayerClan.Heroes)
					{
						if (hero != Hero.MainHero && hero.CanMarry() && !this._acceptedMarriageOffersThatWaitingForAvailability.ContainsKey(hero) && this.ConsiderMarriageForPlayerClanMember(hero, consideringClan))
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x060045BE RID: 17854 RVA: 0x00152478 File Offset: 0x00150678
		private void MarryHeroesViaOffer(Hero playerClanHero, Hero otherClanHero, bool showSceneNotification)
		{
			if (playerClanHero != Hero.MainHero && showSceneNotification)
			{
				Hero hero = (playerClanHero.IsFemale ? otherClanHero : playerClanHero);
				Hero hero2 = (playerClanHero.IsFemale ? playerClanHero : otherClanHero);
				MBInformationManager.ShowSceneNotification(new MarriageSceneNotificationItem(hero, hero2, CampaignTime.Now, SceneNotificationData.RelevantContextType.Any));
			}
			ChangeRelationAction.ApplyPlayerRelation(otherClanHero.Clan.Leader, 10, true, true);
			MarriageAction.Apply(playerClanHero, otherClanHero, true);
		}

		// Token: 0x060045BF RID: 17855 RVA: 0x001524DC File Offset: 0x001506DC
		public void OnMarriageOfferAcceptedOnPopUp()
		{
			if (Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(this._currentOfferedPlayerClanHero, this._currentOfferedOtherClanHero))
			{
				this.MarryHeroesViaOffer(this._currentOfferedPlayerClanHero, this._currentOfferedOtherClanHero, true);
			}
			else
			{
				this._acceptedMarriageOffersThatWaitingForAvailability.Add(this._currentOfferedPlayerClanHero, this._currentOfferedOtherClanHero);
				InformationManager.ShowInquiry(new InquiryData(string.Empty, this.MarriagePreparationStartedText.ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), string.Empty, null, null, "", 0f, null, null, null), false, false);
			}
			this.FinalizeMarriageOffer();
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x0015257F File Offset: 0x0015077F
		public void OnMarriageOfferedToPlayer(Hero suitor, Hero maiden)
		{
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x00152584 File Offset: 0x00150784
		public void OnMarriageOfferDeclinedOnPopUp()
		{
			CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x001525D1 File Offset: 0x001507D1
		public void OnMarriageOfferCanceled(Hero suitor, Hero maiden)
		{
			this.FinalizeMarriageOffer();
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x001525DC File Offset: 0x001507DC
		public bool IsHeroEngaged(Hero hero)
		{
			foreach (KeyValuePair<Hero, Hero> keyValuePair in this._acceptedMarriageOffersThatWaitingForAvailability)
			{
				if (keyValuePair.Key == hero || keyValuePair.Value == hero)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x00152644 File Offset: 0x00150844
		private void HourlyTick()
		{
			if (this.IsThereActiveMarriageOffer && this._lastMarriageOfferTime.ElapsedDaysUntilNow >= 2f)
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
			MarriageModel marriageModel = Campaign.Current.Models.MarriageModel;
			List<Hero> list = new List<Hero>();
			foreach (KeyValuePair<Hero, Hero> keyValuePair in this._acceptedMarriageOffersThatWaitingForAvailability)
			{
				Hero key = keyValuePair.Key;
				Hero value = keyValuePair.Value;
				if (marriageModel.IsCoupleSuitableForMarriage(key, value))
				{
					this.MarryHeroesViaOffer(key, value, false);
					list.Add(key);
				}
				else if (!marriageModel.ShouldNpcMarriageBetweenClansBeAllowed(Clan.PlayerClan, value.Clan))
				{
					list.Add(key);
				}
			}
			foreach (Hero hero in list)
			{
				this._acceptedMarriageOffersThatWaitingForAvailability.Remove(hero);
			}
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x00152798 File Offset: 0x00150998
		private void OnHeroPrisonerTaken(PartyBase capturer, Hero prisoner)
		{
			if (this.IsThereActiveMarriageOffer && (prisoner == Hero.MainHero || prisoner == this._currentOfferedPlayerClanHero || prisoner == this._currentOfferedOtherClanHero))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x00152808 File Offset: 0x00150A08
		private void OnHeroesMarried(Hero hero1, Hero hero2, bool showNotification = true)
		{
			if (this.IsThereActiveMarriageOffer && ((hero1 == this._currentOfferedPlayerClanHero && hero2 == this._currentOfferedOtherClanHero) || (hero1 == this._currentOfferedOtherClanHero && hero2 == this._currentOfferedPlayerClanHero)))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x00152884 File Offset: 0x00150A84
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			if (this.IsThereActiveMarriageOffer && (victim == Hero.MainHero || victim == this._currentOfferedPlayerClanHero || victim == this._currentOfferedOtherClanHero))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
			Hero hero = null;
			foreach (KeyValuePair<Hero, Hero> keyValuePair in this._acceptedMarriageOffersThatWaitingForAvailability)
			{
				if (keyValuePair.Key == victim || keyValuePair.Value == victim)
				{
					hero = keyValuePair.Key;
				}
			}
			if (hero != null)
			{
				Hero hero2 = null;
				Hero hero3 = null;
				if (hero.IsDead)
				{
					hero2 = hero;
					hero3 = this._acceptedMarriageOffersThatWaitingForAvailability[hero];
				}
				else if (this._acceptedMarriageOffersThatWaitingForAvailability[hero].IsDead)
				{
					hero2 = this._acceptedMarriageOffersThatWaitingForAvailability[hero];
					hero3 = hero;
				}
				this.MarriagePreparationCanceledText.SetCharacterProperties("DEAD_HERO", hero2.CharacterObject, false);
				this.MarriagePreparationCanceledText.SetTextVariable("OTHER_HERO", hero3.Name);
				InformationManager.ShowInquiry(new InquiryData(string.Empty, this.MarriagePreparationCanceledText.ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), string.Empty, null, null, "", 0f, null, null, null), false, false);
				this._acceptedMarriageOffersThatWaitingForAvailability.Remove(hero);
			}
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x00152A18 File Offset: 0x00150C18
		private void OnArmyCreated(Army army)
		{
			if (this.IsThereActiveMarriageOffer)
			{
				MobileParty partyBelongedTo = this._currentOfferedPlayerClanHero.PartyBelongedTo;
				if (((partyBelongedTo != null) ? partyBelongedTo.Army : null) == null)
				{
					MobileParty partyBelongedTo2 = this._currentOfferedOtherClanHero.PartyBelongedTo;
					if (((partyBelongedTo2 != null) ? partyBelongedTo2.Army : null) == null)
					{
						return;
					}
				}
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x00152AA0 File Offset: 0x00150CA0
		private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			if (this.IsThereActiveMarriageOffer)
			{
				MobileParty partyBelongedTo = this._currentOfferedPlayerClanHero.PartyBelongedTo;
				if (((partyBelongedTo != null) ? partyBelongedTo.MapEvent : null) == null)
				{
					MobileParty partyBelongedTo2 = this._currentOfferedOtherClanHero.PartyBelongedTo;
					if (((partyBelongedTo2 != null) ? partyBelongedTo2.MapEvent : null) == null)
					{
						return;
					}
				}
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x00152B28 File Offset: 0x00150D28
		private void CharacterBecameFugitive(Hero hero, bool showNotification)
		{
			if (this.IsThereActiveMarriageOffer && (!this._currentOfferedPlayerClanHero.IsActive || !this._currentOfferedOtherClanHero.IsActive))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x00152B98 File Offset: 0x00150D98
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail declareWarDetail)
		{
			if (this.IsThereActiveMarriageOffer && (!Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(this._currentOfferedPlayerClanHero, this._currentOfferedOtherClanHero) || !Campaign.Current.Models.MarriageModel.ShouldNpcMarriageBetweenClansBeAllowed(Clan.PlayerClan, this._currentOfferedOtherClanHero.Clan)))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x00152C38 File Offset: 0x00150E38
		private void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
			if (this.IsThereActiveMarriageOffer && (effectiveHero.Clan == this._currentOfferedPlayerClanHero.Clan || effectiveHero.Clan == this._currentOfferedOtherClanHero.Clan) && (effectiveHeroGainedRelationWith.Clan == this._currentOfferedPlayerClanHero.Clan || effectiveHeroGainedRelationWith.Clan == this._currentOfferedOtherClanHero.Clan) && !Campaign.Current.Models.MarriageModel.ShouldNpcMarriageBetweenClansBeAllowed(this._currentOfferedPlayerClanHero.Clan, this._currentOfferedOtherClanHero.Clan))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x00152D0C File Offset: 0x00150F0C
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (this.IsThereActiveMarriageOffer && (this._currentOfferedPlayerClanHero.Clan == clan || this._currentOfferedOtherClanHero.Clan == clan) && !Campaign.Current.Models.MarriageModel.ShouldNpcMarriageBetweenClansBeAllowed(this._currentOfferedPlayerClanHero.Clan, this._currentOfferedOtherClanHero.Clan))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x00152DAC File Offset: 0x00150FAC
		private bool CanOfferMarriageForClan(Clan consideringClan)
		{
			return !this.IsThereActiveMarriageOffer && this._lastMarriageOfferTime.ElapsedDaysUntilNow >= 7f && !Hero.MainHero.IsPrisoner && !MobileParty.MainParty.IsInNavalAutoTravel && consideringClan != Clan.PlayerClan && !consideringClan.HasBloodFeudWithPlayer && Campaign.Current.Models.MarriageModel.IsClanSuitableForMarriage(consideringClan) && Campaign.Current.Models.MarriageModel.ShouldNpcMarriageBetweenClansBeAllowed(Clan.PlayerClan, consideringClan);
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x00152E30 File Offset: 0x00151030
		private bool ConsiderMarriageForPlayerClanMember(Hero playerClanHero, Clan consideringClan)
		{
			MarriageModel marriageModel = Campaign.Current.Models.MarriageModel;
			foreach (Hero hero in consideringClan.Heroes)
			{
				float num = marriageModel.NpcCoupleMarriageChance(playerClanHero, hero);
				if (num > 0f && MBRandom.RandomFloat < num)
				{
					foreach (Romance.RomanticState romanticState in Romance.RomanticStateList)
					{
						if (romanticState.Level >= Romance.RomanceLevelEnum.MatchMadeByFamily && (romanticState.Person1 == playerClanHero || romanticState.Person2 == playerClanHero || romanticState.Person1 == hero || romanticState.Person2 == hero))
						{
							return false;
						}
					}
					this.CreateMarriageOffer(playerClanHero, hero);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x00152F30 File Offset: 0x00151130
		private void FinalizeMarriageOffer()
		{
			this._currentOfferedPlayerClanHero = null;
			this._currentOfferedOtherClanHero = null;
		}

		// Token: 0x04001416 RID: 5142
		private const int MarriageOfferCooldownDurationAsDays = 7;

		// Token: 0x04001417 RID: 5143
		private const int OfferRelationGainAmountWithTheMarriageClan = 10;

		// Token: 0x04001418 RID: 5144
		private const float MapNotificationAutoDeclineDurationInDays = 2f;

		// Token: 0x04001419 RID: 5145
		private readonly TextObject MarriageOfferPanelExplanationText = new TextObject("{=CZwrlJMJ}A courier with a marriage offer for {CLAN_MEMBER.NAME} from {OFFERING_CLAN_NAME} has arrived.", null);

		// Token: 0x0400141A RID: 5146
		private readonly TextObject MarriagePreparationStartedText = new TextObject("{=yz78jqbx}Ceremony is being prepared and will be conducted as soon as possible.", null);

		// Token: 0x0400141B RID: 5147
		private readonly TextObject MarriagePreparationCanceledText = new TextObject("{=dp044PNk}Due to the untimely death of {DEAD_HERO}, {?DEAD_HERO.GENDER}her{?}his{\\\\?} marriage with {OTHER_HERO} has been cancelled.", null);

		// Token: 0x0400141C RID: 5148
		private Hero _currentOfferedPlayerClanHero;

		// Token: 0x0400141D RID: 5149
		private Hero _currentOfferedOtherClanHero;

		// Token: 0x0400141E RID: 5150
		private CampaignTime _lastMarriageOfferTime;

		// Token: 0x0400141F RID: 5151
		private Dictionary<Hero, Hero> _acceptedMarriageOffersThatWaitingForAvailability = new Dictionary<Hero, Hero>();
	}
}
