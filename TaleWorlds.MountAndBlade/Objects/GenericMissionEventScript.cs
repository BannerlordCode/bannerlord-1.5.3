using System;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x020003AC RID: 940
	public class GenericMissionEventScript : ScriptComponentBehavior
	{
		// Token: 0x060035C1 RID: 13761 RVA: 0x000DDF58 File Offset: 0x000DC158
		public GenericMissionEventScript()
		{
			this.EventId = string.Empty;
			this.Parameter = string.Empty;
			this.IsDisabled = false;
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x000DDF7D File Offset: 0x000DC17D
		public GenericMissionEventScript(string eventId, string parameter)
		{
			this.EventId = eventId;
			this.Parameter = parameter;
			this.IsDisabled = false;
		}

		// Token: 0x040016EA RID: 5866
		public string EventId;

		// Token: 0x040016EB RID: 5867
		public string Parameter;

		// Token: 0x040016EC RID: 5868
		[EditableScriptComponentVariable(false, "")]
		public bool IsDisabled;
	}
}
