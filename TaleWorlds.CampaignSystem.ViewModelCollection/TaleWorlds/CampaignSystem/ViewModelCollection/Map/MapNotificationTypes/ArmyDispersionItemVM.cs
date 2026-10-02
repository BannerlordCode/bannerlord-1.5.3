using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000040 RID: 64
	public class ArmyDispersionItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x060005F0 RID: 1520 RVA: 0x0001F6D0 File Offset: 0x0001D8D0
		public ArmyDispersionItemVM(ArmyDispersionMapNotification data)
			: base(data)
		{
			ArmyDispersionItemVM <>4__this = this;
			base.NotificationIdentifier = "armydispersion";
			this._onInspect = delegate
			{
				INavigationHandler navigationHandler = <>4__this.NavigationHandler;
				if (navigationHandler != null)
				{
					navigationHandler.OpenKingdom(data.DispersedArmy);
				}
				<>4__this.ExecuteRemove();
			};
		}
	}
}
