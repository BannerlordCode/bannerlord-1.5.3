using System;
using TaleWorlds.Library;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x02000020 RID: 32
	internal class ArrayReference : TextExpression
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00004FC8 File Offset: 0x000031C8
		public ArrayReference(string rawValue, TextExpression indexExp)
		{
			base.RawValue = rawValue;
			this._indexExp = indexExp;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004FE0 File Offset: 0x000031E0
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			int num = base.EvaluateAsNumber(this._indexExp, context, parent);
			MultiStatement arrayAccess = context.GetArrayAccess(base.RawValue, num);
			if (arrayAccess != null)
			{
				MBStringBuilder mbstringBuilder = default(MBStringBuilder);
				mbstringBuilder.Initialize(16, "EvaluateString");
				foreach (TextExpression textExpression in arrayAccess.SubStatements)
				{
					mbstringBuilder.Append<string>(textExpression.EvaluateString(context, parent));
				}
				return mbstringBuilder.ToStringAndRelease();
			}
			return "";
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00005084 File Offset: 0x00003284
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.ArrayAccess;
			}
		}

		// Token: 0x04000051 RID: 81
		private TextExpression _indexExp;
	}
}
