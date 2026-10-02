using System;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation.Tags;

namespace StoryMode
{
	// Token: 0x02000009 RID: 9
	public class IsStoryModeMentorTag : ConversationTag
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002AA6 File Offset: 0x00000CA6
		public override string StringId
		{
			get
			{
				return "IsStoryModeMentorTag";
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002AAD File Offset: 0x00000CAD
		public override bool IsApplicableTo(CharacterObject character)
		{
			return StoryModeHeroes.AntiImperialMentor.CharacterObject == character || StoryModeHeroes.ImperialMentor.CharacterObject == character;
		}

		// Token: 0x04000014 RID: 20
		public const string Id = "IsStoryModeMentorTag";
	}
}
