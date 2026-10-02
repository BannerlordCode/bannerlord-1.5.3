using System;
using System.Linq;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200025E RID: 606
	public class PlayerIsSisterTag : ConversationTag
	{
		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x060023EB RID: 9195 RVA: 0x0009E487 File Offset: 0x0009C687
		public override string StringId
		{
			get
			{
				return "PlayerIsSisterTag";
			}
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x0009E48E File Offset: 0x0009C68E
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Hero.MainHero.IsFemale && character.IsHero && character.HeroObject.Siblings.Contains(Hero.MainHero);
		}

		// Token: 0x04000AAC RID: 2732
		public const string Id = "PlayerIsSisterTag";
	}
}
