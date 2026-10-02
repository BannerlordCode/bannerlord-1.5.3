using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000255 RID: 597
	public class PlayerIsEnemyTag : ConversationTag
	{
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x060023D0 RID: 9168 RVA: 0x0009E2A0 File Offset: 0x0009C4A0
		public override string StringId
		{
			get
			{
				return "PlayerIsEnemyTag";
			}
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x0009E2A7 File Offset: 0x0009C4A7
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && FactionManager.IsAtWarAgainstFaction(character.HeroObject.MapFaction, Hero.MainHero.MapFaction);
		}

		// Token: 0x04000AA3 RID: 2723
		public const string Id = "PlayerIsEnemyTag";
	}
}
