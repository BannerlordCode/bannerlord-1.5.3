using System;
using TaleWorlds.Network;

namespace TaleWorlds.Core
{
	// Token: 0x020000DF RID: 223
	public class WaitForGameState : CoroutineState
	{
		// Token: 0x06000B7C RID: 2940 RVA: 0x000253CD File Offset: 0x000235CD
		public WaitForGameState(Type stateType)
		{
			this._stateType = stateType;
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x000253DC File Offset: 0x000235DC
		protected override bool IsFinished
		{
			get
			{
				GameState gameState = ((GameStateManager.Current != null) ? GameStateManager.Current.ActiveState : null);
				return gameState != null && this._stateType.IsInstanceOfType(gameState);
			}
		}

		// Token: 0x04000695 RID: 1685
		private Type _stateType;
	}
}
