using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x0200001A RID: 26
	internal class SimpleExpression : TextExpression
	{
		// Token: 0x060000BF RID: 191 RVA: 0x00004AED File Offset: 0x00002CED
		public SimpleExpression(TextExpression innerExpression)
		{
			this._innerExpression = innerExpression;
			base.RawValue = innerExpression.RawValue;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004B08 File Offset: 0x00002D08
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			return this._innerExpression.EvaluateString(context, parent);
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00004B17 File Offset: 0x00002D17
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.SimpleExpression;
			}
		}

		// Token: 0x04000046 RID: 70
		private TextExpression _innerExpression;
	}
}
