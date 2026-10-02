using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000420 RID: 1056
	public interface IFacegenCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x06004341 RID: 17217
		IFaceGeneratorCustomFilter GetFaceGenFilter();
	}
}
