using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.Core.ViewModelCollection.Tutorial
{
	// Token: 0x0200000F RID: 15
	public class TutorialNotificationElementChangeEvent : EventBase
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003496 File Offset: 0x00001696
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x0000349E File Offset: 0x0000169E
		public string NewNotificationElementID { get; private set; }

		// Token: 0x060000C2 RID: 194 RVA: 0x000034A7 File Offset: 0x000016A7
		public TutorialNotificationElementChangeEvent(string newNotificationElementID)
		{
			this.NewNotificationElementID = newNotificationElementID ?? string.Empty;
		}
	}
}
