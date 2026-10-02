using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x02000039 RID: 57
	[OverrideView(typeof(MissionSingleplayerKillNotificationUIHandler))]
	public class MissionGauntletKillNotificationSingleplayerUIHandler : MissionBattleUIBaseView
	{
		// Token: 0x06000299 RID: 665 RVA: 0x0000F68C File Offset: 0x0000D88C
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this.ViewOrderPriority = 17;
			this._isGeneralFeedEnabled = BannerlordConfig.KillFeedVisualType < 2;
			this._isPersonalFeedEnabled = BannerlordConfig.ReportPersonalDamage;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnOptionChange));
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000F6E0 File Offset: 0x0000D8E0
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnOptionChange));
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000F708 File Offset: 0x0000D908
		protected override void OnCreateView()
		{
			this._dataSource = new SPKillFeedVM();
			this._gauntletLayer = new GauntletLayer("MissionSPKillFeed", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("SingleplayerKillfeed", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			CombatLogManager.OnGenerateCombatLog += this.OnCombatLogManagerOnPrintCombatLog;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000F770 File Offset: 0x0000D970
		protected override void OnDestroyView()
		{
			CombatLogManager.OnGenerateCombatLog -= this.OnCombatLogManagerOnPrintCombatLog;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000F7AD File Offset: 0x0000D9AD
		protected override void OnSuspendView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000F7C3 File Offset: 0x0000D9C3
		protected override void OnResumeView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000F7DC File Offset: 0x0000D9DC
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._dataSource != null)
			{
				bool isPaused = MBCommon.IsPaused;
				for (int i = 0; i < this._dataSource.GeneralCasualty.NotificationList.Count; i++)
				{
					this._dataSource.GeneralCasualty.NotificationList[i].IsPaused = isPaused;
				}
				for (int j = 0; j < this._dataSource.PersonalFeed.NotificationList.Count; j++)
				{
					this._dataSource.PersonalFeed.NotificationList[j].IsPaused = isPaused;
				}
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000F876 File Offset: 0x0000DA76
		private void OnOptionChange(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.ReportCasualtiesType)
			{
				this._isGeneralFeedEnabled = BannerlordConfig.KillFeedVisualType < 2;
				return;
			}
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.ReportPersonalDamage)
			{
				this._isPersonalFeedEnabled = BannerlordConfig.ReportPersonalDamage;
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000F89C File Offset: 0x0000DA9C
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			if (!base.IsViewCreated || affectorAgent == null || (agentState != AgentState.Killed && agentState != AgentState.Unconscious))
			{
				return;
			}
			bool flag = killingBlow.IsHeadShot();
			if (this._isPersonalFeedEnabled && affectorAgent == Agent.Main && (affectedAgent.IsHuman || affectedAgent.IsMount))
			{
				bool flag2 = affectedAgent.Team == affectorAgent.Team || affectedAgent.IsFriendOf(affectorAgent);
				SPKillFeedVM dataSource = this._dataSource;
				int inflictedDamage = killingBlow.InflictedDamage;
				bool isMount = affectedAgent.IsMount;
				bool flag3 = flag2;
				bool flag4 = flag;
				BasicCharacterObject character = affectedAgent.Character;
				dataSource.OnPersonalKill(inflictedDamage, isMount, flag3, flag4, (character != null) ? character.Name.ToString() : null, agentState == AgentState.Unconscious);
			}
			if (this._isGeneralFeedEnabled && affectedAgent.IsHuman)
			{
				this._dataSource.OnAgentRemoved(affectedAgent, affectorAgent, flag, affectedAgent == affectorAgent, affectedAgent == affectorAgent && affectedAgent.IsInWater());
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000F970 File Offset: 0x0000DB70
		private void OnCombatLogManagerOnPrintCombatLog(CombatLogData logData)
		{
			if (this._isPersonalFeedEnabled && (logData.IsAttackerAgentMine || logData.IsAttackerAgentRiderAgentMine) && logData.TotalDamage > 0 && (!logData.IsFatalDamage || (logData.IsEntityToEntityCollisionDamage && logData.IsSpecialDamage)))
			{
				this._dataSource.OnPersonalDamage(logData.TotalDamage, logData.IsVictimAgentMount, logData.IsFriendlyFire || logData.IsVictimAgentMine, logData.VictimAgentName);
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000F9E6 File Offset: 0x0000DBE6
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000FA0B File Offset: 0x0000DC0B
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x04000153 RID: 339
		protected SPKillFeedVM _dataSource;

		// Token: 0x04000154 RID: 340
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000155 RID: 341
		protected bool _isGeneralFeedEnabled = true;

		// Token: 0x04000156 RID: 342
		protected bool _isPersonalFeedEnabled = true;
	}
}
