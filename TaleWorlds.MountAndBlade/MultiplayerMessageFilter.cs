using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002F2 RID: 754
	[Flags]
	public enum MultiplayerMessageFilter : ulong
	{
		// Token: 0x040010D6 RID: 4310
		None = 0UL,
		// Token: 0x040010D7 RID: 4311
		Peers = 1UL,
		// Token: 0x040010D8 RID: 4312
		Messaging = 2UL,
		// Token: 0x040010D9 RID: 4313
		Items = 4UL,
		// Token: 0x040010DA RID: 4314
		General = 8UL,
		// Token: 0x040010DB RID: 4315
		Equipment = 16UL,
		// Token: 0x040010DC RID: 4316
		EquipmentDetailed = 32UL,
		// Token: 0x040010DD RID: 4317
		Formations = 64UL,
		// Token: 0x040010DE RID: 4318
		Agents = 128UL,
		// Token: 0x040010DF RID: 4319
		AgentsDetailed = 256UL,
		// Token: 0x040010E0 RID: 4320
		Mission = 512UL,
		// Token: 0x040010E1 RID: 4321
		MissionDetailed = 1024UL,
		// Token: 0x040010E2 RID: 4322
		AgentAnimations = 2048UL,
		// Token: 0x040010E3 RID: 4323
		SiegeWeapons = 4096UL,
		// Token: 0x040010E4 RID: 4324
		MissionObjects = 8192UL,
		// Token: 0x040010E5 RID: 4325
		MissionObjectsDetailed = 16384UL,
		// Token: 0x040010E6 RID: 4326
		SiegeWeaponsDetailed = 32768UL,
		// Token: 0x040010E7 RID: 4327
		Orders = 65536UL,
		// Token: 0x040010E8 RID: 4328
		GameMode = 131072UL,
		// Token: 0x040010E9 RID: 4329
		Administration = 262144UL,
		// Token: 0x040010EA RID: 4330
		Particles = 524288UL,
		// Token: 0x040010EB RID: 4331
		RPC = 1048576UL,
		// Token: 0x040010EC RID: 4332
		All = 4294967295UL,
		// Token: 0x040010ED RID: 4333
		LightLogging = 139913UL,
		// Token: 0x040010EE RID: 4334
		NormalLogging = 1979037UL,
		// Token: 0x040010EF RID: 4335
		AllWithoutDetails = 2044639UL
	}
}
