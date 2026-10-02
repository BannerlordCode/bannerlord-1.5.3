using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000259 RID: 601
	public class PlayerIsDaughterTag : ConversationTag
	{
		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x060023DC RID: 9180 RVA: 0x0009E357 File Offset: 0x0009C557
		public override string StringId
		{
			get
			{
				return "PlayerIsDaughterTag";
			}
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x0009E35E File Offset: 0x0009C55E
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && Hero.MainHero.IsFemale && (Hero.MainHero.Father == character.HeroObject || Hero.MainHero.Mother == character.HeroObject);
		}

		// Token: 0x04000AA7 RID: 2727
		public const string Id = "PlayerIsDaughterTag";
	}
}
