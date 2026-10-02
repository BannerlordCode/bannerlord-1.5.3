using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200005A RID: 90
	public class TraitChangedNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x06000673 RID: 1651 RVA: 0x000210D4 File Offset: 0x0001F2D4
		public TraitChangedNotificationItemVM(TraitChangedMapNotification data)
			: base(data)
		{
			int currentTraitLevel = data.CurrentTraitLevel;
			int previousTraitLevel = data.PreviousTraitLevel;
			if (currentTraitLevel == 0 && previousTraitLevel != 0)
			{
				base.NotificationIdentifier = "traitlost_" + data.Trait.StringId.ToLower() + "_by_" + ((previousTraitLevel > 0) ? "decrease" : "increase");
			}
			else
			{
				base.NotificationIdentifier = "traitgained_" + data.Trait.StringId.ToLower() + "_" + currentTraitLevel.ToString();
			}
			this._onInspect = delegate
			{
				INavigationHandler navigationHandler = base.NavigationHandler;
				if (navigationHandler != null)
				{
					navigationHandler.OpenCharacterDeveloper();
				}
				base.ExecuteRemove();
			};
		}
	}
}
