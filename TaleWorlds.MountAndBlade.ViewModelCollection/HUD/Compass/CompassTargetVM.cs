using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.Compass
{
	// Token: 0x02000068 RID: 104
	public class CompassTargetVM : ViewModel
	{
		// Token: 0x0600080E RID: 2062 RVA: 0x0001BDC8 File Offset: 0x00019FC8
		public CompassTargetVM(TargetIconType iconType, uint color, uint color2, Banner banner, bool isAttacker, bool isAlly)
		{
			this.IconType = iconType.ToString();
			this.LetterCode = this.GetLetterCode(iconType);
			this.RefreshColor(color, color2);
			this.IsFlag = iconType >= TargetIconType.Flag_A && iconType <= TargetIconType.Flag_I;
			this.IsAttacker = isAttacker;
			this.IsEnemy = !isAlly;
			if (banner == null)
			{
				this.Banner = new BannerImageIdentifierVM(null, false);
				return;
			}
			this.Banner = new BannerImageIdentifierVM(banner, false);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0001BE4C File Offset: 0x0001A04C
		private string GetLetterCode(TargetIconType iconType)
		{
			switch (iconType)
			{
			case TargetIconType.Flag_A:
				return "A";
			case TargetIconType.Flag_B:
				return "B";
			case TargetIconType.Flag_C:
				return "C";
			case TargetIconType.Flag_D:
				return "D";
			case TargetIconType.Flag_E:
				return "E";
			case TargetIconType.Flag_F:
				return "F";
			case TargetIconType.Flag_G:
				return "G";
			case TargetIconType.Flag_H:
				return "H";
			case TargetIconType.Flag_I:
				return "I";
			default:
				return "";
			}
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0001BEC4 File Offset: 0x0001A0C4
		public void RefreshColor(uint color, uint color2)
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

		// Token: 0x06000811 RID: 2065 RVA: 0x0001BF96 File Offset: 0x0001A196
		public virtual void Refresh(float circleX, float x, float distance)
		{
			this.FullPosition = circleX;
			this.Position = x;
			this.Distance = MathF.Round(distance);
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x0001BFB2 File Offset: 0x0001A1B2
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x0001BFBC File Offset: 0x0001A1BC
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner
		{
			get
			{
				return this._banner;
			}
			set
			{
				if (value != this._banner && (value == null || this._banner == null || this._banner.Id != value.Id))
				{
					this._banner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner");
				}
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x0001C008 File Offset: 0x0001A208
		// (set) Token: 0x06000815 RID: 2069 RVA: 0x0001C010 File Offset: 0x0001A210
		[DataSourceProperty]
		public bool IsFlag
		{
			get
			{
				return this._isFlag;
			}
			set
			{
				if (value != this._isFlag)
				{
					this._isFlag = value;
					base.OnPropertyChangedWithValue(value, "IsFlag");
				}
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x0001C02E File Offset: 0x0001A22E
		// (set) Token: 0x06000817 RID: 2071 RVA: 0x0001C036 File Offset: 0x0001A236
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

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x0001C054 File Offset: 0x0001A254
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x0001C05C File Offset: 0x0001A25C
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

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x0001C07F File Offset: 0x0001A27F
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x0001C087 File Offset: 0x0001A287
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

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x0001C0AA File Offset: 0x0001A2AA
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x0001C0B2 File Offset: 0x0001A2B2
		[DataSourceProperty]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChangedWithValue<string>(value, "IconType");
					this.IconSpriteType = value;
				}
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x0001C0DC File Offset: 0x0001A2DC
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x0001C0E4 File Offset: 0x0001A2E4
		[DataSourceProperty]
		public string IconSpriteType
		{
			get
			{
				return this._iconSpriteType;
			}
			set
			{
				if (value != this._iconSpriteType)
				{
					this._iconSpriteType = value;
					base.OnPropertyChangedWithValue<string>(value, "IconSpriteType");
				}
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x0001C107 File Offset: 0x0001A307
		// (set) Token: 0x06000821 RID: 2081 RVA: 0x0001C10F File Offset: 0x0001A30F
		[DataSourceProperty]
		public string LetterCode
		{
			get
			{
				return this._letterCode;
			}
			set
			{
				if (value != this._letterCode)
				{
					this._letterCode = value;
					base.OnPropertyChangedWithValue<string>(value, "LetterCode");
				}
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x0001C132 File Offset: 0x0001A332
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x0001C13A File Offset: 0x0001A33A
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

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x0001C163 File Offset: 0x0001A363
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x0001C16B File Offset: 0x0001A36B
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

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x0001C194 File Offset: 0x0001A394
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x0001C19C File Offset: 0x0001A39C
		[DataSourceProperty]
		public bool IsAttacker
		{
			get
			{
				return this._isAttacker;
			}
			set
			{
				if (value != this._isAttacker)
				{
					this._isAttacker = value;
					base.OnPropertyChangedWithValue(value, "IsAttacker");
				}
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x0001C1BA File Offset: 0x0001A3BA
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x0001C1C2 File Offset: 0x0001A3C2
		[DataSourceProperty]
		public bool IsEnemy
		{
			get
			{
				return this._isEnemy;
			}
			set
			{
				if (value != this._isEnemy)
				{
					this._isEnemy = value;
					base.OnPropertyChangedWithValue(value, "IsEnemy");
				}
			}
		}

		// Token: 0x040003AA RID: 938
		private int _distance;

		// Token: 0x040003AB RID: 939
		private string _color;

		// Token: 0x040003AC RID: 940
		private string _color2;

		// Token: 0x040003AD RID: 941
		private BannerImageIdentifierVM _banner;

		// Token: 0x040003AE RID: 942
		private string _iconType;

		// Token: 0x040003AF RID: 943
		private string _iconSpriteType;

		// Token: 0x040003B0 RID: 944
		private string _letterCode;

		// Token: 0x040003B1 RID: 945
		private float _position;

		// Token: 0x040003B2 RID: 946
		private float _fullPosition;

		// Token: 0x040003B3 RID: 947
		private bool _isAttacker;

		// Token: 0x040003B4 RID: 948
		private bool _isEnemy;

		// Token: 0x040003B5 RID: 949
		private bool _isFlag;
	}
}
