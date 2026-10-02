using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaleWorlds.Library;
using TaleWorlds.Library.Http;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x02000039 RID: 57
	public class ClientRestSession : IClientSession
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000149 RID: 329 RVA: 0x00003F84 File Offset: 0x00002184
		// (remove) Token: 0x0600014A RID: 330 RVA: 0x00003FBC File Offset: 0x000021BC
		public event MessageHandledDelegate MessageReceived;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600014B RID: 331 RVA: 0x00003FF4 File Offset: 0x000021F4
		// (remove) Token: 0x0600014C RID: 332 RVA: 0x0000402C File Offset: 0x0000222C
		public event ConnectedDelegate Connected;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600014D RID: 333 RVA: 0x00004064 File Offset: 0x00002264
		// (remove) Token: 0x0600014E RID: 334 RVA: 0x0000409C File Offset: 0x0000229C
		public event DisconnectedDelegate Disconnected;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600014F RID: 335 RVA: 0x000040D4 File Offset: 0x000022D4
		// (remove) Token: 0x06000150 RID: 336 RVA: 0x0000410C File Offset: 0x0000230C
		public event OnCantConnectDelegate ConnectionFailed;

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00004141 File Offset: 0x00002341
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00004149 File Offset: 0x00002349
		public bool IsConnected { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00004152 File Offset: 0x00002352
		// (set) Token: 0x06000154 RID: 340 RVA: 0x0000415A File Offset: 0x0000235A
		public int AliveCheckInterval { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00004163 File Offset: 0x00002363
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00004170 File Offset: 0x00002370
		public string Address
		{
			get
			{
				return this._address;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					Debug.Print("ClientRestSession.Address: ignoring empty address.", 0, Debug.DebugColor.White, 17592186044416UL);
					return;
				}
				if (!this.IsIdle)
				{
					Debug.Print("ClientRestSession.Address: ignoring assignment while the session is not idle.", 0, Debug.DebugColor.White, 17592186044416UL);
					return;
				}
				this._address = value;
				this._consecutiveFailures = 0;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000157 RID: 343 RVA: 0x000041CB File Offset: 0x000023CB
		public bool IsIdle
		{
			get
			{
				return !this.IsConnected && this._currentMessageTask == null && this._aliveTask == null && this._messageTaskQueue.Count == 0;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000158 RID: 344 RVA: 0x000041F5 File Offset: 0x000023F5
		// (set) Token: 0x06000159 RID: 345 RVA: 0x000041FD File Offset: 0x000023FD
		public int MaxConsecutiveFailuresBeforeDisconnect { get; set; } = 3;

		// Token: 0x0600015A RID: 346 RVA: 0x00004208 File Offset: 0x00002408
		public ClientRestSession(string address, IHttpDriver platformNetworkClient, int aliveCheckInterval)
		{
			this.AliveCheckInterval = aliveCheckInterval;
			this._sessionInitialized = false;
			this._platformNetworkClient = platformNetworkClient;
			this.ResetTimer();
			this._address = address;
			this._messageTaskQueue = new Queue<ClientRestSessionTask>();
			this._restDataJsonConverter = new RestDataJsonConverter();
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000425C File Offset: 0x0000245C
		private void ResetTimer()
		{
			this._timer = new Stopwatch();
			this._timer.Start();
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00004274 File Offset: 0x00002474
		private void AssignRequestJob(ClientRestSessionTask requestMessageTask)
		{
			RestRequestMessage restRequestMessage = requestMessageTask.RestRequestMessage;
			bool flag = false;
			if (restRequestMessage is ConnectMessage)
			{
				if (!this.IsConnected)
				{
					flag = true;
				}
			}
			else if (restRequestMessage is DisconnectMessage)
			{
				if (this.IsConnected)
				{
					flag = true;
				}
			}
			else if (this.IsConnected)
			{
				flag = true;
			}
			if (flag)
			{
				this._currentMessageTask = requestMessageTask;
				this._currentMessageTask.SetRequestData(this._userCertificate, this._address, this._platformNetworkClient);
				return;
			}
			Debug.Print("Setting new request message as failed because can't assign it", 0, Debug.DebugColor.White, 17592186044416UL);
			requestMessageTask.SetFinishedAsFailed();
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00004304 File Offset: 0x00002504
		private void RemoveRequestJob()
		{
			this._lastRequestOperationTime = this._timer.ElapsedMilliseconds;
			string text = "[AliveChannel] Main-channel response complete ({0}) — alive timer reset at {1}ms.";
			RestRequestMessage restRequestMessage = this._currentMessageTask.RestRequestMessage;
			Debug.Print(string.Format(text, (restRequestMessage != null) ? restRequestMessage.TypeName : null, this._lastRequestOperationTime), 0, Debug.DebugColor.White, 17592186044416UL);
			this._currentMessageTask = null;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00004368 File Offset: 0x00002568
		void IClientSession.Tick()
		{
			this.TryAssignJob();
			if (this._currentMessageTask != null)
			{
				this._currentMessageTask.Tick();
				if (this._currentMessageTask.IsCompletelyFinished)
				{
					this.ProcessCompletedMessageTask(this._currentMessageTask);
					if (this._currentMessageTask != null && this._currentMessageTask.Finished)
					{
						this.RemoveRequestJob();
					}
				}
			}
			this.TryAssignAliveJob();
			if (this._aliveTask != null)
			{
				this._aliveTask.Tick();
				if (this._aliveTask.IsCompletelyFinished)
				{
					ClientRestSessionTask aliveTask = this._aliveTask;
					this._aliveTask = null;
					this.ProcessCompletedAliveTask(aliveTask);
				}
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00004400 File Offset: 0x00002600
		[return: TupleElementNames(new string[] { "result", "response" })]
		private ValueTuple<ClientRestSession.RequestResult, RestResponse> ClassifyRequestResult(ClientRestSessionTask task)
		{
			if (!task.Request.Successful)
			{
				return new ValueTuple<ClientRestSession.RequestResult, RestResponse>(ClientRestSession.RequestResult.TransientFailure, null);
			}
			string responseData = task.Request.ResponseData;
			if (string.IsNullOrEmpty(responseData))
			{
				return new ValueTuple<ClientRestSession.RequestResult, RestResponse>(ClientRestSession.RequestResult.TransientFailure, null);
			}
			RestResponse restResponse = JsonConvert.DeserializeObject<RestResponse>(responseData, new JsonConverter[] { this._restDataJsonConverter });
			if (restResponse.Successful)
			{
				return new ValueTuple<ClientRestSession.RequestResult, RestResponse>(ClientRestSession.RequestResult.Success, restResponse);
			}
			string successfulReason = restResponse.SuccessfulReason;
			if (ClientRestSession.IsSessionFatal(successfulReason))
			{
				return new ValueTuple<ClientRestSession.RequestResult, RestResponse>(ClientRestSession.RequestResult.SessionFatal, restResponse);
			}
			RestObjectRequestMessage restObjectRequestMessage;
			if (successfulReason == "HandlerFailed" && (restObjectRequestMessage = task.RestRequestMessage as RestObjectRequestMessage) != null && restObjectRequestMessage.MessageType == MessageType.Function)
			{
				return new ValueTuple<ClientRestSession.RequestResult, RestResponse>(ClientRestSession.RequestResult.HandlerRejected, restResponse);
			}
			return new ValueTuple<ClientRestSession.RequestResult, RestResponse>(ClientRestSession.RequestResult.TransientFailure, restResponse);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000044B0 File Offset: 0x000026B0
		private static bool IsSessionFatal(string reason)
		{
			if (string.IsNullOrEmpty(reason))
			{
				return false;
			}
			using (IEnumerator<string> enumerator = ClientRestSession.SessionFatalReasons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == reason)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00004510 File Offset: 0x00002710
		private void ProcessCompletedMessageTask(ClientRestSessionTask task)
		{
			if (task.RestRequestMessage is ConnectMessage)
			{
				if (!task.Request.Successful)
				{
					Debug.Print("[Resilience] ConnectMessage HTTP transport failure.", 0, Debug.DebugColor.White, 17592186044416UL);
					task.SetFinishedAsFailed();
					this._userCertificate = null;
					this.ResetTimer();
					OnCantConnectDelegate connectionFailed = this.ConnectionFailed;
					if (connectionFailed == null)
					{
						return;
					}
					connectionFailed();
					return;
				}
				else
				{
					task.SetFinishedAsSuccessful(null);
					this.IsConnected = true;
					ConnectedDelegate connected = this.Connected;
					if (connected == null)
					{
						return;
					}
					connected();
					return;
				}
			}
			else
			{
				if (task.RestRequestMessage is DisconnectMessage)
				{
					task.SetFinishedAsSuccessful(null);
					this.OnDisconnected();
					return;
				}
				ValueTuple<ClientRestSession.RequestResult, RestResponse> valueTuple = this.ClassifyRequestResult(task);
				ClientRestSession.RequestResult item = valueTuple.Item1;
				RestResponse item2 = valueTuple.Item2;
				RestRequestMessage restRequestMessage = task.RestRequestMessage;
				string text = ((restRequestMessage != null) ? restRequestMessage.TypeName : null);
				switch (item)
				{
				case ClientRestSession.RequestResult.Success:
					this._consecutiveFailures = 0;
					this._userCertificate = item2.UserCertificate;
					task.SetFinishedAsSuccessful(item2);
					this.DrainSessionMessages(item2);
					return;
				case ClientRestSession.RequestResult.SessionFatal:
					Debug.Print(string.Concat(new string[]
					{
						"[Resilience] Session-fatal (",
						(item2 != null) ? item2.SuccessfulReason : null,
						") for ",
						text,
						" — disconnecting."
					}), 0, Debug.DebugColor.White, 17592186044416UL);
					task.SetFinishedAsFailed(item2);
					this.OnDisconnected();
					return;
				case ClientRestSession.RequestResult.TransientFailure:
					if (task.Request.Successful && task.RetryCount < 1)
					{
						Debug.Print(string.Format("[Resilience] Retrying {0} (app retry {1}/{2}).", text, task.RetryCount + 1, 1), 0, Debug.DebugColor.White, 17592186044416UL);
						task.ResetForRetry();
						return;
					}
					task.SetFinishedAsFailed(item2);
					this._consecutiveFailures++;
					Debug.Print(string.Format("[Resilience] Consecutive failures: {0}/{1}", this._consecutiveFailures, this.MaxConsecutiveFailuresBeforeDisconnect) + string.Format(" — {0}, total HTTP attempts: {1}.", text, task.TotalHttpAttempts), 0, Debug.DebugColor.White, 17592186044416UL);
					if (this._consecutiveFailures >= this.MaxConsecutiveFailuresBeforeDisconnect)
					{
						Debug.Print("[Resilience] Threshold reached — disconnecting.", 0, Debug.DebugColor.White, 17592186044416UL);
						this.OnDisconnected();
					}
					return;
				case ClientRestSession.RequestResult.HandlerRejected:
					Debug.Print(string.Concat(new string[]
					{
						"[Resilience] Handler rejected ",
						text,
						" (",
						(item2 != null) ? item2.SuccessfulReason : null,
						") — caller will handle."
					}), 0, Debug.DebugColor.White, 17592186044416UL);
					task.SetFinishedAsFailed(item2);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00004788 File Offset: 0x00002988
		private void ProcessCompletedAliveTask(ClientRestSessionTask task)
		{
			ValueTuple<ClientRestSession.RequestResult, RestResponse> valueTuple = this.ClassifyRequestResult(task);
			ClientRestSession.RequestResult item = valueTuple.Item1;
			RestResponse item2 = valueTuple.Item2;
			switch (item)
			{
			case ClientRestSession.RequestResult.Success:
				this._consecutiveFailures = 0;
				this._userCertificate = item2.UserCertificate;
				if (item2.Polled)
				{
					this._lastRequestOperationTime = this._timer.ElapsedMilliseconds;
					Debug.Print(string.Format("[AliveChannel] Polled — timer reset. Messages: {0}.", item2.RemainingMessageCount), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				else
				{
					Debug.Print("[AliveChannel] Polled=false (old server?) — timer not reset.", 0, Debug.DebugColor.White, 17592186044416UL);
				}
				this.DrainSessionMessages(item2);
				return;
			case ClientRestSession.RequestResult.SessionFatal:
				Debug.Print("[Resilience][AliveChannel] Session-fatal (" + ((item2 != null) ? item2.SuccessfulReason : null) + ") — disconnecting.", 0, Debug.DebugColor.White, 17592186044416UL);
				this.OnDisconnected();
				return;
			case ClientRestSession.RequestResult.TransientFailure:
			case ClientRestSession.RequestResult.HandlerRejected:
				this._consecutiveFailures++;
				Debug.Print(string.Format("[Resilience][AliveChannel] Transient failure — consecutive: {0}/{1}", this._consecutiveFailures, this.MaxConsecutiveFailuresBeforeDisconnect) + string.Format(", total HTTP attempts: {0}.", task.TotalHttpAttempts), 0, Debug.DebugColor.White, 17592186044416UL);
				if (this._consecutiveFailures >= this.MaxConsecutiveFailuresBeforeDisconnect)
				{
					Debug.Print("[Resilience][AliveChannel] Threshold reached — disconnecting.", 0, Debug.DebugColor.White, 17592186044416UL);
					this.OnDisconnected();
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000048E8 File Offset: 0x00002AE8
		private void DrainSessionMessages(RestResponse restResponse)
		{
			int num = 0;
			while (restResponse.RemainingMessageCount > 0)
			{
				RestResponseMessage restResponseMessage = restResponse.TryDequeueMessage();
				try
				{
					Message message = restResponseMessage.GetMessage();
					if (message != null)
					{
						this.HandleMessage(message);
						num++;
					}
				}
				catch (Exception ex)
				{
					Debug.Print("[SessionMessages] Failed to deliver session message (" + ex.Message + "); skipping.", 0, Debug.DebugColor.White, 17592186044416UL);
				}
			}
			if (num > 0)
			{
				Debug.Print(string.Format("[SessionMessages] Delivered {0} session message(s).", num), 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00004980 File Offset: 0x00002B80
		private void OnDisconnected()
		{
			this.IsConnected = false;
			this.ClearMessageTaskQueueDueToDisconnect();
			this._sessionCredentials = null;
			this._sessionInitialized = false;
			this._userCertificate = null;
			this._aliveTask = null;
			this.ResetTimer();
			DisconnectedDelegate disconnected = this.Disconnected;
			if (disconnected == null)
			{
				return;
			}
			disconnected();
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000049CC File Offset: 0x00002BCC
		private void TryAssignJob()
		{
			if (this._currentMessageTask == null && this._messageTaskQueue.Count > 0)
			{
				ClientRestSessionTask clientRestSessionTask = this._messageTaskQueue.Dequeue();
				this.AssignRequestJob(clientRestSessionTask);
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00004A04 File Offset: 0x00002C04
		private void TryAssignAliveJob()
		{
			if (this._aliveTask != null)
			{
				return;
			}
			if (!this.IsConnected)
			{
				return;
			}
			if (!this._sessionInitialized)
			{
				return;
			}
			if (this._userCertificate == null)
			{
				return;
			}
			long num = this._timer.ElapsedMilliseconds - this._lastRequestOperationTime;
			if (num > (long)this.AliveCheckInterval)
			{
				Debug.Print(string.Format("[AliveChannel] Firing AliveMessage — idle {0}ms > interval {1}ms. MainTask in-flight: {2}. Queue depth: {3}.", new object[]
				{
					num,
					this.AliveCheckInterval,
					this._currentMessageTask != null,
					this._messageTaskQueue.Count
				}), 0, Debug.DebugColor.White, 17592186044416UL);
				this._aliveTask = new ClientRestSessionTask(new AliveMessage(this._sessionCredentials), new CancellationTokenSource().Token, true);
				this._aliveTask.SetRequestData(this._userCertificate, this._address, this._platformNetworkClient);
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00004AF4 File Offset: 0x00002CF4
		private void ClearMessageTaskQueueDueToDisconnect()
		{
			foreach (ClientRestSessionTask clientRestSessionTask in this._messageTaskQueue)
			{
				clientRestSessionTask.SetFinishedAsFailed();
			}
			this._messageTaskQueue.Clear();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00004B50 File Offset: 0x00002D50
		public void Connect()
		{
			this.ResetTimer();
			this.SendMessage(new ConnectMessage());
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00004B63 File Offset: 0x00002D63
		public void Disconnect()
		{
			this._messageTaskQueue.Enqueue(new ClientRestSessionTask(new DisconnectMessage(), CancellationToken.None, false));
			this.ResetTimer();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00004B86 File Offset: 0x00002D86
		private void SendMessage(RestRequestMessage message)
		{
			this._messageTaskQueue.Enqueue(new ClientRestSessionTask(message, CancellationToken.None, true));
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00004BA0 File Offset: 0x00002DA0
		async Task<LoginResult> IClientSession.Login(LoginMessage message)
		{
			ClientRestSessionTask clientRestSessionTask = new ClientRestSessionTask(new RestObjectRequestMessage(null, message, MessageType.Login), CancellationToken.None, true);
			this._messageTaskQueue.Enqueue(clientRestSessionTask);
			await clientRestSessionTask.WaitUntilFinished();
			LoginResult loginResult;
			if (!clientRestSessionTask.Successful && !clientRestSessionTask.Request.Successful)
			{
				loginResult = new LoginResult(LoginErrorCode.LoginRequestFailed.ToString(), null);
			}
			else
			{
				RestFunctionResult functionResult = clientRestSessionTask.RestResponse.FunctionResult;
				LoginResult loginResult2 = null;
				if (functionResult != null)
				{
					loginResult2 = (LoginResult)functionResult.GetFunctionResult();
					if (clientRestSessionTask.Successful)
					{
						this._sessionCredentials = new SessionCredentials(loginResult2.PeerId, loginResult2.SessionKey);
						this._sessionInitialized = true;
					}
				}
				loginResult = loginResult2;
			}
			return loginResult;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00004BED File Offset: 0x00002DED
		void IClientSession.SendMessage(Message message)
		{
			this.SendMessage(new RestObjectRequestMessage(this._sessionCredentials, message, MessageType.Message));
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00004C04 File Offset: 0x00002E04
		async Task<CallResult> IClientSession.CallFunction<TResult>(Message message)
		{
			ClientRestSessionTask clientRestSessionTask = new ClientRestSessionTask(new RestObjectRequestMessage(this._sessionCredentials, message, MessageType.Function), CancellationToken.None, true);
			this._messageTaskQueue.Enqueue(clientRestSessionTask);
			await clientRestSessionTask.WaitUntilFinished();
			CallResult callResult;
			if (clientRestSessionTask.Successful)
			{
				RestFunctionResult functionResult = clientRestSessionTask.RestResponse.FunctionResult;
				callResult = new CallResult(true, (functionResult != null) ? functionResult.GetFunctionResult() : null, null);
			}
			else
			{
				RestResponse restResponse = clientRestSessionTask.RestResponse;
				string text = ((restResponse != null) ? restResponse.SuccessfulReason : null);
				callResult = new CallResult(false, null, text);
			}
			return callResult;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00004C51 File Offset: 0x00002E51
		private void HandleMessage(Message message)
		{
			MessageHandledDelegate messageReceived = this.MessageReceived;
			if (messageReceived == null)
			{
				return;
			}
			messageReceived(message);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00004C64 File Offset: 0x00002E64
		async Task<bool> IClientSession.CheckConnection()
		{
			bool flag;
			try
			{
				string text = this._address + "/Data/Ping";
				await this._platformNetworkClient.HttpGetString(text, false);
				flag = true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0400006A RID: 106
		private readonly Queue<ClientRestSessionTask> _messageTaskQueue;

		// Token: 0x0400006B RID: 107
		private volatile string _address;

		// Token: 0x0400006C RID: 108
		private byte[] _userCertificate;

		// Token: 0x0400006D RID: 109
		private ClientRestSessionTask _currentMessageTask;

		// Token: 0x0400006E RID: 110
		private ClientRestSessionTask _aliveTask;

		// Token: 0x04000070 RID: 112
		private Stopwatch _timer;

		// Token: 0x04000071 RID: 113
		private long _lastRequestOperationTime;

		// Token: 0x04000072 RID: 114
		private bool _sessionInitialized;

		// Token: 0x04000073 RID: 115
		private SessionCredentials _sessionCredentials;

		// Token: 0x04000074 RID: 116
		private RestDataJsonConverter _restDataJsonConverter;

		// Token: 0x04000075 RID: 117
		private IHttpDriver _platformNetworkClient;

		// Token: 0x04000077 RID: 119
		private const int MaxMessageRetries = 1;

		// Token: 0x04000078 RID: 120
		private int _consecutiveFailures;

		// Token: 0x0400007A RID: 122
		private static readonly ReadOnlyCollection<string> SessionFatalReasons = new ReadOnlyCollection<string>(new string[] { "SessionNotFound", "InvalidCredentials", "InvalidCertificate", "UnknownMessageType", "FeatureNotSupported", "PeerTypeMismatch" });

		// Token: 0x02000055 RID: 85
		private enum RequestResult
		{
			// Token: 0x040000EA RID: 234
			Success,
			// Token: 0x040000EB RID: 235
			SessionFatal,
			// Token: 0x040000EC RID: 236
			TransientFailure,
			// Token: 0x040000ED RID: 237
			HandlerRejected
		}
	}
}
