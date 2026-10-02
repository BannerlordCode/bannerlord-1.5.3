using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200013D RID: 317
	public class DefaultPartyNavigationModel : PartyNavigationModel
	{
		// Token: 0x060019D1 RID: 6609 RVA: 0x00080D54 File Offset: 0x0007EF54
		public override float GetEmbarkDisembarkThresholdDistance()
		{
			return 0f;
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x00080D5B File Offset: 0x0007EF5B
		private static bool IsTerrainTypeValidForDefault(TerrainType t)
		{
			return t == TerrainType.Plain || t == TerrainType.Desert || t == TerrainType.Snow || t == TerrainType.Forest || t == TerrainType.Steppe || t == TerrainType.Swamp || t == TerrainType.Dune || t == TerrainType.Bridge || t == TerrainType.Fording || t == TerrainType.Beach;
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x00080D8C File Offset: 0x0007EF8C
		public DefaultPartyNavigationModel()
		{
			List<int> list = new List<int>();
			foreach (object obj in Enum.GetValues(typeof(TerrainType)))
			{
				TerrainType terrainType = (TerrainType)obj;
				if (!DefaultPartyNavigationModel.IsTerrainTypeValidForDefault(terrainType))
				{
					list.Add((int)terrainType);
				}
			}
			this._invalidTerrainTypes = list.ToArray();
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x00080E10 File Offset: 0x0007F010
		public override int[] GetInvalidTerrainTypesForNavigationType(MobileParty.NavigationType navigationType)
		{
			if (navigationType == MobileParty.NavigationType.Default || navigationType == MobileParty.NavigationType.All)
			{
				return this._invalidTerrainTypes;
			}
			return new int[0];
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x00080E27 File Offset: 0x0007F027
		public override bool IsTerrainTypeValidForNavigationType(TerrainType terrainType, MobileParty.NavigationType navigationType)
		{
			return (navigationType == MobileParty.NavigationType.Default || navigationType == MobileParty.NavigationType.All) && DefaultPartyNavigationModel.IsTerrainTypeValidForDefault(terrainType);
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x00080E39 File Offset: 0x0007F039
		public override bool HasNavalNavigationCapability(MobileParty mobileParty)
		{
			return false;
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x00080E3C File Offset: 0x0007F03C
		public override bool CanPlayerNavigateToPosition(CampaignVec2 vec2, out MobileParty.NavigationType navigationType)
		{
			navigationType = MobileParty.NavigationType.Default;
			return vec2.Face.IsValid() && MobileParty.MainParty.Position.IsOnLand && vec2.IsOnLand && !Campaign.Current.Models.PartyNavigationModel.GetInvalidTerrainTypesForNavigationType(navigationType).Contains(vec2.Face.FaceGroupIndex);
		}

		// Token: 0x0400086A RID: 2154
		private int[] _invalidTerrainTypes;
	}
}
