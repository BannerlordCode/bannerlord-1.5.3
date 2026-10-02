using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200025A RID: 602
	public class PlayerIsSonTag : ConversationTag
	{
		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x060023DF RID: 9183 RVA: 0x0009E3A4 File Offset: 0x0009C5A4
		public override string StringId
		{
			get
			{
				return "PlayerIsSonTag";
			}
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x0009E3AB File Offset: 0x0009C5AB
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && !Hero.MainHero.IsFemale && (Hero.MainHero.Father == character.HeroObject || Hero.MainHero.Mother == character.HeroObject);
		}

		// Token: 0x04000AA8 RID: 2728
		public const string Id = "PlayerIsSonTag";
	}
}
