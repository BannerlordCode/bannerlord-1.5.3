using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200036D RID: 877
	[DefineSynchedMissionObjectType(typeof(SynchedMissionObject))]
	public struct BaseSynchedMissionObjectReadableRecord
	{
		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06003282 RID: 12930 RVA: 0x000CF589 File Offset: 0x000CD789
		// (set) Token: 0x06003283 RID: 12931 RVA: 0x000CF591 File Offset: 0x000CD791
		public bool SetVisibilityExcludeParents { get; private set; }

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06003284 RID: 12932 RVA: 0x000CF59A File Offset: 0x000CD79A
		// (set) Token: 0x06003285 RID: 12933 RVA: 0x000CF5A2 File Offset: 0x000CD7A2
		public bool SynchTransform { get; private set; }

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06003286 RID: 12934 RVA: 0x000CF5AB File Offset: 0x000CD7AB
		// (set) Token: 0x06003287 RID: 12935 RVA: 0x000CF5B3 File Offset: 0x000CD7B3
		public MatrixFrame GameObjectFrame { get; private set; }

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06003288 RID: 12936 RVA: 0x000CF5BC File Offset: 0x000CD7BC
		// (set) Token: 0x06003289 RID: 12937 RVA: 0x000CF5C4 File Offset: 0x000CD7C4
		public bool SynchronizeFrameOverTime { get; private set; }

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x0600328A RID: 12938 RVA: 0x000CF5CD File Offset: 0x000CD7CD
		// (set) Token: 0x0600328B RID: 12939 RVA: 0x000CF5D5 File Offset: 0x000CD7D5
		public MatrixFrame LastSynchedFrame { get; private set; }

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x0600328C RID: 12940 RVA: 0x000CF5DE File Offset: 0x000CD7DE
		// (set) Token: 0x0600328D RID: 12941 RVA: 0x000CF5E6 File Offset: 0x000CD7E6
		public float Duration { get; private set; }

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x0600328E RID: 12942 RVA: 0x000CF5EF File Offset: 0x000CD7EF
		// (set) Token: 0x0600328F RID: 12943 RVA: 0x000CF5F7 File Offset: 0x000CD7F7
		public bool HasSkeleton { get; private set; }

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06003290 RID: 12944 RVA: 0x000CF600 File Offset: 0x000CD800
		// (set) Token: 0x06003291 RID: 12945 RVA: 0x000CF608 File Offset: 0x000CD808
		public bool SynchAnimation { get; private set; }

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06003292 RID: 12946 RVA: 0x000CF611 File Offset: 0x000CD811
		// (set) Token: 0x06003293 RID: 12947 RVA: 0x000CF619 File Offset: 0x000CD819
		public int AnimationIndex { get; private set; }

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06003294 RID: 12948 RVA: 0x000CF622 File Offset: 0x000CD822
		// (set) Token: 0x06003295 RID: 12949 RVA: 0x000CF62A File Offset: 0x000CD82A
		public float AnimationSpeed { get; private set; }

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06003296 RID: 12950 RVA: 0x000CF633 File Offset: 0x000CD833
		// (set) Token: 0x06003297 RID: 12951 RVA: 0x000CF63B File Offset: 0x000CD83B
		public float AnimationParameter { get; private set; }

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06003298 RID: 12952 RVA: 0x000CF644 File Offset: 0x000CD844
		// (set) Token: 0x06003299 RID: 12953 RVA: 0x000CF64C File Offset: 0x000CD84C
		public bool IsSkeletonAnimationPaused { get; private set; }

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x0600329A RID: 12954 RVA: 0x000CF655 File Offset: 0x000CD855
		// (set) Token: 0x0600329B RID: 12955 RVA: 0x000CF65D File Offset: 0x000CD85D
		public bool SynchColors { get; private set; }

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x0600329C RID: 12956 RVA: 0x000CF666 File Offset: 0x000CD866
		// (set) Token: 0x0600329D RID: 12957 RVA: 0x000CF66E File Offset: 0x000CD86E
		public uint Color { get; private set; }

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x0600329E RID: 12958 RVA: 0x000CF677 File Offset: 0x000CD877
		// (set) Token: 0x0600329F RID: 12959 RVA: 0x000CF67F File Offset: 0x000CD87F
		public uint Color2 { get; private set; }

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x060032A0 RID: 12960 RVA: 0x000CF688 File Offset: 0x000CD888
		// (set) Token: 0x060032A1 RID: 12961 RVA: 0x000CF690 File Offset: 0x000CD890
		public bool IsDisabled { get; private set; }

		// Token: 0x060032A2 RID: 12962 RVA: 0x000CF69C File Offset: 0x000CD89C
		public bool ReadFromNetwork(ref bool bufferReadValid)
		{
			this.SetVisibilityExcludeParents = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			this.SynchTransform = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			if (this.SynchTransform)
			{
				this.GameObjectFrame = GameNetworkMessage.ReadMatrixFrameFromPacket(ref bufferReadValid);
				this.SynchronizeFrameOverTime = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				if (this.SynchronizeFrameOverTime)
				{
					this.LastSynchedFrame = GameNetworkMessage.ReadMatrixFrameFromPacket(ref bufferReadValid);
					this.Duration = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.FlagCapturePointDurationCompressionInfo, ref bufferReadValid);
				}
			}
			this.HasSkeleton = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			if (this.HasSkeleton)
			{
				this.SynchAnimation = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				if (this.SynchAnimation)
				{
					this.AnimationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AnimationIndexCompressionInfo, ref bufferReadValid);
					this.AnimationSpeed = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationSpeedCompressionInfo, ref bufferReadValid);
					this.AnimationParameter = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationProgressCompressionInfo, ref bufferReadValid);
					this.IsSkeletonAnimationPaused = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				}
			}
			this.SynchColors = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			if (this.SynchColors)
			{
				this.Color = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref bufferReadValid);
				this.Color2 = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref bufferReadValid);
			}
			this.IsDisabled = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			return bufferReadValid;
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x000CF7B1 File Offset: 0x000CD9B1
		public void SetSetVisibilityExcludeParents(bool visible)
		{
			this.SetVisibilityExcludeParents = visible;
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x000CF7BC File Offset: 0x000CD9BC
		public static ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> CreateFromNetworkWithTypeIndex(int typeIndex)
		{
			bool flag = true;
			BaseSynchedMissionObjectReadableRecord baseSynchedMissionObjectReadableRecord = default(BaseSynchedMissionObjectReadableRecord);
			baseSynchedMissionObjectReadableRecord.ReadFromNetwork(ref flag);
			ISynchedMissionObjectReadableRecord synchedMissionObjectReadableRecord = null;
			if (typeIndex >= 0)
			{
				synchedMissionObjectReadableRecord = Activator.CreateInstance(GameNetwork.GetSynchedMissionObjectReadableRecordTypeFromIndex(typeIndex)) as ISynchedMissionObjectReadableRecord;
				synchedMissionObjectReadableRecord.ReadFromNetwork(ref flag);
			}
			return new ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord>(baseSynchedMissionObjectReadableRecord, synchedMissionObjectReadableRecord);
		}
	}
}
