using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200000B RID: 11
	public class Brush
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000030F4 File Offset: 0x000012F4
		// (set) Token: 0x06000070 RID: 112 RVA: 0x000030FC File Offset: 0x000012FC
		public Brush ClonedFrom { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00003105 File Offset: 0x00001305
		// (set) Token: 0x06000072 RID: 114 RVA: 0x0000310D File Offset: 0x0000130D
		public Brush OverriddenBrush { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003116 File Offset: 0x00001316
		// (set) Token: 0x06000074 RID: 116 RVA: 0x0000311E File Offset: 0x0000131E
		[Editor(false)]
		public string Name { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003127 File Offset: 0x00001327
		// (set) Token: 0x06000076 RID: 118 RVA: 0x0000312F File Offset: 0x0000132F
		[Editor(false)]
		public float TransitionDuration { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003138 File Offset: 0x00001338
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00003140 File Offset: 0x00001340
		public Style DefaultStyle { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003149 File Offset: 0x00001349
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00003156 File Offset: 0x00001356
		public Font Font
		{
			get
			{
				return this.DefaultStyle.Font;
			}
			set
			{
				this.DefaultStyle.Font = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003164 File Offset: 0x00001364
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00003171 File Offset: 0x00001371
		public FontStyle FontStyle
		{
			get
			{
				return this.DefaultStyle.FontStyle;
			}
			set
			{
				this.DefaultStyle.FontStyle = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007D RID: 125 RVA: 0x0000317F File Offset: 0x0000137F
		// (set) Token: 0x0600007E RID: 126 RVA: 0x0000318C File Offset: 0x0000138C
		public int FontSize
		{
			get
			{
				return this.DefaultStyle.FontSize;
			}
			set
			{
				this.DefaultStyle.FontSize = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000319A File Offset: 0x0000139A
		// (set) Token: 0x06000080 RID: 128 RVA: 0x000031A2 File Offset: 0x000013A2
		[Editor(false)]
		public TextHorizontalAlignment TextHorizontalAlignment { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000081 RID: 129 RVA: 0x000031AB File Offset: 0x000013AB
		// (set) Token: 0x06000082 RID: 130 RVA: 0x000031B3 File Offset: 0x000013B3
		[Editor(false)]
		public TextVerticalAlignment TextVerticalAlignment { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000083 RID: 131 RVA: 0x000031BC File Offset: 0x000013BC
		// (set) Token: 0x06000084 RID: 132 RVA: 0x000031C4 File Offset: 0x000013C4
		[Editor(false)]
		public float GlobalColorFactor { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000031CD File Offset: 0x000013CD
		// (set) Token: 0x06000086 RID: 134 RVA: 0x000031D5 File Offset: 0x000013D5
		[Editor(false)]
		public float GlobalAlphaFactor { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000087 RID: 135 RVA: 0x000031DE File Offset: 0x000013DE
		// (set) Token: 0x06000088 RID: 136 RVA: 0x000031E6 File Offset: 0x000013E6
		[Editor(false)]
		public Color GlobalColor { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000031EF File Offset: 0x000013EF
		// (set) Token: 0x0600008A RID: 138 RVA: 0x000031F7 File Offset: 0x000013F7
		public SoundProperties SoundProperties { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003200 File Offset: 0x00001400
		// (set) Token: 0x0600008C RID: 140 RVA: 0x0000320D File Offset: 0x0000140D
		public Sprite Sprite
		{
			get
			{
				return this.DefaultStyleLayer.Sprite;
			}
			set
			{
				this.DefaultStyleLayer.Sprite = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600008D RID: 141 RVA: 0x0000321B File Offset: 0x0000141B
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00003228 File Offset: 0x00001428
		[Editor(false)]
		public bool VerticalFlip
		{
			get
			{
				return this.DefaultStyleLayer.VerticalFlip;
			}
			set
			{
				this.DefaultStyleLayer.VerticalFlip = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003236 File Offset: 0x00001436
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00003243 File Offset: 0x00001443
		[Editor(false)]
		public bool HorizontalFlip
		{
			get
			{
				return this.DefaultStyleLayer.HorizontalFlip;
			}
			set
			{
				this.DefaultStyleLayer.HorizontalFlip = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003251 File Offset: 0x00001451
		// (set) Token: 0x06000092 RID: 146 RVA: 0x0000325E File Offset: 0x0000145E
		public Color Color
		{
			get
			{
				return this.DefaultStyleLayer.Color;
			}
			set
			{
				this.DefaultStyleLayer.Color = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000093 RID: 147 RVA: 0x0000326C File Offset: 0x0000146C
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00003279 File Offset: 0x00001479
		public float ColorFactor
		{
			get
			{
				return this.DefaultStyleLayer.ColorFactor;
			}
			set
			{
				this.DefaultStyleLayer.ColorFactor = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003287 File Offset: 0x00001487
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00003294 File Offset: 0x00001494
		public float AlphaFactor
		{
			get
			{
				return this.DefaultStyleLayer.AlphaFactor;
			}
			set
			{
				this.DefaultStyleLayer.AlphaFactor = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000032A2 File Offset: 0x000014A2
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000032AF File Offset: 0x000014AF
		public float HueFactor
		{
			get
			{
				return this.DefaultStyleLayer.HueFactor;
			}
			set
			{
				this.DefaultStyleLayer.HueFactor = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000032BD File Offset: 0x000014BD
		// (set) Token: 0x0600009A RID: 154 RVA: 0x000032CA File Offset: 0x000014CA
		public float SaturationFactor
		{
			get
			{
				return this.DefaultStyleLayer.SaturationFactor;
			}
			set
			{
				this.DefaultStyleLayer.SaturationFactor = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000032D8 File Offset: 0x000014D8
		// (set) Token: 0x0600009C RID: 156 RVA: 0x000032E5 File Offset: 0x000014E5
		public float ValueFactor
		{
			get
			{
				return this.DefaultStyleLayer.ValueFactor;
			}
			set
			{
				this.DefaultStyleLayer.ValueFactor = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600009D RID: 157 RVA: 0x000032F3 File Offset: 0x000014F3
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00003300 File Offset: 0x00001500
		public Color FontColor
		{
			get
			{
				return this.DefaultStyle.FontColor;
			}
			set
			{
				this.DefaultStyle.FontColor = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600009F RID: 159 RVA: 0x0000330E File Offset: 0x0000150E
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x0000331B File Offset: 0x0000151B
		public float TextColorFactor
		{
			get
			{
				return this.DefaultStyle.TextColorFactor;
			}
			set
			{
				this.DefaultStyle.TextColorFactor = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00003329 File Offset: 0x00001529
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003336 File Offset: 0x00001536
		public float TextAlphaFactor
		{
			get
			{
				return this.DefaultStyle.TextAlphaFactor;
			}
			set
			{
				this.DefaultStyle.TextAlphaFactor = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003344 File Offset: 0x00001544
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003351 File Offset: 0x00001551
		public float TextHueFactor
		{
			get
			{
				return this.DefaultStyle.TextHueFactor;
			}
			set
			{
				this.DefaultStyle.TextHueFactor = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x0000335F File Offset: 0x0000155F
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x0000336C File Offset: 0x0000156C
		public float TextSaturationFactor
		{
			get
			{
				return this.DefaultStyle.TextSaturationFactor;
			}
			set
			{
				this.DefaultStyle.TextSaturationFactor = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000337A File Offset: 0x0000157A
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00003387 File Offset: 0x00001587
		public float TextValueFactor
		{
			get
			{
				return this.DefaultStyle.TextValueFactor;
			}
			set
			{
				this.DefaultStyle.TextValueFactor = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003395 File Offset: 0x00001595
		[Editor(false)]
		public Dictionary<string, BrushLayer>.ValueCollection Layers
		{
			get
			{
				return this._layers.Values;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000033A2 File Offset: 0x000015A2
		public StyleLayer DefaultStyleLayer
		{
			get
			{
				return this.DefaultStyle.DefaultLayer;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000033AF File Offset: 0x000015AF
		public BrushLayer DefaultLayer
		{
			get
			{
				return this._layers["Default"];
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000033C4 File Offset: 0x000015C4
		public Brush()
		{
			this._styles = new Dictionary<string, Style>();
			this._layers = new Dictionary<string, BrushLayer>();
			this._brushAnimations = new Dictionary<string, BrushAnimation>();
			this.SoundProperties = new SoundProperties();
			this.TextHorizontalAlignment = TextHorizontalAlignment.Center;
			this.TextVerticalAlignment = TextVerticalAlignment.Center;
			BrushLayer brushLayer = new BrushLayer();
			brushLayer.Name = "Default";
			this._layers.Add(brushLayer.Name, brushLayer);
			this.DefaultStyle = new Style(new List<BrushLayer> { brushLayer });
			this.DefaultStyle.Name = "Default";
			this.DefaultStyle.SetAsDefaultStyle();
			this.AddStyle(this.DefaultStyle);
			this.ClonedFrom = null;
			this.TransitionDuration = 0.05f;
			this.GlobalColorFactor = 1f;
			this.GlobalAlphaFactor = 1f;
			this.GlobalColor = Color.White;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000034A8 File Offset: 0x000016A8
		public Style GetStyle(string name)
		{
			Style style;
			this._styles.TryGetValue(name, out style);
			return style;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000AE RID: 174 RVA: 0x000034C5 File Offset: 0x000016C5
		[Editor(false)]
		public Dictionary<string, Style>.ValueCollection Styles
		{
			get
			{
				return this._styles.Values;
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000034D4 File Offset: 0x000016D4
		public Style GetStyleOrDefault(string name)
		{
			Style style;
			this._styles.TryGetValue(name, out style);
			return style ?? this.DefaultStyle;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000034FC File Offset: 0x000016FC
		public void AddStyle(Style style)
		{
			string name = style.Name;
			this._styles.Add(name, style);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000351D File Offset: 0x0000171D
		public void RemoveStyle(string styleName)
		{
			this._styles.Remove(styleName);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000352C File Offset: 0x0000172C
		public void AddLayer(BrushLayer layer)
		{
			this._layers.Add(layer.Name, layer);
			foreach (Style style in this.Styles)
			{
				style.AddLayer(new StyleLayer(layer));
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003594 File Offset: 0x00001794
		public void RemoveLayer(string layerName)
		{
			this._layers.Remove(layerName);
			foreach (Style style in this.Styles)
			{
				style.RemoveLayer(layerName);
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000035F4 File Offset: 0x000017F4
		public BrushLayer GetLayer(string name)
		{
			BrushLayer brushLayer;
			if (this._layers.TryGetValue(name, out brushLayer))
			{
				return brushLayer;
			}
			return null;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003614 File Offset: 0x00001814
		internal void FillForOverride(Brush originalBrush)
		{
			this.OverriddenBrush = originalBrush;
			this.FillFrom(this.OverriddenBrush);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000362C File Offset: 0x0000182C
		public void FillFrom(Brush brush)
		{
			this.Name = brush.Name;
			this.TransitionDuration = brush.TransitionDuration;
			this.TextVerticalAlignment = brush.TextVerticalAlignment;
			this.TextHorizontalAlignment = brush.TextHorizontalAlignment;
			this.GlobalColorFactor = brush.GlobalColorFactor;
			this.GlobalAlphaFactor = brush.GlobalAlphaFactor;
			this.GlobalColor = brush.GlobalColor;
			this._layers = new Dictionary<string, BrushLayer>();
			foreach (BrushLayer brushLayer in brush._layers.Values)
			{
				BrushLayer brushLayer2 = new BrushLayer();
				brushLayer2.FillFrom(brushLayer);
				this._layers.Add(brushLayer2.Name, brushLayer2);
			}
			this._styles = new Dictionary<string, Style>();
			Style style = brush._styles["Default"];
			Style style2 = new Style(this._layers.Values);
			style2.SetAsDefaultStyle();
			style2.FillFrom(style);
			this._styles.Add(style2.Name, style2);
			this.DefaultStyle = style2;
			foreach (Style style3 in brush._styles.Values)
			{
				if (style3.Name != "Default")
				{
					Style style4 = new Style(this._layers.Values);
					style4.DefaultStyle = this.DefaultStyle;
					style4.FillFrom(style3);
					this._styles.Add(style4.Name, style4);
				}
			}
			this._brushAnimations = new Dictionary<string, BrushAnimation>();
			foreach (BrushAnimation brushAnimation in brush._brushAnimations.Values)
			{
				BrushAnimation brushAnimation2 = new BrushAnimation();
				brushAnimation2.FillFrom(brushAnimation);
				this._brushAnimations.Add(brushAnimation2.Name, brushAnimation2);
			}
			this.SoundProperties = new SoundProperties();
			this.SoundProperties.FillFrom(brush.SoundProperties);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003870 File Offset: 0x00001A70
		public Brush Clone()
		{
			Brush brush = new Brush();
			brush.FillFrom(this);
			brush.Name = this.Name + "(Clone)";
			brush.ClonedFrom = this;
			return brush;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000389B File Offset: 0x00001A9B
		public void AddAnimation(BrushAnimation animation)
		{
			this._brushAnimations.Add(animation.Name, animation);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000038B0 File Offset: 0x00001AB0
		public BrushAnimation GetAnimation(string name)
		{
			BrushAnimation brushAnimation;
			if (name != null && this._brushAnimations.TryGetValue(name, out brushAnimation))
			{
				return brushAnimation;
			}
			return null;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000038D3 File Offset: 0x00001AD3
		public IEnumerable<BrushAnimation> GetAnimations()
		{
			return this._brushAnimations.Values;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000038E0 File Offset: 0x00001AE0
		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.Name))
			{
				return base.ToString();
			}
			return this.Name;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000038FC File Offset: 0x00001AFC
		public bool IsCloneRelated(Brush brush)
		{
			return this.ClonedFrom == brush || brush.ClonedFrom == this || brush.ClonedFrom == this.ClonedFrom;
		}

		// Token: 0x04000025 RID: 37
		private const float DefaultTransitionDuration = 0.05f;

		// Token: 0x0400002D RID: 45
		private Dictionary<string, Style> _styles;

		// Token: 0x0400002E RID: 46
		private Dictionary<string, BrushLayer> _layers;

		// Token: 0x0400002F RID: 47
		private Dictionary<string, BrushAnimation> _brushAnimations;
	}
}
