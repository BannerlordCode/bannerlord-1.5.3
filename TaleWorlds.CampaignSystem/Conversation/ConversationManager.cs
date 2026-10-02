using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Helpers;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Conversation.Tags;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x02000245 RID: 581
	public class ConversationManager
	{
		// Token: 0x06002310 RID: 8976 RVA: 0x0009BBDA File Offset: 0x00099DDA
		public int CreateConversationSentenceIndex()
		{
			int numConversationSentencesCreated = this._numConversationSentencesCreated;
			this._numConversationSentencesCreated++;
			return numConversationSentencesCreated;
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06002311 RID: 8977 RVA: 0x0009BBF0 File Offset: 0x00099DF0
		public string CurrentSentenceText
		{
			get
			{
				TextObject textObject = this._currentSentenceText;
				if (this.OneToOneConversationCharacter != null)
				{
					textObject = this.FindMatchingTextOrNull(textObject.GetID(), this.OneToOneConversationCharacter);
					if (textObject == null)
					{
						textObject = this._currentSentenceText;
					}
				}
				return MBTextManager.DiscardAnimationTagsAndCheckAnimationTagPositions(textObject.CopyTextObject().ToString());
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x0009BC3F File Offset: 0x00099E3F
		private int DialogRepeatCount
		{
			get
			{
				if (this._dialogRepeatObjects.Count > 0)
				{
					return this._dialogRepeatObjects[this._currentRepeatedDialogSetIndex].Count;
				}
				return 1;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002313 RID: 8979 RVA: 0x0009BC67 File Offset: 0x00099E67
		public bool IsConversationFlowActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x0009BC70 File Offset: 0x00099E70
		public ConversationManager()
		{
			this._sentences = new List<ConversationSentence>();
			this.stateMap = new Dictionary<string, int>();
			this.stateMap.Add("start", 0);
			this.stateMap.Add("event_triggered", 1);
			this.stateMap.Add("member_chat", 2);
			this.stateMap.Add("prisoner_chat", 3);
			this.stateMap.Add("close_window", 4);
			this._numberOfStateIndices = 5;
			this._isActive = false;
			this._executeDoOptionContinue = false;
			this.InitializeTags();
			this.ConversationAnimationManager = new ConversationAnimationManager();
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002315 RID: 8981 RVA: 0x0009BD4B File Offset: 0x00099F4B
		// (set) Token: 0x06002316 RID: 8982 RVA: 0x0009BD53 File Offset: 0x00099F53
		public List<ConversationSentenceOption> CurOptions { get; protected set; }

		// Token: 0x06002317 RID: 8983 RVA: 0x0009BD5C File Offset: 0x00099F5C
		public void StartNew(int startingToken, bool setActionsInstantly)
		{
			this._usedIndices.Clear();
			this.ActiveToken = startingToken;
			this._currentSentence = -1;
			this.ResetRepeatedDialogSystem();
			this._lastSelectedDialogObject = null;
			Debug.Print("--------------- Conversation Start --------------- ", 0, Debug.DebugColor.White, 4503599627370496UL);
			Debug.Print(string.Concat(new object[]
			{
				"Conversation character name: ",
				this.OneToOneConversationCharacter.Name,
				"\nid: ",
				this.OneToOneConversationCharacter.StringId,
				"\nculture:",
				this.OneToOneConversationCharacter.Culture,
				"\npersona:",
				this.OneToOneConversationCharacter.GetPersona().Name
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			this._mainAgent.OnConversationStarted();
			if (CampaignMission.Current != null)
			{
				foreach (IAgent agent in this.ConversationAgents)
				{
					CampaignMission.Current.OnConversationStart(agent, setActionsInstantly);
				}
			}
			this.ProcessPartnerSentence();
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x0009BE7C File Offset: 0x0009A07C
		private bool ProcessPartnerSentence()
		{
			List<ConversationSentenceOption> sentenceOptions = this.GetSentenceOptions(false, false);
			bool flag = false;
			if (sentenceOptions.Count > 0)
			{
				this.ProcessSentence(sentenceOptions[0]);
				flag = true;
			}
			IConversationStateHandler handler = this.Handler;
			if (handler != null)
			{
				handler.OnConversationContinue();
			}
			return flag;
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x0009BEC0 File Offset: 0x0009A0C0
		public void ProcessSentence(ConversationSentenceOption conversationSentenceOption)
		{
			ConversationSentence conversationSentence = this._sentences[conversationSentenceOption.SentenceNo];
			Debug.Print(conversationSentenceOption.DebugInfo, 0, Debug.DebugColor.White, 4503599627370496UL);
			this.ActiveToken = conversationSentence.OutputToken;
			this.UpdateSpeakerAndListenerAgents(conversationSentence);
			if (CampaignMission.Current != null)
			{
				CampaignMission.Current.OnProcessSentence();
			}
			this._lastSelectedDialogObject = conversationSentenceOption.RepeatObject;
			this._currentSentence = conversationSentenceOption.SentenceNo;
			if (Game.Current == null)
			{
				throw new MBNullParameterException("Game");
			}
			this.UpdateCurrentSentenceText();
			int count = this._sentences.Count;
			conversationSentence.RunConsequence(Game.Current);
			if (conversationSentence.IsUsedOnce)
			{
				this._usedIndices.Add(conversationSentence.Index);
			}
			if (CampaignMission.Current != null)
			{
				string[] conversationAnimations = MBTextManager.GetConversationAnimations(this._currentSentenceText);
				string text = "";
				VoiceObject voiceObject;
				string text2;
				if (MBTextManager.TryGetVoiceObject(this._currentSentenceText, out voiceObject, out text2))
				{
					text = Campaign.Current.Models.VoiceOverModel.GetSoundPathForCharacter((CharacterObject)this.SpeakerAgent.Character, voiceObject);
				}
				CampaignMission.Current.OnConversationPlay(conversationAnimations[0], conversationAnimations[1], conversationAnimations[2], conversationAnimations[3], text);
			}
			if (0 > this._currentSentence || this._currentSentence >= count)
			{
				Debug.FailedAssert("CurrentSentence is not valid.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Conversation\\ConversationManager.cs", "ProcessSentence", 417);
			}
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x0009C010 File Offset: 0x0009A210
		private void UpdateSpeakerAndListenerAgents(ConversationSentence sentence)
		{
			if (sentence.IsSpeaker != null)
			{
				if (sentence.IsSpeaker(this._mainAgent))
				{
					this.SetSpeakerAgent(this._mainAgent);
					goto IL_008B;
				}
				using (IEnumerator<IAgent> enumerator = this.ConversationAgents.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IAgent agent = enumerator.Current;
						if (sentence.IsSpeaker(agent))
						{
							this.SetSpeakerAgent(agent);
							break;
						}
					}
					goto IL_008B;
				}
			}
			this.SetSpeakerAgent((!sentence.IsPlayer) ? this.ConversationAgents[0] : this._mainAgent);
			IL_008B:
			if (sentence.IsListener != null)
			{
				if (sentence.IsListener(this._mainAgent))
				{
					this.SetListenerAgent(this._mainAgent);
					return;
				}
				using (IEnumerator<IAgent> enumerator = this.ConversationAgents.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IAgent agent2 = enumerator.Current;
						if (sentence.IsListener(agent2))
						{
							this.SetListenerAgent(agent2);
							break;
						}
					}
					return;
				}
			}
			this.SetListenerAgent((!sentence.IsPlayer) ? this._mainAgent : this.ConversationAgents[0]);
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x0009C150 File Offset: 0x0009A350
		private void SetSpeakerAgent(IAgent agent)
		{
			if (this._speakerAgent != agent)
			{
				this._speakerAgent = agent;
				if (this._speakerAgent != null && this._speakerAgent.Character is CharacterObject)
				{
					StringHelpers.SetCharacterProperties("SPEAKER", agent.Character as CharacterObject, null, false);
				}
			}
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0009C1A0 File Offset: 0x0009A3A0
		private void SetListenerAgent(IAgent agent)
		{
			if (this._listenerAgent != agent)
			{
				this._listenerAgent = agent;
				if (this._listenerAgent != null && this._listenerAgent.Character is CharacterObject)
				{
					StringHelpers.SetCharacterProperties("LISTENER", agent.Character as CharacterObject, null, false);
				}
			}
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x0009C1F0 File Offset: 0x0009A3F0
		public void UpdateCurrentSentenceText()
		{
			TextObject textObject;
			if (this._currentSentence >= 0)
			{
				textObject = this._sentences[this._currentSentence].Text;
			}
			else
			{
				if (Campaign.Current == null)
				{
					throw new MBNullParameterException("Campaign");
				}
				textObject = GameTexts.FindText("str_error_string", null);
			}
			this._currentSentenceText = textObject;
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x0009C244 File Offset: 0x0009A444
		public bool IsConversationEnded()
		{
			return this.ActiveToken == 4;
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x0009C24F File Offset: 0x0009A44F
		public void ClearCurrentOptions()
		{
			if (this.CurOptions == null)
			{
				this.CurOptions = new List<ConversationSentenceOption>();
			}
			this.CurOptions.Clear();
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x0009C270 File Offset: 0x0009A470
		public void AddToCurrentOptions(TextObject text, string id, bool isClickable, TextObject hintText)
		{
			ConversationSentenceOption conversationSentenceOption = new ConversationSentenceOption
			{
				SentenceNo = 0,
				Text = text,
				Id = id,
				RepeatObject = null,
				DebugInfo = null,
				IsClickable = isClickable,
				HintText = hintText
			};
			this.CurOptions.Add(conversationSentenceOption);
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x0009C2CC File Offset: 0x0009A4CC
		public void GetPlayerSentenceOptions()
		{
			this.CurOptions = this.GetSentenceOptions(true, true);
			if (this.CurOptions.Count > 0)
			{
				ConversationSentenceOption conversationSentenceOption = this.CurOptions[0];
				foreach (ConversationSentenceOption conversationSentenceOption2 in this.CurOptions)
				{
					if (this._sentences[conversationSentenceOption2.SentenceNo].IsListener != null)
					{
						conversationSentenceOption = conversationSentenceOption2;
						break;
					}
				}
				this.UpdateSpeakerAndListenerAgents(this._sentences[conversationSentenceOption.SentenceNo]);
			}
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x0009C374 File Offset: 0x0009A574
		public int GetStateIndex(string str)
		{
			int num;
			if (this.stateMap.ContainsKey(str))
			{
				num = this.stateMap[str];
			}
			else
			{
				num = this._numberOfStateIndices;
				Dictionary<string, int> dictionary = this.stateMap;
				int numberOfStateIndices = this._numberOfStateIndices;
				this._numberOfStateIndices = numberOfStateIndices + 1;
				dictionary.Add(str, numberOfStateIndices);
			}
			return num;
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x0009C3C3 File Offset: 0x0009A5C3
		internal void Build()
		{
			this.SortSentences();
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x0009C3CB File Offset: 0x0009A5CB
		public void DisableSentenceSort()
		{
			this._sortSentenceIsDisabled = true;
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x0009C3D4 File Offset: 0x0009A5D4
		public void EnableSentenceSort()
		{
			this._sortSentenceIsDisabled = false;
			this.SortSentences();
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x0009C3E3 File Offset: 0x0009A5E3
		private void SortSentences()
		{
			this._sentences = this._sentences.OrderByDescending<ConversationSentence, int>((ConversationSentence pair) => pair.Priority).ToList<ConversationSentence>();
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x0009C41C File Offset: 0x0009A61C
		private void SortLastSentence()
		{
			int num = this._sentences.Count - 1;
			ConversationSentence conversationSentence = this._sentences[num];
			int priority = conversationSentence.Priority;
			int num2 = num - 1;
			while (num2 >= 0 && this._sentences[num2].Priority < priority)
			{
				this._sentences[num2 + 1] = this._sentences[num2];
				num = num2;
				num2--;
			}
			this._sentences[num] = conversationSentence;
			if (this.CurOptions != null)
			{
				for (int i = 0; i < this.CurOptions.Count; i++)
				{
					if (this.CurOptions[i].SentenceNo >= num)
					{
						ConversationSentenceOption conversationSentenceOption = this.CurOptions[i];
						conversationSentenceOption.SentenceNo = this.CurOptions[i].SentenceNo + 1;
						this.CurOptions[i] = conversationSentenceOption;
					}
				}
			}
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0009C508 File Offset: 0x0009A708
		private List<ConversationSentenceOption> GetSentenceOptions(bool onlyPlayer, bool processAfterOneOption)
		{
			List<ConversationSentenceOption> list = new List<ConversationSentenceOption>();
			ConversationManager.SetupTextVariables();
			for (int i = 0; i < this._sentences.Count; i++)
			{
				if (this.GetSentenceMatch(i, onlyPlayer))
				{
					ConversationSentence conversationSentence = this._sentences[i];
					int num = 1;
					this._dialogRepeatLines.Clear();
					this._currentRepeatIndex = 0;
					if (conversationSentence.IsRepeatable)
					{
						num = this.DialogRepeatCount;
					}
					for (int j = 0; j < num; j++)
					{
						this._dialogRepeatLines.Add(conversationSentence.Text.CopyTextObject());
						if (conversationSentence.RunCondition())
						{
							conversationSentence.IsClickable = conversationSentence.RunClickableCondition();
							if (conversationSentence.IsWithVariation)
							{
								TextObject textObject = this.FindMatchingTextOrNull(conversationSentence.Id, this.OneToOneConversationCharacter);
								GameTexts.SetVariable("VARIATION_TEXT_TAGGED_LINE", textObject);
							}
							string text = (conversationSentence.IsPlayer ? "P  -> (" : "AI -> (") + conversationSentence.Id + ") - ";
							ConversationSentenceOption conversationSentenceOption = new ConversationSentenceOption
							{
								SentenceNo = i,
								Text = this.GetCurrentDialogLine(),
								Id = conversationSentence.Id,
								RepeatObject = this.GetCurrentProcessedRepeatObject(),
								DebugInfo = text,
								IsClickable = conversationSentence.IsClickable,
								HasPersuasion = conversationSentence.HasPersuasion,
								SkillName = conversationSentence.SkillName,
								TraitName = conversationSentence.TraitName,
								IsSpecial = conversationSentence.IsSpecial,
								IsUsedOnce = conversationSentence.IsUsedOnce,
								HintText = conversationSentence.HintText,
								PersuationOptionArgs = conversationSentence.PersuationOptionArgs
							};
							list.Add(conversationSentenceOption);
							if (conversationSentence.IsRepeatable)
							{
								this._currentRepeatIndex++;
							}
							if (!processAfterOneOption)
							{
								return list;
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x0009C6E4 File Offset: 0x0009A8E4
		private bool GetSentenceMatch(int sentenceIndex, bool onlyPlayer)
		{
			if (0 > sentenceIndex || sentenceIndex >= this._sentences.Count)
			{
				throw new MBOutOfRangeException("Sentence index is not valid.");
			}
			bool flag = this._sentences[sentenceIndex].InputToken != this.ActiveToken;
			if (!flag && onlyPlayer)
			{
				flag = !this._sentences[sentenceIndex].IsPlayer;
			}
			if (!flag)
			{
				flag = this._sentences[sentenceIndex].IsUsedOnce && this._usedIndices.Contains(this._sentences[sentenceIndex].Index);
			}
			return !flag;
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x0009C782 File Offset: 0x0009A982
		internal object GetCurrentProcessedRepeatObject()
		{
			if (this._dialogRepeatObjects.Count <= 0)
			{
				return null;
			}
			return this._dialogRepeatObjects[this._currentRepeatedDialogSetIndex][this._currentRepeatIndex];
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x0009C7B0 File Offset: 0x0009A9B0
		internal TextObject GetCurrentDialogLine()
		{
			if (this._dialogRepeatLines.Count <= this._currentRepeatIndex)
			{
				return null;
			}
			return this._dialogRepeatLines[this._currentRepeatIndex];
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x0009C7D8 File Offset: 0x0009A9D8
		internal object GetSelectedRepeatObject()
		{
			return this._lastSelectedDialogObject;
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x0009C7E0 File Offset: 0x0009A9E0
		internal void SetDialogRepeatCount(IReadOnlyList<object> dialogRepeatObjects, int maxRepeatedDialogsInConversation)
		{
			this._dialogRepeatObjects.Clear();
			bool flag = dialogRepeatObjects.Count > maxRepeatedDialogsInConversation + 1;
			List<object> list = new List<object>(maxRepeatedDialogsInConversation);
			for (int i = 0; i < dialogRepeatObjects.Count; i++)
			{
				object obj = dialogRepeatObjects[i];
				if (flag && i % maxRepeatedDialogsInConversation == 0)
				{
					list = new List<object>(maxRepeatedDialogsInConversation);
					this._dialogRepeatObjects.Add(list);
				}
				list.Add(obj);
			}
			if (!flag && !list.IsEmpty<object>())
			{
				this._dialogRepeatObjects.Add(list);
			}
			this._currentRepeatedDialogSetIndex = 0;
			this._currentRepeatIndex = 0;
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x0009C86C File Offset: 0x0009AA6C
		internal static void DialogRepeatContinueListing()
		{
			Campaign campaign = Campaign.Current;
			ConversationManager conversationManager = ((campaign != null) ? campaign.ConversationManager : null);
			if (conversationManager != null)
			{
				conversationManager._currentRepeatedDialogSetIndex++;
				if (conversationManager._currentRepeatedDialogSetIndex >= conversationManager._dialogRepeatObjects.Count)
				{
					conversationManager._currentRepeatedDialogSetIndex = 0;
				}
				conversationManager._currentRepeatIndex = 0;
			}
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x0009C8C0 File Offset: 0x0009AAC0
		internal static bool IsThereMultipleRepeatablePages()
		{
			Campaign campaign = Campaign.Current;
			if (campaign == null)
			{
				return false;
			}
			ConversationManager conversationManager = campaign.ConversationManager;
			int? num = ((conversationManager != null) ? new int?(conversationManager._dialogRepeatObjects.Count) : null);
			int num2 = 1;
			return (num.GetValueOrDefault() > num2) & (num != null);
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x0009C910 File Offset: 0x0009AB10
		private void ResetRepeatedDialogSystem()
		{
			this._currentRepeatedDialogSetIndex = 0;
			this._currentRepeatIndex = 0;
			this._dialogRepeatObjects.Clear();
			this._dialogRepeatLines.Clear();
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x0009C936 File Offset: 0x0009AB36
		internal ConversationSentence AddDialogLine(ConversationSentence dialogLine)
		{
			this._sentences.Add(dialogLine);
			if (!this._sortSentenceIsDisabled)
			{
				this.SortLastSentence();
			}
			return dialogLine;
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x0009C954 File Offset: 0x0009AB54
		public void AddDialogFlow(DialogFlow dialogFlow, object relatedObject = null)
		{
			foreach (DialogFlowLine dialogFlowLine in dialogFlow.Lines)
			{
				string text = this.CreateId();
				uint num = (dialogFlowLine.ByPlayer ? 1U : 0U) | (dialogFlowLine.IsRepeatable ? 2U : 0U) | (dialogFlowLine.IsSpecialOption ? 4U : 0U) | (dialogFlowLine.IsUsedOnce ? 8U : 0U);
				this.AddDialogLine(new ConversationSentence(text, dialogFlowLine.HasVariation ? new TextObject("{=!}{VARIATION_TEXT_TAGGED_LINE}", null) : dialogFlowLine.Text, dialogFlowLine.InputToken, dialogFlowLine.OutputToken, dialogFlowLine.ConditionDelegate, dialogFlowLine.ClickableConditionDelegate, dialogFlowLine.ConsequenceDelegate, num, dialogFlow.Priority, 0, 0, relatedObject, dialogFlowLine.HasVariation, dialogFlowLine.SpeakerDelegate, dialogFlowLine.ListenerDelegate, null));
				GameText gameText = Game.Current.GameTextManager.AddGameText(text);
				foreach (KeyValuePair<TextObject, List<GameTextManager.ChoiceTag>> keyValuePair in dialogFlowLine.Variations)
				{
					gameText.AddVariationWithId("", keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0009CACC File Offset: 0x0009ACCC
		public ConversationSentence AddDialogLineMultiAgent(string id, string inputToken, string outputToken, TextObject text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, int agentIndex, int nextAgentIndex, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, text, inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 0U, priority, agentIndex, nextAgentIndex, null, false, null, null, null));
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0009CAFB File Offset: 0x0009ACFB
		internal string CreateToken()
		{
			string text = string.Format("atk:{0}", this._autoToken);
			this._autoToken++;
			return text;
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x0009CB20 File Offset: 0x0009AD20
		private string CreateId()
		{
			string text = string.Format("adg:{0}", this._autoId);
			this._autoId++;
			return text;
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x0009CB45 File Offset: 0x0009AD45
		internal void SetupGameStringsForConversation()
		{
			StringHelpers.SetCharacterProperties("PLAYER", Hero.MainHero.CharacterObject, null, false);
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0009CB5E File Offset: 0x0009AD5E
		internal void OnConsequence(ConversationSentence sentence)
		{
			Action<ConversationSentence> consequenceRunned = this.ConsequenceRunned;
			if (consequenceRunned == null)
			{
				return;
			}
			consequenceRunned(sentence);
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x0009CB71 File Offset: 0x0009AD71
		internal void OnCondition(ConversationSentence sentence)
		{
			Action<ConversationSentence> conditionRunned = this.ConditionRunned;
			if (conditionRunned == null)
			{
				return;
			}
			conditionRunned(sentence);
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x0009CB84 File Offset: 0x0009AD84
		internal void OnClickableCondition(ConversationSentence sentence)
		{
			Action<ConversationSentence> clickableConditionRunned = this.ClickableConditionRunned;
			if (clickableConditionRunned == null)
			{
				return;
			}
			clickableConditionRunned(sentence);
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600233A RID: 9018 RVA: 0x0009CB98 File Offset: 0x0009AD98
		// (remove) Token: 0x0600233B RID: 9019 RVA: 0x0009CBD0 File Offset: 0x0009ADD0
		public event Action<ConversationSentence> ConsequenceRunned;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600233C RID: 9020 RVA: 0x0009CC08 File Offset: 0x0009AE08
		// (remove) Token: 0x0600233D RID: 9021 RVA: 0x0009CC40 File Offset: 0x0009AE40
		public event Action<ConversationSentence> ConditionRunned;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600233E RID: 9022 RVA: 0x0009CC78 File Offset: 0x0009AE78
		// (remove) Token: 0x0600233F RID: 9023 RVA: 0x0009CCB0 File Offset: 0x0009AEB0
		public event Action<ConversationSentence> ClickableConditionRunned;

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002340 RID: 9024 RVA: 0x0009CCE5 File Offset: 0x0009AEE5
		public IReadOnlyList<IAgent> ConversationAgents
		{
			get
			{
				return this._conversationAgents;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002341 RID: 9025 RVA: 0x0009CCED File Offset: 0x0009AEED
		public IAgent OneToOneConversationAgent
		{
			get
			{
				if (this.ConversationAgents.IsEmpty<IAgent>() || this.ConversationAgents.Count > 1)
				{
					return null;
				}
				return this.ConversationAgents[0];
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002342 RID: 9026 RVA: 0x0009CD18 File Offset: 0x0009AF18
		public IAgent SpeakerAgent
		{
			get
			{
				if (this.ConversationAgents != null)
				{
					return this._speakerAgent;
				}
				return null;
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002343 RID: 9027 RVA: 0x0009CD2A File Offset: 0x0009AF2A
		public IAgent ListenerAgent
		{
			get
			{
				if (this.ConversationAgents != null)
				{
					return this._listenerAgent;
				}
				return null;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06002344 RID: 9028 RVA: 0x0009CD3C File Offset: 0x0009AF3C
		// (set) Token: 0x06002345 RID: 9029 RVA: 0x0009CD44 File Offset: 0x0009AF44
		public bool IsConversationInProgress { get; private set; }

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06002346 RID: 9030 RVA: 0x0009CD4D File Offset: 0x0009AF4D
		public Hero OneToOneConversationHero
		{
			get
			{
				if (this.OneToOneConversationCharacter != null)
				{
					return this.OneToOneConversationCharacter.HeroObject;
				}
				return null;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06002347 RID: 9031 RVA: 0x0009CD64 File Offset: 0x0009AF64
		public CharacterObject OneToOneConversationCharacter
		{
			get
			{
				if (this.OneToOneConversationAgent != null)
				{
					return (CharacterObject)this.OneToOneConversationAgent.Character;
				}
				return null;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06002348 RID: 9032 RVA: 0x0009CD80 File Offset: 0x0009AF80
		public IEnumerable<CharacterObject> ConversationCharacters
		{
			get
			{
				new List<CharacterObject>();
				foreach (IAgent agent in this.ConversationAgents)
				{
					yield return (CharacterObject)agent.Character;
				}
				IEnumerator<IAgent> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x0009CD90 File Offset: 0x0009AF90
		public bool IsAgentInConversation(IAgent agent)
		{
			return this.ConversationAgents.Contains(agent);
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x0600234A RID: 9034 RVA: 0x0009CD9E File Offset: 0x0009AF9E
		public MobileParty ConversationParty
		{
			get
			{
				return this._conversationParty;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x0600234B RID: 9035 RVA: 0x0009CDA6 File Offset: 0x0009AFA6
		// (set) Token: 0x0600234C RID: 9036 RVA: 0x0009CDAE File Offset: 0x0009AFAE
		public bool NeedsToActivateForMapConversation { get; private set; }

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600234D RID: 9037 RVA: 0x0009CDB8 File Offset: 0x0009AFB8
		// (remove) Token: 0x0600234E RID: 9038 RVA: 0x0009CDF0 File Offset: 0x0009AFF0
		public event Action ConversationSetup;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600234F RID: 9039 RVA: 0x0009CE28 File Offset: 0x0009B028
		// (remove) Token: 0x06002350 RID: 9040 RVA: 0x0009CE60 File Offset: 0x0009B060
		public event Action ConversationBegin;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06002351 RID: 9041 RVA: 0x0009CE98 File Offset: 0x0009B098
		// (remove) Token: 0x06002352 RID: 9042 RVA: 0x0009CED0 File Offset: 0x0009B0D0
		public event Action ConversationEnd;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06002353 RID: 9043 RVA: 0x0009CF08 File Offset: 0x0009B108
		// (remove) Token: 0x06002354 RID: 9044 RVA: 0x0009CF40 File Offset: 0x0009B140
		public event Action ConversationEndOneShot;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06002355 RID: 9045 RVA: 0x0009CF78 File Offset: 0x0009B178
		// (remove) Token: 0x06002356 RID: 9046 RVA: 0x0009CFB0 File Offset: 0x0009B1B0
		public event Action ConversationContinued;

		// Token: 0x06002357 RID: 9047 RVA: 0x0009CFE5 File Offset: 0x0009B1E5
		private void SetupConversation()
		{
			this.IsConversationInProgress = true;
			IConversationStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnConversationInstall();
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x0009CFFE File Offset: 0x0009B1FE
		public void BeginConversation()
		{
			this.IsConversationInProgress = true;
			if (this.ConversationSetup != null)
			{
				this.ConversationSetup();
			}
			if (this.ConversationBegin != null)
			{
				this.ConversationBegin();
			}
			this.NeedsToActivateForMapConversation = false;
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x0009D034 File Offset: 0x0009B234
		public void EndConversation()
		{
			Debug.Print("--------------- Conversation End --------------- ", 0, Debug.DebugColor.White, 4503599627370496UL);
			if (CampaignMission.Current != null)
			{
				foreach (IAgent agent in this.ConversationAgents)
				{
					CampaignMission.Current.OnConversationEnd(agent);
				}
			}
			this._conversationParty = null;
			if (this.ConversationEndOneShot != null)
			{
				this.ConversationEndOneShot();
				this.ConversationEndOneShot = null;
			}
			if (this.ConversationEnd != null)
			{
				this.ConversationEnd();
			}
			this.IsConversationInProgress = false;
			foreach (IAgent agent2 in this.ConversationAgents)
			{
				agent2.SetAsConversationAgent(false);
			}
			Campaign.Current.CurrentConversationContext = ConversationContext.Default;
			CampaignEventDispatcher.Instance.OnConversationEnded(this.ConversationCharacters);
			if (ConversationManager.GetPersuasionIsActive())
			{
				ConversationManager.EndPersuasion();
			}
			this._conversationAgents.Clear();
			this._speakerAgent = null;
			this._listenerAgent = null;
			this._mainAgent = null;
			if (this.IsConversationFlowActive)
			{
				this.OnConversationDeactivate();
			}
			IConversationStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnConversationUninstall();
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x0009D17C File Offset: 0x0009B37C
		public void DoOption(int optionIndex)
		{
			this.LastSelectedButtonIndex = optionIndex;
			this.ProcessSentence(this.CurOptions[optionIndex]);
			if (this._isActive)
			{
				this.DoOptionContinue();
				return;
			}
			this._executeDoOptionContinue = true;
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x0009D1B0 File Offset: 0x0009B3B0
		public void DoOption(string optionID)
		{
			int count = Campaign.Current.ConversationManager.CurOptions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.CurOptions[i].Id == optionID)
				{
					this.DoOption(i);
					return;
				}
			}
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x0009D1FF File Offset: 0x0009B3FF
		public void DoConversationContinuedCallback()
		{
			if (this.ConversationContinued != null)
			{
				this.ConversationContinued();
			}
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x0009D214 File Offset: 0x0009B414
		public void DoOptionContinue()
		{
			if (this.IsConversationEnded() && this._sentences[this._currentSentence].IsPlayer)
			{
				this.EndConversation();
				return;
			}
			this.ProcessPartnerSentence();
			this.DoConversationContinuedCallback();
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x0009D24C File Offset: 0x0009B44C
		public void ContinueConversation()
		{
			if (this.CurOptions.Count <= 1)
			{
				if (this.IsConversationEnded())
				{
					this.EndConversation();
					return;
				}
				if (!this.ProcessPartnerSentence() && this.ListenerAgent.Character == Hero.MainHero.CharacterObject)
				{
					this.EndConversation();
					return;
				}
				this.DoConversationContinuedCallback();
				if (CampaignMission.Current != null)
				{
					CampaignMission.Current.OnConversationContinue();
				}
			}
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x0009D2B4 File Offset: 0x0009B4B4
		public void SetupAndStartMissionConversation(IAgent agent, IAgent mainAgent, bool setActionsInstantly)
		{
			this.SetupConversation();
			this._mainAgent = mainAgent;
			this._conversationAgents.Clear();
			this.AddConversationAgent(agent);
			this._conversationParty = null;
			this.StartNew(0, setActionsInstantly);
			if (!this.IsConversationFlowActive)
			{
				this.OnConversationActivate();
			}
			this.BeginConversation();
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0009D304 File Offset: 0x0009B504
		public void SetupAndStartMissionConversationWithMultipleAgents(IEnumerable<IAgent> agents, IAgent mainAgent)
		{
			this.SetupConversation();
			this._mainAgent = mainAgent;
			this._conversationAgents.Clear();
			this.AddConversationAgents(agents, true);
			this._conversationParty = null;
			this.StartNew(0, true);
			if (!this.IsConversationFlowActive)
			{
				this.OnConversationActivate();
			}
			this.BeginConversation();
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0009D354 File Offset: 0x0009B554
		public void SetupAndStartMapConversation(MobileParty party, IAgent agent, IAgent mainAgent)
		{
			this._conversationParty = party;
			this._mainAgent = mainAgent;
			this._conversationAgents.Clear();
			this.AddConversationAgent(agent);
			this.SetupConversation();
			this.StartNew(0, true);
			this.NeedsToActivateForMapConversation = true;
			if (!this.IsConversationFlowActive)
			{
				this.OnConversationActivate();
			}
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0009D3A4 File Offset: 0x0009B5A4
		public void AddConversationAgents(IEnumerable<IAgent> agents, bool setActionsInstantly)
		{
			foreach (IAgent agent in agents)
			{
				if (agent.IsActive() && !this.ConversationAgents.Contains(agent))
				{
					this.AddConversationAgent(agent);
					CampaignMission.Current.OnConversationStart(agent, setActionsInstantly);
				}
			}
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x0009D410 File Offset: 0x0009B610
		public void RemoveConversationAgent(IAgent agent)
		{
			if (agent.IsActive() && this.ConversationAgents.Contains(agent) && this.ConversationAgents.Count > 1)
			{
				CampaignMission.Current.OnConversationEnd(agent);
				agent.SetAsConversationAgent(false);
				this._conversationAgents.Remove(agent);
				return;
			}
			Debug.FailedAssert("Failed to remove conversation agent.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Conversation\\ConversationManager.cs", "RemoveConversationAgent", 1249);
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x0009D47A File Offset: 0x0009B67A
		private void AddConversationAgent(IAgent agent)
		{
			this._conversationAgents.Add(agent);
			agent.SetAsConversationAgent(true);
			CampaignEventDispatcher.Instance.OnAgentJoinedConversation(agent);
			agent.OnConversationStarted();
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x0009D4A0 File Offset: 0x0009B6A0
		public bool IsConversationAgent(IAgent agent)
		{
			return this.ConversationAgents != null && this.ConversationAgents.Contains(agent);
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x0009D4B8 File Offset: 0x0009B6B8
		public void RemoveRelatedLines(object o)
		{
			this._sentences.RemoveAll((ConversationSentence s) => s.RelatedObject == o);
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002367 RID: 9063 RVA: 0x0009D4EA File Offset: 0x0009B6EA
		// (set) Token: 0x06002368 RID: 9064 RVA: 0x0009D4F2 File Offset: 0x0009B6F2
		public IConversationStateHandler Handler { get; set; }

		// Token: 0x06002369 RID: 9065 RVA: 0x0009D4FB File Offset: 0x0009B6FB
		public void OnConversationDeactivate()
		{
			this._isActive = false;
			IConversationStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnConversationDeactivate();
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x0009D514 File Offset: 0x0009B714
		public void OnConversationActivate()
		{
			this._isActive = true;
			if (this._executeDoOptionContinue)
			{
				this._executeDoOptionContinue = false;
				this.DoOptionContinue();
			}
			IConversationStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnConversationActivate();
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x0009D544 File Offset: 0x0009B744
		public TextObject FindMatchingTextOrNull(string id, CharacterObject character)
		{
			float num = -2.1474836E+09f;
			TextObject textObject = null;
			GameText gameText = Game.Current.GameTextManager.GetGameText(id);
			if (gameText != null)
			{
				foreach (GameText.GameTextVariation gameTextVariation in gameText.Variations)
				{
					float num2 = this.FindMatchingScore(character, gameTextVariation.Tags);
					if (num2 > num)
					{
						textObject = gameTextVariation.Text;
						num = num2;
					}
				}
			}
			return textObject;
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x0009D5C8 File Offset: 0x0009B7C8
		private float FindMatchingScore(CharacterObject character, GameTextManager.ChoiceTag[] choiceTags)
		{
			float num = 0f;
			foreach (GameTextManager.ChoiceTag choiceTag in choiceTags)
			{
				if (choiceTag.TagName != "DefaultTag")
				{
					if (this.IsTagApplicable(choiceTag.TagName, character) == choiceTag.IsTagReversed)
					{
						return -2.1474836E+09f;
					}
					uint weight = choiceTag.Weight;
					num += weight;
				}
			}
			return num;
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x0009D634 File Offset: 0x0009B834
		private void InitializeTags()
		{
			this._tags = new Dictionary<string, ConversationTag>();
			string name = typeof(ConversationTag).Assembly.GetName().Name;
			foreach (Assembly assembly in ModuleHelper.GetActiveGameAssemblies())
			{
				bool flag = false;
				if (name == assembly.GetName().Name)
				{
					flag = true;
				}
				else
				{
					AssemblyName[] referencedAssembliesSafe = assembly.GetReferencedAssembliesSafe();
					for (int i = 0; i < referencedAssembliesSafe.Length; i++)
					{
						if (referencedAssembliesSafe[i].Name == name)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					foreach (Type type in assembly.GetTypesSafe(null))
					{
						if (type.IsSubclassOf(typeof(ConversationTag)))
						{
							ConversationTag conversationTag = Activator.CreateInstance(type) as ConversationTag;
							this._tags.Add(conversationTag.StringId, conversationTag);
						}
					}
				}
			}
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x0009D76C File Offset: 0x0009B96C
		private static void SetupTextVariables()
		{
			StringHelpers.SetCharacterProperties("PLAYER", Hero.MainHero.CharacterObject, null, false);
			int num = 1;
			foreach (CharacterObject characterObject in CharacterObject.ConversationCharacters)
			{
				string text = ((num == 1) ? "" : ("_" + num));
				StringHelpers.SetCharacterProperties("CONVERSATION_CHARACTER" + text, characterObject, null, false);
			}
			MBTextManager.SetTextVariable("CURRENT_SETTLEMENT_NAME", (Settlement.CurrentSettlement == null) ? TextObject.GetEmpty() : Settlement.CurrentSettlement.Name, false);
			ConversationHelper.ConversationTroopCommentShown = false;
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x0009D824 File Offset: 0x0009BA24
		public IEnumerable<string> GetApplicableTagNames(CharacterObject character)
		{
			foreach (ConversationTag conversationTag in this._tags.Values)
			{
				if (conversationTag.IsApplicableTo(character))
				{
					yield return conversationTag.StringId;
				}
			}
			Dictionary<string, ConversationTag>.ValueCollection.Enumerator enumerator = default(Dictionary<string, ConversationTag>.ValueCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x0009D83C File Offset: 0x0009BA3C
		public bool IsTagApplicable(string tagId, CharacterObject character)
		{
			ConversationTag conversationTag;
			if (this._tags.TryGetValue(tagId, out conversationTag))
			{
				return conversationTag.IsApplicableTo(character);
			}
			Debug.FailedAssert("Asking for a nonexistent tag: " + tagId, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Conversation\\ConversationManager.cs", "IsTagApplicable", 1486);
			return false;
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x0009D884 File Offset: 0x0009BA84
		public void OpenMapConversation(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData)
		{
			GameStateManager gameStateManager = GameStateManager.Current;
			(((gameStateManager != null) ? gameStateManager.ActiveState : null) as MapState).OnMapConversationStarts(playerCharacterData, conversationPartnerData);
			PartyBase party = conversationPartnerData.Party;
			this.SetupAndStartMapConversation((party != null) ? party.MobileParty : null, new MapConversationAgent(conversationPartnerData.Character), new MapConversationAgent(CharacterObject.PlayerCharacter));
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x0009D8DB File Offset: 0x0009BADB
		public static void StartPersuasion(float goalValue, float successValue, float failValue, float criticalSuccessValue, float criticalFailValue, float initialProgress = -1f, PersuasionDifficulty difficulty = PersuasionDifficulty.Medium)
		{
			ConversationManager._persuasion = new Persuasion(goalValue, successValue, failValue, criticalSuccessValue, criticalFailValue, initialProgress, difficulty);
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x0009D8F1 File Offset: 0x0009BAF1
		public static void EndPersuasion()
		{
			ConversationManager._persuasion = null;
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x0009D8F9 File Offset: 0x0009BAF9
		public static void PersuasionCommitProgress(PersuasionOptionArgs persuasionOptionArgs)
		{
			ConversationManager._persuasion.CommitProgress(persuasionOptionArgs);
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x0009D906 File Offset: 0x0009BB06
		public static void Clear()
		{
			ConversationManager._persuasion = null;
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x0009D90E File Offset: 0x0009BB0E
		public void GetPersuasionChanceValues(out float successValue, out float critSuccessValue, out float critFailValue)
		{
			successValue = ConversationManager._persuasion.SuccessValue;
			critSuccessValue = ConversationManager._persuasion.CriticalSuccessValue;
			critFailValue = ConversationManager._persuasion.CriticalFailValue;
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x0009D934 File Offset: 0x0009BB34
		public static bool GetPersuasionIsActive()
		{
			return ConversationManager._persuasion != null;
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x0009D93E File Offset: 0x0009BB3E
		public static bool GetPersuasionProgressSatisfied()
		{
			return ConversationManager._persuasion.Progress >= ConversationManager._persuasion.GoalValue;
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x0009D959 File Offset: 0x0009BB59
		public static bool GetPersuasionIsFailure()
		{
			return ConversationManager._persuasion.Progress < 0f;
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x0009D96C File Offset: 0x0009BB6C
		public static float GetPersuasionProgress()
		{
			return ConversationManager._persuasion.Progress;
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x0009D978 File Offset: 0x0009BB78
		public static float GetPersuasionGoalValue()
		{
			return ConversationManager._persuasion.GoalValue;
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x0009D984 File Offset: 0x0009BB84
		public static IEnumerable<Tuple<PersuasionOptionArgs, PersuasionOptionResult>> GetPersuasionChosenOptions()
		{
			return ConversationManager._persuasion.GetChosenOptions();
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x0009D990 File Offset: 0x0009BB90
		public void GetPersuasionChances(ConversationSentenceOption conversationSentenceOption, out float successChance, out float critSuccessChance, out float critFailChance, out float failChance)
		{
			ConversationSentence conversationSentence = this._sentences[conversationSentenceOption.SentenceNo];
			if (conversationSentenceOption.HasPersuasion)
			{
				Campaign.Current.Models.PersuasionModel.GetChances(conversationSentence.PersuationOptionArgs, out successChance, out critSuccessChance, out critFailChance, out failChance, ConversationManager._persuasion.DifficultyMultiplier);
				return;
			}
			successChance = 0f;
			critSuccessChance = 0f;
			critFailChance = 0f;
			failChance = 0f;
		}

		// Token: 0x04000A48 RID: 2632
		private int _currentRepeatedDialogSetIndex;

		// Token: 0x04000A49 RID: 2633
		private int _currentRepeatIndex;

		// Token: 0x04000A4A RID: 2634
		private int _autoId;

		// Token: 0x04000A4B RID: 2635
		private int _autoToken;

		// Token: 0x04000A4C RID: 2636
		private HashSet<int> _usedIndices = new HashSet<int>();

		// Token: 0x04000A4D RID: 2637
		private int _numConversationSentencesCreated;

		// Token: 0x04000A4E RID: 2638
		private List<ConversationSentence> _sentences;

		// Token: 0x04000A4F RID: 2639
		private int _numberOfStateIndices;

		// Token: 0x04000A50 RID: 2640
		public int ActiveToken;

		// Token: 0x04000A51 RID: 2641
		private int _currentSentence;

		// Token: 0x04000A52 RID: 2642
		private TextObject _currentSentenceText;

		// Token: 0x04000A53 RID: 2643
		public List<Tuple<string, CharacterObject>> DetailedDebugLog = new List<Tuple<string, CharacterObject>>();

		// Token: 0x04000A54 RID: 2644
		public string CurrentFaceAnimationRecord;

		// Token: 0x04000A55 RID: 2645
		private object _lastSelectedDialogObject;

		// Token: 0x04000A56 RID: 2646
		private readonly List<List<object>> _dialogRepeatObjects = new List<List<object>>();

		// Token: 0x04000A57 RID: 2647
		private readonly List<TextObject> _dialogRepeatLines = new List<TextObject>();

		// Token: 0x04000A58 RID: 2648
		private bool _isActive;

		// Token: 0x04000A59 RID: 2649
		private bool _executeDoOptionContinue;

		// Token: 0x04000A5A RID: 2650
		public int LastSelectedButtonIndex;

		// Token: 0x04000A5B RID: 2651
		public ConversationAnimationManager ConversationAnimationManager;

		// Token: 0x04000A5C RID: 2652
		private IAgent _mainAgent;

		// Token: 0x04000A5D RID: 2653
		private IAgent _speakerAgent;

		// Token: 0x04000A5E RID: 2654
		private IAgent _listenerAgent;

		// Token: 0x04000A5F RID: 2655
		private Dictionary<string, ConversationTag> _tags;

		// Token: 0x04000A60 RID: 2656
		private bool _sortSentenceIsDisabled;

		// Token: 0x04000A61 RID: 2657
		private Dictionary<string, int> stateMap;

		// Token: 0x04000A66 RID: 2662
		private List<IAgent> _conversationAgents = new List<IAgent>();

		// Token: 0x04000A68 RID: 2664
		public bool CurrentConversationIsFirst;

		// Token: 0x04000A69 RID: 2665
		private MobileParty _conversationParty;

		// Token: 0x04000A71 RID: 2673
		private static Persuasion _persuasion;

		// Token: 0x02000674 RID: 1652
		public class TaggedString
		{
			// Token: 0x04001AE4 RID: 6884
			public TextObject Text;

			// Token: 0x04001AE5 RID: 6885
			public List<GameTextManager.ChoiceTag> ChoiceTags = new List<GameTextManager.ChoiceTag>();

			// Token: 0x04001AE6 RID: 6886
			public int FacialAnimation;
		}
	}
}
