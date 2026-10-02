using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000057 RID: 87
	public class RebellionNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000669 RID: 1641 RVA: 0x00020F40 File Offset: 0x0001F140
		public RebellionNotificationItemVM(SettlementRebellionMapNotification data)
			: base(data)
		{
			this._settlement = data.RebelliousSettlement;
			this._onInspect = (this._onInspectAction = delegate
			{
				base.GoToMapPosition(this._settlement.Position);
			});
			base.NotificationIdentifier = "rebellion";
		}

		// Token: 0x040002B5 RID: 693
		private Settlement _settlement;

		// Token: 0x040002B6 RID: 694
		protected Action _onInspectAction;
	}
}
