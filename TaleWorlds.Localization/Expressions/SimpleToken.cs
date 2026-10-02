using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x02000011 RID: 17
	internal class SimpleToken : TextExpression
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x000046E0 File Offset: 0x000028E0
		public SimpleToken(TokenType tokenType, string value)
		{
			base.RawValue = value;
			this._tokenType = tokenType;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000046F8 File Offset: 0x000028F8
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			switch (this.TokenType)
			{
			case TokenType.FunctionParam:
				return context.GetFunctionParam(base.RawValue).ToStringWithoutClear();
			case TokenType.ParameterWithMarkerOccurance:
				return context.GetParameterWithMarkerOccurance(base.RawValue, parent);
			case TokenType.ParameterWithMultipleMarkerOccurances:
				return context.GetParameterWithMarkerOccurances(base.RawValue, parent);
			default:
				return base.RawValue;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004757 File Offset: 0x00002957
		internal override TokenType TokenType
		{
			get
			{
				return this._tokenType;
			}
		}

		// Token: 0x0400002C RID: 44
		public static readonly SimpleToken SequenceTerminator = new SimpleToken(TokenType.SequenceTerminator, ".");

		// Token: 0x0400002D RID: 45
		private readonly TokenType _tokenType;
	}
}
