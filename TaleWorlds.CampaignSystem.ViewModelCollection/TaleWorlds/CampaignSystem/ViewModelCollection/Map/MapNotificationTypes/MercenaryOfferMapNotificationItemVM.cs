using System;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200004F RID: 79
	public class MercenaryOfferMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x0600063F RID: 1599 RVA: 0x00020404 File Offset: 0x0001E604
		public MercenaryOfferMapNotificationItemVM(MercenaryOfferMapNotification data)
			: base(data)
		{
			this._offeredKingdom = data.OfferedKingdom;
			base.NotificationIdentifier = "vote";
			this._onInspect = delegate
			{
				CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferedToPlayer(this._offeredKingdom);
				this._playerInspectedNotification = true;
				base.ExecuteRemove();
			};
			CampaignEvents.OnVassalOrMercenaryServiceOfferCanceledEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnVassalOrMercenaryServiceOfferCanceled));
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00020458 File Offset: 0x0001E658
		private void OnVassalOrMercenaryServiceOfferCanceled(Kingdom offeredKingdom)
		{
			if (Campaign.Current.CampaignInformationManager.InformationDataExists<MercenaryOfferMapNotification>((MercenaryOfferMapNotification x) => x.OfferedKingdom == offeredKingdom))
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00020495 File Offset: 0x0001E695
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			if (!this._playerInspectedNotification)
			{
				IVassalAndMercenaryOfferCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IVassalAndMercenaryOfferCampaignBehavior>();
				if (campaignBehavior == null)
				{
					return;
				}
				campaignBehavior.CancelVassalOrMercenaryServiceOffer(this._offeredKingdom);
			}
		}

		// Token: 0x0400029F RID: 671
		private bool _playerInspectedNotification;

		// Token: 0x040002A0 RID: 672
		private readonly Kingdom _offeredKingdom;
	}
}
