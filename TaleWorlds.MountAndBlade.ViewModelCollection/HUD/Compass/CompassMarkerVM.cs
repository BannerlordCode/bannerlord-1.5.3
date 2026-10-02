using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.Compass
{
	// Token: 0x02000067 RID: 103
	public class CompassMarkerVM : ViewModel
	{
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x0001BC8A File Offset: 0x00019E8A
		// (set) Token: 0x06000801 RID: 2049 RVA: 0x0001BC92 File Offset: 0x00019E92
		public float Angle { get; private set; }

		// Token: 0x06000802 RID: 2050 RVA: 0x0001BC9B File Offset: 0x00019E9B
		public CompassMarkerVM(bool isPrimary, float angle, string text)
		{
			this.IsPrimary = isPrimary;
			this.Angle = angle;
			this.Text = (this.IsPrimary ? text : ("-" + text + "-"));
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001BCD2 File Offset: 0x00019ED2
		public void Refresh(float circleX, float x, float distance)
		{
			this.FullPosition = circleX;
			this.Position = x;
			this.Distance = MathF.Round(distance);
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x0001BCEE File Offset: 0x00019EEE
		// (set) Token: 0x06000805 RID: 2053 RVA: 0x0001BCF6 File Offset: 0x00019EF6
		[DataSourceProperty]
		public bool IsPrimary
		{
			get
			{
				return this._isPrimary;
			}
			set
			{
				if (value != this._isPrimary)
				{
					this._isPrimary = value;
					base.OnPropertyChangedWithValue(value, "IsPrimary");
				}
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x0001BD14 File Offset: 0x00019F14
		// (set) Token: 0x06000807 RID: 2055 RVA: 0x0001BD1C File Offset: 0x00019F1C
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x0001BD3F File Offset: 0x00019F3F
		// (set) Token: 0x06000809 RID: 2057 RVA: 0x0001BD47 File Offset: 0x00019F47
		[DataSourceProperty]
		public int Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (value != this._distance)
				{
					this._distance = value;
					base.OnPropertyChangedWithValue(value, "Distance");
				}
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x0001BD65 File Offset: 0x00019F65
		// (set) Token: 0x0600080B RID: 2059 RVA: 0x0001BD6D File Offset: 0x00019F6D
		[DataSourceProperty]
		public float Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (MathF.Abs(value - this._position) > 1E-45f)
				{
					this._position = value;
					base.OnPropertyChangedWithValue(value, "Position");
				}
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x0001BD96 File Offset: 0x00019F96
		// (set) Token: 0x0600080D RID: 2061 RVA: 0x0001BD9E File Offset: 0x00019F9E
		[DataSourceProperty]
		public float FullPosition
		{
			get
			{
				return this._fullPosition;
			}
			set
			{
				if (MathF.Abs(value - this._fullPosition) > 1E-45f)
				{
					this._fullPosition = value;
					base.OnPropertyChangedWithValue(value, "FullPosition");
				}
			}
		}

		// Token: 0x040003A5 RID: 933
		private bool _isPrimary;

		// Token: 0x040003A6 RID: 934
		private string _text;

		// Token: 0x040003A7 RID: 935
		private int _distance;

		// Token: 0x040003A8 RID: 936
		private float _position;

		// Token: 0x040003A9 RID: 937
		private float _fullPosition;
	}
}
