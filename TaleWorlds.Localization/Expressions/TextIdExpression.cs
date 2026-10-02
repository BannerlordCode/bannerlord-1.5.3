using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x02000010 RID: 16
	internal class TextIdExpression : TextExpression
	{
		// Token: 0x060000A5 RID: 165 RVA: 0x000046C6 File Offset: 0x000028C6
		public TextIdExpression(string innerText)
		{
			base.RawValue = innerText;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000046D5 File Offset: 0x000028D5
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			return "";
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000046DC File Offset: 0x000028DC
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.TextId;
			}
		}
	}
}
