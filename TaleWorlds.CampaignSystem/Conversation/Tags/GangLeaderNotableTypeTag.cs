using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200027F RID: 639
	public class GangLeaderNotableTypeTag : ConversationTag
	{
		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x0600244E RID: 9294 RVA: 0x0009ED4C File Offset: 0x0009CF4C
		public override string StringId
		{
			get
			{
				return "GangLeaderNotableTypeTag";
			}
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x0009ED53 File Offset: 0x0009CF53
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.Occupation == Occupation.GangLeader;
		}

		// Token: 0x04000ACE RID: 2766
		public const string Id = "GangLeaderNotableTypeTag";
	}
}
