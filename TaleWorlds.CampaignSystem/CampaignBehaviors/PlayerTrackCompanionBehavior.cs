using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000454 RID: 1108
	public class PlayerTrackCompanionBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004790 RID: 18320 RVA: 0x0015F9C0 File Offset: 0x0015DBC0
		public override void RegisterEvents()
		{
			CampaignEvents.CharacterBecameFugitiveEvent.AddNonSerializedListener(this, new Action<Hero, bool>(this.HeroBecameFugitive));
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.CompanionRemoved));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.SettlementEntered));
			CampaignEvents.NewCompanionAdded.AddNonSerializedListener(this, new Action<Hero>(this.CompanionAdded));
			CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, new Action<Hero, PartyBase, IFaction, EndCaptivityDetail, bool>(this.OnHeroPrisonerReleased));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.MobilePartyCreated.AddNonSerializedListener(this, new Action<MobileParty>(this.OnMobilePartyCreated));
			CampaignEvents.OnHeroTeleportationRequestedEvent.AddNonSerializedListener(this, new Action<Hero, Settlement, MobileParty, TeleportHeroAction.TeleportationDetail>(this.OnHeroTeleportationRequested));
			CampaignEvents.OnHeroJoinedPartyEvent.AddNonSerializedListener(this, new Action<Hero, MobileParty>(this.OnHeroJoinedParty));
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x0015FA9C File Offset: 0x0015DC9C
		private void OnHeroJoinedParty(Hero hero, MobileParty party)
		{
			if (party == MobileParty.MainParty && this._scatteredCompanions.ContainsKey(hero))
			{
				this._scatteredCompanions.Remove(hero);
			}
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x0015FAC1 File Offset: 0x0015DCC1
		private void OnHeroTeleportationRequested(Hero hero, Settlement settlement, MobileParty party, TeleportHeroAction.TeleportationDetail detail)
		{
			if (hero.IsPlayerCompanion && party == MobileParty.MainParty && detail == TeleportHeroAction.TeleportationDetail.DelayedTeleportToParty && this._scatteredCompanions.ContainsKey(hero))
			{
				this._scatteredCompanions.Remove(hero);
			}
		}

		// Token: 0x06004793 RID: 18323 RVA: 0x0015FAF4 File Offset: 0x0015DCF4
		private void OnGameLoadFinished()
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0)))
			{
				foreach (Hero hero in this._scatteredCompanions.Keys.ToList<Hero>())
				{
					if (hero.PartyBelongedTo != null || hero.GovernorOf != null || Campaign.Current.IssueManager.IssueSolvingCompanionList.Contains(hero))
					{
						this._scatteredCompanions.Remove(hero);
					}
				}
			}
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x0015FBA4 File Offset: 0x0015DDA4
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Hero, CampaignTime>>("ScatteredCompanions", ref this._scatteredCompanions);
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x0015FBB8 File Offset: 0x0015DDB8
		private void AddHeroToScatteredCompanions(Hero hero)
		{
			if (hero.IsPlayerCompanion)
			{
				if (!this._scatteredCompanions.ContainsKey(hero))
				{
					this._scatteredCompanions.Add(hero, CampaignTime.Now);
					return;
				}
				this._scatteredCompanions[hero] = CampaignTime.Now;
			}
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x0015FBF3 File Offset: 0x0015DDF3
		private void HeroBecameFugitive(Hero hero, bool showNotification)
		{
			this.AddHeroToScatteredCompanions(hero);
		}

		// Token: 0x06004797 RID: 18327 RVA: 0x0015FBFC File Offset: 0x0015DDFC
		private void OnHeroPrisonerReleased(Hero releasedHero, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail, bool showNotification)
		{
			this.AddHeroToScatteredCompanions(releasedHero);
		}

		// Token: 0x06004798 RID: 18328 RVA: 0x0015FC08 File Offset: 0x0015DE08
		private void SettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			if (party == MobileParty.MainParty)
			{
				foreach (Hero hero2 in this._scatteredCompanions.Keys.ToMBList<Hero>())
				{
					if (hero2.CurrentSettlement == settlement)
					{
						TextObject textObject = new TextObject("{=ahpSGaow}You hear that your companion {COMPANION.LINK}, who was separated from you after a battle, is currently in this settlement.", null);
						StringHelpers.SetCharacterProperties("COMPANION", hero2.CharacterObject, textObject, false);
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=dx0hmeH6}Tracking", null).ToString(), textObject.ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
						this._scatteredCompanions.Remove(hero2);
					}
				}
			}
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x0015FCEC File Offset: 0x0015DEEC
		private void CompanionAdded(Hero companion)
		{
			if (this._scatteredCompanions.ContainsKey(companion))
			{
				this._scatteredCompanions.Remove(companion);
			}
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x0015FD09 File Offset: 0x0015DF09
		private void CompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			if (this._scatteredCompanions.ContainsKey(companion))
			{
				this._scatteredCompanions.Remove(companion);
			}
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x0015FD26 File Offset: 0x0015DF26
		private void OnMobilePartyCreated(MobileParty mobileParty)
		{
			if (mobileParty.LeaderHero != null && mobileParty.LeaderHero.IsPlayerCompanion && this._scatteredCompanions.ContainsKey(mobileParty.LeaderHero))
			{
				this._scatteredCompanions.Remove(mobileParty.LeaderHero);
			}
		}

		// Token: 0x04001452 RID: 5202
		private Dictionary<Hero, CampaignTime> _scatteredCompanions = new Dictionary<Hero, CampaignTime>();
	}
}
