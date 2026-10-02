using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x0200000F RID: 15
	internal class LangaugeMarkerExpression : TextExpression
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x000046AB File Offset: 0x000028AB
		public LangaugeMarkerExpression(string innerText)
		{
			base.RawValue = innerText;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000046BA File Offset: 0x000028BA
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			return base.RawValue;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000046C2 File Offset: 0x000028C2
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.LanguageMarker;
			}
		}
	}
}
