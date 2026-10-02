using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000148 RID: 328
	public struct PlayerDataExperience
	{
		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x0000D657 File Offset: 0x0000B857
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x0000D65F File Offset: 0x0000B85F
		public int Experience { get; private set; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x0000D668 File Offset: 0x0000B868
		public int Level
		{
			get
			{
				return PlayerDataExperience.CalculateLevelFromExperience(this.Experience);
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x0000D675 File Offset: 0x0000B875
		public int ExperienceToNextLevel
		{
			get
			{
				return PlayerDataExperience.CalculateExperienceFromLevel(this.Level + 1) - this.Experience;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x0000D68B File Offset: 0x0000B88B
		public int ExperienceInCurrentLevel
		{
			get
			{
				return this.Experience - PlayerDataExperience.CalculateExperienceFromLevel(this.Level);
			}
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0000D69F File Offset: 0x0000B89F
		static PlayerDataExperience()
		{
			PlayerDataExperience.InitializeXPRequirements();
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0000D6AD File Offset: 0x0000B8AD
		public PlayerDataExperience(int experience)
		{
			this.Experience = experience;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0000D6B8 File Offset: 0x0000B8B8
		public static int CalculateLevelFromExperience(int experience)
		{
			int num = 1;
			int i = 0;
			while (i <= experience)
			{
				i += PlayerDataExperience.ExperienceRequiredForLevel(num + 1);
				if (i <= experience)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0000D6E3 File Offset: 0x0000B8E3
		public static int CalculateExperienceFromLevel(int level)
		{
			if (level == 1)
			{
				return 0;
			}
			if (level < PlayerDataExperience._maxLevelForXPRequirementCalculation)
			{
				return PlayerDataExperience._levelToXP[level];
			}
			return PlayerDataExperience.ExperienceRequiredForLevel(level) + PlayerDataExperience.CalculateExperienceFromLevel(level - 1);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0000D70A File Offset: 0x0000B90A
		public static int ExperienceRequiredForLevel(int level)
		{
			return Convert.ToInt32(Math.Floor(100.0 * Math.Pow((double)(level - 1), 1.03)));
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0000D734 File Offset: 0x0000B934
		private static void InitializeXPRequirements()
		{
			PlayerDataExperience._levelToXP = new int[PlayerDataExperience._maxLevelForXPRequirementCalculation];
			int num = 0;
			for (int i = 2; i < PlayerDataExperience._maxLevelForXPRequirementCalculation; i++)
			{
				num += PlayerDataExperience.ExperienceRequiredForLevel(i);
				PlayerDataExperience._levelToXP[i] = num;
			}
		}

		// Token: 0x040003F6 RID: 1014
		private static int[] _levelToXP;

		// Token: 0x040003F7 RID: 1015
		private static readonly int _maxLevelForXPRequirementCalculation = 30;
	}
}
