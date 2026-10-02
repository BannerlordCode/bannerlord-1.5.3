using System;

namespace TaleWorlds.MountAndBlade.ViewModelCollection
{
	// Token: 0x02000008 RID: 8
	public interface IMissionScreen
	{
		// Token: 0x06000081 RID: 129
		bool GetDisplayDialog();

		// Token: 0x06000082 RID: 130
		void SetOrderFlagVisibility(bool value);

		// Token: 0x06000083 RID: 131
		string GetFollowText();

		// Token: 0x06000084 RID: 132
		string GetFollowPartyText();
	}
}
