using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x020003A7 RID: 935
	public class AnimalSpawnSettings : ScriptComponentBehavior
	{
		// Token: 0x0600359B RID: 13723 RVA: 0x000DD7FD File Offset: 0x000DB9FD
		public static void CheckAndSetAnimalAgentFlags(GameEntity spawnEntity, Agent animalAgent)
		{
			if (spawnEntity.HasScriptOfType<AnimalSpawnSettings>() && spawnEntity.GetFirstScriptOfType<AnimalSpawnSettings>().DisableWandering)
			{
				animalAgent.SetAgentFlags(animalAgent.GetAgentFlags() & ~AgentFlag.CanWander);
			}
		}

		// Token: 0x040016D9 RID: 5849
		public bool DisableWandering = true;
	}
}
