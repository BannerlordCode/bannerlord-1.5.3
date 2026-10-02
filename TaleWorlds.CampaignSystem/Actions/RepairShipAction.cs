using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004EB RID: 1259
	public static class RepairShipAction
	{
		// Token: 0x06004DDB RID: 19931 RVA: 0x00189BBB File Offset: 0x00187DBB
		private static void ApplyInternal(Ship ship, float newHitpoints, Settlement repairPort = null)
		{
			SkillLevelingManager.OnShipRepaired(ship, newHitpoints - ship.HitPoints);
			ship.HitPoints = newHitpoints;
			CampaignEventDispatcher.Instance.OnShipRepaired(ship, repairPort);
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x00189BE0 File Offset: 0x00187DE0
		public static void Apply(Ship ship, Settlement repairPort)
		{
			PartyBase owner = ship.Owner;
			if (owner.IsMobile && (owner.MobileParty.IsCaravan || owner.MobileParty.IsLordParty))
			{
				int num = (int)Campaign.Current.Models.ShipCostModel.GetShipRepairCost(ship, owner);
				GiveGoldAction.ApplyForPartyToSettlement(owner, repairPort, num, false);
			}
			RepairShipAction.ApplyInternal(ship, ship.MaxHitPoints, repairPort);
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x00189C44 File Offset: 0x00187E44
		public static void ApplyForFree(Ship ship)
		{
			RepairShipAction.ApplyInternal(ship, ship.MaxHitPoints, null);
		}

		// Token: 0x06004DDE RID: 19934 RVA: 0x00189C53 File Offset: 0x00187E53
		public static void ApplyForBanditShip(Ship ship)
		{
			if (ship.HitPoints < ship.MaxHitPoints * 0.8f)
			{
				RepairShipAction.ApplyInternal(ship, ship.MaxHitPoints * 0.8f, null);
			}
		}
	}
}
