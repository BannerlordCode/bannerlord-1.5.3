using System;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000AF RID: 175
	public class InterruptingBehaviorGroup : AgentBehaviorGroup
	{
		// Token: 0x06000754 RID: 1876 RVA: 0x00031ACF File Offset: 0x0002FCCF
		public InterruptingBehaviorGroup(AgentNavigator navigator, Mission mission)
			: base(navigator, mission)
		{
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00031ADC File Offset: 0x0002FCDC
		public override void Tick(float dt, bool isSimulation)
		{
			if (base.IsActive)
			{
				if (base.ScriptedBehavior != null)
				{
					if (!base.ScriptedBehavior.IsActive)
					{
						base.DisableAllBehaviors();
						base.ScriptedBehavior.IsActive = true;
					}
				}
				else
				{
					int bestBehaviorIndex = this.GetBestBehaviorIndex(isSimulation);
					if (bestBehaviorIndex != -1 && !this.Behaviors[bestBehaviorIndex].IsActive)
					{
						base.DisableAllBehaviors();
						this.Behaviors[bestBehaviorIndex].IsActive = true;
					}
				}
				this.TickActiveBehaviors(dt, isSimulation);
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00031B5C File Offset: 0x0002FD5C
		private void TickActiveBehaviors(float dt, bool isSimulation)
		{
			for (int i = this.Behaviors.Count - 1; i >= 0; i--)
			{
				AgentBehavior agentBehavior = this.Behaviors[i];
				if (agentBehavior.IsActive)
				{
					agentBehavior.Tick(dt, isSimulation);
				}
			}
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00031B9E File Offset: 0x0002FD9E
		public override float GetScore(bool isSimulation)
		{
			if (this.GetBestBehaviorIndex(isSimulation) != -1)
			{
				return 0.75f;
			}
			return 0f;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00031BB8 File Offset: 0x0002FDB8
		private int GetBestBehaviorIndex(bool isSimulation)
		{
			float num = 0f;
			int num2 = -1;
			for (int i = 0; i < this.Behaviors.Count; i++)
			{
				float availability = this.Behaviors[i].GetAvailability(isSimulation);
				if (availability > num)
				{
					num = availability;
					num2 = i;
				}
			}
			return num2;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00031BFF File Offset: 0x0002FDFF
		public override void ForceThink(float inSeconds)
		{
			this.Navigator.RefreshBehaviorGroups(false);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00031C10 File Offset: 0x0002FE10
		public override void ConversationTick()
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				if (agentBehavior.IsActive)
				{
					agentBehavior.ConversationTick();
				}
			}
		}
	}
}
