using System;
using Helpers;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000256 RID: 598
	public class AlliedLordTag : ConversationTag
	{
		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x060023D3 RID: 9171 RVA: 0x0009E2D5 File Offset: 0x0009C4D5
		public override string StringId
		{
			get
			{
				return "PlayerIsAlliedTag";
			}
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x0009E2DC File Offset: 0x0009C4DC
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && DiplomacyHelper.IsSameFactionAndNotEliminated(character.HeroObject.MapFaction, Hero.MainHero.MapFaction);
		}

		// Token: 0x04000AA4 RID: 2724
		public const string Id = "PlayerIsAlliedTag";
	}
}
