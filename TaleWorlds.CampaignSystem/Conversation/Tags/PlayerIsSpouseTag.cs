using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000258 RID: 600
	public class PlayerIsSpouseTag : ConversationTag
	{
		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x060023D9 RID: 9177 RVA: 0x0009E32A File Offset: 0x0009C52A
		public override string StringId
		{
			get
			{
				return "PlayerIsSpouseTag";
			}
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x0009E331 File Offset: 0x0009C531
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && Hero.MainHero.Spouse == character.HeroObject;
		}

		// Token: 0x04000AA6 RID: 2726
		public const string Id = "PlayerIsSpouseTag";
	}
}
