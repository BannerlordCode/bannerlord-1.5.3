using System;
using System.Threading.Tasks;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200000E RID: 14
	public interface IClient
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004E RID: 78
		bool IsInCriticalState { get; }

		// Token: 0x0600004F RID: 79
		void HandleMessage(Message message);

		// Token: 0x06000050 RID: 80
		void OnConnected();

		// Token: 0x06000051 RID: 81
		void OnCantConnect();

		// Token: 0x06000052 RID: 82
		void OnDisconnected();

		// Token: 0x06000053 RID: 83
		Task<bool> CheckConnection();

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000054 RID: 84
		ILoginAccessProvider AccessProvider { get; }
	}
}
