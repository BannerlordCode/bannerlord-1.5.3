using System;
using TaleWorlds.Library;
using TaleWorlds.Localization.Expressions;

namespace TaleWorlds.Localization.TextProcessor
{
	// Token: 0x0200002A RID: 42
	public static class TextGrammarProcessor
	{
		// Token: 0x06000123 RID: 291 RVA: 0x0000617C File Offset: 0x0000437C
		public static string Process(MBTextModel dataRepresentation, TextProcessingContext textContext, TextObject parent = null)
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "Process");
			foreach (TextExpression textExpression in dataRepresentation.RootExpressions)
			{
				if (textExpression != null)
				{
					string text = textExpression.EvaluateString(textContext, parent).ToString();
					mbstringBuilder.Append<string>(text);
				}
				else
				{
					MBTextManager.ThrowLocalizationError("Exp should not be null!");
				}
			}
			return mbstringBuilder.ToStringAndRelease();
		}
	}
}
