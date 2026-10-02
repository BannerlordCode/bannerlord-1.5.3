using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000087 RID: 135
	public interface IGameStateListener
	{
		// Token: 0x060008A1 RID: 2209
		void OnActivate();

		// Token: 0x060008A2 RID: 2210
		void OnDeactivate();

		// Token: 0x060008A3 RID: 2211
		void OnInitialize();

		// Token: 0x060008A4 RID: 2212
		void OnFinalize();
	}
}
