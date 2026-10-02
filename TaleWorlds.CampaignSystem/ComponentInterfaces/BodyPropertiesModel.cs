using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000208 RID: 520
	public abstract class BodyPropertiesModel : MBGameModel<BodyPropertiesModel>
	{
		// Token: 0x06002041 RID: 8257
		public abstract int[] GetHairIndicesForCulture(int race, int gender, float age, CultureObject culture);

		// Token: 0x06002042 RID: 8258
		public abstract int[] GetBeardIndicesForCulture(int race, int gender, float age, CultureObject culture);

		// Token: 0x06002043 RID: 8259
		public abstract int[] GetTattooIndicesForCulture(int race, int gender, float age, CultureObject culture);
	}
}
