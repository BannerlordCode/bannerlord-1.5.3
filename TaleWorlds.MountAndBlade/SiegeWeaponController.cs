using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000169 RID: 361
	public class SiegeWeaponController
	{
		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x0600129D RID: 4765 RVA: 0x0003A936 File Offset: 0x00038B36
		public MBReadOnlyList<SiegeWeapon> SelectedWeapons
		{
			get
			{
				return this._selectedWeapons;
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600129E RID: 4766 RVA: 0x0003A940 File Offset: 0x00038B40
		// (remove) Token: 0x0600129F RID: 4767 RVA: 0x0003A978 File Offset: 0x00038B78
		public event Action<SiegeWeaponOrderType, IEnumerable<SiegeWeapon>> OnOrderIssued;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060012A0 RID: 4768 RVA: 0x0003A9B0 File Offset: 0x00038BB0
		// (remove) Token: 0x060012A1 RID: 4769 RVA: 0x0003A9E8 File Offset: 0x00038BE8
		public event Action OnSelectedSiegeWeaponsChanged;

		// Token: 0x060012A2 RID: 4770 RVA: 0x0003AA1D File Offset: 0x00038C1D
		public SiegeWeaponController(Mission mission, Team team)
		{
			this._mission = mission;
			this._team = team;
			this._selectedWeapons = new MBList<SiegeWeapon>();
			this.InitializeWeaponsForDeployment();
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0003AA44 File Offset: 0x00038C44
		private void InitializeWeaponsForDeployment()
		{
			IEnumerable<SiegeWeapon> enumerable = from w in (from dp in this._mission.ActiveMissionObjects.FindAllWithType<DeploymentPoint>()
					where dp.Side == this._team.Side
					select dp).SelectMany<DeploymentPoint, SynchedMissionObject>((DeploymentPoint dp) => dp.DeployableWeapons)
				select w as SiegeWeapon;
			this._availableWeapons = enumerable.ToList<SiegeWeapon>();
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0003AAC8 File Offset: 0x00038CC8
		private void InitializeWeapons()
		{
			this._availableWeapons = new List<SiegeWeapon>();
			this._availableWeapons.AddRange(from w in this._mission.ActiveMissionObjects.FindAllWithType<RangedSiegeWeapon>()
				where w.Side == this._team.Side
				select w);
			if (this._team.Side == BattleSideEnum.Attacker)
			{
				this._availableWeapons.AddRange(from w in this._mission.ActiveMissionObjects.FindAllWithType<SiegeWeapon>()
					where w is IPrimarySiegeWeapon && !(w is RangedSiegeWeapon)
					select w);
			}
			this._availableWeapons.Sort((SiegeWeapon w1, SiegeWeapon w2) => this.GetShortcutIndexOf(w1).CompareTo(this.GetShortcutIndexOf(w2)));
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0003AB70 File Offset: 0x00038D70
		public void Select(SiegeWeapon weapon)
		{
			if (this.SelectedWeapons.Contains(weapon) || !SiegeWeaponController.IsWeaponSelectable(weapon))
			{
				Debug.FailedAssert("Weapon already selected or is not selectable", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\SiegeWeaponController.cs", "Select", 82);
				return;
			}
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new SelectSiegeWeapon(weapon.Id));
				GameNetwork.EndModuleEventAsClient();
			}
			this._selectedWeapons.Add(weapon);
			Action onSelectedSiegeWeaponsChanged = this.OnSelectedSiegeWeaponsChanged;
			if (onSelectedSiegeWeaponsChanged == null)
			{
				return;
			}
			onSelectedSiegeWeaponsChanged();
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x0003ABE7 File Offset: 0x00038DE7
		public void ClearSelectedWeapons()
		{
			bool isClient = GameNetwork.IsClient;
			this._selectedWeapons.Clear();
			Action onSelectedSiegeWeaponsChanged = this.OnSelectedSiegeWeaponsChanged;
			if (onSelectedSiegeWeaponsChanged == null)
			{
				return;
			}
			onSelectedSiegeWeaponsChanged();
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x0003AC0C File Offset: 0x00038E0C
		public void Deselect(SiegeWeapon weapon)
		{
			if (!this.SelectedWeapons.Contains(weapon))
			{
				Debug.FailedAssert("Trying to deselect an unselected weapon", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\SiegeWeaponController.cs", "Deselect", 113);
				return;
			}
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new UnselectSiegeWeapon(weapon.Id));
				GameNetwork.EndModuleEventAsClient();
			}
			this._selectedWeapons.Remove(weapon);
			Action onSelectedSiegeWeaponsChanged = this.OnSelectedSiegeWeaponsChanged;
			if (onSelectedSiegeWeaponsChanged == null)
			{
				return;
			}
			onSelectedSiegeWeaponsChanged();
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x0003AC7C File Offset: 0x00038E7C
		public void SelectAll()
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new SelectAllSiegeWeapons());
				GameNetwork.EndModuleEventAsClient();
			}
			this._selectedWeapons.Clear();
			foreach (SiegeWeapon siegeWeapon in this._availableWeapons)
			{
				if (SiegeWeaponController.IsWeaponSelectable(siegeWeapon))
				{
					this._selectedWeapons.Add(siegeWeapon);
				}
			}
			Action onSelectedSiegeWeaponsChanged = this.OnSelectedSiegeWeaponsChanged;
			if (onSelectedSiegeWeaponsChanged == null)
			{
				return;
			}
			onSelectedSiegeWeaponsChanged();
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0003AD14 File Offset: 0x00038F14
		public static bool IsWeaponSelectable(SiegeWeapon weapon)
		{
			return !weapon.IsDestroyed && !weapon.IsDeactivated;
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0003AD2C File Offset: 0x00038F2C
		public static SiegeWeaponOrderType GetActiveOrderOf(SiegeWeapon weapon)
		{
			if (!weapon.ForcedUse)
			{
				return SiegeWeaponOrderType.Stop;
			}
			if (!(weapon is RangedSiegeWeapon))
			{
				return SiegeWeaponOrderType.Attack;
			}
			switch (((RangedSiegeWeapon)weapon).Focus)
			{
			case RangedSiegeWeapon.FiringFocus.Troops:
				return SiegeWeaponOrderType.FireAtTroops;
			case RangedSiegeWeapon.FiringFocus.Walls:
				return SiegeWeaponOrderType.FireAtWalls;
			case RangedSiegeWeapon.FiringFocus.RangedSiegeWeapons:
				return SiegeWeaponOrderType.FireAtRangedSiegeWeapons;
			case RangedSiegeWeapon.FiringFocus.PrimarySiegeWeapons:
				return SiegeWeaponOrderType.FireAtPrimarySiegeWeapons;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\SiegeWeaponController.cs", "GetActiveOrderOf", 169);
				return SiegeWeaponOrderType.FireAtTroops;
			}
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x0003AD93 File Offset: 0x00038F93
		public static SiegeWeaponOrderType GetActiveMovementOrderOf(SiegeWeapon weapon)
		{
			if (!weapon.ForcedUse)
			{
				return SiegeWeaponOrderType.Stop;
			}
			return SiegeWeaponOrderType.Attack;
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0003ADA0 File Offset: 0x00038FA0
		public static SiegeWeaponOrderType GetActiveFacingOrderOf(SiegeWeapon weapon)
		{
			if (!(weapon is RangedSiegeWeapon))
			{
				return SiegeWeaponOrderType.FireAtWalls;
			}
			switch (((RangedSiegeWeapon)weapon).Focus)
			{
			case RangedSiegeWeapon.FiringFocus.Troops:
				return SiegeWeaponOrderType.FireAtTroops;
			case RangedSiegeWeapon.FiringFocus.Walls:
				return SiegeWeaponOrderType.FireAtWalls;
			case RangedSiegeWeapon.FiringFocus.RangedSiegeWeapons:
				return SiegeWeaponOrderType.FireAtRangedSiegeWeapons;
			case RangedSiegeWeapon.FiringFocus.PrimarySiegeWeapons:
				return SiegeWeaponOrderType.FireAtPrimarySiegeWeapons;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\SiegeWeaponController.cs", "GetActiveFacingOrderOf", 207);
				return SiegeWeaponOrderType.FireAtTroops;
			}
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0003ADFD File Offset: 0x00038FFD
		public static SiegeWeaponOrderType GetActiveFiringOrderOf(SiegeWeapon weapon)
		{
			if (!weapon.ForcedUse)
			{
				return SiegeWeaponOrderType.Stop;
			}
			return SiegeWeaponOrderType.Attack;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0003AE0A File Offset: 0x0003900A
		public static SiegeWeaponOrderType GetActiveAIControlOrderOf(SiegeWeapon weapon)
		{
			if (weapon.ForcedUse)
			{
				return SiegeWeaponOrderType.AIControlOn;
			}
			return SiegeWeaponOrderType.AIControlOff;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0003AE18 File Offset: 0x00039018
		private void SetOrderAux(SiegeWeaponOrderType order, SiegeWeapon weapon)
		{
			switch (order)
			{
			case SiegeWeaponOrderType.Stop:
			case SiegeWeaponOrderType.AIControlOff:
				weapon.SetForcedUse(false);
				return;
			case SiegeWeaponOrderType.Attack:
			case SiegeWeaponOrderType.AIControlOn:
				weapon.SetForcedUse(true);
				return;
			case SiegeWeaponOrderType.FireAtWalls:
			{
				weapon.SetForcedUse(true);
				RangedSiegeWeapon rangedSiegeWeapon = weapon as RangedSiegeWeapon;
				if (rangedSiegeWeapon != null)
				{
					rangedSiegeWeapon.Focus = RangedSiegeWeapon.FiringFocus.Walls;
					return;
				}
				break;
			}
			case SiegeWeaponOrderType.FireAtTroops:
			{
				weapon.SetForcedUse(true);
				RangedSiegeWeapon rangedSiegeWeapon2 = weapon as RangedSiegeWeapon;
				if (rangedSiegeWeapon2 != null)
				{
					rangedSiegeWeapon2.Focus = RangedSiegeWeapon.FiringFocus.Troops;
					return;
				}
				break;
			}
			case SiegeWeaponOrderType.FireAtRangedSiegeWeapons:
			{
				weapon.SetForcedUse(true);
				RangedSiegeWeapon rangedSiegeWeapon3 = weapon as RangedSiegeWeapon;
				if (rangedSiegeWeapon3 != null)
				{
					rangedSiegeWeapon3.Focus = RangedSiegeWeapon.FiringFocus.RangedSiegeWeapons;
					return;
				}
				break;
			}
			case SiegeWeaponOrderType.FireAtPrimarySiegeWeapons:
			{
				weapon.SetForcedUse(true);
				RangedSiegeWeapon rangedSiegeWeapon4 = weapon as RangedSiegeWeapon;
				if (rangedSiegeWeapon4 != null)
				{
					rangedSiegeWeapon4.Focus = RangedSiegeWeapon.FiringFocus.PrimarySiegeWeapons;
					return;
				}
				break;
			}
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\SiegeWeaponController.cs", "SetOrderAux", 297);
				break;
			}
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0003AEDC File Offset: 0x000390DC
		public void SetOrder(SiegeWeaponOrderType order)
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ApplySiegeWeaponOrder(order));
				GameNetwork.EndModuleEventAsClient();
			}
			foreach (SiegeWeapon siegeWeapon in this.SelectedWeapons)
			{
				this.SetOrderAux(order, siegeWeapon);
			}
			Action<SiegeWeaponOrderType, IEnumerable<SiegeWeapon>> onOrderIssued = this.OnOrderIssued;
			if (onOrderIssued == null)
			{
				return;
			}
			onOrderIssued(order, this.SelectedWeapons);
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0003AF64 File Offset: 0x00039164
		public int GetShortcutIndexOf(SiegeWeapon weapon)
		{
			FormationAI.BehaviorSide sideOf = SiegeWeaponController.GetSideOf(weapon);
			int num = ((sideOf == FormationAI.BehaviorSide.Left) ? 1 : ((sideOf == FormationAI.BehaviorSide.Right) ? 2 : 0));
			if (!(weapon is IPrimarySiegeWeapon))
			{
				num += 3;
			}
			return num;
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0003AF94 File Offset: 0x00039194
		private static FormationAI.BehaviorSide GetSideOf(SiegeWeapon weapon)
		{
			IPrimarySiegeWeapon primarySiegeWeapon = weapon as IPrimarySiegeWeapon;
			if (primarySiegeWeapon != null)
			{
				return primarySiegeWeapon.WeaponSide;
			}
			if (weapon is RangedSiegeWeapon)
			{
				return FormationAI.BehaviorSide.Middle;
			}
			Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\SiegeWeaponController.cs", "GetSideOf", 349);
			return FormationAI.BehaviorSide.Middle;
		}

		// Token: 0x04000497 RID: 1175
		private readonly Mission _mission;

		// Token: 0x04000498 RID: 1176
		private readonly Team _team;

		// Token: 0x04000499 RID: 1177
		private List<SiegeWeapon> _availableWeapons;

		// Token: 0x0400049A RID: 1178
		private MBList<SiegeWeapon> _selectedWeapons;
	}
}
