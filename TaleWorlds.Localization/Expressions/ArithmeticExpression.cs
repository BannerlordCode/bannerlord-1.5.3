using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x02000019 RID: 25
	internal class ArithmeticExpression : NumeralExpression
	{
		// Token: 0x060000BB RID: 187 RVA: 0x000049D5 File Offset: 0x00002BD5
		public ArithmeticExpression(ArithmeticOperation op, TextExpression exp1, TextExpression exp2)
		{
			this._op = op;
			this._exp1 = exp1;
			this._exp2 = exp2;
			base.RawValue = exp1.RawValue + op + exp2.RawValue;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004A10 File Offset: 0x00002C10
		internal override int EvaluateNumber(TextProcessingContext context, TextObject parent)
		{
			switch (this._op)
			{
			case ArithmeticOperation.Add:
				return base.EvaluateAsNumber(this._exp1, context, parent) + base.EvaluateAsNumber(this._exp2, context, parent);
			case ArithmeticOperation.Subtract:
				return base.EvaluateAsNumber(this._exp1, context, parent) - base.EvaluateAsNumber(this._exp2, context, parent);
			case ArithmeticOperation.Multiply:
				return base.EvaluateAsNumber(this._exp1, context, parent) * base.EvaluateAsNumber(this._exp2, context, parent);
			case ArithmeticOperation.Divide:
				return base.EvaluateAsNumber(this._exp1, context, parent) / base.EvaluateAsNumber(this._exp2, context, parent);
			default:
				return 0;
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004AB8 File Offset: 0x00002CB8
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			return this.EvaluateNumber(context, parent).ToString();
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004AD5 File Offset: 0x00002CD5
		internal override TokenType TokenType
		{
			get
			{
				if (this._op != ArithmeticOperation.Add && this._op != ArithmeticOperation.Subtract)
				{
					return TokenType.ArithmeticProduct;
				}
				return TokenType.ArithmeticSum;
			}
		}

		// Token: 0x04000043 RID: 67
		private readonly ArithmeticOperation _op;

		// Token: 0x04000044 RID: 68
		private readonly TextExpression _exp1;

		// Token: 0x04000045 RID: 69
		private readonly TextExpression _exp2;
	}
}
