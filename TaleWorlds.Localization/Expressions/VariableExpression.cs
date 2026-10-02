using System;
using TaleWorlds.Library;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x0200001E RID: 30
	internal class VariableExpression : TextExpression
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00004CCF File Offset: 0x00002ECF
		public string IdentifierName
		{
			get
			{
				return this._identifierName;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004CD7 File Offset: 0x00002ED7
		public VariableExpression(string identifierName, VariableExpression innerExpression)
		{
			base.RawValue = identifierName;
			this._identifierName = identifierName;
			this._innerVariable = innerExpression;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004CF4 File Offset: 0x00002EF4
		internal MultiStatement GetValue(TextProcessingContext context, TextObject parent)
		{
			if (this._innerVariable == null)
			{
				return context.GetVariableValue(this._identifierName, parent);
			}
			MultiStatement value = this._innerVariable.GetValue(context, parent);
			if (value != null && value != null)
			{
				foreach (TextExpression textExpression in value.SubStatements)
				{
					FieldExpression fieldExpression = textExpression as FieldExpression;
					if (fieldExpression != null && fieldExpression.FieldName == this._identifierName)
					{
						if (fieldExpression.InnerExpression is MultiStatement)
						{
							return fieldExpression.InnerExpression as MultiStatement;
						}
						return new MultiStatement(new TextExpression[] { fieldExpression.InnerExpression });
					}
				}
			}
			return null;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004DBC File Offset: 0x00002FBC
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			MultiStatement value = this.GetValue(context, parent);
			if (value != null)
			{
				MBStringBuilder mbstringBuilder = default(MBStringBuilder);
				mbstringBuilder.Initialize(16, "EvaluateString");
				foreach (TextExpression textExpression in value.SubStatements)
				{
					if (textExpression != null)
					{
						mbstringBuilder.Append<string>(textExpression.EvaluateString(context, parent));
					}
				}
				return mbstringBuilder.ToStringAndRelease();
			}
			return "";
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00004E4C File Offset: 0x0000304C
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.Identifier;
			}
		}

		// Token: 0x0400004D RID: 77
		private VariableExpression _innerVariable;

		// Token: 0x0400004E RID: 78
		private string _identifierName;
	}
}
