using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000080 RID: 128
	public class SandboxGeneralsAndCaptainsAssignmentLogic : GeneralsAndCaptainsAssignmentLogic
	{
		// Token: 0x0600052B RID: 1323 RVA: 0x00022D5D File Offset: 0x00020F5D
		public SandboxGeneralsAndCaptainsAssignmentLogic(TextObject attackerGeneralName, TextObject defenderGeneralName, TextObject attackerAllyGeneralName = null, TextObject defenderAllyGeneralName = null, bool createBodyguard = true)
			: base(attackerGeneralName, defenderGeneralName, attackerAllyGeneralName, defenderAllyGeneralName, createBodyguard)
		{
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00022D6C File Offset: 0x00020F6C
		protected override void SortCaptainsByPriority(Team team, ref List<Agent> captains)
		{
			EncounterModel encounterModel = Campaign.Current.Models.EncounterModel;
			if (encounterModel != null)
			{
				captains = captains.OrderByDescending<Agent, float>(delegate(Agent captain)
				{
					if (captain != team.GeneralAgent)
					{
						CharacterObject characterObject;
						return (float)(((characterObject = captain.Character as CharacterObject) != null && characterObject.HeroObject != null) ? encounterModel.GetCharacterSergeantScore(characterObject.HeroObject) : 0);
					}
					return float.MaxValue;
				}).ToList<Agent>();
				return;
			}
			base.SortCaptainsByPriority(team, ref captains);
		}
	}
}
