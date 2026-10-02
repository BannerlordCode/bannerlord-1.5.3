using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x02000248 RID: 584
	public class ConversationSentence
	{
		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06002384 RID: 9092 RVA: 0x0009DA00 File Offset: 0x0009BC00
		// (set) Token: 0x06002385 RID: 9093 RVA: 0x0009DA08 File Offset: 0x0009BC08
		public TextObject Text { get; private set; }

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06002386 RID: 9094 RVA: 0x0009DA11 File Offset: 0x0009BC11
		// (set) Token: 0x06002387 RID: 9095 RVA: 0x0009DA19 File Offset: 0x0009BC19
		public int Index { get; internal set; }

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002388 RID: 9096 RVA: 0x0009DA22 File Offset: 0x0009BC22
		// (set) Token: 0x06002389 RID: 9097 RVA: 0x0009DA2A File Offset: 0x0009BC2A
		public string Id { get; private set; }

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x0600238A RID: 9098 RVA: 0x0009DA33 File Offset: 0x0009BC33
		// (set) Token: 0x0600238B RID: 9099 RVA: 0x0009DA3C File Offset: 0x0009BC3C
		public bool IsPlayer
		{
			get
			{
				return this.GetFlags(ConversationSentence.DialogLineFlags.PlayerLine);
			}
			internal set
			{
				this.set_flags(value, ConversationSentence.DialogLineFlags.PlayerLine);
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x0600238C RID: 9100 RVA: 0x0009DA46 File Offset: 0x0009BC46
		// (set) Token: 0x0600238D RID: 9101 RVA: 0x0009DA4F File Offset: 0x0009BC4F
		public bool IsRepeatable
		{
			get
			{
				return this.GetFlags(ConversationSentence.DialogLineFlags.RepeatForObjects);
			}
			internal set
			{
				this.set_flags(value, ConversationSentence.DialogLineFlags.RepeatForObjects);
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x0600238E RID: 9102 RVA: 0x0009DA59 File Offset: 0x0009BC59
		// (set) Token: 0x0600238F RID: 9103 RVA: 0x0009DA62 File Offset: 0x0009BC62
		public bool IsSpecial
		{
			get
			{
				return this.GetFlags(ConversationSentence.DialogLineFlags.SpecialLine);
			}
			internal set
			{
				this.set_flags(value, ConversationSentence.DialogLineFlags.SpecialLine);
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x0009DA6C File Offset: 0x0009BC6C
		// (set) Token: 0x06002391 RID: 9105 RVA: 0x0009DA75 File Offset: 0x0009BC75
		public bool IsUsedOnce
		{
			get
			{
				return this.GetFlags(ConversationSentence.DialogLineFlags.UsedOnce);
			}
			internal set
			{
				this.set_flags(value, ConversationSentence.DialogLineFlags.UsedOnce);
			}
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x0009DA7F File Offset: 0x0009BC7F
		private bool GetFlags(ConversationSentence.DialogLineFlags flag)
		{
			return (this._flags & (uint)flag) > 0U;
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x0009DA8C File Offset: 0x0009BC8C
		private void set_flags(bool val, ConversationSentence.DialogLineFlags newFlag)
		{
			if (val)
			{
				this._flags |= (uint)newFlag;
				return;
			}
			this._flags &= (uint)(~(uint)newFlag);
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06002394 RID: 9108 RVA: 0x0009DAAF File Offset: 0x0009BCAF
		// (set) Token: 0x06002395 RID: 9109 RVA: 0x0009DAB7 File Offset: 0x0009BCB7
		public int Priority { get; private set; }

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002396 RID: 9110 RVA: 0x0009DAC0 File Offset: 0x0009BCC0
		// (set) Token: 0x06002397 RID: 9111 RVA: 0x0009DAC8 File Offset: 0x0009BCC8
		public int InputToken { get; private set; }

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002398 RID: 9112 RVA: 0x0009DAD1 File Offset: 0x0009BCD1
		// (set) Token: 0x06002399 RID: 9113 RVA: 0x0009DAD9 File Offset: 0x0009BCD9
		public int OutputToken { get; private set; }

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x0600239A RID: 9114 RVA: 0x0009DAE2 File Offset: 0x0009BCE2
		// (set) Token: 0x0600239B RID: 9115 RVA: 0x0009DAEA File Offset: 0x0009BCEA
		public object RelatedObject { get; private set; }

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x0600239D RID: 9117 RVA: 0x0009DAFC File Offset: 0x0009BCFC
		// (set) Token: 0x0600239C RID: 9116 RVA: 0x0009DAF3 File Offset: 0x0009BCF3
		public bool IsWithVariation { get; private set; }

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x0600239E RID: 9118 RVA: 0x0009DB04 File Offset: 0x0009BD04
		// (set) Token: 0x0600239F RID: 9119 RVA: 0x0009DB0C File Offset: 0x0009BD0C
		public PersuasionOptionArgs PersuationOptionArgs { get; private set; }

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x060023A0 RID: 9120 RVA: 0x0009DB15 File Offset: 0x0009BD15
		public bool HasPersuasion
		{
			get
			{
				return this._onPersuasionOption != null;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060023A1 RID: 9121 RVA: 0x0009DB20 File Offset: 0x0009BD20
		public string SkillName
		{
			get
			{
				if (!this.HasPersuasion)
				{
					return "";
				}
				return this.PersuationOptionArgs.SkillUsed.ToString();
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x060023A2 RID: 9122 RVA: 0x0009DB40 File Offset: 0x0009BD40
		public string TraitName
		{
			get
			{
				if (!this.HasPersuasion)
				{
					return "";
				}
				if (this.PersuationOptionArgs.TraitUsed == null)
				{
					return "";
				}
				return this.PersuationOptionArgs.TraitUsed.ToString();
			}
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x0009DB74 File Offset: 0x0009BD74
		internal ConversationSentence(string idString, TextObject text, string inputToken, string outputToken, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, uint flags = 0U, int priority = 100, int agentIndex = 0, int nextAgentIndex = 0, object relatedObject = null, bool withVariation = false, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, ConversationSentence.OnPersuasionOptionDelegate persuasionOptionDelegate = null)
		{
			this.Index = Campaign.Current.ConversationManager.CreateConversationSentenceIndex();
			this.Id = idString;
			this.Text = text;
			this.InputToken = Campaign.Current.ConversationManager.GetStateIndex(inputToken);
			this.OutputToken = Campaign.Current.ConversationManager.GetStateIndex(outputToken);
			this.OnCondition = conditionDelegate;
			this.OnClickableCondition = clickableConditionDelegate;
			this.OnConsequence = consequenceDelegate;
			this._flags = flags;
			this.Priority = priority;
			this.AgentIndex = agentIndex;
			this.NextAgentIndex = nextAgentIndex;
			this.RelatedObject = relatedObject;
			this.IsWithVariation = withVariation;
			this.IsSpeaker = speakerDelegate;
			this.IsListener = listenerDelegate;
			this._onPersuasionOption = persuasionOptionDelegate;
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x0009DC3E File Offset: 0x0009BE3E
		internal ConversationSentence(int index)
		{
			this.Index = index;
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x0009DC54 File Offset: 0x0009BE54
		public ConversationSentence Variation(params object[] list)
		{
			Game.Current.GameTextManager.AddGameText(this.Id).AddVariation((string)list[0], list.Skip<object>(1).ToArray<object>());
			return this;
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x0009DC85 File Offset: 0x0009BE85
		internal void RunConsequence(Game game)
		{
			if (this.OnConsequence != null)
			{
				this.OnConsequence();
			}
			Campaign.Current.ConversationManager.OnConsequence(this);
			if (this.HasPersuasion)
			{
				ConversationManager.PersuasionCommitProgress(this.PersuationOptionArgs);
			}
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x0009DCC0 File Offset: 0x0009BEC0
		internal bool RunCondition()
		{
			bool flag = true;
			if (this.OnCondition != null)
			{
				flag = this.OnCondition();
			}
			if (flag && this.HasPersuasion)
			{
				this.PersuationOptionArgs = this._onPersuasionOption();
			}
			Campaign.Current.ConversationManager.OnCondition(this);
			return flag;
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x0009DD10 File Offset: 0x0009BF10
		internal bool RunClickableCondition()
		{
			bool flag = true;
			if (this.OnClickableCondition != null)
			{
				flag = this.OnClickableCondition(out this.HintText);
			}
			Campaign.Current.ConversationManager.OnClickableCondition(this);
			return flag;
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x0009DD4C File Offset: 0x0009BF4C
		public void Deserialize(XmlNode node, Type typeOfConversationCallbacks, ConversationManager conversationManager, int defaultPriority)
		{
			if (node.Attributes == null)
			{
				throw new TWXmlLoadException("node.Attributes != null");
			}
			this.Id = node.Attributes["id"].Value;
			XmlNode xmlNode = node.Attributes["on_condition"];
			if (xmlNode != null)
			{
				string innerText = xmlNode.InnerText;
				this._methodOnCondition = typeOfConversationCallbacks.GetMethod(innerText);
				if (this._methodOnCondition == null)
				{
					throw new MBMethodNameNotFoundException(innerText);
				}
				this.OnCondition = Delegate.CreateDelegate(typeof(ConversationSentence.OnConditionDelegate), null, this._methodOnCondition) as ConversationSentence.OnConditionDelegate;
			}
			XmlNode xmlNode2 = node.Attributes["on_clickable_condition"];
			if (xmlNode2 != null)
			{
				string innerText2 = xmlNode2.InnerText;
				this._methodOnClickableCondition = typeOfConversationCallbacks.GetMethod(innerText2);
				if (this._methodOnClickableCondition == null)
				{
					throw new MBMethodNameNotFoundException(innerText2);
				}
				this.OnClickableCondition = Delegate.CreateDelegate(typeof(ConversationSentence.OnClickableConditionDelegate), null, this._methodOnClickableCondition) as ConversationSentence.OnClickableConditionDelegate;
			}
			XmlNode xmlNode3 = node.Attributes["on_consequence"];
			if (xmlNode3 != null)
			{
				string innerText3 = xmlNode3.InnerText;
				this._methodOnConsequence = typeOfConversationCallbacks.GetMethod(innerText3);
				if (this._methodOnConsequence == null)
				{
					throw new MBMethodNameNotFoundException(innerText3);
				}
				this.OnConsequence = Delegate.CreateDelegate(typeof(ConversationSentence.OnConsequenceDelegate), null, this._methodOnConsequence) as ConversationSentence.OnConsequenceDelegate;
			}
			XmlNode xmlNode4 = node.Attributes["is_player"];
			if (xmlNode4 != null)
			{
				string innerText4 = xmlNode4.InnerText;
				this.IsPlayer = Convert.ToBoolean(innerText4);
			}
			XmlNode xmlNode5 = node.Attributes["is_repeatable"];
			if (xmlNode5 != null)
			{
				string innerText5 = xmlNode5.InnerText;
				this.IsRepeatable = Convert.ToBoolean(innerText5);
			}
			XmlNode xmlNode6 = node.Attributes["is_speacial_option"];
			if (xmlNode6 != null)
			{
				string innerText6 = xmlNode6.InnerText;
				this.IsSpecial = Convert.ToBoolean(innerText6);
			}
			XmlNode xmlNode7 = node.Attributes["is_used_once"];
			if (xmlNode7 != null)
			{
				string innerText7 = xmlNode7.InnerText;
				this.IsUsedOnce = Convert.ToBoolean(innerText7);
			}
			XmlNode xmlNode8 = node.Attributes["text"];
			if (xmlNode8 != null)
			{
				this.Text = new TextObject(xmlNode8.InnerText, null);
			}
			XmlNode xmlNode9 = node.Attributes["istate"];
			if (xmlNode9 != null)
			{
				this.InputToken = conversationManager.GetStateIndex(xmlNode9.InnerText);
			}
			XmlNode xmlNode10 = node.Attributes["ostate"];
			if (xmlNode10 != null)
			{
				this.OutputToken = conversationManager.GetStateIndex(xmlNode10.InnerText);
			}
			XmlNode xmlNode11 = node.Attributes["priority"];
			this.Priority = ((xmlNode11 != null) ? int.Parse(xmlNode11.InnerText) : defaultPriority);
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x060023AA RID: 9130 RVA: 0x0009DFFE File Offset: 0x0009C1FE
		public static object CurrentProcessedRepeatObject
		{
			get
			{
				return Campaign.Current.ConversationManager.GetCurrentProcessedRepeatObject();
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060023AB RID: 9131 RVA: 0x0009E00F File Offset: 0x0009C20F
		public static object SelectedRepeatObject
		{
			get
			{
				return Campaign.Current.ConversationManager.GetSelectedRepeatObject();
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x060023AC RID: 9132 RVA: 0x0009E020 File Offset: 0x0009C220
		public static TextObject SelectedRepeatLine
		{
			get
			{
				return Campaign.Current.ConversationManager.GetCurrentDialogLine();
			}
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x0009E031 File Offset: 0x0009C231
		public static void SetObjectsToRepeatOver(IReadOnlyList<object> objectsToRepeatOver, int maxRepeatedDialogsInConversation = 5)
		{
			Campaign.Current.ConversationManager.SetDialogRepeatCount(objectsToRepeatOver, maxRepeatedDialogsInConversation);
		}

		// Token: 0x04000A7F RID: 2687
		public const int DefaultPriority = 100;

		// Token: 0x04000A83 RID: 2691
		public int AgentIndex;

		// Token: 0x04000A84 RID: 2692
		public int NextAgentIndex;

		// Token: 0x04000A85 RID: 2693
		public bool IsClickable = true;

		// Token: 0x04000A86 RID: 2694
		public TextObject HintText;

		// Token: 0x04000A8B RID: 2699
		private MethodInfo _methodOnCondition;

		// Token: 0x04000A8C RID: 2700
		public ConversationSentence.OnConditionDelegate OnCondition;

		// Token: 0x04000A8D RID: 2701
		private MethodInfo _methodOnClickableCondition;

		// Token: 0x04000A8E RID: 2702
		public ConversationSentence.OnClickableConditionDelegate OnClickableCondition;

		// Token: 0x04000A8F RID: 2703
		private MethodInfo _methodOnConsequence;

		// Token: 0x04000A90 RID: 2704
		public ConversationSentence.OnConsequenceDelegate OnConsequence;

		// Token: 0x04000A91 RID: 2705
		public ConversationSentence.OnMultipleConversationConsequenceDelegate IsSpeaker;

		// Token: 0x04000A92 RID: 2706
		public ConversationSentence.OnMultipleConversationConsequenceDelegate IsListener;

		// Token: 0x04000A93 RID: 2707
		private uint _flags;

		// Token: 0x04000A95 RID: 2709
		private ConversationSentence.OnPersuasionOptionDelegate _onPersuasionOption;

		// Token: 0x02000679 RID: 1657
		public enum DialogLineFlags
		{
			// Token: 0x04001AF7 RID: 6903
			PlayerLine = 1,
			// Token: 0x04001AF8 RID: 6904
			RepeatForObjects,
			// Token: 0x04001AF9 RID: 6905
			SpecialLine = 4,
			// Token: 0x04001AFA RID: 6906
			UsedOnce = 8
		}

		// Token: 0x0200067A RID: 1658
		// (Invoke) Token: 0x06005456 RID: 21590
		public delegate bool OnConditionDelegate();

		// Token: 0x0200067B RID: 1659
		// (Invoke) Token: 0x0600545A RID: 21594
		public delegate bool OnClickableConditionDelegate(out TextObject explanation);

		// Token: 0x0200067C RID: 1660
		// (Invoke) Token: 0x0600545E RID: 21598
		public delegate PersuasionOptionArgs OnPersuasionOptionDelegate();

		// Token: 0x0200067D RID: 1661
		// (Invoke) Token: 0x06005462 RID: 21602
		public delegate void OnConsequenceDelegate();

		// Token: 0x0200067E RID: 1662
		// (Invoke) Token: 0x06005466 RID: 21606
		public delegate bool OnMultipleConversationConsequenceDelegate(IAgent agent);
	}
}
