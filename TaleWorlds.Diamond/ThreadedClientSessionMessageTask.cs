using System;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000030 RID: 48
	internal sealed class ThreadedClientSessionMessageTask : ThreadedClientSessionTask
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00003CD1 File Offset: 0x00001ED1
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00003CD9 File Offset: 0x00001ED9
		public Message Message { get; private set; }

		// Token: 0x06000126 RID: 294 RVA: 0x00003CE2 File Offset: 0x00001EE2
		public ThreadedClientSessionMessageTask(IClientSession session, Message message)
			: base(session)
		{
			this.Message = message;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00003CF2 File Offset: 0x00001EF2
		public override void BeginJob()
		{
			base.Session.SendMessage(this.Message);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00003D05 File Offset: 0x00001F05
		public override void DoMainThreadJob()
		{
			base.Finished = true;
		}
	}
}
