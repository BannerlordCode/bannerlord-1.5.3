using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200003D RID: 61
	public class AlleyUnderAttackMapNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005DE RID: 1502 RVA: 0x0001F29C File Offset: 0x0001D49C
		public AlleyUnderAttackMapNotificationItemVM(AlleyUnderAttackMapNotification data)
			: base(data)
		{
			this._alley = data.Alley;
			base.NotificationIdentifier = "alley_under_attack";
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEnter));
			this._onInspect = delegate
			{
				base.GoToMapPosition(this._alley.Settlement.Position);
			};
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001F2F0 File Offset: 0x0001D4F0
		private void OnSettlementEnter(MobileParty party, Settlement settlement, Hero hero)
		{
			if (party != null && party.IsMainParty && settlement == this._alley.Settlement)
			{
				CampaignEventDispatcher.Instance.RemoveListeners(this);
				base.ExecuteRemove();
			}
		}

		// Token: 0x04000282 RID: 642
		private Alley _alley;
	}
}
