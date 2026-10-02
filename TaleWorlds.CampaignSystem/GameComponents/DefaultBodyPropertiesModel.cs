using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FE RID: 254
	public class DefaultBodyPropertiesModel : BodyPropertiesModel
	{
		// Token: 0x06001719 RID: 5913 RVA: 0x0006BC2E File Offset: 0x00069E2E
		public override int[] GetHairIndicesForCulture(int race, int gender, float age, CultureObject culture)
		{
			return FaceGen.GetHairIndicesByTag(race, gender, age, culture.StringId);
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x0006BC3F File Offset: 0x00069E3F
		public override int[] GetBeardIndicesForCulture(int race, int gender, float age, CultureObject culture)
		{
			return FaceGen.GetFacialIndicesByTag(race, gender, age, culture.StringId);
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x0006BC50 File Offset: 0x00069E50
		public override int[] GetTattooIndicesForCulture(int race, int gender, float age, CultureObject culture)
		{
			return FaceGen.GetTattooIndicesByTag(race, gender, age, culture.StringId);
		}
	}
}
