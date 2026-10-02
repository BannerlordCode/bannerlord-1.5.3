using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Scoreboard;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tournament
{
	// Token: 0x02000053 RID: 83
	public class TournamentScreenWidget : Widget
	{
		// Token: 0x0600048C RID: 1164 RVA: 0x0000E802 File Offset: 0x0000CA02
		public TournamentScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000E80B File Offset: 0x0000CA0B
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._isAnimationActive && this.IsOver)
			{
				this.StartBattleResultAnimation();
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000E82C File Offset: 0x0000CA2C
		private void StartBattleResultAnimation()
		{
			this._isAnimationActive = true;
			DelayedStateChanger shieldStateChanger = this.ShieldStateChanger;
			if (shieldStateChanger != null)
			{
				shieldStateChanger.Start();
			}
			DelayedStateChanger winnerTextContainer = this.WinnerTextContainer1;
			if (winnerTextContainer != null)
			{
				winnerTextContainer.Start();
			}
			DelayedStateChanger characterContainer = this.CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.Start();
			}
			DelayedStateChanger rewardsContainer = this.RewardsContainer;
			if (rewardsContainer != null)
			{
				rewardsContainer.Start();
			}
			DelayedStateChanger flagsSuccess = this.FlagsSuccess;
			if (flagsSuccess != null)
			{
				flagsSuccess.Start();
			}
			ScoreboardBattleRewardsWidget scoreboardBattleRewardsWidget = this.ScoreboardBattleRewardsWidget;
			if (scoreboardBattleRewardsWidget == null)
			{
				return;
			}
			scoreboardBattleRewardsWidget.StartAnimation();
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x0000E8A5 File Offset: 0x0000CAA5
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x0000E8AD File Offset: 0x0000CAAD
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
				}
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x0000E8CB File Offset: 0x0000CACB
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x0000E8D3 File Offset: 0x0000CAD3
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

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0000E8F1 File Offset: 0x0000CAF1
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x0000E8F9 File Offset: 0x0000CAF9
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

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0000E917 File Offset: 0x0000CB17
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x0000E91F File Offset: 0x0000CB1F
		[Editor(false)]
		public DelayedStateChanger WinnerTextContainer1
		{
			get
			{
				return this._winnerTextContainer1;
			}
			set
			{
				if (this._winnerTextContainer1 != value)
				{
					this._winnerTextContainer1 = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "WinnerTextContainer1");
				}
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x0000E93D File Offset: 0x0000CB3D
		// (set) Token: 0x06000498 RID: 1176 RVA: 0x0000E945 File Offset: 0x0000CB45
		[Editor(false)]
		public DelayedStateChanger CharacterContainer
		{
			get
			{
				return this._characterContainer;
			}
			set
			{
				if (this._characterContainer != value)
				{
					this._characterContainer = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "CharacterContainer");
				}
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0000E963 File Offset: 0x0000CB63
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x0000E96B File Offset: 0x0000CB6B
		[Editor(false)]
		public DelayedStateChanger RewardsContainer
		{
			get
			{
				return this._rewardsContainer;
			}
			set
			{
				if (this._rewardsContainer != value)
				{
					this._rewardsContainer = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "RewardsContainer");
				}
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0000E989 File Offset: 0x0000CB89
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x0000E991 File Offset: 0x0000CB91
		[Editor(false)]
		public ScoreboardBattleRewardsWidget ScoreboardBattleRewardsWidget
		{
			get
			{
				return this._scoreboardBattleRewardsWidget;
			}
			set
			{
				if (this._scoreboardBattleRewardsWidget != value)
				{
					this._scoreboardBattleRewardsWidget = value;
					base.OnPropertyChanged<ScoreboardBattleRewardsWidget>(value, "ScoreboardBattleRewardsWidget");
				}
			}
		}

		// Token: 0x040001EE RID: 494
		private bool _isAnimationActive;

		// Token: 0x040001EF RID: 495
		private bool _isOver;

		// Token: 0x040001F0 RID: 496
		private DelayedStateChanger _flagsSuccess;

		// Token: 0x040001F1 RID: 497
		private DelayedStateChanger _shieldStateChanger;

		// Token: 0x040001F2 RID: 498
		private DelayedStateChanger _winnerTextContainer1;

		// Token: 0x040001F3 RID: 499
		private DelayedStateChanger _characterContainer;

		// Token: 0x040001F4 RID: 500
		private DelayedStateChanger _rewardsContainer;

		// Token: 0x040001F5 RID: 501
		private ScoreboardBattleRewardsWidget _scoreboardBattleRewardsWidget;
	}
}
