using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200044E RID: 1102
	public class PeaceOfferCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x060046B4 RID: 18100 RVA: 0x0015940C File Offset: 0x0015760C
		private static TextObject PeacePanelTitleText
		{
			get
			{
				return new TextObject("{=ho5EndaV}Decision", null);
			}
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x060046B5 RID: 18101 RVA: 0x00159419 File Offset: 0x00157619
		private static TextObject PeacePanelOkText
		{
			get
			{
				return new TextObject("{=oHaWR73d}Ok", null);
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x060046B6 RID: 18102 RVA: 0x00159426 File Offset: 0x00157626
		private static TextObject PeacePanelAffirmativeText
		{
			get
			{
				return new TextObject("{=Y94H6XnK}Accept", null);
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x060046B7 RID: 18103 RVA: 0x00159433 File Offset: 0x00157633
		private static TextObject PeacePanelNegativeText
		{
			get
			{
				return new TextObject("{=cOgmdp9e}Decline", null);
			}
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x00159440 File Offset: 0x00157640
		public override void RegisterEvents()
		{
			CampaignEvents.OnPeaceOfferedToPlayerEvent.AddNonSerializedListener(this, new Action<IFaction, int, int>(this.OnPeaceOffered));
			CampaignEvents.OnPeaceOfferResolvedEvent.AddNonSerializedListener(this, new Action<IFaction>(this.OnPeaceOfferResolved));
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x00159470 File Offset: 0x00157670
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<int>("_currentPeaceOfferTributeAmount", ref this._currentPeaceOfferTributeAmount);
			dataStore.SyncData<IFaction>("_opponentFaction", ref this._opponentFaction);
		}

		// Token: 0x060046BA RID: 18106 RVA: 0x00159498 File Offset: 0x00157698
		private void OnPeaceOffered(IFaction opponentFaction, int tributeAmount, int tributeDuration)
		{
			if (this._opponentFaction == null)
			{
				this._opponentFaction = opponentFaction;
				this._currentPeaceOfferTributeAmount = tributeAmount;
				this._currentPeaceOfferTributeDuration = tributeDuration;
				TextObject textObject = ((tributeAmount > 0) ? ((Hero.MainHero.MapFaction.Leader == Hero.MainHero) ? PeaceOfferCampaignBehavior.PeaceOfferTributePaidPanelDescriptionText : PeaceOfferCampaignBehavior.PeaceOfferTributePaidPanelPlayerIsVassalDescriptionText) : ((tributeAmount < 0) ? ((Hero.MainHero.MapFaction.Leader == Hero.MainHero) ? PeaceOfferCampaignBehavior.PeaceOfferTributeWantedPanelDescriptionText : PeaceOfferCampaignBehavior.PeaceOfferTributeWantedPanelPlayerIsVassalDescriptionText) : ((Hero.MainHero.MapFaction.Leader == Hero.MainHero) ? PeaceOfferCampaignBehavior.PeaceOfferDefaultPanelDescriptionText : PeaceOfferCampaignBehavior.PeaceOfferDefaultPanelPlayerIsVassalDescriptionText)));
				textObject.SetTextVariable("MAP_FACTION_NAME", opponentFaction.InformalName);
				textObject.SetTextVariable("GOLD_AMOUNT", MathF.Abs(this._currentPeaceOfferTributeAmount));
				textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
				TextObject peacePanelNegativeText = PeaceOfferCampaignBehavior.PeacePanelNegativeText;
				this._influenceCostOfDecline = 0;
				Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
				if (Hero.MainHero.MapFaction.Leader == Hero.MainHero)
				{
					InformationManager.ShowInquiry(new InquiryData(PeaceOfferCampaignBehavior.PeacePanelTitleText.ToString(), textObject.ToString(), true, (float)this._influenceCostOfDecline <= 0.1f || Hero.MainHero.Clan.Influence >= (float)this._influenceCostOfDecline, PeaceOfferCampaignBehavior.PeacePanelAffirmativeText.ToString(), peacePanelNegativeText.ToString(), new Action(this.AcceptPeaceOffer), new Action(this.DeclinePeaceOffer), "", 0f, null, null, null), true, false);
					return;
				}
				InformationManager.ShowInquiry(new InquiryData(PeaceOfferCampaignBehavior.PeacePanelTitleText.ToString(), textObject.ToString(), false, true, PeaceOfferCampaignBehavior.PeacePanelOkText.ToString(), PeaceOfferCampaignBehavior.PeacePanelOkText.ToString(), new Action(this.OkPeaceOffer), new Action(this.OkPeaceOffer), "", 0f, null, null, null), true, false);
			}
		}

		// Token: 0x060046BB RID: 18107 RVA: 0x00159676 File Offset: 0x00157876
		private void OnPeaceOfferResolved(IFaction opponentFaction)
		{
			if (Hero.MainHero.MapFaction.Leader != Hero.MainHero && opponentFaction != null)
			{
				this._opponentFaction = opponentFaction;
				this.OkPeaceOffer();
			}
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x001596A0 File Offset: 0x001578A0
		private void OkPeaceOffer()
		{
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				this.AcceptPeaceOffer();
				return;
			}
			Kingdom kingdom = Clan.PlayerClan.Kingdom;
			KingdomDecision kingdomDecision = kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision s)
			{
				MakePeaceKingdomDecision makePeaceKingdomDecision2;
				return (makePeaceKingdomDecision2 = s as MakePeaceKingdomDecision) != null && makePeaceKingdomDecision2.ProposerClan.MapFaction == Hero.MainHero.MapFaction && makePeaceKingdomDecision2.FactionToMakePeaceWith == this._opponentFaction;
			});
			if (kingdomDecision != null)
			{
				kingdom.RemoveDecision(kingdomDecision);
			}
			MakePeaceKingdomDecision makePeaceKingdomDecision = new MakePeaceKingdomDecision(Hero.MainHero.MapFaction.Leader.Clan, this._opponentFaction, -this._currentPeaceOfferTributeAmount, this._currentPeaceOfferTributeDuration, true, true);
			((Kingdom)Hero.MainHero.MapFaction).AddDecision(makePeaceKingdomDecision, true);
			this._opponentFaction = null;
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x00159739 File Offset: 0x00157939
		private void AcceptPeaceOffer()
		{
			MakePeaceAction.ApplyByKingdomDecision(this._opponentFaction, Hero.MainHero.MapFaction, this._currentPeaceOfferTributeAmount, this._currentPeaceOfferTributeDuration);
			this._opponentFaction = null;
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x00159763 File Offset: 0x00157963
		private void DeclinePeaceOffer()
		{
			this._opponentFaction = null;
			ChangeClanInfluenceAction.Apply(Clan.PlayerClan, (float)(-(float)this._influenceCostOfDecline));
		}

		// Token: 0x0400143D RID: 5181
		private static TextObject PeaceOfferDefaultPanelDescriptionText = new TextObject("{=IB1xsVEr}A courier has arrived from the {MAP_FACTION_NAME}. They offer you a white peace. Your vassals have left the decision with you.", null);

		// Token: 0x0400143E RID: 5182
		private static TextObject PeaceOfferTributePaidPanelDescriptionText = new TextObject("{=JJQ0Hp4m}A courier has arrived from the {MAP_FACTION_NAME}. The {MAP_FACTION_NAME} will pay {GOLD_AMOUNT} {GOLD_ICON} in tribute each day to end the war between your realms. Your vassals have left the decision with you.", null);

		// Token: 0x0400143F RID: 5183
		private static TextObject PeaceOfferTributeWantedPanelDescriptionText = new TextObject("{=Nd0Vhkxn}A courier has arrived from the {MAP_FACTION_NAME}. They offer you peace if you agree to pay a {GOLD_AMOUNT} {GOLD_ICON} daily tribute. Your vassals have left the decision with you.", null);

		// Token: 0x04001440 RID: 5184
		private static TextObject PeaceOfferDefaultPanelPlayerIsVassalDescriptionText = new TextObject("{=gNf0ALKw}A courier has arrived from the {MAP_FACTION_NAME}. They offer you a white peace. Your kingdom will vote whether to accept the offer.", null);

		// Token: 0x04001441 RID: 5185
		private static TextObject PeaceOfferTributePaidPanelPlayerIsVassalDescriptionText = new TextObject("{=SR9FC5jH}A courier has arrived from the {MAP_FACTION_NAME} bearing a peace offer. The {MAP_FACTION_NAME} will pay {GOLD_AMOUNT} {GOLD_ICON} in tribute each day to end the war between your realms. Your kingdom will vote whether to accept the offer.", null);

		// Token: 0x04001442 RID: 5186
		private static TextObject PeaceOfferTributeWantedPanelPlayerIsVassalDescriptionText = new TextObject("{=sbFboHmV}A courier has arrived from the {MAP_FACTION_NAME}. They offer you peace if you agree to pay a {GOLD_AMOUNT} {GOLD_ICON} daily tribute. Your kingdom will vote whether to accept the offer.", null);

		// Token: 0x04001443 RID: 5187
		private IFaction _opponentFaction;

		// Token: 0x04001444 RID: 5188
		private int _currentPeaceOfferTributeAmount;

		// Token: 0x04001445 RID: 5189
		private int _currentPeaceOfferTributeDuration;

		// Token: 0x04001446 RID: 5190
		private int _influenceCostOfDecline;
	}
}
