using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.FormationMarker;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x02000038 RID: 56
	[OverrideView(typeof(MissionFormationMarkerUIHandler))]
	public class MissionGauntletFormationMarker : MissionBattleUIBaseView
	{
		// Token: 0x0600028B RID: 651 RVA: 0x0000F050 File Offset: 0x0000D250
		public void SetMarkerDistanceConfig(float farDistanceCutoff, float farAlphaTarget, float alwaysOnDistance)
		{
			MissionFormationMarkerVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.SetMarkerDistanceConfig(farDistanceCutoff, farAlphaTarget, alwaysOnDistance);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000F068 File Offset: 0x0000D268
		protected override void OnCreateView()
		{
			this._dataSource = new MissionFormationMarkerVM(base.Mission, GameNetwork.IsMultiplayer);
			string text = "MissionFormationMarker";
			int viewOrderPriority = this.ViewOrderPriority;
			this.ViewOrderPriority = viewOrderPriority + 1;
			this._gauntletLayer = new GauntletLayer(text, viewOrderPriority, false);
			this._gauntletLayer.LoadMovie("FormationMarker", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			this._formationTargetHandler = base.Mission.GetMissionBehavior<MissionFormationTargetSelectionHandler>();
			if (this._formationTargetHandler != null)
			{
				this._formationTargetHandler.OnFormationFocused += this.OnFormationFocusedFromHandler;
			}
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			this.UpdateShowDistanceTexts();
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000F12C File Offset: 0x0000D32C
		protected override void OnDestroyView()
		{
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			if (this._formationTargetHandler != null)
			{
				this._formationTargetHandler.OnFormationFocused -= this.OnFormationFocusedFromHandler;
			}
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000F1A2 File Offset: 0x0000D3A2
		protected override void OnSuspendView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000F1B8 File Offset: 0x0000D3B8
		protected override void OnResumeView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000F1CE File Offset: 0x0000D3CE
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType optionType)
		{
			if (optionType == ManagedOptions.ManagedOptionsType.ShowFormationDistances)
			{
				this.UpdateShowDistanceTexts();
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000F1DB File Offset: 0x0000D3DB
		private void UpdateShowDistanceTexts()
		{
			this._showDistanceTexts = ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.ShowFormationDistances) > 1E-05f;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000F1F4 File Offset: 0x0000D3F4
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (base.IsViewCreated)
			{
				if (base.Mission.Mode != MissionMode.Deployment)
				{
					this._dataSource.IsEnabled = base.Input.IsGameKeyDown(5) || base.Mission.IsOrderMenuOpen;
				}
				this._dataSource.IsFormationTargetRelevant = this._formationTargetHandler != null && base.Mission.IsOrderMenuOpen;
				this._dataSource.ShowDistanceTexts = this._showDistanceTexts;
				if (this._dataSource.IsEnabled)
				{
					this._dataSource.RefreshFormationMarkers();
					this.RefreshTargetProperties();
					this.UpdateMarkerPositions();
					this._fadeOutTimer = 2f;
					return;
				}
				if (this._fadeOutTimer >= 0f)
				{
					this._fadeOutTimer -= dt;
					this.UpdateMarkerPositions();
				}
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000F2CC File Offset: 0x0000D4CC
		private void UpdateMarkerPositions()
		{
			for (int i = 0; i < this._dataSource.Targets.Count; i++)
			{
				MissionFormationMarkerTargetVM missionFormationMarkerTargetVM = this._dataSource.Targets[i];
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				WorldPosition cachedMedianPosition = missionFormationMarkerTargetVM.Formation.CachedMedianPosition;
				if (cachedMedianPosition.IsValid)
				{
					MBWindowManager.WorldToScreen(base.MissionScreen.CombatCamera, cachedMedianPosition.GetGroundVec3() + this._heightOffset, ref num, ref num2, ref num3);
					if (!MathF.IsValidValue(num3) || !MathF.IsValidValue(num) || !MathF.IsValidValue(num2))
					{
						num = -10000f;
						num2 = -10000f;
						num3 = -1f;
					}
					missionFormationMarkerTargetVM.WSign = ((num3 < 0f) ? (-1) : 1);
					missionFormationMarkerTargetVM.Distance = base.MissionScreen.CombatCamera.Position.Distance(cachedMedianPosition.GetGroundVec3());
					missionFormationMarkerTargetVM.ScreenPosition = new Vec2(num, num2);
					missionFormationMarkerTargetVM.VisibilityRatio = ((this._formationTargetHandler != null) ? this._formationTargetHandler.GetFormationVisibilityRatio(missionFormationMarkerTargetVM.Formation) : 1f);
					missionFormationMarkerTargetVM.VisibilityState = this.GetVisibilityState(missionFormationMarkerTargetVM);
					if (this._dataSource.ShowDistanceTexts)
					{
						MissionFormationMarkerTargetVM missionFormationMarkerTargetVM2 = missionFormationMarkerTargetVM;
						Agent main = Agent.Main;
						missionFormationMarkerTargetVM2.DistanceText = ((main != null && main.IsActive()) ? ((int)Agent.Main.Position.Distance(cachedMedianPosition.GetGroundVec3())).ToString() : ((int)missionFormationMarkerTargetVM.Distance).ToString());
					}
					else
					{
						missionFormationMarkerTargetVM.DistanceText = string.Empty;
					}
				}
				else
				{
					missionFormationMarkerTargetVM.WSign = -1;
					missionFormationMarkerTargetVM.Distance = 10000f;
					missionFormationMarkerTargetVM.DistanceText = string.Empty;
					missionFormationMarkerTargetVM.ScreenPosition = new Vec2(-10000f, -10000f);
					missionFormationMarkerTargetVM.VisibilityRatio = 1f;
					missionFormationMarkerTargetVM.VisibilityState = -1;
				}
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000F4B8 File Offset: 0x0000D6B8
		private int GetVisibilityState(MissionFormationMarkerTargetVM target)
		{
			if (this._formationTargetHandler == null)
			{
				return -1;
			}
			MissionFormationTargetSelectionHandler.FormationMarkerVisibility formationMarkerVisibility = this._formationTargetHandler.GetFormationMarkerVisibility(target.Formation);
			if (formationMarkerVisibility == MissionFormationTargetSelectionHandler.FormationMarkerVisibility.NotEvaluated)
			{
				return -1;
			}
			if (formationMarkerVisibility == MissionFormationTargetSelectionHandler.FormationMarkerVisibility.Hidden)
			{
				return 0;
			}
			if (target.Distance > target.AlwaysOnDistance)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000F500 File Offset: 0x0000D700
		private unsafe void RefreshTargetProperties()
		{
			if (!this._dataSource.IsFormationTargetRelevant)
			{
				for (int i = 0; i < this._dataSource.Targets.Count; i++)
				{
					this._dataSource.Targets[i].SetTargetedState(false, false);
				}
				return;
			}
			List<Formation> list = new List<Formation>();
			Agent main = Agent.Main;
			MBReadOnlyList<Formation> mbreadOnlyList;
			if (main == null)
			{
				mbreadOnlyList = null;
			}
			else
			{
				OrderController playerOrderController = main.Team.PlayerOrderController;
				mbreadOnlyList = ((playerOrderController != null) ? playerOrderController.SelectedFormations : null);
			}
			MBReadOnlyList<Formation> mbreadOnlyList2 = mbreadOnlyList;
			if (mbreadOnlyList2 != null)
			{
				for (int j = 0; j < mbreadOnlyList2.Count; j++)
				{
					if (mbreadOnlyList2[j].TargetFormation != null)
					{
						MovementOrder movementOrder = *mbreadOnlyList2[j].GetReadonlyMovementOrderReference();
						if (movementOrder.OrderType == OrderType.Charge || movementOrder.OrderType == OrderType.Advance)
						{
							list.Add(mbreadOnlyList2[j].TargetFormation);
						}
					}
				}
			}
			for (int k = 0; k < this._dataSource.Targets.Count; k++)
			{
				MissionFormationMarkerTargetVM missionFormationMarkerTargetVM = this._dataSource.Targets[k];
				if (missionFormationMarkerTargetVM.TeamType == 2)
				{
					bool flag = list.Contains(missionFormationMarkerTargetVM.Formation);
					missionFormationMarkerTargetVM.SetTargetedState(this._focusedFormation == missionFormationMarkerTargetVM.Formation, flag);
				}
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000F637 File Offset: 0x0000D837
		private void OnFormationFocusedFromHandler(Formation focusedFormation)
		{
			this._focusedFormation = focusedFormation;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000F640 File Offset: 0x0000D840
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000F665 File Offset: 0x0000D865
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x0400014C RID: 332
		private MissionFormationMarkerVM _dataSource;

		// Token: 0x0400014D RID: 333
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400014E RID: 334
		private MissionFormationTargetSelectionHandler _formationTargetHandler;

		// Token: 0x0400014F RID: 335
		private Formation _focusedFormation;

		// Token: 0x04000150 RID: 336
		private readonly Vec3 _heightOffset = new Vec3(0f, 0f, 3f, -1f);

		// Token: 0x04000151 RID: 337
		private float _fadeOutTimer;

		// Token: 0x04000152 RID: 338
		private bool _showDistanceTexts;
	}
}
