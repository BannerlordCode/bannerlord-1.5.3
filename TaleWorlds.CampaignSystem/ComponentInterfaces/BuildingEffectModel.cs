using System;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E2 RID: 482
	public abstract class BuildingEffectModel : MBGameModel<BuildingEffectModel>
	{
		// Token: 0x06001F40 RID: 8000
		public abstract ExplainedNumber GetBuildingEffect(Building building, BuildingEffectEnum effect);
	}
}
