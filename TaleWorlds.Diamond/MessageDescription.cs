using System;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000020 RID: 32
	public class MessageDescription : Attribute
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00002D57 File Offset: 0x00000F57
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00002D5F File Offset: 0x00000F5F
		public string To { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00002D68 File Offset: 0x00000F68
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00002D70 File Offset: 0x00000F70
		public string From { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00002D79 File Offset: 0x00000F79
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00002D81 File Offset: 0x00000F81
		public bool EndSessionOnFail { get; private set; }

		// Token: 0x060000B1 RID: 177 RVA: 0x00002D8A File Offset: 0x00000F8A
		public MessageDescription(string from, string to, bool endSessionOnFail = true)
		{
			this.From = from;
			this.To = to;
			this.EndSessionOnFail = endSessionOnFail;
		}
	}
}
