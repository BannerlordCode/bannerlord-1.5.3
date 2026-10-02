using System;
using TaleWorlds.Core;

namespace Sandbox.View.GameStates
{
	// Token: 0x0200002B RID: 43
	public class PreloadState : GameState
	{
		// Token: 0x0600018F RID: 399 RVA: 0x00012130 File Offset: 0x00010330
		public PreloadState()
		{
			this.LoadDelayInFrames = 1;
			this.SaveToLoad = string.Empty;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0001214A File Offset: 0x0001034A
		public PreloadState(string saveName)
		{
			this.LoadDelayInFrames = 2;
			this.SaveToLoad = saveName;
		}

		// Token: 0x04000008 RID: 8
		public readonly string SaveToLoad;

		// Token: 0x04000009 RID: 9
		public readonly int LoadDelayInFrames;
	}
}
