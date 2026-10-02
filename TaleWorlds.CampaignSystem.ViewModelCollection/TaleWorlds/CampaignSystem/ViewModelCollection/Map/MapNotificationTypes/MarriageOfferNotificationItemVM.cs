using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200004E RID: 78
	public class MarriageOfferNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x0600063B RID: 1595 RVA: 0x000202FC File Offset: 0x0001E4FC
		public MarriageOfferNotificationItemVM(MarriageOfferMapNotification data)
			: base(data)
		{
			this._suitor = data.Suitor;
			this._maiden = data.Maiden;
			base.NotificationIdentifier = "marriage";
			this._onInspect = delegate
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferedToPlayer(this._suitor, this._maiden);
				this._playerInspectedNotification = true;
				Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
				base.ExecuteRemove();
			};
			CampaignEvents.OnMarriageOfferCanceledEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnMarriageOfferCanceled));
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0002035C File Offset: 0x0001E55C
		private void OnMarriageOfferCanceled(Hero suitor, Hero maiden)
		{
			if (Campaign.Current.CampaignInformationManager.InformationDataExists<MarriageOfferMapNotification>((MarriageOfferMapNotification x) => x.Suitor == suitor && x.Maiden == maiden))
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x000203A0 File Offset: 0x0001E5A0
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			if (!this._playerInspectedNotification)
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._suitor, this._maiden);
			}
		}

		// Token: 0x0400029C RID: 668
		private bool _playerInspectedNotification;

		// Token: 0x0400029D RID: 669
		private readonly Hero _suitor;

		// Token: 0x0400029E RID: 670
		private readonly Hero _maiden;
	}
}
