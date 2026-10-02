using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200027E RID: 638
	public class HeadmanNotableTypeTag : ConversationTag
	{
		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x0600244B RID: 9291 RVA: 0x0009ED27 File Offset: 0x0009CF27
		public override string StringId
		{
			get
			{
				return "HeadmanNotableTypeTag";
			}
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x0009ED2E File Offset: 0x0009CF2E
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.Occupation == Occupation.Headman;
		}

		// Token: 0x04000ACD RID: 2765
		public const string Id = "HeadmanNotableTypeTag";
	}
}
