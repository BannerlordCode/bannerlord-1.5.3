using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200029A RID: 666
	public struct MissionSpawnSettings
	{
		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06002512 RID: 9490 RVA: 0x00086FE9 File Offset: 0x000851E9
		// (set) Token: 0x06002513 RID: 9491 RVA: 0x00086FF1 File Offset: 0x000851F1
		public float GlobalReinforcementInterval
		{
			get
			{
				return this._globalReinforcementInterval;
			}
			set
			{
				this._globalReinforcementInterval = MathF.Max(value, 1f);
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06002514 RID: 9492 RVA: 0x00087004 File Offset: 0x00085204
		// (set) Token: 0x06002515 RID: 9493 RVA: 0x0008700C File Offset: 0x0008520C
		public float DefenderAdvantageFactor
		{
			get
			{
				return this._defenderAdvantageFactor;
			}
			set
			{
				this._defenderAdvantageFactor = MathF.Clamp(value, 0.1f, 10f);
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06002516 RID: 9494 RVA: 0x00087024 File Offset: 0x00085224
		// (set) Token: 0x06002517 RID: 9495 RVA: 0x0008702C File Offset: 0x0008522C
		public float MaximumBattleSideRatio
		{
			get
			{
				return this._maximumBattleSizeRatio;
			}
			set
			{
				this._maximumBattleSizeRatio = MathF.Clamp(value, 0.5f, 0.99f);
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002518 RID: 9496 RVA: 0x00087044 File Offset: 0x00085244
		// (set) Token: 0x06002519 RID: 9497 RVA: 0x0008704C File Offset: 0x0008524C
		public MissionSpawnSettings.InitialSpawnMethod InitialTroopsSpawnMethod { get; private set; }

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x0600251A RID: 9498 RVA: 0x00087055 File Offset: 0x00085255
		// (set) Token: 0x0600251B RID: 9499 RVA: 0x0008705D File Offset: 0x0008525D
		public MissionSpawnSettings.ReinforcementTimingMethod ReinforcementTroopsTimingMethod { get; private set; }

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x0600251C RID: 9500 RVA: 0x00087066 File Offset: 0x00085266
		// (set) Token: 0x0600251D RID: 9501 RVA: 0x0008706E File Offset: 0x0008526E
		public MissionSpawnSettings.ReinforcementSpawnMethod ReinforcementTroopsSpawnMethod { get; private set; }

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x0600251E RID: 9502 RVA: 0x00087077 File Offset: 0x00085277
		// (set) Token: 0x0600251F RID: 9503 RVA: 0x0008707F File Offset: 0x0008527F
		public float ReinforcementBatchPercentage
		{
			get
			{
				return this._reinforcementBatchPercentage;
			}
			set
			{
				this._reinforcementBatchPercentage = MathF.Clamp(value, 0f, 1f);
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002520 RID: 9504 RVA: 0x00087097 File Offset: 0x00085297
		// (set) Token: 0x06002521 RID: 9505 RVA: 0x0008709F File Offset: 0x0008529F
		public float DesiredReinforcementPercentage
		{
			get
			{
				return this._desiredReinforcementPercentage;
			}
			set
			{
				this._desiredReinforcementPercentage = MathF.Clamp(value, 0f, 1f);
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002522 RID: 9506 RVA: 0x000870B7 File Offset: 0x000852B7
		// (set) Token: 0x06002523 RID: 9507 RVA: 0x000870BF File Offset: 0x000852BF
		public float ReinforcementWavePercentage
		{
			get
			{
				return this._reinforcementWavePercentage;
			}
			set
			{
				this._reinforcementWavePercentage = MathF.Clamp(value, 0f, 1f);
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002524 RID: 9508 RVA: 0x000870D7 File Offset: 0x000852D7
		// (set) Token: 0x06002525 RID: 9509 RVA: 0x000870DF File Offset: 0x000852DF
		public int MaximumReinforcementWaveCount
		{
			get
			{
				return this._maximumReinforcementWaveCount;
			}
			set
			{
				this._maximumReinforcementWaveCount = MathF.Max(value, 0);
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06002526 RID: 9510 RVA: 0x000870EE File Offset: 0x000852EE
		// (set) Token: 0x06002527 RID: 9511 RVA: 0x000870F6 File Offset: 0x000852F6
		public float DefenderReinforcementBatchPercentage
		{
			get
			{
				return this._defenderReinforcementBatchPercentage;
			}
			set
			{
				this._defenderReinforcementBatchPercentage = MathF.Clamp(value, 0f, 1f);
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06002528 RID: 9512 RVA: 0x0008710E File Offset: 0x0008530E
		// (set) Token: 0x06002529 RID: 9513 RVA: 0x00087116 File Offset: 0x00085316
		public float AttackerReinforcementBatchPercentage
		{
			get
			{
				return this._attackerReinforcementBatchPercentage;
			}
			set
			{
				this._attackerReinforcementBatchPercentage = MathF.Clamp(value, 0f, 1f);
			}
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x00087130 File Offset: 0x00085330
		public MissionSpawnSettings(MissionSpawnSettings.InitialSpawnMethod initialTroopsSpawnMethod, MissionSpawnSettings.ReinforcementTimingMethod reinforcementTimingMethod, MissionSpawnSettings.ReinforcementSpawnMethod reinforcementTroopsSpawnMethod, float globalReinforcementInterval = 0f, float reinforcementBatchPercentage = 0f, float desiredReinforcementPercentage = 0f, float reinforcementWavePercentage = 0f, int maximumReinforcementWaveCount = 0, float defenderReinforcementBatchPercentage = 0f, float attackerReinforcementBatchPercentage = 0f, float defenderAdvantageFactor = 1f, float maximumBattleSizeRatio = 0.75f)
		{
			this = default(MissionSpawnSettings);
			this.InitialTroopsSpawnMethod = initialTroopsSpawnMethod;
			this.ReinforcementTroopsTimingMethod = reinforcementTimingMethod;
			this.ReinforcementTroopsSpawnMethod = reinforcementTroopsSpawnMethod;
			this.GlobalReinforcementInterval = globalReinforcementInterval;
			this.ReinforcementBatchPercentage = reinforcementBatchPercentage;
			this.DesiredReinforcementPercentage = desiredReinforcementPercentage;
			this.ReinforcementWavePercentage = reinforcementWavePercentage;
			this.MaximumReinforcementWaveCount = maximumReinforcementWaveCount;
			this.DefenderReinforcementBatchPercentage = defenderReinforcementBatchPercentage;
			this.AttackerReinforcementBatchPercentage = attackerReinforcementBatchPercentage;
			this.DefenderAdvantageFactor = defenderAdvantageFactor;
			this.MaximumBattleSideRatio = maximumBattleSizeRatio;
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x000871A4 File Offset: 0x000853A4
		public static MissionSpawnSettings CreateDefaultSpawnSettings()
		{
			return new MissionSpawnSettings(MissionSpawnSettings.InitialSpawnMethod.BattleSizeAllocating, MissionSpawnSettings.ReinforcementTimingMethod.GlobalTimer, MissionSpawnSettings.ReinforcementSpawnMethod.Balanced, 10f, 0.05f, 0.166f, 0f, 0, 0f, 0f, 1f, 0.75f);
		}

		// Token: 0x04000E45 RID: 3653
		public const float MinimumReinforcementInterval = 1f;

		// Token: 0x04000E46 RID: 3654
		public const float MinimumDefenderAdvantageFactor = 0.1f;

		// Token: 0x04000E47 RID: 3655
		public const float MaximumDefenderAdvantageFactor = 10f;

		// Token: 0x04000E48 RID: 3656
		public const float MinimumBattleSizeRatioLimit = 0.5f;

		// Token: 0x04000E49 RID: 3657
		public const float MaximumBattleSizeRatioLimit = 0.99f;

		// Token: 0x04000E4A RID: 3658
		public const float DefaultMaximumBattleSizeRatio = 0.75f;

		// Token: 0x04000E4B RID: 3659
		public const float DefaultDefenderAdvantageFactor = 1f;

		// Token: 0x04000E4F RID: 3663
		private float _globalReinforcementInterval;

		// Token: 0x04000E50 RID: 3664
		private float _defenderAdvantageFactor;

		// Token: 0x04000E51 RID: 3665
		private float _maximumBattleSizeRatio;

		// Token: 0x04000E52 RID: 3666
		private float _reinforcementBatchPercentage;

		// Token: 0x04000E53 RID: 3667
		private float _desiredReinforcementPercentage;

		// Token: 0x04000E54 RID: 3668
		private float _reinforcementWavePercentage;

		// Token: 0x04000E55 RID: 3669
		private int _maximumReinforcementWaveCount;

		// Token: 0x04000E56 RID: 3670
		private float _defenderReinforcementBatchPercentage;

		// Token: 0x04000E57 RID: 3671
		private float _attackerReinforcementBatchPercentage;

		// Token: 0x02000573 RID: 1395
		public enum ReinforcementSpawnMethod
		{
			// Token: 0x04001E9B RID: 7835
			Balanced,
			// Token: 0x04001E9C RID: 7836
			Wave,
			// Token: 0x04001E9D RID: 7837
			Fixed
		}

		// Token: 0x02000574 RID: 1396
		public enum ReinforcementTimingMethod
		{
			// Token: 0x04001E9F RID: 7839
			GlobalTimer,
			// Token: 0x04001EA0 RID: 7840
			CustomTimer
		}

		// Token: 0x02000575 RID: 1397
		public enum InitialSpawnMethod
		{
			// Token: 0x04001EA2 RID: 7842
			BattleSizeAllocating,
			// Token: 0x04001EA3 RID: 7843
			FreeAllocation
		}
	}
}
