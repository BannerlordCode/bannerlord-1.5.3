using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000252 RID: 594
	public class HighRegisterTag : ConversationTag
	{
		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060023C7 RID: 9159 RVA: 0x0009E232 File Offset: 0x0009C432
		public override string StringId
		{
			get
			{
				return "HighRegisterTag";
			}
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x0009E239 File Offset: 0x0009C439
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && ConversationTagHelper.UsesHighRegister(character);
		}

		// Token: 0x04000AA0 RID: 2720
		public const string Id = "HighRegisterTag";
	}
}
