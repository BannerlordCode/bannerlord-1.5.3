using System;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Missions
{
	// Token: 0x0200002C RID: 44
	public class MissionAgentAlarmTargetVM : ViewModel
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00010094 File Offset: 0x0000E294
		public bool HasCautiousness
		{
			get
			{
				return this.TargetAgent.AIStateFlags.HasAnyFlag(Agent.AIStateFlag.Alarmed) || this.AlarmedBehaviorGroup.AlarmFactor > 0f;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x000100BD File Offset: 0x0000E2BD
		public AlarmedBehaviorGroup AlarmedBehaviorGroup
		{
			get
			{
				if (this._alarmedBehaviorGroupCache == null)
				{
					AgentNavigator agentNavigator = this.TargetAgent.GetComponent<CampaignAgentComponent>().AgentNavigator;
					this._alarmedBehaviorGroupCache = ((agentNavigator != null) ? agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>() : null);
				}
				return this._alarmedBehaviorGroupCache;
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x000100EF File Offset: 0x0000E2EF
		public MissionAgentAlarmTargetVM(Agent agent, Action<MissionAgentAlarmTargetVM> onRemove)
		{
			this.TargetAgent = agent;
			this._onRemove = onRemove;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00010108 File Offset: 0x0000E308
		public void UpdateValues()
		{
			string agentAlarmState = MissionAgentAlarmTargetVM.GetAgentAlarmState(this.TargetAgent.AIStateFlags);
			AlarmedBehaviorGroup alarmedBehaviorGroup = this.AlarmedBehaviorGroup;
			float num = ((alarmedBehaviorGroup != null) ? alarmedBehaviorGroup.AlarmFactor : 0f);
			if (num > 1f)
			{
				num = MathF.Min(num, 2f);
				num -= 1f;
				num = MathF.Lerp(0.3f, 1f, num, 1E-05f);
			}
			if (!this.IsInVision || !this.IsStealthModeEnabled || ((float)this.AlarmProgress <= 0f && !this.IsMainAgentInVisibilityRange))
			{
				this.AlarmProgress = 0;
				this.AlarmState = MissionAgentAlarmTargetVM.AlarmStateEnum.Invalid.ToString();
				return;
			}
			this.AlarmState = agentAlarmState;
			this.AlarmProgress = (int)(num * 100f);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x000101D0 File Offset: 0x0000E3D0
		private static string GetAgentAlarmState(Agent.AIStateFlag stateFlag)
		{
			if ((stateFlag & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.Alarmed)
			{
				return MissionAgentAlarmTargetVM.AlarmStateEnum.Alarmed.ToString();
			}
			if ((stateFlag & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.Cautious)
			{
				return MissionAgentAlarmTargetVM.AlarmStateEnum.Cautious.ToString();
			}
			if ((stateFlag & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.PatrollingCautious)
			{
				return MissionAgentAlarmTargetVM.AlarmStateEnum.PatrollingCautious.ToString();
			}
			return MissionAgentAlarmTargetVM.AlarmStateEnum.None.ToString();
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00010230 File Offset: 0x0000E430
		public void UpdateScreenPosition(Camera missionCamera)
		{
			Vec3 position = this.TargetAgent.Position;
			position.z += this.TargetAgent.GetEyeGlobalHeight() + 0.35f;
			this._latestX = 0f;
			this._latestY = 0f;
			this._latestW = 0f;
			MBWindowManager.WorldToScreenInsideUsableArea(missionCamera, position, ref this._latestX, ref this._latestY, ref this._latestW);
			this._wPosAfterPositionCalculation = ((this._latestW < 0f) ? (-1f) : 1.1f);
			this.WSign = (int)this._wPosAfterPositionCalculation;
			this.ScreenPosition = new Vec2(this._latestX, this._latestY);
			int wsign = this.WSign;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x000102EC File Offset: 0x0000E4EC
		public void ExecuteRemove()
		{
			Action<MissionAgentAlarmTargetVM> onRemove = this._onRemove;
			if (onRemove == null)
			{
				return;
			}
			onRemove(this);
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x000102FF File Offset: 0x0000E4FF
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x00010307 File Offset: 0x0000E507
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

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060003AA RID: 938 RVA: 0x00010325 File Offset: 0x0000E525
		// (set) Token: 0x060003AB RID: 939 RVA: 0x0001032D File Offset: 0x0000E52D
		[DataSourceProperty]
		public bool IsMainAgentInVisibilityRange
		{
			get
			{
				return this._isMainAgentInVisibilityRange;
			}
			set
			{
				if (value != this._isMainAgentInVisibilityRange)
				{
					this._isMainAgentInVisibilityRange = value;
					base.OnPropertyChangedWithValue(value, "IsMainAgentInVisibilityRange");
				}
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0001034B File Offset: 0x0000E54B
		// (set) Token: 0x060003AD RID: 941 RVA: 0x00010353 File Offset: 0x0000E553
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

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060003AE RID: 942 RVA: 0x00010371 File Offset: 0x0000E571
		// (set) Token: 0x060003AF RID: 943 RVA: 0x00010379 File Offset: 0x0000E579
		[DataSourceProperty]
		public bool IsSuspected
		{
			get
			{
				return this._isSuspected;
			}
			set
			{
				if (value != this._isSuspected)
				{
					this._isSuspected = value;
					base.OnPropertyChangedWithValue(value, "IsSuspected");
				}
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x00010397 File Offset: 0x0000E597
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x0001039F File Offset: 0x0000E59F
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

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x000103BD File Offset: 0x0000E5BD
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x000103C5 File Offset: 0x0000E5C5
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

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x000103E8 File Offset: 0x0000E5E8
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x000103F0 File Offset: 0x0000E5F0
		[DataSourceProperty]
		public int WSign
		{
			get
			{
				return this._wSign;
			}
			set
			{
				if (value != this._wSign)
				{
					this._wSign = value;
					base.OnPropertyChangedWithValue(value, "WSign");
				}
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0001040E File Offset: 0x0000E60E
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x00010416 File Offset: 0x0000E616
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value.x != this._screenPosition.x || value.y != this._screenPosition.y)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x040001DA RID: 474
		public readonly Agent TargetAgent;

		// Token: 0x040001DB RID: 475
		private readonly Action<MissionAgentAlarmTargetVM> _onRemove;

		// Token: 0x040001DC RID: 476
		private float _latestX;

		// Token: 0x040001DD RID: 477
		private float _latestY;

		// Token: 0x040001DE RID: 478
		private float _latestW;

		// Token: 0x040001DF RID: 479
		private float _wPosAfterPositionCalculation;

		// Token: 0x040001E0 RID: 480
		private AlarmedBehaviorGroup _alarmedBehaviorGroupCache;

		// Token: 0x040001E1 RID: 481
		private bool _isStealthModeEnabled;

		// Token: 0x040001E2 RID: 482
		private bool _isMainAgentInVisibilityRange;

		// Token: 0x040001E3 RID: 483
		private bool _isInVision;

		// Token: 0x040001E4 RID: 484
		private bool _isSuspected;

		// Token: 0x040001E5 RID: 485
		private string _alarmState;

		// Token: 0x040001E6 RID: 486
		private int _wSign;

		// Token: 0x040001E7 RID: 487
		private int _alarmProgress;

		// Token: 0x040001E8 RID: 488
		private Vec2 _screenPosition;

		// Token: 0x020000A0 RID: 160
		private enum AlarmStateEnum
		{
			// Token: 0x040003EE RID: 1006
			Invalid = -1,
			// Token: 0x040003EF RID: 1007
			None,
			// Token: 0x040003F0 RID: 1008
			Default,
			// Token: 0x040003F1 RID: 1009
			Cautious,
			// Token: 0x040003F2 RID: 1010
			PatrollingCautious,
			// Token: 0x040003F3 RID: 1011
			Alarmed
		}
	}
}
