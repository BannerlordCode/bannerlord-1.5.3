using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002F5 RID: 757
	[AttributeUsage(AttributeTargets.Class)]
	internal sealed class DefineGameNetworkMessageType : Attribute
	{
		// Token: 0x06002B81 RID: 11137 RVA: 0x000A7F98 File Offset: 0x000A6198
		public DefineGameNetworkMessageType(GameNetworkMessageSendType sendType)
		{
			this.SendType = sendType;
		}

		// Token: 0x040010F6 RID: 4342
		public readonly GameNetworkMessageSendType SendType;
	}
}
