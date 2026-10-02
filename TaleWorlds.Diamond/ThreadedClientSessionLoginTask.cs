using System;
using System.Threading.Tasks;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200002F RID: 47
	internal sealed class ThreadedClientSessionLoginTask : ThreadedClientSessionTask
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00003BE9 File Offset: 0x00001DE9
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00003BF1 File Offset: 0x00001DF1
		public LoginResult LoginResult { get; private set; }

		// Token: 0x0600011F RID: 287 RVA: 0x00003BFA File Offset: 0x00001DFA
		public ThreadedClientSessionLoginTask(IClientSession session, LoginMessage message)
			: base(session)
		{
			this._message = message;
			this._taskCompletionSource = new TaskCompletionSource<bool>();
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00003C15 File Offset: 0x00001E15
		public override void BeginJob()
		{
			this._task = this.Login();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00003C24 File Offset: 0x00001E24
		private async Task Login()
		{
			LoginResult loginResult = await base.Session.Login(this._message);
			this.LoginResult = loginResult;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00003C69 File Offset: 0x00001E69
		public override void DoMainThreadJob()
		{
			if (this._task.IsCompleted)
			{
				this._taskCompletionSource.SetResult(true);
				base.Finished = true;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00003C8C File Offset: 0x00001E8C
		public async Task Wait()
		{
			await this._taskCompletionSource.Task;
		}

		// Token: 0x04000059 RID: 89
		private TaskCompletionSource<bool> _taskCompletionSource;

		// Token: 0x0400005A RID: 90
		private LoginMessage _message;

		// Token: 0x0400005C RID: 92
		private Task _task;
	}
}
