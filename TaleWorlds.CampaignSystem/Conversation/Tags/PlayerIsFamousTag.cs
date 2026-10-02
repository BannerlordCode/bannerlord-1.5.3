using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000260 RID: 608
	public class PlayerIsFamousTag : ConversationTag
	{
		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x060023F1 RID: 9201 RVA: 0x0009E53A File Offset: 0x0009C73A
		public override string StringId
		{
			get
			{
				return "PlayerIsFamousTag";
			}
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x0009E541 File Offset: 0x0009C741
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Clan.PlayerClan.Renown >= 50f;
		}

		// Token: 0x04000AAE RID: 2734
		public const string Id = "PlayerIsFamousTag";
	}
}
