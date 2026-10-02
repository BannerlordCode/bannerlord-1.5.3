using System;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000017 RID: 23
	public interface ILoginAccessProvider
	{
		// Token: 0x0600007E RID: 126
		void Initialize(string preferredUserName, PlatformInitParams initParams);

		// Token: 0x0600007F RID: 127
		string GetUserName();

		// Token: 0x06000080 RID: 128
		PlayerId GetPlayerId();

		// Token: 0x06000081 RID: 129
		AccessObjectResult CreateAccessObject();
	}
}
