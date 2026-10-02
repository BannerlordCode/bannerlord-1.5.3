using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x0200000D RID: 13
	internal class SimpleText : TextExpression
	{
		// Token: 0x0600009C RID: 156 RVA: 0x00004675 File Offset: 0x00002875
		public SimpleText(string value)
		{
			base.RawValue = value;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00004684 File Offset: 0x00002884
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.Text;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004688 File Offset: 0x00002888
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			return base.RawValue;
		}
	}
}
