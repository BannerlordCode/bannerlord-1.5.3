using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization.Expressions;

namespace TaleWorlds.Localization.TextProcessor
{
	// Token: 0x02000027 RID: 39
	internal class MBTextParser
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00005283 File Offset: 0x00003483
		internal TextExpression LookAheadFirst
		{
			get
			{
				return this._lookaheadFirst;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x0000528B File Offset: 0x0000348B
		internal TextExpression LookAheadSecond
		{
			get
			{
				return this._lookaheadSecond;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00005293 File Offset: 0x00003493
		internal TextExpression LookAheadThird
		{
			get
			{
				return this._lookaheadThird;
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000529B File Offset: 0x0000349B
		private static void Clear()
		{
			MBTextParser._instance._symbolSequence.Clear();
			MBTextParser._instance._lookaheadFirst = null;
			MBTextParser._instance._lookaheadSecond = null;
			MBTextParser._instance._lookaheadThird = null;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000052D0 File Offset: 0x000034D0
		private TextExpression GetSimpleToken(TokenType tokenType, string strValue)
		{
			if (tokenType == TokenType.Text)
			{
				return new SimpleText(strValue);
			}
			if (tokenType == TokenType.Number)
			{
				return new SimpleNumberExpression(strValue);
			}
			if (tokenType == TokenType.Identifier)
			{
				return new VariableExpression(strValue, null);
			}
			if (tokenType == TokenType.LanguageMarker)
			{
				return new LangaugeMarkerExpression(strValue);
			}
			if (tokenType == TokenType.TextId)
			{
				return new TextIdExpression(strValue);
			}
			if (tokenType == TokenType.QualifiedIdentifier)
			{
				return new QualifiedIdentifierExpression(strValue);
			}
			if (tokenType == TokenType.ParameterWithAttribute)
			{
				return new ParameterWithAttributeExpression(strValue);
			}
			if (tokenType == TokenType.StartsWith)
			{
				return new StartsWithExpression(strValue);
			}
			return new SimpleToken(tokenType, strValue);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005348 File Offset: 0x00003548
		private void LoadSequenceStack(List<MBTextToken> tokens)
		{
			for (int i = tokens.Count - 1; i >= 0; i--)
			{
				TextExpression simpleToken = this.GetSimpleToken(tokens[i].TokenType, tokens[i].Value);
				this._symbolSequence.Push(simpleToken);
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005393 File Offset: 0x00003593
		private void PushToken(TextExpression token)
		{
			this._symbolSequence.Push(token);
			this.UpdateLookAheads();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000053A8 File Offset: 0x000035A8
		private void UpdateLookAheads()
		{
			if (this._symbolSequence.Count == 0)
			{
				this._lookaheadFirst = SimpleToken.SequenceTerminator;
			}
			else
			{
				this._lookaheadFirst = this._symbolSequence.Peek();
			}
			if (this._symbolSequence.Count < 2)
			{
				this._lookaheadSecond = SimpleToken.SequenceTerminator;
			}
			else
			{
				TextExpression textExpression = this._symbolSequence.Pop();
				this._lookaheadSecond = this._symbolSequence.Peek();
				this._symbolSequence.Push(textExpression);
			}
			if (this._symbolSequence.Count < 3)
			{
				this._lookaheadThird = SimpleToken.SequenceTerminator;
				return;
			}
			TextExpression textExpression2 = this._symbolSequence.Pop();
			TextExpression textExpression3 = this._symbolSequence.Pop();
			this._lookaheadThird = this._symbolSequence.Peek();
			this._symbolSequence.Push(textExpression3);
			this._symbolSequence.Push(textExpression2);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000547F File Offset: 0x0000367F
		private void DiscardToken()
		{
			if (this._symbolSequence.Count > 0)
			{
				this._symbolSequence.Pop();
			}
			this.UpdateLookAheads();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000054A4 File Offset: 0x000036A4
		private void DiscardToken(TokenType tokenType)
		{
			if (this._lookaheadFirst.TokenType != tokenType)
			{
				MBTextManager.ThrowLocalizationError(string.Format("Unxpected token: {1} while expecting: {0}", tokenType.ToString().ToUpper(), this._lookaheadFirst.RawValue));
			}
			this.DiscardToken();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000054F4 File Offset: 0x000036F4
		private void Statements()
		{
			TextExpression rootExpressions = this.GetRootExpressions();
			this._queryModel.AddRootExpression(rootExpressions);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005514 File Offset: 0x00003714
		private bool IsRootExpression(TokenType tokenType)
		{
			return tokenType == TokenType.Text || tokenType == TokenType.SimpleExpression || tokenType == TokenType.ConditionalExpression || tokenType == TokenType.TextId || tokenType == TokenType.SelectionExpression || tokenType == TokenType.MultiStatement || tokenType == TokenType.FieldExpression || tokenType == TokenType.LanguageMarker;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005540 File Offset: 0x00003740
		private void GetRootExpressionsImp(List<TextExpression> expList)
		{
			for (;;)
			{
				if (!this.RunRootGrammarRulesExceptCollapse())
				{
					if (!this.IsRootExpression(this.LookAheadFirst.TokenType))
					{
						break;
					}
					TextExpression lookAheadFirst = this.LookAheadFirst;
					this.DiscardToken();
					expList.Add(lookAheadFirst);
				}
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005580 File Offset: 0x00003780
		private TextExpression GetRootExpressions()
		{
			List<TextExpression> list = new List<TextExpression>();
			this.GetRootExpressionsImp(list);
			if (list.Count == 0)
			{
				return null;
			}
			if (list.Count == 1)
			{
				return list[0];
			}
			return new MultiStatement(list);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000055BB File Offset: 0x000037BB
		private bool RunRootGrammarRulesExceptCollapse()
		{
			return this.CheckSimpleStatement() || this.CheckConditionalStatement() || this.CheckSelectionStatement() || this.CheckFieldStatement();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000055E0 File Offset: 0x000037E0
		private bool CollapseStatements()
		{
			if (!this.IsRootExpression(this.LookAheadFirst.TokenType) || this.LookAheadFirst.TokenType == TokenType.MultiStatement)
			{
				return false;
			}
			List<TextExpression> list = new List<TextExpression>();
			TextExpression lookAheadFirst = this.LookAheadFirst;
			this.DiscardToken();
			list.Add(lookAheadFirst);
			bool flag = false;
			while (!flag)
			{
				while (this.RunRootGrammarRulesExceptCollapse())
				{
				}
				if (this.IsRootExpression(this.LookAheadFirst.TokenType))
				{
					TextExpression lookAheadFirst2 = this.LookAheadFirst;
					this.DiscardToken();
					list.Add(lookAheadFirst2);
				}
				else
				{
					flag = true;
				}
			}
			this.PushToken(new MultiStatement(list));
			return true;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005674 File Offset: 0x00003874
		private bool CheckSimpleStatement()
		{
			if (this.LookAheadFirst.TokenType != TokenType.OpenBraces)
			{
				return false;
			}
			this.DiscardToken(TokenType.OpenBraces);
			bool flag = false;
			while (!flag)
			{
				flag = !this.DoExpressionRules();
			}
			TokenType tokenType = this.LookAheadFirst.TokenType;
			if (this.IsArithmeticExpression(tokenType))
			{
				TextExpression textExpression = new SimpleExpression(this.LookAheadFirst);
				this.DiscardToken();
				this.DiscardToken(TokenType.CloseBraces);
				this.PushToken(textExpression);
			}
			else
			{
				this.DiscardToken(TokenType.CloseBraces);
			}
			return true;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000056F0 File Offset: 0x000038F0
		private bool CheckFieldStatement()
		{
			if (this.LookAheadFirst.TokenType != TokenType.FieldStarter)
			{
				return false;
			}
			this.DiscardToken(TokenType.FieldStarter);
			bool flag = false;
			while (!flag)
			{
				flag = !this.DoExpressionRules();
			}
			if (this.LookAheadFirst.TokenType != TokenType.Identifier)
			{
				Debug.FailedAssert("Can not parse the text: " + this.LookAheadFirst, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\TextProcessor\\MbTextParser.cs", "CheckFieldStatement", 299);
				return false;
			}
			TextExpression lookAheadFirst = this.LookAheadFirst;
			this.DiscardToken(TokenType.Identifier);
			this.DiscardToken(TokenType.CloseBraces);
			TextExpression textExpression = this.GetRootExpressions();
			if (textExpression == null)
			{
				textExpression = new SimpleToken(TokenType.Text, "");
			}
			this.DiscardToken(TokenType.FieldFinalizer);
			FieldExpression fieldExpression = new FieldExpression(lookAheadFirst, textExpression);
			this.PushToken(fieldExpression);
			return true;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000057A0 File Offset: 0x000039A0
		private bool CheckConditionalStatement()
		{
			if (this.LookAheadFirst.TokenType != TokenType.ConditionStarter)
			{
				return false;
			}
			bool flag = false;
			List<TextExpression> list = new List<TextExpression>();
			List<TextExpression> list2 = new List<TextExpression>();
			while (!flag)
			{
				TokenType tokenType = this.LookAheadFirst.TokenType;
				if (this.LookAheadFirst.TokenType == TokenType.ConditionStarter || this.LookAheadFirst.TokenType == TokenType.ConditionFollowUp)
				{
					this.DiscardToken();
					while (this.DoExpressionRules())
					{
					}
					tokenType = this.LookAheadFirst.TokenType;
					if (!this.IsArithmeticExpression(tokenType))
					{
						Debug.FailedAssert("Can not parse the text: " + this.LookAheadFirst, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\TextProcessor\\MbTextParser.cs", "CheckConditionalStatement", 346);
						return false;
					}
					list.Add(this.LookAheadFirst);
					this.DiscardToken();
					this.DiscardToken(TokenType.CloseBraces);
				}
				else
				{
					if (tokenType != TokenType.ConditionSeperator && tokenType != TokenType.Seperator)
					{
						MBTextManager.ThrowLocalizationError("Can not parse the text: " + this.LookAheadFirst);
						return false;
					}
					this.DiscardToken();
					flag = true;
				}
				TextExpression textExpression = this.GetRootExpressions();
				if (textExpression == null)
				{
					textExpression = new SimpleToken(TokenType.Text, "");
				}
				list2.Add(textExpression);
			}
			while (!flag)
			{
			}
			this.DiscardToken(TokenType.ConditionFinalizer);
			ConditionExpression conditionExpression = new ConditionExpression(list, list2);
			this.PushToken(conditionExpression);
			return true;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000058D4 File Offset: 0x00003AD4
		private bool CheckSelectionStatement()
		{
			if (this.LookAheadFirst.TokenType != TokenType.SelectionStarter)
			{
				return false;
			}
			this.DiscardToken(TokenType.SelectionStarter);
			while (this.DoExpressionRules())
			{
			}
			TokenType tokenType = this.LookAheadFirst.TokenType;
			if (!this.IsArithmeticExpression(tokenType))
			{
				Debug.FailedAssert("Can not parse the text: " + this.LookAheadFirst, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\TextProcessor\\MbTextParser.cs", "CheckSelectionStatement", 392);
				return false;
			}
			TextExpression lookAheadFirst = this.LookAheadFirst;
			this.DiscardToken();
			this.DiscardToken(TokenType.CloseBraces);
			bool flag = false;
			List<TextExpression> list = new List<TextExpression>();
			for (;;)
			{
				TextExpression textExpression = this.GetRootExpressions();
				if (textExpression == null)
				{
					textExpression = new SimpleToken(TokenType.Text, "");
				}
				list.Add(textExpression);
				TokenType tokenType2 = this.LookAheadFirst.TokenType;
				if (tokenType2 == TokenType.SelectionSeperator)
				{
					this.DiscardToken();
				}
				else
				{
					if (tokenType2 != TokenType.SelectionFinalizer)
					{
						break;
					}
					flag = true;
					this.DiscardToken();
				}
				if (flag)
				{
					goto Block_7;
				}
			}
			Debug.FailedAssert("Can not parse the text: " + this.LookAheadFirst, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\TextProcessor\\MbTextParser.cs", "CheckSelectionStatement", 424);
			return false;
			Block_7:
			SelectionExpression selectionExpression = new SelectionExpression(lookAheadFirst, list);
			this.PushToken(selectionExpression);
			return true;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000059E4 File Offset: 0x00003BE4
		private bool DoExpressionRules()
		{
			return this.ConsumeArrayAccessExpression() || this.ConsumeFunction() || this.ConsumeMarkerOccuranceExpression() || this.ConsumeNegativeAritmeticExpression() || this.ConsumeParenthesisExpression() || this.ConsumeInnerAritmeticExpression() || this.ConsumeOuterAritmeticExpression() || this.ConsumeComparisonExpression();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005A34 File Offset: 0x00003C34
		private bool ConsumeFunction()
		{
			if (this.LookAheadFirst.TokenType != TokenType.FunctionIdentifier)
			{
				return false;
			}
			string text = this.LookAheadFirst.RawValue.Substring(0, this.LookAheadFirst.RawValue.Length - 1);
			this.DiscardToken();
			bool flag = false;
			List<TextExpression> list = new List<TextExpression>();
			while (this.LookAheadFirst.TokenType != TokenType.CloseParenthesis && !flag)
			{
				if (list.Count > 0)
				{
					this.DiscardToken(TokenType.Comma);
				}
				while (this.DoExpressionRules())
				{
				}
				TokenType tokenType = this.LookAheadFirst.TokenType;
				if (!this.IsArithmeticExpression(tokenType))
				{
					Debug.FailedAssert("Can not parse the text: " + this.LookAheadFirst, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\TextProcessor\\MbTextParser.cs", "ConsumeFunction", 482);
					return false;
				}
				list.Add(this.LookAheadFirst);
				this.DiscardToken();
			}
			this.DiscardToken(TokenType.CloseParenthesis);
			FunctionCall functionCall = new FunctionCall(text, list);
			this.PushToken(functionCall);
			return true;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005B1C File Offset: 0x00003D1C
		private bool ConsumeMarkerOccuranceExpression()
		{
			if (this.LookAheadFirst.TokenType == TokenType.Identifier && this.LookAheadSecond.TokenType == TokenType.MarkerOccuranceIdentifier)
			{
				VariableExpression variableExpression = this.LookAheadFirst as VariableExpression;
				TextExpression lookAheadSecond = this.LookAheadSecond;
				this.DiscardToken();
				this.DiscardToken();
				MarkerOccuranceTextExpression markerOccuranceTextExpression = new MarkerOccuranceTextExpression(lookAheadSecond.RawValue.Substring(2), variableExpression);
				this.PushToken(markerOccuranceTextExpression);
				return true;
			}
			return false;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005B84 File Offset: 0x00003D84
		private bool ConsumeArrayAccessExpression()
		{
			if (this.LookAheadFirst.TokenType == TokenType.Identifier && this.LookAheadSecond.TokenType == TokenType.OpenBrackets)
			{
				TextExpression lookAheadFirst = this.LookAheadFirst;
				this.DiscardToken();
				this.DiscardToken(TokenType.OpenBrackets);
				while (this.DoExpressionRules())
				{
				}
				TokenType tokenType = this.LookAheadFirst.TokenType;
				if (this.IsArithmeticExpression(tokenType))
				{
					TextExpression lookAheadFirst2 = this.LookAheadFirst;
					this.DiscardToken();
					this.DiscardToken(TokenType.CloseBrackets);
					ArrayReference arrayReference = new ArrayReference(lookAheadFirst.RawValue, lookAheadFirst2);
					this.PushToken(arrayReference);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005C10 File Offset: 0x00003E10
		private bool ConsumeNegativeAritmeticExpression()
		{
			if (this.LookAheadFirst.TokenType == TokenType.Minus)
			{
				this.ConsumeAritmeticOperation();
				TokenType tokenType = this.LookAheadFirst.TokenType;
				if (this.IsArithmeticExpression(tokenType))
				{
					ArithmeticExpression arithmeticExpression = new ArithmeticExpression(ArithmeticOperation.Subtract, new SimpleToken(TokenType.Number, "0"), this.LookAheadFirst);
					this.PushToken(arithmeticExpression);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005C6C File Offset: 0x00003E6C
		private bool ConsumeParenthesisExpression()
		{
			if (this.LookAheadFirst.TokenType != TokenType.OpenParenthesis)
			{
				return false;
			}
			this.DiscardToken(TokenType.OpenParenthesis);
			while (this.DoExpressionRules())
			{
			}
			TokenType tokenType = this.LookAheadFirst.TokenType;
			if (this.IsArithmeticExpression(tokenType))
			{
				ParanthesisExpression paranthesisExpression = new ParanthesisExpression(this.LookAheadFirst);
				this.DiscardToken();
				this.DiscardToken(TokenType.CloseParenthesis);
				this.PushToken(paranthesisExpression);
				return true;
			}
			this.DiscardToken(TokenType.CloseParenthesis);
			return true;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005CDC File Offset: 0x00003EDC
		private bool IsArithmeticExpression(TokenType t)
		{
			return t == TokenType.ArithmeticProduct || t == TokenType.ArithmeticSum || t == TokenType.Identifier || t == TokenType.QualifiedIdentifier || t == TokenType.MarkerOccuranceExpression || t == TokenType.ParameterWithMarkerOccurance || t == TokenType.ParameterWithMultipleMarkerOccurances || t == TokenType.StartsWith || t == TokenType.Number || t == TokenType.ParenthesisExpression || t == TokenType.ComparisonExpression || t == TokenType.FunctionCall || t == TokenType.FunctionParam || t == TokenType.ArrayAccess || t == TokenType.ParameterWithAttribute;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005D38 File Offset: 0x00003F38
		private bool ConsumeInnerAritmeticExpression()
		{
			TokenType tokenType = this.LookAheadFirst.TokenType;
			TokenType tokenType2 = this.LookAheadSecond.TokenType;
			TokenType tokenType3 = this.LookAheadThird.TokenType;
			if (this.IsArithmeticExpression(tokenType) && (tokenType2 == TokenType.Multiply || tokenType2 == TokenType.Divide))
			{
				TextExpression lookAheadFirst = this.LookAheadFirst;
				this.DiscardToken();
				ArithmeticOperation arithmeticOperation = this.ConsumeAritmeticOperation();
				if (!this.IsArithmeticExpression(this.LookAheadFirst.TokenType))
				{
					while (this.DoExpressionRules())
					{
					}
				}
				TextExpression lookAheadFirst2 = this.LookAheadFirst;
				this.DiscardToken();
				ArithmeticExpression arithmeticExpression = new ArithmeticExpression(arithmeticOperation, lookAheadFirst, lookAheadFirst2);
				this.PushToken(arithmeticExpression);
				return true;
			}
			return false;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005DD0 File Offset: 0x00003FD0
		private bool ConsumeOuterAritmeticExpression()
		{
			TokenType tokenType = this.LookAheadFirst.TokenType;
			TokenType tokenType2 = this.LookAheadSecond.TokenType;
			if (this.IsArithmeticExpression(tokenType) && (tokenType2 == TokenType.Plus || tokenType2 == TokenType.Minus))
			{
				TextExpression lookAheadFirst = this.LookAheadFirst;
				this.DiscardToken();
				ArithmeticOperation arithmeticOperation = this.ConsumeAritmeticOperation();
				while (this.DoExpressionRules())
				{
				}
				if (this.IsArithmeticExpression(this.LookAheadFirst.TokenType))
				{
					TextExpression lookAheadFirst2 = this.LookAheadFirst;
					this.DiscardToken();
					ArithmeticExpression arithmeticExpression = new ArithmeticExpression(arithmeticOperation, lookAheadFirst, lookAheadFirst2);
					this.PushToken(arithmeticExpression);
					return true;
				}
				Debug.FailedAssert("Can not parse the text: " + this.LookAheadFirst, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\TextProcessor\\MbTextParser.cs", "ConsumeOuterAritmeticExpression", 656);
			}
			return false;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005E84 File Offset: 0x00004084
		private ArithmeticOperation ConsumeAritmeticOperation()
		{
			ArithmeticOperation arithmeticOperation = ((this.LookAheadFirst.TokenType == TokenType.Plus) ? ArithmeticOperation.Add : ((this.LookAheadFirst.TokenType == TokenType.Minus) ? ArithmeticOperation.Subtract : ((this.LookAheadFirst.TokenType == TokenType.Multiply) ? ArithmeticOperation.Multiply : ((this.LookAheadFirst.TokenType == TokenType.Divide) ? ArithmeticOperation.Divide : ArithmeticOperation.Add))));
			this.DiscardToken();
			return arithmeticOperation;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005EE0 File Offset: 0x000040E0
		private bool ConsumeComparisonExpression()
		{
			TokenType tokenType = this.LookAheadFirst.TokenType;
			TokenType tokenType2 = this.LookAheadSecond.TokenType;
			if (!this.IsArithmeticExpression(tokenType) || !this.IsComparisonOperator(tokenType2))
			{
				return false;
			}
			TextExpression lookAheadFirst = this.LookAheadFirst;
			this.DiscardToken();
			ComparisonOperation comparisonOp = this.GetComparisonOp(tokenType2);
			this.DiscardToken();
			while (this.DoExpressionRules())
			{
			}
			if (!this.IsArithmeticExpression(this.LookAheadFirst.TokenType))
			{
				Debug.FailedAssert("Can not parse the text: " + this.LookAheadFirst, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\TextProcessor\\MbTextParser.cs", "ConsumeComparisonExpression", 700);
				return false;
			}
			TextExpression lookAheadFirst2 = this.LookAheadFirst;
			this.DiscardToken();
			ComparisonExpression comparisonExpression = new ComparisonExpression(comparisonOp, lookAheadFirst, lookAheadFirst2);
			this.PushToken(comparisonExpression);
			return true;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005F9A File Offset: 0x0000419A
		private bool IsComparisonOperator(TokenType tokenType)
		{
			return tokenType == TokenType.Equals || tokenType == TokenType.NotEquals || tokenType == TokenType.GreaterThan || tokenType == TokenType.GreaterOrEqual || tokenType == TokenType.GreaterThan || tokenType == TokenType.LessOrEqual || tokenType == TokenType.LessThan;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005FBB File Offset: 0x000041BB
		private BooleanOperation GetBooleanOp(TokenType tokenType)
		{
			if (tokenType == TokenType.Or)
			{
				return BooleanOperation.Or;
			}
			if (tokenType == TokenType.And)
			{
				return BooleanOperation.And;
			}
			if (tokenType != TokenType.Not)
			{
				return BooleanOperation.And;
			}
			return BooleanOperation.Not;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005FD0 File Offset: 0x000041D0
		private ComparisonOperation GetComparisonOp(TokenType tokenType)
		{
			if (tokenType == TokenType.Equals)
			{
				return ComparisonOperation.Equals;
			}
			if (tokenType == TokenType.NotEquals)
			{
				return ComparisonOperation.NotEquals;
			}
			if (tokenType == TokenType.GreaterThan)
			{
				return ComparisonOperation.GreaterThan;
			}
			if (tokenType == TokenType.GreaterOrEqual)
			{
				return ComparisonOperation.GreaterOrEqual;
			}
			if (tokenType == TokenType.GreaterThan)
			{
				return ComparisonOperation.GreaterThan;
			}
			if (tokenType == TokenType.LessOrEqual)
			{
				return ComparisonOperation.LessOrEqual;
			}
			if (tokenType != TokenType.LessThan)
			{
				return ComparisonOperation.Equals;
			}
			return ComparisonOperation.LessThan;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005FFE File Offset: 0x000041FE
		private MBTextModel ParseInternal(List<MBTextToken> tokens)
		{
			this.LoadSequenceStack(tokens);
			this.UpdateLookAheads();
			this._queryModel = new MBTextModel();
			this.Statements();
			this.DiscardToken(TokenType.SequenceTerminator);
			return this._queryModel;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000602C File Offset: 0x0000422C
		internal static MBTextModel Parse(List<MBTextToken> tokens)
		{
			if (MBTextParser._instance == null)
			{
				MBTextParser._instance = new MBTextParser();
			}
			else
			{
				MBTextParser.Clear();
			}
			return MBTextParser._instance.ParseInternal(tokens);
		}

		// Token: 0x0400005A RID: 90
		[ThreadStatic]
		private static MBTextParser _instance;

		// Token: 0x0400005B RID: 91
		private Stack<TextExpression> _symbolSequence = new Stack<TextExpression>();

		// Token: 0x0400005C RID: 92
		private TextExpression _lookaheadFirst;

		// Token: 0x0400005D RID: 93
		private TextExpression _lookaheadSecond;

		// Token: 0x0400005E RID: 94
		private TextExpression _lookaheadThird;

		// Token: 0x0400005F RID: 95
		private MBTextModel _queryModel;
	}
}
