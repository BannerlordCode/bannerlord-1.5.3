using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000DB RID: 219
	public class TutorialContextChangedEvent : EventBase
	{
		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x00024E5A File Offset: 0x0002305A
		// (set) Token: 0x06000B54 RID: 2900 RVA: 0x00024E62 File Offset: 0x00023062
		public TutorialContexts NewContext { get; private set; }

		// Token: 0x06000B55 RID: 2901 RVA: 0x00024E6B File Offset: 0x0002306B
		public TutorialContextChangedEvent(TutorialContexts newContext)
		{
			this.NewContext = newContext;
		}
	}
}
