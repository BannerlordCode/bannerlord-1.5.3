using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001FD RID: 509
	public class CustomBattleInitializationModel : BattleInitializationModel
	{
		// Token: 0x06001E04 RID: 7684 RVA: 0x000663F8 File Offset: 0x000645F8
		public override List<FormationClass> GetAllAvailableTroopTypes()
		{
			List<FormationClass> list = new List<FormationClass>();
			foreach (Agent agent in Mission.Current.PlayerTeam.ActiveAgents)
			{
				BasicCharacterObject character = agent.Character;
				if (character.IsInfantry && !character.IsMounted && !list.Contains(FormationClass.Infantry))
				{
					list.Add(FormationClass.Infantry);
				}
				if (character.IsRanged && !character.IsMounted && !list.Contains(FormationClass.Ranged))
				{
					list.Add(FormationClass.Ranged);
				}
				if (character.IsMounted && !character.IsRanged && !list.Contains(FormationClass.Cavalry))
				{
					list.Add(FormationClass.Cavalry);
				}
				if (character.IsMounted && character.IsRanged && !list.Contains(FormationClass.HorseArcher))
				{
					list.Add(FormationClass.HorseArcher);
				}
			}
			return list;
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x000664E0 File Offset: 0x000646E0
		protected override bool CanPlayerSideDeployWithOrderOfBattleAux()
		{
			if (Mission.Current.IsSallyOutBattle)
			{
				return false;
			}
			Team playerTeam = Mission.Current.PlayerTeam;
			return Mission.Current.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>().GetNumberOfPlayerControllableTroops() >= 20;
		}
	}
}
