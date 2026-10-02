using System;
using TaleWorlds.CampaignSystem.Actions;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003F5 RID: 1013
	public class CampaignFactionManagerBehaviour : CampaignBehaviorBase
	{
		// Token: 0x06003DA0 RID: 15776 RVA: 0x00101DF8 File Offset: 0x000FFFF8
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomCreated));
			CampaignEvents.OnClanCreatedEvent.AddNonSerializedListener(this, new Action<Clan, bool>(this.OnClanCreated));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdomEvent));
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x00101E78 File Offset: 0x00100078
		private void OnClanChangedKingdomEvent(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool arg5)
		{
			CampaignFactionManagerBehaviour.RefreshFactionsAtWarWith();
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x00101E7F File Offset: 0x0010007F
		private void OnNewGameCreated(CampaignGameStarter obj)
		{
			CampaignFactionManagerBehaviour.RefreshFactionsAtWarWith();
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x00101E86 File Offset: 0x00100086
		private void OnGameLoaded(CampaignGameStarter obj)
		{
			CampaignFactionManagerBehaviour.RefreshFactionsAtWarWith();
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x00101E8D File Offset: 0x0010008D
		private void OnClanCreated(Clan obj, bool isCompanion)
		{
			CampaignFactionManagerBehaviour.RefreshFactionsAtWarWith();
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x00101E94 File Offset: 0x00100094
		private void OnKingdomCreated(Kingdom obj)
		{
			CampaignFactionManagerBehaviour.RefreshFactionsAtWarWith();
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x00101E9C File Offset: 0x0010009C
		private static void RefreshFactionsAtWarWith()
		{
			foreach (Kingdom kingdom in Kingdom.All)
			{
				kingdom.UpdateFactionsAtWarWith();
			}
			foreach (Clan clan in Clan.All)
			{
				clan.UpdateFactionsAtWarWith();
			}
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x00101F2C File Offset: 0x0010012C
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
