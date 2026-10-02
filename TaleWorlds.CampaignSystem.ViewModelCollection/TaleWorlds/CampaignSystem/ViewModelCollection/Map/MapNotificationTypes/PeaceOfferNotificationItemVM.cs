using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000053 RID: 83
	public class PeaceOfferNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000651 RID: 1617 RVA: 0x000208C8 File Offset: 0x0001EAC8
		public PeaceOfferNotificationItemVM(PeaceOfferMapNotification data)
			: base(data)
		{
			PeaceOfferNotificationItemVM <>4__this = this;
			this._shouldDecisionBeCreatedOnClosed = true;
			this._opponentFaction = data.OpponentFaction;
			this._tributeAmount = data.TributeAmount;
			this._tributeDurationInDays = data.TributeDurationInDays;
			this._onInspect = delegate
			{
				CampaignEventDispatcher.Instance.OnPeaceOfferedToPlayer(data.OpponentFaction, data.TributeAmount, data.TributeDurationInDays);
				<>4__this.RemovePeaceOfferNotification(false);
			};
			CampaignEvents.OnPeaceOfferResolvedEvent.AddNonSerializedListener(this, new Action<IFaction>(this.OnPeaceOfferClosed));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnMakePeace));
			base.NotificationIdentifier = "ransom";
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00020991 File Offset: 0x0001EB91
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan)
			{
				this.RemovePeaceOfferNotification(false);
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x000209A2 File Offset: 0x0001EBA2
		private void OnMakePeace(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			if ((side1Faction == Hero.MainHero.MapFaction && side2Faction == this._opponentFaction) || (side2Faction == Hero.MainHero.MapFaction && side1Faction == this._opponentFaction))
			{
				this.RemovePeaceOfferNotification(false);
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x000209D7 File Offset: 0x0001EBD7
		private void OnPeaceOfferClosed(IFaction opponentFaction)
		{
			if (Campaign.Current.CampaignInformationManager.InformationDataExists<PeaceOfferMapNotification>((PeaceOfferMapNotification x) => x == base.Data))
			{
				this.RemovePeaceOfferNotification(true);
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x000209FD File Offset: 0x0001EBFD
		private void RemovePeaceOfferNotification(bool shouldDecisionCreatedOnClosed)
		{
			this._shouldDecisionBeCreatedOnClosed = shouldDecisionCreatedOnClosed;
			base.ExecuteRemove();
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00020A0C File Offset: 0x0001EC0C
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			if (this._shouldDecisionBeCreatedOnClosed && Hero.MainHero.MapFaction.Leader != Hero.MainHero)
			{
				bool flag = false;
				foreach (KingdomDecision kingdomDecision in ((Kingdom)Hero.MainHero.MapFaction).UnresolvedDecisions)
				{
					if (kingdomDecision is MakePeaceKingdomDecision && ((MakePeaceKingdomDecision)kingdomDecision).ProposerClan.MapFaction == Hero.MainHero.MapFaction && ((MakePeaceKingdomDecision)kingdomDecision).FactionToMakePeaceWith == this._opponentFaction)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					MakePeaceKingdomDecision makePeaceKingdomDecision = new MakePeaceKingdomDecision(Hero.MainHero.MapFaction.Leader.Clan, this._opponentFaction, -this._tributeAmount, this._tributeDurationInDays, true, true);
					((Kingdom)Hero.MainHero.MapFaction).AddDecision(makePeaceKingdomDecision, true);
				}
			}
		}

		// Token: 0x040002A7 RID: 679
		private bool _shouldDecisionBeCreatedOnClosed;

		// Token: 0x040002A8 RID: 680
		private readonly IFaction _opponentFaction;

		// Token: 0x040002A9 RID: 681
		private readonly int _tributeAmount;

		// Token: 0x040002AA RID: 682
		private readonly int _tributeDurationInDays;
	}
}
