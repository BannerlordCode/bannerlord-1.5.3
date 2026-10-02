using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000019 RID: 25
	public class ValueBasedVisibilityWidget : Widget
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00007545 File Offset: 0x00005745
		// (set) Token: 0x0600014C RID: 332 RVA: 0x0000754D File Offset: 0x0000574D
		public ValueBasedVisibilityWidget.WatchTypes WatchType
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
					this.UpdateIsVisible();
				}
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007565 File Offset: 0x00005765
		public ValueBasedVisibilityWidget(UIContext context)
			: base(context)
		{
			this.UpdateIsVisible();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007580 File Offset: 0x00005780
		private void UpdateIsVisible()
		{
			switch (this.WatchType)
			{
			case ValueBasedVisibilityWidget.WatchTypes.Equal:
				base.IsVisible = this.IndexToWatchFloat == this.IndexToBeVisibleFloat;
				return;
			case ValueBasedVisibilityWidget.WatchTypes.BiggerThan:
				base.IsVisible = this.IndexToWatchFloat > this.IndexToBeVisibleFloat;
				return;
			case ValueBasedVisibilityWidget.WatchTypes.BiggerThanEqual:
				base.IsVisible = this.IndexToWatchFloat >= this.IndexToBeVisibleFloat;
				return;
			case ValueBasedVisibilityWidget.WatchTypes.LessThan:
				base.IsVisible = this.IndexToWatchFloat < this.IndexToBeVisibleFloat;
				return;
			case ValueBasedVisibilityWidget.WatchTypes.LessThanEqual:
				base.IsVisible = this.IndexToWatchFloat <= this.IndexToBeVisibleFloat;
				return;
			case ValueBasedVisibilityWidget.WatchTypes.NotEqual:
				base.IsVisible = this.IndexToWatchFloat != this.IndexToBeVisibleFloat;
				return;
			default:
				return;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00007639 File Offset: 0x00005839
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00007642 File Offset: 0x00005842
		[Editor(false)]
		public int IndexToWatch
		{
			get
			{
				return (int)this.IndexToWatchFloat;
			}
			set
			{
				this.IndexToWatchFloat = (float)value;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000151 RID: 337 RVA: 0x0000764C File Offset: 0x0000584C
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00007654 File Offset: 0x00005854
		[Editor(false)]
		public float IndexToWatchFloat
		{
			get
			{
				return this._indexToWatchFloat;
			}
			set
			{
				if (this._indexToWatchFloat != value)
				{
					this._indexToWatchFloat = value;
					base.OnPropertyChanged(value, "IndexToWatchFloat");
					this.UpdateIsVisible();
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00007678 File Offset: 0x00005878
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00007681 File Offset: 0x00005881
		[Editor(false)]
		public int IndexToBeVisible
		{
			get
			{
				return (int)this.IndexToBeVisibleFloat;
			}
			set
			{
				this.IndexToBeVisibleFloat = (float)value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000768B File Offset: 0x0000588B
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00007693 File Offset: 0x00005893
		[Editor(false)]
		public float IndexToBeVisibleFloat
		{
			get
			{
				return this._indexToBeVisibleFloat;
			}
			set
			{
				if (this._indexToBeVisibleFloat != value)
				{
					this._indexToBeVisibleFloat = value;
					base.OnPropertyChanged(value, "IndexToBeVisibleFloat");
					this.UpdateIsVisible();
				}
			}
		}

		// Token: 0x040000A2 RID: 162
		private ValueBasedVisibilityWidget.WatchTypes _watchType;

		// Token: 0x040000A3 RID: 163
		private float _indexToBeVisibleFloat;

		// Token: 0x040000A4 RID: 164
		private float _indexToWatchFloat = -1f;

		// Token: 0x02000023 RID: 35
		public enum WatchTypes
		{
			// Token: 0x040000DA RID: 218
			Equal,
			// Token: 0x040000DB RID: 219
			BiggerThan,
			// Token: 0x040000DC RID: 220
			BiggerThanEqual,
			// Token: 0x040000DD RID: 221
			LessThan,
			// Token: 0x040000DE RID: 222
			LessThanEqual,
			// Token: 0x040000DF RID: 223
			NotEqual
		}
	}
}
