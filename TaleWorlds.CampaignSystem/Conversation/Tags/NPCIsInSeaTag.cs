using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000298 RID: 664
	public class NPCIsInSeaTag : ConversationTag
	{
		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06002499 RID: 9369 RVA: 0x0009F203 File Offset: 0x0009D403
		public override string StringId
		{
			get
			{
				return "NPCIsInSeaTag";
			}
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x0009F20C File Offset: 0x0009D40C
		public override bool IsApplicableTo(CharacterObject character)
		{
			bool flag = false;
			if (character.IsHero)
			{
				flag = (character.HeroObject.IsPrisoner ? character.HeroObject.PartyBelongedToAsPrisoner.MobileParty : character.HeroObject.PartyBelongedTo).IsCurrentlyAtSea;
			}
			return flag;
		}

		// Token: 0x04000AE7 RID: 2791
		public const string Id = "NPCIsInSeaTag";
	}
}
