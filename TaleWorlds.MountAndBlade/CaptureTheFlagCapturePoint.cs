using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000334 RID: 820
	public class CaptureTheFlagCapturePoint
	{
		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06002E86 RID: 11910 RVA: 0x000B4309 File Offset: 0x000B2509
		// (set) Token: 0x06002E87 RID: 11911 RVA: 0x000B4311 File Offset: 0x000B2511
		public float Progress { get; set; }

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06002E88 RID: 11912 RVA: 0x000B431A File Offset: 0x000B251A
		// (set) Token: 0x06002E89 RID: 11913 RVA: 0x000B4322 File Offset: 0x000B2522
		public CaptureTheFlagFlagDirection Direction { get; set; }

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06002E8A RID: 11914 RVA: 0x000B432B File Offset: 0x000B252B
		// (set) Token: 0x06002E8B RID: 11915 RVA: 0x000B4333 File Offset: 0x000B2533
		public float Speed { get; set; }

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06002E8C RID: 11916 RVA: 0x000B433C File Offset: 0x000B253C
		// (set) Token: 0x06002E8D RID: 11917 RVA: 0x000B4344 File Offset: 0x000B2544
		public MatrixFrame InitialFlagFrame { get; private set; }

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002E8E RID: 11918 RVA: 0x000B434D File Offset: 0x000B254D
		// (set) Token: 0x06002E8F RID: 11919 RVA: 0x000B4355 File Offset: 0x000B2555
		public GameEntity FlagEntity { get; private set; }

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06002E90 RID: 11920 RVA: 0x000B435E File Offset: 0x000B255E
		// (set) Token: 0x06002E91 RID: 11921 RVA: 0x000B4366 File Offset: 0x000B2566
		public SynchedMissionObject FlagHolder { get; private set; }

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06002E92 RID: 11922 RVA: 0x000B436F File Offset: 0x000B256F
		// (set) Token: 0x06002E93 RID: 11923 RVA: 0x000B4377 File Offset: 0x000B2577
		public GameEntity FlagBottomBoundary { get; private set; }

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06002E94 RID: 11924 RVA: 0x000B4380 File Offset: 0x000B2580
		// (set) Token: 0x06002E95 RID: 11925 RVA: 0x000B4388 File Offset: 0x000B2588
		public GameEntity FlagTopBoundary { get; private set; }

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002E96 RID: 11926 RVA: 0x000B4391 File Offset: 0x000B2591
		public BattleSideEnum BattleSide { get; }

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002E97 RID: 11927 RVA: 0x000B4399 File Offset: 0x000B2599
		public int Index { get; }

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06002E98 RID: 11928 RVA: 0x000B43A1 File Offset: 0x000B25A1
		// (set) Token: 0x06002E99 RID: 11929 RVA: 0x000B43A9 File Offset: 0x000B25A9
		public bool UpdateFlag { get; set; }

		// Token: 0x06002E9A RID: 11930 RVA: 0x000B43B4 File Offset: 0x000B25B4
		public CaptureTheFlagCapturePoint(GameEntity flagPole, BattleSideEnum battleSide, int index)
		{
			this.Reset();
			this.BattleSide = battleSide;
			this.Index = index;
			this.FlagHolder = flagPole.CollectChildrenEntitiesWithTag("score_stand").SingleOrDefault<GameEntity>().GetFirstScriptOfType<SynchedMissionObject>();
			this.FlagEntity = GameEntity.CreateFromWeakEntity(this.FlagHolder.GameEntity.GetChildren().Single<WeakGameEntity>((WeakGameEntity q) => q.HasTag("flag")));
			this.FlagHolder.GameEntity.SetEntityFlags(this.FlagHolder.GameEntity.EntityFlags | EntityFlags.NoOcclusionCulling);
			this.FlagEntity.EntityFlags |= EntityFlags.NoOcclusionCulling;
			this.FlagBottomBoundary = flagPole.GetChildren().Single<GameEntity>((GameEntity q) => q.HasTag("flag_raising_bottom"));
			this.FlagTopBoundary = flagPole.GetChildren().Single<GameEntity>((GameEntity q) => q.HasTag("flag_raising_top"));
			MatrixFrame globalFrame = this.FlagHolder.GameEntity.GetGlobalFrame();
			globalFrame.origin.z = this.FlagBottomBoundary.GetGlobalFrame().origin.z;
			this.InitialFlagFrame = globalFrame;
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x000B4517 File Offset: 0x000B2717
		public void Reset()
		{
			this.Progress = 0f;
			this.Direction = CaptureTheFlagFlagDirection.None;
			this.Speed = 0f;
			this.UpdateFlag = false;
		}
	}
}
