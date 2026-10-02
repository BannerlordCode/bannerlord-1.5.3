using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x02000012 RID: 18
	internal class MultiStatement : TextExpression
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00004772 File Offset: 0x00002972
		public MultiStatement(IEnumerable<TextExpression> subStatements)
		{
			this._subStatements = subStatements.ToMBList<TextExpression>();
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004791 File Offset: 0x00002991
		public MBReadOnlyList<TextExpression> SubStatements
		{
			get
			{
				return this._subStatements;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00004799 File Offset: 0x00002999
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.MultiStatement;
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000479D File Offset: 0x0000299D
		public void AddStatement(TextExpression s2)
		{
			this._subStatements.Add(s2);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000047AC File Offset: 0x000029AC
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "EvaluateString");
			foreach (TextExpression textExpression in this._subStatements)
			{
				if (textExpression != null)
				{
					mbstringBuilder.Append<string>(textExpression.EvaluateString(context, parent));
				}
			}
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x0400002E RID: 46
		private MBList<TextExpression> _subStatements = new MBList<TextExpression>();
	}
}
