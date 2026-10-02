using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000282 RID: 642
	public class AnyNotableTypeTag : ConversationTag
	{
		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x0009EDBB File Offset: 0x0009CFBB
		public override string StringId
		{
			get
			{
				return "AnyNotableTypeTag";
			}
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x0009EDC2 File Offset: 0x0009CFC2
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsNotable;
		}

		// Token: 0x04000AD1 RID: 2769
		public const string Id = "AnyNotableTypeTag";
	}
}
