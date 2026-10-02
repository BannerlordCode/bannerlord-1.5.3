using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200003B RID: 59
	public class AcceptCallToWarOfferNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005D2 RID: 1490 RVA: 0x0001EEEC File Offset: 0x0001D0EC
		public AcceptCallToWarOfferNotificationItemVM(AcceptCallToWarOfferMapNotification data)
			: base(data)
		{
			AcceptCallToWarOfferNotificationItemVM <>4__this = this;
			this._shouldDecisionBeCreatedOnClosed = false;
			this._offeringKingdom = data.OfferingKingdom;
			this._kingdomToCallToWarAgainst = data.KingdomToCallToWarAgainst;
			this._onInspect = delegate
			{
				bool flag = false;
				if (data != null && data.IsValid() && Clan.PlayerClan.Kingdom != null)
				{
					TextObject textObject;
					flag = new AcceptCallToWarAgreementDecision(Clan.PlayerClan, <>4__this._offeringKingdom, <>4__this._kingdomToCallToWarAgainst).CanMakeDecision(out textObject, false);
				}
				if (flag)
				{
					IAllianceCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IAllianceCampaignBehavior>();
					if (campaignBehavior != null)
					{
						campaignBehavior.OnCallToWarAgreementProposedToPlayer(data.OfferingKingdom, data.KingdomToCallToWarAgainst);
					}
					<>4__this.RemoveAcceptCallToWarOfferNotification(false);
					return;
				}
				InformationManager.ShowInquiry(new InquiryData("", new TextObject("{=oGgjuQav}This call to war offer is no longer relevant.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
				<>4__this.RemoveAcceptCallToWarOfferNotification(false);
			};
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnPeaceDeclared));
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
			CampaignEvents.OnAllianceEndedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnAllianceEnded));
			base.NotificationIdentifier = "ransom";
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001EFD2 File Offset: 0x0001D1D2
		private void OnPeaceDeclared(IFaction faction1, IFaction faction2, MakePeaceAction.MakePeaceDetail detail)
		{
			if ((faction1 == this._offeringKingdom && faction2 == this._kingdomToCallToWarAgainst) || (faction2 == this._offeringKingdom && faction1 == this._kingdomToCallToWarAgainst))
			{
				this.RemoveAcceptCallToWarOfferNotification(false);
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001EFFF File Offset: 0x0001D1FF
		private void OnAllianceEnded(Kingdom kingdom1, Kingdom kingdom2)
		{
			if ((kingdom1 == Clan.PlayerClan.Kingdom && kingdom2 == this._offeringKingdom) || (kingdom2 == Clan.PlayerClan.Kingdom && kingdom1 == this._offeringKingdom))
			{
				this.RemoveAcceptCallToWarOfferNotification(false);
			}
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001F034 File Offset: 0x0001D234
		private void OnKingdomDestroyed(Kingdom kingdom)
		{
			if (kingdom == Clan.PlayerClan.Kingdom || this._offeringKingdom == kingdom || this._kingdomToCallToWarAgainst == kingdom)
			{
				this.RemoveAcceptCallToWarOfferNotification(false);
			}
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001F05C File Offset: 0x0001D25C
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan)
			{
				this.RemoveAcceptCallToWarOfferNotification(false);
				return;
			}
			if (newKingdom == Clan.PlayerClan.Kingdom)
			{
				this.RemoveAcceptCallToWarOfferNotification(true);
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0001F082 File Offset: 0x0001D282
		private void OnWarDeclared(IFaction side1Faction, IFaction side2Faction, DeclareWarAction.DeclareWarDetail detail)
		{
			if ((side1Faction == Hero.MainHero.MapFaction && side2Faction == this._kingdomToCallToWarAgainst) || (side2Faction == Hero.MainHero.MapFaction && side1Faction == this._kingdomToCallToWarAgainst))
			{
				this.RemoveAcceptCallToWarOfferNotification(false);
			}
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001F0B7 File Offset: 0x0001D2B7
		private void RemoveAcceptCallToWarOfferNotification(bool shouldDecisionCreatedOnClosed)
		{
			this._shouldDecisionBeCreatedOnClosed = shouldDecisionCreatedOnClosed;
			base.ExecuteRemove();
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001F0C8 File Offset: 0x0001D2C8
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			if (this._shouldDecisionBeCreatedOnClosed && Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom.Clans.Count > 1 && Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision s)
			{
				AcceptCallToWarAgreementDecision acceptCallToWarAgreementDecision2;
				return (acceptCallToWarAgreementDecision2 = s as AcceptCallToWarAgreementDecision) != null && acceptCallToWarAgreementDecision2.CallingKingdom == this._offeringKingdom;
			}) == null)
			{
				AcceptCallToWarAgreementDecision acceptCallToWarAgreementDecision = new AcceptCallToWarAgreementDecision(Clan.PlayerClan, this._offeringKingdom, this._kingdomToCallToWarAgainst);
				TextObject textObject;
				if (acceptCallToWarAgreementDecision.CanMakeDecision(out textObject, false))
				{
					Clan.PlayerClan.Kingdom.AddDecision(acceptCallToWarAgreementDecision, true);
				}
			}
		}

		// Token: 0x0400027E RID: 638
		private readonly Kingdom _offeringKingdom;

		// Token: 0x0400027F RID: 639
		private readonly Kingdom _kingdomToCallToWarAgainst;

		// Token: 0x04000280 RID: 640
		private bool _shouldDecisionBeCreatedOnClosed;
	}
}
