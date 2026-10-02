using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.Missions.MissionLogics;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x0200002C RID: 44
	[OverrideView(typeof(MissionAgentStatusUIHandler))]
	public class MissionGauntletAgentStatus : MissionAgentStatusUIHandler
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000A9B4 File Offset: 0x00008BB4
		public MissionAgentStatusVM DataSource
		{
			get
			{
				return this._dataSource;
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000A9C4 File Offset: 0x00008BC4
		public override void AddInteractionMessage(MissionInteractionItemBaseVM message)
		{
			base.AddInteractionMessage(message);
			MissionAgentStatusVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.InteractionInterface.AddSecondaryMessage(message);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000A9E3 File Offset: 0x00008BE3
		public override void RemoveInteractionMessage(MissionInteractionItemBaseVM message)
		{
			base.RemoveInteractionMessage(message);
			MissionAgentStatusVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.InteractionInterface.RemoveSecondaryMessage(message);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000AA03 File Offset: 0x00008C03
		public override bool HasInteractionMessage(MissionInteractionItemBaseVM message)
		{
			return this._dataSource != null && this._dataSource.InteractionInterface.HasSecondaryInteractionMessage(message);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000AA20 File Offset: 0x00008C20
		public override void OnMissionStateActivated()
		{
			base.OnMissionStateActivated();
			MissionAgentStatusVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnMainAgentWeaponChange();
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000AA38 File Offset: 0x00008C38
		public override void EarlyStart()
		{
			base.EarlyStart();
			this._dataSource = new MissionAgentStatusVM(base.Mission, base.MissionScreen.CombatCamera, new Func<float>(base.MissionScreen.GetCameraToggleProgress));
			this._gauntletLayer = new GauntletLayer("MainAgentHUD", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MainAgentHUD", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			this._dataSource.TakenDamageController.SetIsEnabled(BannerlordConfig.EnableDamageTakenVisuals);
			this.RegisterInteractionEvents();
			CombatLogManager.OnGenerateCombatLog += this.OnGenerateCombatLog;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000AB03 File Offset: 0x00008D03
		protected override void OnCreateView()
		{
			this._dataSource.IsAgentStatusAvailable = true;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000AB11 File Offset: 0x00008D11
		protected override void OnDestroyView()
		{
			this._dataSource.IsAgentStatusAvailable = false;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000AB1F File Offset: 0x00008D1F
		protected override void OnSuspendView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000AB35 File Offset: 0x00008D35
		protected override void OnResumeView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000AB4B File Offset: 0x00008D4B
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.EnableDamageTakenVisuals)
			{
				MissionAgentStatusVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return;
				}
				dataSource.TakenDamageController.SetIsEnabled(BannerlordConfig.EnableDamageTakenVisuals);
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000AB6C File Offset: 0x00008D6C
		public override void AfterStart()
		{
			base.AfterStart();
			MissionAgentStatusVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.InitializeMainAgentPropterties();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000AB84 File Offset: 0x00008D84
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._isInDeployment = base.Mission.Mode == MissionMode.Deployment;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000ABA0 File Offset: 0x00008DA0
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			this._isInDeployment = false;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000ABB0 File Offset: 0x00008DB0
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this.UnregisterInteractionEvents();
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			CombatLogManager.OnGenerateCombatLog -= this.OnGenerateCombatLog;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			MissionAgentStatusVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._dataSource = null;
			this._missionMainAgentController = null;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000AC34 File Offset: 0x00008E34
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			this._dataSource.IsInDeployement = this._isInDeployment;
			this._dataSource.Tick(dt);
			this._dataSource.InteractionInterface.DisplayInteractionText = !base.MissionScreen.IsRadialMenuActive && !base.Mission.IsOrderMenuOpen;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000AC93 File Offset: 0x00008E93
		public override void OnFocusGained(Agent mainAgent, IFocusable focusableObject, bool isInteractable)
		{
			base.OnFocusGained(mainAgent, focusableObject, isInteractable);
			MissionAgentStatusVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnFocusGained(mainAgent, focusableObject, isInteractable);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000ACB1 File Offset: 0x00008EB1
		public override void OnAgentInteraction(Agent userAgent, Agent agent, sbyte agentBoneIndex)
		{
			base.OnAgentInteraction(userAgent, agent, agentBoneIndex);
			MissionAgentStatusVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnAgentInteraction(userAgent, agent, agentBoneIndex);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000ACCF File Offset: 0x00008ECF
		public override void OnFocusLost(Agent agent, IFocusable focusableObject)
		{
			base.OnFocusLost(agent, focusableObject);
			MissionAgentStatusVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnFocusLost(agent, focusableObject);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000ACEB File Offset: 0x00008EEB
		public override void OnAgentDeleted(Agent affectedAgent)
		{
			MissionAgentStatusVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnAgentDeleted(affectedAgent);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000ACFE File Offset: 0x00008EFE
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			MissionAgentStatusVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnAgentRemoved(affectedAgent);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000AD14 File Offset: 0x00008F14
		private void OnGenerateCombatLog(CombatLogData logData)
		{
			if (!logData.IsVictimAgentMine || logData.TotalDamage <= 0)
			{
				if (logData.IsAttackerAgentMine && logData.ReflectedDamage > 0)
				{
					MissionAgentStatusVM dataSource = this._dataSource;
					if (dataSource == null)
					{
						return;
					}
					dataSource.OnMainAgentHit(logData.ReflectedDamage, (float)(logData.IsRangedAttack ? 1 : 0));
				}
				return;
			}
			MissionAgentStatusVM dataSource2 = this._dataSource;
			if (dataSource2 == null)
			{
				return;
			}
			dataSource2.OnMainAgentHit(logData.TotalDamage, (float)(logData.IsRangedAttack ? 1 : 0));
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000AD8C File Offset: 0x00008F8C
		private void RegisterInteractionEvents()
		{
			this._missionMainAgentController = base.Mission.GetMissionBehavior<MissionMainAgentController>();
			if (this._missionMainAgentController != null)
			{
				this._missionMainAgentController.InteractionComponent.OnFocusGained += this._dataSource.OnSecondaryFocusGained;
				this._missionMainAgentController.InteractionComponent.OnFocusLost += this._dataSource.OnSecondaryFocusLost;
				this._missionMainAgentController.InteractionComponent.OnFocusHealthChanged += this._dataSource.InteractionInterface.OnFocusedHealthChanged;
			}
			this._missionMainAgentEquipmentControllerView = base.Mission.GetMissionBehavior<MissionGauntletMainAgentEquipmentControllerView>();
			if (this._missionMainAgentEquipmentControllerView != null)
			{
				this._missionMainAgentEquipmentControllerView.OnEquipmentDropInteractionViewToggled += this._dataSource.OnEquipmentInteractionViewToggled;
				this._missionMainAgentEquipmentControllerView.OnEquipmentEquipInteractionViewToggled += this._dataSource.OnEquipmentInteractionViewToggled;
			}
			this._missionHintLogic = base.Mission.GetMissionBehavior<MissionHintLogic>();
			if (this._missionHintLogic != null)
			{
				this._missionHintLogic.OnActiveHintChanged += this._dataSource.InteractionInterface.OnActiveMissionHintChanged;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000AEA8 File Offset: 0x000090A8
		private void UnregisterInteractionEvents()
		{
			if (this._missionMainAgentController != null)
			{
				this._missionMainAgentController.InteractionComponent.OnFocusGained -= this._dataSource.OnSecondaryFocusGained;
				this._missionMainAgentController.InteractionComponent.OnFocusLost -= this._dataSource.OnSecondaryFocusLost;
				this._missionMainAgentController.InteractionComponent.OnFocusHealthChanged -= this._dataSource.InteractionInterface.OnFocusedHealthChanged;
			}
			if (this._missionMainAgentEquipmentControllerView != null)
			{
				this._missionMainAgentEquipmentControllerView.OnEquipmentDropInteractionViewToggled -= this._dataSource.OnEquipmentInteractionViewToggled;
				this._missionMainAgentEquipmentControllerView.OnEquipmentEquipInteractionViewToggled -= this._dataSource.OnEquipmentInteractionViewToggled;
			}
			this._missionHintLogic = base.Mission.GetMissionBehavior<MissionHintLogic>();
			if (this._missionHintLogic != null)
			{
				this._missionHintLogic.OnActiveHintChanged -= this._dataSource.InteractionInterface.OnActiveMissionHintChanged;
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000AF9F File Offset: 0x0000919F
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
			this.UnregisterInteractionEvents();
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000AFCA File Offset: 0x000091CA
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
			this.RegisterInteractionEvents();
		}

		// Token: 0x040000ED RID: 237
		protected GauntletLayer _gauntletLayer;

		// Token: 0x040000EE RID: 238
		protected MissionAgentStatusVM _dataSource;

		// Token: 0x040000EF RID: 239
		protected MissionMainAgentController _missionMainAgentController;

		// Token: 0x040000F0 RID: 240
		protected MissionGauntletMainAgentEquipmentControllerView _missionMainAgentEquipmentControllerView;

		// Token: 0x040000F1 RID: 241
		protected MissionHintLogic _missionHintLogic;

		// Token: 0x040000F2 RID: 242
		protected bool _isInDeployment;
	}
}
