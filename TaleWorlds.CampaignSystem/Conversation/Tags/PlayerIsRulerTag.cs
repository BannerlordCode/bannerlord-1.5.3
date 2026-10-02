using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000263 RID: 611
	public class PlayerIsRulerTag : ConversationTag
	{
		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x060023FA RID: 9210 RVA: 0x0009E633 File Offset: 0x0009C833
		public override string StringId
		{
			get
			{
				return "PlayerIsRulerTag";
			}
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x0009E63A File Offset: 0x0009C83A
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Hero.MainHero.Clan.Leader == Hero.MainHero;
		}

		// Token: 0x04000AB1 RID: 2737
		public const string Id = "PlayerIsRulerTag";
	}
}
