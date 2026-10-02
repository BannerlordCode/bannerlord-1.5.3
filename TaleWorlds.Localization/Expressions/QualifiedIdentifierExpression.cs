using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x02000022 RID: 34
	internal class QualifiedIdentifierExpression : TextExpression
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000050CD File Offset: 0x000032CD
		public string IdentifierName
		{
			get
			{
				return this._identifierName;
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000050D5 File Offset: 0x000032D5
		public QualifiedIdentifierExpression(string identifierName)
		{
			this._identifierName = identifierName;
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000050E4 File Offset: 0x000032E4
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.QualifiedIdentifier;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000050E8 File Offset: 0x000032E8
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			return context.GetQualifiedVariableValue(this._identifierName, parent).Item1.ToStringWithoutClear();
		}

		// Token: 0x04000054 RID: 84
		private readonly string _identifierName;
	}
}
