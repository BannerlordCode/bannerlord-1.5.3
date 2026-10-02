using System;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200002D RID: 45
	internal sealed class ThreadedClientSessionConnectTask : ThreadedClientSessionTask
	{
		// Token: 0x06000117 RID: 279 RVA: 0x00003BAB File Offset: 0x00001DAB
		public ThreadedClientSessionConnectTask(IClientSession session)
			: base(session)
		{
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00003BB4 File Offset: 0x00001DB4
		public override void BeginJob()
		{
			base.Session.Connect();
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00003BC1 File Offset: 0x00001DC1
		public override void DoMainThreadJob()
		{
			base.Finished = true;
		}
	}
}
