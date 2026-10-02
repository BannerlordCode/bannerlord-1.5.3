using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.NameMarker
{
	// Token: 0x020000F6 RID: 246
	public class DuelTargetMarkerListPanel : ListPanel
	{
		// Token: 0x06000CAC RID: 3244 RVA: 0x00022B76 File Offset: 0x00020D76
		public DuelTargetMarkerListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00022B80 File Offset: 0x00020D80
		protected override void OnLateUpdate(float dt)
		{
			if (!this.IsAvailable)
			{
				base.IsVisible = false;
				return;
			}
			float x = base.Context.EventManager.PageSize.X;
			float y = base.Context.EventManager.PageSize.Y;
			Vec2 vec = this.Position;
			if (this.WSign > 0 && vec.x - base.Size.X / 2f > 0f && vec.x + base.Size.X / 2f < base.Context.EventManager.PageSize.X && vec.y > 0f && vec.y + base.Size.Y < base.Context.EventManager.PageSize.Y)
			{
				base.ScaledPositionXOffset = vec.x - base.Size.X / 2f;
				base.ScaledPositionYOffset = vec.y - base.Size.Y - 20f;
				this._actionText.ScaledPositionXOffset = base.ScaledPositionXOffset;
				this._actionText.ScaledPositionYOffset = base.ScaledPositionYOffset + base.Size.Y;
				base.IsVisible = true;
				return;
			}
			if (this.IsTracked)
			{
				Vec2 vec2 = new Vec2(base.Context.EventManager.PageSize.X / 2f, base.Context.EventManager.PageSize.Y / 2f);
				vec -= vec2;
				if (this.WSign < 0)
				{
					vec *= -1f;
				}
				float num = Mathf.Atan2(vec.y, vec.x) - 1.5707964f;
				float num2 = Mathf.Cos(num);
				float num3 = Mathf.Sin(num);
				vec = vec2 + new Vec2(num3 * 150f, num2 * 150f);
				float num4 = num2 / num3;
				Vec2 vec3 = vec2 * 1f;
				vec = ((num2 > 0f) ? new Vec2(-vec3.y / num4, vec2.y) : new Vec2(vec3.y / num4, -vec2.y));
				if (vec.x > vec3.x)
				{
					vec = new Vec2(vec3.x, -vec3.x * num4);
				}
				else if (vec.x < -vec3.x)
				{
					vec = new Vec2(-vec3.x, vec3.x * num4);
				}
				vec += vec2;
				base.ScaledPositionXOffset = Mathf.Clamp(vec.x - base.Size.X / 2f, 0f, x - base.Size.X);
				base.ScaledPositionYOffset = Mathf.Clamp(vec.y - base.Size.Y, 0f, y - base.Size.Y);
				base.IsVisible = true;
				return;
			}
			base.IsVisible = false;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00022E9C File Offset: 0x0002109C
		private void UpdateChildrenFocusStates()
		{
			string text = (this.HasTargetSentDuelRequest ? "Tracked" : ((this.HasPlayerSentDuelRequest || this.IsAgentFocused) ? "Focused" : "Default"));
			this.Background.SetState(text);
			this.Border.SetState(text);
			BrushWidget troopClassBorder = this.TroopClassBorder;
			if (troopClassBorder == null)
			{
				return;
			}
			troopClassBorder.SetState(text);
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x00022EFE File Offset: 0x000210FE
		// (set) Token: 0x06000CB0 RID: 3248 RVA: 0x00022F06 File Offset: 0x00021106
		[Editor(false)]
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (value != this._position)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x00022F29 File Offset: 0x00021129
		// (set) Token: 0x06000CB2 RID: 3250 RVA: 0x00022F31 File Offset: 0x00021131
		[Editor(false)]
		public bool IsAgentInScreenBoundaries
		{
			get
			{
				return this._isAgentInScreenBoundaries;
			}
			set
			{
				if (value != this._isAgentInScreenBoundaries)
				{
					this._isAgentInScreenBoundaries = value;
					base.OnPropertyChanged(value, "IsAgentInScreenBoundaries");
				}
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x00022F4F File Offset: 0x0002114F
		// (set) Token: 0x06000CB4 RID: 3252 RVA: 0x00022F57 File Offset: 0x00021157
		[Editor(false)]
		public bool IsAvailable
		{
			get
			{
				return this._isAvailable;
			}
			set
			{
				if (value != this._isAvailable)
				{
					this._isAvailable = value;
					base.OnPropertyChanged(value, "IsAvailable");
				}
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x00022F75 File Offset: 0x00021175
		// (set) Token: 0x06000CB6 RID: 3254 RVA: 0x00022F7D File Offset: 0x0002117D
		[Editor(false)]
		public bool IsTracked
		{
			get
			{
				return this._isTracked;
			}
			set
			{
				if (value != this._isTracked)
				{
					this._isTracked = value;
					base.OnPropertyChanged(value, "IsTracked");
				}
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00022F9B File Offset: 0x0002119B
		// (set) Token: 0x06000CB8 RID: 3256 RVA: 0x00022FA3 File Offset: 0x000211A3
		[Editor(false)]
		public bool IsAgentFocused
		{
			get
			{
				return this._isAgentFocused;
			}
			set
			{
				if (value != this._isAgentFocused)
				{
					this._isAgentFocused = value;
					base.OnPropertyChanged(value, "IsAgentFocused");
					this.UpdateChildrenFocusStates();
				}
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00022FC7 File Offset: 0x000211C7
		// (set) Token: 0x06000CBA RID: 3258 RVA: 0x00022FCF File Offset: 0x000211CF
		[Editor(false)]
		public bool HasTargetSentDuelRequest
		{
			get
			{
				return this._hasTargetSentDuelRequest;
			}
			set
			{
				if (value != this._hasTargetSentDuelRequest)
				{
					this._hasTargetSentDuelRequest = value;
					base.OnPropertyChanged(value, "HasTargetSentDuelRequest");
					this.UpdateChildrenFocusStates();
				}
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x00022FF3 File Offset: 0x000211F3
		// (set) Token: 0x06000CBC RID: 3260 RVA: 0x00022FFB File Offset: 0x000211FB
		[Editor(false)]
		public bool HasPlayerSentDuelRequest
		{
			get
			{
				return this._hasPlayerSentDuelRequest;
			}
			set
			{
				if (value != this._hasPlayerSentDuelRequest)
				{
					this._hasPlayerSentDuelRequest = value;
					base.OnPropertyChanged(value, "HasPlayerSentDuelRequest");
					this.UpdateChildrenFocusStates();
				}
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x0002301F File Offset: 0x0002121F
		// (set) Token: 0x06000CBE RID: 3262 RVA: 0x00023027 File Offset: 0x00021227
		[Editor(false)]
		public int WSign
		{
			get
			{
				return this._wSign;
			}
			set
			{
				if (this._wSign != value)
				{
					this._wSign = value;
					base.OnPropertyChanged(value, "WSign");
				}
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x00023045 File Offset: 0x00021245
		// (set) Token: 0x06000CC0 RID: 3264 RVA: 0x0002304D File Offset: 0x0002124D
		[Editor(false)]
		public RichTextWidget ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (value != this._actionText)
				{
					this._actionText = value;
					base.OnPropertyChanged<RichTextWidget>(value, "ActionText");
				}
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0002306B File Offset: 0x0002126B
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x00023073 File Offset: 0x00021273
		[Editor(false)]
		public BrushWidget Background
		{
			get
			{
				return this._background;
			}
			set
			{
				if (value != this._background)
				{
					this._background = value;
					base.OnPropertyChanged<BrushWidget>(value, "Background");
				}
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x00023091 File Offset: 0x00021291
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x00023099 File Offset: 0x00021299
		[Editor(false)]
		public BrushWidget Border
		{
			get
			{
				return this._border;
			}
			set
			{
				if (value != this._border)
				{
					this._border = value;
					base.OnPropertyChanged<BrushWidget>(value, "Border");
				}
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x000230B7 File Offset: 0x000212B7
		// (set) Token: 0x06000CC6 RID: 3270 RVA: 0x000230BF File Offset: 0x000212BF
		[Editor(false)]
		public BrushWidget TroopClassBorder
		{
			get
			{
				return this._troopClassBorder;
			}
			set
			{
				if (value != this._troopClassBorder)
				{
					this._troopClassBorder = value;
					base.OnPropertyChanged<BrushWidget>(value, "TroopClassBorder");
				}
			}
		}

		// Token: 0x040005BD RID: 1469
		private const string DefaultState = "Default";

		// Token: 0x040005BE RID: 1470
		private const string FocusedState = "Focused";

		// Token: 0x040005BF RID: 1471
		private const string TrackedState = "Tracked";

		// Token: 0x040005C0 RID: 1472
		private Vec2 _position;

		// Token: 0x040005C1 RID: 1473
		private bool _isAgentInScreenBoundaries;

		// Token: 0x040005C2 RID: 1474
		private bool _isAvailable;

		// Token: 0x040005C3 RID: 1475
		private bool _isTracked;

		// Token: 0x040005C4 RID: 1476
		private bool _isAgentFocused;

		// Token: 0x040005C5 RID: 1477
		private bool _hasTargetSentDuelRequest;

		// Token: 0x040005C6 RID: 1478
		private bool _hasPlayerSentDuelRequest;

		// Token: 0x040005C7 RID: 1479
		private int _wSign;

		// Token: 0x040005C8 RID: 1480
		private RichTextWidget _actionText;

		// Token: 0x040005C9 RID: 1481
		private BrushWidget _background;

		// Token: 0x040005CA RID: 1482
		private BrushWidget _border;

		// Token: 0x040005CB RID: 1483
		private BrushWidget _troopClassBorder;
	}
}
