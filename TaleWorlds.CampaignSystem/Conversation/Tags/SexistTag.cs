using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200026F RID: 623
	public class SexistTag : ConversationTag
	{
		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x0009E8FD File Offset: 0x0009CAFD
		public override string StringId
		{
			get
			{
				return "SexistTag";
			}
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x0009E904 File Offset: 0x0009CB04
		public override bool IsApplicableTo(CharacterObject character)
		{
			bool flag = character.HeroObject.Clan.Heroes.Any<Hero>((Hero x) => x.IsFemale && x.IsCommander);
			int num = character.GetTraitLevel(DefaultTraits.Calculating) + character.GetTraitLevel(DefaultTraits.Mercy);
			int num2 = character.GetTraitLevel(DefaultTraits.Valor) + character.GetTraitLevel(DefaultTraits.Generosity);
			return num < 0 && num2 <= 0 && !flag;
		}

		// Token: 0x04000ABD RID: 2749
		public const string Id = "SexistTag";
	}
}
