using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000474 RID: 1140
	public class VillageTradeBoundCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004A3B RID: 19003 RVA: 0x0017583C File Offset: 0x00173A3C
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.WarDeclared));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnMakePeace));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.ClanChangedKingdom));
			CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, new Action<Clan>(this.OnClanDestroyed));
		}

		// Token: 0x06004A3C RID: 19004 RVA: 0x001758EA File Offset: 0x00173AEA
		private void OnClanDestroyed(Clan obj)
		{
			this.UpdateTradeBounds();
		}

		// Token: 0x06004A3D RID: 19005 RVA: 0x001758F2 File Offset: 0x00173AF2
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004A3E RID: 19006 RVA: 0x001758F4 File Offset: 0x00173AF4
		private void ClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			this.UpdateTradeBounds();
		}

		// Token: 0x06004A3F RID: 19007 RVA: 0x001758FC File Offset: 0x00173AFC
		private void OnGameLoaded(CampaignGameStarter obj)
		{
			this.UpdateTradeBounds();
		}

		// Token: 0x06004A40 RID: 19008 RVA: 0x00175904 File Offset: 0x00173B04
		private void OnMakePeace(IFaction faction1, IFaction faction2, MakePeaceAction.MakePeaceDetail detail)
		{
			this.UpdateTradeBounds();
		}

		// Token: 0x06004A41 RID: 19009 RVA: 0x0017590C File Offset: 0x00173B0C
		private void WarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail declareWarDetail)
		{
			this.UpdateTradeBounds();
		}

		// Token: 0x06004A42 RID: 19010 RVA: 0x00175914 File Offset: 0x00173B14
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			this.UpdateTradeBounds();
		}

		// Token: 0x06004A43 RID: 19011 RVA: 0x0017591C File Offset: 0x00173B1C
		public void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
			this.UpdateTradeBounds();
		}

		// Token: 0x06004A44 RID: 19012 RVA: 0x00175924 File Offset: 0x00173B24
		private void UpdateTradeBounds()
		{
			foreach (Town town in Campaign.Current.AllCastles)
			{
				foreach (Village village in town.Villages)
				{
					village.TradeBound = Campaign.Current.Models.VillageTradeModel.GetTradeBoundToAssignForVillage(village);
				}
			}
		}
	}
}
