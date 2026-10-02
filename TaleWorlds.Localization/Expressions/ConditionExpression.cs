using System;
using System.Collections.Generic;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x0200001C RID: 28
	internal class ConditionExpression : TextExpression
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x00004B61 File Offset: 0x00002D61
		public ConditionExpression(TextExpression condition, TextExpression part1, TextExpression part2)
		{
			this._conditionExpressions = new TextExpression[] { condition };
			this._resultExpressions = new TextExpression[] { part1, part2 };
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004B8D File Offset: 0x00002D8D
		public ConditionExpression(List<TextExpression> conditionExpressions, List<TextExpression> resultExpressions2)
		{
			this._conditionExpressions = conditionExpressions.ToArray();
			this._resultExpressions = resultExpressions2.ToArray();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004BB0 File Offset: 0x00002DB0
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			bool flag = false;
			int num = 0;
			TextExpression textExpression = null;
			while (!flag && num < this._conditionExpressions.Length)
			{
				TextExpression textExpression2 = this._conditionExpressions[num];
				string text = textExpression2.EvaluateString(context, parent);
				if (text.Length != 0)
				{
					if (textExpression2.TokenType == TokenType.ParameterWithAttribute || textExpression2.TokenType == TokenType.StartsWith)
					{
						flag = !string.IsNullOrEmpty(text);
					}
					else
					{
						flag = base.EvaluateAsNumber(textExpression2, context, parent) != 0;
					}
				}
				if (flag)
				{
					if (num < this._resultExpressions.Length)
					{
						textExpression = this._resultExpressions[num];
					}
				}
				else
				{
					num++;
				}
			}
			if (textExpression == null && num < this._resultExpressions.Length)
			{
				textExpression = this._resultExpressions[num];
			}
			return ((textExpression != null) ? textExpression.EvaluateString(context, parent) : null) ?? "";
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00004C68 File Offset: 0x00002E68
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.ConditionalExpression;
			}
		}

		// Token: 0x04000049 RID: 73
		private TextExpression[] _conditionExpressions;

		// Token: 0x0400004A RID: 74
		private TextExpression[] _resultExpressions;
	}
}
