using System;
using System.Threading.Tasks;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000013 RID: 19
	public interface IClientSession
	{
		// Token: 0x06000065 RID: 101
		void Connect();

		// Token: 0x06000066 RID: 102
		void Disconnect();

		// Token: 0x06000067 RID: 103
		void Tick();

		// Token: 0x06000068 RID: 104
		Task<LoginResult> Login(LoginMessage message);

		// Token: 0x06000069 RID: 105
		void SendMessage(Message message);

		// Token: 0x0600006A RID: 106
		Task<CallResult> CallFunction<T>(Message message) where T : FunctionResult;

		// Token: 0x0600006B RID: 107
		Task<bool> CheckConnection();

		// Token: 0x17000015 RID: 21
		// (set) Token: 0x0600006C RID: 108
		int AliveCheckInterval { set; }

		// Token: 0x17000016 RID: 22
		// (set) Token: 0x0600006D RID: 109
		int MaxConsecutiveFailuresBeforeDisconnect { set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600006E RID: 110
		// (set) Token: 0x0600006F RID: 111
		string Address { get; set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000070 RID: 112
		// (remove) Token: 0x06000071 RID: 113
		event MessageHandledDelegate MessageReceived;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000072 RID: 114
		// (remove) Token: 0x06000073 RID: 115
		event ConnectedDelegate Connected;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000074 RID: 116
		// (remove) Token: 0x06000075 RID: 117
		event DisconnectedDelegate Disconnected;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000076 RID: 118
		// (remove) Token: 0x06000077 RID: 119
		event OnCantConnectDelegate ConnectionFailed;
	}
}
