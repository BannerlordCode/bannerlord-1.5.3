using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000206 RID: 518
	public abstract class BuildingModel : MBGameModel<BuildingModel>
	{
		// Token: 0x0600203C RID: 8252
		public abstract bool CanAddBuildingTypeToTown(BuildingType buildingType, Town town);
	}
}
