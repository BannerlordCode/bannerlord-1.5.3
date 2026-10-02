using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200005D RID: 93
	public class WarNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x0600067B RID: 1659 RVA: 0x000212AC File Offset: 0x0001F4AC
		public WarNotificationItemVM(WarMapNotification data)
			: base(data)
		{
			base.NotificationIdentifier = "battle";
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnPeaceMade));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			this._otherFaction = ((data.FirstFaction == Hero.MainHero.MapFaction) ? data.SecondFaction : data.FirstFaction);
			if (this._otherFaction.IsKingdomFaction)
			{
				this._onInspect = delegate
				{
					INavigationHandler navigationHandler = base.NavigationHandler;
					if (navigationHandler == null)
					{
						return;
					}
					navigationHandler.OpenKingdom(this._otherFaction);
				};
				return;
			}
			this._onInspect = null;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00021346 File Offset: 0x0001F546
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.MakePeace.ClearListeners(this);
			CampaignEvents.OnClanChangedKingdomEvent.ClearListeners(this);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00021364 File Offset: 0x0001F564
		private void OnPeaceMade(IFaction faction1, IFaction faction2, MakePeaceAction.MakePeaceDetail detail)
		{
			if ((faction1 == Hero.MainHero.Clan && this._otherFaction == faction2) || (faction2 == Hero.MainHero.Clan && this._otherFaction == faction1))
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00021398 File Offset: 0x0001F598
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x040002BB RID: 699
		private readonly IFaction _otherFaction;
	}
}
