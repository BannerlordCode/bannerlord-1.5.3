using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000478 RID: 1144
	public class CommentChildbirthBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004AB9 RID: 19129 RVA: 0x0017980E File Offset: 0x00177A0E
		public override void RegisterEvents()
		{
			CampaignEvents.OnGivenBirthEvent.AddNonSerializedListener(this, new Action<Hero, List<Hero>, int>(this.OnGivenBirthEvent));
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x00179827 File Offset: 0x00177A27
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x0017982C File Offset: 0x00177A2C
		private void OnGivenBirthEvent(Hero mother, List<Hero> aliveChildren, int stillbornCount)
		{
			if (mother.IsHumanPlayerCharacter || mother.Clan == Hero.MainHero.Clan)
			{
				for (int i = 0; i < stillbornCount; i++)
				{
					ChildbirthLogEntry childbirthLogEntry = new ChildbirthLogEntry(mother, null);
					LogEntry.AddLogEntry(childbirthLogEntry);
					Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new ChildBornMapNotification(null, childbirthLogEntry.GetEncyclopediaText(), CampaignTime.Now));
				}
				foreach (Hero hero in aliveChildren)
				{
					ChildbirthLogEntry childbirthLogEntry2 = new ChildbirthLogEntry(mother, hero);
					LogEntry.AddLogEntry(childbirthLogEntry2);
					Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new ChildBornMapNotification(hero, childbirthLogEntry2.GetEncyclopediaText(), CampaignTime.Now));
				}
			}
		}
	}
}
