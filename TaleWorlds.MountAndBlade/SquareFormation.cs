using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200014D RID: 333
	public class SquareFormation : LineFormation
	{
		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001127 RID: 4391 RVA: 0x0003102A File Offset: 0x0002F22A
		// (set) Token: 0x06001128 RID: 4392 RVA: 0x00031044 File Offset: 0x0002F244
		public override float Width
		{
			get
			{
				return SquareFormation.GetSideWidthFromUnitCount(this.UnitCountOfOuterSide, base.Interval, base.UnitDiameter);
			}
			set
			{
				this.FormFromBorderSideWidth(value);
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06001129 RID: 4393 RVA: 0x0003105A File Offset: 0x0002F25A
		public override float Depth
		{
			get
			{
				return SquareFormation.GetSideWidthFromUnitCount(this.UnitCountOfOuterSide, base.Interval, base.UnitDiameter);
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x0600112A RID: 4394 RVA: 0x00031074 File Offset: 0x0002F274
		public override float MinimumWidth
		{
			get
			{
				int num;
				int maximumRankCount = SquareFormation.GetMaximumRankCount(base.GetUnitCountWithOverride(), out num);
				return SquareFormation.GetSideWidthFromUnitCount(this.GetUnitsPerSideFromRankCount(maximumRankCount), this.owner.MinimumInterval, base.UnitDiameter);
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x0600112B RID: 4395 RVA: 0x000310AC File Offset: 0x0002F2AC
		public override float MaximumWidth
		{
			get
			{
				return SquareFormation.GetSideWidthFromUnitCount(this.GetUnitsPerSideFromRankCount(1), this.owner.MaximumInterval, base.UnitDiameter);
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x000310CB File Offset: 0x0002F2CB
		private int UnitCountOfOuterSide
		{
			get
			{
				return MathF.Ceiling((float)base.FileCount / 4f) + 1;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x0600112D RID: 4397 RVA: 0x000310E1 File Offset: 0x0002F2E1
		private int MaxRank
		{
			get
			{
				return (this.UnitCountOfOuterSide + 1) / 2;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x000310ED File Offset: 0x0002F2ED
		private new float Distance
		{
			get
			{
				return base.Interval;
			}
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x000310F5 File Offset: 0x0002F2F5
		public SquareFormation(IFormation owner)
			: base(owner, true, true)
		{
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00031100 File Offset: 0x0002F300
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new SquareFormation(formation);
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00031108 File Offset: 0x0002F308
		public override void DeepCopyFrom(IFormationArrangement arrangement)
		{
			base.DeepCopyFrom(arrangement);
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00031114 File Offset: 0x0002F314
		public void FormFromBorderSideWidth(float borderSideWidth)
		{
			int num = MathF.Max(1, (int)((borderSideWidth - base.UnitDiameter) / (base.Interval + base.UnitDiameter) + 1E-05f)) + 1;
			this.FormFromBorderUnitCountPerSide(num);
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x0003114E File Offset: 0x0002F34E
		public void FormFromBorderUnitCountPerSide(int unitCountPerSide)
		{
			if (unitCountPerSide == 1)
			{
				base.FlankWidth = base.UnitDiameter;
				return;
			}
			base.FlankWidth = (float)(4 * (unitCountPerSide - 1) - 1) * (base.Interval + base.UnitDiameter) + base.UnitDiameter;
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x00031184 File Offset: 0x0002F384
		public int GetUnitsPerSideFromRankCount(int rankCount)
		{
			int unitCountWithOverride = base.GetUnitCountWithOverride();
			int num;
			rankCount = MathF.Min(SquareFormation.GetMaximumRankCount(unitCountWithOverride, out num), rankCount);
			float num2 = (float)unitCountWithOverride / (4f * (float)rankCount) + (float)rankCount;
			int num3 = MathF.Ceiling(num2);
			int num4 = MathF.Round(num2);
			if (num4 < num3 && num4 * num4 == unitCountWithOverride)
			{
				num3 = num4;
			}
			if (num3 == 0)
			{
				num3 = 1;
			}
			return num3;
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x000311D8 File Offset: 0x0002F3D8
		protected static int GetMaximumRankCount(int unitCount, out int minimumFlankCount)
		{
			int num = (int)MathF.Sqrt((float)unitCount);
			if (num * num != unitCount)
			{
				num++;
			}
			minimumFlankCount = num;
			return MathF.Max(1, (num + 1) / 2);
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00031208 File Offset: 0x0002F408
		public void FormFromRankCount(int rankCount)
		{
			int unitsPerSideFromRankCount = this.GetUnitsPerSideFromRankCount(rankCount);
			this.FormFromBorderUnitCountPerSide(unitsPerSideFromRankCount);
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00031224 File Offset: 0x0002F424
		private SquareFormation.Side GetSideOfUnitPosition(int fileIndex)
		{
			return (SquareFormation.Side)(fileIndex / (this.UnitCountOfOuterSide - 1));
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00031230 File Offset: 0x0002F430
		private SquareFormation.Side? GetSideOfUnitPosition(int fileIndex, int rankIndex)
		{
			SquareFormation.Side sideOfUnitPosition = this.GetSideOfUnitPosition(fileIndex);
			if (rankIndex == 0)
			{
				return new SquareFormation.Side?(sideOfUnitPosition);
			}
			int num = this.UnitCountOfOuterSide - 2 * rankIndex;
			if (num == 1 && sideOfUnitPosition != SquareFormation.Side.Front)
			{
				return null;
			}
			int num2 = fileIndex % (this.UnitCountOfOuterSide - 1);
			int num3 = this.UnitCountOfOuterSide - num;
			num3 /= 2;
			if (num2 >= num3 && this.UnitCountOfOuterSide - num2 - 1 > num3)
			{
				return new SquareFormation.Side?(sideOfUnitPosition);
			}
			return null;
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x000312AC File Offset: 0x0002F4AC
		private Vec2 GetLocalPositionOfUnitAux(int fileIndex, int rankIndex, float usedInterval)
		{
			if (this.UnitCountOfOuterSide == 1)
			{
				return Vec2.Zero;
			}
			SquareFormation.Side sideOfUnitPosition = this.GetSideOfUnitPosition(fileIndex);
			float num = (float)(this.UnitCountOfOuterSide - 1) * (usedInterval + base.UnitDiameter);
			float num2 = (float)(fileIndex % (this.UnitCountOfOuterSide - 1)) * (usedInterval + base.UnitDiameter);
			float num3 = (float)rankIndex * (this.Distance + base.UnitDiameter);
			Vec2 vec;
			switch (sideOfUnitPosition)
			{
			case SquareFormation.Side.Front:
				vec = new Vec2(-num / 2f, 0f);
				vec += new Vec2(num2, -num3);
				break;
			case SquareFormation.Side.Right:
				vec = new Vec2(num / 2f, 0f);
				vec += new Vec2(-num3, -num2);
				break;
			case SquareFormation.Side.Rear:
				vec = new Vec2(num / 2f, -num);
				vec += new Vec2(-num2, num3);
				break;
			case SquareFormation.Side.Left:
				vec = new Vec2(-num / 2f, -num);
				vec += new Vec2(num3, num2);
				break;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Formation\\SquareFormation.cs", "GetLocalPositionOfUnitAux", 369);
				vec = Vec2.Zero;
				break;
			}
			return vec;
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x000313E0 File Offset: 0x0002F5E0
		protected override Vec2 GetLocalPositionOfUnit(int fileIndex, int rankIndex)
		{
			int num = this.ShiftFileIndex(fileIndex);
			return this.GetLocalPositionOfUnitAux(num, rankIndex, base.Interval);
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00031404 File Offset: 0x0002F604
		protected override Vec2 GetLocalPositionOfUnitWithAdjustment(int fileIndex, int rankIndex, float distanceBetweenAgentsAdjustment)
		{
			int num = this.ShiftFileIndex(fileIndex);
			return this.GetLocalPositionOfUnitAux(num, rankIndex, base.Interval + distanceBetweenAgentsAdjustment);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0003142C File Offset: 0x0002F62C
		protected override Vec2 GetLocalDirectionOfUnit(int fileIndex, int rankIndex)
		{
			int num = this.ShiftFileIndex(fileIndex);
			switch (this.GetSideOfUnitPosition(num))
			{
			case SquareFormation.Side.Front:
				return Vec2.Forward;
			case SquareFormation.Side.Right:
				return Vec2.Side;
			case SquareFormation.Side.Rear:
				return -Vec2.Forward;
			case SquareFormation.Side.Left:
				return -Vec2.Side;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Formation\\SquareFormation.cs", "GetLocalDirectionOfUnit", 448);
				return Vec2.Forward;
			}
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x000314A4 File Offset: 0x0002F6A4
		public override Vec2? GetLocalDirectionOfUnitOrDefault(IFormationUnit unit)
		{
			if (unit.FormationFileIndex < 0 || unit.FormationRankIndex < 0)
			{
				return null;
			}
			return new Vec2?(this.GetLocalDirectionOfUnit(unit.FormationFileIndex, unit.FormationRankIndex));
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x000314E4 File Offset: 0x0002F6E4
		protected override bool IsUnitPositionRestrained(int fileIndex, int rankIndex)
		{
			if (base.IsUnitPositionRestrained(fileIndex, rankIndex))
			{
				return true;
			}
			if (rankIndex >= this.MaxRank)
			{
				return true;
			}
			int num = this.ShiftFileIndex(fileIndex);
			return this.GetSideOfUnitPosition(num, rankIndex) == null;
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x00031524 File Offset: 0x0002F724
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

		// Token: 0x06001140 RID: 4416 RVA: 0x0003156C File Offset: 0x0002F76C
		private SquareFormation.Side GetSideOfLocalPosition(Vec2 localPosition)
		{
			float num = (float)(this.UnitCountOfOuterSide - 1) * (base.Interval + base.UnitDiameter);
			Vec2 vec = new Vec2(0f, -num / 2f);
			Vec2 vec2 = localPosition - vec;
			vec2.y *= (base.Interval + base.UnitDiameter) / (this.Distance + base.UnitDiameter);
			float num2 = vec2.RotationInRadians;
			if (num2 < 0f)
			{
				num2 += 6.2831855f;
			}
			if (num2 <= 0.7863982f || num2 > 5.4987874f)
			{
				return SquareFormation.Side.Front;
			}
			if (num2 <= 2.3571944f)
			{
				return SquareFormation.Side.Left;
			}
			if (num2 <= 3.927991f)
			{
				return SquareFormation.Side.Rear;
			}
			return SquareFormation.Side.Right;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00031614 File Offset: 0x0002F814
		protected override bool TryGetUnitPositionIndexFromLocalPosition(Vec2 localPosition, out int fileIndex, out int rankIndex)
		{
			SquareFormation.Side sideOfLocalPosition = this.GetSideOfLocalPosition(localPosition);
			float num = (float)(this.UnitCountOfOuterSide - 1) * (base.Interval + base.UnitDiameter);
			float num2;
			float num3;
			switch (sideOfLocalPosition)
			{
			case SquareFormation.Side.Front:
			{
				Vec2 vec = localPosition - new Vec2(-num / 2f, 0f);
				num2 = vec.x;
				num3 = -vec.y;
				break;
			}
			case SquareFormation.Side.Right:
			{
				Vec2 vec2 = localPosition - new Vec2(num / 2f, 0f);
				num2 = -vec2.y;
				num3 = -vec2.x;
				break;
			}
			case SquareFormation.Side.Rear:
			{
				Vec2 vec3 = localPosition - new Vec2(num / 2f, -num);
				num2 = -vec3.x;
				num3 = vec3.y;
				break;
			}
			case SquareFormation.Side.Left:
			{
				Vec2 vec4 = localPosition - new Vec2(-num / 2f, -num);
				num2 = vec4.y;
				num3 = vec4.x;
				break;
			}
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Formation\\SquareFormation.cs", "TryGetUnitPositionIndexFromLocalPosition", 575);
				num2 = 0f;
				num3 = 0f;
				break;
			}
			rankIndex = MathF.Round(num3 / (this.Distance + base.UnitDiameter));
			if (rankIndex < 0 || rankIndex >= base.RankCount || rankIndex >= this.MaxRank)
			{
				fileIndex = -1;
				return false;
			}
			int num4 = MathF.Round(num2 / (base.Interval + base.UnitDiameter));
			if (num4 >= this.UnitCountOfOuterSide - 1)
			{
				fileIndex = 1;
				return false;
			}
			int num5 = num4 + (this.UnitCountOfOuterSide - 1) * (int)sideOfLocalPosition;
			fileIndex = this.UnshiftFileIndex(num5);
			return fileIndex >= 0 && fileIndex < base.FileCount;
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x000317A4 File Offset: 0x0002F9A4
		private int ShiftFileIndex(int fileIndex)
		{
			int num = this.UnitCountOfOuterSide + this.UnitCountOfOuterSide / 2 - 2;
			int num2 = fileIndex - num;
			if (num2 < 0)
			{
				num2 += (this.UnitCountOfOuterSide - 1) * 4;
			}
			return num2;
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x000317DC File Offset: 0x0002F9DC
		private int UnshiftFileIndex(int shiftedFileIndex)
		{
			int num = this.UnitCountOfOuterSide + this.UnitCountOfOuterSide / 2 - 2;
			int num2 = shiftedFileIndex + num;
			if (num2 >= (this.UnitCountOfOuterSide - 1) * 4)
			{
				num2 -= (this.UnitCountOfOuterSide - 1) * 4;
			}
			return num2;
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x0003181A File Offset: 0x0002FA1A
		protected static float GetSideWidthFromUnitCount(int sideUnitCount, float interval, float unitDiameter)
		{
			if (sideUnitCount > 0)
			{
				return (float)(sideUnitCount - 1) * (interval + unitDiameter) + unitDiameter;
			}
			return 0f;
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00031830 File Offset: 0x0002FA30
		public override void TurnBackwards()
		{
			int num = base.FileCount / 2;
			for (int i = 0; i <= base.FileCount / 2; i++)
			{
				for (int j = 0; j < base.RankCount; j++)
				{
					int num2 = i + num;
					if (num2 < base.FileCount)
					{
						IFormationUnit unitAt = base.GetUnitAt(i, j);
						IFormationUnit unitAt2 = base.GetUnitAt(num2, j);
						if (unitAt != unitAt2)
						{
							if (unitAt != null && unitAt2 != null)
							{
								base.SwitchUnitLocations(unitAt, unitAt2);
							}
							else if (unitAt != null)
							{
								if (base.IsUnitPositionAvailable(num2, j))
								{
									base.RelocateUnit(unitAt, num2, j);
								}
							}
							else if (unitAt2 != null && base.IsUnitPositionAvailable(i, j))
							{
								base.RelocateUnit(unitAt2, i, j);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x000318DD File Offset: 0x0002FADD
		protected override void UpdateFrontUnitTypeDelegate()
		{
		}

		// Token: 0x0200046D RID: 1133
		private enum Side
		{
			// Token: 0x04001A92 RID: 6802
			Front,
			// Token: 0x04001A93 RID: 6803
			Right,
			// Token: 0x04001A94 RID: 6804
			Rear,
			// Token: 0x04001A95 RID: 6805
			Left
		}
	}
}
