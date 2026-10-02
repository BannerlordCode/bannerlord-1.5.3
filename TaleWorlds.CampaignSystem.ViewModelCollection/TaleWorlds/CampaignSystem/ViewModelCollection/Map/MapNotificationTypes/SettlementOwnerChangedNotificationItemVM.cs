using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000058 RID: 88
	public class SettlementOwnerChangedNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x0600066B RID: 1643 RVA: 0x00020F9C File Offset: 0x0001F19C
		public SettlementOwnerChangedNotificationItemVM(SettlementOwnerChangedMapNotification data)
			: base(data)
		{
			this._settlement = data.Settlement;
			this._newOwner = data.NewOwner;
			base.NotificationIdentifier = "settlementownerchanged";
			this._onInspect = delegate
			{
				base.GoToMapPosition(this._settlement.Position);
				base.ExecuteRemove();
			};
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00020FFC File Offset: 0x0001F1FC
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (settlement == this._settlement && newOwner != this._newOwner)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00021016 File Offset: 0x0001F216
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.OnSettlementOwnerChangedEvent.ClearListeners(this);
		}

		// Token: 0x040002B7 RID: 695
		private Settlement _settlement;

		// Token: 0x040002B8 RID: 696
		private Hero _newOwner;
	}
}
