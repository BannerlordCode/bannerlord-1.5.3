using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200024A RID: 586
	public abstract class ConversationTag
	{
		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x060023AE RID: 9134
		public abstract string StringId { get; }

		// Token: 0x060023AF RID: 9135
		public abstract bool IsApplicableTo(CharacterObject character);

		// Token: 0x060023B0 RID: 9136 RVA: 0x0009E044 File Offset: 0x0009C244
		public override string ToString()
		{
			return this.StringId;
		}
	}
}
