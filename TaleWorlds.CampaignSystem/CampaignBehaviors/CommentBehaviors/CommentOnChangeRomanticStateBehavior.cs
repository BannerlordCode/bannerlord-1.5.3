using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000479 RID: 1145
	public class CommentOnChangeRomanticStateBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004ABD RID: 19133 RVA: 0x00179904 File Offset: 0x00177B04
		public override void RegisterEvents()
		{
			CampaignEvents.RomanticStateChanged.AddNonSerializedListener(this, new Action<Hero, Hero, Romance.RomanceLevelEnum>(this.OnRomanticStateChanged));
		}

		// Token: 0x06004ABE RID: 19134 RVA: 0x0017991D File Offset: 0x00177B1D
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004ABF RID: 19135 RVA: 0x0017991F File Offset: 0x00177B1F
		private void OnRomanticStateChanged(Hero hero1, Hero hero2, Romance.RomanceLevelEnum level)
		{
			if (hero1 == Hero.MainHero || hero2 == Hero.MainHero || hero1.Clan.Leader == hero1 || hero2.Clan.Leader == hero2)
			{
				LogEntry.AddLogEntry(new ChangeRomanticStateLogEntry(hero1, hero2, level));
			}
		}
	}
}
