using System;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x0200006F RID: 111
	[DefaultView]
	public abstract class MissionCheatView : MissionView
	{
		// Token: 0x06000448 RID: 1096
		public abstract bool GetIsCheatsAvailable();

		// Token: 0x06000449 RID: 1097
		public abstract void InitializeScreen();

		// Token: 0x0600044A RID: 1098
		public abstract void FinalizeScreen();
	}
}
