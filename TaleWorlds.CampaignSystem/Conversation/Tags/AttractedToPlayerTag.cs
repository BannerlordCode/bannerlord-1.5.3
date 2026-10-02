using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000275 RID: 629
	public class AttractedToPlayerTag : ConversationTag
	{
		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06002430 RID: 9264 RVA: 0x0009EACD File Offset: 0x0009CCCD
		public override string StringId
		{
			get
			{
				return "AttractedToPlayerTag";
			}
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x0009EAD4 File Offset: 0x0009CCD4
		public override bool IsApplicableTo(CharacterObject character)
		{
			Hero heroObject = character.HeroObject;
			return heroObject != null && Hero.MainHero.IsFemale != heroObject.IsFemale && !FactionManager.IsAtWarAgainstFaction(heroObject.MapFaction, Hero.MainHero.MapFaction) && Campaign.Current.Models.RomanceModel.GetAttractionValuePercentage(heroObject, Hero.MainHero) > 70 && heroObject.Spouse == null && Hero.MainHero.Spouse == null;
		}

		// Token: 0x04000AC3 RID: 2755
		public const string Id = "AttractedToPlayerTag";

		// Token: 0x04000AC4 RID: 2756
		private const int MinimumFlirtPercentageForComment = 70;
	}
}
