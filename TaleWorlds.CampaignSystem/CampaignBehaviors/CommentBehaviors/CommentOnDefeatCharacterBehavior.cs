using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000480 RID: 1152
	public class CommentOnDefeatCharacterBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004ADA RID: 19162 RVA: 0x00179CB9 File Offset: 0x00177EB9
		public override void RegisterEvents()
		{
			CampaignEvents.CharacterDefeated.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnCharacterDefeated));
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x00179CD2 File Offset: 0x00177ED2
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x00179CD4 File Offset: 0x00177ED4
		private void OnCharacterDefeated(Hero winner, Hero loser)
		{
			LogEntry.AddLogEntry(new DefeatCharacterLogEntry(winner, loser));
		}
	}
}
