using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000052 RID: 82
	public interface ILoadingWindowManager
	{
		// Token: 0x06000886 RID: 2182
		void EnableLoadingWindow();

		// Token: 0x06000887 RID: 2183
		void DisableLoadingWindow();

		// Token: 0x06000888 RID: 2184
		void SetCurrentModeIsMultiplayer(bool isMultiplayer);

		// Token: 0x06000889 RID: 2185
		void Initialize();

		// Token: 0x0600088A RID: 2186
		void Destroy();
	}
}
