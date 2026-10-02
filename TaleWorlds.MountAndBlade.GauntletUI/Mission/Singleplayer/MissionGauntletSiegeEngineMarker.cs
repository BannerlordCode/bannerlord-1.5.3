using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.FormationMarker;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x0200003D RID: 61
	[OverrideView(typeof(MissionSiegeEngineMarkerView))]
	public class MissionGauntletSiegeEngineMarker : MissionBattleUIBaseView
	{
		// Token: 0x060002D0 RID: 720 RVA: 0x00010C20 File Offset: 0x0000EE20
		protected override void OnCreateView()
		{
			this._dataSource = new MissionSiegeEngineMarkerVM(base.Mission, base.MissionScreen.CombatCamera);
			this._gauntletLayer = new GauntletLayer("MissionSiegeEngineMarker", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("SiegeEngineMarker", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			this._orderHandler = base.Mission.GetMissionBehavior<MissionGauntletSingleplayerOrderUIHandler>();
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00010C9C File Offset: 0x0000EE9C
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			this._siegeEngines = new List<SiegeWeapon>();
			using (List<MissionObject>.Enumerator enumerator = base.Mission.ActiveMissionObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SiegeWeapon siegeWeapon;
					if ((siegeWeapon = enumerator.Current as SiegeWeapon) != null && siegeWeapon.DestructionComponent != null && siegeWeapon.Side != BattleSideEnum.None)
					{
						this._siegeEngines.Add(siegeWeapon);
					}
				}
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00010D24 File Offset: 0x0000EF24
		protected override void OnDestroyView()
		{
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00010D50 File Offset: 0x0000EF50
		protected override void OnSuspendView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00010D66 File Offset: 0x0000EF66
		protected override void OnResumeView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00010D7C File Offset: 0x0000EF7C
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (base.IsViewCreated)
			{
				if (!this._dataSource.IsInitialized && this._siegeEngines != null)
				{
					this._dataSource.InitializeWith(this._siegeEngines);
				}
				if (!this._orderHandler.IsDeployment)
				{
					this._dataSource.IsEnabled = base.Input.IsGameKeyDown(5);
				}
				this._dataSource.Tick(dt);
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00010DEE File Offset: 0x0000EFEE
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00010E13 File Offset: 0x0000F013
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x04000179 RID: 377
		private List<SiegeWeapon> _siegeEngines;

		// Token: 0x0400017A RID: 378
		private MissionSiegeEngineMarkerVM _dataSource;

		// Token: 0x0400017B RID: 379
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400017C RID: 380
		private MissionGauntletSingleplayerOrderUIHandler _orderHandler;
	}
}
