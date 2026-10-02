using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000D4 RID: 212
	public class BattleTypeEnumResolver : IEnumResolver
	{
		// Token: 0x060014EA RID: 5354 RVA: 0x0006227C File Offset: 0x0006047C
		public string ResolveObject(string originalObject)
		{
			if (string.IsNullOrEmpty(originalObject))
			{
				Debug.FailedAssert("EndCaptivityDetail data is null or empty", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\SaveCompability\\BattleTypeEnumResolver.cs", "ResolveObject", 16);
				return MapEvent.BattleTypes.None.ToString();
			}
			if (originalObject.Equals("AlleyFight"))
			{
				return MapEvent.BattleTypes.None.ToString();
			}
			return originalObject;
		}
	}
}
