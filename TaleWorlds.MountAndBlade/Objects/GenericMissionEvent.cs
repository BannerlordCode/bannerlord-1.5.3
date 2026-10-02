using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x020003AB RID: 939
	public class GenericMissionEvent : EventBase
	{
		// Token: 0x060035C0 RID: 13760 RVA: 0x000DDF42 File Offset: 0x000DC142
		public GenericMissionEvent(string eventId, string parameter)
		{
			this.EventId = eventId;
			this.Parameter = parameter;
		}

		// Token: 0x040016E8 RID: 5864
		public readonly string EventId;

		// Token: 0x040016E9 RID: 5865
		public readonly string Parameter;
	}
}
