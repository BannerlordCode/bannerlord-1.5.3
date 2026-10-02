using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Library;

namespace SandBox.View.Conversation
{
	// Token: 0x0200007E RID: 126
	public class ConversationViewManager
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00029057 File Offset: 0x00027257
		public static ConversationViewManager Instance
		{
			get
			{
				return SandBoxViewSubModule.ConversationViewManager;
			}
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00029060 File Offset: 0x00027260
		public ConversationViewManager()
		{
			this.FillEventHandlers();
			Campaign.Current.ConversationManager.ConditionRunned += this.OnCondition;
			Campaign.Current.ConversationManager.ConsequenceRunned += this.OnConsequence;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x000290B0 File Offset: 0x000272B0
		private void FillEventHandlers()
		{
			this._conditionEventHandlers = new Dictionary<string, ConversationViewEventHandlerDelegate>();
			this._consequenceEventHandlers = new Dictionary<string, ConversationViewEventHandlerDelegate>();
			Assembly assembly = typeof(ConversationViewEventHandlerDelegate).Assembly;
			this.FillEventHandlersWith(assembly);
			foreach (Assembly assembly2 in assembly.GetReferencingAssembliesSafe(null))
			{
				this.FillEventHandlersWith(assembly2);
			}
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0002910C File Offset: 0x0002730C
		private void FillEventHandlersWith(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypesSafe(null))
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					object[] customAttributesSafe = methodInfo.GetCustomAttributesSafe(typeof(ConversationViewEventHandler), false);
					if (customAttributesSafe != null && customAttributesSafe.Length != 0)
					{
						foreach (ConversationViewEventHandler conversationViewEventHandler in customAttributesSafe)
						{
							ConversationViewEventHandlerDelegate conversationViewEventHandlerDelegate = Delegate.CreateDelegate(typeof(ConversationViewEventHandlerDelegate), methodInfo) as ConversationViewEventHandlerDelegate;
							if (conversationViewEventHandler.Type == ConversationViewEventHandler.EventType.OnCondition)
							{
								if (!this._conditionEventHandlers.ContainsKey(conversationViewEventHandler.Id))
								{
									this._conditionEventHandlers.Add(conversationViewEventHandler.Id, conversationViewEventHandlerDelegate);
								}
								else
								{
									this._conditionEventHandlers[conversationViewEventHandler.Id] = conversationViewEventHandlerDelegate;
								}
							}
							else if (conversationViewEventHandler.Type == ConversationViewEventHandler.EventType.OnConsequence)
							{
								if (!this._consequenceEventHandlers.ContainsKey(conversationViewEventHandler.Id))
								{
									this._consequenceEventHandlers.Add(conversationViewEventHandler.Id, conversationViewEventHandlerDelegate);
								}
								else
								{
									this._consequenceEventHandlers[conversationViewEventHandler.Id] = conversationViewEventHandlerDelegate;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00029280 File Offset: 0x00027480
		private void OnConsequence(ConversationSentence sentence)
		{
			ConversationViewEventHandlerDelegate conversationViewEventHandlerDelegate;
			if (this._consequenceEventHandlers.TryGetValue(sentence.Id, out conversationViewEventHandlerDelegate))
			{
				conversationViewEventHandlerDelegate();
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x000292A8 File Offset: 0x000274A8
		private void OnCondition(ConversationSentence sentence)
		{
			ConversationViewEventHandlerDelegate conversationViewEventHandlerDelegate;
			if (this._conditionEventHandlers.TryGetValue(sentence.Id, out conversationViewEventHandlerDelegate))
			{
				conversationViewEventHandlerDelegate();
			}
		}

		// Token: 0x0400028F RID: 655
		private Dictionary<string, ConversationViewEventHandlerDelegate> _conditionEventHandlers;

		// Token: 0x04000290 RID: 656
		private Dictionary<string, ConversationViewEventHandlerDelegate> _consequenceEventHandlers;
	}
}
