using System;

namespace TaleWorlds.Localization.TextProcessor
{
	// Token: 0x0200002E RID: 46
	internal enum TokenType
	{
		// Token: 0x0400006B RID: 107
		NotDefined,
		// Token: 0x0400006C RID: 108
		And,
		// Token: 0x0400006D RID: 109
		Or,
		// Token: 0x0400006E RID: 110
		Not,
		// Token: 0x0400006F RID: 111
		Equals,
		// Token: 0x04000070 RID: 112
		NotEquals,
		// Token: 0x04000071 RID: 113
		GreaterThan,
		// Token: 0x04000072 RID: 114
		LessThan,
		// Token: 0x04000073 RID: 115
		GreaterOrEqual,
		// Token: 0x04000074 RID: 116
		LessOrEqual,
		// Token: 0x04000075 RID: 117
		Comma,
		// Token: 0x04000076 RID: 118
		OpenBraces,
		// Token: 0x04000077 RID: 119
		CloseBraces,
		// Token: 0x04000078 RID: 120
		OpenParenthesis,
		// Token: 0x04000079 RID: 121
		CloseParenthesis,
		// Token: 0x0400007A RID: 122
		OpenBrackets,
		// Token: 0x0400007B RID: 123
		CloseBrackets,
		// Token: 0x0400007C RID: 124
		Number,
		// Token: 0x0400007D RID: 125
		Identifier,
		// Token: 0x0400007E RID: 126
		VariableExpression,
		// Token: 0x0400007F RID: 127
		MarkerOccuranceIdentifier,
		// Token: 0x04000080 RID: 128
		Match,
		// Token: 0x04000081 RID: 129
		ConditionSeperator,
		// Token: 0x04000082 RID: 130
		ConditionFollowUp,
		// Token: 0x04000083 RID: 131
		Seperator,
		// Token: 0x04000084 RID: 132
		ConditionStarter,
		// Token: 0x04000085 RID: 133
		ConditionFinalizer,
		// Token: 0x04000086 RID: 134
		SelectionSeperator,
		// Token: 0x04000087 RID: 135
		SelectionStarter,
		// Token: 0x04000088 RID: 136
		SelectionFinalizer,
		// Token: 0x04000089 RID: 137
		FieldStarter,
		// Token: 0x0400008A RID: 138
		FieldFinalizer,
		// Token: 0x0400008B RID: 139
		SequenceTerminator,
		// Token: 0x0400008C RID: 140
		Text,
		// Token: 0x0400008D RID: 141
		LanguageMarker,
		// Token: 0x0400008E RID: 142
		UnrecognizedTokenError,
		// Token: 0x0400008F RID: 143
		Plus,
		// Token: 0x04000090 RID: 144
		Minus,
		// Token: 0x04000091 RID: 145
		Multiply,
		// Token: 0x04000092 RID: 146
		Divide,
		// Token: 0x04000093 RID: 147
		ArithmeticProduct,
		// Token: 0x04000094 RID: 148
		ArithmeticSum,
		// Token: 0x04000095 RID: 149
		StringExpression,
		// Token: 0x04000096 RID: 150
		SimpleExpression,
		// Token: 0x04000097 RID: 151
		ConditionalExpression,
		// Token: 0x04000098 RID: 152
		SelectionExpression,
		// Token: 0x04000099 RID: 153
		ParenthesisExpression,
		// Token: 0x0400009A RID: 154
		ArrayAccess,
		// Token: 0x0400009B RID: 155
		MultiStatement,
		// Token: 0x0400009C RID: 156
		ComparisonExpression,
		// Token: 0x0400009D RID: 157
		FieldExpression,
		// Token: 0x0400009E RID: 158
		MarkerOccuranceExpression,
		// Token: 0x0400009F RID: 159
		FunctionIdentifier,
		// Token: 0x040000A0 RID: 160
		FunctionCall,
		// Token: 0x040000A1 RID: 161
		FunctionParam,
		// Token: 0x040000A2 RID: 162
		ParameterWithMarkerOccurance,
		// Token: 0x040000A3 RID: 163
		ParameterWithMultipleMarkerOccurances,
		// Token: 0x040000A4 RID: 164
		QualifiedIdentifier,
		// Token: 0x040000A5 RID: 165
		ParameterWithAttribute,
		// Token: 0x040000A6 RID: 166
		StartsWith,
		// Token: 0x040000A7 RID: 167
		TextId
	}
}
