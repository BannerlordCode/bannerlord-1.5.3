using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000283 RID: 643
	public class WandererTag : ConversationTag
	{
		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x0600245A RID: 9306 RVA: 0x0009EDE1 File Offset: 0x0009CFE1
		public override string StringId
		{
			get
			{
				return "WandererTag";
			}
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x0009EDE8 File Offset: 0x0009CFE8
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsWanderer;
		}

		// Token: 0x04000AD2 RID: 2770
		public const string Id = "WandererTag";
	}
}
