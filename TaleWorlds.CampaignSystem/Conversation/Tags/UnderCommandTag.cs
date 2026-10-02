using System;
using Helpers;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000270 RID: 624
	public class UnderCommandTag : ConversationTag
	{
		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06002421 RID: 9249 RVA: 0x0009E98A File Offset: 0x0009CB8A
		public override string StringId
		{
			get
			{
				return "UnderCommandTag";
			}
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x0009E991 File Offset: 0x0009CB91
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.Spouse != Hero.MainHero && HeroHelper.UnderPlayerCommand(character.HeroObject);
		}

		// Token: 0x04000ABE RID: 2750
		public const string Id = "UnderCommandTag";
	}
}
