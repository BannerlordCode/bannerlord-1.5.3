using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000267 RID: 615
	public class PlayerIsMaleTag : ConversationTag
	{
		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06002406 RID: 9222 RVA: 0x0009E6A6 File Offset: 0x0009C8A6
		public override string StringId
		{
			get
			{
				return "PlayerIsMaleTag";
			}
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x0009E6AD File Offset: 0x0009C8AD
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !Hero.MainHero.IsFemale;
		}

		// Token: 0x04000AB5 RID: 2741
		public const string Id = "PlayerIsMaleTag";
	}
}
