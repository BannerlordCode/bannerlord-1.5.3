using System;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004F1 RID: 1265
	public static class StartBattleAction
	{
		// Token: 0x06004DF4 RID: 19956 RVA: 0x0018A8AC File Offset: 0x00188AAC
		private static void ApplyInternal(PartyBase attackerParty, PartyBase defenderParty, object subject, MapEvent.BattleTypes battleType)
		{
			if (defenderParty.MapEvent == null)
			{
				Campaign.Current.Models.EncounterModel.CreateMapEventComponentForEncounter(attackerParty, defenderParty, battleType);
				if (defenderParty.MapEvent == null)
				{
					return;
				}
			}
			else
			{
				BattleSideEnum battleSideEnum = BattleSideEnum.Attacker;
				if (defenderParty.Side == BattleSideEnum.Attacker)
				{
					battleSideEnum = BattleSideEnum.Defender;
				}
				attackerParty.MapEventSide = defenderParty.MapEvent.GetMapEventSide(battleSideEnum);
			}
			if (attackerParty.MapEvent != null && defenderParty.MapEvent != null)
			{
				if (defenderParty.MapEvent.IsPlayerMapEvent && !defenderParty.MapEvent.IsSallyOut && PlayerEncounter.Current != null && MobileParty.MainParty.CurrentSettlement != null)
				{
					PlayerEncounter.Current.InterruptEncounter("encounter_interrupted");
				}
				MobileParty mobileParty = attackerParty.MobileParty;
				bool flag;
				if (((mobileParty != null) ? mobileParty.Army : null) != null)
				{
					MobileParty mobileParty2 = attackerParty.MobileParty;
					if (((mobileParty2 != null) ? mobileParty2.Army.LeaderParty : null) != attackerParty.MobileParty)
					{
						flag = false;
						goto IL_0106;
					}
				}
				MobileParty mobileParty3 = defenderParty.MobileParty;
				if (((mobileParty3 != null) ? mobileParty3.Army : null) != null)
				{
					MobileParty mobileParty4 = defenderParty.MobileParty;
					flag = ((mobileParty4 != null) ? mobileParty4.Army.LeaderParty : null) == defenderParty.MobileParty;
				}
				else
				{
					flag = true;
				}
				IL_0106:
				bool flag2 = flag;
				if (flag2 && defenderParty.IsSettlement && defenderParty.MapEvent.DefenderSide.Parties.Count > 1)
				{
					flag2 = false;
				}
				CampaignEventDispatcher.Instance.OnStartBattle(attackerParty, defenderParty, subject, flag2);
			}
		}

		// Token: 0x06004DF5 RID: 19957 RVA: 0x0018A9F4 File Offset: 0x00188BF4
		public static void Apply(PartyBase attackerParty, PartyBase defenderParty)
		{
			MapEvent.BattleTypes battleTypes = MapEvent.BattleTypes.None;
			object obj = null;
			Settlement settlement;
			if (defenderParty.MapEvent == null)
			{
				if (attackerParty.MobileParty != null && attackerParty.MobileParty.IsGarrison)
				{
					settlement = attackerParty.MobileParty.CurrentSettlement;
					battleTypes = (attackerParty.MobileParty.IsTargetingPort ? MapEvent.BattleTypes.BlockadeSallyOutBattle : MapEvent.BattleTypes.SallyOut);
				}
				else if (attackerParty.MobileParty.CurrentSettlement != null)
				{
					settlement = attackerParty.MobileParty.CurrentSettlement;
				}
				else if (defenderParty.MobileParty.CurrentSettlement != null)
				{
					settlement = defenderParty.MobileParty.CurrentSettlement;
				}
				else if (attackerParty.MobileParty.BesiegedSettlement != null)
				{
					settlement = attackerParty.MobileParty.BesiegedSettlement;
					if (!defenderParty.IsSettlement)
					{
						battleTypes = MapEvent.BattleTypes.SiegeOutside;
					}
				}
				else if (defenderParty.MobileParty.BesiegedSettlement != null)
				{
					settlement = defenderParty.MobileParty.BesiegedSettlement;
					battleTypes = MapEvent.BattleTypes.SiegeOutside;
				}
				else
				{
					battleTypes = MapEvent.BattleTypes.FieldBattle;
					settlement = null;
				}
				if (settlement != null && battleTypes == MapEvent.BattleTypes.None)
				{
					if (settlement.IsTown)
					{
						battleTypes = MapEvent.BattleTypes.Siege;
						if (attackerParty.IsMobile && defenderParty.SiegeEvent != null && attackerParty.SiegeEvent != null && attackerParty.MobileParty.IsCurrentlyAtSea && attackerParty.MobileParty.IsTargetingPort)
						{
							battleTypes = MapEvent.BattleTypes.BlockadeBattle;
						}
					}
					else if (settlement.IsHideout)
					{
						battleTypes = MapEvent.BattleTypes.Hideout;
					}
					else if (settlement.IsVillage)
					{
						battleTypes = MapEvent.BattleTypes.FieldBattle;
					}
					else
					{
						Debug.FailedAssert("Missing settlement type in StartBattleAction.GetGameAction", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Actions\\StartBattleAction.cs", "Apply", 139);
					}
				}
			}
			else
			{
				Settlement mapEventSettlement = defenderParty.MapEvent.MapEventSettlement;
				if (defenderParty.MapEvent.IsFieldBattle)
				{
					battleTypes = MapEvent.BattleTypes.FieldBattle;
				}
				else if (defenderParty.MapEvent.IsRaid)
				{
					battleTypes = MapEvent.BattleTypes.Raid;
					if (defenderParty.MobileParty.IsCurrentlyAtSea && attackerParty.MobileParty.IsCurrentlyAtSea)
					{
						if (!mapEventSettlement.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Raid).AnyQ<PartyBase>((PartyBase t) => t.NumberOfHealthyMembers > 0))
						{
							defenderParty.MapEventSide = null;
							battleTypes = MapEvent.BattleTypes.FieldBattle;
						}
					}
				}
				else if (defenderParty.MapEvent.IsSiegeAssault)
				{
					battleTypes = MapEvent.BattleTypes.Siege;
				}
				else if (defenderParty.MapEvent.IsSallyOut)
				{
					battleTypes = MapEvent.BattleTypes.SallyOut;
				}
				else if (defenderParty.MapEvent.IsSiegeOutside)
				{
					battleTypes = MapEvent.BattleTypes.SiegeOutside;
				}
				else if (defenderParty.MapEvent.IsBlockade)
				{
					battleTypes = MapEvent.BattleTypes.BlockadeBattle;
				}
				else if (defenderParty.MapEvent.IsBlockadeSallyOut)
				{
					battleTypes = MapEvent.BattleTypes.BlockadeSallyOutBattle;
				}
				else
				{
					Debug.FailedAssert("Missing mapEventType?", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Actions\\StartBattleAction.cs", "Apply", 184);
				}
				settlement = mapEventSettlement;
			}
			obj = obj ?? settlement;
			StartBattleAction.ApplyInternal(attackerParty, defenderParty, obj, battleTypes);
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x0018AC66 File Offset: 0x00188E66
		public static void ApplyStartBattle(MobileParty attackerParty, MobileParty defenderParty)
		{
			StartBattleAction.ApplyInternal(attackerParty.Party, defenderParty.Party, null, MapEvent.BattleTypes.FieldBattle);
		}

		// Token: 0x06004DF7 RID: 19959 RVA: 0x0018AC7B File Offset: 0x00188E7B
		public static void ApplyStartRaid(MobileParty attackerParty, Settlement settlement)
		{
			StartBattleAction.ApplyInternal(attackerParty.Party, settlement.Party, settlement, MapEvent.BattleTypes.Raid);
		}

		// Token: 0x06004DF8 RID: 19960 RVA: 0x0018AC90 File Offset: 0x00188E90
		public static void ApplyStartSallyOut(Settlement settlement, MobileParty defenderParty)
		{
			StartBattleAction.ApplyInternal(settlement.Town.GarrisonParty.Party, defenderParty.Party, settlement, MapEvent.BattleTypes.SallyOut);
		}

		// Token: 0x06004DF9 RID: 19961 RVA: 0x0018ACAF File Offset: 0x00188EAF
		public static void ApplyStartAssaultAgainstWalls(MobileParty attackerParty, Settlement settlement)
		{
			StartBattleAction.ApplyInternal(attackerParty.Party, settlement.Party, settlement, MapEvent.BattleTypes.Siege);
		}
	}
}
