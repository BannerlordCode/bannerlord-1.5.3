using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200005B RID: 91
	public class CampaignGameStarter : IGameStarter
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x00027D72 File Offset: 0x00025F72
		public ICollection<CampaignBehaviorBase> CampaignBehaviors
		{
			get
			{
				return this._campaignBehaviors;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x00027D7A File Offset: 0x00025F7A
		public IEnumerable<GameModel> Models
		{
			get
			{
				return this._models;
			}
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00027D82 File Offset: 0x00025F82
		public CampaignGameStarter(GameMenuManager gameMenuManager, ConversationManager conversationManager)
		{
			this._conversationManager = conversationManager;
			this._gameMenuManager = gameMenuManager;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00027DAE File Offset: 0x00025FAE
		public void UnregisterNonReadyObjects()
		{
			Game.Current.ObjectManager.UnregisterNonReadyObjects();
			this._gameMenuManager.UnregisterNonReadyObjects();
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00027DCA File Offset: 0x00025FCA
		public void AddBehavior(CampaignBehaviorBase campaignBehavior)
		{
			if (campaignBehavior != null)
			{
				this._campaignBehaviors.Add(campaignBehavior);
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00027DDC File Offset: 0x00025FDC
		public void RemoveBehaviors<T>() where T : CampaignBehaviorBase
		{
			for (int i = this._campaignBehaviors.Count - 1; i >= 0; i--)
			{
				if (this._campaignBehaviors[i] is T)
				{
					this._campaignBehaviors.RemoveAt(i);
				}
			}
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00027E20 File Offset: 0x00026020
		public bool RemoveBehavior<T>(T behavior) where T : CampaignBehaviorBase
		{
			return this._campaignBehaviors.Remove(behavior);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00027E34 File Offset: 0x00026034
		public T GetModel<T>() where T : GameModel
		{
			for (int i = this._models.Count - 1; i >= 0; i--)
			{
				T t;
				if ((t = this._models[i] as T) != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00027E83 File Offset: 0x00026083
		public void AddModel(GameModel gameModel)
		{
			this._models.Add(gameModel);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00027E94 File Offset: 0x00026094
		public void AddModel<T>(MBGameModel<T> gameModel) where T : GameModel
		{
			T model = this.GetModel<T>();
			gameModel.Initialize(model);
			this._models.Add(gameModel);
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00027EBB File Offset: 0x000260BB
		public void AddGameMenu(string menuId, string menuText, OnInitDelegate initDelegate, GameMenu.MenuOverlayType overlay = GameMenu.MenuOverlayType.None, GameMenu.MenuFlags menuFlags = GameMenu.MenuFlags.None, object relatedObject = null)
		{
			this.GetPresumedGameMenu(menuId).Initialize(new TextObject(menuText, null), initDelegate, overlay, menuFlags, relatedObject);
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00027ED8 File Offset: 0x000260D8
		public void AddWaitGameMenu(string idString, string text, OnInitDelegate initDelegate, OnConditionDelegate condition, OnConsequenceDelegate consequence, OnTickDelegate tick, GameMenu.MenuAndOptionType type, GameMenu.MenuOverlayType overlay = GameMenu.MenuOverlayType.None, float targetWaitHours = 0f, GameMenu.MenuFlags flags = GameMenu.MenuFlags.None, object relatedObject = null)
		{
			this.GetPresumedGameMenu(idString).Initialize(new TextObject(text, null), initDelegate, condition, consequence, tick, type, overlay, targetWaitHours, flags, relatedObject);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00027F0C File Offset: 0x0002610C
		public void AddGameMenuOption(string menuId, string optionId, string optionText, GameMenuOption.OnConditionDelegate condition, GameMenuOption.OnConsequenceDelegate consequence, bool isLeave = false, int index = -1, bool isRepeatable = false, object relatedObject = null)
		{
			this.GetPresumedGameMenu(menuId).AddOption(optionId, new TextObject(optionText, null), condition, consequence, index, isLeave, isRepeatable, relatedObject);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x00027F3C File Offset: 0x0002613C
		public GameMenu GetPresumedGameMenu(string stringId)
		{
			GameMenu gameMenu = this._gameMenuManager.GetGameMenu(stringId);
			if (gameMenu == null)
			{
				gameMenu = new GameMenu(stringId);
				this._gameMenuManager.AddGameMenu(gameMenu);
			}
			return gameMenu;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00027F6D File Offset: 0x0002616D
		private ConversationSentence AddDialogLine(ConversationSentence dialogLine)
		{
			this._conversationManager.AddDialogLine(dialogLine);
			return dialogLine;
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00027F7D File Offset: 0x0002617D
		public void AddDialogFlow(DialogFlow dialogFlow, object relatedObject = null)
		{
			this._conversationManager.AddDialogFlow(dialogFlow, relatedObject);
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00027F8C File Offset: 0x0002618C
		public ConversationSentence AddPlayerLine(string id, string inputToken, string outputToken, string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null, ConversationSentence.OnPersuasionOptionDelegate persuasionOptionDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, new TextObject(text, null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 1U, priority, 0, 0, null, false, null, null, persuasionOptionDelegate));
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00027FC0 File Offset: 0x000261C0
		public ConversationSentence AddRepeatablePlayerLine(string id, string inputToken, string outputToken, string text, string continueListingRepeatedObjectsText, string continueListingOptionOutputToken, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null)
		{
			ConversationSentence conversationSentence = this.AddDialogLine(new ConversationSentence(id, new TextObject(text, null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 3U, priority, 0, 0, null, false, null, null, null));
			this.AddDialogLine(new ConversationSentence(id + "_continue", new TextObject(continueListingRepeatedObjectsText, null), inputToken, continueListingOptionOutputToken, new ConversationSentence.OnConditionDelegate(ConversationManager.IsThereMultipleRepeatablePages), null, new ConversationSentence.OnConsequenceDelegate(ConversationManager.DialogRepeatContinueListing), 1U, priority, 0, 0, null, false, null, null, null));
			return conversationSentence;
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00028038 File Offset: 0x00026238
		public ConversationSentence AddDialogLineWithVariation(string id, string inputToken, string outputToken, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, int priority = 100, string idleActionId = "", string idleFaceAnimId = "", string reactionId = "", string reactionFaceAnimId = "", ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, new TextObject("{=!}{VARIATION_TEXT_TAGGED_LINE}", null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 0U, priority, 0, 0, null, true, null, null, null));
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00028070 File Offset: 0x00026270
		public ConversationSentence AddDialogLine(string id, string inputToken, string outputToken, string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, new TextObject(text, null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 0U, priority, 0, 0, null, false, null, null, null));
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x000280A4 File Offset: 0x000262A4
		public ConversationSentence AddDialogLineMultiAgent(string id, string inputToken, string outputToken, TextObject text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, int agentIndex, int nextAgentIndex, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, text, inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 0U, priority, agentIndex, nextAgentIndex, null, false, null, null, null));
		}

		// Token: 0x040002CA RID: 714
		private readonly GameMenuManager _gameMenuManager;

		// Token: 0x040002CB RID: 715
		private readonly ConversationManager _conversationManager;

		// Token: 0x040002CC RID: 716
		private readonly List<CampaignBehaviorBase> _campaignBehaviors = new List<CampaignBehaviorBase>();

		// Token: 0x040002CD RID: 717
		private readonly List<GameModel> _models = new List<GameModel>();
	}
}
