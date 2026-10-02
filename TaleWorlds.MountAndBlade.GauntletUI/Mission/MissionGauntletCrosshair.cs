using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x0200002F RID: 47
	[OverrideView(typeof(MissionCrosshair))]
	public class MissionGauntletCrosshair : MissionBattleUIBaseView
	{
		// Token: 0x060001EE RID: 494 RVA: 0x0000B340 File Offset: 0x00009540
		protected override void OnCreateView()
		{
			CombatLogManager.OnGenerateCombatLog += this.OnCombatLogGenerated;
			this._dataSource = new CrosshairVM();
			this._layer = new GauntletLayer("MissionCrosshair", 1, false);
			this._movie = this._layer.LoadMovie("Crosshair", this._dataSource);
			if (base.Mission.Mode != MissionMode.Conversation && base.Mission.Mode != MissionMode.CutScene)
			{
				base.MissionScreen.AddLayer(this._layer);
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000B3C8 File Offset: 0x000095C8
		protected override void OnDestroyView()
		{
			CombatLogManager.OnGenerateCombatLog -= this.OnCombatLogGenerated;
			if (base.Mission.Mode != MissionMode.Conversation && base.Mission.Mode != MissionMode.CutScene)
			{
				base.MissionScreen.RemoveLayer(this._layer);
			}
			this._dataSource = null;
			this._movie = null;
			this._layer = null;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000B429 File Offset: 0x00009629
		protected override void OnSuspendView()
		{
			if (this._layer != null)
			{
				ScreenManager.SetSuspendLayer(this._layer, true);
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000B43F File Offset: 0x0000963F
		protected override void OnResumeView()
		{
			if (this._layer != null)
			{
				ScreenManager.SetSuspendLayer(this._layer, false);
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000B458 File Offset: 0x00009658
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (base.DebugInput.IsKeyReleased(InputKey.F5) && base.IsViewCreated)
			{
				this.OnDestroyView();
				this.OnCreateView();
			}
			if (!base.IsViewCreated)
			{
				return;
			}
			if (base.IsViewSuspended != this._layer.IsActive)
			{
				ScreenManager.SetSuspendLayer(this._layer, base.IsViewSuspended);
			}
			this._dataSource.IsVisible = this.GetShouldCrosshairBeVisible();
			bool flag = true;
			bool flag2 = false;
			for (int i = 0; i < this._targetGadgetOpacities.Length; i++)
			{
				this._targetGadgetOpacities[i] = 0.0;
			}
			if (this.GetShouldArrowsBeVisible())
			{
				this._dataSource.CrosshairType = BannerlordConfig.CrosshairType;
				Agent mainAgent = base.Mission.MainAgent;
				double num = (double)(base.MissionScreen.CameraViewAngle * 0.017453292f);
				double num2 = 2.0 * Math.Tan((double)(mainAgent.CurrentAimingError + mainAgent.CurrentAimingTurbulance) * (0.5 / Math.Tan(num * 0.5)));
				this._dataSource.SetProperties(num2, (double)(1f + (base.MissionScreen.CombatCamera.HorizontalFov - 1.5707964f) / 1.5707964f));
				WeaponInfo wieldedWeaponInfo = mainAgent.GetWieldedWeaponInfo(Agent.HandIndex.MainHand);
				float num3 = MBMath.WrapAngle(mainAgent.LookDirection.AsVec2.RotationInRadians - mainAgent.GetMovementDirection().RotationInRadians);
				if (wieldedWeaponInfo.IsValid && wieldedWeaponInfo.IsRangedWeapon && BannerlordConfig.DisplayTargetingReticule)
				{
					Agent.ActionCodeType currentActionType = mainAgent.GetCurrentActionType(1);
					MissionWeapon wieldedWeapon = mainAgent.WieldedWeapon;
					if (wieldedWeapon.ReloadPhaseCount > 1 && wieldedWeapon.IsReloading && currentActionType == Agent.ActionCodeType.Reload)
					{
						StackArray.StackArray10FloatFloatTuple stackArray10FloatFloatTuple = default(StackArray.StackArray10FloatFloatTuple);
						ActionIndexCache itemUsageReloadActionCode = MBItem.GetItemUsageReloadActionCode(wieldedWeapon.CurrentUsageItem.ItemUsage, 9, mainAgent.HasMount, -1, mainAgent.GetIsLeftStance(), mainAgent.IsLookDirectionLow);
						this.FillReloadDurationsFromActions(ref stackArray10FloatFloatTuple, (int)wieldedWeapon.ReloadPhaseCount, mainAgent, itemUsageReloadActionCode);
						float num4 = mainAgent.GetCurrentActionProgress(1);
						ActionIndexCache currentAction = mainAgent.GetCurrentAction(1);
						if (currentAction != ActionIndexCache.act_none)
						{
							float num5 = 1f - MBActionSet.GetActionBlendOutStartProgress(mainAgent.ActionSet, in currentAction);
							num4 += num5;
						}
						float animationParameter = MBAnimation.GetAnimationParameter2(mainAgent.AgentVisuals.GetSkeleton().GetAnimationAtChannel(1));
						bool flag3 = num4 > animationParameter;
						float num6 = (flag3 ? 1f : (num4 / animationParameter));
						short reloadPhase = wieldedWeapon.ReloadPhase;
						for (int j = 0; j < (int)reloadPhase; j++)
						{
							stackArray10FloatFloatTuple[j] = new ValueTuple<float, float>(1f, stackArray10FloatFloatTuple[j].Item2);
						}
						if (!flag3)
						{
							stackArray10FloatFloatTuple[(int)reloadPhase] = new ValueTuple<float, float>(num6, stackArray10FloatFloatTuple[(int)reloadPhase].Item2);
							this._dataSource.SetReloadProperties(in stackArray10FloatFloatTuple, (int)wieldedWeapon.ReloadPhaseCount);
						}
						flag = false;
					}
					if (currentActionType == Agent.ActionCodeType.ReadyRanged)
					{
						Vec2 bodyRotationConstraint = mainAgent.GetBodyRotationConstraint(1);
						flag2 = base.Mission.MainAgent.MountAgent != null && !MBMath.IsBetween(num3, bodyRotationConstraint.x, bodyRotationConstraint.y) && (bodyRotationConstraint.x < -0.1f || bodyRotationConstraint.y > 0.1f);
					}
				}
				else if (!wieldedWeaponInfo.IsValid || wieldedWeaponInfo.IsMeleeWeapon)
				{
					Agent.ActionCodeType currentActionType2 = mainAgent.GetCurrentActionType(1);
					Agent.UsageDirection currentActionDirection = mainAgent.GetCurrentActionDirection(1);
					if (BannerlordConfig.DisplayAttackDirection && (currentActionType2 == Agent.ActionCodeType.ReadyMelee || MBMath.IsBetween((int)currentActionType2, 1, 15)))
					{
						if (currentActionType2 == Agent.ActionCodeType.ReadyMelee)
						{
							switch (mainAgent.AttackDirection)
							{
							case Agent.UsageDirection.AttackUp:
								this._targetGadgetOpacities[0] = 0.7;
								break;
							case Agent.UsageDirection.AttackDown:
								this._targetGadgetOpacities[2] = 0.7;
								break;
							case Agent.UsageDirection.AttackLeft:
								this._targetGadgetOpacities[3] = 0.7;
								break;
							case Agent.UsageDirection.AttackRight:
								this._targetGadgetOpacities[1] = 0.7;
								break;
							}
						}
						else
						{
							flag2 = true;
							switch (currentActionDirection)
							{
							case Agent.UsageDirection.AttackEnd:
								this._targetGadgetOpacities[0] = 0.7;
								break;
							case Agent.UsageDirection.DefendDown:
								this._targetGadgetOpacities[2] = 0.7;
								break;
							case Agent.UsageDirection.DefendLeft:
								this._targetGadgetOpacities[3] = 0.7;
								break;
							case Agent.UsageDirection.DefendRight:
								this._targetGadgetOpacities[1] = 0.7;
								break;
							}
						}
					}
					else if (BannerlordConfig.DisplayAttackDirection)
					{
						Agent.UsageDirection usageDirection = mainAgent.PlayerAttackDirection();
						if (usageDirection >= Agent.UsageDirection.AttackUp && usageDirection < Agent.UsageDirection.AttackEnd)
						{
							if (usageDirection == Agent.UsageDirection.AttackUp)
							{
								this._targetGadgetOpacities[0] = 0.7;
							}
							else if (usageDirection == Agent.UsageDirection.AttackRight)
							{
								this._targetGadgetOpacities[1] = 0.7;
							}
							else if (usageDirection == Agent.UsageDirection.AttackDown)
							{
								this._targetGadgetOpacities[2] = 0.7;
							}
							else if (usageDirection == Agent.UsageDirection.AttackLeft)
							{
								this._targetGadgetOpacities[3] = 0.7;
							}
						}
					}
				}
			}
			if (flag)
			{
				StackArray.StackArray10FloatFloatTuple stackArray10FloatFloatTuple2 = default(StackArray.StackArray10FloatFloatTuple);
				this._dataSource.SetReloadProperties(in stackArray10FloatFloatTuple2, 0);
			}
			this._dataSource.SetArrowProperties(this._targetGadgetOpacities[0], this._targetGadgetOpacities[1], this._targetGadgetOpacities[2], this._targetGadgetOpacities[3]);
			this._dataSource.IsTargetInvalid = flag2;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000B9B4 File Offset: 0x00009BB4
		protected virtual bool GetShouldArrowsBeVisible()
		{
			return !base.IsViewSuspended && base.Mission.MainAgent != null && base.Mission.Mode != MissionMode.Conversation && base.Mission.Mode != MissionMode.CutScene && base.Mission.Mode != MissionMode.Deployment && !base.MissionScreen.IsViewingCharacter() && !this.IsMissionScreenUsingCustomCamera() && !ScreenManager.GetMouseVisibility();
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000BA20 File Offset: 0x00009C20
		protected virtual bool GetShouldCrosshairBeVisible()
		{
			return this.GetShouldArrowsBeVisible() && BannerlordConfig.DisplayTargetingReticule && !base.Mission.MainAgent.WieldedWeapon.IsEmpty && base.Mission.MainAgent.WieldedWeapon.CurrentUsageItem.IsRangedWeapon && (base.Mission.MainAgent.WieldedWeapon.CurrentUsageItem.WeaponClass != WeaponClass.Crossbow || !base.Mission.MainAgent.WieldedWeapon.IsReloading);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000BAB5 File Offset: 0x00009CB5
		private bool IsMissionScreenUsingCustomCamera()
		{
			return base.MissionScreen.CustomCamera != null;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000BAC8 File Offset: 0x00009CC8
		private void OnCombatLogGenerated(CombatLogData logData)
		{
			bool isAttackerAgentMine = logData.IsAttackerAgentMine;
			bool flag = !logData.IsVictimAgentSameAsAttackerAgent && !logData.IsFriendlyFire;
			bool flag2 = logData.IsAttackerAgentHuman && logData.BodyPartHit == BoneBodyPartType.Head;
			if (isAttackerAgentMine && flag && logData.TotalDamage > 0)
			{
				CrosshairVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return;
				}
				dataSource.ShowHitMarker(logData.IsFatalDamage, flag2);
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000BB2C File Offset: 0x00009D2C
		private void FillReloadDurationsFromActions(ref StackArray.StackArray10FloatFloatTuple reloadPhases, int reloadPhaseCount, Agent mainAgent, ActionIndexCache reloadAction)
		{
			float num = 0f;
			for (int i = 0; i < reloadPhaseCount; i++)
			{
				if (reloadAction != ActionIndexCache.act_none)
				{
					float num2 = MBAnimation.GetAnimationParameter2(MBActionSet.GetAnimationIndexOfAction(mainAgent.ActionSet, in reloadAction)) * MBActionSet.GetActionAnimationDuration(mainAgent.ActionSet, in reloadAction);
					reloadPhases[i] = new ValueTuple<float, float>(reloadPhases[i].Item1, num2);
					if (num2 > num)
					{
						num = num2;
					}
					reloadAction = MBActionSet.GetActionAnimationContinueToAction(mainAgent.ActionSet, in reloadAction);
				}
			}
			if (num > 1E-05f)
			{
				for (int j = 0; j < reloadPhaseCount; j++)
				{
					reloadPhases[j] = new ValueTuple<float, float>(reloadPhases[j].Item1, reloadPhases[j].Item2 / num);
				}
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000BBE4 File Offset: 0x00009DE4
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (base.IsViewCreated)
			{
				this._layer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000BC09 File Offset: 0x00009E09
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (base.IsViewCreated)
			{
				this._layer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x040000F9 RID: 249
		private GauntletLayer _layer;

		// Token: 0x040000FA RID: 250
		private CrosshairVM _dataSource;

		// Token: 0x040000FB RID: 251
		private GauntletMovieIdentifier _movie;

		// Token: 0x040000FC RID: 252
		private double[] _targetGadgetOpacities = new double[4];
	}
}
