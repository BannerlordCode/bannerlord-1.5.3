using System;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation.Tags;

namespace StoryMode
{
	// Token: 0x02000008 RID: 8
	public class IsArzagosTag : ConversationTag
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002A88 File Offset: 0x00000C88
		public override string StringId
		{
			get
			{
				return "IsArzagosTag";
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002A8F File Offset: 0x00000C8F
		public override bool IsApplicableTo(CharacterObject character)
		{
			return StoryModeHeroes.AntiImperialMentor.CharacterObject == character;
		}

		// Token: 0x04000013 RID: 19
		public const string Id = "IsArzagosTag";
	}
}
