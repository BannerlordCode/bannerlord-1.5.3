using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200002B RID: 43
	public class ThreadedClientSession : IClientSession
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000F1 RID: 241 RVA: 0x000035B4 File Offset: 0x000017B4
		// (remove) Token: 0x060000F2 RID: 242 RVA: 0x000035EC File Offset: 0x000017EC
		public event MessageHandledDelegate MessageReceived;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000F3 RID: 243 RVA: 0x00003624 File Offset: 0x00001824
		// (remove) Token: 0x060000F4 RID: 244 RVA: 0x0000365C File Offset: 0x0000185C
		public event ConnectedDelegate Connected;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000F5 RID: 245 RVA: 0x00003694 File Offset: 0x00001894
		// (remove) Token: 0x060000F6 RID: 246 RVA: 0x000036CC File Offset: 0x000018CC
		public event DisconnectedDelegate Disconnected;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060000F7 RID: 247 RVA: 0x00003704 File Offset: 0x00001904
		// (remove) Token: 0x060000F8 RID: 248 RVA: 0x0000373C File Offset: 0x0000193C
		public event OnCantConnectDelegate ConnectionFailed;

		// Token: 0x17000037 RID: 55
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00003771 File Offset: 0x00001971
		public int AliveCheckInterval
		{
			set
			{
				this._session.AliveCheckInterval = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (set) Token: 0x060000FA RID: 250 RVA: 0x0000377F File Offset: 0x0000197F
		public int MaxConsecutiveFailuresBeforeDisconnect
		{
			set
			{
				this._session.MaxConsecutiveFailuresBeforeDisconnect = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000FB RID: 251 RVA: 0x0000378D File Offset: 0x0000198D
		// (set) Token: 0x060000FC RID: 252 RVA: 0x0000379A File Offset: 0x0000199A
		public string Address
		{
			get
			{
				return this._session.Address;
			}
			set
			{
				this._session.Address = value;
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000037A8 File Offset: 0x000019A8
		public ThreadedClientSession(IClientSession session, int threadSleepTime)
		{
			this._session = session;
			this._session.Connected += this.SessionConnected;
			this._session.Disconnected += this.SessionDisconnected;
			this._session.ConnectionFailed += this.SessionConnectionFailed;
			this._session.MessageReceived += this.SessionMessageReceived;
			this._task = null;
			this._taskBegunJob = false;
			this._threadSleepTime = threadSleepTime;
			this.RefreshTask(null);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00003852 File Offset: 0x00001A52
		private void SessionConnected()
		{
			this._eventQueue.Enqueue(delegate
			{
				ConnectedDelegate connected = this.Connected;
				if (connected == null)
				{
					return;
				}
				connected();
			});
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000386B File Offset: 0x00001A6B
		private void SessionDisconnected()
		{
			this._eventQueue.Enqueue(delegate
			{
				DisconnectedDelegate disconnected = this.Disconnected;
				if (disconnected == null)
				{
					return;
				}
				disconnected();
			});
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00003884 File Offset: 0x00001A84
		private void SessionConnectionFailed()
		{
			this._eventQueue.Enqueue(delegate
			{
				OnCantConnectDelegate connectionFailed = this.ConnectionFailed;
				if (connectionFailed == null)
				{
					return;
				}
				connectionFailed();
			});
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000038A0 File Offset: 0x00001AA0
		private void SessionMessageReceived(Message message)
		{
			this._eventQueue.Enqueue(delegate
			{
				MessageHandledDelegate messageReceived = this.MessageReceived;
				if (messageReceived == null)
				{
					return;
				}
				messageReceived(message);
			});
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000038D8 File Offset: 0x00001AD8
		private void RefreshTask(Task previousTask)
		{
			if (previousTask == null || previousTask.IsCompleted)
			{
				Task.Run(async delegate
				{
					this.ThreadMain();
					await Task.Delay(this._threadSleepTime);
				}).ContinueWith(delegate(Task t)
				{
					this.RefreshTask(t);
				}, TaskContinuationOptions.ExecuteSynchronously);
				return;
			}
			if (previousTask.IsFaulted)
			{
				throw new Exception("ThreadedClientSession.ThreadMain Task is faulted", previousTask.Exception);
			}
			throw new Exception("RefreshTask is called before task is completed");
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000393C File Offset: 0x00001B3C
		private void ThreadMain()
		{
			this._session.Tick();
			if (!this._taskBegunJob && this._tasks.TryDequeue(out this._task))
			{
				this._task.BeginJob();
				this._taskBegunJob = true;
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000397C File Offset: 0x00001B7C
		void IClientSession.Connect()
		{
			ThreadedClientSessionConnectTask threadedClientSessionConnectTask = new ThreadedClientSessionConnectTask(this._session);
			this._tasks.Enqueue(threadedClientSessionConnectTask);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000039A4 File Offset: 0x00001BA4
		void IClientSession.Disconnect()
		{
			ThreadedClientSessionDisconnectTask threadedClientSessionDisconnectTask = new ThreadedClientSessionDisconnectTask(this._session);
			this._tasks.Enqueue(threadedClientSessionDisconnectTask);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000039CC File Offset: 0x00001BCC
		void IClientSession.Tick()
		{
			if (this._taskBegunJob)
			{
				this._task.DoMainThreadJob();
				if (this._task.Finished)
				{
					this._task = null;
					this._taskBegunJob = false;
				}
			}
			Action action;
			while (this._eventQueue.TryDequeue(out action))
			{
				action();
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00003A24 File Offset: 0x00001C24
		async Task<LoginResult> IClientSession.Login(LoginMessage message)
		{
			ThreadedClientSessionLoginTask task = new ThreadedClientSessionLoginTask(this._session, message);
			this._tasks.Enqueue(task);
			await task.Wait();
			return task.LoginResult;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00003A74 File Offset: 0x00001C74
		void IClientSession.SendMessage(Message message)
		{
			ThreadedClientSessionMessageTask threadedClientSessionMessageTask = new ThreadedClientSessionMessageTask(this._session, message);
			this._tasks.Enqueue(threadedClientSessionMessageTask);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00003A9C File Offset: 0x00001C9C
		async Task<CallResult> IClientSession.CallFunction<TReturn>(Message message)
		{
			ThreadedClientSessionFunctionTask task = new ThreadedClientSessionFunctionTask(this._session, message);
			this._tasks.Enqueue(task);
			await task.Wait();
			return task.CallResult;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00003AE9 File Offset: 0x00001CE9
		Task<bool> IClientSession.CheckConnection()
		{
			return this._session.CheckConnection();
		}

		// Token: 0x0400004D RID: 77
		private IClientSession _session;

		// Token: 0x0400004E RID: 78
		private ThreadedClientSessionTask _task;

		// Token: 0x0400004F RID: 79
		private volatile bool _taskBegunJob;

		// Token: 0x04000050 RID: 80
		private readonly int _threadSleepTime;

		// Token: 0x04000055 RID: 85
		private ConcurrentQueue<Action> _eventQueue = new ConcurrentQueue<Action>();

		// Token: 0x04000056 RID: 86
		private ConcurrentQueue<ThreadedClientSessionTask> _tasks = new ConcurrentQueue<ThreadedClientSessionTask>();
	}
}
