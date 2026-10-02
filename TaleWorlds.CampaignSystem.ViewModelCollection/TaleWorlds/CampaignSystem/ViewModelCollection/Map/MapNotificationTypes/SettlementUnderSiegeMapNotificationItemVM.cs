using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000059 RID: 89
	public class SettlementUnderSiegeMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x0600066F RID: 1647 RVA: 0x00021044 File Offset: 0x0001F244
		public SettlementUnderSiegeMapNotificationItemVM(SettlementUnderSiegeMapNotification data)
			: base(data)
		{
			this._settlement = data.BesiegedSettlement;
			base.NotificationIdentifier = "settlementundersiege";
			this._onInspect = delegate
			{
				base.GoToMapPosition(this._settlement.Position);
			};
			CampaignEvents.OnSiegeEventEndedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventEnded));
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00021098 File Offset: 0x0001F298
		private void OnSiegeEventEnded(SiegeEvent obj)
		{
			if (obj.BesiegedSettlement == this._settlement)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x000210AE File Offset: 0x0001F2AE
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.OnSiegeEventEndedEvent.ClearListeners(this);
		}

		// Token: 0x040002B9 RID: 697
		private Settlement _settlement;
	}
}
