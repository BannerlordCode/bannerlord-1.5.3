using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Scoreboard
{
	// Token: 0x02000057 RID: 87
	public class ScoreboardScreenWidget : Widget
	{
		// Token: 0x060004B8 RID: 1208 RVA: 0x0000EF90 File Offset: 0x0000D190
		public ScoreboardScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0000EF9C File Offset: 0x0000D19C
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.ScrollablePanel != null && this.ScrollGradient != null)
			{
				this.ScrollGradient.IsVisible = this.ScrollablePanel.InnerPanel.Size.Y > this.ScrollablePanel.ClipRect.Size.Y;
			}
			if (!this._isAnimationActive && this.ShowScoreboard && this.IsOver)
			{
				this.StartBattleResultAnimation();
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0000F018 File Offset: 0x0000D218
		private void UpdateControlButtonsPanel()
		{
			this._controlButtonsPanel.IsVisible = this.ShowScoreboard || this.IsMainCharacterDead;
			this.InputKeysPanel.IsVisible = !this.ShowScoreboard && !this.IsSimulation && this.IsMainCharacterDead;
			this.ShowMouseIconWidget.IsVisible = !this.IsMouseEnabled && this.ShowScoreboard && !this.IsSimulation && !this.IsMainCharacterDead;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0000F094 File Offset: 0x0000D294
		private void UpdateControlButtons()
		{
			this._fastForwardWidget.IsVisible = (this.IsMainCharacterDead || this.IsSimulation) && this.ShowScoreboard;
			this._quitButton.IsVisible = this.ShowScoreboard;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000F0CC File Offset: 0x0000D2CC
		private void StartBattleResultAnimation()
		{
			this._isAnimationActive = true;
			ScoreboardBattleRewardsWidget battleRewardsWidget = this.BattleRewardsWidget;
			if (battleRewardsWidget != null)
			{
				battleRewardsWidget.StartAnimation();
			}
			DelayedStateChanger shieldStateChanger = this.ShieldStateChanger;
			if (shieldStateChanger != null)
			{
				shieldStateChanger.Start();
			}
			DelayedStateChanger shipsStateChanger = this.ShipsStateChanger;
			if (shipsStateChanger != null)
			{
				shipsStateChanger.Start();
			}
			DelayedStateChanger titleStateChanger = this.TitleStateChanger;
			if (titleStateChanger != null)
			{
				titleStateChanger.Start();
			}
			DelayedStateChanger titleBackgroundStateChanger = this.TitleBackgroundStateChanger;
			if (titleBackgroundStateChanger != null)
			{
				titleBackgroundStateChanger.Start();
			}
			if (this.BattleResult == 0)
			{
				if (this.FlagsDefeat != null)
				{
					this.FlagsDefeat.IsVisible = true;
					this.FlagsDefeat.Start();
					return;
				}
			}
			else if (this.BattleResult == 1)
			{
				if (this.FlagsSuccess != null)
				{
					this.FlagsSuccess.IsVisible = true;
					this.FlagsSuccess.Start();
					return;
				}
			}
			else if (this.BattleResult == 2 && this.FlagsRetreat != null)
			{
				this.FlagsRetreat.IsVisible = true;
				this.FlagsRetreat.Start();
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0000F1AE File Offset: 0x0000D3AE
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x0000F1B6 File Offset: 0x0000D3B6
		[Editor(false)]
		public bool ShowScoreboard
		{
			get
			{
				return this._showScoreboard;
			}
			set
			{
				if (this._showScoreboard != value)
				{
					this._showScoreboard = value;
					base.OnPropertyChanged(value, "ShowScoreboard");
					this.UpdateControlButtonsPanel();
					this.UpdateControlButtons();
				}
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0000F1E0 File Offset: 0x0000D3E0
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x0000F1E8 File Offset: 0x0000D3E8
		[Editor(false)]
		public bool IsOver
		{
			get
			{
				return this._isOver;
			}
			set
			{
				if (this._isOver != value)
				{
					this._isOver = value;
					base.OnPropertyChanged(value, "IsOver");
					this.UpdateControlButtons();
				}
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x0000F20C File Offset: 0x0000D40C
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x0000F214 File Offset: 0x0000D414
		[Editor(false)]
		public int BattleResult
		{
			get
			{
				return this._battleResult;
			}
			set
			{
				if (this._battleResult != value)
				{
					this._battleResult = value;
					base.OnPropertyChanged(value, "BattleResult");
				}
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0000F232 File Offset: 0x0000D432
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x0000F23A File Offset: 0x0000D43A
		[Editor(false)]
		public bool IsMainCharacterDead
		{
			get
			{
				return this._isMainCharacterDead;
			}
			set
			{
				if (this._isMainCharacterDead != value)
				{
					this._isMainCharacterDead = value;
					base.OnPropertyChanged(value, "IsMainCharacterDead");
					this.UpdateControlButtonsPanel();
					this.UpdateControlButtons();
				}
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x0000F264 File Offset: 0x0000D464
		// (set) Token: 0x060004C6 RID: 1222 RVA: 0x0000F26C File Offset: 0x0000D46C
		[Editor(false)]
		public bool IsSimulation
		{
			get
			{
				return this._isSimulation;
			}
			set
			{
				if (this._isSimulation != value)
				{
					this._isSimulation = value;
					base.OnPropertyChanged(value, "IsSimulation");
					this.UpdateControlButtonsPanel();
					this.UpdateControlButtons();
				}
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x0000F296 File Offset: 0x0000D496
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x0000F29E File Offset: 0x0000D49E
		[Editor(false)]
		public bool IsMouseEnabled
		{
			get
			{
				return this._isMouseEnabled;
			}
			set
			{
				if (this._isMouseEnabled != value)
				{
					this._isMouseEnabled = value;
					base.OnPropertyChanged(value, "IsMouseEnabled");
					this.UpdateControlButtonsPanel();
				}
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x0000F2C2 File Offset: 0x0000D4C2
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x0000F2CA File Offset: 0x0000D4CA
		[Editor(false)]
		public ScrollablePanel ScrollablePanel
		{
			get
			{
				return this._scrollablePanel;
			}
			set
			{
				if (this._scrollablePanel != value)
				{
					this._scrollablePanel = value;
					base.OnPropertyChanged<ScrollablePanel>(value, "ScrollablePanel");
				}
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x0000F2E8 File Offset: 0x0000D4E8
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x0000F2F0 File Offset: 0x0000D4F0
		[Editor(false)]
		public Widget ScrollGradient
		{
			get
			{
				return this._scrollGradient;
			}
			set
			{
				if (this._scrollGradient != value)
				{
					this._scrollGradient = value;
					base.OnPropertyChanged<Widget>(value, "ScrollGradient");
				}
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x0000F30E File Offset: 0x0000D50E
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x0000F316 File Offset: 0x0000D516
		[Editor(false)]
		public Widget ControlButtonsPanel
		{
			get
			{
				return this._controlButtonsPanel;
			}
			set
			{
				if (this._controlButtonsPanel != value)
				{
					this._controlButtonsPanel = value;
					base.OnPropertyChanged<Widget>(value, "ControlButtonsPanel");
				}
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x0000F334 File Offset: 0x0000D534
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x0000F33C File Offset: 0x0000D53C
		[Editor(false)]
		public ListPanel InputKeysPanel
		{
			get
			{
				return this._inputKeysPanel;
			}
			set
			{
				if (this._inputKeysPanel != value)
				{
					this._inputKeysPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "InputKeysPanel");
				}
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x0000F35A File Offset: 0x0000D55A
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x0000F362 File Offset: 0x0000D562
		[Editor(false)]
		public Widget ShowMouseIconWidget
		{
			get
			{
				return this._showMouseIconWidget;
			}
			set
			{
				if (this._showMouseIconWidget != value)
				{
					this._showMouseIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "ShowMouseIconWidget");
				}
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x0000F380 File Offset: 0x0000D580
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x0000F388 File Offset: 0x0000D588
		[Editor(false)]
		public Widget FastForwardWidget
		{
			get
			{
				return this._fastForwardWidget;
			}
			set
			{
				if (this._fastForwardWidget != value)
				{
					this._fastForwardWidget = value;
					base.OnPropertyChanged<Widget>(value, "FastForwardWidget");
				}
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0000F3A6 File Offset: 0x0000D5A6
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x0000F3AE File Offset: 0x0000D5AE
		[Editor(false)]
		public ButtonWidget QuitButton
		{
			get
			{
				return this._quitButton;
			}
			set
			{
				if (this._quitButton != value)
				{
					this._quitButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "QuitButton");
				}
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0000F3CC File Offset: 0x0000D5CC
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x0000F3D4 File Offset: 0x0000D5D4
		[Editor(false)]
		public ButtonWidget ShowScoreboardToggle
		{
			get
			{
				return this._showScoreboardToggle;
			}
			set
			{
				if (this._showScoreboardToggle != value)
				{
					this._showScoreboardToggle = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ShowScoreboardToggle");
				}
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0000F3F2 File Offset: 0x0000D5F2
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x0000F3FA File Offset: 0x0000D5FA
		[Editor(false)]
		public ScoreboardBattleRewardsWidget BattleRewardsWidget
		{
			get
			{
				return this._battleRewardsWidget;
			}
			set
			{
				if (this._battleRewardsWidget != value)
				{
					this._battleRewardsWidget = value;
					base.OnPropertyChanged<ScoreboardBattleRewardsWidget>(value, "BattleRewardsWidget");
				}
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x0000F418 File Offset: 0x0000D618
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x0000F420 File Offset: 0x0000D620
		[Editor(false)]
		public DelayedStateChanger FlagsSuccess
		{
			get
			{
				return this._flagsSuccess;
			}
			set
			{
				if (this._flagsSuccess != value)
				{
					this._flagsSuccess = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "FlagsSuccess");
				}
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0000F43E File Offset: 0x0000D63E
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x0000F446 File Offset: 0x0000D646
		[Editor(false)]
		public DelayedStateChanger FlagsRetreat
		{
			get
			{
				return this._flagsRetreat;
			}
			set
			{
				if (this._flagsRetreat != value)
				{
					this._flagsRetreat = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "FlagsRetreat");
				}
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x0000F464 File Offset: 0x0000D664
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x0000F46C File Offset: 0x0000D66C
		[Editor(false)]
		public DelayedStateChanger FlagsDefeat
		{
			get
			{
				return this._flagsDefeat;
			}
			set
			{
				if (this._flagsDefeat != value)
				{
					this._flagsDefeat = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "FlagsDefeat");
				}
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x0000F48A File Offset: 0x0000D68A
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x0000F492 File Offset: 0x0000D692
		[Editor(false)]
		public DelayedStateChanger ShieldStateChanger
		{
			get
			{
				return this._shieldStateChanger;
			}
			set
			{
				if (this._shieldStateChanger != value)
				{
					this._shieldStateChanger = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "ShieldStateChanger");
				}
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0000F4B0 File Offset: 0x0000D6B0
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x0000F4B8 File Offset: 0x0000D6B8
		[Editor(false)]
		public DelayedStateChanger ShipsStateChanger
		{
			get
			{
				return this._shipsStateChanger;
			}
			set
			{
				if (this._shipsStateChanger != value)
				{
					this._shipsStateChanger = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "ShipsStateChanger");
				}
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0000F4D6 File Offset: 0x0000D6D6
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x0000F4DE File Offset: 0x0000D6DE
		[Editor(false)]
		public DelayedStateChanger TitleStateChanger
		{
			get
			{
				return this._titleStateChanger;
			}
			set
			{
				if (this._titleStateChanger != value)
				{
					this._titleStateChanger = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "TitleStateChanger");
				}
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0000F4FC File Offset: 0x0000D6FC
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x0000F504 File Offset: 0x0000D704
		[Editor(false)]
		public DelayedStateChanger TitleBackgroundStateChanger
		{
			get
			{
				return this._titleBackgroundStateChanger;
			}
			set
			{
				if (this._titleBackgroundStateChanger != value)
				{
					this._titleBackgroundStateChanger = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "TitleBackgroundStateChanger");
				}
			}
		}

		// Token: 0x04000202 RID: 514
		private bool _isAnimationActive;

		// Token: 0x04000203 RID: 515
		private bool _showScoreboard;

		// Token: 0x04000204 RID: 516
		private bool _isOver;

		// Token: 0x04000205 RID: 517
		private int _battleResult;

		// Token: 0x04000206 RID: 518
		private bool _isMainCharacterDead;

		// Token: 0x04000207 RID: 519
		private bool _isSimulation;

		// Token: 0x04000208 RID: 520
		private bool _isMouseEnabled;

		// Token: 0x04000209 RID: 521
		private ScrollablePanel _scrollablePanel;

		// Token: 0x0400020A RID: 522
		private Widget _scrollGradient;

		// Token: 0x0400020B RID: 523
		private Widget _controlButtonsPanel;

		// Token: 0x0400020C RID: 524
		private Widget _showMouseIconWidget;

		// Token: 0x0400020D RID: 525
		private ListPanel _inputKeysPanel;

		// Token: 0x0400020E RID: 526
		private Widget _fastForwardWidget;

		// Token: 0x0400020F RID: 527
		private ButtonWidget _showScoreboardToggle;

		// Token: 0x04000210 RID: 528
		private ButtonWidget _quitButton;

		// Token: 0x04000211 RID: 529
		private ScoreboardBattleRewardsWidget _battleRewardsWidget;

		// Token: 0x04000212 RID: 530
		private DelayedStateChanger _flagsSuccess;

		// Token: 0x04000213 RID: 531
		private DelayedStateChanger _flagsDefeat;

		// Token: 0x04000214 RID: 532
		private DelayedStateChanger _flagsRetreat;

		// Token: 0x04000215 RID: 533
		private DelayedStateChanger _shieldStateChanger;

		// Token: 0x04000216 RID: 534
		private DelayedStateChanger _shipsStateChanger;

		// Token: 0x04000217 RID: 535
		private DelayedStateChanger _titleStateChanger;

		// Token: 0x04000218 RID: 536
		private DelayedStateChanger _titleBackgroundStateChanger;
	}
}
