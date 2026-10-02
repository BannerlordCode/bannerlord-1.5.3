using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x0200001B RID: 27
	internal class FieldExpression : TextExpression
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004B1B File Offset: 0x00002D1B
		public string FieldName
		{
			get
			{
				return base.RawValue;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00004B23 File Offset: 0x00002D23
		public TextExpression InnerExpression
		{
			get
			{
				return this.part2;
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004B2B File Offset: 0x00002D2B
		public FieldExpression(TextExpression innerExpression)
		{
			this._innerExpression = innerExpression;
			base.RawValue = innerExpression.RawValue;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004B46 File Offset: 0x00002D46
		public FieldExpression(TextExpression innerExpression, TextExpression part2)
			: this(innerExpression)
		{
			this.part2 = part2;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004B56 File Offset: 0x00002D56
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			return "";
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004B5D File Offset: 0x00002D5D
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.FieldExpression;
			}
		}

		// Token: 0x04000047 RID: 71
		private TextExpression _innerExpression;

		// Token: 0x04000048 RID: 72
		private TextExpression part2;
	}
}
