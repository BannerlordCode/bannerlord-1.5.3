using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000141 RID: 321
	public class CircularFormation : LineFormation
	{
		// Token: 0x06000F74 RID: 3956 RVA: 0x00029ED8 File Offset: 0x000280D8
		public CircularFormation(IFormation owner)
			: base(owner, true, true)
		{
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00029EE3 File Offset: 0x000280E3
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new CircularFormation(formation);
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00029EEC File Offset: 0x000280EC
		private float GetDistanceFromCenterOfRank(int rankIndex)
		{
			float num = this.Radius - (float)rankIndex * (base.Distance + base.UnitDiameter);
			if (num >= 0f)
			{
				return num;
			}
			return 0f;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00029F20 File Offset: 0x00028120
		protected override bool IsDeepenApplicable()
		{
			return this.Radius - (float)base.RankCount * (base.Distance + base.UnitDiameter) >= 0f;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00029F48 File Offset: 0x00028148
		protected override bool IsNarrowApplicable(int amount)
		{
			return ((float)(base.FileCount - 1 - amount) * (base.Interval + base.UnitDiameter) + base.UnitDiameter) / 6.2831855f - (float)base.RankCount * (base.Distance + base.UnitDiameter) >= 0f;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00029F9C File Offset: 0x0002819C
		private int GetUnitCountOfRank(int rankIndex)
		{
			if (rankIndex == 0)
			{
				return base.FileCount;
			}
			float distanceFromCenterOfRank = this.GetDistanceFromCenterOfRank(rankIndex);
			int num = MathF.Floor(6.2831855f * distanceFromCenterOfRank / (base.Interval + base.UnitDiameter));
			return MathF.Max(1, num);
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000F7A RID: 3962 RVA: 0x00029FDD File Offset: 0x000281DD
		// (set) Token: 0x06000F7B RID: 3963 RVA: 0x00029FE8 File Offset: 0x000281E8
		public override float Width
		{
			get
			{
				return this.Diameter;
			}
			set
			{
				float num = 3.1415927f * value;
				this.FormFromCircumference(num);
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000F7C RID: 3964 RVA: 0x0002A006 File Offset: 0x00028206
		public override float Depth
		{
			get
			{
				return this.Diameter;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000F7D RID: 3965 RVA: 0x0002A00E File Offset: 0x0002820E
		private float Diameter
		{
			get
			{
				return 2f * this.Radius;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000F7E RID: 3966 RVA: 0x0002A01C File Offset: 0x0002821C
		private float Radius
		{
			get
			{
				return (base.FlankWidth + base.Interval) / 6.2831855f;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x0002A034 File Offset: 0x00028234
		public override float MinimumWidth
		{
			get
			{
				int unitCountWithOverride = base.GetUnitCountWithOverride();
				int currentMaximumRankCount = this.GetCurrentMaximumRankCount(unitCountWithOverride);
				float num = this.owner.MinimumInterval + base.UnitDiameter;
				float num2 = this.owner.MinimumDistance + base.UnitDiameter;
				return this.GetCircumferenceAux(unitCountWithOverride, currentMaximumRankCount, num, num2) / 3.1415927f;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000F80 RID: 3968 RVA: 0x0002A088 File Offset: 0x00028288
		public override float MaximumWidth
		{
			get
			{
				int unitCountWithOverride = base.GetUnitCountWithOverride();
				float num = this.owner.MaximumInterval + base.UnitDiameter;
				return MathF.Max(0f, (float)unitCountWithOverride * num) / 3.1415927f;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x0002A0C3 File Offset: 0x000282C3
		private int MaxRank
		{
			get
			{
				return MathF.Floor(this.Radius / (base.Distance + base.UnitDiameter));
			}
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x0002A0E0 File Offset: 0x000282E0
		protected override bool IsUnitPositionRestrained(int fileIndex, int rankIndex)
		{
			if (base.IsUnitPositionRestrained(fileIndex, rankIndex))
			{
				return true;
			}
			if (rankIndex > this.MaxRank)
			{
				return true;
			}
			int unitCountOfRank = this.GetUnitCountOfRank(rankIndex);
			int num = (base.FileCount - unitCountOfRank) / 2;
			return fileIndex < num || fileIndex >= num + unitCountOfRank;
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0002A124 File Offset: 0x00028324
		protected override void MakeRestrainedPositionsUnavailable()
		{
			for (int i = 0; i < base.FileCount; i++)
			{
				for (int j = 0; j < base.RankCount; j++)
				{
					if (this.IsUnitPositionRestrained(i, j))
					{
						this.UnitPositionAvailabilities[i, j] = 1;
					}
				}
			}
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0002A16C File Offset: 0x0002836C
		protected override Vec2 GetLocalDirectionOfUnit(int fileIndex, int rankIndex)
		{
			int unitCountOfRank = this.GetUnitCountOfRank(rankIndex);
			int num = (base.FileCount - unitCountOfRank) / 2;
			Vec2 vec = Vec2.FromRotation((float)((fileIndex - num) * 2) * 3.1415927f / (float)unitCountOfRank + 3.1415927f);
			vec.x *= -1f;
			return vec;
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0002A1B8 File Offset: 0x000283B8
		public override Vec2? GetLocalDirectionOfUnitOrDefault(IFormationUnit unit)
		{
			if (unit.FormationFileIndex < 0 || unit.FormationRankIndex < 0)
			{
				return null;
			}
			return new Vec2?(this.GetLocalDirectionOfUnit(unit.FormationFileIndex, unit.FormationRankIndex));
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0002A1F8 File Offset: 0x000283F8
		protected override Vec2 GetLocalPositionOfUnit(int fileIndex, int rankIndex)
		{
			Vec2 vec = new Vec2(0f, -this.Radius);
			Vec2 localDirectionOfUnit = this.GetLocalDirectionOfUnit(fileIndex, rankIndex);
			float distanceFromCenterOfRank = this.GetDistanceFromCenterOfRank(rankIndex);
			return vec + localDirectionOfUnit * distanceFromCenterOfRank;
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0002A233 File Offset: 0x00028433
		protected override Vec2 GetLocalPositionOfUnitWithAdjustment(int fileIndex, int rankIndex, float distanceBetweenAgentsAdjustment)
		{
			return this.GetLocalPositionOfUnit(fileIndex, rankIndex);
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0002A240 File Offset: 0x00028440
		protected override bool TryGetUnitPositionIndexFromLocalPosition(Vec2 localPosition, out int fileIndex, out int rankIndex)
		{
			Vec2 vec = new Vec2(0f, -this.Radius);
			Vec2 vec2 = localPosition - vec;
			float length = vec2.Length;
			rankIndex = MathF.Round((length - this.Radius) / (base.Distance + base.UnitDiameter) * -1f);
			if (rankIndex < 0 || rankIndex >= base.RankCount)
			{
				fileIndex = -1;
				return false;
			}
			if (this.Radius - (float)rankIndex * (base.Distance + base.UnitDiameter) < 0f)
			{
				fileIndex = -1;
				return false;
			}
			int unitCountOfRank = this.GetUnitCountOfRank(rankIndex);
			int num = (base.FileCount - unitCountOfRank) / 2;
			vec2.x *= -1f;
			float num2 = vec2.RotationInRadians;
			num2 -= 3.1415927f;
			if (num2 < 0f)
			{
				num2 += 6.2831855f;
			}
			int num3 = MathF.Round(num2 / 2f / 3.1415927f * (float)unitCountOfRank);
			fileIndex = num3 + num;
			return fileIndex >= 0 && fileIndex < base.FileCount;
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0002A348 File Offset: 0x00028548
		protected int GetCurrentMaximumRankCount(int unitCount)
		{
			int num = 0;
			int i = 0;
			float num2 = base.Interval + base.UnitDiameter;
			float num3 = base.Distance + base.UnitDiameter;
			while (i < unitCount)
			{
				float num4 = (float)num * num3;
				int num5 = (int)(6.2831855f * num4 / num2);
				i += MathF.Max(1, num5);
				num++;
			}
			return MathF.Max(num, 1);
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0002A3A4 File Offset: 0x000285A4
		public float GetCircumferenceFromRankCount(int rankCount)
		{
			int unitCountWithOverride = base.GetUnitCountWithOverride();
			rankCount = MathF.Min(this.GetCurrentMaximumRankCount(unitCountWithOverride), rankCount);
			float num = base.Interval + base.UnitDiameter;
			float num2 = base.Distance + base.UnitDiameter;
			return this.GetCircumferenceAux(unitCountWithOverride, rankCount, num, num2);
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0002A3F0 File Offset: 0x000285F0
		public void FormFromCircumference(float circumference)
		{
			int unitCountWithOverride = base.GetUnitCountWithOverride();
			int currentMaximumRankCount = this.GetCurrentMaximumRankCount(unitCountWithOverride);
			float num = base.Interval + base.UnitDiameter;
			float num2 = base.Distance + base.UnitDiameter;
			float circumferenceAux = this.GetCircumferenceAux(unitCountWithOverride, currentMaximumRankCount, num, num2);
			float num3 = MathF.Max(0f, (float)unitCountWithOverride * num);
			circumference = MBMath.ClampFloat(circumference, circumferenceAux, num3);
			base.FlankWidth = Math.Max(circumference - base.Interval, base.UnitDiameter);
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0002A46C File Offset: 0x0002866C
		protected float GetCircumferenceAux(int unitCount, int rankCount, float radialInterval, float distanceInterval)
		{
			float num = (float)(6.283185307179586 * (double)distanceInterval);
			float num2 = MathF.Max(0f, (float)unitCount * radialInterval);
			float num3;
			int unitCountAux;
			do
			{
				num3 = num2;
				num2 = MathF.Max(0f, num3 - num);
				unitCountAux = CircularFormation.GetUnitCountAux(num2, rankCount, radialInterval, distanceInterval);
			}
			while (unitCountAux > unitCount && num3 > 0f);
			return num3;
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0002A4C0 File Offset: 0x000286C0
		private static int GetUnitCountAux(float circumference, int rankCount, float radialInterval, float distanceInterval)
		{
			int num = 0;
			double num2 = 6.283185307179586 * (double)distanceInterval;
			for (int i = 1; i <= rankCount; i++)
			{
				num += (int)(Math.Max(0.0, (double)circumference - (double)(rankCount - i) * num2) / (double)radialInterval);
			}
			return num;
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0002A507 File Offset: 0x00028707
		protected override void UpdateFrontUnitTypeDelegate()
		{
		}
	}
}
