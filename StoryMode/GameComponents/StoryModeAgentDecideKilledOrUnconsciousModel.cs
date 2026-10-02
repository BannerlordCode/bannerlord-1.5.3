using System;
using StoryMode.StoryModeObjects;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace StoryMode.GameComponents
{
	// Token: 0x0200003B RID: 59
	public class StoryModeAgentDecideKilledOrUnconsciousModel : AgentDecideKilledOrUnconsciousModel
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x00018CF4 File Offset: 0x00016EF4
		public override float GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out float useSurgeryProbability)
		{
			useSurgeryProbability = 1f;
			if (effectedAgent.Character.IsHero && (effectedAgent.Character == StoryModeHeroes.ElderBrother.CharacterObject || effectedAgent.Character == StoryModeHeroes.Radagos.CharacterObject || effectedAgent.Character == StoryModeHeroes.RadagosHenchman.CharacterObject) && !StoryModeManager.Current.MainStoryLine.IsCompleted)
			{
				return 0f;
			}
			if (!StoryModeManager.Current.MainStoryLine.TutorialPhase.IsCompleted && Mission.Current.GetMemberCountOfSide(effectedAgent.Team.Side) > 4)
			{
				return 0f;
			}
			return base.BaseModel.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, weaponFlags, out useSurgeryProbability);
		}
	}
}
