using System;
using System.Collections.Generic;
using SandBox.Missions.MissionLogics;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;

namespace SandBox.ViewModelCollection.Missions
{
	// Token: 0x0200002B RID: 43
	public class MissionAgentAlarmStateVM : ViewModel
	{
		// Token: 0x0600038F RID: 911 RVA: 0x0000FC05 File Offset: 0x0000DE05
		public MissionAgentAlarmStateVM()
		{
			this.Targets = new MBBindingList<MissionAgentAlarmTargetVM>();
			this._stealthBoxes = new List<StealthBox>();
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000FC24 File Offset: 0x0000DE24
		public void Initialize(Mission mission, Camera camera)
		{
			this._mission = mission;
			this._camera = camera;
			this._isInitialized = true;
			this._areStealthBoxesDirty = true;
			this.RefreshTargets();
			StealthBox.OnBoxInitialized += this.OnStealthBoxInitialized;
			StealthBox.OnBoxRemoved += this.OnStealthBoxRemoved;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000FC75 File Offset: 0x0000DE75
		public override void OnFinalize()
		{
			base.OnFinalize();
			StealthBox.OnBoxInitialized -= this.OnStealthBoxInitialized;
			StealthBox.OnBoxRemoved -= this.OnStealthBoxRemoved;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000FC9F File Offset: 0x0000DE9F
		private void OnStealthBoxInitialized(StealthBox stealthBox)
		{
			this._areStealthBoxesDirty = true;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000FCA8 File Offset: 0x0000DEA8
		private void OnStealthBoxRemoved(StealthBox stealthBox)
		{
			this._areStealthBoxesDirty = true;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000FCB4 File Offset: 0x0000DEB4
		private void RefreshStealthBoxEntities()
		{
			this._stealthBoxes.Clear();
			Mission mission = Mission.Current;
			if (((mission != null) ? mission.Scene : null) == null)
			{
				return;
			}
			List<GameEntity> list = new List<GameEntity>();
			Mission.Current.Scene.GetAllEntitiesWithScriptComponent<StealthBox>(ref list);
			for (int i = 0; i < list.Count; i++)
			{
				StealthBox firstScriptOfTypeRecursive = list[i].GetFirstScriptOfTypeRecursive<StealthBox>();
				if (firstScriptOfTypeRecursive != null)
				{
					this._stealthBoxes.Add(firstScriptOfTypeRecursive);
				}
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000FD2C File Offset: 0x0000DF2C
		public void Update()
		{
			if (!this._isInitialized)
			{
				return;
			}
			if (this._disguiseMissionLogic == null)
			{
				Mission mission = this._mission;
				this._disguiseMissionLogic = ((mission != null) ? mission.GetMissionBehavior<DisguiseMissionLogic>() : null);
			}
			DisguiseMissionLogic disguiseMissionLogic = this._disguiseMissionLogic;
			bool flag = disguiseMissionLogic != null && disguiseMissionLogic.IsInStealthMode;
			this.IsMainAgentInSafeArea = this.IsMainAgentInStealthArea();
			for (int i = 0; i < this.Targets.Count; i++)
			{
				MissionAgentAlarmTargetVM missionAgentAlarmTargetVM = this.Targets[i];
				if (this._disguiseMissionLogic == null)
				{
					missionAgentAlarmTargetVM.IsStealthModeEnabled = true;
					missionAgentAlarmTargetVM.IsMainAgentInVisibilityRange = SandBoxUIHelper.IsAgentInVisibilityRangeApproximate(missionAgentAlarmTargetVM.TargetAgent, Agent.Main);
					missionAgentAlarmTargetVM.IsInVision = true;
					missionAgentAlarmTargetVM.IsSuspected = missionAgentAlarmTargetVM.AlarmProgress > 0;
					missionAgentAlarmTargetVM.UpdateScreenPosition(this._camera);
					missionAgentAlarmTargetVM.UpdateValues();
				}
				else
				{
					missionAgentAlarmTargetVM.IsStealthModeEnabled = flag;
					DisguiseMissionLogic.ShadowingAgentOffenseInfo agentOffenseInfo = this._disguiseMissionLogic.GetAgentOffenseInfo(missionAgentAlarmTargetVM.TargetAgent);
					if (agentOffenseInfo != null)
					{
						missionAgentAlarmTargetVM.IsMainAgentInVisibilityRange = SandBoxUIHelper.IsAgentInVisibilityRangeApproximate(missionAgentAlarmTargetVM.TargetAgent, Agent.Main);
						missionAgentAlarmTargetVM.IsInVision = agentOffenseInfo.CanPlayerCameraSeeTheAgent;
						missionAgentAlarmTargetVM.IsSuspected = missionAgentAlarmTargetVM.AlarmProgress > 0;
					}
					missionAgentAlarmTargetVM.UpdateScreenPosition(this._camera);
					missionAgentAlarmTargetVM.UpdateValues();
				}
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000FE58 File Offset: 0x0000E058
		private bool IsMainAgentInStealthArea()
		{
			Agent main = Agent.Main;
			if (main == null)
			{
				return false;
			}
			Mission mission = Mission.Current;
			if (((mission != null) ? mission.Scene : null) == null)
			{
				return false;
			}
			if (this._areStealthBoxesDirty)
			{
				this.RefreshStealthBoxEntities();
				this._areStealthBoxesDirty = false;
			}
			for (int i = 0; i < this._stealthBoxes.Count; i++)
			{
				if (this._stealthBoxes[i].IsAgentInside(main))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000FED0 File Offset: 0x0000E0D0
		public void OnAgentRemoved(Agent agent)
		{
			MissionAgentAlarmTargetVM agentTargetFromAgent = this.GetAgentTargetFromAgent(agent);
			if (agentTargetFromAgent != null)
			{
				this.Targets.Remove(agentTargetFromAgent);
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000FEF8 File Offset: 0x0000E0F8
		private void RefreshTargets()
		{
			this.Targets.Clear();
			foreach (Agent agent in Mission.Current.Agents)
			{
				if (agent != null && SandBoxUIHelper.CanAgentBeAlarmed(agent))
				{
					this.Targets.Add(new MissionAgentAlarmTargetVM(agent, new Action<MissionAgentAlarmTargetVM>(this.OnRemoveTarget)));
				}
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000FF7C File Offset: 0x0000E17C
		public void OnAgentBuild(Agent agent, Banner banner)
		{
			this.RefreshTargets();
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000FF84 File Offset: 0x0000E184
		public void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
		{
			if (agent != null && agent == Agent.Main)
			{
				this.RefreshTargets();
				return;
			}
			MissionAgentAlarmTargetVM agentTargetFromAgent = this.GetAgentTargetFromAgent(agent);
			if (agentTargetFromAgent == null && SandBoxUIHelper.CanAgentBeAlarmed(agent))
			{
				this.Targets.Add(new MissionAgentAlarmTargetVM(agent, new Action<MissionAgentAlarmTargetVM>(this.OnRemoveTarget)));
				return;
			}
			if (agentTargetFromAgent != null && (newTeam == Team.Invalid || (newTeam == null || newTeam.IsPlayerAlly)))
			{
				this.Targets.Remove(agentTargetFromAgent);
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000FFFA File Offset: 0x0000E1FA
		private void OnRemoveTarget(MissionAgentAlarmTargetVM targetToRemove)
		{
			this.Targets.Remove(targetToRemove);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0001000C File Offset: 0x0000E20C
		private MissionAgentAlarmTargetVM GetAgentTargetFromAgent(Agent agent)
		{
			for (int i = 0; i < this.Targets.Count; i++)
			{
				MissionAgentAlarmTargetVM missionAgentAlarmTargetVM = this.Targets[i];
				if (missionAgentAlarmTargetVM.TargetAgent == agent)
				{
					return missionAgentAlarmTargetVM;
				}
			}
			return null;
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00010048 File Offset: 0x0000E248
		// (set) Token: 0x0600039E RID: 926 RVA: 0x00010050 File Offset: 0x0000E250
		[DataSourceProperty]
		public MBBindingList<MissionAgentAlarmTargetVM> Targets
		{
			get
			{
				return this._targets;
			}
			set
			{
				if (value != this._targets)
				{
					this._targets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionAgentAlarmTargetVM>>(value, "Targets");
				}
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600039F RID: 927 RVA: 0x0001006E File Offset: 0x0000E26E
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x00010076 File Offset: 0x0000E276
		[DataSourceProperty]
		public bool IsMainAgentInSafeArea
		{
			get
			{
				return this._isMainAgentInSafeArea;
			}
			set
			{
				if (value != this._isMainAgentInSafeArea)
				{
					this._isMainAgentInSafeArea = value;
					base.OnPropertyChangedWithValue(value, "IsMainAgentInSafeArea");
				}
			}
		}

		// Token: 0x040001D2 RID: 466
		private bool _isInitialized;

		// Token: 0x040001D3 RID: 467
		private Mission _mission;

		// Token: 0x040001D4 RID: 468
		private Camera _camera;

		// Token: 0x040001D5 RID: 469
		private DisguiseMissionLogic _disguiseMissionLogic;

		// Token: 0x040001D6 RID: 470
		private bool _areStealthBoxesDirty;

		// Token: 0x040001D7 RID: 471
		private List<StealthBox> _stealthBoxes;

		// Token: 0x040001D8 RID: 472
		private bool _isMainAgentInSafeArea;

		// Token: 0x040001D9 RID: 473
		private MBBindingList<MissionAgentAlarmTargetVM> _targets;
	}
}
