using System;
using System.Linq;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200025F RID: 607
	public class PlayerIsKinTag : ConversationTag
	{
		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x060023EE RID: 9198 RVA: 0x0009E4C3 File Offset: 0x0009C6C3
		public override string StringId
		{
			get
			{
				return "PlayerIsKinTag";
			}
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x0009E4CC File Offset: 0x0009C6CC
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && (character.HeroObject.Siblings.Contains(Hero.MainHero) || character.HeroObject.Mother == Hero.MainHero || character.HeroObject.Father == Hero.MainHero || character.HeroObject.Spouse == Hero.MainHero);
		}

		// Token: 0x04000AAD RID: 2733
		public const string Id = "PlayerIsKinTag";
	}
}
