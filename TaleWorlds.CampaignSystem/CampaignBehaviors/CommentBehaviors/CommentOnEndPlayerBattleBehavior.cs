using System;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000482 RID: 1154
	public class CommentOnEndPlayerBattleBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004AE2 RID: 19170 RVA: 0x00179D76 File Offset: 0x00177F76
		public override void RegisterEvents()
		{
			CampaignEvents.OnPlayerBattleEndEvent.AddNonSerializedListener(this, new Action<MapEvent>(this.OnPlayerBattleEnded));
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x00179D8F File Offset: 0x00177F8F
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x00179D91 File Offset: 0x00177F91
		private void OnPlayerBattleEnded(MapEvent mapEvent)
		{
			if (!mapEvent.IsHideoutBattle || mapEvent.BattleState != BattleState.None)
			{
				LogEntry.AddLogEntry(new PlayerBattleEndedLogEntry(mapEvent));
			}
		}
	}
}
