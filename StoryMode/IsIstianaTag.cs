using System;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation.Tags;

namespace StoryMode
{
	// Token: 0x02000007 RID: 7
	public class IsIstianaTag : ConversationTag
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002A6A File Offset: 0x00000C6A
		public override string StringId
		{
			get
			{
				return "IsIstianaTag";
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002A71 File Offset: 0x00000C71
		public override bool IsApplicableTo(CharacterObject character)
		{
			return StoryModeHeroes.ImperialMentor.CharacterObject == character;
		}

		// Token: 0x04000012 RID: 18
		public const string Id = "IsIstianaTag";
	}
}
