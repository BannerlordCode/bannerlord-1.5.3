using System;
using TaleWorlds.Library;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x0200001F RID: 31
	internal class MarkerOccuranceTextExpression : TextExpression
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00004E50 File Offset: 0x00003050
		public string IdentifierName
		{
			get
			{
				return this._identifierName;
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004E58 File Offset: 0x00003058
		public MarkerOccuranceTextExpression(string identifierName, VariableExpression innerExpression)
		{
			base.RawValue = identifierName;
			this._identifierName = identifierName;
			this._innerVariable = innerExpression;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004E78 File Offset: 0x00003078
		private string MarkerOccuranceExpression(string identifierName, string text)
		{
			int i = 0;
			int num = 0;
			int num2 = 0;
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "MarkerOccuranceExpression");
			while (i < text.Length)
			{
				if (text[i] != '{')
				{
					if (num == 1 && num2 == 0)
					{
						mbstringBuilder.Append(text[i]);
					}
				}
				else
				{
					string text2 = TextProcessingContext.ReadFirstToken(text, ref i);
					if (TextProcessingContext.IsDeclarationFinalizer(text2))
					{
						num--;
						if (num2 > num)
						{
							num2 = num;
						}
					}
					else if (TextProcessingContext.IsDeclaration(text2))
					{
						string text3 = text2.Substring(1);
						bool flag = num2 == num && string.Compare(identifierName, text3, StringComparison.InvariantCultureIgnoreCase) == 0;
						num++;
						if (flag)
						{
							num2 = num;
						}
					}
				}
				i++;
			}
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004F28 File Offset: 0x00003128
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			MultiStatement value = this._innerVariable.GetValue(context, parent);
			if (value != null)
			{
				foreach (TextExpression textExpression in value.SubStatements)
				{
					if (textExpression.TokenType == TokenType.LanguageMarker && textExpression.RawValue.Substring(2, textExpression.RawValue.Length - 3) == this.IdentifierName)
					{
						return "1";
					}
				}
			}
			return "0";
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00004FC4 File Offset: 0x000031C4
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.MarkerOccuranceExpression;
			}
		}

		// Token: 0x0400004F RID: 79
		private VariableExpression _innerVariable;

		// Token: 0x04000050 RID: 80
		private string _identifierName;
	}
}
