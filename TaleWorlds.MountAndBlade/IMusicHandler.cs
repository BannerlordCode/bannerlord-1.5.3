using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001DA RID: 474
	public interface IMusicHandler
	{
		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001C50 RID: 7248
		bool IsPausable { get; }

		// Token: 0x06001C51 RID: 7249
		void OnUpdated(float dt);
	}
}
