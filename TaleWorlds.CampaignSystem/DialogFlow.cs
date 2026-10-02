using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000089 RID: 137
	public class DialogFlow
	{
		// Token: 0x0600115B RID: 4443 RVA: 0x00053F40 File Offset: 0x00052140
		private DialogFlow(string startingToken, int priority = 100)
		{
			this._currentToken = startingToken;
			this.Priority = priority;
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00053F64 File Offset: 0x00052164
		private DialogFlow Line(TextObject text, bool byPlayer, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, bool isRepeatable = false, string inputToken = null, string outputToken = null)
		{
			string text2 = outputToken ?? Campaign.Current.ConversationManager.CreateToken();
			this.AddLine(text, inputToken ?? this._currentToken, text2, byPlayer, speakerDelegate, listenerDelegate, isRepeatable, false, false);
			this._currentToken = text2;
			return this;
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00053FAC File Offset: 0x000521AC
		public DialogFlow Variation(string text, params object[] propertiesAndWeights)
		{
			return this.Variation(new TextObject(text, null), propertiesAndWeights);
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x00053FBC File Offset: 0x000521BC
		public DialogFlow Variation(TextObject text, params object[] propertiesAndWeights)
		{
			for (int i = 0; i < propertiesAndWeights.Length; i += 2)
			{
				string text2 = (string)propertiesAndWeights[i];
				int num = Convert.ToInt32(propertiesAndWeights[i + 1]);
				List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
				list.Add(new GameTextManager.ChoiceTag(text2, num));
				this.Lines[this.Lines.Count - 1].AddVariation(text, list);
			}
			return this;
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x0005401E File Offset: 0x0005221E
		public DialogFlow NpcLine(string npcText, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			return this.NpcLine(new TextObject(npcText, null), speakerDelegate, listenerDelegate, inputToken, outputToken);
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00054033 File Offset: 0x00052233
		public DialogFlow NpcLine(TextObject npcText, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			return this.Line(npcText, false, speakerDelegate, listenerDelegate, false, inputToken, outputToken);
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x00054044 File Offset: 0x00052244
		public DialogFlow NpcLineWithVariation(string npcText, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			DialogFlow dialogFlow = this.Line(TextObject.GetEmpty(), false, speakerDelegate, listenerDelegate, false, inputToken, outputToken);
			List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
			list.Add(new GameTextManager.ChoiceTag("DefaultTag", 1));
			this.Lines[this.Lines.Count - 1].AddVariation(new TextObject(npcText, null), list);
			return dialogFlow;
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x000540A0 File Offset: 0x000522A0
		public DialogFlow NpcLineWithVariation(TextObject npcText, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			DialogFlow dialogFlow = this.Line(TextObject.GetEmpty(), false, speakerDelegate, listenerDelegate, false, inputToken, outputToken);
			List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
			list.Add(new GameTextManager.ChoiceTag("DefaultTag", 1));
			this.Lines[this.Lines.Count - 1].AddVariation(npcText, list);
			return dialogFlow;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x000540F6 File Offset: 0x000522F6
		public DialogFlow PlayerLine(string playerText, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			return this.Line(new TextObject(playerText, null), true, null, listenerDelegate, false, inputToken, outputToken);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0005410C File Offset: 0x0005230C
		public DialogFlow PlayerLine(TextObject playerText, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			return this.Line(playerText, true, null, listenerDelegate, false, inputToken, outputToken);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0005411C File Offset: 0x0005231C
		private DialogFlow BeginOptions(bool byPlayer, string inputToken = null, bool optionUsedOnce = false)
		{
			this._curDialogFlowContext = new DialogFlowContext(inputToken ?? this._currentToken, byPlayer, this._curDialogFlowContext, optionUsedOnce);
			return this;
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x0005413D File Offset: 0x0005233D
		public DialogFlow BeginPlayerOptions(string inputToken = null, bool optionUsedOnce = false)
		{
			return this.BeginOptions(true, inputToken, optionUsedOnce);
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00054148 File Offset: 0x00052348
		public DialogFlow BeginNpcOptions(string inputToken = null, bool optionUsedOnce = false)
		{
			return this.BeginOptions(false, inputToken, optionUsedOnce);
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00054154 File Offset: 0x00052354
		private DialogFlow Option(TextObject text, bool byPlayer, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, bool isRepeatable = false, bool isSpecialOption = false, string inputToken = null, string outputToken = null)
		{
			string text2 = outputToken ?? Campaign.Current.ConversationManager.CreateToken();
			this.AddLine(text, inputToken ?? this._curDialogFlowContext.Token, text2, byPlayer, speakerDelegate, listenerDelegate, isRepeatable, isSpecialOption, this._curDialogFlowContext.OptionsUsedOnlyOnce);
			this._currentToken = text2;
			return this;
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x000541AC File Offset: 0x000523AC
		public DialogFlow PlayerOption(string text, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			return this.PlayerOption(new TextObject(text, null), listenerDelegate, inputToken, outputToken);
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x000541C0 File Offset: 0x000523C0
		public DialogFlow PlayerOption(TextObject text, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.Option(text, true, null, listenerDelegate, false, false, inputToken, outputToken);
			return this;
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x000541E0 File Offset: 0x000523E0
		public DialogFlow PlayerSpecialOption(TextObject text, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.Option(text, true, null, listenerDelegate, false, true, inputToken, outputToken);
			return this;
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x00054200 File Offset: 0x00052400
		public DialogFlow PlayerRepeatableOption(TextObject text, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.Option(text, true, null, listenerDelegate, true, false, inputToken, outputToken);
			return this;
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00054220 File Offset: 0x00052420
		public DialogFlow NpcOption(string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.Option(new TextObject(text, null), false, speakerDelegate, listenerDelegate, false, false, inputToken, outputToken);
			this._lastLine.ConditionDelegate = conditionDelegate;
			return this;
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00054254 File Offset: 0x00052454
		public DialogFlow NpcOption(TextObject text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.Option(text, false, speakerDelegate, listenerDelegate, false, false, inputToken, outputToken);
			this._lastLine.ConditionDelegate = conditionDelegate;
			return this;
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00054280 File Offset: 0x00052480
		public DialogFlow NpcOptionWithVariation(string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.NpcOptionWithVariation(new TextObject(text, null), conditionDelegate, speakerDelegate, listenerDelegate, inputToken, outputToken);
			return this;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x0005429C File Offset: 0x0005249C
		public DialogFlow NpcOptionWithVariation(TextObject text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.Option(TextObject.GetEmpty(), false, speakerDelegate, listenerDelegate, false, false, inputToken, outputToken);
			List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
			list.Add(new GameTextManager.ChoiceTag("DefaultTag", 1));
			this._lastLine.AddVariation(text, list);
			this._lastLine.ConditionDelegate = conditionDelegate;
			return this;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x000542F0 File Offset: 0x000524F0
		private DialogFlow EndOptions(bool byPlayer)
		{
			this._curDialogFlowContext = this._curDialogFlowContext.Parent;
			return this;
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00054304 File Offset: 0x00052504
		public DialogFlow EndPlayerOptions()
		{
			return this.EndOptions(true);
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x0005430D File Offset: 0x0005250D
		public DialogFlow EndNpcOptions()
		{
			return this.EndOptions(false);
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00054316 File Offset: 0x00052516
		public DialogFlow Condition(ConversationSentence.OnConditionDelegate conditionDelegate)
		{
			this._lastLine.ConditionDelegate = conditionDelegate;
			return this;
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00054325 File Offset: 0x00052525
		public DialogFlow ClickableCondition(ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate)
		{
			this._lastLine.ClickableConditionDelegate = clickableConditionDelegate;
			return this;
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00054334 File Offset: 0x00052534
		public DialogFlow Consequence(ConversationSentence.OnConsequenceDelegate consequenceDelegate)
		{
			this._lastLine.ConsequenceDelegate = consequenceDelegate;
			return this;
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00054343 File Offset: 0x00052543
		public static DialogFlow CreateDialogFlow(string inputToken = null, int priority = 100)
		{
			return new DialogFlow(inputToken ?? Campaign.Current.ConversationManager.CreateToken(), priority);
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00054360 File Offset: 0x00052560
		private DialogFlowLine AddLine(TextObject text, string inputToken, string outputToken, bool byPlayer, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate, bool isRepeatable, bool isSpecialOption = false, bool usedOncePerConversation = false)
		{
			DialogFlowLine dialogFlowLine = new DialogFlowLine();
			dialogFlowLine.Text = text;
			dialogFlowLine.InputToken = inputToken;
			dialogFlowLine.OutputToken = outputToken;
			dialogFlowLine.ByPlayer = byPlayer;
			dialogFlowLine.SpeakerDelegate = speakerDelegate;
			dialogFlowLine.ListenerDelegate = listenerDelegate;
			dialogFlowLine.IsRepeatable = isRepeatable;
			dialogFlowLine.IsSpecialOption = isSpecialOption;
			dialogFlowLine.IsUsedOnce = usedOncePerConversation;
			this.Lines.Add(dialogFlowLine);
			this._lastLine = dialogFlowLine;
			return dialogFlowLine;
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x000543CC File Offset: 0x000525CC
		public DialogFlow NpcDefaultOption(string text)
		{
			return this.NpcOption(text, null, null, null, null, null);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x000543DA File Offset: 0x000525DA
		public DialogFlow GenerateToken(out string token)
		{
			token = Campaign.Current.ConversationManager.CreateToken();
			return this;
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x000543EE File Offset: 0x000525EE
		public DialogFlow GotoDialogState(string input)
		{
			this._lastLine.OutputToken = input;
			this._currentToken = input;
			return this;
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x00054404 File Offset: 0x00052604
		public DialogFlow GotoDialogStateBranched(string input, ConversationSentence.OnConditionDelegate conditionDelegate, string alternative)
		{
			string text = ((conditionDelegate != null && conditionDelegate()) ? input : alternative);
			this._lastLine.OutputToken = text;
			this._currentToken = text;
			return this;
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00054435 File Offset: 0x00052635
		public DialogFlow GetOutputToken(out string oState)
		{
			oState = this._lastLine.OutputToken;
			return this;
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x00054445 File Offset: 0x00052645
		public DialogFlow GoBackToDialogState(string iState)
		{
			this._currentToken = iState;
			return this;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x0005444F File Offset: 0x0005264F
		public DialogFlow CloseDialog()
		{
			this.GotoDialogState("close_window");
			return this;
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x0005445E File Offset: 0x0005265E
		private ConversationSentence AddDialogLine(ConversationSentence dialogLine)
		{
			Campaign.Current.ConversationManager.AddDialogLine(dialogLine);
			return dialogLine;
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x00054474 File Offset: 0x00052674
		public ConversationSentence AddPlayerLine(string id, string inputToken, string outputToken, string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, object relatedObject, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null, ConversationSentence.OnPersuasionOptionDelegate persuasionOptionDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, new TextObject(text, null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 1U, priority, 0, 0, relatedObject, false, speakerDelegate, listenerDelegate, persuasionOptionDelegate));
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x000544B0 File Offset: 0x000526B0
		public ConversationSentence AddDialogLine(string id, string inputToken, string outputToken, string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, object relatedObject, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, new TextObject(text, null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 0U, priority, 0, 0, relatedObject, false, speakerDelegate, listenerDelegate, null));
		}

		// Token: 0x0400056A RID: 1386
		internal readonly List<DialogFlowLine> Lines = new List<DialogFlowLine>();

		// Token: 0x0400056B RID: 1387
		internal readonly int Priority;

		// Token: 0x0400056C RID: 1388
		private string _currentToken;

		// Token: 0x0400056D RID: 1389
		private DialogFlowLine _lastLine;

		// Token: 0x0400056E RID: 1390
		private DialogFlowContext _curDialogFlowContext;
	}
}
