using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x0200009C RID: 156
	public class MultiplayerLobbyAfterBattlePopupWidget : Widget
	{
		// Token: 0x06000874 RID: 2164 RVA: 0x00018735 File Offset: 0x00016935
		public MultiplayerLobbyAfterBattlePopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00018740 File Offset: 0x00016940
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.IsActive = base.IsVisible;
			if (this._isActive)
			{
				this._timePassed += dt;
			}
			if (this._isFinished)
			{
				return;
			}
			if (this._timePassed >= this.AnimationDuration + this.AnimationDelay + (float)this._currentRewardIndex * this.RewardRevealDuration && this._currentRewardIndex < this.RewardsListPanel.Children.Count)
			{
				(this.RewardsListPanel.Children[this._currentRewardIndex] as MultiplayerLobbyBattleRewardWidget).StartAnimation();
				this._currentRewardIndex++;
			}
			if (this._timePassed >= this.AnimationDelay + this.AnimationDuration + (float)this.RewardsListPanel.Children.Count * this.RewardRevealDuration)
			{
				this._isFinished = true;
				this.ClickToContinueTextWidget.IsVisible = true;
			}
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0001882C File Offset: 0x00016A2C
		public void StartAnimation()
		{
			foreach (Widget widget in this.RewardsListPanel.Children)
			{
				(widget as MultiplayerLobbyBattleRewardWidget).StartPreAnimation();
			}
			this._isFinished = false;
			this._timePassed = 0f;
			this._currentRewardIndex = 0;
			this.ClickToContinueTextWidget.IsVisible = false;
			this.ExperiencePanel.StartAnimation(this.AnimationDelay);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x000188BC File Offset: 0x00016ABC
		private void Reset()
		{
			this.ExperiencePanel.Reset();
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x000188C9 File Offset: 0x00016AC9
		private void IsActiveUpdated()
		{
			if (this.IsActive)
			{
				this.StartAnimation();
				return;
			}
			this.Reset();
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x000188E0 File Offset: 0x00016AE0
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x000188E8 File Offset: 0x00016AE8
		[Editor(false)]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
					this.IsActiveUpdated();
				}
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x0001890C File Offset: 0x00016B0C
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x00018914 File Offset: 0x00016B14
		[Editor(false)]
		public float AnimationDelay
		{
			get
			{
				return this._animationDelay;
			}
			set
			{
				if (value != this._animationDelay)
				{
					this._animationDelay = value;
					base.OnPropertyChanged(value, "AnimationDelay");
				}
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x00018932 File Offset: 0x00016B32
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x0001893A File Offset: 0x00016B3A
		[Editor(false)]
		public float AnimationDuration
		{
			get
			{
				return this._animationDuration;
			}
			set
			{
				if (value != this._animationDuration)
				{
					this._animationDuration = value;
					base.OnPropertyChanged(value, "AnimationDuration");
				}
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x00018958 File Offset: 0x00016B58
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x00018960 File Offset: 0x00016B60
		[Editor(false)]
		public float RewardRevealDuration
		{
			get
			{
				return this._rewardRevealDuration;
			}
			set
			{
				if (value != this._rewardRevealDuration)
				{
					this._rewardRevealDuration = value;
					base.OnPropertyChanged(value, "RewardRevealDuration");
				}
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x0001897E File Offset: 0x00016B7E
		// (set) Token: 0x06000882 RID: 2178 RVA: 0x00018986 File Offset: 0x00016B86
		[Editor(false)]
		public MultiplayerLobbyAfterBattleExperiencePanelWidget ExperiencePanel
		{
			get
			{
				return this._experiencePanel;
			}
			set
			{
				if (value != this._experiencePanel)
				{
					this._experiencePanel = value;
					base.OnPropertyChanged<MultiplayerLobbyAfterBattleExperiencePanelWidget>(value, "ExperiencePanel");
				}
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x000189A4 File Offset: 0x00016BA4
		// (set) Token: 0x06000884 RID: 2180 RVA: 0x000189AC File Offset: 0x00016BAC
		[Editor(false)]
		public TextWidget ClickToContinueTextWidget
		{
			get
			{
				return this._clickToContinueTextWidget;
			}
			set
			{
				if (value != this._clickToContinueTextWidget)
				{
					this._clickToContinueTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "ClickToContinueTextWidget");
				}
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x000189CA File Offset: 0x00016BCA
		// (set) Token: 0x06000886 RID: 2182 RVA: 0x000189D2 File Offset: 0x00016BD2
		[Editor(false)]
		public ListPanel RewardsListPanel
		{
			get
			{
				return this._rewardsListPanel;
			}
			set
			{
				if (value != this._rewardsListPanel)
				{
					this._rewardsListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "RewardsListPanel");
				}
			}
		}

		// Token: 0x040003C1 RID: 961
		private bool _isFinished;

		// Token: 0x040003C2 RID: 962
		private float _timePassed;

		// Token: 0x040003C3 RID: 963
		private int _currentRewardIndex;

		// Token: 0x040003C4 RID: 964
		private bool _isActive;

		// Token: 0x040003C5 RID: 965
		private float _animationDelay;

		// Token: 0x040003C6 RID: 966
		private float _animationDuration;

		// Token: 0x040003C7 RID: 967
		private float _rewardRevealDuration;

		// Token: 0x040003C8 RID: 968
		private MultiplayerLobbyAfterBattleExperiencePanelWidget _experiencePanel;

		// Token: 0x040003C9 RID: 969
		private TextWidget _clickToContinueTextWidget;

		// Token: 0x040003CA RID: 970
		private ListPanel _rewardsListPanel;
	}
}
