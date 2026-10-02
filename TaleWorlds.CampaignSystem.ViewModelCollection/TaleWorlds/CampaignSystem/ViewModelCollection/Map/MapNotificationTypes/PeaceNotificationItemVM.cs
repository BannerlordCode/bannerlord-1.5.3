using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x02000052 RID: 82
	public class PeaceNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x0600064C RID: 1612 RVA: 0x000207B4 File Offset: 0x0001E9B4
		public PeaceNotificationItemVM(PeaceMapNotification data)
			: base(data)
		{
			base.NotificationIdentifier = "peace";
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
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

		// Token: 0x0600064D RID: 1613 RVA: 0x0002084E File Offset: 0x0001EA4E
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.WarDeclared.ClearListeners(this);
			CampaignEvents.OnClanChangedKingdomEvent.ClearListeners(this);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0002086C File Offset: 0x0001EA6C
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
		{
			if ((faction1 == Hero.MainHero.Clan && this._otherFaction == faction2) || (faction2 == Hero.MainHero.Clan && this._otherFaction == faction1))
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x000208A0 File Offset: 0x0001EAA0
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x040002A6 RID: 678
		private readonly IFaction _otherFaction;
	}
}
