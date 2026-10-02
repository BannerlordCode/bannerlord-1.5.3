using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x02000021 RID: 33
	internal class FunctionCall : TextExpression
	{
		// Token: 0x060000DC RID: 220 RVA: 0x00005088 File Offset: 0x00003288
		public FunctionCall(string functionName, IEnumerable<TextExpression> functionParams)
		{
			this._functionName = functionName;
			this._functionParams = functionParams.ToList<TextExpression>();
			base.RawValue = this._functionName;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000050AF File Offset: 0x000032AF
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			return context.CallFunction(this._functionName, this._functionParams, parent).ToStringWithoutClear();
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000050C9 File Offset: 0x000032C9
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.FunctionCall;
			}
		}

		// Token: 0x04000052 RID: 82
		private string _functionName;

		// Token: 0x04000053 RID: 83
		private List<TextExpression> _functionParams;
	}
}
