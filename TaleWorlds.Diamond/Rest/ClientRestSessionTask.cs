using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.Library.Http;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x0200003A RID: 58
	internal class ClientRestSessionTask
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00004CF9 File Offset: 0x00002EF9
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00004D01 File Offset: 0x00002F01
		public RestRequestMessage RestRequestMessage { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00004D0A File Offset: 0x00002F0A
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00004D12 File Offset: 0x00002F12
		public bool Finished { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00004D1B File Offset: 0x00002F1B
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00004D23 File Offset: 0x00002F23
		public bool Successful { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00004D2C File Offset: 0x00002F2C
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00004D34 File Offset: 0x00002F34
		public IHttpRequestTask Request { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00004D3D File Offset: 0x00002F3D
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00004D45 File Offset: 0x00002F45
		public CancellationToken CancellationToken { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00004D4E File Offset: 0x00002F4E
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00004D56 File Offset: 0x00002F56
		public RestResponse RestResponse { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00004D5F File Offset: 0x00002F5F
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00004D67 File Offset: 0x00002F67
		public int RetryCount { get; private set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00004D70 File Offset: 0x00002F70
		public int TotalHttpAttempts
		{
			get
			{
				return this._currentIterationCount + 1;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00004D7A File Offset: 0x00002F7A
		public bool IsCompletelyFinished
		{
			get
			{
				return !this._willTryAgain && this._resultExamined && this.Request.State == HttpRequestTaskState.Finished;
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00004DA0 File Offset: 0x00002FA0
		public ClientRestSessionTask(RestRequestMessage restRequestMessage, CancellationToken cancellationToken, bool retry = true)
		{
			if (!retry)
			{
				this._maxIterationCount = 0;
			}
			this.CancellationToken = cancellationToken;
			this.RestRequestMessage = restRequestMessage;
			this._taskCompletionSource = new TaskCompletionSource<bool>();
			this._sw = new Stopwatch();
			this._messageName = this.RestRequestMessage.TypeName;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00004E46 File Offset: 0x00003046
		public void SetRequestData(byte[] userCertificate, string address, IHttpDriver networkClient)
		{
			this.RestRequestMessage.UserCertificate = userCertificate;
			this._requestAddress = address;
			this._postData = this.RestRequestMessage.SerializeAsJson();
			this._networkClient = networkClient;
			this.CreateAndSetRequest();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00004E7C File Offset: 0x0000307C
		private void DetermineNextTry()
		{
			if (this._sw.ElapsedMilliseconds >= (long)ClientRestSessionTask.RequestRetryTimeout)
			{
				this._willTryAgain = false;
				Debug.Print(string.Format("[Resilience][Transport] {0} — issuing transport retry {1}/{2}.", this._messageName, this._currentIterationCount, this._maxIterationCount), 0, Debug.DebugColor.White, 17592186044416UL);
				this.CreateAndSetRequest();
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00004EE0 File Offset: 0x000030E0
		private static string GetCode(WebException webException)
		{
			if (webException.Response != null && webException.Response is HttpWebResponse)
			{
				return ((HttpWebResponse)webException.Response).StatusCode.ToString();
			}
			return "NoCode";
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00004F28 File Offset: 0x00003128
		private void ExamineResult()
		{
			if (!this.Request.Successful)
			{
				Exception exception = this.Request.Exception;
				string text = ((exception != null) ? exception.GetType().Name : null) ?? "null";
				if (this.Request.Exception != null && this.RetryableExceptions.Any<Type>((Type e) => e == this.Request.Exception.GetType()))
				{
					if (this._currentIterationCount < this._maxIterationCount)
					{
						this._sw.Restart();
						this._willTryAgain = true;
						this._currentIterationCount++;
						Debug.Print(string.Format("[Resilience][Transport] {0} failed ({1}) — retryable, transport retry {2}/{3} in {4}ms.", new object[]
						{
							this._messageName,
							text,
							this._currentIterationCount,
							this._maxIterationCount,
							ClientRestSessionTask.RequestRetryTimeout
						}), 0, Debug.DebugColor.White, 17592186044416UL);
					}
					else
					{
						this._willTryAgain = false;
						Debug.Print(string.Format("[Resilience][Transport] {0} — transport retries exhausted ({1}/{2}), surfacing to Layer 2.", this._messageName, this._maxIterationCount, this._maxIterationCount), 0, Debug.DebugColor.White, 17592186044416UL);
					}
				}
				else
				{
					this._willTryAgain = false;
					Debug.Print(string.Concat(new string[] { "[Resilience][Transport] ", this._messageName, " failed (", text, ") — non-retryable exception type, surfacing to Layer 2." }), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				if (this.Request.Exception != null)
				{
					this.PrintExceptions(this.Request.Exception);
				}
			}
			else if (this._currentIterationCount > 0)
			{
				Debug.Print(string.Format("[Resilience][Transport] {0} — succeeded after {1} transport retries.", this._messageName, this._currentIterationCount), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			this._resultExamined = true;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00005108 File Offset: 0x00003308
		public void Tick()
		{
			switch (this.Request.State)
			{
			case HttpRequestTaskState.NotStarted:
				this.Request.Start();
				return;
			case HttpRequestTaskState.Working:
				break;
			case HttpRequestTaskState.Finished:
				if (!this._resultExamined)
				{
					this.ExamineResult();
					return;
				}
				this.DetermineNextTry();
				break;
			default:
				return;
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00005158 File Offset: 0x00003358
		public async Task WaitUntilFinished()
		{
			Debug.Print("ClientRestSessionTask::WaitUntilFinished::" + this._messageName, 0, Debug.DebugColor.White, 17592186044416UL);
			await this._taskCompletionSource.Task;
			Debug.Print("ClientRestSessionTask::WaitUntilFinished::" + this._messageName + " done", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000051A0 File Offset: 0x000033A0
		public void SetFinishedAsSuccessful(RestResponse restResponse)
		{
			Debug.Print("ClientRestSessionTask::SetFinishedAsSuccessful::" + this._messageName, 0, Debug.DebugColor.White, 17592186044416UL);
			this.RestResponse = restResponse;
			this.Successful = true;
			this.Finished = true;
			this._taskCompletionSource.SetResult(true);
			Debug.Print("ClientRestSessionTask::SetFinishedAsSuccessful::" + this._messageName + " done", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00005218 File Offset: 0x00003418
		public void ResetForRetry()
		{
			int retryCount = this.RetryCount;
			this.RetryCount = retryCount + 1;
			this._resultExamined = false;
			this.CreateAndSetRequest();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00005242 File Offset: 0x00003442
		public void SetFinishedAsFailed()
		{
			this.SetFinishedAsFailed(null);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000524C File Offset: 0x0000344C
		public void SetFinishedAsFailed(RestResponse restResponse)
		{
			Debug.Print("ClientRestSessionTask::SetFinishedAsFailed::" + this._messageName, 0, Debug.DebugColor.White, 17592186044416UL);
			this.RestResponse = restResponse;
			this.Successful = false;
			this.Finished = true;
			this._taskCompletionSource.SetResult(true);
			Debug.Print("ClientRestSessionTask::SetFinishedAsFailed:: " + this._messageName + " done", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000052C4 File Offset: 0x000034C4
		private void CreateAndSetRequest()
		{
			RestObjectRequestMessage restObjectRequestMessage;
			bool flag = (restObjectRequestMessage = this.RestRequestMessage as RestObjectRequestMessage) != null && restObjectRequestMessage.MessageType == MessageType.Login;
			string text = this._requestAddress + (flag ? "/Data/Login" : "/Data/ProcessMessage");
			this.Request = this._networkClient.CreateHttpPostRequestTask(text, this._postData, true, this.CancellationToken);
			this._resultExamined = false;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00005330 File Offset: 0x00003530
		private void PrintExceptions(Exception e)
		{
			if (e != null)
			{
				Exception ex = e;
				int num = 0;
				while (ex != null)
				{
					Debug.Print(string.Concat(new object[] { "Exception #", num, ": ", ex.Message, " ||| StackTrace: ", ex.InnerException }), 0, Debug.DebugColor.White, 17592186044416UL);
					ex = ex.InnerException;
					num++;
				}
			}
		}

		// Token: 0x0400007B RID: 123
		private static readonly int RequestRetryTimeout = 1000;

		// Token: 0x0400007C RID: 124
		private readonly Type[] RetryableExceptions = new Type[]
		{
			typeof(HttpRequestException),
			typeof(TaskCanceledException),
			typeof(IOException),
			typeof(SocketException),
			typeof(InvalidOperationException)
		};

		// Token: 0x04000081 RID: 129
		public bool _willTryAgain;

		// Token: 0x04000085 RID: 133
		private string _requestAddress;

		// Token: 0x04000086 RID: 134
		private string _postData;

		// Token: 0x04000087 RID: 135
		private string _messageName;

		// Token: 0x04000088 RID: 136
		private int _maxIterationCount = 5;

		// Token: 0x04000089 RID: 137
		private int _currentIterationCount;

		// Token: 0x0400008A RID: 138
		private Stopwatch _sw;

		// Token: 0x0400008B RID: 139
		private TaskCompletionSource<bool> _taskCompletionSource;

		// Token: 0x0400008C RID: 140
		private IHttpDriver _networkClient;

		// Token: 0x0400008D RID: 141
		private bool _resultExamined;
	}
}
