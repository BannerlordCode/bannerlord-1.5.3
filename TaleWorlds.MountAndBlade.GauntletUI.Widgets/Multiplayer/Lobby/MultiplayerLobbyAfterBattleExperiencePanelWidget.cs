using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x0200009B RID: 155
	public class MultiplayerLobbyAfterBattleExperiencePanelWidget : Widget
	{
		// Token: 0x06000865 RID: 2149 RVA: 0x00018583 File Offset: 0x00016783
		public MultiplayerLobbyAfterBattleExperiencePanelWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0001858C File Offset: 0x0001678C
		public void StartAnimation(float animationDelay)
		{
			this.ExperienceFillBar.StartAnimation(animationDelay);
			this.EarnedExperienceCounterTextWidget.IntTarget = this.GainedExperience;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x000185AB File Offset: 0x000167AB
		public void Reset()
		{
			MultiplayerScoreboardAnimatedFillBarWidget experienceFillBar = this.ExperienceFillBar;
			if (experienceFillBar != null)
			{
				experienceFillBar.Reset();
			}
			CounterTextBrushWidget earnedExperienceCounterTextWidget = this.EarnedExperienceCounterTextWidget;
			if (earnedExperienceCounterTextWidget == null)
			{
				return;
			}
			earnedExperienceCounterTextWidget.SetInitialValue(0f);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x000185D3 File Offset: 0x000167D3
		private void OnFillBarFill(bool isPositive)
		{
			this.CurrentLevelTextWidget.IntText += (isPositive ? 1 : (-1));
			this.NextLevelTextWidget.IntText += (isPositive ? 1 : (-1));
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00018607 File Offset: 0x00016807
		protected override void RefreshState()
		{
			if (base.IsHidden)
			{
				MultiplayerScoreboardAnimatedFillBarWidget experienceFillBar = this.ExperienceFillBar;
				if (experienceFillBar == null)
				{
					return;
				}
				experienceFillBar.Reset();
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x00018621 File Offset: 0x00016821
		// (set) Token: 0x0600086B RID: 2155 RVA: 0x00018629 File Offset: 0x00016829
		[Editor(false)]
		public int GainedExperience
		{
			get
			{
				return this._gainedExperience;
			}
			set
			{
				if (value != this._gainedExperience)
				{
					this._gainedExperience = value;
					base.OnPropertyChanged(value, "GainedExperience");
				}
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x00018647 File Offset: 0x00016847
		// (set) Token: 0x0600086D RID: 2157 RVA: 0x00018650 File Offset: 0x00016850
		[Editor(false)]
		public MultiplayerScoreboardAnimatedFillBarWidget ExperienceFillBar
		{
			get
			{
				return this._experienceFillBar;
			}
			set
			{
				if (value != this._experienceFillBar)
				{
					if (this._experienceFillBar != null)
					{
						this._experienceFillBar.OnFullFillFinished -= this.OnFillBarFill;
					}
					this._experienceFillBar = value;
					if (this._experienceFillBar != null)
					{
						this._experienceFillBar.OnFullFillFinished += this.OnFillBarFill;
					}
					base.OnPropertyChanged<MultiplayerScoreboardAnimatedFillBarWidget>(value, "ExperienceFillBar");
					this.Reset();
				}
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x000186BD File Offset: 0x000168BD
		// (set) Token: 0x0600086F RID: 2159 RVA: 0x000186C5 File Offset: 0x000168C5
		[Editor(false)]
		public CounterTextBrushWidget EarnedExperienceCounterTextWidget
		{
			get
			{
				return this._earnedExperienceCounterTextWidget;
			}
			set
			{
				if (value != this._earnedExperienceCounterTextWidget)
				{
					this._earnedExperienceCounterTextWidget = value;
					base.OnPropertyChanged<CounterTextBrushWidget>(value, "EarnedExperienceCounterTextWidget");
					this.Reset();
				}
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x000186E9 File Offset: 0x000168E9
		// (set) Token: 0x06000871 RID: 2161 RVA: 0x000186F1 File Offset: 0x000168F1
		[Editor(false)]
		public TextWidget CurrentLevelTextWidget
		{
			get
			{
				return this._currentLevelTextWidget;
			}
			set
			{
				if (value != this._currentLevelTextWidget)
				{
					this._currentLevelTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "CurrentLevelTextWidget");
				}
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x0001870F File Offset: 0x0001690F
		// (set) Token: 0x06000873 RID: 2163 RVA: 0x00018717 File Offset: 0x00016917
		public TextWidget NextLevelTextWidget
		{
			get
			{
				return this._nextLevelTextWidget;
			}
			set
			{
				if (value != this._nextLevelTextWidget)
				{
					this._nextLevelTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "NextLevelTextWidget");
				}
			}
		}

		// Token: 0x040003BC RID: 956
		private int _gainedExperience;

		// Token: 0x040003BD RID: 957
		private MultiplayerScoreboardAnimatedFillBarWidget _experienceFillBar;

		// Token: 0x040003BE RID: 958
		private CounterTextBrushWidget _earnedExperienceCounterTextWidget;

		// Token: 0x040003BF RID: 959
		private TextWidget _currentLevelTextWidget;

		// Token: 0x040003C0 RID: 960
		private TextWidget _nextLevelTextWidget;
	}
}
