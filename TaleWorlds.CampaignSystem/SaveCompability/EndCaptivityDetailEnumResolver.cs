using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000D8 RID: 216
	public class EndCaptivityDetailEnumResolver : IEnumResolver
	{
		// Token: 0x060014FB RID: 5371 RVA: 0x00062418 File Offset: 0x00060618
		public string ResolveObject(string originalObject)
		{
			if (string.IsNullOrEmpty(originalObject))
			{
				Debug.FailedAssert("EndCaptivityDetail data is null or empty", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\SaveCompability\\EndCaptivityDetailEnumResolver.cs", "ResolveObject", 15);
				return EndCaptivityDetail.ReleasedByChoice.ToString();
			}
			if (originalObject.Equals("EscapeFromLootedParty"))
			{
				return EndCaptivityDetail.ReleasedAfterEscape.ToString();
			}
			if (originalObject.Equals("ReleasedFromPartyScreen"))
			{
				return EndCaptivityDetail.ReleasedByChoice.ToString();
			}
			if (originalObject.Equals("RemovedParty"))
			{
				return EndCaptivityDetail.ReleasedByChoice.ToString();
			}
			return originalObject;
		}
	}
}
