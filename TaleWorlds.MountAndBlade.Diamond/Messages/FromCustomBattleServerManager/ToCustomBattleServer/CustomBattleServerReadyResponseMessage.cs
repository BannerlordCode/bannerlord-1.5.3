using System;
using System.Runtime.Serialization;
using TaleWorlds.Diamond;

namespace Messages.FromCustomBattleServerManager.ToCustomBattleServer
{
	// Token: 0x02000011 RID: 17
	[MessageDescription("CustomBattleServerManager", "CustomBattleServer", true)]
	[DataContract]
	[Serializable]
	public class CustomBattleServerReadyResponseMessage : LoginResultObject
	{
	}
}
