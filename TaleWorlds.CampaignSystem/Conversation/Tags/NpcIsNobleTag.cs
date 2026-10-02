using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000268 RID: 616
	public class NpcIsNobleTag : ConversationTag
	{
		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06002409 RID: 9225 RVA: 0x0009E6C4 File Offset: 0x0009C8C4
		public override string StringId
		{
			get
			{
				return "NpcIsNobleTag";
			}
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x0009E6CC File Offset: 0x0009C8CC
		public override bool IsApplicableTo(CharacterObject character)
		{
			Hero heroObject = character.HeroObject;
			if (heroObject == null)
			{
				return false;
			}
			Clan clan = heroObject.Clan;
			bool? flag = ((clan != null) ? new bool?(clan.IsNoble) : null);
			bool flag2 = true;
			return (flag.GetValueOrDefault() == flag2) & (flag != null);
		}

		// Token: 0x04000AB6 RID: 2742
		public const string Id = "NpcIsNobleTag";
	}
}
