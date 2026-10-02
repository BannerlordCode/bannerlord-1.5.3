using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000A9 RID: 169
	public class DailyBehaviorGroup : AgentBehaviorGroup
	{
		// Token: 0x06000710 RID: 1808 RVA: 0x0002F77C File Offset: 0x0002D97C
		public DailyBehaviorGroup(AgentNavigator navigator, Mission mission)
			: base(navigator, mission)
		{
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0002F788 File Offset: 0x0002D988
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
				else if (this.CheckBehaviorTimer == null || this.CheckBehaviorTimer.Check(base.Mission.CurrentTime))
				{
					this.Think(isSimulation);
				}
				this.TickActiveBehaviors(dt, isSimulation);
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0002F7F8 File Offset: 0x0002D9F8
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

		// Token: 0x06000713 RID: 1811 RVA: 0x0002F854 File Offset: 0x0002DA54
		private void Think(bool isSimulation)
		{
			float num = 0f;
			float[] array = new float[this.Behaviors.Count];
			for (int i = 0; i < this.Behaviors.Count; i++)
			{
				array[i] = this.Behaviors[i].GetAvailability(isSimulation);
				num += array[i];
			}
			if (num > 0f)
			{
				float num2 = MBRandom.RandomFloat * num;
				int j = 0;
				while (j < array.Length)
				{
					float num3 = array[j];
					num2 -= num3;
					if (num2 < 0f)
					{
						if (!this.Behaviors[j].IsActive)
						{
							base.DisableAllBehaviors();
							this.Behaviors[j].IsActive = true;
							this.CheckBehaviorTime = this.Behaviors[j].CheckTime;
							this.SetCheckBehaviorTimer(this.CheckBehaviorTime);
							return;
						}
						break;
					}
					else
					{
						j++;
					}
				}
			}
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0002F930 File Offset: 0x0002DB30
		private void TickActiveBehaviors(float dt, bool isSimulation)
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				if (agentBehavior.IsActive)
				{
					agentBehavior.Tick(dt, isSimulation);
				}
			}
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0002F98C File Offset: 0x0002DB8C
		private void SetCheckBehaviorTimer(float time)
		{
			if (this.CheckBehaviorTimer == null)
			{
				this.CheckBehaviorTimer = new Timer(base.Mission.CurrentTime, time, true);
				return;
			}
			this.CheckBehaviorTimer.Reset(base.Mission.CurrentTime, time);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0002F9C6 File Offset: 0x0002DBC6
		public override float GetScore(bool isSimulation)
		{
			return 0.5f;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0002F9D0 File Offset: 0x0002DBD0
		public override void OnAgentRemoved(Agent agent)
		{
			if (base.IsActive)
			{
				foreach (AgentBehavior agentBehavior in this.Behaviors)
				{
					if (agentBehavior.IsActive)
					{
						agentBehavior.OnAgentRemoved(agent);
					}
				}
			}
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0002FA34 File Offset: 0x0002DC34
		protected override void OnActivate()
		{
			if (CampaignMission.Current.Location != null)
			{
				LocationCharacter locationCharacter = CampaignMission.Current.Location.GetLocationCharacter(base.OwnerAgent.Origin);
				if (locationCharacter != null && locationCharacter.ActionSetCode != locationCharacter.AlarmedActionSetCode)
				{
					AnimationSystemData animationSystemData = locationCharacter.GetAgentBuildData().AgentMonster.FillAnimationSystemData(MBGlobals.GetActionSet(locationCharacter.ActionSetCode), locationCharacter.Character.GetStepSize(), false);
					base.OwnerAgent.SetActionSet(ref animationSystemData);
				}
			}
			this.Navigator.SetItemsVisibility(true);
			this.Navigator.SetSpecialItem();
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0002FACA File Offset: 0x0002DCCA
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this.CheckBehaviorTimer = null;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0002FAD9 File Offset: 0x0002DCD9
		public override void ForceThink(float inSeconds)
		{
			if (MathF.Abs(inSeconds) < 1E-45f)
			{
				this.Think(false);
				return;
			}
			this.SetCheckBehaviorTimer(inSeconds);
		}
	}
}
