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
	// Token: 0x0200003E RID: 62
	public class AllianceOfferNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005E1 RID: 1505 RVA: 0x0001F334 File Offset: 0x0001D534
		public AllianceOfferNotificationItemVM(AllianceOfferMapNotification data)
			: base(data)
		{
			AllianceOfferNotificationItemVM <>4__this = this;
			this._shouldDecisionBeCreatedOnClosed = false;
			this._offeringKingdom = data.OfferingKingdom;
			this._onInspect = delegate
			{
				bool flag = false;
				if (data != null && data.IsValid() && Clan.PlayerClan.Kingdom != null)
				{
					TextObject textObject;
					flag = new StartAllianceDecision(Clan.PlayerClan, <>4__this._offeringKingdom).CanMakeDecision(out textObject, false);
				}
				if (flag)
				{
					IAllianceCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IAllianceCampaignBehavior>();
					if (campaignBehavior != null)
					{
						campaignBehavior.OnAllianceOfferedToPlayer(data.OfferingKingdom);
					}
					<>4__this.RemoveAllianceOfferNotification(false);
					return;
				}
				InformationManager.ShowInquiry(new InquiryData("", new TextObject("{=4vPm9bFW}This alliance offer is no longer relevant.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
				<>4__this.RemoveAllianceOfferNotification(false);
			};
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
			CampaignEvents.OnAllianceStartedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnAllianceStarted));
			base.NotificationIdentifier = "ransom";
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001F3F2 File Offset: 0x0001D5F2
		private void OnAllianceStarted(Kingdom kingdom1, Kingdom kingdom2)
		{
			if ((kingdom1 == Clan.PlayerClan.Kingdom && kingdom2 == this._offeringKingdom) || (kingdom2 == Clan.PlayerClan.Kingdom && kingdom1 == this._offeringKingdom))
			{
				this.RemoveAllianceOfferNotification(false);
			}
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001F427 File Offset: 0x0001D627
		private void OnKingdomDestroyed(Kingdom kingdom)
		{
			if (kingdom == Clan.PlayerClan.Kingdom || this._offeringKingdom == kingdom)
			{
				this.RemoveAllianceOfferNotification(false);
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001F446 File Offset: 0x0001D646
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan)
			{
				this.RemoveAllianceOfferNotification(false);
				return;
			}
			if (newKingdom == Clan.PlayerClan.Kingdom)
			{
				this.RemoveAllianceOfferNotification(true);
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001F46C File Offset: 0x0001D66C
		private void OnWarDeclared(IFaction side1Faction, IFaction side2Faction, DeclareWarAction.DeclareWarDetail detail)
		{
			if ((side1Faction == Hero.MainHero.MapFaction && side2Faction == this._offeringKingdom) || (side2Faction == Hero.MainHero.MapFaction && side1Faction == this._offeringKingdom))
			{
				this.RemoveAllianceOfferNotification(false);
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001F4A1 File Offset: 0x0001D6A1
		private void RemoveAllianceOfferNotification(bool shouldDecisionCreatedOnClosed)
		{
			this._shouldDecisionBeCreatedOnClosed = shouldDecisionCreatedOnClosed;
			base.ExecuteRemove();
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001F4B0 File Offset: 0x0001D6B0
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			if (this._shouldDecisionBeCreatedOnClosed && Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom.Clans.Count > 1 && Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision s)
			{
				StartAllianceDecision startAllianceDecision2;
				return (startAllianceDecision2 = s as StartAllianceDecision) != null && startAllianceDecision2.KingdomToStartAllianceWith == this._offeringKingdom;
			}) == null)
			{
				StartAllianceDecision startAllianceDecision = new StartAllianceDecision(Clan.PlayerClan, this._offeringKingdom);
				TextObject textObject;
				if (startAllianceDecision.CanMakeDecision(out textObject, false))
				{
					Clan.PlayerClan.Kingdom.AddDecision(startAllianceDecision, true);
				}
			}
		}

		// Token: 0x04000283 RID: 643
		private readonly Kingdom _offeringKingdom;

		// Token: 0x04000284 RID: 644
		private bool _shouldDecisionBeCreatedOnClosed;
	}
}
