using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000155 RID: 341
	public class OrderController
	{
		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001194 RID: 4500 RVA: 0x00032974 File Offset: 0x00030B74
		// (set) Token: 0x06001195 RID: 4501 RVA: 0x0003297C File Offset: 0x00030B7C
		public SiegeWeaponController SiegeWeaponController { get; private set; }

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x00032985 File Offset: 0x00030B85
		public MBReadOnlyList<Formation> SelectedFormations
		{
			get
			{
				return this._selectedFormations;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001197 RID: 4503 RVA: 0x0003298D File Offset: 0x00030B8D
		public bool FormationUpdateEnabledAfterSetOrder
		{
			get
			{
				return this._formationUpdateEnabledAfterSetOrder;
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06001198 RID: 4504 RVA: 0x00032998 File Offset: 0x00030B98
		// (remove) Token: 0x06001199 RID: 4505 RVA: 0x000329D0 File Offset: 0x00030BD0
		public event OnOrderIssuedDelegate OnOrderIssued;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600119A RID: 4506 RVA: 0x00032A08 File Offset: 0x00030C08
		// (remove) Token: 0x0600119B RID: 4507 RVA: 0x00032A40 File Offset: 0x00030C40
		public event Action OnSelectedFormationsChanged;

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x0600119C RID: 4508 RVA: 0x00032A75 File Offset: 0x00030C75
		// (set) Token: 0x0600119D RID: 4509 RVA: 0x00032A7D File Offset: 0x00030C7D
		public Dictionary<Formation, Formation> simulationFormations { get; private set; }

		// Token: 0x0600119E RID: 4510 RVA: 0x00032A88 File Offset: 0x00030C88
		public OrderController(Mission mission, Team team, Agent owner)
		{
			this._mission = mission;
			this.Team = team;
			this.Owner = owner;
			this._gesturesEnabled = true;
			this._selectedFormations = new MBList<Formation>();
			this.SiegeWeaponController = new SiegeWeaponController(mission, this.Team);
			this.simulationFormations = new Dictionary<Formation, Formation>();
			this.actualWidths = new Dictionary<Formation, float>();
			this.actualUnitSpacings = new Dictionary<Formation, int>();
			this._yellingAfterChargeOrderTimer = new MissionTimer(30f);
			this._yellingAfterChargeOrderTimer.Set(-30f);
			foreach (Formation formation in this.Team.FormationsIncludingEmpty)
			{
				formation.OnWidthChanged += this.Formation_OnWidthChanged;
				formation.OnUnitSpacingChanged += this.Formation_OnUnitSpacingChanged;
			}
			if (this.Team.IsPlayerGeneral)
			{
				foreach (Formation formation2 in this.Team.FormationsIncludingEmpty)
				{
					formation2.PlayerOwner = owner;
				}
			}
			this.CreateDefaultOrderOverrides();
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x00032BDC File Offset: 0x00030DDC
		internal void AssignDelegatesToController(OrderController newController)
		{
			newController.OnOrderIssued = this.OnOrderIssued;
			newController.OnSelectedFormationsChanged = this.OnSelectedFormationsChanged;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00032BF6 File Offset: 0x00030DF6
		private void Formation_OnUnitSpacingChanged(Formation formation)
		{
			this.actualUnitSpacings.Remove(formation);
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00032C05 File Offset: 0x00030E05
		private void Formation_OnWidthChanged(Formation formation)
		{
			this.actualWidths.Remove(formation);
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x00032C14 File Offset: 0x00030E14
		protected void OnSelectedFormationsCollectionChanged()
		{
			Action onSelectedFormationsChanged = this.OnSelectedFormationsChanged;
			if (onSelectedFormationsChanged != null)
			{
				onSelectedFormationsChanged();
			}
			foreach (Formation formation in this.SelectedFormations.Except<Formation>(this.simulationFormations.Keys))
			{
				this.simulationFormations[formation] = new Formation(null, -1);
			}
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x00032C90 File Offset: 0x00030E90
		protected virtual void SelectFormation(Formation formation, Agent selectorAgent)
		{
			if (!this._selectedFormations.Contains(formation) && this.IsFormationSelectable(formation, selectorAgent))
			{
				if (GameNetwork.IsClient)
				{
					GameNetwork.BeginModuleEventAsClient();
					GameNetwork.WriteMessage(new SelectFormation(formation.Index));
					GameNetwork.EndModuleEventAsClient();
				}
				if (selectorAgent != null && this.AreGesturesEnabled())
				{
					OrderController.PlayFormationSelectedGesture(formation, selectorAgent);
				}
				MBDebug.Print(((formation != null) ? new FormationClass?(formation.RepresentativeClass) : null) + " added to selected formations.", 0, Debug.DebugColor.White, 17592186044416UL);
				this._selectedFormations.Add(formation);
				this.OnSelectedFormationsCollectionChanged();
				return;
			}
			Debug.FailedAssert("Formation already selected or is not selectable", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "SelectFormation", 194);
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x00032D50 File Offset: 0x00030F50
		public void SelectFormation(Formation formation)
		{
			this.SelectFormation(formation, this.Owner);
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x00032D60 File Offset: 0x00030F60
		public void DeselectFormation(Formation formation)
		{
			if (this._selectedFormations.Contains(formation))
			{
				if (GameNetwork.IsClient)
				{
					GameNetwork.BeginModuleEventAsClient();
					GameNetwork.WriteMessage(new UnselectFormation(formation.Index));
					GameNetwork.EndModuleEventAsClient();
				}
				MBDebug.Print(((formation != null) ? new FormationClass?(formation.RepresentativeClass) : null) + " is removed from selected formations.", 0, Debug.DebugColor.White, 17592186044416UL);
				this._selectedFormations.Remove(formation);
				this.OnSelectedFormationsCollectionChanged();
				return;
			}
			Debug.FailedAssert("Trying to deselect an unselected formation", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "DeselectFormation", 220);
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x00032E02 File Offset: 0x00031002
		public bool IsFormationListening(Formation formation)
		{
			return this.SelectedFormations.Contains(formation);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00032E10 File Offset: 0x00031010
		public bool IsFormationSelectable(Formation formation)
		{
			return this.IsFormationSelectable(formation, this.Owner);
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00032E1F File Offset: 0x0003101F
		public bool BackupAndDisableGesturesEnabled()
		{
			bool gesturesEnabled = this._gesturesEnabled;
			this._gesturesEnabled = false;
			return gesturesEnabled;
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00032E2E File Offset: 0x0003102E
		public void RestoreGesturesEnabled(bool oldValue)
		{
			this._gesturesEnabled = oldValue;
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x00032E37 File Offset: 0x00031037
		protected bool IsFormationSelectable(Formation formation, Agent selectorAgent)
		{
			if (selectorAgent == null || formation.PlayerOwner == selectorAgent)
			{
				return formation.GetCountOfUnitsWithCondition((Agent agent) => agent.Health > 0f) > 0;
			}
			return false;
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x00032E6F File Offset: 0x0003106F
		protected bool AreGesturesEnabled()
		{
			return this._gesturesEnabled && this._mission.IsOrderGesturesEnabled() && !GameNetwork.IsClientOrReplay;
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00032E90 File Offset: 0x00031090
		protected virtual void SelectAllFormations(Agent selectorAgent, bool uiFeedback)
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new SelectAllFormations());
				GameNetwork.EndModuleEventAsClient();
			}
			if (uiFeedback && selectorAgent != null && this.AreGesturesEnabled())
			{
				selectorAgent.MakeVoice(SkinVoiceManager.VoiceType.Everyone, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
			}
			MBDebug.Print("Selected formations being cleared. Select all formations:", 0, Debug.DebugColor.White, 17592186044416UL);
			this._selectedFormations.Clear();
			IEnumerable<Formation> formationsIncludingEmpty = this.Team.FormationsIncludingEmpty;
			Func<Formation, bool> <>9__0;
			Func<Formation, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (Formation f) => f.CountOfUnits > 0 && this.IsFormationSelectable(f, selectorAgent));
			}
			foreach (Formation formation in formationsIncludingEmpty.Where<Formation>(func))
			{
				MBDebug.Print(formation.RepresentativeClass + " added to selected formations.", 0, Debug.DebugColor.White, 17592186044416UL);
				this._selectedFormations.Add(formation);
			}
			this.OnSelectedFormationsCollectionChanged();
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x00032FAC File Offset: 0x000311AC
		public void SelectAllFormations(bool uiFeedback = false)
		{
			this.SelectAllFormations(this.Owner, uiFeedback);
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x00032FBC File Offset: 0x000311BC
		public void ClearSelectedFormations()
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ClearSelectedFormations());
				GameNetwork.EndModuleEventAsClient();
			}
			MBDebug.Print("Selected formations being cleared.", 0, Debug.DebugColor.White, 17592186044416UL);
			this._selectedFormations.Clear();
			this.OnSelectedFormationsCollectionChanged();
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x0003300C File Offset: 0x0003120C
		public unsafe virtual void SetOrder(OrderType orderType)
		{
			MBDebug.Print("SetOrder " + orderType + "on team", 0, Debug.DebugColor.White, 17592186044416UL);
			this.BeforeSetOrder(orderType);
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ApplyOrder(orderType));
				GameNetwork.EndModuleEventAsClient();
			}
			switch (orderType)
			{
			case OrderType.Charge:
			{
				using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Formation formation = enumerator.Current;
						formation.SetMovementOrder(MovementOrder.MovementOrderCharge);
					}
					goto IL_0799;
				}
				break;
			}
			case OrderType.ChargeWithTarget:
			case OrderType.FollowMe:
			case OrderType.FollowEntity:
			case OrderType.LookAtDirection:
			case OrderType.FormCustom:
			case OrderType.CohesionHigh:
			case OrderType.CohesionMedium:
			case OrderType.CohesionLow:
			case OrderType.RideFree:
				goto IL_0780;
			case OrderType.StandYourGround:
				break;
			case OrderType.Retreat:
				goto IL_0298;
			case OrderType.AdvanceTenPaces:
				goto IL_0154;
			case OrderType.FallBackTenPaces:
				goto IL_01BA;
			case OrderType.Advance:
				goto IL_0222;
			case OrderType.FallBack:
				goto IL_025D;
			case OrderType.LookAtEnemy:
				goto IL_02D3;
			case OrderType.ArrangementLine:
				goto IL_04B8;
			case OrderType.ArrangementCloseOrder:
				goto IL_04F9;
			case OrderType.ArrangementLoose:
				goto IL_053A;
			case OrderType.ArrangementCircular:
				goto IL_057B;
			case OrderType.ArrangementSchiltron:
				goto IL_05BC;
			case OrderType.ArrangementVee:
				goto IL_05FD;
			case OrderType.ArrangementColumn:
				goto IL_063E;
			case OrderType.ArrangementScatter:
				goto IL_067F;
			case OrderType.FormDeep:
				goto IL_06C0;
			case OrderType.FormWide:
				goto IL_0702;
			case OrderType.FormWider:
				goto IL_0741;
			case OrderType.HoldFire:
				goto IL_0318;
			case OrderType.FireAtWill:
				goto IL_0353;
			case OrderType.Mount:
				goto IL_03EB;
			case OrderType.Dismount:
				goto IL_038E;
			case OrderType.AIControlOn:
				goto IL_0448;
			case OrderType.AIControlOff:
				goto IL_0480;
			default:
				goto IL_0780;
			}
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation2 = enumerator.Current;
					formation2.SetMovementOrder(MovementOrder.MovementOrderStop);
				}
				goto IL_0799;
			}
			IL_0154:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation3 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation3);
					if (formation3.GetReadonlyMovementOrderReference().OrderEnum == MovementOrder.MovementOrderEnum.Move)
					{
						MovementOrder movementOrder = *formation3.GetReadonlyMovementOrderReference();
						movementOrder.Advance(formation3, 7f);
						formation3.SetMovementOrder(movementOrder);
					}
				}
				goto IL_0799;
			}
			IL_01BA:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation4 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation4);
					if (formation4.GetReadonlyMovementOrderReference().OrderEnum == MovementOrder.MovementOrderEnum.Move)
					{
						MovementOrder movementOrder2 = *formation4.GetReadonlyMovementOrderReference();
						movementOrder2.FallBack(formation4, 7f);
						formation4.SetMovementOrder(movementOrder2);
					}
				}
				goto IL_0799;
			}
			IL_0222:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation5 = enumerator.Current;
					formation5.SetMovementOrder(MovementOrder.MovementOrderAdvance);
				}
				goto IL_0799;
			}
			IL_025D:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation6 = enumerator.Current;
					formation6.SetMovementOrder(MovementOrder.MovementOrderFallBack);
				}
				goto IL_0799;
			}
			IL_0298:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation7 = enumerator.Current;
					formation7.SetMovementOrder(MovementOrder.MovementOrderRetreat);
				}
				goto IL_0799;
			}
			IL_02D3:
			FacingOrder facingOrderLookAtEnemy = FacingOrder.FacingOrderLookAtEnemy;
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation8 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation8);
					formation8.SetFacingOrder(facingOrderLookAtEnemy);
				}
				goto IL_0799;
			}
			IL_0318:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation9 = enumerator.Current;
					formation9.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
				}
				goto IL_0799;
			}
			IL_0353:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation10 = enumerator.Current;
					formation10.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
				}
				goto IL_0799;
			}
			IL_038E:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation11 = enumerator.Current;
					if (formation11.PhysicalClass.IsMounted() || formation11.HasAnyMountedUnit)
					{
						OrderController.TryCancelStopOrder(formation11);
					}
					formation11.SetRidingOrder(RidingOrder.RidingOrderDismount);
				}
				goto IL_0799;
			}
			IL_03EB:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation12 = enumerator.Current;
					if (formation12.PhysicalClass.IsMounted() || formation12.HasAnyMountedUnit)
					{
						OrderController.TryCancelStopOrder(formation12);
					}
					formation12.SetRidingOrder(RidingOrder.RidingOrderMount);
				}
				goto IL_0799;
			}
			IL_0448:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation13 = enumerator.Current;
					formation13.SetControlledByAI(true, false);
				}
				goto IL_0799;
			}
			IL_0480:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation14 = enumerator.Current;
					formation14.SetControlledByAI(false, false);
				}
				goto IL_0799;
			}
			IL_04B8:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation15 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation15);
					formation15.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
				}
				goto IL_0799;
			}
			IL_04F9:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation16 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation16);
					formation16.SetArrangementOrder(ArrangementOrder.ArrangementOrderShieldWall);
				}
				goto IL_0799;
			}
			IL_053A:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation17 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation17);
					formation17.SetArrangementOrder(ArrangementOrder.ArrangementOrderLoose);
				}
				goto IL_0799;
			}
			IL_057B:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation18 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation18);
					formation18.SetArrangementOrder(ArrangementOrder.ArrangementOrderCircle);
				}
				goto IL_0799;
			}
			IL_05BC:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation19 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation19);
					formation19.SetArrangementOrder(ArrangementOrder.ArrangementOrderSquare);
				}
				goto IL_0799;
			}
			IL_05FD:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation20 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation20);
					formation20.SetArrangementOrder(ArrangementOrder.ArrangementOrderSkein);
				}
				goto IL_0799;
			}
			IL_063E:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation21 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation21);
					formation21.SetArrangementOrder(ArrangementOrder.ArrangementOrderColumn);
				}
				goto IL_0799;
			}
			IL_067F:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation22 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation22);
					formation22.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
				}
				goto IL_0799;
			}
			IL_06C0:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation23 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation23);
					formation23.SetFormOrder(FormOrder.FormOrderDeep, true);
				}
				goto IL_0799;
			}
			IL_0702:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation24 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation24);
					formation24.SetFormOrder(FormOrder.FormOrderWide, true);
				}
				goto IL_0799;
			}
			IL_0741:
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation25 = enumerator.Current;
					OrderController.TryCancelStopOrder(formation25);
					formation25.SetFormOrder(FormOrder.FormOrderWider, true);
				}
				goto IL_0799;
			}
			IL_0780:
			Debug.FailedAssert("[DEBUG]Invalid order type.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "SetOrder", 622);
			IL_0799:
			this.AfterSetOrder(orderType);
			this.FireOnOrderIssued(orderType, this.SelectedFormations, this, Array.Empty<object>());
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x00033A28 File Offset: 0x00031C28
		private void PlayOrderGestures(OrderType orderType)
		{
			if (!LoadingWindow.IsLoadingWindowActive)
			{
				switch (orderType)
				{
				case OrderType.Move:
				case OrderType.MoveToLineSegment:
				case OrderType.MoveToLineSegmentWithHorizontalLayout:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.Move, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.Charge:
				case OrderType.ChargeWithTarget:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.Charge, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.StandYourGround:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.Stop, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.FollowMe:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.Follow, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.Retreat:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.Retreat, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.AdvanceTenPaces:
				case OrderType.Advance:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.Advance, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.FallBackTenPaces:
				case OrderType.FallBack:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.FallBack, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.LookAtEnemy:
					if (Mission.Current.IsNavalBattle)
					{
						this.Owner.MakeVoice(SkinVoiceManager.VoiceType.BoardAtWill, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					}
					else
					{
						this.Owner.MakeVoice(SkinVoiceManager.VoiceType.FaceEnemy, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					}
					break;
				case OrderType.LookAtDirection:
					if (Mission.Current.IsNavalBattle)
					{
						this.Owner.MakeVoice(SkinVoiceManager.VoiceType.AvoidBoarding, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					}
					else
					{
						this.Owner.MakeVoice(SkinVoiceManager.VoiceType.FaceDirection, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					}
					break;
				case OrderType.ArrangementLine:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.FormLine, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.ArrangementCloseOrder:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.FormShieldWall, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.ArrangementLoose:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.FormLoose, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.ArrangementCircular:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.FormCircle, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.ArrangementSchiltron:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.FormSquare, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.ArrangementVee:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.FormSkein, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.ArrangementColumn:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.FormColumn, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.ArrangementScatter:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.FormScatter, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.HoldFire:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.HoldFire, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.FireAtWill:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.FireAtWill, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.Mount:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.Mount, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.Dismount:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.Dismount, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.AIControlOn:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.CommandDelegate, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				case OrderType.AIControlOff:
					this.Owner.MakeVoice(SkinVoiceManager.VoiceType.CommandUndelegate, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					break;
				}
			}
			if (this._selectedFormations.Count > 0 && this.Owner != null && this.Owner.Controller != AgentControllerType.AI)
			{
				MissionWeapon wieldedWeapon = this.Owner.WieldedWeapon;
				switch (wieldedWeapon.IsEmpty ? WeaponClass.Undefined : wieldedWeapon.Item.PrimaryWeapon.WeaponClass)
				{
				case WeaponClass.Undefined:
				case WeaponClass.Sling:
				case WeaponClass.Stone:
				case WeaponClass.BallistaStone:
				{
					ActionIndexCache actionIndexCache;
					if (this.Owner.MountAgent == null)
					{
						Agent owner = this.Owner;
						int num = 1;
						actionIndexCache = ((orderType == OrderType.FollowMe) ? (this.Owner.GetIsLeftStance() ? ActionIndexCache.act_command_follow_unarmed_leftstance : ActionIndexCache.act_command_follow_unarmed) : (this.Owner.GetIsLeftStance() ? ActionIndexCache.act_command_unarmed_leftstance : ActionIndexCache.act_command_unarmed));
						owner.SetActionChannel(num, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						goto IL_06AE;
					}
					Agent owner2 = this.Owner;
					int num2 = 1;
					actionIndexCache = ((orderType == OrderType.FollowMe) ? ActionIndexCache.act_horse_command_follow_unarmed : ActionIndexCache.act_horse_command_unarmed);
					owner2.SetActionChannel(num2, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					goto IL_06AE;
				}
				case WeaponClass.Dagger:
				case WeaponClass.OneHandedSword:
				case WeaponClass.OneHandedAxe:
				case WeaponClass.Mace:
				case WeaponClass.Pick:
				case WeaponClass.OneHandedPolearm:
				case WeaponClass.ThrowingAxe:
				case WeaponClass.ThrowingKnife:
				{
					ActionIndexCache actionIndexCache;
					if (this.Owner.MountAgent == null)
					{
						Agent owner3 = this.Owner;
						int num3 = 1;
						actionIndexCache = ((orderType == OrderType.FollowMe) ? (this.Owner.GetIsLeftStance() ? ActionIndexCache.act_command_follow_leftstance : ActionIndexCache.act_command_follow) : (this.Owner.GetIsLeftStance() ? ActionIndexCache.act_command_leftstance : ActionIndexCache.act_command));
						owner3.SetActionChannel(num3, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						goto IL_06AE;
					}
					Agent owner4 = this.Owner;
					int num4 = 1;
					actionIndexCache = ((orderType == OrderType.FollowMe) ? ActionIndexCache.act_horse_command_follow : ActionIndexCache.act_horse_command);
					owner4.SetActionChannel(num4, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					goto IL_06AE;
				}
				case WeaponClass.TwoHandedSword:
				case WeaponClass.TwoHandedAxe:
				case WeaponClass.TwoHandedMace:
				case WeaponClass.TwoHandedPolearm:
				case WeaponClass.LowGripPolearm:
				case WeaponClass.Crossbow:
				case WeaponClass.Javelin:
				case WeaponClass.Pistol:
				case WeaponClass.Musket:
				{
					ActionIndexCache actionIndexCache;
					if (this.Owner.MountAgent == null)
					{
						Agent owner5 = this.Owner;
						int num5 = 1;
						actionIndexCache = ((orderType == OrderType.FollowMe) ? (this.Owner.GetIsLeftStance() ? ActionIndexCache.act_command_follow_2h_leftstance : ActionIndexCache.act_command_follow_2h) : (this.Owner.GetIsLeftStance() ? ActionIndexCache.act_command_2h_leftstance : ActionIndexCache.act_command_2h));
						owner5.SetActionChannel(num5, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						goto IL_06AE;
					}
					Agent owner6 = this.Owner;
					int num6 = 1;
					actionIndexCache = ((orderType == OrderType.FollowMe) ? ActionIndexCache.act_horse_command_follow_2h : ActionIndexCache.act_horse_command_2h);
					owner6.SetActionChannel(num6, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					goto IL_06AE;
				}
				case WeaponClass.Bow:
				{
					ActionIndexCache actionIndexCache;
					if (this.Owner.MountAgent == null)
					{
						Agent owner7 = this.Owner;
						int num7 = 1;
						actionIndexCache = ((orderType == OrderType.FollowMe) ? ActionIndexCache.act_command_follow_bow : ActionIndexCache.act_command_bow);
						owner7.SetActionChannel(num7, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						goto IL_06AE;
					}
					Agent owner8 = this.Owner;
					int num8 = 1;
					actionIndexCache = ((orderType == OrderType.FollowMe) ? ActionIndexCache.act_horse_command_follow_bow : ActionIndexCache.act_horse_command_bow);
					owner8.SetActionChannel(num8, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					goto IL_06AE;
				}
				case WeaponClass.Boulder:
				case WeaponClass.BallistaBoulder:
					goto IL_06AE;
				}
				Debug.FailedAssert("Unexpected weapon class.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "PlayOrderGestures", 821);
			}
			IL_06AE:
			foreach (Formation formation in this._selectedFormations)
			{
				Agent medianAgent = formation.GetMedianAgent(false, true, formation.CachedAveragePosition);
				float countOfUnitsWithCondition = (float)formation.GetCountOfUnitsWithCondition((Agent agent) => agent.IsFemale);
				int countOfUnits = formation.CountOfUnits;
				float num9 = countOfUnitsWithCondition / (float)countOfUnits;
				SoundEventParameter soundEventParameter = new SoundEventParameter("GenderRatio", num9);
				if (medianAgent != null)
				{
					Vec3 position = medianAgent.Position;
					switch (orderType)
					{
					case OrderType.Move:
					case OrderType.MoveToLineSegment:
					case OrderType.MoveToLineSegmentWithHorizontalLayout:
					case OrderType.FollowMe:
					case OrderType.AdvanceTenPaces:
					case OrderType.Advance:
						MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/nods/move"), ref soundEventParameter, position);
						continue;
					case OrderType.Charge:
					case OrderType.ChargeWithTarget:
						break;
					case OrderType.StandYourGround:
						MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/nods/stop"), ref soundEventParameter, position);
						continue;
					case OrderType.FollowEntity:
					case OrderType.Retreat:
					case OrderType.FallBackTenPaces:
					case OrderType.FallBack:
					case OrderType.LookAtEnemy:
					case OrderType.LookAtDirection:
						continue;
					case OrderType.ArrangementLine:
					case OrderType.ArrangementCloseOrder:
					case OrderType.ArrangementLoose:
					case OrderType.ArrangementCircular:
					case OrderType.ArrangementSchiltron:
					case OrderType.ArrangementVee:
					case OrderType.ArrangementColumn:
					case OrderType.ArrangementScatter:
						MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/nods/formation"), ref soundEventParameter, position);
						continue;
					default:
						if (orderType != OrderType.AttackEntity)
						{
							continue;
						}
						break;
					}
					MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/nods/attack"), ref soundEventParameter, position);
					if (this._yellingAfterChargeOrderTimer.Check(true))
					{
						formation.ApplyActionOnEachUnit(delegate(Agent yellingAgent)
						{
							yellingAgent.YellingBehaviour();
						}, null);
					}
				}
			}
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00034294 File Offset: 0x00032494
		protected static void PlayFormationSelectedGesture(Formation formation, Agent agent)
		{
			if (formation.SecondaryLogicalClasses.Any<FormationClass>())
			{
				agent.MakeVoice(SkinVoiceManager.VoiceType.MixedFormation, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
				return;
			}
			switch (formation.LogicalClass)
			{
			case FormationClass.Infantry:
				agent.MakeVoice(SkinVoiceManager.VoiceType.Infantry, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
				return;
			case FormationClass.Ranged:
				agent.MakeVoice(SkinVoiceManager.VoiceType.Archers, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
				return;
			case FormationClass.Cavalry:
				agent.MakeVoice(SkinVoiceManager.VoiceType.Cavalry, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
				return;
			case FormationClass.HorseArcher:
				agent.MakeVoice(SkinVoiceManager.VoiceType.HorseArchers, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
				return;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "PlayFormationSelectedGesture", 906);
				return;
			}
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00034328 File Offset: 0x00032528
		private unsafe void AfterSetOrder(OrderType orderType)
		{
			MBDebug.Print("After set order called, number of selected formations: " + this.SelectedFormations.Count, 0, Debug.DebugColor.White, 17592186044416UL);
			foreach (Formation formation in this.SelectedFormations)
			{
				MBDebug.Print(((formation != null) ? new FormationClass?(formation.FormationIndex) : null) + " formation being processed.", 0, Debug.DebugColor.White, 17592186044416UL);
				if (this._formationUpdateEnabledAfterSetOrder)
				{
					bool flag = false;
					if (formation.IsPlayerTroopInFormation)
					{
						flag = formation.GetReadonlyMovementOrderReference()->OrderEnum == MovementOrder.MovementOrderEnum.Follow;
					}
					formation.ApplyActionOnEachUnit(delegate(Agent agent)
					{
						agent.ForceUpdateCachedAndFormationValues(false, false);
					}, flag ? Mission.Current.MainAgent : null);
					formation.SetHasPendingUnitPositions(false);
				}
				MBDebug.Print("Update cached and formation values on each agent complete, number of selected formations: " + this.SelectedFormations.Count, 0, Debug.DebugColor.White, 17592186044416UL);
				this._mission.SetRandomDecideTimeOfAgentsWithIndices(formation.CollectUnitIndices(), null, null);
				MBDebug.Print("Set random decide time of agents with indices complete, number of selected formations: " + this.SelectedFormations.Count, 0, Debug.DebugColor.White, 17592186044416UL);
			}
			MBDebug.Print("After set order loop complete, number of selected formations: " + this.SelectedFormations.Count, 0, Debug.DebugColor.White, 17592186044416UL);
			if (this.Owner != null && this.AreGesturesEnabled())
			{
				this.PlayOrderGestures(orderType);
			}
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x0003450C File Offset: 0x0003270C
		protected void BeforeSetOrder(OrderType orderType)
		{
			foreach (Formation formation in this.SelectedFormations.Where<Formation>((Formation f) => !this.IsFormationSelectable(f, this.Owner)).ToList<Formation>())
			{
				this.DeselectFormation(formation);
			}
			if (!GameNetwork.IsClientOrReplay && orderType != OrderType.AIControlOff && orderType != OrderType.AIControlOn)
			{
				foreach (Formation formation2 in this.SelectedFormations)
				{
					if (formation2.IsAIControlled && formation2.PlayerOwner != null)
					{
						formation2.SetControlledByAI(false, false);
					}
				}
			}
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x000345DC File Offset: 0x000327DC
		public virtual void SetOrderWithAgent(OrderType orderType, Agent agent)
		{
			MBDebug.Print(string.Concat(new object[] { "SetOrderWithAgent ", orderType, " ", agent.Name, "on team" }), 0, Debug.DebugColor.White, 17592186044416UL);
			this.BeforeSetOrder(orderType);
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ApplyOrderWithAgent(orderType, agent.Index));
				GameNetwork.EndModuleEventAsClient();
			}
			if (orderType == OrderType.FollowMe)
			{
				using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Formation formation = enumerator.Current;
						formation.SetMovementOrder(MovementOrder.MovementOrderFollow(agent));
					}
					goto IL_00C5;
				}
			}
			Debug.FailedAssert("[DEBUG]Invalid order type.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "SetOrderWithAgent", 995);
			IL_00C5:
			this.AfterSetOrder(orderType);
			this.FireOnOrderIssued(orderType, this.SelectedFormations, this, new object[] { agent });
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x000346E0 File Offset: 0x000328E0
		public virtual void SetOrderWithPosition(OrderType orderType, WorldPosition orderPosition)
		{
			MBDebug.Print(string.Concat(new object[] { "SetOrderWithPosition ", orderType, " ", orderPosition, "on team" }), 0, Debug.DebugColor.White, 17592186044416UL);
			this.BeforeSetOrder(orderType);
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ApplyOrderWithPosition(orderType, orderPosition.GetGroundVec3()));
				GameNetwork.EndModuleEventAsClient();
			}
			if (orderType != OrderType.Move)
			{
				if (orderType != OrderType.LookAtDirection)
				{
					if (orderType != OrderType.FormCustom)
					{
						goto IL_015B;
					}
					goto IL_010E;
				}
			}
			else
			{
				using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Formation formation = enumerator.Current;
						formation.SetMovementOrder(MovementOrder.MovementOrderMove(orderPosition));
					}
					goto IL_0174;
				}
			}
			FacingOrder facingOrder = FacingOrder.FacingOrderLookAtDirection(OrderController.GetOrderLookAtDirection(this.SelectedFormations, orderPosition.AsVec2));
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation2 = enumerator.Current;
					formation2.SetFacingOrder(facingOrder);
				}
				goto IL_0174;
			}
			IL_010E:
			float orderFormCustomWidth = OrderController.GetOrderFormCustomWidth(this.SelectedFormations, orderPosition.GetGroundVec3());
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation3 = enumerator.Current;
					formation3.SetFormOrder(FormOrder.FormOrderCustom(orderFormCustomWidth), true);
				}
				goto IL_0174;
			}
			IL_015B:
			Debug.FailedAssert("[DEBUG]Invalid order type.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "SetOrderWithPosition", 1045);
			IL_0174:
			this.AfterSetOrder(orderType);
			this.FireOnOrderIssued(orderType, this.SelectedFormations, this, new object[] { orderPosition });
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x000348B0 File Offset: 0x00032AB0
		public virtual void SetOrderWithFormation(OrderType orderType, Formation orderFormation)
		{
			MBDebug.Print(string.Concat(new object[] { "SetOrderWithFormation ", orderType, " ", orderFormation, "on team" }), 0, Debug.DebugColor.White, 17592186044416UL);
			this.BeforeSetOrder(orderType);
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ApplyOrderWithFormation(orderType, orderFormation.Index));
				GameNetwork.EndModuleEventAsClient();
			}
			if (orderType != OrderType.Charge)
			{
				if (orderType != OrderType.Advance)
				{
					goto IL_00F3;
				}
			}
			else
			{
				using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Formation formation = enumerator.Current;
						formation.SetMovementOrder(MovementOrder.MovementOrderCharge);
						formation.SetTargetFormation(orderFormation);
					}
					goto IL_010C;
				}
			}
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation2 = enumerator.Current;
					formation2.SetMovementOrder(MovementOrder.MovementOrderAdvance);
					formation2.SetTargetFormation(orderFormation);
				}
				goto IL_010C;
			}
			IL_00F3:
			Debug.FailedAssert("Invalid order type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "SetOrderWithFormation", 1085);
			IL_010C:
			this.AfterSetOrder(orderType);
			this.FireOnOrderIssued(orderType, this.SelectedFormations, this, new object[] { orderFormation });
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00034A04 File Offset: 0x00032C04
		public void SetOrderWithFormationAndPercentage(OrderType orderType, Formation orderFormation, float percentage)
		{
			int num = (int)(percentage * 100f);
			num = MBMath.ClampInt(num, 0, 100);
			this.BeforeSetOrder(orderType);
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ApplyOrderWithFormationAndPercentage(orderType, orderFormation.Index, num));
				GameNetwork.EndModuleEventAsClient();
			}
			Debug.FailedAssert("Invalid order type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "SetOrderWithFormationAndPercentage", 1123);
			this.AfterSetOrder(orderType);
			this.FireOnOrderIssued(orderType, this.SelectedFormations, this, new object[] { orderFormation, percentage });
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00034A90 File Offset: 0x00032C90
		public void TransferUnitWithPriorityFunction(Formation orderFormation, int number, bool hasShield, bool hasSpear, bool hasThrown, bool isHeavy, bool isRanged, bool isMounted, bool excludeBannerman, List<Agent> excludedAgents)
		{
			Func<Agent, int> func;
			TroopFilteringUtilities.GetPriorityFunction(TroopFilteringUtilities.GetFilter(isMounted, isRanged, !isRanged, isHeavy, hasThrown, hasSpear, hasShield), out func);
			this.BeforeSetOrder(OrderType.Transfer);
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ApplyOrderWithFormationAndNumber(OrderType.Transfer, orderFormation.Index, number));
				GameNetwork.EndModuleEventAsClient();
			}
			List<int> list = null;
			int num = this.SelectedFormations.Sum<Formation>((Formation f) => f.CountOfUnits);
			int num2 = number;
			int num3 = 0;
			if (this.SelectedFormations.Count > 1)
			{
				list = new List<int>();
			}
			foreach (Formation formation in this.SelectedFormations)
			{
				int countOfUnits = formation.CountOfUnits;
				int num4 = num2 * countOfUnits / num;
				if (!GameNetwork.IsClientOrReplay)
				{
					formation.OnMassUnitTransferStart();
					orderFormation.OnMassUnitTransferStart();
					formation.TransferUnitsWithPriorityFunction(orderFormation, num4, func, excludeBannerman, excludedAgents);
					formation.OnMassUnitTransferEnd();
					orderFormation.OnMassUnitTransferEnd();
				}
				if (list != null)
				{
					list.Add(num4);
				}
				num2 -= num4;
				num -= countOfUnits;
				num3 += num4;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				orderFormation.QuerySystem.Expire();
			}
			this.AfterSetOrder(OrderType.Transfer);
			if (this.OnOrderIssued != null)
			{
				if (list != null)
				{
					object[] array = new object[list.Count + 1];
					array[0] = number;
					for (int i = 0; i < list.Count; i++)
					{
						array[i + 1] = list[i];
					}
					this.FireOnOrderIssued(OrderType.Transfer, this.SelectedFormations, this, new object[] { orderFormation, array });
					return;
				}
				this.FireOnOrderIssued(OrderType.Transfer, this.SelectedFormations, this, new object[] { orderFormation, number });
			}
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00034C74 File Offset: 0x00032E74
		public void RearrangeFormationsAccordingToFilters(Team team, [TupleElementNames(new string[] { "formation", "troopCount", "troopFilter", "excludedAgents" })] List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>> MassTransferData)
		{
			team.RearrangeFormationsAccordingToFilter(MassTransferData);
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00034C80 File Offset: 0x00032E80
		public void SetOrderWithFormationAndNumber(OrderType orderType, Formation orderFormation, int number)
		{
			this.BeforeSetOrder(orderType);
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ApplyOrderWithFormationAndNumber(orderType, orderFormation.Index, number));
				GameNetwork.EndModuleEventAsClient();
			}
			List<int> list = null;
			if (orderType == OrderType.Transfer)
			{
				int num = this.SelectedFormations.Sum<Formation>((Formation f) => f.CountOfUnits);
				int num2 = number;
				int num3 = 0;
				if (this.SelectedFormations.Count > 1)
				{
					list = new List<int>();
				}
				foreach (Formation formation in this.SelectedFormations)
				{
					int countOfUnits = formation.CountOfUnits;
					int num4 = num2 * countOfUnits / num;
					if (!GameNetwork.IsClientOrReplay)
					{
						formation.OnMassUnitTransferStart();
						orderFormation.OnMassUnitTransferStart();
						formation.TransferUnitsAux(orderFormation, num4, true, num4 < countOfUnits && orderFormation.CountOfUnits > 0 && orderFormation.OrderPositionIsValid);
						formation.OnMassUnitTransferEnd();
						orderFormation.OnMassUnitTransferEnd();
					}
					if (list != null)
					{
						list.Add(num4);
					}
					num2 -= num4;
					num -= countOfUnits;
					num3 += num4;
				}
				if (!GameNetwork.IsClientOrReplay)
				{
					orderFormation.QuerySystem.Expire();
				}
			}
			else
			{
				Debug.FailedAssert("[DEBUG]Invalid order type.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "SetOrderWithFormationAndNumber", 1365);
			}
			this.AfterSetOrder(orderType);
			if (this.OnOrderIssued != null)
			{
				if (list != null)
				{
					object[] array = new object[list.Count + 1];
					array[0] = number;
					for (int i = 0; i < list.Count; i++)
					{
						array[i + 1] = list[i];
					}
					this.FireOnOrderIssued(orderType, this.SelectedFormations, this, new object[] { orderFormation, array });
					return;
				}
				this.FireOnOrderIssued(orderType, this.SelectedFormations, this, new object[] { orderFormation, number });
			}
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00034E7C File Offset: 0x0003307C
		public virtual void SetOrderWithTwoPositions(OrderType orderType, WorldPosition position1, WorldPosition position2)
		{
			MBDebug.Print(string.Concat(new object[] { "SetOrderWithTwoPositions ", orderType, " ", position1, " ", position2, "on team" }), 0, Debug.DebugColor.White, 17592186044416UL);
			this.BeforeSetOrder(orderType);
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ApplyOrderWithTwoPositions(orderType, position1.GetGroundVec3(), position2.GetGroundVec3()));
				GameNetwork.EndModuleEventAsClient();
			}
			if (orderType - OrderType.MoveToLineSegment <= 1)
			{
				bool flag = orderType == OrderType.MoveToLineSegment;
				IEnumerable<Formation> enumerable = this.SelectedFormations.Where<Formation>((Formation f) => f.CountOfUnitsWithoutDetachedOnes > 0);
				if (enumerable.Any<Formation>())
				{
					this.MoveToLineSegment(enumerable, position1, position2, flag);
				}
			}
			else
			{
				Debug.FailedAssert("Invalid order type.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "SetOrderWithTwoPositions", 1419);
			}
			this.AfterSetOrder(orderType);
			this.FireOnOrderIssued(orderType, this.SelectedFormations, this, new object[] { position1, position2 });
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00034FA4 File Offset: 0x000331A4
		public virtual void SetOrderWithOrderableObject(IOrderable target)
		{
			BattleSideEnum side = this.SelectedFormations[0].Team.Side;
			OrderType order = target.GetOrder(side);
			this.BeforeSetOrder(order);
			MissionObject missionObject = target as MissionObject;
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ApplyOrderWithMissionObject(missionObject.Id));
				GameNetwork.EndModuleEventAsClient();
			}
			switch (order)
			{
			case OrderType.Move:
				break;
			case OrderType.MoveToLineSegment:
				goto IL_01A8;
			case OrderType.MoveToLineSegmentWithHorizontalLayout:
			{
				IPointDefendable pointDefendable = target as IPointDefendable;
				Vec3 globalPosition = pointDefendable.DefencePoints.Last<DefencePoint>().GameEntity.GlobalPosition;
				Vec3 globalPosition2 = pointDefendable.DefencePoints.First<DefencePoint>().GameEntity.GlobalPosition;
				IEnumerable<Formation> enumerable = this.SelectedFormations.Where<Formation>((Formation f) => f.CountOfUnitsWithoutDetachedOnes > 0);
				if (enumerable.Any<Formation>())
				{
					WorldPosition worldPosition = new WorldPosition(this._mission.Scene, UIntPtr.Zero, globalPosition, false);
					WorldPosition worldPosition2 = new WorldPosition(this._mission.Scene, UIntPtr.Zero, globalPosition2, false);
					this.MoveToLineSegment(enumerable, worldPosition, worldPosition2, false);
					goto IL_0391;
				}
				goto IL_0391;
			}
			default:
			{
				if (order == OrderType.FollowEntity)
				{
					GameEntity waitEntity = (target as UsableMachine).WaitEntity;
					Vec2 vec = waitEntity.GetGlobalFrame().rotation.f.AsVec2.Normalized();
					foreach (Formation formation in this.SelectedFormations)
					{
						formation.SetFacingOrder(FacingOrder.FacingOrderLookAtDirection(vec));
						formation.SetMovementOrder(MovementOrder.MovementOrderFollowEntity(waitEntity));
					}
					goto IL_0391;
				}
				switch (order)
				{
				case OrderType.Use:
				{
					UsableMachine usableMachine = target as UsableMachine;
					this.ToggleSideOrderUse(this.SelectedFormations, usableMachine);
					goto IL_0391;
				}
				case OrderType.AttackEntity:
				{
					WeakGameEntity gameEntity = missionObject.GameEntity;
					using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Formation formation2 = enumerator.Current;
							formation2.SetMovementOrder(MovementOrder.MovementOrderAttackEntity(GameEntity.CreateFromWeakEntity(gameEntity), !(missionObject is CastleGate)));
						}
						goto IL_0391;
					}
					break;
				}
				case OrderType.PointDefence:
					break;
				default:
					goto IL_0391;
				}
				IPointDefendable pointDefendable2 = target as IPointDefendable;
				using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Formation formation3 = enumerator.Current;
						formation3.SetMovementOrder(MovementOrder.MovementOrderMove(pointDefendable2.MiddleFrame.Origin));
					}
					goto IL_0391;
				}
				break;
			}
			}
			WorldPosition worldPosition3 = new WorldPosition(this._mission.Scene, UIntPtr.Zero, missionObject.GameEntity.GlobalPosition, false);
			using (List<Formation>.Enumerator enumerator = this.SelectedFormations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation4 = enumerator.Current;
					formation4.SetMovementOrder(MovementOrder.MovementOrderMove(worldPosition3));
				}
				goto IL_0391;
			}
			IL_01A8:
			IPointDefendable pointDefendable3 = target as IPointDefendable;
			Vec3 globalPosition3 = pointDefendable3.DefencePoints.Last<DefencePoint>().GameEntity.GlobalPosition;
			Vec3 globalPosition4 = pointDefendable3.DefencePoints.First<DefencePoint>().GameEntity.GlobalPosition;
			IEnumerable<Formation> enumerable2 = this.SelectedFormations.Where<Formation>((Formation f) => f.CountOfUnitsWithoutDetachedOnes > 0);
			if (enumerable2.Any<Formation>())
			{
				WorldPosition worldPosition4 = new WorldPosition(this._mission.Scene, UIntPtr.Zero, globalPosition3, false);
				WorldPosition worldPosition5 = new WorldPosition(this._mission.Scene, UIntPtr.Zero, globalPosition4, false);
				this.MoveToLineSegment(enumerable2, worldPosition4, worldPosition5, true);
			}
			IL_0391:
			this.AfterSetOrder(order);
			this.FireOnOrderIssued(order, this.SelectedFormations, this, new object[] { target });
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00035398 File Offset: 0x00033598
		public unsafe static OrderType GetActiveMovementOrderOf(Formation formation)
		{
			MovementOrder movementOrder = *formation.GetReadonlyMovementOrderReference();
			switch (movementOrder.MovementState)
			{
			case MovementOrder.MovementStateEnum.Charge:
				return OrderType.Charge;
			case MovementOrder.MovementStateEnum.Hold:
			{
				movementOrder = *formation.GetReadonlyMovementOrderReference();
				OrderType orderType = movementOrder.OrderType;
				if (orderType <= OrderType.FollowMe)
				{
					if (orderType == OrderType.ChargeWithTarget)
					{
						return OrderType.Charge;
					}
					if (orderType == OrderType.FollowMe)
					{
						return OrderType.FollowMe;
					}
				}
				else
				{
					if (orderType == OrderType.Advance)
					{
						return OrderType.Advance;
					}
					if (orderType == OrderType.FallBack)
					{
						return OrderType.FallBack;
					}
				}
				return OrderType.Move;
			}
			case MovementOrder.MovementStateEnum.Retreat:
				return OrderType.Retreat;
			case MovementOrder.MovementStateEnum.StandGround:
				return OrderType.StandYourGround;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "GetActiveMovementOrderOf", 1572);
				return OrderType.Move;
			}
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0003542C File Offset: 0x0003362C
		public static OrderType GetActiveFacingOrderOf(Formation formation)
		{
			if (formation.FacingOrder.OrderType == OrderType.LookAtDirection)
			{
				return OrderType.LookAtDirection;
			}
			return OrderType.LookAtEnemy;
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x00035450 File Offset: 0x00033650
		public static OrderType GetActiveRidingOrderOf(Formation formation)
		{
			OrderType orderType = formation.RidingOrder.OrderType;
			if (orderType == OrderType.RideFree)
			{
				return OrderType.Mount;
			}
			return orderType;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x00035478 File Offset: 0x00033678
		public static OrderType GetActiveArrangementOrderOf(Formation formation)
		{
			return formation.ArrangementOrder.OrderType;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00035494 File Offset: 0x00033694
		public static OrderType GetActiveFormOrderOf(Formation formation)
		{
			return formation.FormOrder.OrderType;
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x000354B0 File Offset: 0x000336B0
		public static OrderType GetActiveFiringOrderOf(Formation formation)
		{
			return formation.FiringOrder.OrderType;
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x000354CB File Offset: 0x000336CB
		public static OrderType GetActiveAIControlOrderOf(Formation formation)
		{
			if (formation.IsAIControlled)
			{
				return OrderType.AIControlOn;
			}
			return OrderType.AIControlOff;
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x000354DC File Offset: 0x000336DC
		public void SimulateNewOrderWithPositionAndDirection(WorldPosition formationLineBegin, WorldPosition formationLineEnd, out List<WorldPosition> simulationAgentFrames, bool isFormationLayoutVertical)
		{
			IEnumerable<Formation> enumerable = this.SelectedFormations.Where<Formation>((Formation f) => f.CountOfUnitsWithoutDetachedOnes > 0);
			if (enumerable.Any<Formation>())
			{
				OrderController.SimulateNewOrderWithPositionAndDirection(enumerable, this.simulationFormations, formationLineBegin, formationLineEnd, out simulationAgentFrames, isFormationLayoutVertical);
				return;
			}
			simulationAgentFrames = new List<WorldPosition>();
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x00035538 File Offset: 0x00033738
		public void SimulateNewFacingOrder(Vec2 direction, out List<WorldPosition> simulationAgentFrames)
		{
			IEnumerable<Formation> enumerable = this.SelectedFormations.Where<Formation>((Formation f) => f.CountOfUnitsWithoutDetachedOnes > 0);
			if (enumerable.Any<Formation>())
			{
				OrderController.SimulateNewFacingOrder(enumerable, this.simulationFormations, direction, out simulationAgentFrames);
				return;
			}
			simulationAgentFrames = new List<WorldPosition>();
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00035590 File Offset: 0x00033790
		public void SimulateNewCustomWidthOrder(float width, out List<WorldPosition> simulationAgentFrames)
		{
			IEnumerable<Formation> enumerable = this.SelectedFormations.Where<Formation>((Formation f) => f.CountOfUnitsWithoutDetachedOnes > 0);
			if (enumerable.Any<Formation>())
			{
				OrderController.SimulateNewCustomWidthOrder(enumerable, this.simulationFormations, width, out simulationAgentFrames);
				return;
			}
			simulationAgentFrames = new List<WorldPosition>();
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x000355E8 File Offset: 0x000337E8
		private static void SimulateNewOrderWithPositionAndDirectionAux(IEnumerable<Formation> formations, Dictionary<Formation, Formation> simulationFormations, WorldPosition formationLineBegin, WorldPosition formationLineEnd, bool isSimulatingAgentFrames, out List<WorldPosition> simulationAgentFrames, bool isSimulatingFormationChanges, out List<ValueTuple<Formation, int, float, WorldPosition, Vec2>> simulationFormationChanges, out bool isLineShort, bool isFormationLayoutVertical = true)
		{
			float length = (formationLineEnd.AsVec2 - formationLineBegin.AsVec2).Length;
			isLineShort = false;
			if (length < ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalRadius))
			{
				isLineShort = true;
			}
			else
			{
				float num;
				if (isFormationLayoutVertical)
				{
					num = formations.Sum<Formation>((Formation f) => f.MinimumWidth) + (float)(formations.Count<Formation>() - 1) * 1.5f;
				}
				else
				{
					num = formations.Max<Formation>((Formation f) => f.Width);
				}
				if (length < num)
				{
					isLineShort = true;
				}
			}
			if (isLineShort)
			{
				float num2;
				if (isFormationLayoutVertical)
				{
					num2 = formations.Sum<Formation>((Formation f) => f.Width);
					num2 += (float)(formations.Count<Formation>() - 1) * 1.5f;
				}
				else
				{
					num2 = formations.Max<Formation>((Formation f) => f.Width);
				}
				Vec2 direction = formations.MaxBy<Formation, int>((Formation f) => f.CountOfUnitsWithoutDetachedOnes).Direction;
				direction.RotateCCW(-1.5707964f);
				direction.Normalize();
				formationLineEnd = Mission.Current.GetStraightPathToTarget(formationLineBegin.AsVec2 + num2 / 2f * direction, formationLineBegin, 1f, true);
				formationLineBegin = Mission.Current.GetStraightPathToTarget(formationLineBegin.AsVec2 - num2 / 2f * direction, formationLineBegin, 1f, true);
			}
			else
			{
				formationLineEnd = Mission.Current.GetStraightPathToTarget(formationLineEnd.AsVec2, formationLineBegin, 1f, true);
			}
			if (isFormationLayoutVertical)
			{
				OrderController.SimulateNewOrderWithVerticalLayout(formations, simulationFormations, formationLineBegin, formationLineEnd, isSimulatingAgentFrames, out simulationAgentFrames, isSimulatingFormationChanges, out simulationFormationChanges);
				return;
			}
			OrderController.SimulateNewOrderWithHorizontalLayout(formations, simulationFormations, formationLineBegin, formationLineEnd, isSimulatingAgentFrames, out simulationAgentFrames, isSimulatingFormationChanges, out simulationFormationChanges);
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x000357DC File Offset: 0x000339DC
		private static Formation GetSimulationFormation(Formation formation, Dictionary<Formation, Formation> simulationFormations)
		{
			if (simulationFormations == null)
			{
				return null;
			}
			return simulationFormations[formation];
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x000357EC File Offset: 0x000339EC
		private static void SimulateNewFacingOrder(IEnumerable<Formation> formations, Dictionary<Formation, Formation> simulationFormations, Vec2 direction, out List<WorldPosition> simulationAgentFrames)
		{
			simulationAgentFrames = new List<WorldPosition>();
			foreach (Formation formation in formations)
			{
				float width = formation.Width;
				WorldPosition worldPosition = formation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None);
				int num = 0;
				OrderController.DecreaseUnitSpacingAndWidthIfNotAllUnitsFit(formation, OrderController.GetSimulationFormation(formation, simulationFormations), in worldPosition, in direction, ref width, ref num);
				float num2;
				OrderController.SimulateNewOrderWithFrameAndWidth(formation, OrderController.GetSimulationFormation(formation, simulationFormations), simulationAgentFrames, null, in worldPosition, in direction, width, num, false, out num2);
			}
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x00035874 File Offset: 0x00033A74
		private static void SimulateNewCustomWidthOrder(IEnumerable<Formation> formations, Dictionary<Formation, Formation> simulationFormations, float width, out List<WorldPosition> simulationAgentFrames)
		{
			simulationAgentFrames = new List<WorldPosition>();
			foreach (Formation formation in formations)
			{
				float num = width;
				num = MathF.Min(num, formation.MaximumWidth);
				Mat3 identity = Mat3.Identity;
				Vec2 vec = formation.Direction;
				identity.f = vec.ToVec3(0f);
				identity.Orthonormalize();
				WorldPosition worldPosition = formation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None);
				int num2 = 0;
				Formation formation2 = formation;
				Formation simulationFormation = OrderController.GetSimulationFormation(formation, simulationFormations);
				vec = formation.Direction;
				OrderController.DecreaseUnitSpacingAndWidthIfNotAllUnitsFit(formation2, simulationFormation, in worldPosition, in vec, ref num, ref num2);
				int count = simulationAgentFrames.Count;
				Formation formation3 = formation;
				Formation simulationFormation2 = OrderController.GetSimulationFormation(formation, simulationFormations);
				List<WorldPosition> list = simulationAgentFrames;
				List<ValueTuple<Formation, int, float, WorldPosition, Vec2>> list2 = null;
				vec = formation.Direction;
				float num3;
				OrderController.SimulateNewOrderWithFrameAndWidth(formation3, simulationFormation2, list, list2, in worldPosition, in vec, num, num2, false, out num3);
				float lastSimulatedFormationsOccupationWidthIfLesserThanActualWidth = Formation.GetLastSimulatedFormationsOccupationWidthIfLesserThanActualWidth(OrderController.GetSimulationFormation(formation, simulationFormations));
				if (lastSimulatedFormationsOccupationWidthIfLesserThanActualWidth > 0f)
				{
					simulationAgentFrames.RemoveRange(count, simulationAgentFrames.Count - count);
					Formation formation4 = formation;
					Formation simulationFormation3 = OrderController.GetSimulationFormation(formation, simulationFormations);
					List<WorldPosition> list3 = simulationAgentFrames;
					List<ValueTuple<Formation, int, float, WorldPosition, Vec2>> list4 = null;
					vec = formation.Direction;
					OrderController.SimulateNewOrderWithFrameAndWidth(formation4, simulationFormation3, list3, list4, in worldPosition, in vec, lastSimulatedFormationsOccupationWidthIfLesserThanActualWidth, num2, false, out num3);
				}
			}
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0003599C File Offset: 0x00033B9C
		public static void SimulateNewOrderWithPositionAndDirection(IEnumerable<Formation> formations, Dictionary<Formation, Formation> simulationFormations, WorldPosition formationLineBegin, WorldPosition formationLineEnd, out List<WorldPosition> simulationAgentFrames, bool isFormationLayoutVertical = true)
		{
			List<ValueTuple<Formation, int, float, WorldPosition, Vec2>> list;
			bool flag;
			OrderController.SimulateNewOrderWithPositionAndDirectionAux(formations, simulationFormations, formationLineBegin, formationLineEnd, true, out simulationAgentFrames, false, out list, out flag, isFormationLayoutVertical);
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x000359BC File Offset: 0x00033BBC
		public static void SimulateNewOrderWithPositionAndDirection(IEnumerable<Formation> formations, Dictionary<Formation, Formation> simulationFormations, WorldPosition formationLineBegin, WorldPosition formationLineEnd, out List<ValueTuple<Formation, int, float, WorldPosition, Vec2>> formationChanges, out bool isLineShort, bool isFormationLayoutVertical = true)
		{
			List<WorldPosition> list;
			OrderController.SimulateNewOrderWithPositionAndDirectionAux(formations, simulationFormations, formationLineBegin, formationLineEnd, false, out list, true, out formationChanges, out isLineShort, isFormationLayoutVertical);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x000359DC File Offset: 0x00033BDC
		private static void SimulateNewOrderWithVerticalLayout(IEnumerable<Formation> formations, Dictionary<Formation, Formation> simulationFormations, WorldPosition formationLineBegin, WorldPosition formationLineEnd, bool isSimulatingAgentFrames, out List<WorldPosition> simulationAgentFrames, bool isSimulatingFormationChanges, out List<ValueTuple<Formation, int, float, WorldPosition, Vec2>> simulationFormationChanges)
		{
			simulationAgentFrames = ((!isSimulatingAgentFrames) ? null : new List<WorldPosition>());
			simulationFormationChanges = ((!isSimulatingFormationChanges) ? null : new List<ValueTuple<Formation, int, float, WorldPosition, Vec2>>());
			Vec2 vec = formationLineEnd.AsVec2 - formationLineBegin.AsVec2;
			float length = vec.Length;
			vec.Normalize();
			float num = MathF.Max(0f, length - (float)(formations.Count<Formation>() - 1) * 1.5f);
			float num2 = formations.Sum<Formation>((Formation f) => f.Width);
			bool flag = num.ApproximatelyEqualsTo(num2, 0.1f);
			float num3 = formations.Sum<Formation>((Formation f) => f.MinimumWidth);
			formations.Count<Formation>();
			Vec2 vec2 = new Vec2(-vec.y, vec.x).Normalized();
			float num4 = 0f;
			foreach (Formation formation in formations)
			{
				float minimumWidth = formation.MinimumWidth;
				float num5 = (flag ? formation.Width : MathF.Min((num < num2) ? formation.Width : float.MaxValue, num * (minimumWidth / num3)));
				num5 = MathF.Min(num5, formation.MaximumWidth);
				WorldPosition worldPosition = formationLineBegin;
				worldPosition.SetVec2(worldPosition.AsVec2 + vec * (num5 * 0.5f + num4));
				int num6 = 0;
				OrderController.DecreaseUnitSpacingAndWidthIfNotAllUnitsFit(formation, OrderController.GetSimulationFormation(formation, simulationFormations), in worldPosition, in vec2, ref num5, ref num6);
				float num7;
				OrderController.SimulateNewOrderWithFrameAndWidth(formation, OrderController.GetSimulationFormation(formation, simulationFormations), simulationAgentFrames, simulationFormationChanges, in worldPosition, in vec2, num5, num6, false, out num7);
				num4 += num5 + 1.5f;
			}
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00035BC4 File Offset: 0x00033DC4
		private static void DecreaseUnitSpacingAndWidthIfNotAllUnitsFit(Formation formation, Formation simulationFormation, in WorldPosition formationPosition, in Vec2 formationDirection, ref float formationWidth, ref int unitSpacingReduction)
		{
			if (simulationFormation.UnitSpacing != formation.UnitSpacing)
			{
				simulationFormation = new Formation(null, -1);
			}
			int num = formation.CountOfUnitsWithoutDetachedOnes - 1;
			float num2 = formationWidth;
			do
			{
				WorldPosition? worldPosition;
				Vec2? vec;
				formation.GetUnitPositionWithIndexAccordingToNewOrder(simulationFormation, num, in formationPosition, in formationDirection, formationWidth, formation.UnitSpacing - unitSpacingReduction, out worldPosition, out vec, out num2);
				if (worldPosition != null)
				{
					break;
				}
				unitSpacingReduction++;
			}
			while (formation.UnitSpacing - unitSpacingReduction >= 0);
			unitSpacingReduction = MathF.Min(unitSpacingReduction, formation.UnitSpacing);
			if (unitSpacingReduction > 0)
			{
				formationWidth = num2;
			}
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x00035C4C File Offset: 0x00033E4C
		private static float GetGapBetweenLinesOfFormation(Formation f, float unitSpacing)
		{
			float num = 0f;
			float num2 = 0.2f;
			if (f.HasAnyMountedUnit && !(f.RidingOrder == RidingOrder.RidingOrderDismount))
			{
				num = 2f;
				num2 = 0.6f;
			}
			return num + unitSpacing * num2;
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x00035C90 File Offset: 0x00033E90
		private static void SimulateNewOrderWithHorizontalLayout(IEnumerable<Formation> formations, Dictionary<Formation, Formation> simulationFormations, WorldPosition formationLineBegin, WorldPosition formationLineEnd, bool isSimulatingAgentFrames, out List<WorldPosition> simulationAgentFrames, bool isSimulatingFormationChanges, out List<ValueTuple<Formation, int, float, WorldPosition, Vec2>> simulationFormationChanges)
		{
			simulationAgentFrames = ((!isSimulatingAgentFrames) ? null : new List<WorldPosition>());
			simulationFormationChanges = ((!isSimulatingFormationChanges) ? null : new List<ValueTuple<Formation, int, float, WorldPosition, Vec2>>());
			Vec2 vec = formationLineEnd.AsVec2 - formationLineBegin.AsVec2;
			float num = vec.Normalize();
			float num2 = formations.Max<Formation>((Formation f) => f.MinimumWidth);
			if (num < num2)
			{
				num = num2;
			}
			Vec2 vec2 = new Vec2(-vec.y, vec.x).Normalized();
			float num3 = 0f;
			foreach (Formation formation in formations)
			{
				float num4 = num;
				num4 = MathF.Min(num4, formation.MaximumWidth);
				WorldPosition worldPosition = formationLineBegin;
				worldPosition.SetVec2((formationLineEnd.AsVec2 + formationLineBegin.AsVec2) * 0.5f - vec2 * num3);
				int num5 = 0;
				OrderController.DecreaseUnitSpacingAndWidthIfNotAllUnitsFit(formation, OrderController.GetSimulationFormation(formation, simulationFormations), in worldPosition, in vec2, ref num4, ref num5);
				float num6;
				OrderController.SimulateNewOrderWithFrameAndWidth(formation, OrderController.GetSimulationFormation(formation, simulationFormations), simulationAgentFrames, simulationFormationChanges, in worldPosition, in vec2, num4, num5, true, out num6);
				num3 += num6 + OrderController.GetGapBetweenLinesOfFormation(formation, (float)(formation.UnitSpacing - num5));
			}
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x00035E04 File Offset: 0x00034004
		private static void SimulateNewOrderWithFrameAndWidth(Formation formation, Formation simulationFormation, List<WorldPosition> simulationAgentFrames, List<ValueTuple<Formation, int, float, WorldPosition, Vec2>> simulationFormationChanges, in WorldPosition formationPosition, in Vec2 formationDirection, float formationWidth, int unitSpacingReduction, bool simulateFormationDepth, out float simulatedFormationDepth)
		{
			int num = 0;
			float num2 = (simulateFormationDepth ? 0f : float.NaN);
			bool flag = Mission.Current.Mode != MissionMode.Deployment || Mission.Current.IsOrderPositionAvailable(in formationPosition, formation.Team);
			foreach (Agent agent in from u in formation.GetUnitsWithoutDetachedOnes()
				orderby MBCommon.Hash(u.Index, u)
				select u)
			{
				WorldPosition? worldPosition = null;
				Vec2? vec = null;
				if (flag)
				{
					formation.GetUnitPositionWithIndexAccordingToNewOrder(simulationFormation, num, in formationPosition, in formationDirection, formationWidth, formation.UnitSpacing - unitSpacingReduction, out worldPosition, out vec);
				}
				else
				{
					worldPosition = new WorldPosition?(agent.GetWorldPosition());
					vec = new Vec2?(agent.GetMovementDirection());
				}
				if (worldPosition != null)
				{
					if (simulationAgentFrames != null)
					{
						simulationAgentFrames.Add(worldPosition.Value);
					}
					if (simulateFormationDepth)
					{
						WorldPosition worldPosition2 = formationPosition;
						Vec2 asVec = worldPosition2.AsVec2;
						worldPosition2 = formationPosition;
						Vec2 asVec2 = worldPosition2.AsVec2;
						Vec2 vec2 = formationDirection;
						float num3 = Vec2.DistanceToLine(asVec, asVec2 + vec2.RightVec(), worldPosition.Value.AsVec2);
						if (num3 > num2)
						{
							num2 = num3;
						}
					}
				}
				num++;
			}
			if (flag)
			{
				if (simulationFormationChanges != null)
				{
					simulationFormationChanges.Add(ValueTuple.Create<Formation, int, float, WorldPosition, Vec2>(formation, unitSpacingReduction, formationWidth, formationPosition, formationDirection));
				}
			}
			else
			{
				WorldPosition worldPosition3 = formation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None);
				if (simulationFormationChanges != null)
				{
					simulationFormationChanges.Add(ValueTuple.Create<Formation, int, float, WorldPosition, Vec2>(formation, unitSpacingReduction, formationWidth, worldPosition3, formation.Direction));
				}
			}
			simulatedFormationDepth = num2 + formation.UnitDiameter;
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x00035FC4 File Offset: 0x000341C4
		public void SimulateDestinationFrames(out List<WorldPosition> simulationAgentFrames, float minDistance = 3f)
		{
			List<Formation> selectedFormations = this.SelectedFormations;
			simulationAgentFrames = new List<WorldPosition>(100);
			float minDistanceSq = minDistance * minDistance;
			Action<Agent, List<WorldPosition>> <>9__0;
			foreach (Formation formation in selectedFormations)
			{
				Action<Agent, List<WorldPosition>> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(Agent agent, List<WorldPosition> localSimulationAgentFrames)
					{
						WorldPosition worldPosition;
						if (this._mission.IsTeleportingAgents && !agent.CanTeleport())
						{
							worldPosition = agent.GetWorldPosition();
						}
						else
						{
							worldPosition = agent.Formation.GetOrderPositionOfUnit(agent);
						}
						bool flag = worldPosition.IsValid;
						if (!GameNetwork.IsMultiplayer && this._mission.Mode == MissionMode.Deployment && !this._mission.IsNavalBattle)
						{
							IMissionDeploymentPlan deploymentPlan = agent.Mission.DeploymentPlan;
							if (deploymentPlan.SupportsNavmesh(agent.Team))
							{
								deploymentPlan.ProjectPositionToDeploymentBoundaries(agent.Formation.Team, ref worldPosition);
							}
							flag = this._mission.IsFormationUnitPositionAvailable(ref worldPosition, agent.Formation.Team);
						}
						if (flag && agent.Position.AsVec2.DistanceSquared(worldPosition.AsVec2) >= minDistanceSq)
						{
							localSimulationAgentFrames.Add(worldPosition);
						}
					});
				}
				formation.ApplyActionOnEachUnit(action, simulationAgentFrames);
			}
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x00036054 File Offset: 0x00034254
		private void ToggleSideOrderUse(IEnumerable<Formation> formations, UsableMachine usable)
		{
			IEnumerable<Formation> enumerable = formations.Where<Formation>(new Func<Formation, bool>(usable.IsUsedByFormation));
			if (enumerable.IsEmpty<Formation>())
			{
				foreach (Formation formation in formations)
				{
					formation.StartUsingMachine(usable, true);
				}
				if (!usable.HasWaitFrame)
				{
					return;
				}
				using (IEnumerator<Formation> enumerator = formations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Formation formation2 = enumerator.Current;
						formation2.SetMovementOrder(MovementOrder.MovementOrderFollowEntity(usable.WaitEntity));
					}
					return;
				}
			}
			foreach (Formation formation3 in enumerable)
			{
				formation3.StopUsingMachine(usable, true);
			}
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x00036134 File Offset: 0x00034334
		private static int GetLineOrderByClass(FormationClass formationClass)
		{
			return Array.IndexOf<FormationClass>(new FormationClass[]
			{
				FormationClass.HeavyInfantry,
				FormationClass.Infantry,
				FormationClass.HeavyCavalry,
				FormationClass.Cavalry,
				FormationClass.LightCavalry,
				FormationClass.NumberOfDefaultFormations,
				FormationClass.Ranged,
				FormationClass.HorseArcher
			}, formationClass);
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0003614D File Offset: 0x0003434D
		public static IEnumerable<Formation> SortFormationsForHorizontalLayout(IEnumerable<Formation> formations)
		{
			return formations.OrderBy<Formation, int>((Formation f) => OrderController.GetLineOrderByClass(f.FormationIndex));
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x00036174 File Offset: 0x00034374
		private static IEnumerable<Formation> GetSortedFormations(IEnumerable<Formation> formations, bool isFormationLayoutVertical)
		{
			if (isFormationLayoutVertical)
			{
				return formations;
			}
			return OrderController.SortFormationsForHorizontalLayout(formations);
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x00036184 File Offset: 0x00034384
		private void MoveToLineSegment(IEnumerable<Formation> formations, WorldPosition TargetLineSegmentBegin, WorldPosition TargetLineSegmentEnd, bool isFormationLayoutVertical = true)
		{
			foreach (Formation formation in formations)
			{
				int num;
				if (this.actualUnitSpacings.TryGetValue(formation, out num))
				{
					formation.SetPositioning(null, null, new int?(num));
				}
				float num2;
				if (this.actualWidths.TryGetValue(formation, out num2))
				{
					formation.SetFormOrder(FormOrder.FormOrderCustom(num2), true);
				}
			}
			formations = OrderController.GetSortedFormations(formations, isFormationLayoutVertical);
			List<ValueTuple<Formation, int, float, WorldPosition, Vec2>> list;
			bool flag;
			OrderController.SimulateNewOrderWithPositionAndDirection(formations, this.simulationFormations, TargetLineSegmentBegin, TargetLineSegmentEnd, out list, out flag, isFormationLayoutVertical);
			if (!formations.Any<Formation>())
			{
				return;
			}
			foreach (ValueTuple<Formation, int, float, WorldPosition, Vec2> valueTuple in list)
			{
				Formation item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				float item3 = valueTuple.Item3;
				WorldPosition item4 = valueTuple.Item4;
				Vec2 item5 = valueTuple.Item5;
				int unitSpacing = item.UnitSpacing;
				float width = item.Width;
				if (item2 > 0)
				{
					int num3 = MathF.Max(item.UnitSpacing - item2, 0);
					item.SetPositioning(null, null, new int?(num3));
					if (item.UnitSpacing != unitSpacing)
					{
						this.actualUnitSpacings[item] = unitSpacing;
					}
				}
				if (item.Width != item3 && item.ArrangementOrder.OrderEnum != ArrangementOrder.ArrangementOrderEnum.Column)
				{
					item.SetFormOrder(FormOrder.FormOrderCustom(item3), true);
					if (flag)
					{
						this.actualWidths[item] = width;
					}
				}
				if (!flag)
				{
					item.SetMovementOrder(MovementOrder.MovementOrderMove(item4));
					item.SetFacingOrder(FacingOrder.FacingOrderLookAtDirection(item5));
					item.SetFormOrder(FormOrder.FormOrderCustom(item3), true);
					MBList<Formation> mblist = new MBList<Formation> { item };
					this.FireOnOrderIssued(OrderType.Move, mblist, this, new object[] { item4 });
					this.FireOnOrderIssued(OrderType.LookAtDirection, mblist, this, new object[] { item5 });
					this.FireOnOrderIssued(OrderType.FormCustom, mblist, this, new object[] { item3 });
				}
				else
				{
					Formation formation2 = formations.MaxBy<Formation, int>((Formation f) => f.CountOfUnitsWithoutDetachedOnes);
					OrderType activeFacingOrderOf = OrderController.GetActiveFacingOrderOf(formation2);
					if (activeFacingOrderOf == OrderType.LookAtEnemy)
					{
						item.SetMovementOrder(MovementOrder.MovementOrderMove(item4));
						MBList<Formation> mblist2 = new MBList<Formation> { item };
						this.FireOnOrderIssued(OrderType.Move, mblist2, this, new object[] { item4 });
						this.FireOnOrderIssued(OrderType.LookAtEnemy, mblist2, this, Array.Empty<object>());
					}
					else if (activeFacingOrderOf == OrderType.LookAtDirection)
					{
						item.SetMovementOrder(MovementOrder.MovementOrderMove(item4));
						item.SetFacingOrder(FacingOrder.FacingOrderLookAtDirection(formation2.Direction));
						MBList<Formation> mblist3 = new MBList<Formation> { item };
						this.FireOnOrderIssued(OrderType.Move, mblist3, this, new object[] { item4 });
						this.FireOnOrderIssued(OrderType.LookAtDirection, mblist3, this, new object[] { formation2.Direction });
					}
					else
					{
						Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "MoveToLineSegment", 2386);
					}
				}
			}
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x000364F8 File Offset: 0x000346F8
		public static Vec2 GetOrderLookAtDirection(IEnumerable<Formation> formations, Vec2 target)
		{
			if (!formations.Any<Formation>())
			{
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\OrderController.cs", "GetOrderLookAtDirection", 2406);
				return Vec2.One;
			}
			Formation formation = formations.MaxBy<Formation, int>((Formation f) => f.CountOfUnitsWithoutDetachedOnes);
			return (target - formation.OrderPosition).Normalized();
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x00036568 File Offset: 0x00034768
		public static float GetOrderFormCustomWidth(IEnumerable<Formation> formations, Vec3 orderPosition)
		{
			return (Agent.Main.Position - orderPosition).Length;
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x00036590 File Offset: 0x00034790
		public void TransferUnits(Formation source, Formation target, int count)
		{
			source.TransferUnitsAux(target, count, false, count < source.CountOfUnits && target.CountOfUnits > 0);
			this.FireOnOrderIssued(OrderType.Transfer, new MBList<Formation> { source }, this, new object[] { target, count });
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x000365E4 File Offset: 0x000347E4
		public IEnumerable<Formation> SplitFormation(Formation formation, int count = 2)
		{
			if (!formation.IsSplittableByAI || formation.CountOfUnitsWithoutDetachedOnes < count)
			{
				return new List<Formation> { formation };
			}
			MBDebug.Print(string.Concat(new object[]
			{
				(formation.Team.Side == BattleSideEnum.Attacker) ? "Attacker team" : "Defender team",
				" formation ",
				(int)formation.FormationIndex,
				" split"
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			List<Formation> list = new List<Formation> { formation };
			while (count > 1)
			{
				int num = formation.CountOfUnits / count;
				for (int i = 0; i < 8; i++)
				{
					Formation formation2 = formation.Team.GetFormation((FormationClass)i);
					if (formation2.CountOfUnits == 0)
					{
						formation.TransferUnitsAux(formation2, num, false, false);
						list.Add(formation2);
						this.FireOnOrderIssued(OrderType.Transfer, new MBList<Formation> { formation }, this, new object[] { formation2, num });
						break;
					}
				}
				count--;
			}
			return list;
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x000366E4 File Offset: 0x000348E4
		protected void FireOnOrderIssued(OrderType orderType, MBReadOnlyList<Formation> appliedFormations, OrderController orderController, params object[] delegateParams)
		{
			if (this.OnOrderIssued != null)
			{
				this.OnOrderIssued(orderType, appliedFormations, orderController, delegateParams);
			}
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x000366FE File Offset: 0x000348FE
		[Conditional("DEBUG")]
		public void TickDebug()
		{
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00036700 File Offset: 0x00034900
		public void AddOrderOverride(Func<Formation, MovementOrder, MovementOrder> orderOverride)
		{
			if (this.orderOverrides == null)
			{
				this.orderOverrides = new List<Func<Formation, MovementOrder, MovementOrder>>();
				this.overridenOrders = new List<ValueTuple<Formation, OrderType>>();
			}
			this.orderOverrides.Add(orderOverride);
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0003672C File Offset: 0x0003492C
		public OrderType GetOverridenOrderType(Formation formation)
		{
			if (this.overridenOrders == null)
			{
				return OrderType.None;
			}
			ValueTuple<Formation, OrderType> valueTuple = this.overridenOrders.FirstOrDefault<ValueTuple<Formation, OrderType>>((ValueTuple<Formation, OrderType> oo) => oo.Item1 == formation);
			if (valueTuple.Item1 != null)
			{
				return valueTuple.Item2;
			}
			return OrderType.None;
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00036778 File Offset: 0x00034978
		public void SetFormationUpdateEnabledAfterSetOrder(bool value)
		{
			this._formationUpdateEnabledAfterSetOrder = value;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00036781 File Offset: 0x00034981
		private void CreateDefaultOrderOverrides()
		{
			this.AddOrderOverride(delegate(Formation formation, MovementOrder order)
			{
				if (formation.ArrangementOrder.OrderType == OrderType.ArrangementCloseOrder && order.OrderType == OrderType.StandYourGround)
				{
					Vec2 cachedAveragePosition = formation.CachedAveragePosition;
					WorldPosition cachedMedianPosition = formation.CachedMedianPosition;
					cachedMedianPosition.SetVec2(cachedAveragePosition + formation.Direction * formation.Depth * (0.5f + formation.CachedMovementSpeed));
					return MovementOrder.MovementOrderMove(cachedMedianPosition);
				}
				return MovementOrder.MovementOrderStop;
			});
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x000367A8 File Offset: 0x000349A8
		public static void TryCancelStopOrder(Formation formation)
		{
			if (!GameNetwork.IsClientOrReplay && formation.GetReadonlyMovementOrderReference().OrderEnum == MovementOrder.MovementOrderEnum.Stop)
			{
				WorldPosition worldPosition = formation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None);
				if (worldPosition.IsValid)
				{
					formation.SetMovementOrder(MovementOrder.MovementOrderMove(worldPosition));
				}
			}
		}

		// Token: 0x0400043D RID: 1085
		public const float FormationGapInLine = 1.5f;

		// Token: 0x0400043E RID: 1086
		protected readonly Mission _mission;

		// Token: 0x0400043F RID: 1087
		public readonly Team Team;

		// Token: 0x04000440 RID: 1088
		public Agent Owner;

		// Token: 0x04000442 RID: 1090
		protected readonly MBList<Formation> _selectedFormations;

		// Token: 0x04000446 RID: 1094
		private Dictionary<Formation, float> actualWidths;

		// Token: 0x04000447 RID: 1095
		private Dictionary<Formation, int> actualUnitSpacings;

		// Token: 0x04000448 RID: 1096
		private List<Func<Formation, MovementOrder, MovementOrder>> orderOverrides;

		// Token: 0x04000449 RID: 1097
		private List<ValueTuple<Formation, OrderType>> overridenOrders;

		// Token: 0x0400044A RID: 1098
		protected bool _formationUpdateEnabledAfterSetOrder = true;

		// Token: 0x0400044B RID: 1099
		private bool _gesturesEnabled;

		// Token: 0x0400044C RID: 1100
		private MissionTimer _yellingAfterChargeOrderTimer;
	}
}
