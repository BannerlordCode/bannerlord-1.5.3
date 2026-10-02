using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004CF RID: 1231
	public static class DestroyClanAction
	{
		// Token: 0x06004D63 RID: 19811 RVA: 0x00187528 File Offset: 0x00185728
		private static void ApplyInternal(Clan destroyedClan, DestroyClanAction.DestroyClanActionDetails details)
		{
			CampaignEventDispatcher.Instance.OnClanDestroyed(destroyedClan);
			destroyedClan.DeactivateClan();
			foreach (WarPartyComponent warPartyComponent in destroyedClan.WarPartyComponents.ToList<WarPartyComponent>())
			{
				PartyBase partyBase = null;
				if (warPartyComponent.MobileParty.MapEvent != null)
				{
					partyBase = warPartyComponent.MobileParty.MapEventSide.OtherSide.LeaderParty;
					if (warPartyComponent.MobileParty.MapEvent != MobileParty.MainParty.MapEvent)
					{
						warPartyComponent.MobileParty.MapEventSide = null;
					}
				}
				DestroyPartyAction.Apply(partyBase, warPartyComponent.MobileParty);
			}
			List<Hero> list = destroyedClan.Heroes.Where<Hero>((Hero x) => x.IsAlive).ToList<Hero>();
			for (int i = 0; i < list.Count; i++)
			{
				Hero hero = list[i];
				if (details != DestroyClanAction.DestroyClanActionDetails.ClanLeaderDeath || hero != destroyedClan.Leader)
				{
					KillCharacterAction.ApplyByRemove(hero, false, true);
				}
			}
			if (details != DestroyClanAction.DestroyClanActionDetails.ClanLeaderDeath && destroyedClan.Leader != null && destroyedClan.Leader.IsAlive && destroyedClan.Leader.DeathMark == KillCharacterAction.KillCharacterActionDetail.None)
			{
				KillCharacterAction.ApplyByRemove(destroyedClan.Leader, false, true);
			}
			if (!destroyedClan.Settlements.IsEmpty<Settlement>())
			{
				Clan clan = FactionHelper.ChooseHeirClanForFiefs(destroyedClan);
				foreach (Settlement settlement in destroyedClan.Settlements.ToList<Settlement>())
				{
					if (settlement.IsTown || settlement.IsCastle)
					{
						Hero randomElementWithPredicate = clan.AliveLords.GetRandomElementWithPredicate<Hero>((Hero x) => !x.IsChild);
						ChangeOwnerOfSettlementAction.ApplyByDestroyClan(settlement, randomElementWithPredicate);
					}
				}
			}
			FactionManager.Instance.RemoveFactionsFromCampaignWars(destroyedClan);
			if (destroyedClan.Kingdom != null)
			{
				ChangeKingdomAction.ApplyByLeaveKingdomByClanDestruction(destroyedClan, true);
			}
			if (destroyedClan.IsRebelClan)
			{
				Campaign.Current.CampaignObjectManager.RemoveClan(destroyedClan);
			}
		}

		// Token: 0x06004D64 RID: 19812 RVA: 0x0018774C File Offset: 0x0018594C
		public static void Apply(Clan destroyedClan)
		{
			DestroyClanAction.ApplyInternal(destroyedClan, DestroyClanAction.DestroyClanActionDetails.Default);
		}

		// Token: 0x06004D65 RID: 19813 RVA: 0x00187755 File Offset: 0x00185955
		public static void ApplyByFailedRebellion(Clan failedRebellionClan)
		{
			DestroyClanAction.ApplyInternal(failedRebellionClan, DestroyClanAction.DestroyClanActionDetails.RebellionFailure);
		}

		// Token: 0x06004D66 RID: 19814 RVA: 0x0018775E File Offset: 0x0018595E
		public static void ApplyByClanLeaderDeath(Clan destroyedClan)
		{
			DestroyClanAction.ApplyInternal(destroyedClan, DestroyClanAction.DestroyClanActionDetails.ClanLeaderDeath);
		}

		// Token: 0x020008E1 RID: 2273
		private enum DestroyClanActionDetails
		{
			// Token: 0x0400267A RID: 9850
			Default,
			// Token: 0x0400267B RID: 9851
			RebellionFailure,
			// Token: 0x0400267C RID: 9852
			ClanLeaderDeath
		}
	}
}
