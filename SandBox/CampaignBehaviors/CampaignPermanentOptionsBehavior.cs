using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.MountAndBlade;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000D3 RID: 211
	public class CampaignPermanentOptionsBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000947 RID: 2375 RVA: 0x00043940 File Offset: 0x00041B40
		public override void RegisterEvents()
		{
			CampaignEvents.RulingClanChanged.AddNonSerializedListener(this, new Action<Kingdom, Clan>(this.OnRulingClanChanged));
			CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomCreated));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.OnMercenaryServiceEndedEvent.AddNonSerializedListener(this, new Action<Clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails>(this.OnMercenaryServiceEnded));
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x000439C0 File Offset: 0x00041BC0
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x000439C2 File Offset: 0x00041BC2
		private void OnGameLoaded(CampaignGameStarter starter)
		{
			if (this.CheckKingPlaythroughIsCompleted() || this.CheckVassalPlaythroughIsCompleted() || this.CheckMercenaryPlaythroughIsCompleted())
			{
				BannerlordConfig.Save();
			}
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x000439E2 File Offset: 0x00041BE2
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			if (newKingdom != null && (this.CheckVassalPlaythroughIsCompleted() || this.CheckMercenaryPlaythroughIsCompleted()))
			{
				BannerlordConfig.Save();
			}
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x000439FD File Offset: 0x00041BFD
		private void OnRulingClanChanged(Kingdom kingdom, Clan oldRulingClan)
		{
			if (this.CheckKingPlaythroughIsCompleted())
			{
				BannerlordConfig.Save();
			}
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00043A0D File Offset: 0x00041C0D
		private void OnMercenaryServiceEnded(Clan clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails details)
		{
			if (clan == Clan.PlayerClan && this.CheckVassalPlaythroughIsCompleted())
			{
				BannerlordConfig.Save();
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00043A25 File Offset: 0x00041C25
		private void OnKingdomCreated(Kingdom kingdom)
		{
			if (this.CheckKingPlaythroughIsCompleted())
			{
				BannerlordConfig.Save();
			}
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00043A35 File Offset: 0x00041C35
		private bool CheckKingPlaythroughIsCompleted()
		{
			return Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom.RulingClan == Clan.PlayerClan && this.TryUnlockKingPlaythrough();
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00043A61 File Offset: 0x00041C61
		private bool CheckVassalPlaythroughIsCompleted()
		{
			return Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom.RulingClan != Clan.PlayerClan && !Clan.PlayerClan.IsUnderMercenaryService && this.TryUnlockVassalPlaythrough();
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00043A99 File Offset: 0x00041C99
		private bool CheckMercenaryPlaythroughIsCompleted()
		{
			return Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom.RulingClan != Clan.PlayerClan && Clan.PlayerClan.IsUnderMercenaryService && this.TryUnlockMercenaryPlaythrough();
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00043AD4 File Offset: 0x00041CD4
		private bool TryUnlockKingPlaythrough()
		{
			bool flag = !BannerlordConfig.CompletedKingPlaythrough;
			BannerlordConfig.CompletedKingPlaythrough = true;
			bool flag2 = this.TryUnlockVassalPlaythrough();
			return flag || flag2;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x00043AF8 File Offset: 0x00041CF8
		private bool TryUnlockVassalPlaythrough()
		{
			bool flag = !BannerlordConfig.CompletedVassalPlaythrough;
			BannerlordConfig.CompletedVassalPlaythrough = true;
			bool flag2 = this.TryUnlockMercenaryPlaythrough();
			return flag || flag2;
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x00043B1C File Offset: 0x00041D1C
		private bool TryUnlockMercenaryPlaythrough()
		{
			bool flag = !BannerlordConfig.CompletedMercenaryPlaythrough;
			BannerlordConfig.CompletedMercenaryPlaythrough = true;
			return flag;
		}
	}
}
