using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000037 RID: 55
	public class VisualState
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0000FC2E File Offset: 0x0000DE2E
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x0000FC36 File Offset: 0x0000DE36
		public string State { get; private set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0000FC3F File Offset: 0x0000DE3F
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x0000FC47 File Offset: 0x0000DE47
		public float TransitionDuration
		{
			get
			{
				return this._transitionDuration;
			}
			set
			{
				this._transitionDuration = value;
				this.GotTransitionDuration = true;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x0000FC57 File Offset: 0x0000DE57
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x0000FC5F File Offset: 0x0000DE5F
		public float PositionXOffset
		{
			get
			{
				return this._positionXOffset;
			}
			set
			{
				this._positionXOffset = value;
				this.GotPositionXOffset = true;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0000FC6F File Offset: 0x0000DE6F
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x0000FC77 File Offset: 0x0000DE77
		public float PositionYOffset
		{
			get
			{
				return this._positionYOffset;
			}
			set
			{
				this._positionYOffset = value;
				this.GotPositionYOffset = true;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x0000FC87 File Offset: 0x0000DE87
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x0000FC8F File Offset: 0x0000DE8F
		public float SuggestedWidth
		{
			get
			{
				return this._suggestedWidth;
			}
			set
			{
				this._suggestedWidth = value;
				this.GotSuggestedWidth = true;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060003CA RID: 970 RVA: 0x0000FC9F File Offset: 0x0000DE9F
		// (set) Token: 0x060003CB RID: 971 RVA: 0x0000FCA7 File Offset: 0x0000DEA7
		public float SuggestedHeight
		{
			get
			{
				return this._suggestedHeight;
			}
			set
			{
				this._suggestedHeight = value;
				this.GotSuggestedHeight = true;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0000FCB7 File Offset: 0x0000DEB7
		// (set) Token: 0x060003CD RID: 973 RVA: 0x0000FCBF File Offset: 0x0000DEBF
		public float MarginTop
		{
			get
			{
				return this._marginTop;
			}
			set
			{
				this._marginTop = value;
				this.GotMarginTop = true;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0000FCCF File Offset: 0x0000DECF
		// (set) Token: 0x060003CF RID: 975 RVA: 0x0000FCD7 File Offset: 0x0000DED7
		public float MarginBottom
		{
			get
			{
				return this._marginBottom;
			}
			set
			{
				this._marginBottom = value;
				this.GotMarginBottom = true;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0000FCE7 File Offset: 0x0000DEE7
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x0000FCEF File Offset: 0x0000DEEF
		public float MarginLeft
		{
			get
			{
				return this._marginLeft;
			}
			set
			{
				this._marginLeft = value;
				this.GotMarginLeft = true;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x0000FCFF File Offset: 0x0000DEFF
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x0000FD07 File Offset: 0x0000DF07
		public float MarginRight
		{
			get
			{
				return this._marginRight;
			}
			set
			{
				this._marginRight = value;
				this.GotMarginRight = true;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0000FD17 File Offset: 0x0000DF17
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x0000FD1F File Offset: 0x0000DF1F
		public bool GotTransitionDuration { get; private set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x0000FD28 File Offset: 0x0000DF28
		// (set) Token: 0x060003D7 RID: 983 RVA: 0x0000FD30 File Offset: 0x0000DF30
		public bool GotPositionXOffset { get; private set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x0000FD39 File Offset: 0x0000DF39
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x0000FD41 File Offset: 0x0000DF41
		public bool GotPositionYOffset { get; private set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0000FD4A File Offset: 0x0000DF4A
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0000FD52 File Offset: 0x0000DF52
		public bool GotSuggestedWidth { get; private set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0000FD5B File Offset: 0x0000DF5B
		// (set) Token: 0x060003DD RID: 989 RVA: 0x0000FD63 File Offset: 0x0000DF63
		public bool GotSuggestedHeight { get; private set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0000FD6C File Offset: 0x0000DF6C
		// (set) Token: 0x060003DF RID: 991 RVA: 0x0000FD74 File Offset: 0x0000DF74
		public bool GotMarginTop { get; private set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x0000FD7D File Offset: 0x0000DF7D
		// (set) Token: 0x060003E1 RID: 993 RVA: 0x0000FD85 File Offset: 0x0000DF85
		public bool GotMarginBottom { get; private set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x0000FD8E File Offset: 0x0000DF8E
		// (set) Token: 0x060003E3 RID: 995 RVA: 0x0000FD96 File Offset: 0x0000DF96
		public bool GotMarginLeft { get; private set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x0000FD9F File Offset: 0x0000DF9F
		// (set) Token: 0x060003E5 RID: 997 RVA: 0x0000FDA7 File Offset: 0x0000DFA7
		public bool GotMarginRight { get; private set; }

		// Token: 0x060003E6 RID: 998 RVA: 0x0000FDB0 File Offset: 0x0000DFB0
		public VisualState(string state)
		{
			this.State = state;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000FDC0 File Offset: 0x0000DFC0
		public void FillFromWidget(Widget widget)
		{
			this.PositionXOffset = widget.PositionXOffset;
			this.PositionYOffset = widget.PositionYOffset;
			this.SuggestedWidth = widget.SuggestedWidth;
			this.SuggestedHeight = widget.SuggestedHeight;
			this.MarginTop = widget.MarginTop;
			this.MarginBottom = widget.MarginBottom;
			this.MarginLeft = widget.MarginLeft;
			this.MarginRight = widget.MarginRight;
		}

		// Token: 0x040001DA RID: 474
		private float _transitionDuration;

		// Token: 0x040001DB RID: 475
		private float _positionXOffset;

		// Token: 0x040001DC RID: 476
		private float _positionYOffset;

		// Token: 0x040001DD RID: 477
		private float _suggestedWidth;

		// Token: 0x040001DE RID: 478
		private float _suggestedHeight;

		// Token: 0x040001DF RID: 479
		private float _marginTop;

		// Token: 0x040001E0 RID: 480
		private float _marginBottom;

		// Token: 0x040001E1 RID: 481
		private float _marginLeft;

		// Token: 0x040001E2 RID: 482
		private float _marginRight;
	}
}
