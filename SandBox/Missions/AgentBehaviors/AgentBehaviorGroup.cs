using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000A4 RID: 164
	public abstract class AgentBehaviorGroup
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0002D04B File Offset: 0x0002B24B
		public Agent OwnerAgent
		{
			get
			{
				return this.Navigator.OwnerAgent;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x0002D058 File Offset: 0x0002B258
		// (set) Token: 0x060006C5 RID: 1733 RVA: 0x0002D060 File Offset: 0x0002B260
		public AgentBehavior ScriptedBehavior { get; private set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x0002D069 File Offset: 0x0002B269
		// (set) Token: 0x060006C7 RID: 1735 RVA: 0x0002D071 File Offset: 0x0002B271
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

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x0002D098 File Offset: 0x0002B298
		// (set) Token: 0x060006C9 RID: 1737 RVA: 0x0002D0A0 File Offset: 0x0002B2A0
		public Mission Mission { get; private set; }

		// Token: 0x060006CA RID: 1738 RVA: 0x0002D0A9 File Offset: 0x0002B2A9
		protected AgentBehaviorGroup(AgentNavigator navigator, Mission mission)
		{
			this.Mission = mission;
			this.Behaviors = new List<AgentBehavior>();
			this.Navigator = navigator;
			this._isActive = false;
			this.ScriptedBehavior = null;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0002D0E4 File Offset: 0x0002B2E4
		public T AddBehavior<T>() where T : AgentBehavior
		{
			T t = Activator.CreateInstance(typeof(T), new object[] { this }) as T;
			if (t != null)
			{
				foreach (AgentBehavior agentBehavior in this.Behaviors)
				{
					if (agentBehavior.GetType() == t.GetType())
					{
						return agentBehavior as T;
					}
				}
				this.Behaviors.Add(t);
				return t;
			}
			return t;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0002D198 File Offset: 0x0002B398
		public T GetBehavior<T>() where T : AgentBehavior
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				if (agentBehavior is T)
				{
					return (T)((object)agentBehavior);
				}
			}
			return default(T);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0002D200 File Offset: 0x0002B400
		public bool HasBehavior<T>() where T : AgentBehavior
		{
			using (List<AgentBehavior>.Enumerator enumerator = this.Behaviors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is T)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0002D25C File Offset: 0x0002B45C
		public void RemoveBehavior<T>() where T : AgentBehavior
		{
			for (int i = 0; i < this.Behaviors.Count; i++)
			{
				if (this.Behaviors[i] is T)
				{
					bool isActive = this.Behaviors[i].IsActive;
					this.Behaviors[i].IsActive = false;
					if (this.ScriptedBehavior == this.Behaviors[i])
					{
						this.ScriptedBehavior = null;
					}
					this.Behaviors.RemoveAt(i);
					if (isActive)
					{
						this.ForceThink(0f);
					}
				}
			}
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0002D2EC File Offset: 0x0002B4EC
		public void SetScriptedBehavior<T>() where T : AgentBehavior
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				if (agentBehavior is T)
				{
					this.ScriptedBehavior = agentBehavior;
					this.ForceThink(0f);
					break;
				}
			}
			foreach (AgentBehavior agentBehavior2 in this.Behaviors)
			{
				if (agentBehavior2 != this.ScriptedBehavior)
				{
					agentBehavior2.IsActive = false;
				}
			}
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0002D3A0 File Offset: 0x0002B5A0
		public void DisableScriptedBehavior()
		{
			if (this.ScriptedBehavior != null)
			{
				this.ScriptedBehavior.IsActive = false;
				this.ScriptedBehavior = null;
				this.ForceThink(0f);
			}
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0002D3C8 File Offset: 0x0002B5C8
		public void DisableAllBehaviors()
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				agentBehavior.IsActive = false;
			}
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0002D41C File Offset: 0x0002B61C
		public AgentBehavior GetActiveBehavior()
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				if (agentBehavior.IsActive)
				{
					return agentBehavior;
				}
			}
			return null;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0002D478 File Offset: 0x0002B678
		public virtual void Tick(float dt, bool isSimulation)
		{
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0002D47A File Offset: 0x0002B67A
		public virtual void ConversationTick()
		{
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0002D47C File Offset: 0x0002B67C
		public virtual void OnAgentRemoved(Agent agent)
		{
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002D47E File Offset: 0x0002B67E
		protected virtual void OnActivate()
		{
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0002D480 File Offset: 0x0002B680
		protected virtual void OnDeactivate()
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				agentBehavior.IsActive = false;
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0002D4D4 File Offset: 0x0002B6D4
		public virtual float GetScore(bool isSimulation)
		{
			return 0f;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0002D4DB File Offset: 0x0002B6DB
		public virtual void ForceThink(float inSeconds)
		{
		}

		// Token: 0x04000398 RID: 920
		public AgentNavigator Navigator;

		// Token: 0x04000399 RID: 921
		public List<AgentBehavior> Behaviors;

		// Token: 0x0400039A RID: 922
		protected float CheckBehaviorTime = 5f;

		// Token: 0x0400039B RID: 923
		protected Timer CheckBehaviorTimer;

		// Token: 0x0400039D RID: 925
		private bool _isActive;
	}
}
