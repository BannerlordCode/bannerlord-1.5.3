using System;

namespace SandBox.ViewModelCollection.GameOver
{
	// Token: 0x0200005C RID: 92
	public class StatItem
	{
		// Token: 0x060005AF RID: 1455 RVA: 0x00015660 File Offset: 0x00013860
		public StatItem(string id, string value, StatItem.StatType type = StatItem.StatType.None)
		{
			this.ID = id;
			this.Value = value;
			this.Type = type;
		}

		// Token: 0x040002D8 RID: 728
		public readonly string ID;

		// Token: 0x040002D9 RID: 729
		public readonly string Value;

		// Token: 0x040002DA RID: 730
		public readonly StatItem.StatType Type;

		// Token: 0x020000BD RID: 189
		public enum StatType
		{
			// Token: 0x0400044C RID: 1100
			None,
			// Token: 0x0400044D RID: 1101
			Influence,
			// Token: 0x0400044E RID: 1102
			Issue,
			// Token: 0x0400044F RID: 1103
			Tournament,
			// Token: 0x04000450 RID: 1104
			Gold,
			// Token: 0x04000451 RID: 1105
			Crime,
			// Token: 0x04000452 RID: 1106
			Kill
		}
	}
}
