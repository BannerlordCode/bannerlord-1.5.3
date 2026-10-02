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
	// Token: 0x02000054 RID: 84
	public class ProposeCallToWarOfferNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000658 RID: 1624 RVA: 0x00020B2C File Offset: 0x0001ED2C
		public ProposeCallToWarOfferNotificationItemVM(ProposeCallToWarOfferMapNotification data)
			: base(data)
		{
			ProposeCallToWarOfferNotificationItemVM <>4__this = this;
			this._shouldDecisionBeCreatedOnClosed = false;
			this._offeredKingdom = data.OfferedKingdom;
			this._kingdomToCallToWarAgainst = data.KingdomToCallToWarAgainst;
			this._onInspect = delegate
			{
				bool flag = false;
				if (data != null && data.IsValid() && Clan.PlayerClan.Kingdom != null)
				{
					TextObject textObject;
					flag = new ProposeCallToWarAgreementDecision(Clan.PlayerClan, <>4__this._offeredKingdom, <>4__this._kingdomToCallToWarAgainst).CanMakeDecision(out textObject, false);
				}
				if (flag)
				{
					IAllianceCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IAllianceCampaignBehavior>();
					if (campaignBehavior != null)
					{
						campaignBehavior.OnCallToWarAgreementProposedByPlayer(data.OfferedKingdom, data.KingdomToCallToWarAgainst);
					}
					<>4__this.RemoveProposeCallToWarOfferNotification(false);
					return;
				}
				InformationManager.ShowInquiry(new InquiryData("", new TextObject("{=oGgjuQav}This call to war offer is no longer relevant.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
				<>4__this.RemoveProposeCallToWarOfferNotification(false);
			};
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnPeaceDeclared));
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
			CampaignEvents.OnAllianceEndedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnAllianceEnded));
			base.NotificationIdentifier = "ransom";
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00020C12 File Offset: 0x0001EE12
		private void OnPeaceDeclared(IFaction faction1, IFaction faction2, MakePeaceAction.MakePeaceDetail detail)
		{
			if ((faction1 == this._offeredKingdom && faction2 == this._kingdomToCallToWarAgainst) || (faction2 == this._offeredKingdom && faction1 == this._kingdomToCallToWarAgainst))
			{
				this.RemoveProposeCallToWarOfferNotification(false);
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00020C3F File Offset: 0x0001EE3F
		private void OnAllianceEnded(Kingdom kingdom1, Kingdom kingdom2)
		{
			if ((kingdom1 == Clan.PlayerClan.Kingdom && kingdom2 == this._offeredKingdom) || (kingdom2 == Clan.PlayerClan.Kingdom && kingdom1 == this._offeredKingdom))
			{
				this.RemoveProposeCallToWarOfferNotification(false);
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00020C74 File Offset: 0x0001EE74
		private void OnKingdomDestroyed(Kingdom kingdom)
		{
			if (kingdom == Clan.PlayerClan.Kingdom || this._offeredKingdom == kingdom || this._kingdomToCallToWarAgainst == kingdom)
			{
				this.RemoveProposeCallToWarOfferNotification(false);
			}
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00020C9C File Offset: 0x0001EE9C
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan)
			{
				this.RemoveProposeCallToWarOfferNotification(false);
				return;
			}
			if (newKingdom == Clan.PlayerClan.Kingdom)
			{
				this.RemoveProposeCallToWarOfferNotification(true);
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00020CC2 File Offset: 0x0001EEC2
		private void OnWarDeclared(IFaction side1Faction, IFaction side2Faction, DeclareWarAction.DeclareWarDetail detail)
		{
			if ((side1Faction == Hero.MainHero.MapFaction && side2Faction == this._kingdomToCallToWarAgainst) || (side2Faction == Hero.MainHero.MapFaction && side1Faction == this._kingdomToCallToWarAgainst))
			{
				this.RemoveProposeCallToWarOfferNotification(false);
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00020CF7 File Offset: 0x0001EEF7
		private void RemoveProposeCallToWarOfferNotification(bool shouldDecisionCreatedOnClosed)
		{
			this._shouldDecisionBeCreatedOnClosed = shouldDecisionCreatedOnClosed;
			base.ExecuteRemove();
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00020D08 File Offset: 0x0001EF08
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			if (this._shouldDecisionBeCreatedOnClosed && Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom.Clans.Count > 1 && Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision s)
			{
				ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision2;
				return (proposeCallToWarAgreementDecision2 = s as ProposeCallToWarAgreementDecision) != null && proposeCallToWarAgreementDecision2.CalledKingdom == this._offeredKingdom;
			}) == null)
			{
				ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision = new ProposeCallToWarAgreementDecision(Clan.PlayerClan, this._offeredKingdom, this._kingdomToCallToWarAgainst);
				TextObject textObject;
				if (proposeCallToWarAgreementDecision.CanMakeDecision(out textObject, false))
				{
					Clan.PlayerClan.Kingdom.AddDecision(proposeCallToWarAgreementDecision, true);
				}
			}
		}

		// Token: 0x040002AB RID: 683
		private readonly Kingdom _offeredKingdom;

		// Token: 0x040002AC RID: 684
		private readonly Kingdom _kingdomToCallToWarAgainst;

		// Token: 0x040002AD RID: 685
		private bool _shouldDecisionBeCreatedOnClosed;
	}
}
