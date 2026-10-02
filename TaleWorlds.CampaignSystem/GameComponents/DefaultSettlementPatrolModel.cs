using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000156 RID: 342
	public class DefaultSettlementPatrolModel : SettlementPatrolModel
	{
		// Token: 0x06001AB5 RID: 6837 RVA: 0x000874D4 File Offset: 0x000856D4
		public override CampaignTime GetPatrolPartySpawnDuration(Settlement settlement, bool naval)
		{
			Building guardHouse = this.GetGuardHouse(settlement);
			return CampaignTime.Days(10f - ((float)guardHouse.CurrentLevel - 1f) * 2f);
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x00087507 File Offset: 0x00085707
		public override bool CanSettlementHavePatrolParties(Settlement settlement, bool naval)
		{
			return settlement.OwnerClan != null && !settlement.OwnerClan.IsRebelClan && settlement.IsTown && this.HasGuardHouse(settlement);
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x0008752F File Offset: 0x0008572F
		private bool HasGuardHouse(Settlement settlement)
		{
			return this.GetGuardHouse(settlement) != null;
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x0008753C File Offset: 0x0008573C
		private Building GetGuardHouse(Settlement settlement)
		{
			if (settlement.Town != null)
			{
				foreach (Building building in settlement.Town.Buildings)
				{
					if (building.BuildingType == DefaultBuildingTypes.SettlementGuardHouse && building.CurrentLevel > 0)
					{
						return building;
					}
				}
			}
			return null;
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x000875B4 File Offset: 0x000857B4
		public override PartyTemplateObject GetPartyTemplateForPatrolParty(Settlement settlement, bool naval)
		{
			Building guardHouse = this.GetGuardHouse(settlement);
			if (guardHouse == null)
			{
				return null;
			}
			switch ((int)Campaign.Current.Models.BuildingEffectModel.GetBuildingEffect(guardHouse, BuildingEffectEnum.PatrolPartyStrength).ResultNumber)
			{
			case 1:
				return settlement.OwnerClan.Culture.SettlementPatrolPartyTemplateWeak;
			case 2:
				return settlement.OwnerClan.Culture.SettlementPatrolPartyTemplateModerate;
			case 3:
				return settlement.OwnerClan.Culture.SettlementPatrolPartyTemplateStrong;
			default:
				return settlement.OwnerClan.Culture.SettlementPatrolPartyTemplateWeak;
			}
		}
	}
}
