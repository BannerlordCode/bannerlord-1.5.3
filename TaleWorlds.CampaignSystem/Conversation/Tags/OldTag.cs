using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000277 RID: 631
	public class OldTag : ConversationTag
	{
		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06002436 RID: 9270 RVA: 0x0009EB7F File Offset: 0x0009CD7F
		public override string StringId
		{
			get
			{
				return "OldTag";
			}
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x0009EB86 File Offset: 0x0009CD86
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Age > (float)Campaign.Current.Models.AgeModel.BecomeOldAge;
		}

		// Token: 0x04000AC6 RID: 2758
		public const string Id = "OldTag";
	}
}
