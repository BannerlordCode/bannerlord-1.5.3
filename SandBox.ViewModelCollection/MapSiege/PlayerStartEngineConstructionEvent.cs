using System;
using TaleWorlds.Core;
using TaleWorlds.Library.EventSystem;

namespace SandBox.ViewModelCollection.MapSiege
{
	// Token: 0x02000055 RID: 85
	public class PlayerStartEngineConstructionEvent : EventBase
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00014A15 File Offset: 0x00012C15
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x00014A1D File Offset: 0x00012C1D
		public SiegeEngineType Engine { get; private set; }

		// Token: 0x0600056A RID: 1386 RVA: 0x00014A26 File Offset: 0x00012C26
		public PlayerStartEngineConstructionEvent(SiegeEngineType engine)
		{
			this.Engine = engine;
		}
	}
}
