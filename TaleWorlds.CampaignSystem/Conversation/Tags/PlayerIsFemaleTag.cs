using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000266 RID: 614
	public class PlayerIsFemaleTag : ConversationTag
	{
		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06002403 RID: 9219 RVA: 0x0009E68B File Offset: 0x0009C88B
		public override string StringId
		{
			get
			{
				return "PlayerIsFemaleTag";
			}
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x0009E692 File Offset: 0x0009C892
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Hero.MainHero.IsFemale;
		}

		// Token: 0x04000AB4 RID: 2740
		public const string Id = "PlayerIsFemaleTag";
	}
}
