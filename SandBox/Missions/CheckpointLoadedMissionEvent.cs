using System;
using TaleWorlds.Library.EventSystem;

namespace SandBox.Missions
{
	// Token: 0x02000059 RID: 89
	public class CheckpointLoadedMissionEvent : EventBase
	{
		// Token: 0x06000383 RID: 899 RVA: 0x000146B0 File Offset: 0x000128B0
		public CheckpointLoadedMissionEvent(int loadedCheckpointUniqueId)
		{
			this.LoadedCheckpointUniqueId = loadedCheckpointUniqueId;
		}

		// Token: 0x040001CC RID: 460
		public readonly int LoadedCheckpointUniqueId;
	}
}
