using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x02000013 RID: 19
	internal abstract class NumeralExpression : TextExpression
	{
		// Token: 0x060000B1 RID: 177
		internal abstract int EvaluateNumber(TextProcessingContext context, TextObject parent);
	}
}
