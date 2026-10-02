using System;
using System.Collections.Generic;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x0200001D RID: 29
	internal class SelectionExpression : TextExpression
	{
		// Token: 0x060000CC RID: 204 RVA: 0x00004C6C File Offset: 0x00002E6C
		public SelectionExpression(TextExpression selection, List<TextExpression> selectionExpressions)
		{
			this._selection = selection;
			this._selectionExpressions = selectionExpressions;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004C84 File Offset: 0x00002E84
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			int num = base.EvaluateAsNumber(this._selection, context, parent);
			if (num >= 0 && num < this._selectionExpressions.Count)
			{
				return this._selectionExpressions[num].EvaluateString(context, parent);
			}
			return "";
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00004CCB File Offset: 0x00002ECB
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.SelectionExpression;
			}
		}

		// Token: 0x0400004B RID: 75
		private TextExpression _selection;

		// Token: 0x0400004C RID: 76
		private List<TextExpression> _selectionExpressions;
	}
}
