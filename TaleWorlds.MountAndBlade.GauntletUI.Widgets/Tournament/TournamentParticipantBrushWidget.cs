using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tournament
{
	// Token: 0x02000052 RID: 82
	public class TournamentParticipantBrushWidget : BrushWidget
	{
		// Token: 0x06000478 RID: 1144 RVA: 0x0000E50E File Offset: 0x0000C70E
		public TournamentParticipantBrushWidget(UIContext context)
			: base(context)
		{
			base.AddState("Current");
			base.AddState("Over");
			base.AddState("Dead");
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000E538 File Offset: 0x0000C738
		protected override void OnMousePressed()
		{
			base.OnMousePressed();
			base.EventFired("ClickEvent", Array.Empty<object>());
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000E550 File Offset: 0x0000C750
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.AddState("Current");
			child.AddState("Over");
			child.AddState("Dead");
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000E57A File Offset: 0x0000C77A
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			base.ParentWidget.AddState("Current");
			base.ParentWidget.AddState("Over");
			base.ParentWidget.AddState("Dead");
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000E5B2 File Offset: 0x0000C7B2
		private void SetWidgetState(string state)
		{
			base.ParentWidget.SetState(state);
			this.SetState(state);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000E5C8 File Offset: 0x0000C7C8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._brushApplied)
			{
				this.NameTextWidget.Brush = (this.IsMainHero ? this.MainHeroTextBrush : this.NormalTextBrush);
				this._brushApplied = true;
			}
			if (this._stateChanged && base.ReadOnlyBrush != null && base.BrushRenderer.Brush != null)
			{
				this._stateChanged = false;
				this.SetWidgetState("Default");
				foreach (BrushLayer brushLayer in base.Brush.Layers)
				{
					brushLayer.Color = base.Brush.Color;
				}
				if (this.OnMission)
				{
					base.Brush.GlobalAlphaFactor = 0.75f;
				}
				else
				{
					base.Brush.GlobalAlphaFactor = 1f;
				}
				if (this.MatchState == 0)
				{
					this.SetWidgetState("Default");
					return;
				}
				if (this.MatchState == 1)
				{
					this.SetWidgetState("Current");
					return;
				}
				if (this.MatchState == 2)
				{
					this.SetWidgetState("Over");
					return;
				}
				if (this.MatchState == 3)
				{
					if (this._isDead && this.OnMission)
					{
						this.SetWidgetState("Dead");
						return;
					}
					this.SetWidgetState("Default");
				}
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x0000E730 File Offset: 0x0000C930
		// (set) Token: 0x0600047F RID: 1151 RVA: 0x0000E738 File Offset: 0x0000C938
		public TextWidget NameTextWidget
		{
			get
			{
				return this._nameTextWidget;
			}
			set
			{
				if (this._nameTextWidget != value)
				{
					this._nameTextWidget = value;
				}
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x0000E74A File Offset: 0x0000C94A
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x0000E752 File Offset: 0x0000C952
		public int MatchState
		{
			get
			{
				return this._matchState;
			}
			set
			{
				if (this._matchState != value)
				{
					this._stateChanged = true;
					this._matchState = value;
				}
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x0000E76B File Offset: 0x0000C96B
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x0000E773 File Offset: 0x0000C973
		public bool IsDead
		{
			get
			{
				return this._isDead;
			}
			set
			{
				if (this._isDead != value)
				{
					this._stateChanged = true;
					this._isDead = value;
				}
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x0000E78C File Offset: 0x0000C98C
		// (set) Token: 0x06000485 RID: 1157 RVA: 0x0000E794 File Offset: 0x0000C994
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (this._isMainHero != value)
				{
					this._isMainHero = value;
					this._brushApplied = false;
				}
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x0000E7AD File Offset: 0x0000C9AD
		// (set) Token: 0x06000487 RID: 1159 RVA: 0x0000E7B5 File Offset: 0x0000C9B5
		public Brush MainHeroTextBrush
		{
			get
			{
				return this._mainHeroTextBrush;
			}
			set
			{
				if (this._mainHeroTextBrush != value)
				{
					this._mainHeroTextBrush = value;
				}
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x0000E7C7 File Offset: 0x0000C9C7
		// (set) Token: 0x06000489 RID: 1161 RVA: 0x0000E7CF File Offset: 0x0000C9CF
		public Brush NormalTextBrush
		{
			get
			{
				return this._normalTextBrush;
			}
			set
			{
				if (this._normalTextBrush != value)
				{
					this._normalTextBrush = value;
				}
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x0000E7E1 File Offset: 0x0000C9E1
		// (set) Token: 0x0600048B RID: 1163 RVA: 0x0000E7E9 File Offset: 0x0000C9E9
		public bool OnMission
		{
			get
			{
				return this._onMission;
			}
			set
			{
				if (this._onMission != value)
				{
					this._stateChanged = true;
					this._onMission = value;
				}
			}
		}

		// Token: 0x040001E5 RID: 485
		private bool _stateChanged;

		// Token: 0x040001E6 RID: 486
		private bool _brushApplied;

		// Token: 0x040001E7 RID: 487
		private int _matchState;

		// Token: 0x040001E8 RID: 488
		private bool _isDead;

		// Token: 0x040001E9 RID: 489
		private bool _onMission;

		// Token: 0x040001EA RID: 490
		private bool _isMainHero;

		// Token: 0x040001EB RID: 491
		private Brush _mainHeroTextBrush;

		// Token: 0x040001EC RID: 492
		private Brush _normalTextBrush;

		// Token: 0x040001ED RID: 493
		private TextWidget _nameTextWidget;
	}
}
