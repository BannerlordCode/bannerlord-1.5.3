using System;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200002E RID: 46
	internal sealed class ThreadedClientSessionDisconnectTask : ThreadedClientSessionTask
	{
		// Token: 0x0600011A RID: 282 RVA: 0x00003BCA File Offset: 0x00001DCA
		public ThreadedClientSessionDisconnectTask(IClientSession session)
			: base(session)
		{
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00003BD3 File Offset: 0x00001DD3
		public override void BeginJob()
		{
			base.Session.Disconnect();
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00003BE0 File Offset: 0x00001DE0
		public override void DoMainThreadJob()
		{
			base.Finished = true;
		}
	}
}
