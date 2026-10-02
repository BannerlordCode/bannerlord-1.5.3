using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000253 RID: 595
	public class LowRegisterTag : ConversationTag
	{
		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x060023CA RID: 9162 RVA: 0x0009E253 File Offset: 0x0009C453
		public override string StringId
		{
			get
			{
				return "LowRegisterTag";
			}
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x0009E25A File Offset: 0x0009C45A
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && !ConversationTagHelper.UsesHighRegister(character) && ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x04000AA1 RID: 2721
		public const string Id = "LowRegisterTag";
	}
}
