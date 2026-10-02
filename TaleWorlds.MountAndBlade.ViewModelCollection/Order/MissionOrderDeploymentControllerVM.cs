using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Handlers;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x02000019 RID: 25
	public class MissionOrderDeploymentControllerVM : ViewModel
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00007144 File Offset: 0x00005344
		private Mission Mission
		{
			get
			{
				return Mission.Current;
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000714C File Offset: 0x0000534C
		public MissionOrderDeploymentControllerVM(MissionOrderVM missionOrder)
		{
			this._missionOrder = missionOrder;
			this._deploymentHandler = this.Mission.GetMissionBehavior<DeploymentHandler>();
			if (this._deploymentHandler != null)
			{
				this._deploymentHandler.OnPlayerSideDeploymentReady += this.ExecuteDeployPlayerSide;
				SiegeDeploymentHandler siegeDeploymentHandler;
				if ((siegeDeploymentHandler = this._deploymentHandler as SiegeDeploymentHandler) != null)
				{
					this._siegeDeploymentHandler = siegeDeploymentHandler;
					this._siegeDeploymentHandler.OnEnemySideDeploymentReady += this.ExecuteDeployEnemySide;
				}
			}
			this._siegeDeployQueryData = new InquiryData(new TextObject("{=TxphX8Uk}Deployment", null).ToString(), new TextObject("{=LlrlE199}You can still deploy siege engines.{newline}Begin anyway?", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), delegate
			{
				this._siegeDeploymentHandler.FinishDeployment();
				this._missionOrder.TryCloseToggleOrder(false);
			}, null, "", 0f, null, null, null);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000722C File Offset: 0x0000542C
		internal void DeployFormationsOfPlayer()
		{
			if (this._siegeDeploymentHandler != null)
			{
				this._siegeDeploymentHandler.AutoDeployTeamUsingTeamAI(this.Mission.PlayerTeam, false);
			}
			else if (!this.Mission.IsNavalBattle && !this.Mission.IsNavalRaidBattle && this._deploymentHandler != null)
			{
				this._deploymentHandler.AutoDeployTeamUsingDeploymentPlan(this.Mission.PlayerTeam);
			}
			AssignPlayerRoleInTeamMissionController missionBehavior = Mission.Current.GetMissionBehavior<AssignPlayerRoleInTeamMissionController>();
			if (missionBehavior != null)
			{
				missionBehavior.OnPlayerTeamDeployed();
			}
			if (this._siegeDeploymentHandler != null)
			{
				this._siegeDeploymentHandler.AutoAssignDetachmentsForDeployment(this.Mission.PlayerTeam);
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x000072C5 File Offset: 0x000054C5
		public void ExecuteBeginMission(bool showSiegeMachineInquiry = false)
		{
			if (showSiegeMachineInquiry)
			{
				InformationManager.ShowInquiry(this._siegeDeployQueryData, false, false);
				return;
			}
			if (this._deploymentHandler != null)
			{
				this._missionOrder.TryCloseToggleOrder(false);
				this._deploymentHandler.FinishDeployment();
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000072F8 File Offset: 0x000054F8
		public void ExecuteAutoDeploy()
		{
			IMissionDeploymentPlan missionDeploymentPlan;
			this.Mission.GetDeploymentPlan<IMissionDeploymentPlan>(out missionDeploymentPlan);
			missionDeploymentPlan.RemakeDeploymentPlan(this.Mission.PlayerTeam);
			if (this._siegeDeploymentHandler != null)
			{
				this._siegeDeploymentHandler.AutoDeployTeamUsingTeamAI(this.Mission.PlayerTeam, true);
			}
			else if (this._deploymentHandler != null)
			{
				this._deploymentHandler.AutoDeployTeamUsingDeploymentPlan(this.Mission.PlayerTeam);
			}
			if (this._deploymentHandler != null)
			{
				this._deploymentHandler.HandleGeneralsDeploymentFrames();
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00007378 File Offset: 0x00005578
		public void ExecuteDeployPlayerSide()
		{
			if (this._siegeDeploymentHandler != null)
			{
				this.Mission.ForceTickOccasionally = true;
				bool isTeleportingAgents = Mission.Current.IsTeleportingAgents;
				if (!this.Mission.IsNavalBattle)
				{
					this.Mission.IsTeleportingAgents = true;
				}
				if (!this.Mission.IsSallyOutBattle || this.Mission.PlayerTeam.Side == BattleSideEnum.Attacker)
				{
					this.DeployFormationsOfPlayer();
					this._siegeDeploymentHandler.ForceUpdateAllUnits();
				}
				this._missionOrder.OnDeployAll();
				if (!this.Mission.IsNavalBattle)
				{
					this.Mission.IsTeleportingAgents = isTeleportingAgents;
				}
				this.Mission.ForceTickOccasionally = false;
				return;
			}
			if (this._deploymentHandler != null)
			{
				this.DeployFormationsOfPlayer();
				this._deploymentHandler.ForceUpdateAllUnits();
				this._missionOrder.OnDeployAll();
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007448 File Offset: 0x00005648
		private void ExecuteDeployEnemySide()
		{
			if (this._siegeDeploymentHandler != null)
			{
				this.Mission.ForceTickOccasionally = true;
				bool isTeleportingAgents = Mission.Current.IsTeleportingAgents;
				if (!this.Mission.IsNavalBattle)
				{
					this.Mission.IsTeleportingAgents = true;
				}
				if (!this.Mission.IsSallyOutBattle || this.Mission.PlayerTeam.Side == BattleSideEnum.Defender)
				{
					this._siegeDeploymentHandler.AutoDeployTeamUsingTeamAI(this.Mission.PlayerEnemyTeam, true);
					this._siegeDeploymentHandler.ForceUpdateAllUnits();
				}
				this._missionOrder.OnDeployAll();
				if (!this.Mission.IsNavalBattle)
				{
					this.Mission.IsTeleportingAgents = isTeleportingAgents;
				}
				this.Mission.ForceTickOccasionally = false;
				return;
			}
			if (this._deploymentHandler != null)
			{
				this._deploymentHandler.ForceUpdateAllUnits();
				this._missionOrder.OnDeployAll();
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00007520 File Offset: 0x00005720
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this._deploymentHandler != null)
			{
				this._deploymentHandler.OnPlayerSideDeploymentReady -= this.ExecuteDeployPlayerSide;
			}
			if (this._siegeDeploymentHandler != null)
			{
				this._siegeDeploymentHandler.OnEnemySideDeploymentReady -= this.ExecuteDeployEnemySide;
			}
			this._siegeDeploymentHandler = null;
			this._siegeDeployQueryData = null;
		}

		// Token: 0x040000EC RID: 236
		private DeploymentHandler _deploymentHandler;

		// Token: 0x040000ED RID: 237
		private SiegeDeploymentHandler _siegeDeploymentHandler;

		// Token: 0x040000EE RID: 238
		private InquiryData _siegeDeployQueryData;

		// Token: 0x040000EF RID: 239
		private readonly MissionOrderVM _missionOrder;
	}
}
