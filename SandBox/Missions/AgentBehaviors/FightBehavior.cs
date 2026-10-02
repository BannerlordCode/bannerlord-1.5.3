using System;
using SandBox.Missions.MissionLogics;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000AB RID: 171
	public class FightBehavior : AgentBehavior
	{
		// Token: 0x0600072B RID: 1835 RVA: 0x00030413 File Offset: 0x0002E613
		public FightBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			if (base.OwnerAgent.HumanAIComponent == null)
			{
				base.OwnerAgent.AddComponent(new HumanAIComponent(base.OwnerAgent));
			}
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0003043F File Offset: 0x0002E63F
		public override float GetAvailability(bool isSimulation)
		{
			if (!MissionFightHandler.IsAgentAggressive(base.OwnerAgent))
			{
				return 0.1f;
			}
			return 1f;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0003045C File Offset: 0x0002E65C
		protected override void OnActivate()
		{
			TextObject textObject = new TextObject("{=!}{p0} {p1} activate alarmed behavior group.", null);
			textObject.SetTextVariable("p0", base.OwnerAgent.Name.ToString());
			textObject.SetTextVariable("p1", base.OwnerAgent.Index.ToString());
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x000304B0 File Offset: 0x0002E6B0
		protected override void OnDeactivate()
		{
			TextObject textObject = new TextObject("{=!}{p0} {p1} deactivate fight behavior.", null);
			textObject.SetTextVariable("p0", base.OwnerAgent.Name.ToString());
			textObject.SetTextVariable("p1", base.OwnerAgent.Index.ToString());
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00030502 File Offset: 0x0002E702
		public override string GetDebugInfo()
		{
			return "Fight";
		}
	}
}
