using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x02000409 RID: 1033
	public abstract class FormationArrangementModel : MBGameModel<FormationArrangementModel>
	{
		// Token: 0x06003896 RID: 14486
		public abstract List<FormationArrangementModel.ArrangementPosition> GetBannerBearerPositions(Formation formation, int maxCount);

		// Token: 0x020006B1 RID: 1713
		public struct ArrangementPosition
		{
			// Token: 0x17000B24 RID: 2852
			// (get) Token: 0x060042B6 RID: 17078 RVA: 0x00100D1E File Offset: 0x000FEF1E
			public bool IsValid
			{
				get
				{
					return this.FileIndex > -1 && this.RankIndex > -1;
				}
			}

			// Token: 0x17000B25 RID: 2853
			// (get) Token: 0x060042B7 RID: 17079 RVA: 0x00100D34 File Offset: 0x000FEF34
			public static FormationArrangementModel.ArrangementPosition Invalid
			{
				get
				{
					return default(FormationArrangementModel.ArrangementPosition);
				}
			}

			// Token: 0x060042B8 RID: 17080 RVA: 0x00100D4A File Offset: 0x000FEF4A
			public ArrangementPosition(int fileIndex = -1, int rankIndex = -1)
			{
				this.FileIndex = fileIndex;
				this.RankIndex = rankIndex;
			}

			// Token: 0x0400237A RID: 9082
			public readonly int FileIndex;

			// Token: 0x0400237B RID: 9083
			public readonly int RankIndex;
		}
	}
}
