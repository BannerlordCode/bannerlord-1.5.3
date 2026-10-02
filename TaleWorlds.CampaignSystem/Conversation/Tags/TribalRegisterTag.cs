using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000254 RID: 596
	public class TribalRegisterTag : ConversationTag
	{
		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x060023CD RID: 9165 RVA: 0x0009E27C File Offset: 0x0009C47C
		public override string StringId
		{
			get
			{
				return "TribalRegisterTag";
			}
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x0009E283 File Offset: 0x0009C483
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !ConversationTagHelper.UsesHighRegister(character) && !ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x04000AA2 RID: 2722
		public const string Id = "TribalRegisterTag";
	}
}
