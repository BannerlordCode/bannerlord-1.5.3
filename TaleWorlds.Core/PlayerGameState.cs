using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000C5 RID: 197
	public abstract class PlayerGameState : GameState
	{
		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x0002320B File Offset: 0x0002140B
		// (set) Token: 0x06000ADE RID: 2782 RVA: 0x00023213 File Offset: 0x00021413
		public VirtualPlayer Peer
		{
			get
			{
				return this._peer;
			}
			private set
			{
				this._peer = value;
			}
		}

		// Token: 0x04000608 RID: 1544
		private VirtualPlayer _peer;
	}
}
