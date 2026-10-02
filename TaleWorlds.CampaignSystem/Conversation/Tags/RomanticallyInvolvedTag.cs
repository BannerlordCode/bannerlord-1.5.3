using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000274 RID: 628
	public class RomanticallyInvolvedTag : ConversationTag
	{
		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x0009EA97 File Offset: 0x0009CC97
		public override string StringId
		{
			get
			{
				return "RomanticallyInvolvedTag";
			}
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0009EA9E File Offset: 0x0009CC9E
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && Romance.GetRomanticLevel(character.HeroObject, CharacterObject.PlayerCharacter.HeroObject) >= Romance.RomanceLevelEnum.CourtshipStarted;
		}

		// Token: 0x04000AC2 RID: 2754
		public const string Id = "RomanticallyInvolvedTag";
	}
}
