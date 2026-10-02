using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200026B RID: 619
	public class NoConflictTag : ConversationTag
	{
		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06002412 RID: 9234 RVA: 0x0009E840 File Offset: 0x0009CA40
		public override string StringId
		{
			get
			{
				return "NoConflictTag";
			}
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x0009E848 File Offset: 0x0009CA48
		public override bool IsApplicableTo(CharacterObject character)
		{
			bool flag = new HostileRelationshipTag().IsApplicableTo(character);
			bool flag2 = new PlayerIsEnemyTag().IsApplicableTo(character);
			return !flag && !flag2;
		}

		// Token: 0x04000AB9 RID: 2745
		public const string Id = "NoConflictTag";
	}
}
