using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Conversation
{
	// Token: 0x02000175 RID: 373
	public class ConversationScreenButtonWidget : ButtonWidget
	{
		// Token: 0x0600139A RID: 5018 RVA: 0x00035752 File Offset: 0x00033952
		public ConversationScreenButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00035768 File Offset: 0x00033968
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.AnswerList != null && this.ContinueButton != null)
			{
				this.ContinueButton.IsVisible = this.AnswerList.ChildCount == 0;
				this.ContinueButton.IsEnabled = this.AnswerList.ChildCount == 0;
			}
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x000357C0 File Offset: 0x000339C0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			foreach (ConversationOptionListPanel conversationOptionListPanel in this._newlyAddedItems)
			{
				conversationOptionListPanel.OptionButtonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnOptionSelection));
			}
			this._newlyAddedItems.Clear();
			ListPanel answerList = this.AnswerList;
			if (answerList != null && answerList.ChildCount > 0 && this.AnswerList.GetChild(this.AnswerList.ChildCount - 1) != null)
			{
				this.AnswerList.GetChild(this.AnswerList.ChildCount - 1).MarginBottom = 5f;
			}
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x0003588C File Offset: 0x00033A8C
		private void OnOptionSelection(Widget obj)
		{
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00035890 File Offset: 0x00033A90
		private void OnOptionRemoved(Widget obj, Widget child)
		{
			ConversationOptionListPanel conversationOptionListPanel;
			if ((conversationOptionListPanel = obj as ConversationOptionListPanel) != null)
			{
				conversationOptionListPanel.OptionButtonWidget.ClickEventHandlers.Remove(new Action<Widget>(this.OnOptionSelection));
			}
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x000358C4 File Offset: 0x00033AC4
		private void OnNewOptionAdded(Widget parent, Widget child)
		{
			this._newlyAddedItems.Add(child as ConversationOptionListPanel);
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x060013A0 RID: 5024 RVA: 0x000358D7 File Offset: 0x00033AD7
		// (set) Token: 0x060013A1 RID: 5025 RVA: 0x000358E0 File Offset: 0x00033AE0
		[Editor(false)]
		public ListPanel AnswerList
		{
			get
			{
				return this._answerList;
			}
			set
			{
				if (value != this._answerList)
				{
					if (value != null)
					{
						value.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnNewOptionAdded));
						value.ItemRemoveEventHandlers.Add(new Action<Widget, Widget>(this.OnOptionRemoved));
					}
					if (this._answerList != null)
					{
						value.ItemAddEventHandlers.Remove(new Action<Widget, Widget>(this.OnNewOptionAdded));
						value.ItemRemoveEventHandlers.Remove(new Action<Widget, Widget>(this.OnOptionRemoved));
					}
					this._answerList = value;
					base.OnPropertyChanged<ListPanel>(value, "AnswerList");
				}
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x060013A2 RID: 5026 RVA: 0x00035972 File Offset: 0x00033B72
		// (set) Token: 0x060013A3 RID: 5027 RVA: 0x0003597A File Offset: 0x00033B7A
		[Editor(false)]
		public ButtonWidget ContinueButton
		{
			get
			{
				return this._continueButton;
			}
			set
			{
				if (value != this._continueButton)
				{
					this._continueButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ContinueButton");
				}
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060013A4 RID: 5028 RVA: 0x00035998 File Offset: 0x00033B98
		// (set) Token: 0x060013A5 RID: 5029 RVA: 0x000359A0 File Offset: 0x00033BA0
		[Editor(false)]
		public bool IsPersuasionActive
		{
			get
			{
				return this._isPersuasionActive;
			}
			set
			{
				if (value != this._isPersuasionActive)
				{
					this._isPersuasionActive = value;
					base.OnPropertyChanged(value, "IsPersuasionActive");
				}
			}
		}

		// Token: 0x040008EC RID: 2284
		private List<ConversationOptionListPanel> _newlyAddedItems = new List<ConversationOptionListPanel>();

		// Token: 0x040008ED RID: 2285
		private ListPanel _answerList;

		// Token: 0x040008EE RID: 2286
		private ButtonWidget _continueButton;

		// Token: 0x040008EF RID: 2287
		private bool _isPersuasionActive;
	}
}
