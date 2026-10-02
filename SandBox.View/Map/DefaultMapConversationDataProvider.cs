using System;
using TaleWorlds.Core;

namespace SandBox.View.Map
{
	// Token: 0x0200004B RID: 75
	public class DefaultMapConversationDataProvider : IMapConversationDataProvider
	{
		// Token: 0x0600027D RID: 637 RVA: 0x0001739C File Offset: 0x0001559C
		string IMapConversationDataProvider.GetAtmosphereNameFromData(MapConversationTableauData data)
		{
			string text;
			if (data.TimeOfDay <= 3f || data.TimeOfDay >= 21f)
			{
				text = "night";
			}
			else if (data.TimeOfDay > 8f && data.TimeOfDay < 16f)
			{
				text = "noon";
			}
			else
			{
				text = "sunset";
			}
			if (data.Settlement == null || data.Settlement.IsHideout)
			{
				if (data.IsCurrentTerrainUnderSnow)
				{
					return "conv_snow_" + text + "_0";
				}
				switch (data.ConversationTerrainType)
				{
				case TerrainType.Desert:
					return "conv_desert_" + text + "_0";
				case TerrainType.Forest:
					return "conv_forest_" + text + "_0";
				case TerrainType.Steppe:
					return "conv_steppe_" + text + "_0";
				}
				return "conv_plains_" + text + "_0";
			}
			else
			{
				string stringId = data.Settlement.Culture.StringId;
				bool flag;
				string locationNameFromLocationId = DefaultMapConversationDataProvider.GetLocationNameFromLocationId(data.LocationId, out flag);
				if (locationNameFromLocationId == null)
				{
					return string.Concat(new string[] { "conv_", stringId, "_town_", text, "_0" });
				}
				if (flag)
				{
					return string.Concat(new string[] { "conv_", stringId, "_", locationNameFromLocationId, "_0" });
				}
				return string.Concat(new string[] { "conv_", stringId, "_", locationNameFromLocationId, "_", text, "_0" });
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00017574 File Offset: 0x00015774
		private static string GetLocationNameFromLocationId(string locationId, out bool isLocationInside)
		{
			if (locationId == "tavern")
			{
				isLocationInside = true;
				return "tavern";
			}
			if (locationId == "lordshall")
			{
				isLocationInside = true;
				return "lordshall";
			}
			if (locationId == "port")
			{
				isLocationInside = false;
				return "shipyard";
			}
			isLocationInside = false;
			return null;
		}
	}
}
