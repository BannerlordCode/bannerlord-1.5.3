using System;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.MountAndBlade;

namespace SandBox
{
	// Token: 0x02000020 RID: 32
	public static class LocationCharacterMissionExtensions
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x0000619A File Offset: 0x0000439A
		public static AgentBuildData GetAgentBuildData(this LocationCharacter locationCharacter)
		{
			return new AgentBuildData(locationCharacter.AgentData);
		}
	}
}
