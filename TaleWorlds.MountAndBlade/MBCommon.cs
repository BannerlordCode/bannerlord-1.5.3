using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C8 RID: 456
	public class MBCommon
	{
		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x000604C5 File Offset: 0x0005E6C5
		// (set) Token: 0x06001B8A RID: 7050 RVA: 0x000604CC File Offset: 0x0005E6CC
		public static MBCommon.GameType CurrentGameType
		{
			get
			{
				return MBCommon._currentGameType;
			}
			set
			{
				MBCommon._currentGameType = value;
				MBAPI.IMBWorld.SetGameType((int)value);
			}
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x000604DF File Offset: 0x0005E6DF
		public static void PauseGameEngine()
		{
			MBCommon.IsPaused = true;
			MBAPI.IMBWorld.PauseGame();
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x000604F1 File Offset: 0x0005E6F1
		public static void UnPauseGameEngine()
		{
			MBCommon.IsPaused = false;
			MBAPI.IMBWorld.UnpauseGame();
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x00060503 File Offset: 0x0005E703
		public static float GetApplicationTime()
		{
			return MBAPI.IMBWorld.GetGlobalTime(MBCommon.TimeType.Application);
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x00060510 File Offset: 0x0005E710
		public static float GetTotalMissionTime()
		{
			return MBAPI.IMBWorld.GetGlobalTime(MBCommon.TimeType.Mission);
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001B8F RID: 7055 RVA: 0x0006051D File Offset: 0x0005E71D
		public static bool IsDebugMode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x00060520 File Offset: 0x0005E720
		public static void FixSkeletons()
		{
			MBAPI.IMBWorld.FixSkeletons();
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001B91 RID: 7057 RVA: 0x0006052C File Offset: 0x0005E72C
		// (set) Token: 0x06001B92 RID: 7058 RVA: 0x00060533 File Offset: 0x0005E733
		public static bool IsPaused { get; private set; }

		// Token: 0x06001B93 RID: 7059 RVA: 0x0006053B File Offset: 0x0005E73B
		public static void CheckResourceModifications()
		{
			MBAPI.IMBWorld.CheckResourceModifications();
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x00060548 File Offset: 0x0005E748
		public static int Hash(int i, object o)
		{
			return ((i * 397) ^ o.GetHashCode()).ToString().GetHashCode();
		}

		// Token: 0x0400091C RID: 2332
		private static MBCommon.GameType _currentGameType;

		// Token: 0x02000511 RID: 1297
		public enum GameType
		{
			// Token: 0x04001D3B RID: 7483
			Single,
			// Token: 0x04001D3C RID: 7484
			MultiClient,
			// Token: 0x04001D3D RID: 7485
			MultiServer,
			// Token: 0x04001D3E RID: 7486
			MultiClientServer,
			// Token: 0x04001D3F RID: 7487
			SingleReplay,
			// Token: 0x04001D40 RID: 7488
			SingleRecord
		}

		// Token: 0x02000512 RID: 1298
		[EngineStruct("rglTimer_type", false, null)]
		public enum TimeType
		{
			// Token: 0x04001D42 RID: 7490
			[CustomEngineStructMemberData("Real_timer")]
			Application,
			// Token: 0x04001D43 RID: 7491
			[CustomEngineStructMemberData("Tactical_timer")]
			Mission
		}
	}
}
