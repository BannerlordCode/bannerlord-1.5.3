using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000286 RID: 646
	public class EmpireTag : ConversationTag
	{
		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06002463 RID: 9315 RVA: 0x0009EEAB File Offset: 0x0009D0AB
		public override string StringId
		{
			get
			{
				return "EmpireTag";
			}
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x0009EEB2 File Offset: 0x0009D0B2
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "empire";
		}

		// Token: 0x04000AD5 RID: 2773
		public const string Id = "EmpireTag";
	}
}
