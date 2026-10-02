using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x02000024 RID: 36
	internal class StartsWithExpression : TextExpression
	{
		// Token: 0x060000E6 RID: 230 RVA: 0x00005184 File Offset: 0x00003384
		public StartsWithExpression(string identifierName)
		{
			int num = identifierName.IndexOf('(');
			int num2 = identifierName.IndexOf(')');
			this._parameter = identifierName.Remove(num);
			this._functionParams = identifierName.Substring(num + 1, num2 - num - 1).Split(new char[] { ',' });
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x000051DA File Offset: 0x000033DA
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.StartsWith;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000051E0 File Offset: 0x000033E0
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			TextObject functionParamWithoutEvaluate = context.GetFunctionParamWithoutEvaluate(this._parameter);
			ValueTuple<TextObject, bool> qualifiedVariableValue = context.GetQualifiedVariableValue(functionParamWithoutEvaluate.ToStringWithoutClear(), parent);
			TextObject item = qualifiedVariableValue.Item1;
			if (qualifiedVariableValue.Item2)
			{
				foreach (string text in this._functionParams)
				{
					if (item.ToStringWithoutClear().StartsWith(text, StringComparison.InvariantCultureIgnoreCase))
					{
						return text;
					}
				}
			}
			return "";
		}

		// Token: 0x04000057 RID: 87
		private readonly string _parameter;

		// Token: 0x04000058 RID: 88
		private readonly string[] _functionParams;
	}
}
