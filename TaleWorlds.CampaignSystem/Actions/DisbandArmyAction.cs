using System;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004D4 RID: 1236
	public static class DisbandArmyAction
	{
		// Token: 0x06004D72 RID: 19826 RVA: 0x00187A14 File Offset: 0x00185C14
		private static void ApplyInternal(Army army, Army.ArmyDispersionReason reason)
		{
			if (reason == Army.ArmyDispersionReason.DismissalRequestedWithInfluence)
			{
				DiplomacyModel diplomacyModel = Campaign.Current.Models.DiplomacyModel;
				ChangeClanInfluenceAction.Apply(Clan.PlayerClan, (float)(-(float)diplomacyModel.GetInfluenceCostOfDisbandingArmy()));
				foreach (MobileParty mobileParty in army.Parties.ToList<MobileParty>())
				{
					if (mobileParty != MobileParty.MainParty && mobileParty.LeaderHero != null)
					{
						ChangeRelationAction.ApplyPlayerRelation(mobileParty.LeaderHero, diplomacyModel.GetRelationCostOfDisbandingArmy(mobileParty == mobileParty.Army.LeaderParty), true, true);
					}
				}
			}
			army.DisperseInternal(reason);
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x00187AC8 File Offset: 0x00185CC8
		public static void ApplyByReleasedByPlayerAfterBattle(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.DismissalRequestedWithInfluence);
		}

		// Token: 0x06004D74 RID: 19828 RVA: 0x00187AD1 File Offset: 0x00185CD1
		public static void ApplyByArmyLeaderIsDead(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.ArmyLeaderIsDead);
		}

		// Token: 0x06004D75 RID: 19829 RVA: 0x00187ADB File Offset: 0x00185CDB
		public static void ApplyByNotEnoughParty(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.NotEnoughParty);
		}

		// Token: 0x06004D76 RID: 19830 RVA: 0x00187AE4 File Offset: 0x00185CE4
		public static void ApplyByObjectiveFinished(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.ObjectiveFinished);
		}

		// Token: 0x06004D77 RID: 19831 RVA: 0x00187AED File Offset: 0x00185CED
		public static void ApplyByPlayerTakenPrisoner(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.PlayerTakenPrisoner);
		}

		// Token: 0x06004D78 RID: 19832 RVA: 0x00187AF6 File Offset: 0x00185CF6
		public static void ApplyByFoodProblem(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.FoodProblem);
		}

		// Token: 0x06004D79 RID: 19833 RVA: 0x00187B00 File Offset: 0x00185D00
		public static void ApplyByInactivity(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.Inactivity);
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x00187B0A File Offset: 0x00185D0A
		public static void ApplyByCohesionDepleted(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.CohesionDepleted);
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x00187B13 File Offset: 0x00185D13
		public static void ApplyByNoActiveWar(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.NoActiveWar);
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x00187B1D File Offset: 0x00185D1D
		public static void ApplyByUnknownReason(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.Unknown);
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x00187B26 File Offset: 0x00185D26
		public static void ApplyByLeaderPartyRemoved(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.LeaderPartyRemoved);
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x00187B2F File Offset: 0x00185D2F
		public static void ApplyByNoShip(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.NoShipToUse);
		}
	}
}
