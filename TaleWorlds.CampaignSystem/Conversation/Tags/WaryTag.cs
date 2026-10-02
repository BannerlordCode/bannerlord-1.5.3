using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200024F RID: 591
	public class WaryTag : ConversationTag
	{
		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x060023BE RID: 9150 RVA: 0x0009E0DB File Offset: 0x0009C2DB
		public override string StringId
		{
			get
			{
				return "WaryTag";
			}
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x0009E0E4 File Offset: 0x0009C2E4
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.MapFaction != Hero.MainHero.MapFaction && (Settlement.CurrentSettlement == null || Settlement.CurrentSettlement.SiegeEvent != null) && (Campaign.Current.ConversationManager.CurrentConversationIsFirst || FactionManager.IsAtWarAgainstFaction(character.HeroObject.MapFaction, Hero.MainHero.MapFaction));
		}

		// Token: 0x04000A9D RID: 2717
		public const string Id = "WaryTag";
	}
}
