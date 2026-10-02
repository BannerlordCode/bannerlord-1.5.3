using System;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200002C RID: 44
	internal abstract class ThreadedClientSessionTask
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00003B7A File Offset: 0x00001D7A
		// (set) Token: 0x06000111 RID: 273 RVA: 0x00003B82 File Offset: 0x00001D82
		public IClientSession Session { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00003B8B File Offset: 0x00001D8B
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00003B93 File Offset: 0x00001D93
		public bool Finished { get; protected set; }

		// Token: 0x06000114 RID: 276 RVA: 0x00003B9C File Offset: 0x00001D9C
		protected ThreadedClientSessionTask(IClientSession session)
		{
			this.Session = session;
		}

		// Token: 0x06000115 RID: 277
		public abstract void BeginJob();

		// Token: 0x06000116 RID: 278
		public abstract void DoMainThreadJob();
	}
}
