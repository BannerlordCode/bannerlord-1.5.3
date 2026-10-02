using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200025C RID: 604
	public class PlayerIsMotherTag : ConversationTag
	{
		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x060023E5 RID: 9189 RVA: 0x0009E41E File Offset: 0x0009C61E
		public override string StringId
		{
			get
			{
				return "PlayerIsMotherTag";
			}
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x0009E425 File Offset: 0x0009C625
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.Mother == Hero.MainHero;
		}

		// Token: 0x04000AAA RID: 2730
		public const string Id = "PlayerIsMotherTag";
	}
}
