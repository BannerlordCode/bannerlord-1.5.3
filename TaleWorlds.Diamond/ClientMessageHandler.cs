using System;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000007 RID: 7
	// (Invoke) Token: 0x06000030 RID: 48
	public delegate void ClientMessageHandler<TMessage>(TMessage message) where TMessage : Message;
}
