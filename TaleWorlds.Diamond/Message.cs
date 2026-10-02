using System;
using Newtonsoft.Json;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200001F RID: 31
	[JsonConverter(typeof(MessageJsonConverter))]
	[Serializable]
	public abstract class Message
	{
	}
}
