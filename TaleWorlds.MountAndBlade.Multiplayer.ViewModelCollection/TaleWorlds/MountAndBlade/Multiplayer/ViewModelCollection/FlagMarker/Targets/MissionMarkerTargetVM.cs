using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker.Targets
{
	// Token: 0x0200009F RID: 159
	public abstract class MissionMarkerTargetVM : ViewModel
	{
		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06000F9D RID: 3997
		public abstract Vec3 WorldPosition { get; }

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06000F9E RID: 3998
		protected abstract float HeightOffset { get; }

		// Token: 0x06000F9F RID: 3999 RVA: 0x00030D78 File Offset: 0x0002EF78
		public MissionMarkerTargetVM(MissionMarkerType markerType)
		{
			this.MissionMarkerType = markerType;
			this.MarkerType = (int)markerType;
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00030D90 File Offset: 0x0002EF90
		public virtual void UpdateScreenPosition(Camera missionCamera)
		{
			float num = -100f;
			float num2 = -100f;
			float num3 = 0f;
			Vec3 worldPosition = this.WorldPosition;
			worldPosition.z += this.HeightOffset;
			MBWindowManager.WorldToScreenInsideUsableArea(missionCamera, worldPosition, ref num, ref num2, ref num3);
			if (num3 > 0f)
			{
				this.ScreenPosition = new Vec2(num, num2);
				this.Distance = (int)(this.WorldPosition - missionCamera.Position).Length;
				return;
			}
			this.Distance = -1;
			this.ScreenPosition = new Vec2(-100f, -100f);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00030E28 File Offset: 0x0002F028
		protected void RefreshColor(uint color, uint color2)
		{
			if (color != 0U)
			{
				string text = color.ToString("X");
				char c = text[0];
				char c2 = text[1];
				text = text.Remove(0, 2);
				text = text.Add(c.ToString() + c2.ToString(), false);
				this.Color = "#" + text;
			}
			else
			{
				this.Color = "#FFFFFFFF";
			}
			if (color2 != 0U)
			{
				string text2 = color2.ToString("X");
				char c3 = text2[0];
				char c4 = text2[1];
				text2 = text2.Remove(0, 2);
				text2 = text2.Add(c3.ToString() + c4.ToString(), false);
				this.Color2 = "#" + text2;
				return;
			}
			this.Color2 = "#FFFFFFFF";
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x00030EFA File Offset: 0x0002F0FA
		// (set) Token: 0x06000FA3 RID: 4003 RVA: 0x00030F02 File Offset: 0x0002F102
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value.x != this._screenPosition.x || value.y != this._screenPosition.y)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x00030F3D File Offset: 0x0002F13D
		// (set) Token: 0x06000FA5 RID: 4005 RVA: 0x00030F45 File Offset: 0x0002F145
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x00030F68 File Offset: 0x0002F168
		// (set) Token: 0x06000FA7 RID: 4007 RVA: 0x00030F70 File Offset: 0x0002F170
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

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x00030F8E File Offset: 0x0002F18E
		// (set) Token: 0x06000FA9 RID: 4009 RVA: 0x00030F96 File Offset: 0x0002F196
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06000FAA RID: 4010 RVA: 0x00030FB4 File Offset: 0x0002F1B4
		// (set) Token: 0x06000FAB RID: 4011 RVA: 0x00030FBC File Offset: 0x0002F1BC
		[DataSourceProperty]
		public string Color
		{
			get
			{
				return this._color;
			}
			set
			{
				if (value != this._color)
				{
					this._color = value;
					base.OnPropertyChangedWithValue<string>(value, "Color");
				}
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x00030FDF File Offset: 0x0002F1DF
		// (set) Token: 0x06000FAD RID: 4013 RVA: 0x00030FE7 File Offset: 0x0002F1E7
		[DataSourceProperty]
		public string Color2
		{
			get
			{
				return this._color2;
			}
			set
			{
				if (value != this._color2)
				{
					this._color2 = value;
					base.OnPropertyChangedWithValue<string>(value, "Color2");
				}
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06000FAE RID: 4014 RVA: 0x0003100A File Offset: 0x0002F20A
		// (set) Token: 0x06000FAF RID: 4015 RVA: 0x00031012 File Offset: 0x0002F212
		[DataSourceProperty]
		public int MarkerType
		{
			get
			{
				return this._markerType;
			}
			set
			{
				if (value != this._markerType)
				{
					this._markerType = value;
					base.OnPropertyChangedWithValue(value, "MarkerType");
				}
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x00031030 File Offset: 0x0002F230
		// (set) Token: 0x06000FB1 RID: 4017 RVA: 0x00031038 File Offset: 0x0002F238
		[DataSourceProperty]
		public string VisualState
		{
			get
			{
				return this._visualState;
			}
			set
			{
				if (value != this._visualState)
				{
					this._visualState = value;
					base.OnPropertyChangedWithValue<string>(value, "VisualState");
				}
			}
		}

		// Token: 0x04000747 RID: 1863
		public readonly MissionMarkerType MissionMarkerType;

		// Token: 0x04000748 RID: 1864
		private Vec2 _screenPosition;

		// Token: 0x04000749 RID: 1865
		private int _distance;

		// Token: 0x0400074A RID: 1866
		private string _name;

		// Token: 0x0400074B RID: 1867
		private bool _isEnabled;

		// Token: 0x0400074C RID: 1868
		private string _color;

		// Token: 0x0400074D RID: 1869
		private string _color2;

		// Token: 0x0400074E RID: 1870
		private int _markerType;

		// Token: 0x0400074F RID: 1871
		private string _visualState;
	}
}
