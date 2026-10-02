using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000279 RID: 633
	public class OnTheRoadTag : ConversationTag
	{
		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x0600243C RID: 9276 RVA: 0x0009EBF5 File Offset: 0x0009CDF5
		public override string StringId
		{
			get
			{
				return "OnTheRoadTag";
			}
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x0009EBFC File Offset: 0x0009CDFC
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Settlement.CurrentSettlement == null;
		}

		// Token: 0x04000AC8 RID: 2760
		public const string Id = "OnTheRoadTag";
	}
}
