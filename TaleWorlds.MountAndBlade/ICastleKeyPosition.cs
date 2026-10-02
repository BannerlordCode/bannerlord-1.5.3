using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000351 RID: 849
	public interface ICastleKeyPosition
	{
		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06003016 RID: 12310
		// (set) Token: 0x06003017 RID: 12311
		IPrimarySiegeWeapon AttackerSiegeWeapon { get; set; }

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06003018 RID: 12312
		TacticalPosition MiddlePosition { get; }

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06003019 RID: 12313
		TacticalPosition WaitPosition { get; }

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x0600301A RID: 12314
		WorldFrame MiddleFrame { get; }

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x0600301B RID: 12315
		WorldFrame DefenseWaitFrame { get; }

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x0600301C RID: 12316
		FormationAI.BehaviorSide DefenseSide { get; }

		// Token: 0x0600301D RID: 12317
		Vec3 GetPosition();
	}
}
