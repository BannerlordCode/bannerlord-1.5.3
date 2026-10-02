using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x02000017 RID: 23
	internal class ParanthesisExpression : TextExpression
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00004830 File Offset: 0x00002A30
		public ParanthesisExpression(TextExpression innerExpression)
		{
			this._innerExp = innerExpression;
			base.RawValue = "(" + innerExpression.RawValue + ")";
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000485A File Offset: 0x00002A5A
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			return this._innerExp.EvaluateString(context, parent);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004869 File Offset: 0x00002A69
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.ParenthesisExpression;
			}
		}

		// Token: 0x0400003F RID: 63
		private readonly TextExpression _innerExp;
	}
}
