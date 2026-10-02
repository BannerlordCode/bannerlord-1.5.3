using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000477 RID: 1143
	public class CommentCharacterBornBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004AB5 RID: 19125 RVA: 0x001797DB File Offset: 0x001779DB
		public override void RegisterEvents()
		{
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.HeroCreated));
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x001797F4 File Offset: 0x001779F4
		private void HeroCreated(Hero hero, bool isBornNaturally)
		{
			if (isBornNaturally)
			{
				LogEntry.AddLogEntry(new CharacterBornLogEntry(hero));
			}
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x00179804 File Offset: 0x00177A04
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
