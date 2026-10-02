using System;
using Galaxy.Api;

namespace TaleWorlds.PlatformService.GOG
{
	// Token: 0x0200000B RID: 11
	public class UserInformationRetrieveListener : IUserInformationRetrieveListener
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003225 File Offset: 0x00001425
		// (set) Token: 0x0600007B RID: 123 RVA: 0x0000322D File Offset: 0x0000142D
		public bool GotResult { get; private set; }

		// Token: 0x0600007C RID: 124 RVA: 0x00003236 File Offset: 0x00001436
		public override void OnUserInformationRetrieveFailure(GalaxyID userID, IUserInformationRetrieveListener.FailureReason failureReason)
		{
			this.GotResult = true;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000323F File Offset: 0x0000143F
		public override void OnUserInformationRetrieveSuccess(GalaxyID userID)
		{
			this.GotResult = true;
		}
	}
}
