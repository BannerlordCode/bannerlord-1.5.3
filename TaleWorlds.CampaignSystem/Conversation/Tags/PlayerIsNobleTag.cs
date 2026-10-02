using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000262 RID: 610
	public class PlayerIsNobleTag : ConversationTag
	{
		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x060023F7 RID: 9207 RVA: 0x0009E5F9 File Offset: 0x0009C7F9
		public override string StringId
		{
			get
			{
				return "PlayerIsNobleTag";
			}
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x0009E600 File Offset: 0x0009C800
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Settlement.All.Any<Settlement>((Settlement x) => x.OwnerClan == Hero.MainHero.Clan);
		}

		// Token: 0x04000AB0 RID: 2736
		public const string Id = "PlayerIsNobleTag";
	}
}
