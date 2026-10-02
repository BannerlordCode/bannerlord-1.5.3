using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Conversation
{
	// Token: 0x0200011D RID: 285
	public class ConversationItemVM : ViewModel
	{
		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x060019B2 RID: 6578 RVA: 0x00061C24 File Offset: 0x0005FE24
		private ConversationSentenceOption _option
		{
			get
			{
				List<ConversationSentenceOption> curOptions = Campaign.Current.ConversationManager.CurOptions;
				if (curOptions == null || curOptions.Count <= 0)
				{
					return default(ConversationSentenceOption);
				}
				return Campaign.Current.ConversationManager.CurOptions[this.Index];
			}
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x00061C78 File Offset: 0x0005FE78
		public ConversationItemVM(Action<int> action, Action onReadyToContinue, Action<ConversationItemVM> setCurrentAnswer, int index)
		{
			this.ActionWihIntIndex = action;
			this.Index = index;
			this._onReadyToContinue = onReadyToContinue;
			this.IsEnabled = this._option.IsClickable;
			this.HasPersuasion = this._option.HasPersuasion;
			this._setCurrentAnswer = setCurrentAnswer;
			this.PersuasionItem = new PersuasionOptionVM(Campaign.Current.ConversationManager, index, new Action(this.OnReadyToContinue));
			this.IsSpecial = this._option.IsSpecial;
			this.RefreshValues();
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x00061D04 File Offset: 0x0005FF04
		private void OnReadyToContinue()
		{
			this._onReadyToContinue.DynamicInvokeWithLog(Array.Empty<object>());
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x00061D17 File Offset: 0x0005FF17
		public ConversationItemVM()
		{
			this.Index = 0;
			this.ItemText = "";
			this.IsEnabled = false;
			this.OptionHint = new HintViewModel();
			this.HasPersuasion = false;
			this._setCurrentAnswer = null;
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x00061D54 File Offset: 0x0005FF54
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject text = this._option.Text;
			string text2 = ((text != null) ? text.ToString() : null) ?? "";
			this.OptionHint = new HintViewModel((this._option.HintText != null) ? this._option.HintText : TextObject.GetEmpty(), null);
			PersuasionOptionVM persuasionItem = this.PersuasionItem;
			if (persuasionItem != null)
			{
				persuasionItem.RefreshValues();
			}
			if (this.PersuasionItem != null)
			{
				string persuasionAdditionalText = this.PersuasionItem.GetPersuasionAdditionalText();
				if (!string.IsNullOrEmpty(persuasionAdditionalText))
				{
					GameTexts.SetVariable("STR1", text2);
					GameTexts.SetVariable("STR2", persuasionAdditionalText);
					text2 = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
				}
			}
			this.ItemText = text2;
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x00061E14 File Offset: 0x00060014
		public void ExecuteAction()
		{
			Action<int> actionWihIntIndex = this.ActionWihIntIndex;
			if (actionWihIntIndex == null)
			{
				return;
			}
			actionWihIntIndex(this.Index);
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x00061E2C File Offset: 0x0006002C
		public void SetCurrentAnswer()
		{
			Action<ConversationItemVM> setCurrentAnswer = this._setCurrentAnswer;
			if (setCurrentAnswer == null)
			{
				return;
			}
			setCurrentAnswer(this);
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x00061E3F File Offset: 0x0006003F
		public void ResetCurrentAnswer()
		{
			this._setCurrentAnswer(null);
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x00061E4D File Offset: 0x0006004D
		internal void OnPersuasionProgress(Tuple<PersuasionOptionArgs, PersuasionOptionResult> result)
		{
			PersuasionOptionVM persuasionItem = this.PersuasionItem;
			if (persuasionItem == null)
			{
				return;
			}
			persuasionItem.OnPersuasionProgress(result);
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x060019BB RID: 6587 RVA: 0x00061E60 File Offset: 0x00060060
		// (set) Token: 0x060019BC RID: 6588 RVA: 0x00061E68 File Offset: 0x00060068
		[DataSourceProperty]
		public PersuasionOptionVM PersuasionItem
		{
			get
			{
				return this._persuasionItem;
			}
			set
			{
				if (this._persuasionItem != value)
				{
					this._persuasionItem = value;
					base.OnPropertyChangedWithValue<PersuasionOptionVM>(value, "PersuasionItem");
				}
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x060019BD RID: 6589 RVA: 0x00061E86 File Offset: 0x00060086
		// (set) Token: 0x060019BE RID: 6590 RVA: 0x00061E8E File Offset: 0x0006008E
		[DataSourceProperty]
		public bool HasPersuasion
		{
			get
			{
				return this._hasPersuasion;
			}
			set
			{
				if (this._hasPersuasion != value)
				{
					this._hasPersuasion = value;
					base.OnPropertyChangedWithValue(value, "HasPersuasion");
				}
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x060019BF RID: 6591 RVA: 0x00061EAC File Offset: 0x000600AC
		// (set) Token: 0x060019C0 RID: 6592 RVA: 0x00061EB4 File Offset: 0x000600B4
		[DataSourceProperty]
		public int IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (this._iconType != value)
				{
					this._iconType = value;
					base.OnPropertyChangedWithValue(value, "IconType");
				}
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x060019C1 RID: 6593 RVA: 0x00061ED2 File Offset: 0x000600D2
		// (set) Token: 0x060019C2 RID: 6594 RVA: 0x00061EDA File Offset: 0x000600DA
		[DataSourceProperty]
		public HintViewModel OptionHint
		{
			get
			{
				return this._optionHint;
			}
			set
			{
				if (this._optionHint != value)
				{
					this._optionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "OptionHint");
				}
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x060019C3 RID: 6595 RVA: 0x00061EF8 File Offset: 0x000600F8
		// (set) Token: 0x060019C4 RID: 6596 RVA: 0x00061F00 File Offset: 0x00060100
		[DataSourceProperty]
		public string ItemText
		{
			get
			{
				return this._itemText;
			}
			set
			{
				if (this._itemText != value)
				{
					this._itemText = value;
					base.OnPropertyChangedWithValue<string>(value, "ItemText");
				}
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060019C5 RID: 6597 RVA: 0x00061F23 File Offset: 0x00060123
		// (set) Token: 0x060019C6 RID: 6598 RVA: 0x00061F2B File Offset: 0x0006012B
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (this._isEnabled != value)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060019C7 RID: 6599 RVA: 0x00061F49 File Offset: 0x00060149
		// (set) Token: 0x060019C8 RID: 6600 RVA: 0x00061F51 File Offset: 0x00060151
		[DataSourceProperty]
		public bool IsSpecial
		{
			get
			{
				return this._isSpecial;
			}
			set
			{
				if (this._isSpecial != value)
				{
					this._isSpecial = value;
					base.OnPropertyChangedWithValue(value, "IsSpecial");
				}
			}
		}

		// Token: 0x04000BC2 RID: 3010
		public Action<int> ActionWihIntIndex;

		// Token: 0x04000BC3 RID: 3011
		public Action<ConversationItemVM> _setCurrentAnswer;

		// Token: 0x04000BC4 RID: 3012
		public int Index;

		// Token: 0x04000BC5 RID: 3013
		private Action _onReadyToContinue;

		// Token: 0x04000BC6 RID: 3014
		private bool _hasPersuasion;

		// Token: 0x04000BC7 RID: 3015
		private bool _isSpecial;

		// Token: 0x04000BC8 RID: 3016
		private string _itemText;

		// Token: 0x04000BC9 RID: 3017
		private int _iconType;

		// Token: 0x04000BCA RID: 3018
		private bool _isEnabled;

		// Token: 0x04000BCB RID: 3019
		private PersuasionOptionVM _persuasionItem;

		// Token: 0x04000BCC RID: 3020
		private HintViewModel _optionHint;
	}
}
