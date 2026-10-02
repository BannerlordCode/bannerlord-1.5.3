using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x02000023 RID: 35
	internal class ParameterWithAttributeExpression : TextExpression
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00005101 File Offset: 0x00003301
		public ParameterWithAttributeExpression(string identifierName)
		{
			this._parameter = identifierName.Remove(identifierName.IndexOf('.'));
			this._attribute = identifierName.Substring(identifierName.IndexOf('.'));
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00005131 File Offset: 0x00003331
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.ParameterWithAttribute;
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005138 File Offset: 0x00003338
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			TextObject functionParamWithoutEvaluate = context.GetFunctionParamWithoutEvaluate(this._parameter);
			ValueTuple<TextObject, bool> qualifiedVariableValue = context.GetQualifiedVariableValue(functionParamWithoutEvaluate.ToStringWithoutClear() + this._attribute, parent);
			TextObject item = qualifiedVariableValue.Item1;
			if (qualifiedVariableValue.Item2)
			{
				return item.ToStringWithoutClear();
			}
			return "";
		}

		// Token: 0x04000055 RID: 85
		private readonly string _parameter;

		// Token: 0x04000056 RID: 86
		private readonly string _attribute;
	}
}
