using System;
using System.Linq;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200025D RID: 605
	public class PlayerIsBrotherTag : ConversationTag
	{
		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x060023E8 RID: 9192 RVA: 0x0009E44B File Offset: 0x0009C64B
		public override string StringId
		{
			get
			{
				return "PlayerIsBrotherTag";
			}
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x0009E452 File Offset: 0x0009C652
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !Hero.MainHero.IsFemale && character.IsHero && character.HeroObject.Siblings.Contains(Hero.MainHero);
		}

		// Token: 0x04000AAB RID: 2731
		public const string Id = "PlayerIsBrotherTag";
	}
}
