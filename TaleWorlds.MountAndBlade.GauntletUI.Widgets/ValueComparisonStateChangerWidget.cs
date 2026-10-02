using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000046 RID: 70
	public class ValueComparisonStateChangerWidget : BrushWidget
	{
		// Token: 0x060003ED RID: 1005 RVA: 0x0000C7A3 File Offset: 0x0000A9A3
		public ValueComparisonStateChangerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000C7AC File Offset: 0x0000A9AC
		private void UpdateState(float dt)
		{
			bool flag = false;
			switch (this.WatchType)
			{
			case ValueComparisonStateChangerWidget.WatchTypes.Equals:
				flag = this.FirstValueFloat == this.SecondValueFloat;
				break;
			case ValueComparisonStateChangerWidget.WatchTypes.NotEquals:
				flag = this.FirstValueFloat != this.SecondValueFloat;
				break;
			case ValueComparisonStateChangerWidget.WatchTypes.GreaterThan:
				flag = this.FirstValueFloat > this.SecondValueFloat;
				break;
			case ValueComparisonStateChangerWidget.WatchTypes.LessThan:
				flag = this.FirstValueFloat < this.SecondValueFloat;
				break;
			}
			(this.TargetWidget ?? this).SetState(flag ? this.TrueState : this.FalseState);
			this._isScheduledForUpdate = false;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000C847 File Offset: 0x0000AA47
		private void SetDirty()
		{
			if (!this._isScheduledForUpdate)
			{
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.UpdateState), 1);
				this._isScheduledForUpdate = true;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x0000C871 File Offset: 0x0000AA71
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x0000C879 File Offset: 0x0000AA79
		public Widget TargetWidget
		{
			get
			{
				return this._targetWidget;
			}
			set
			{
				if (value != this._targetWidget)
				{
					this._targetWidget = value;
					base.OnPropertyChanged<Widget>(value, "TargetWidget");
					this.SetDirty();
				}
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0000C89D File Offset: 0x0000AA9D
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x0000C8A5 File Offset: 0x0000AAA5
		public ValueComparisonStateChangerWidget.WatchTypes WatchType
		{
			get
			{
				return this._watchType;
			}
			set
			{
				if (value != this._watchType)
				{
					this._watchType = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000C8BD File Offset: 0x0000AABD
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x0000C8C6 File Offset: 0x0000AAC6
		public int FirstValueInt
		{
			get
			{
				return (int)this._firstValueFloat;
			}
			set
			{
				if (value != (int)this._firstValueFloat)
				{
					this._firstValueFloat = (float)value;
					base.OnPropertyChanged(value, "FirstValueInt");
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0000C8EC File Offset: 0x0000AAEC
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x0000C8F5 File Offset: 0x0000AAF5
		public int SecondValueInt
		{
			get
			{
				return (int)this._secondValueFloat;
			}
			set
			{
				if (value != (int)this._secondValueFloat)
				{
					this._secondValueFloat = (float)value;
					base.OnPropertyChanged(value, "SecondValueInt");
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0000C91B File Offset: 0x0000AB1B
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x0000C923 File Offset: 0x0000AB23
		public float FirstValueFloat
		{
			get
			{
				return this._firstValueFloat;
			}
			set
			{
				if (value != this._firstValueFloat)
				{
					this._firstValueFloat = value;
					base.OnPropertyChanged(value, "FirstValueFloat");
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x0000C947 File Offset: 0x0000AB47
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x0000C94F File Offset: 0x0000AB4F
		public float SecondValueFloat
		{
			get
			{
				return this._secondValueFloat;
			}
			set
			{
				if (value != this._secondValueFloat)
				{
					this._secondValueFloat = value;
					base.OnPropertyChanged(value, "SecondValueFloat");
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0000C973 File Offset: 0x0000AB73
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x0000C97B File Offset: 0x0000AB7B
		public string TrueState
		{
			get
			{
				return this._trueState;
			}
			set
			{
				if (value != this._trueState)
				{
					this._trueState = value;
					base.OnPropertyChanged<string>(value, "TrueState");
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0000C9A4 File Offset: 0x0000ABA4
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x0000C9AC File Offset: 0x0000ABAC
		public string FalseState
		{
			get
			{
				return this._falseState;
			}
			set
			{
				if (value != this._falseState)
				{
					this._falseState = value;
					base.OnPropertyChanged<string>(value, "FalseState");
					this.SetDirty();
				}
			}
		}

		// Token: 0x0400019F RID: 415
		private bool _isScheduledForUpdate;

		// Token: 0x040001A0 RID: 416
		private Widget _targetWidget;

		// Token: 0x040001A1 RID: 417
		private ValueComparisonStateChangerWidget.WatchTypes _watchType;

		// Token: 0x040001A2 RID: 418
		private float _firstValueFloat;

		// Token: 0x040001A3 RID: 419
		private float _secondValueFloat;

		// Token: 0x040001A4 RID: 420
		private string _trueState;

		// Token: 0x040001A5 RID: 421
		private string _falseState;

		// Token: 0x020001A6 RID: 422
		public enum WatchTypes
		{
			// Token: 0x040009E4 RID: 2532
			Equals,
			// Token: 0x040009E5 RID: 2533
			NotEquals,
			// Token: 0x040009E6 RID: 2534
			GreaterThan,
			// Token: 0x040009E7 RID: 2535
			LessThan
		}
	}
}
