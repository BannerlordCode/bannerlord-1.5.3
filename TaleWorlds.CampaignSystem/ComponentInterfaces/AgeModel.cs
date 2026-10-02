using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E7 RID: 487
	public abstract class AgeModel : MBGameModel<AgeModel>
	{
		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06001F54 RID: 8020
		public abstract int BecomeInfantAge { get; }

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06001F55 RID: 8021
		public abstract int BecomeChildAge { get; }

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06001F56 RID: 8022
		public abstract int BecomeTeenagerAge { get; }

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06001F57 RID: 8023
		public abstract int HeroComesOfAge { get; }

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06001F58 RID: 8024
		public abstract int BecomeOldAge { get; }

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06001F59 RID: 8025
		public abstract int MiddleAdultHoodAge { get; }

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06001F5A RID: 8026
		public abstract int MaxAge { get; }

		// Token: 0x06001F5B RID: 8027
		public abstract void GetAgeLimitForLocation(CharacterObject character, out int minimumAge, out int maximumAge, string additionalTags = "");
	}
}
