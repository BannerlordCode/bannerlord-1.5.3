using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000074 RID: 116
	[DefaultView]
	public class MissionGamepadEffectsView : MissionView
	{
		// Token: 0x0600046A RID: 1130 RVA: 0x00020FE4 File Offset: 0x0001F1E4
		public override void OnMissionStateActivated()
		{
			base.OnMissionStateActivated();
			this.ResetTriggerFeedback();
			this.ResetTriggerVibration();
			this._isAdaptiveTriggerEnabled = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.EnableVibration) != 0f;
			this._usingAlternativeAiming = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.EnableAlternateAiming) != 0f;
			NativeOptions.OnNativeOptionChanged = (NativeOptions.OnNativeOptionChangedDelegate)Delegate.Combine(NativeOptions.OnNativeOptionChanged, new NativeOptions.OnNativeOptionChangedDelegate(this.OnNativeOptionChanged));
			TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00021071 File Offset: 0x0001F271
		private void OnGamepadActiveStateChanged()
		{
			if (!TaleWorlds.InputSystem.Input.IsGamepadActive)
			{
				this.ResetTriggerFeedback();
				this.ResetTriggerVibration();
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00021088 File Offset: 0x0001F288
		public override void OnMissionStateDeactivated()
		{
			base.OnMissionStateDeactivated();
			this.ResetTriggerFeedback();
			this.ResetTriggerVibration();
			NativeOptions.OnNativeOptionChanged = (NativeOptions.OnNativeOptionChangedDelegate)Delegate.Remove(NativeOptions.OnNativeOptionChanged, new NativeOptions.OnNativeOptionChangedDelegate(this.OnNativeOptionChanged));
			TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000210E8 File Offset: 0x0001F2E8
		public override void OnPreMissionTick(float dt)
		{
			base.OnPreMissionTick(dt);
			Agent mainAgent = base.Mission.MainAgent;
			if (this._isAdaptiveTriggerEnabled)
			{
				if (mainAgent != null && mainAgent.State == AgentState.Active && mainAgent.CombatActionsEnabled && !mainAgent.IsCheering && !base.Mission.IsOrderMenuOpen && this.IsMissionModeApplicableForAdaptiveTrigger(base.Mission.Mode))
				{
					MissionWeapon wieldedWeapon = mainAgent.WieldedWeapon;
					WeaponComponentData currentUsageItem = wieldedWeapon.CurrentUsageItem;
					bool flag = currentUsageItem != null && currentUsageItem.WeaponFlags.HasAllFlags(WeaponFlags.StringHeldByHand);
					WeaponComponentData currentUsageItem2 = wieldedWeapon.CurrentUsageItem;
					bool flag2 = currentUsageItem2 != null && currentUsageItem2.WeaponFlags.HasAllFlags(WeaponFlags.HasString) && !wieldedWeapon.CurrentUsageItem.WeaponFlags.HasAllFlags(WeaponFlags.StringHeldByHand);
					WeaponComponentData currentUsageItem3 = wieldedWeapon.CurrentUsageItem;
					bool flag3 = currentUsageItem3 != null && currentUsageItem3.IsRangedWeapon && wieldedWeapon.CurrentUsageItem.IsConsumable;
					WeaponComponentData currentUsageItem4 = wieldedWeapon.CurrentUsageItem;
					bool flag4 = (currentUsageItem4 != null && currentUsageItem4.WeaponFlags.HasAllFlags(WeaponFlags.MeleeWeapon)) || mainAgent.WieldedOffhandWeapon.IsShield();
					if (flag)
					{
						this.HandleBowAdaptiveTriggers();
						return;
					}
					if (flag2)
					{
						this.HandleCrossbowAdaptiveTriggers();
						return;
					}
					if (flag3)
					{
						this.HandleThrowableAdaptiveTriggers();
						return;
					}
					if (flag4)
					{
						this.HandleMeleeAdaptiveTriggers();
						return;
					}
					if (mainAgent.CurrentlyUsedGameObject != null)
					{
						if (mainAgent.CurrentlyUsedGameObject != this._currentlyUsedMissionObject)
						{
							this._currentlyUsedMissionObject = mainAgent.CurrentlyUsedGameObject;
							UsableMachine usableMachineFromUsableMissionObject = this.GetUsableMachineFromUsableMissionObject(this._currentlyUsedMissionObject);
							RangedSiegeWeapon rangedSiegeWeapon;
							this._currentlyUsedSiegeWeapon = (((rangedSiegeWeapon = usableMachineFromUsableMissionObject as RangedSiegeWeapon) != null) ? rangedSiegeWeapon : null);
						}
						this.HandleRangedSiegeEngineAdaptiveTriggers(this._currentlyUsedSiegeWeapon);
						return;
					}
					this._currentlyUsedSiegeWeapon = null;
					this._currentlyUsedMissionObject = null;
					this.ResetTriggerFeedback();
					this.ResetTriggerVibration();
					return;
				}
				else
				{
					this._currentlyUsedSiegeWeapon = null;
					this._currentlyUsedMissionObject = null;
					this.ResetTriggerFeedback();
					this.ResetTriggerVibration();
				}
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000212CC File Offset: 0x0001F4CC
		public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
			base.OnAgentHit(affectedAgent, affectorAgent, in affectorWeapon, in blow, in attackCollisionData);
			if (affectedAgent == Agent.Main)
			{
				AttackCollisionData attackCollisionData2 = attackCollisionData;
				if (attackCollisionData2.CollisionResult != CombatCollisionResult.Blocked)
				{
					attackCollisionData2 = attackCollisionData;
					if (attackCollisionData2.CollisionResult != CombatCollisionResult.ChamberBlocked)
					{
						attackCollisionData2 = attackCollisionData;
						if (attackCollisionData2.CollisionResult != CombatCollisionResult.Parried)
						{
							goto IL_0092;
						}
					}
				}
				float[] array = new float[] { 0.5f };
				float[] array2 = new float[] { 0.3f };
				float[] array3 = new float[] { 0.3f };
				this.SetTriggerVibration(array, array2, array3, array3.Length, null, null, null, 0);
				this.SetTriggerState(MissionGamepadEffectsView.TriggerState.Off);
				IL_0092:
				if (affectedAgent.WieldedOffhandWeapon.IsEmpty)
				{
					attackCollisionData2 = attackCollisionData;
					if (attackCollisionData2.AttackBlockedWithShield)
					{
						this.SetTriggerState(MissionGamepadEffectsView.TriggerState.Off);
						return;
					}
				}
			}
			else if (affectorAgent == Agent.Main)
			{
				AttackCollisionData attackCollisionData2 = attackCollisionData;
				if (attackCollisionData2.CollisionResult != CombatCollisionResult.StrikeAgent)
				{
					attackCollisionData2 = attackCollisionData;
					if (attackCollisionData2.CollisionResult != CombatCollisionResult.Blocked)
					{
						return;
					}
				}
				MissionWeapon missionWeapon = affectorWeapon;
				if (!missionWeapon.IsEmpty)
				{
					missionWeapon = affectorWeapon;
					if (missionWeapon.IsShield())
					{
						float[] array4 = new float[] { 1f };
						float[] array5 = new float[] { 0.1f };
						float[] array6 = new float[] { 0.35f };
						this.SetTriggerVibration(array4, array5, array6, array6.Length, null, null, null, 0);
					}
				}
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00021430 File Offset: 0x0001F630
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
			if (affectedAgent.IsMainAgent)
			{
				this.ResetTriggerFeedback();
				this.ResetTriggerVibration();
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00021451 File Offset: 0x0001F651
		protected override void OnEndMission()
		{
			base.OnEndMission();
			this.SetTriggerState(MissionGamepadEffectsView.TriggerState.Off);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00021460 File Offset: 0x0001F660
		private void OnNativeOptionChanged(NativeOptions.NativeOptionsType changedNativeOptionsType)
		{
			if (changedNativeOptionsType == NativeOptions.NativeOptionsType.EnableVibration)
			{
				bool isAdaptiveTriggerEnabled = this._isAdaptiveTriggerEnabled;
				this._isAdaptiveTriggerEnabled = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.EnableVibration) != 0f;
				this._usingAlternativeAiming = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.EnableAlternateAiming) != 0f;
				if (isAdaptiveTriggerEnabled && !this._isAdaptiveTriggerEnabled)
				{
					this._currentlyUsedSiegeWeapon = null;
					this._currentlyUsedMissionObject = null;
					this.ResetTriggerFeedback();
					this.ResetTriggerVibration();
				}
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x000214CA File Offset: 0x0001F6CA
		private bool IsMissionModeApplicableForAdaptiveTrigger(MissionMode mode)
		{
			switch (mode)
			{
			case MissionMode.StartUp:
			case MissionMode.Battle:
			case MissionMode.Duel:
			case MissionMode.Stealth:
			case MissionMode.Tournament:
				return true;
			}
			return false;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00021504 File Offset: 0x0001F704
		private void HandleBowAdaptiveTriggers()
		{
			Agent mainAgent = base.Mission.MainAgent;
			Agent.ActionStage actionStage = ((mainAgent != null) ? mainAgent.GetCurrentActionStage(1) : Agent.ActionStage.None);
			if (actionStage == Agent.ActionStage.None || actionStage == Agent.ActionStage.ReloadMidPhase || actionStage == Agent.ActionStage.ReloadLastPhase)
			{
				this.SetTriggerState(this._usingAlternativeAiming ? MissionGamepadEffectsView.TriggerState.SoftTriggerFeedbackLeft : MissionGamepadEffectsView.TriggerState.SoftTriggerFeedbackRight);
				return;
			}
			if (actionStage == Agent.ActionStage.AttackReady)
			{
				float num = mainAgent.GetAimingTimer() - mainAgent.AgentDrivenProperties.WeaponUnsteadyBeginTime;
				if (num > 0f)
				{
					float num2 = mainAgent.AgentDrivenProperties.WeaponUnsteadyEndTime - mainAgent.AgentDrivenProperties.WeaponUnsteadyBeginTime;
					float num3 = MBMath.ClampFloat(num / num2, 0f, 1f);
					float num4 = MBMath.Lerp(0f, 1f, num3, 1E-05f);
					float[] array = new float[] { num4 };
					float num5 = MBMath.ClampFloat(1f - num4, 0.1f, 1f);
					float[] array2 = new float[] { num5 };
					float[] array3 = new float[] { 0.05f };
					if (this._usingAlternativeAiming)
					{
						this.SetTriggerVibration(array, array2, array3, array3.Length, null, null, null, 0);
					}
					else
					{
						this.SetTriggerVibration(null, null, null, 0, array, array2, array3, array3.Length);
					}
					this._triggerState = MissionGamepadEffectsView.TriggerState.Vibration;
				}
				else
				{
					this.SetTriggerState(this._usingAlternativeAiming ? MissionGamepadEffectsView.TriggerState.SoftTriggerFeedbackLeft : MissionGamepadEffectsView.TriggerState.SoftTriggerFeedbackRight);
					float[] array4 = new float[] { 0.07f };
					float[] array5 = new float[] { 0.5f };
					float[] array6 = new float[] { 0.5f };
					if (this._usingAlternativeAiming)
					{
						this.SetTriggerVibration(array4, array5, array6, array6.Length, null, null, null, 0);
					}
					else
					{
						this.SetTriggerVibration(null, null, null, 0, array4, array5, array6, array6.Length);
					}
				}
				if (this._usingAlternativeAiming)
				{
					this.SetTriggerWeaponEffect(0, 0, 0, 3, 7, 8);
					return;
				}
				this.SetTriggerWeaponEffect(0, 0, 0, 0, 0, 0);
				return;
			}
			else
			{
				if (actionStage == Agent.ActionStage.AttackRelease)
				{
					this.SetTriggerState(MissionGamepadEffectsView.TriggerState.Off);
					return;
				}
				this.SetTriggerState(MissionGamepadEffectsView.TriggerState.Off);
				return;
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000216DC File Offset: 0x0001F8DC
		private void HandleCrossbowAdaptiveTriggers()
		{
			Agent mainAgent = base.Mission.MainAgent;
			Agent.ActionStage actionStage = ((mainAgent != null) ? mainAgent.GetCurrentActionStage(1) : Agent.ActionStage.None);
			if (actionStage == Agent.ActionStage.ReloadMidPhase)
			{
				this.SetTriggerState(MissionGamepadEffectsView.TriggerState.Off);
				return;
			}
			if (actionStage == Agent.ActionStage.AttackRelease)
			{
				float[] array = new float[] { 0.01f };
				float[] array2 = new float[] { 0.08f };
				float[] array3 = new float[] { 0.05f };
				this.SetTriggerVibration(null, null, null, 0, array, array2, array3, array3.Length);
				this.SetTriggerState(MissionGamepadEffectsView.TriggerState.Off);
				return;
			}
			if (actionStage == Agent.ActionStage.AttackReady)
			{
				if (this._usingAlternativeAiming)
				{
					this.SetTriggerWeaponEffect(0, 0, 0, 3, 7, 8);
					return;
				}
			}
			else if (!this._usingAlternativeAiming)
			{
				this.SetTriggerWeaponEffect(0, 0, 0, 3, 7, 8);
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00021784 File Offset: 0x0001F984
		private void HandleThrowableAdaptiveTriggers()
		{
			WeaponComponentData currentUsageItem = base.Mission.MainAgent.WieldedOffhandWeapon.CurrentUsageItem;
			bool flag = currentUsageItem != null && currentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.CanBlockRanged);
			this._triggerFeedback[2] = 0;
			this._triggerFeedback[3] = 3;
			if (flag)
			{
				this._triggerFeedback[0] = 4;
				this._triggerFeedback[1] = 2;
			}
			else
			{
				this._triggerFeedback[0] = 0;
				this._triggerFeedback[1] = 0;
			}
			this.SetTriggerFeedback(this._triggerFeedback[0], this._triggerFeedback[1], this._triggerFeedback[2], this._triggerFeedback[3]);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00021820 File Offset: 0x0001FA20
		private void HandleMeleeAdaptiveTriggers()
		{
			Agent mainAgent = base.Mission.MainAgent;
			MissionWeapon wieldedWeapon = mainAgent.WieldedWeapon;
			WeaponComponentData currentUsageItem = wieldedWeapon.CurrentUsageItem;
			bool flag = currentUsageItem != null && currentUsageItem.WeaponFlags.HasAllFlags(WeaponFlags.NotUsableWithOneHand);
			WeaponComponentData currentUsageItem2 = mainAgent.WieldedOffhandWeapon.CurrentUsageItem;
			bool flag2 = currentUsageItem2 != null && currentUsageItem2.WeaponFlags.HasAnyFlag(WeaponFlags.CanBlockRanged);
			if (flag)
			{
				this._triggerFeedback[2] = 3;
				this._triggerFeedback[3] = 0;
			}
			else if (wieldedWeapon.CurrentUsageItem == null)
			{
				this._triggerFeedback[2] = 0;
				this._triggerFeedback[3] = 0;
			}
			else
			{
				this._triggerFeedback[2] = 4;
				this._triggerFeedback[3] = 1;
			}
			if (flag2 || flag || wieldedWeapon.CurrentUsageItem != null)
			{
				this._triggerFeedback[0] = 4;
				this._triggerFeedback[1] = 2;
			}
			else
			{
				this._triggerFeedback[0] = 0;
				this._triggerFeedback[1] = 0;
			}
			this.SetTriggerFeedback(this._triggerFeedback[0], this._triggerFeedback[1], this._triggerFeedback[2], this._triggerFeedback[3]);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00021920 File Offset: 0x0001FB20
		private void HandleRangedSiegeEngineAdaptiveTriggers(RangedSiegeWeapon rangedSiegeWeapon)
		{
			if (!(rangedSiegeWeapon is Ballista) && !(rangedSiegeWeapon is FireBallista))
			{
				this.ResetTriggerFeedback();
				this.ResetTriggerVibration();
				return;
			}
			if (rangedSiegeWeapon.State == RangedSiegeWeapon.WeaponState.Idle)
			{
				this.SetTriggerWeaponEffect(0, 0, 0, 4, 6, 10);
				return;
			}
			if (rangedSiegeWeapon.State == RangedSiegeWeapon.WeaponState.Shooting || rangedSiegeWeapon.State == RangedSiegeWeapon.WeaponState.WaitingBeforeProjectileLeaving)
			{
				this.SetTriggerWeaponEffect(0, 0, 0, 0, 0, 0);
				float[] array = new float[] { 0.2f, 0.4f, 0.2f };
				float[] array2 = new float[] { 0.2f, 0.4f, 0.2f };
				float[] array3 = new float[] { 0.2f, 0.3f, 0.2f };
				this.SetTriggerVibration(null, null, null, 0, array, array2, array3, array3.Length);
				return;
			}
			this.ResetTriggerFeedback();
			this.ResetTriggerVibration();
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000219D4 File Offset: 0x0001FBD4
		private UsableMachine GetUsableMachineFromUsableMissionObject(UsableMissionObject usableMissionObject)
		{
			StandingPoint standingPoint;
			if ((standingPoint = usableMissionObject as StandingPoint) != null)
			{
				WeakGameEntity weakGameEntity = standingPoint.GameEntity;
				while (weakGameEntity.IsValid && !weakGameEntity.HasScriptOfType<UsableMachine>())
				{
					weakGameEntity = weakGameEntity.Parent;
				}
				if (weakGameEntity.IsValid)
				{
					UsableMachine firstScriptOfType = weakGameEntity.GetFirstScriptOfType<UsableMachine>();
					if (firstScriptOfType != null)
					{
						return firstScriptOfType;
					}
				}
			}
			return null;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00021A28 File Offset: 0x0001FC28
		private void SetTriggerState(MissionGamepadEffectsView.TriggerState triggerState)
		{
			if (this._triggerState != triggerState)
			{
				switch (triggerState)
				{
				case MissionGamepadEffectsView.TriggerState.Off:
					this.ResetTriggerFeedback();
					this.ResetTriggerVibration();
					break;
				case MissionGamepadEffectsView.TriggerState.SoftTriggerFeedbackLeft:
					this.SetTriggerFeedback(0, 2, 0, 0);
					this.SetTriggerWeaponEffect(0, 0, 0, 0, 0, 0);
					break;
				case MissionGamepadEffectsView.TriggerState.SoftTriggerFeedbackRight:
					this.SetTriggerFeedback(0, 0, 0, 2);
					this.SetTriggerWeaponEffect(0, 0, 0, 0, 0, 0);
					break;
				case MissionGamepadEffectsView.TriggerState.HardTriggerFeedbackLeft:
					this.SetTriggerFeedback(0, 4, 0, 0);
					this.SetTriggerWeaponEffect(0, 0, 0, 0, 0, 0);
					break;
				case MissionGamepadEffectsView.TriggerState.HardTriggerFeedbackRight:
					this.SetTriggerFeedback(0, 0, 0, 4);
					this.SetTriggerWeaponEffect(0, 0, 0, 0, 0, 0);
					break;
				case MissionGamepadEffectsView.TriggerState.WeaponEffect:
					this.SetTriggerWeaponEffect(0, 0, 0, 4, 7, 7);
					break;
				default:
					Debug.FailedAssert("Unexpected trigger state:" + triggerState, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\MissionViews\\MissionGamepadEffectsView.cs", "SetTriggerState", 495);
					break;
				}
				this._triggerState = triggerState;
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00021B0C File Offset: 0x0001FD0C
		private void ResetTriggerFeedback()
		{
			this._triggerFeedback[0] = 0;
			this._triggerFeedback[1] = 0;
			this._triggerFeedback[2] = 0;
			this._triggerFeedback[3] = 0;
			this.SetTriggerFeedback(0, 0, 0, 0);
			this.SetTriggerWeaponEffect(0, 0, 0, 0, 0, 0);
			this._triggerState = MissionGamepadEffectsView.TriggerState.Off;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00021B5A File Offset: 0x0001FD5A
		private void SetTriggerFeedback(byte leftTriggerPosition, byte leftTriggerStrength, byte rightTriggerPosition, byte rightTriggerStrength)
		{
			TaleWorlds.InputSystem.Input.SetTriggerFeedback(leftTriggerPosition, leftTriggerStrength, rightTriggerPosition, rightTriggerStrength);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00021B66 File Offset: 0x0001FD66
		private void SetTriggerWeaponEffect(byte leftStartPosition, byte leftEnd_position, byte leftStrength, byte rightStartPosition, byte rightEndPosition, byte rightStrength)
		{
			TaleWorlds.InputSystem.Input.SetTriggerWeaponEffect(leftStartPosition, leftEnd_position, leftStrength, rightStartPosition, rightEndPosition, rightStrength);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00021B78 File Offset: 0x0001FD78
		private void ResetTriggerVibration()
		{
			float[] array = new float[1];
			this.SetTriggerVibration(array, array, array, 0, array, array, array, 0);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00021B9A File Offset: 0x0001FD9A
		private void SetTriggerVibration(float[] leftTriggerAmplitudes, float[] leftTriggerFrequencies, float[] leftTriggerDurations, int numLeftTriggerElements, float[] rightTriggerAmplitudes, float[] rightTriggerFrequencies, float[] rightTriggerDurations, int numRightTriggerElements)
		{
			TaleWorlds.InputSystem.Input.SetTriggerVibration(leftTriggerAmplitudes, leftTriggerFrequencies, leftTriggerDurations, numLeftTriggerElements, rightTriggerAmplitudes, rightTriggerFrequencies, rightTriggerDurations, numRightTriggerElements);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00021BAE File Offset: 0x0001FDAE
		private static void SetLightbarColor(float red, float green, float blue)
		{
			TaleWorlds.InputSystem.Input.SetLightbarColor(red, green, blue);
		}

		// Token: 0x04000291 RID: 657
		private MissionGamepadEffectsView.TriggerState _triggerState;

		// Token: 0x04000292 RID: 658
		private readonly byte[] _triggerFeedback = new byte[4];

		// Token: 0x04000293 RID: 659
		private bool _isAdaptiveTriggerEnabled;

		// Token: 0x04000294 RID: 660
		private bool _usingAlternativeAiming;

		// Token: 0x04000295 RID: 661
		private RangedSiegeWeapon _currentlyUsedSiegeWeapon;

		// Token: 0x04000296 RID: 662
		private UsableMissionObject _currentlyUsedMissionObject;

		// Token: 0x020000DD RID: 221
		private enum TriggerState
		{
			// Token: 0x040003EE RID: 1006
			Off,
			// Token: 0x040003EF RID: 1007
			SoftTriggerFeedbackLeft,
			// Token: 0x040003F0 RID: 1008
			SoftTriggerFeedbackRight,
			// Token: 0x040003F1 RID: 1009
			HardTriggerFeedbackLeft,
			// Token: 0x040003F2 RID: 1010
			HardTriggerFeedbackRight,
			// Token: 0x040003F3 RID: 1011
			WeaponEffect,
			// Token: 0x040003F4 RID: 1012
			Vibration
		}
	}
}
