using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000D4 RID: 212
	public class CheckpointCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000955 RID: 2389 RVA: 0x00043B34 File Offset: 0x00041D34
		public override void RegisterEvents()
		{
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00043B36 File Offset: 0x00041D36
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<int>("LastUsedMissionCheckpointId", ref this.LastUsedMissionCheckpointId);
			dataStore.SyncData<List<AgentSaveData>>("CorpseList", ref this.CorpseList);
		}

		// Token: 0x0400047C RID: 1148
		public int LastUsedMissionCheckpointId = -1;

		// Token: 0x0400047D RID: 1149
		public List<AgentSaveData> CorpseList = new List<AgentSaveData>();
	}
}
