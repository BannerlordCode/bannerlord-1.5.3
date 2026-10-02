using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TaleWorlds.Diamond.ClientApplication;
using TaleWorlds.Library;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000006 RID: 6
	public abstract class Client : DiamondClientApplicationObject, IClient
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000012 RID: 18 RVA: 0x0000245D File Offset: 0x0000065D
		// (set) Token: 0x06000013 RID: 19 RVA: 0x00002465 File Offset: 0x00000665
		public bool IsInCriticalState
		{
			get
			{
				return this._isInCriticalState;
			}
			set
			{
				this._isInCriticalState = value;
				this.UpdateAliveCheckInterval();
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002474 File Offset: 0x00000674
		public virtual int AliveCheckTimeInMilliSeconds
		{
			get
			{
				return 2000;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000247B File Offset: 0x0000067B
		public virtual int MaxConsecutiveFailuresBeforeDisconnect
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000247E File Offset: 0x0000067E
		protected void UpdateAliveCheckInterval()
		{
			this._clientSession.AliveCheckInterval = this.AliveCheckTimeInMilliSeconds;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002491 File Offset: 0x00000691
		private void ClientSessionConnected()
		{
			this.OnConnected();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002499 File Offset: 0x00000699
		private void ClientSessionDisconnected()
		{
			this.OnDisconnected();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000024A1 File Offset: 0x000006A1
		private void ClientSessionConnectionFailed()
		{
			this.OnCantConnect();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024A9 File Offset: 0x000006A9
		private void ClientSessionMessageReceived(Message message)
		{
			this.HandleMessage(message);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000024B4 File Offset: 0x000006B4
		protected Client(DiamondClientApplication diamondClientApplication, IClientSessionFactory sessionProvider, bool autoReconnect)
			: base(diamondClientApplication)
		{
			this._clientSession = sessionProvider.CreateSession(this.AliveCheckTimeInMilliSeconds);
			this._clientSession.MaxConsecutiveFailuresBeforeDisconnect = this.MaxConsecutiveFailuresBeforeDisconnect;
			this._clientSession.Connected += this.ClientSessionConnected;
			this._clientSession.ConnectionFailed += this.ClientSessionConnectionFailed;
			this._clientSession.Disconnected += this.ClientSessionDisconnected;
			this._clientSession.MessageReceived += this.ClientSessionMessageReceived;
			this._messageHandlers = new Dictionary<Type, Delegate>();
			this._autoReconnect = autoReconnect;
			if (autoReconnect)
			{
				this.Reset();
				this._connectionState = Client.ConnectionState.ReadyToConnect;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002574 File Offset: 0x00000774
		public void Update()
		{
			this._clientSession.Tick();
			if (this._connectionState == Client.ConnectionState.SleepingToConnectAgain)
			{
				if (this._timer.ElapsedMilliseconds > 5000L)
				{
					this._connectionState = Client.ConnectionState.ReadyToConnect;
					this._timer.Stop();
					this._timer = null;
				}
			}
			else if (this._connectionState == Client.ConnectionState.ReadyToConnect)
			{
				this._connectionState = Client.ConnectionState.Connecting;
				this._clientSession.Connect();
			}
			else
			{
				Client.ConnectionState connectionState = this._connectionState;
			}
			this.OnTick();
		}

		// Token: 0x0600001D RID: 29
		protected abstract void OnTick();

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000025EF File Offset: 0x000007EF
		// (set) Token: 0x0600001F RID: 31 RVA: 0x000025FC File Offset: 0x000007FC
		protected string SessionAddress
		{
			get
			{
				return this._clientSession.Address;
			}
			set
			{
				this._clientSession.Address = value;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000260A File Offset: 0x0000080A
		protected void SendMessage(Message message)
		{
			this._clientSession.SendMessage(message);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002618 File Offset: 0x00000818
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002620 File Offset: 0x00000820
		public ILoginAccessProvider AccessProvider { get; protected set; }

		// Token: 0x06000023 RID: 35 RVA: 0x0000262C File Offset: 0x0000082C
		protected async Task<LoginResult> Login(LoginMessage message)
		{
			Debug.Print("Logging in", 0, Debug.DebugColor.White, 17592186044416UL);
			return await this._clientSession.Login(message);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000267C File Offset: 0x0000087C
		protected async Task<CallResult> CallFunction<TResult>(Message message) where TResult : FunctionResult
		{
			return await this._clientSession.CallFunction<TResult>(message);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000026C9 File Offset: 0x000008C9
		protected void AddMessageHandler<TMessage>(ClientMessageHandler<TMessage> messageHandler) where TMessage : Message
		{
			this._messageHandlers.Add(typeof(TMessage), messageHandler);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000026E1 File Offset: 0x000008E1
		public void HandleMessage(Message message)
		{
			this._messageHandlers[message.GetType()].DynamicInvokeWithLog(new object[] { message });
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002704 File Offset: 0x00000904
		public virtual void OnConnected()
		{
			this._connectionState = Client.ConnectionState.Connected;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000270D File Offset: 0x0000090D
		public virtual void OnCantConnect()
		{
			if (this._autoReconnect)
			{
				this.Reset();
				return;
			}
			this._connectionState = Client.ConnectionState.Idle;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002725 File Offset: 0x00000925
		public virtual void OnDisconnected()
		{
			if (this._autoReconnect)
			{
				this.Reset();
				return;
			}
			this._connectionState = Client.ConnectionState.Idle;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000273D File Offset: 0x0000093D
		protected void BeginConnect()
		{
			this._connectionState = Client.ConnectionState.ReadyToConnect;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002746 File Offset: 0x00000946
		protected void BeginDisconnect()
		{
			this._clientSession.Disconnect();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002753 File Offset: 0x00000953
		protected void SetAliveCheckTime(long time)
		{
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002758 File Offset: 0x00000958
		private void Reset()
		{
			this._connectionState = Client.ConnectionState.SleepingToConnectAgain;
			this._timer = new Stopwatch();
			this._timer.Start();
			Debug.Print("Waiting " + 5000L + " milliseconds for another connection attempt", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000027AD File Offset: 0x000009AD
		public Task<bool> CheckConnection()
		{
			return this._clientSession.CheckConnection();
		}

		// Token: 0x04000005 RID: 5
		private IClientSession _clientSession;

		// Token: 0x04000006 RID: 6
		private Dictionary<Type, Delegate> _messageHandlers;

		// Token: 0x04000007 RID: 7
		private Client.ConnectionState _connectionState;

		// Token: 0x04000008 RID: 8
		private Stopwatch _timer;

		// Token: 0x04000009 RID: 9
		private const long ReconnectTime = 5000L;

		// Token: 0x0400000A RID: 10
		private bool _autoReconnect;

		// Token: 0x0400000B RID: 11
		private bool _isInCriticalState;

		// Token: 0x0400000C RID: 12
		public int CriticalStateCheckTime = 1000;

		// Token: 0x02000047 RID: 71
		private enum ConnectionState
		{
			// Token: 0x040000B5 RID: 181
			Idle,
			// Token: 0x040000B6 RID: 182
			ReadyToConnect,
			// Token: 0x040000B7 RID: 183
			Connecting,
			// Token: 0x040000B8 RID: 184
			Connected,
			// Token: 0x040000B9 RID: 185
			SleepingToConnectAgain
		}
	}
}
