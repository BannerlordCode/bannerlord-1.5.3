using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Inquiries
{
	// Token: 0x02000047 RID: 71
	public class SingleQueryPopUpVM : PopUpBaseVM
	{
		// Token: 0x060005FB RID: 1531 RVA: 0x0001619E File Offset: 0x0001439E
		public SingleQueryPopUpVM(Action closeQuery)
			: base(closeQuery)
		{
			base.ButtonOkHint = new HintViewModel();
			base.ButtonCancelHint = new HintViewModel();
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x000161C0 File Offset: 0x000143C0
		public override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._data != null)
			{
				this.UpdateButtonEnabledStates();
				if (this._data.ExpireTime > 0f)
				{
					if (this._queryTimer > this._data.ExpireTime)
					{
						Action timeoutAction = this._data.TimeoutAction;
						if (timeoutAction != null)
						{
							timeoutAction();
						}
						base.CloseQuery();
						return;
					}
					this._queryTimer += dt;
					this.RemainingQueryTime = this._data.ExpireTime - this._queryTimer;
				}
			}
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001624A File Offset: 0x0001444A
		public override void ExecuteAffirmativeAction()
		{
			Action affirmativeAction = this._data.AffirmativeAction;
			if (affirmativeAction != null)
			{
				affirmativeAction();
			}
			base.CloseQuery();
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00016268 File Offset: 0x00014468
		public override void ExecuteNegativeAction()
		{
			Action negativeAction = this._data.NegativeAction;
			if (negativeAction != null)
			{
				negativeAction();
			}
			base.CloseQuery();
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00016286 File Offset: 0x00014486
		public override void OnClearData()
		{
			base.OnClearData();
			this._data = null;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00016298 File Offset: 0x00014498
		private void UpdateButtonEnabledStates()
		{
			if (this._data.GetIsAffirmativeOptionEnabled != null)
			{
				ValueTuple<bool, string> valueTuple = this._data.GetIsAffirmativeOptionEnabled();
				base.IsButtonOkEnabled = valueTuple.Item1;
				if (!string.Equals(this._lastButtonOkHint, valueTuple.Item2, StringComparison.OrdinalIgnoreCase))
				{
					base.ButtonOkHint.HintText = (string.IsNullOrEmpty(valueTuple.Item2) ? TextObject.GetEmpty() : new TextObject("{=!}" + valueTuple.Item2, null));
					this._lastButtonOkHint = valueTuple.Item2;
				}
			}
			else
			{
				base.IsButtonOkEnabled = true;
				base.ButtonOkHint.HintText = TextObject.GetEmpty();
				this._lastButtonOkHint = string.Empty;
			}
			if (this._data.GetIsNegativeOptionEnabled != null)
			{
				ValueTuple<bool, string> valueTuple2 = this._data.GetIsNegativeOptionEnabled();
				base.IsButtonCancelEnabled = valueTuple2.Item1;
				if (!string.Equals(this._lastButtonCancelHint, valueTuple2.Item2, StringComparison.OrdinalIgnoreCase))
				{
					base.ButtonCancelHint.HintText = (string.IsNullOrEmpty(valueTuple2.Item2) ? TextObject.GetEmpty() : new TextObject("{=!}" + valueTuple2.Item2, null));
					this._lastButtonCancelHint = valueTuple2.Item2;
					return;
				}
			}
			else
			{
				base.IsButtonCancelEnabled = true;
				base.ButtonCancelHint.HintText = TextObject.GetEmpty();
				this._lastButtonCancelHint = string.Empty;
			}
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x000163EC File Offset: 0x000145EC
		public void SetData(InquiryData data)
		{
			this._data = data;
			base.TitleText = this._data.TitleText;
			base.PopUpLabel = this._data.Text;
			base.ButtonOkLabel = this._data.AffirmativeText;
			base.ButtonCancelLabel = this._data.NegativeText;
			base.IsButtonOkShown = this._data.IsAffirmativeOptionShown;
			base.IsButtonCancelShown = this._data.IsNegativeOptionShown;
			this.IsTimerShown = this._data.ExpireTime > 0f;
			base.IsButtonOkEnabled = true;
			base.IsButtonCancelEnabled = true;
			this.UpdateButtonEnabledStates();
			this._queryTimer = 0f;
			this.TotalQueryTime = (float)MathF.Round(this._data.ExpireTime);
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x000164B4 File Offset: 0x000146B4
		// (set) Token: 0x06000603 RID: 1539 RVA: 0x000164BC File Offset: 0x000146BC
		[DataSourceProperty]
		public float RemainingQueryTime
		{
			get
			{
				return this._remainingQueryTime;
			}
			set
			{
				if (value != this._remainingQueryTime)
				{
					this._remainingQueryTime = value;
					base.OnPropertyChangedWithValue(value, "RemainingQueryTime");
				}
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x000164DA File Offset: 0x000146DA
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x000164E2 File Offset: 0x000146E2
		[DataSourceProperty]
		public float TotalQueryTime
		{
			get
			{
				return this._totalQueryTime;
			}
			set
			{
				if (value != this._totalQueryTime)
				{
					this._totalQueryTime = value;
					base.OnPropertyChangedWithValue(value, "TotalQueryTime");
				}
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00016500 File Offset: 0x00014700
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x00016508 File Offset: 0x00014708
		[DataSourceProperty]
		public bool IsTimerShown
		{
			get
			{
				return this._isTimerShown;
			}
			set
			{
				if (value != this._isTimerShown)
				{
					this._isTimerShown = value;
					base.OnPropertyChangedWithValue(value, "IsTimerShown");
				}
			}
		}

		// Token: 0x040002AF RID: 687
		private InquiryData _data;

		// Token: 0x040002B0 RID: 688
		private float _queryTimer;

		// Token: 0x040002B1 RID: 689
		private string _lastButtonOkHint;

		// Token: 0x040002B2 RID: 690
		private string _lastButtonCancelHint;

		// Token: 0x040002B3 RID: 691
		private float _remainingQueryTime;

		// Token: 0x040002B4 RID: 692
		private float _totalQueryTime;

		// Token: 0x040002B5 RID: 693
		private bool _isTimerShown;
	}
}
