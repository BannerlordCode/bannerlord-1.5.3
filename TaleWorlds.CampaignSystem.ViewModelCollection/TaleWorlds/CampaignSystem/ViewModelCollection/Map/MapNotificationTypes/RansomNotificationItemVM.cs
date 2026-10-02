using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000056 RID: 86
	public class RansomNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000666 RID: 1638 RVA: 0x00020E90 File Offset: 0x0001F090
		public RansomNotificationItemVM(RansomOfferMapNotification data)
			: base(data)
		{
			RansomNotificationItemVM <>4__this = this;
			this._hero = data.CaptiveHero;
			this._onInspect = delegate
			{
				<>4__this._playerInspectedNotification = true;
				CampaignEventDispatcher.Instance.OnRansomOfferedToPlayer(data.CaptiveHero);
				<>4__this.ExecuteRemove();
			};
			CampaignEvents.OnRansomOfferCancelledEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnRansomOfferCancelled));
			base.NotificationIdentifier = "ransom";
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00020F02 File Offset: 0x0001F102
		private void OnRansomOfferCancelled(Hero captiveHero)
		{
			if (captiveHero == this._hero)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00020F13 File Offset: 0x0001F113
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.OnRansomOfferCancelledEvent.ClearListeners(this);
			if (!this._playerInspectedNotification)
			{
				CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._hero);
			}
		}

		// Token: 0x040002B3 RID: 691
		private bool _playerInspectedNotification;

		// Token: 0x040002B4 RID: 692
		private Hero _hero;
	}
}
