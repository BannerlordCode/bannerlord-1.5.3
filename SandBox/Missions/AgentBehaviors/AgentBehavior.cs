using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000A3 RID: 163
	public abstract class AgentBehavior
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x0002CF89 File Offset: 0x0002B189
		public AgentNavigator Navigator
		{
			get
			{
				return this.BehaviorGroup.Navigator;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0002CF96 File Offset: 0x0002B196
		// (set) Token: 0x060006B4 RID: 1716 RVA: 0x0002CF9E File Offset: 0x0002B19E
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (this._isActive != value)
				{
					this._isActive = value;
					if (this._isActive)
					{
						this.OnActivate();
						return;
					}
					this.OnDeactivate();
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0002CFC5 File Offset: 0x0002B1C5
		public Agent OwnerAgent
		{
			get
			{
				return this.Navigator.OwnerAgent;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0002CFD2 File Offset: 0x0002B1D2
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x0002CFDA File Offset: 0x0002B1DA
		public Mission Mission { get; private set; }

		// Token: 0x060006B8 RID: 1720 RVA: 0x0002CFE4 File Offset: 0x0002B1E4
		protected AgentBehavior(AgentBehaviorGroup behaviorGroup)
		{
			this.Mission = behaviorGroup.Mission;
			this.CheckTime = 40f + MBRandom.RandomFloat * 20f;
			this.BehaviorGroup = behaviorGroup;
			this._isActive = false;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0002D033 File Offset: 0x0002B233
		public virtual float GetAvailability(bool isSimulation)
		{
			return 0f;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0002D03A File Offset: 0x0002B23A
		public virtual void Tick(float dt, bool isSimulation)
		{
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0002D03C File Offset: 0x0002B23C
		public virtual void ConversationTick()
		{
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002D03E File Offset: 0x0002B23E
		protected virtual void OnActivate()
		{
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0002D040 File Offset: 0x0002B240
		protected virtual void OnDeactivate()
		{
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0002D042 File Offset: 0x0002B242
		public virtual bool CheckStartWithBehavior()
		{
			return false;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0002D045 File Offset: 0x0002B245
		public virtual void OnSpecialTargetChanged()
		{
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0002D047 File Offset: 0x0002B247
		public virtual void SetCustomWanderTarget(UsableMachine customUsableMachine)
		{
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0002D049 File Offset: 0x0002B249
		public virtual void OnAgentRemoved(Agent agent)
		{
		}

		// Token: 0x060006C2 RID: 1730
		public abstract string GetDebugInfo();

		// Token: 0x04000394 RID: 916
		public float CheckTime = 15f;

		// Token: 0x04000395 RID: 917
		protected readonly AgentBehaviorGroup BehaviorGroup;

		// Token: 0x04000396 RID: 918
		private bool _isActive;
	}
}
