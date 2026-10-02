using System;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000D3 RID: 211
	public class ArmyDispersionReasonEnumResolver : IEnumResolver
	{
		// Token: 0x060014E8 RID: 5352 RVA: 0x00062218 File Offset: 0x00060418
		public string ResolveObject(string originalObject)
		{
			if (string.IsNullOrEmpty(originalObject))
			{
				Debug.FailedAssert("ArmyDispersionReason data is null or empty", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\SaveCompability\\ArmyDispersionReasonEnumResolver.cs", "ResolveObject", 16);
				return Army.ArmyDispersionReason.Unknown.ToString();
			}
			if (originalObject.Equals("LowPartySizeRatio"))
			{
				return Army.ArmyDispersionReason.NotEnoughTroop.ToString();
			}
			return originalObject;
		}
	}
}
