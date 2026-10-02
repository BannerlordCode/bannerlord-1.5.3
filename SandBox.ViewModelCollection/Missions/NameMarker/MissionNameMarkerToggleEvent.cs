using System;
using TaleWorlds.Library.EventSystem;

namespace SandBox.ViewModelCollection.Missions.NameMarker
{
	// Token: 0x02000035 RID: 53
	public class MissionNameMarkerToggleEvent : EventBase
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0001128C File Offset: 0x0000F48C
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x00011294 File Offset: 0x0000F494
		public bool NewState { get; private set; }

		// Token: 0x06000416 RID: 1046 RVA: 0x0001129D File Offset: 0x0000F49D
		public MissionNameMarkerToggleEvent(bool newState)
		{
			this.NewState = newState;
		}
	}
}
