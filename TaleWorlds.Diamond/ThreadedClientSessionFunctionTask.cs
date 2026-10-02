using System;
using System.Threading.Tasks;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000031 RID: 49
	internal sealed class ThreadedClientSessionFunctionTask : ThreadedClientSessionTask
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00003D0E File Offset: 0x00001F0E
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00003D16 File Offset: 0x00001F16
		public CallResult CallResult { get; private set; }

		// Token: 0x0600012B RID: 299 RVA: 0x00003D1F File Offset: 0x00001F1F
		public ThreadedClientSessionFunctionTask(IClientSession session, Message message)
			: base(session)
		{
			this._message = message;
			this._taskCompletionSource = new TaskCompletionSource<bool>();
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00003D3A File Offset: 0x00001F3A
		public override void BeginJob()
		{
			this._task = this.CallFunction();
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00003D48 File Offset: 0x00001F48
		private async Task CallFunction()
		{
			CallResult callResult = await base.Session.CallFunction<FunctionResult>(this._message);
			this.CallResult = callResult;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00003D8D File Offset: 0x00001F8D
		public override void DoMainThreadJob()
		{
			if (this._task.IsCompleted)
			{
				this._taskCompletionSource.SetResult(true);
				base.Finished = true;
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00003DB0 File Offset: 0x00001FB0
		public async Task Wait()
		{
			await this._taskCompletionSource.Task;
		}

		// Token: 0x0400005E RID: 94
		private TaskCompletionSource<bool> _taskCompletionSource;

		// Token: 0x0400005F RID: 95
		private Message _message;

		// Token: 0x04000061 RID: 97
		private Task _task;
	}
}
