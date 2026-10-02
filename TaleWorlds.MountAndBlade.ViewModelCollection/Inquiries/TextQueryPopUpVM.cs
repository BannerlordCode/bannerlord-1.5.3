using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Inquiries
{
	// Token: 0x02000048 RID: 72
	public class TextQueryPopUpVM : PopUpBaseVM
	{
		// Token: 0x06000608 RID: 1544 RVA: 0x00016526 File Offset: 0x00014726
		public TextQueryPopUpVM(Action closeQuery)
			: base(closeQuery)
		{
			this.DoneButtonDisabledReasonHint = new HintViewModel();
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001653C File Offset: 0x0001473C
		public void SetData(TextInquiryData data)
		{
			this._data = data;
			base.TitleText = this._data.TitleText;
			base.PopUpLabel = this._data.Text;
			base.ButtonOkLabel = this._data.AffirmativeText;
			base.ButtonCancelLabel = this._data.NegativeText;
			base.IsButtonOkShown = this._data.IsAffirmativeOptionShown;
			base.IsButtonCancelShown = this._data.IsNegativeOptionShown;
			this.IsInputObfuscated = this._data.IsInputObfuscated;
			this.InputText = this._data.DefaultInputText;
			Func<string, Tuple<bool, string>> textCondition = this._data.TextCondition;
			base.IsButtonOkEnabled = textCondition == null || textCondition(this.InputText).Item1;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00016600 File Offset: 0x00014800
		public override void ExecuteAffirmativeAction()
		{
			Action<string> affirmativeAction = this._data.AffirmativeAction;
			if (affirmativeAction != null)
			{
				affirmativeAction(this.InputText);
			}
			base.CloseQuery();
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00016624 File Offset: 0x00014824
		public override void ExecuteNegativeAction()
		{
			Action negativeAction = this._data.NegativeAction;
			if (negativeAction != null)
			{
				negativeAction();
			}
			base.CloseQuery();
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00016642 File Offset: 0x00014842
		public override void OnClearData()
		{
			base.OnClearData();
			this._data = null;
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x000166E8 File Offset: 0x000148E8
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x00016654 File Offset: 0x00014854
		[DataSourceProperty]
		public string InputText
		{
			get
			{
				return this._inputText;
			}
			set
			{
				if (value != this._inputText)
				{
					this._inputText = value;
					base.OnPropertyChangedWithValue<string>(value, "InputText");
					Func<string, Tuple<bool, string>> textCondition = this._data.TextCondition;
					Tuple<bool, string> tuple = ((textCondition != null) ? textCondition(value) : null);
					base.IsButtonOkEnabled = tuple == null || tuple.Item1;
					this.DoneButtonDisabledReasonHint.HintText = (string.IsNullOrEmpty((tuple != null) ? tuple.Item2 : null) ? TextObject.GetEmpty() : new TextObject("{=!}" + tuple.Item2, null));
				}
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x000166F0 File Offset: 0x000148F0
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x000166F8 File Offset: 0x000148F8
		public bool IsInputObfuscated
		{
			get
			{
				return this._isInputObfuscated;
			}
			set
			{
				if (value != this._isInputObfuscated)
				{
					this._isInputObfuscated = value;
					base.OnPropertyChangedWithValue(value, "IsInputObfuscated");
				}
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x00016716 File Offset: 0x00014916
		// (set) Token: 0x06000612 RID: 1554 RVA: 0x0001671E File Offset: 0x0001491E
		[DataSourceProperty]
		public HintViewModel DoneButtonDisabledReasonHint
		{
			get
			{
				return this._doneButtonDisabledReasonHint;
			}
			set
			{
				if (value != this._doneButtonDisabledReasonHint)
				{
					this._doneButtonDisabledReasonHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DoneButtonDisabledReasonHint");
				}
			}
		}

		// Token: 0x040002B6 RID: 694
		private TextInquiryData _data;

		// Token: 0x040002B7 RID: 695
		[DataSourceProperty]
		private string _inputText;

		// Token: 0x040002B8 RID: 696
		private bool _isInputObfuscated;

		// Token: 0x040002B9 RID: 697
		private HintViewModel _doneButtonDisabledReasonHint;
	}
}
