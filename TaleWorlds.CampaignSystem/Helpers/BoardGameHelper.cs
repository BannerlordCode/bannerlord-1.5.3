using System;

namespace Helpers
{
	// Token: 0x0200001B RID: 27
	public static class BoardGameHelper
	{
		// Token: 0x0200050D RID: 1293
		public enum AIDifficulty
		{
			// Token: 0x04001622 RID: 5666
			Easy,
			// Token: 0x04001623 RID: 5667
			Normal,
			// Token: 0x04001624 RID: 5668
			Hard,
			// Token: 0x04001625 RID: 5669
			NumTypes
		}

		// Token: 0x0200050E RID: 1294
		public enum BoardGameState
		{
			// Token: 0x04001627 RID: 5671
			None,
			// Token: 0x04001628 RID: 5672
			Win,
			// Token: 0x04001629 RID: 5673
			Loss,
			// Token: 0x0400162A RID: 5674
			Draw
		}
	}
}
