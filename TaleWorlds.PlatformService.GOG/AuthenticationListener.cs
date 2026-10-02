using System;
using Galaxy.Api;
using TaleWorlds.Library;

namespace TaleWorlds.PlatformService.GOG
{
	// Token: 0x0200000A RID: 10
	public class AuthenticationListener : GlobalAuthListener
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00003192 File Offset: 0x00001392
		// (set) Token: 0x06000075 RID: 117 RVA: 0x0000319A File Offset: 0x0000139A
		public bool GotResult { get; private set; }

		// Token: 0x06000076 RID: 118 RVA: 0x000031A3 File Offset: 0x000013A3
		public AuthenticationListener(GOGPlatformServices gogPlatformServices)
		{
			this._gogPlatformServices = gogPlatformServices;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000031B2 File Offset: 0x000013B2
		public override void OnAuthSuccess()
		{
			Debug.Print("Successfully signed in", 0, Debug.DebugColor.White, 17592186044416UL);
			GalaxyInstance.User().GetGalaxyID();
			this.GotResult = true;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000031DC File Offset: 0x000013DC
		public override void OnAuthFailure(IAuthListener.FailureReason failureReason)
		{
			Debug.Print("Failed to sign in for reason " + failureReason, 0, Debug.DebugColor.White, 17592186044416UL);
			this.GotResult = true;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003206 File Offset: 0x00001406
		public override void OnAuthLost()
		{
			Debug.Print("Authorization lost", 0, Debug.DebugColor.White, 17592186044416UL);
			this.GotResult = true;
		}

		// Token: 0x04000021 RID: 33
		private GOGPlatformServices _gogPlatformServices;
	}
}
