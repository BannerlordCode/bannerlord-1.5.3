using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002F4 RID: 756
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DefineGameNetworkMessageTypeForMod : Attribute
	{
		// Token: 0x06002B80 RID: 11136 RVA: 0x000A7F89 File Offset: 0x000A6189
		public DefineGameNetworkMessageTypeForMod(GameNetworkMessageSendType sendType)
		{
			this.SendType = sendType;
		}

		// Token: 0x040010F5 RID: 4341
		public readonly GameNetworkMessageSendType SendType;
	}
}
