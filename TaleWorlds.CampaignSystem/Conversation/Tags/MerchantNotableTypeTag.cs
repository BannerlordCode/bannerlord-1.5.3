using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000280 RID: 640
	public class MerchantNotableTypeTag : ConversationTag
	{
		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06002451 RID: 9297 RVA: 0x0009ED71 File Offset: 0x0009CF71
		public override string StringId
		{
			get
			{
				return "MerchantNotableTypeTag";
			}
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x0009ED78 File Offset: 0x0009CF78
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.Occupation == Occupation.Merchant;
		}

		// Token: 0x04000ACF RID: 2767
		public const string Id = "MerchantNotableTypeTag";
	}
}
