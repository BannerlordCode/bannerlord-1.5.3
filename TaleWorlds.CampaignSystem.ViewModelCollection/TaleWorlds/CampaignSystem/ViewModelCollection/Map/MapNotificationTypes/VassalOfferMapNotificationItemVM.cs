using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200005C RID: 92
	public class VassalOfferMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000677 RID: 1655 RVA: 0x000211F0 File Offset: 0x0001F3F0
		public VassalOfferMapNotificationItemVM(VassalOfferMapNotification data)
			: base(data)
		{
			this._offeredKingdom = data.OfferedKingdom;
			base.NotificationIdentifier = "vote";
			this._onInspect = delegate
			{
				CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferedToPlayer(this._offeredKingdom);
				base.ExecuteRemove();
			};
			CampaignEvents.OnVassalOrMercenaryServiceOfferCanceledEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnVassalOrMercenaryServiceOfferCanceled));
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00021244 File Offset: 0x0001F444
		private void OnVassalOrMercenaryServiceOfferCanceled(Kingdom offeredKingdom)
		{
			if (Campaign.Current.CampaignInformationManager.InformationDataExists<VassalOfferMapNotification>((VassalOfferMapNotification x) => x.OfferedKingdom == offeredKingdom))
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00021281 File Offset: 0x0001F481
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x040002BA RID: 698
		private readonly Kingdom _offeredKingdom;
	}
}
