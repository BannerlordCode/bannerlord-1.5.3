using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200025B RID: 603
	public class PlayerIsFatherTag : ConversationTag
	{
		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x060023E2 RID: 9186 RVA: 0x0009E3F1 File Offset: 0x0009C5F1
		public override string StringId
		{
			get
			{
				return "PlayerIsFatherTag";
			}
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x0009E3F8 File Offset: 0x0009C5F8
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.Father == Hero.MainHero;
		}

		// Token: 0x04000AA9 RID: 2729
		public const string Id = "PlayerIsFatherTag";
	}
}
