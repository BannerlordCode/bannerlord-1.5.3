using System;
using SandBox.Missions.AgentBehaviors;
using SandBox.Missions.MissionLogics;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Missions.MainAgentDetection
{
	// Token: 0x02000042 RID: 66
	public class MissionDisguiseMarkerItemVM : ViewModel
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x00011EAC File Offset: 0x000100AC
		public DisguiseMissionLogic.ShadowingAgentOffenseInfo OffenseInfo { get; }

		// Token: 0x06000457 RID: 1111 RVA: 0x00011EB4 File Offset: 0x000100B4
		public MissionDisguiseMarkerItemVM(Camera missionCamera, DisguiseMissionLogic.ShadowingAgentOffenseInfo offenseInfo)
		{
			this._missionCamera = missionCamera;
			this.OffenseInfo = offenseInfo;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00011ECA File Offset: 0x000100CA
		public void RefreshVisuals()
		{
			DisguiseMissionLogic.ShadowingAgentOffenseInfo offenseInfo = this.OffenseInfo;
			this.OffenseTypeIdentifier = this.GetOffenseTypeIdentifier((offenseInfo != null) ? offenseInfo.OffenseType : StealthOffenseTypes.None);
			this.UpdateAlarmState();
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00011EF0 File Offset: 0x000100F0
		public void UpdatePosition()
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			Vec3 position = this.OffenseInfo.Agent.Position;
			position.z += this.OffenseInfo.Agent.GetEyeGlobalHeight() + 0.35f;
			if (position.IsValid)
			{
				MBWindowManager.WorldToScreenInsideUsableArea(this._missionCamera, position, ref num, ref num2, ref num3);
			}
			if (!position.IsValid || num3 < 0f || !MathF.IsValidValue(num) || !MathF.IsValidValue(num2))
			{
				num = -10000f;
				num2 = -10000f;
				num3 = 0f;
			}
			this.ScreenPosition = new Vec2(num, num2);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00011FA0 File Offset: 0x000101A0
		private void UpdateAlarmState()
		{
			Agent agent = this.OffenseInfo.Agent;
			AgentNavigator agentNavigator = agent.GetComponent<CampaignAgentComponent>().AgentNavigator;
			AlarmedBehaviorGroup alarmedBehaviorGroup = ((agentNavigator != null) ? agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>() : null);
			Agent.AIStateFlag aistateFlags = agent.AIStateFlags;
			if (aistateFlags.HasAnyFlag(Agent.AIStateFlag.Alarmed))
			{
				this._activeAlarmState = MissionDisguiseMarkerItemVM.AgentAlarmStateEnum.Alarmed;
			}
			else if (aistateFlags.HasAnyFlag(Agent.AIStateFlag.Cautious))
			{
				this._activeAlarmState = MissionDisguiseMarkerItemVM.AgentAlarmStateEnum.Cautious;
			}
			else if (aistateFlags.HasAnyFlag(Agent.AIStateFlag.PatrollingCautious))
			{
				this._activeAlarmState = MissionDisguiseMarkerItemVM.AgentAlarmStateEnum.PatrollingCautious;
			}
			else
			{
				this._activeAlarmState = MissionDisguiseMarkerItemVM.AgentAlarmStateEnum.None;
			}
			float num;
			if (aistateFlags.HasAnyFlag(Agent.AIStateFlag.Alarmed))
			{
				num = 1f;
			}
			else
			{
				num = MathF.Clamp(alarmedBehaviorGroup.AlarmFactor / 2f, 0f, 1f);
			}
			this.AlarmState = this._activeAlarmState.ToString();
			this.AlarmProgress = (int)(num * 100f);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00012068 File Offset: 0x00010268
		private string GetOffenseTypeIdentifier(StealthOffenseTypes offenseType)
		{
			if (this.IsStealthModeEnabled || !this.IsInVision || !this.IsInVisibilityRange)
			{
				this._offenseType = MissionDisguiseMarkerItemVM.AgentStealthOffenseType.None;
				return this._offenseType.ToString();
			}
			switch (offenseType)
			{
			case StealthOffenseTypes.None:
				this._offenseType = MissionDisguiseMarkerItemVM.AgentStealthOffenseType.Default;
				break;
			case StealthOffenseTypes.IsVisible:
				this._offenseType = (this.IsSuspicious ? MissionDisguiseMarkerItemVM.AgentStealthOffenseType.Suspicious : MissionDisguiseMarkerItemVM.AgentStealthOffenseType.Visible);
				break;
			case StealthOffenseTypes.IsInPersonalZone:
				this._offenseType = MissionDisguiseMarkerItemVM.AgentStealthOffenseType.Suspicious;
				break;
			}
			return this._offenseType.ToString();
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x000120EF File Offset: 0x000102EF
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x000120F7 File Offset: 0x000102F7
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value != this._screenPosition)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0001211A File Offset: 0x0001031A
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x00012122 File Offset: 0x00010322
		[DataSourceProperty]
		public int AlarmProgress
		{
			get
			{
				return this._alarmProgress;
			}
			set
			{
				if (value != this._alarmProgress)
				{
					this._alarmProgress = value;
					base.OnPropertyChangedWithValue(value, "AlarmProgress");
				}
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00012140 File Offset: 0x00010340
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x00012148 File Offset: 0x00010348
		[DataSourceProperty]
		public string AlarmState
		{
			get
			{
				return this._alarmState;
			}
			set
			{
				if (value != this._alarmState)
				{
					this._alarmState = value;
					base.OnPropertyChangedWithValue<string>(value, "AlarmState");
				}
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x0001216B File Offset: 0x0001036B
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x00012173 File Offset: 0x00010373
		[DataSourceProperty]
		public string OffenseTypeIdentifier
		{
			get
			{
				return this._offenseTypeIdentifier;
			}
			set
			{
				if (value != this._offenseTypeIdentifier)
				{
					this._offenseTypeIdentifier = value;
					base.OnPropertyChangedWithValue<string>(value, "OffenseTypeIdentifier");
				}
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00012196 File Offset: 0x00010396
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x0001219E File Offset: 0x0001039E
		[DataSourceProperty]
		public bool IsStealthModeEnabled
		{
			get
			{
				return this._isStealthModeEnabled;
			}
			set
			{
				if (value != this._isStealthModeEnabled)
				{
					this._isStealthModeEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsStealthModeEnabled");
				}
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x000121BC File Offset: 0x000103BC
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x000121C4 File Offset: 0x000103C4
		[DataSourceProperty]
		public bool IsSuspicious
		{
			get
			{
				return this._isSuspicious;
			}
			set
			{
				if (value != this._isSuspicious)
				{
					this._isSuspicious = value;
					base.OnPropertyChangedWithValue(value, "IsSuspicious");
				}
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x000121E2 File Offset: 0x000103E2
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x000121EA File Offset: 0x000103EA
		[DataSourceProperty]
		public bool IsTarget
		{
			get
			{
				return this._isTarget;
			}
			set
			{
				if (value != this._isTarget)
				{
					this._isTarget = value;
					base.OnPropertyChangedWithValue(value, "IsTarget");
				}
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x00012208 File Offset: 0x00010408
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x00012210 File Offset: 0x00010410
		[DataSourceProperty]
		public bool IsInVision
		{
			get
			{
				return this._isInVision;
			}
			set
			{
				if (value != this._isInVision)
				{
					this._isInVision = value;
					base.OnPropertyChangedWithValue(value, "IsInVision");
				}
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x0001222E File Offset: 0x0001042E
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x00012236 File Offset: 0x00010436
		[DataSourceProperty]
		public bool IsInVisibilityRange
		{
			get
			{
				return this._isInVisibilityRange;
			}
			set
			{
				if (value != this._isInVisibilityRange)
				{
					this._isInVisibilityRange = value;
					base.OnPropertyChangedWithValue(value, "IsInVisibilityRange");
				}
			}
		}

		// Token: 0x04000238 RID: 568
		private Camera _missionCamera;

		// Token: 0x0400023A RID: 570
		private MissionDisguiseMarkerItemVM.AgentAlarmStateEnum _activeAlarmState;

		// Token: 0x0400023B RID: 571
		private MissionDisguiseMarkerItemVM.AgentStealthOffenseType _offenseType;

		// Token: 0x0400023C RID: 572
		private Vec2 _screenPosition;

		// Token: 0x0400023D RID: 573
		private int _alarmProgress;

		// Token: 0x0400023E RID: 574
		private string _alarmState;

		// Token: 0x0400023F RID: 575
		private string _offenseTypeIdentifier;

		// Token: 0x04000240 RID: 576
		private bool _isStealthModeEnabled;

		// Token: 0x04000241 RID: 577
		private bool _isSuspicious;

		// Token: 0x04000242 RID: 578
		private bool _isTarget;

		// Token: 0x04000243 RID: 579
		private bool _isInVision;

		// Token: 0x04000244 RID: 580
		private bool _isInVisibilityRange;

		// Token: 0x020000A7 RID: 167
		public enum AgentAlarmStateEnum
		{
			// Token: 0x040003FD RID: 1021
			None = -1,
			// Token: 0x040003FE RID: 1022
			Alarmed,
			// Token: 0x040003FF RID: 1023
			Cautious,
			// Token: 0x04000400 RID: 1024
			PatrollingCautious,
			// Token: 0x04000401 RID: 1025
			Suspicious,
			// Token: 0x04000402 RID: 1026
			Visible
		}

		// Token: 0x020000A8 RID: 168
		public enum AgentStealthOffenseType
		{
			// Token: 0x04000404 RID: 1028
			None = -1,
			// Token: 0x04000405 RID: 1029
			Default,
			// Token: 0x04000406 RID: 1030
			Visible,
			// Token: 0x04000407 RID: 1031
			Suspicious
		}
	}
}
