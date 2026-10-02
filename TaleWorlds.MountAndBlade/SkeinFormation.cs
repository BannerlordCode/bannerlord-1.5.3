using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200014C RID: 332
	public class SkeinFormation : LineFormation
	{
		// Token: 0x06001121 RID: 4385 RVA: 0x00030E6A File Offset: 0x0002F06A
		public SkeinFormation(IFormation owner)
			: base(owner, true)
		{
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x00030E74 File Offset: 0x0002F074
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new SkeinFormation(formation);
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00030E7C File Offset: 0x0002F07C
		protected override Vec2 GetLocalPositionOfUnit(int fileIndex, int rankIndex)
		{
			float num = (float)(base.FileCount - 1) * (base.Interval + base.UnitDiameter);
			Vec2 vec = new Vec2((float)fileIndex * (base.Interval + base.UnitDiameter) - num / 2f, (float)(-(float)rankIndex) * (base.Distance + base.UnitDiameter));
			float offsetOfFile = this.GetOffsetOfFile(fileIndex);
			vec.y -= offsetOfFile;
			return vec;
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00030EE8 File Offset: 0x0002F0E8
		protected override Vec2 GetLocalPositionOfUnitWithAdjustment(int fileIndex, int rankIndex, float distanceBetweenAgentsAdjustment)
		{
			float num = base.Interval + distanceBetweenAgentsAdjustment;
			float num2 = (float)(base.FileCount - 1) * (num + base.UnitDiameter);
			Vec2 vec = new Vec2((float)fileIndex * (num + base.UnitDiameter) - num2 / 2f, (float)(-(float)rankIndex) * (base.Distance + base.UnitDiameter));
			float offsetOfFile = this.GetOffsetOfFile(fileIndex);
			vec.y -= offsetOfFile;
			return vec;
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x00030F54 File Offset: 0x0002F154
		private float GetOffsetOfFile(int fileIndex)
		{
			int num = base.FileCount / 2;
			return (float)MathF.Abs(fileIndex - num) * (base.Interval + base.UnitDiameter) / 2f;
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x00030F88 File Offset: 0x0002F188
		protected override bool TryGetUnitPositionIndexFromLocalPosition(Vec2 localPosition, out int fileIndex, out int rankIndex)
		{
			float num = (float)(base.FileCount - 1) * (base.Interval + base.UnitDiameter);
			fileIndex = MathF.Round((localPosition.x + num / 2f) / (base.Interval + base.UnitDiameter));
			if (fileIndex < 0 || fileIndex >= base.FileCount)
			{
				rankIndex = -1;
				return false;
			}
			float offsetOfFile = this.GetOffsetOfFile(fileIndex);
			localPosition.y += offsetOfFile;
			rankIndex = MathF.Round(-localPosition.y / (base.Distance + base.UnitDiameter));
			if (rankIndex < 0 || rankIndex >= base.RankCount)
			{
				fileIndex = -1;
				return false;
			}
			return true;
		}
	}
}
