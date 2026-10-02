using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200014E RID: 334
	public class WedgeFormation : LineFormation
	{
		// Token: 0x06001147 RID: 4423 RVA: 0x000318DF File Offset: 0x0002FADF
		public WedgeFormation(IFormation owner)
			: base(owner, true)
		{
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x000318E9 File Offset: 0x0002FAE9
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new WedgeFormation(formation);
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x000318F4 File Offset: 0x0002FAF4
		private int GetUnitCountOfRank(int rankIndex)
		{
			int num = rankIndex * 2 * 3 + 3;
			return MathF.Min(base.FileCount, num);
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x00031918 File Offset: 0x0002FB18
		protected override bool IsUnitPositionRestrained(int fileIndex, int rankIndex)
		{
			if (base.IsUnitPositionRestrained(fileIndex, rankIndex))
			{
				return true;
			}
			int unitCountOfRank = this.GetUnitCountOfRank(rankIndex);
			int num = (base.FileCount - unitCountOfRank) / 2;
			return fileIndex < num || fileIndex >= num + unitCountOfRank;
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x00031954 File Offset: 0x0002FB54
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
	}
}
