using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x0200000C RID: 12
	internal abstract class TextExpression
	{
		// Token: 0x06000096 RID: 150
		internal abstract string EvaluateString(TextProcessingContext context, TextObject parent);

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000097 RID: 151
		internal abstract TokenType TokenType { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000098 RID: 152 RVA: 0x0000460C File Offset: 0x0000280C
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00004614 File Offset: 0x00002814
		internal string RawValue { get; set; }

		// Token: 0x0600009A RID: 154 RVA: 0x00004620 File Offset: 0x00002820
		internal int EvaluateAsNumber(TextExpression exp, TextProcessingContext context, TextObject parent)
		{
			NumeralExpression numeralExpression = exp as NumeralExpression;
			if (numeralExpression != null)
			{
				return numeralExpression.EvaluateNumber(context, parent);
			}
			int num;
			if (int.TryParse(exp.EvaluateString(context, parent), out num))
			{
				return num;
			}
			if (exp.RawValue == null)
			{
				return 0;
			}
			if (exp.RawValue.Length != 0)
			{
				return 1;
			}
			return 0;
		}
	}
}
