using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x02000018 RID: 24
	internal class ComparisonExpression : NumeralExpression
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x0000486D File Offset: 0x00002A6D
		public ComparisonExpression(ComparisonOperation op, TextExpression exp1, TextExpression exp2)
		{
			this._op = op;
			this._exp1 = exp1;
			this._exp2 = exp2;
			base.RawValue = exp1.RawValue + op + exp2.RawValue;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000048A8 File Offset: 0x00002AA8
		internal bool EvaluateBoolean(TextProcessingContext context, TextObject parent)
		{
			switch (this._op)
			{
			case ComparisonOperation.Equals:
				return base.EvaluateAsNumber(this._exp1, context, parent) == base.EvaluateAsNumber(this._exp2, context, parent);
			case ComparisonOperation.NotEquals:
				return base.EvaluateAsNumber(this._exp1, context, parent) != base.EvaluateAsNumber(this._exp2, context, parent);
			case ComparisonOperation.GreaterThan:
				return base.EvaluateAsNumber(this._exp1, context, parent) > base.EvaluateAsNumber(this._exp2, context, parent);
			case ComparisonOperation.GreaterOrEqual:
				return base.EvaluateAsNumber(this._exp1, context, parent) >= base.EvaluateAsNumber(this._exp2, context, parent);
			case ComparisonOperation.LessThan:
				return base.EvaluateAsNumber(this._exp1, context, parent) < base.EvaluateAsNumber(this._exp2, context, parent);
			case ComparisonOperation.LessOrEqual:
				return base.EvaluateAsNumber(this._exp1, context, parent) <= base.EvaluateAsNumber(this._exp2, context, parent);
			default:
				return false;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000049A3 File Offset: 0x00002BA3
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.ComparisonExpression;
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000049A7 File Offset: 0x00002BA7
		internal override int EvaluateNumber(TextProcessingContext context, TextObject parent)
		{
			if (!this.EvaluateBoolean(context, parent))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000049B8 File Offset: 0x00002BB8
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			return this.EvaluateNumber(context, parent).ToString();
		}

		// Token: 0x04000040 RID: 64
		private readonly ComparisonOperation _op;

		// Token: 0x04000041 RID: 65
		private readonly TextExpression _exp1;

		// Token: 0x04000042 RID: 66
		private readonly TextExpression _exp2;
	}
}
