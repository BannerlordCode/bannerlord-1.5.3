using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000276 RID: 630
	public class EngagedToPlayerTag : ConversationTag
	{
		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06002433 RID: 9267 RVA: 0x0009EB51 File Offset: 0x0009CD51
		public override string StringId
		{
			get
			{
				return "EngagedToPlayerTag";
			}
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x0009EB58 File Offset: 0x0009CD58
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && Romance.GetRomanticLevel(character.HeroObject, Hero.MainHero) == Romance.RomanceLevelEnum.CoupleAgreedOnMarriage;
		}

		// Token: 0x04000AC5 RID: 2757
		public const string Id = "EngagedToPlayerTag";
	}
}
