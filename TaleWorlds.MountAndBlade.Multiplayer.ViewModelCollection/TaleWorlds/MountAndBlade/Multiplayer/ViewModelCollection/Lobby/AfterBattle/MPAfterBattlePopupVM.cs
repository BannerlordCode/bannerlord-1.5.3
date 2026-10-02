using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.MountAndBlade.Diamond.Ranked;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.AfterBattle
{
	// Token: 0x02000087 RID: 135
	public class MPAfterBattlePopupVM : ViewModel
	{
		// Token: 0x06000D37 RID: 3383 RVA: 0x00028C85 File Offset: 0x00026E85
		public MPAfterBattlePopupVM(Func<string> getExitText)
		{
			this._getExitText = getExitText;
			this.RefreshValues();
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00028CBC File Offset: 0x00026EBC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._battleResultsTitleText = new TextObject("{=pguhTmXw}Battle Results", null).ToString();
			this._levelUpTitleText = new TextObject("{=0tUYng4e}Leveled Up!", null).ToString();
			this._rankProgressTitleText = new TextObject("{=XEGaQB2G}Rank Progression", null).ToString();
			this._promotedTitleText = new TextObject("{=bn0v5ST0}Promoted!", null).ToString();
			this._demotedTitleText = new TextObject("{=HUndnpNw}Demoted!", null).ToString();
			this._evaluationFinishedTitleText = new TextObject("{=2KZLf51A}Evaluation Matches Finished", null).ToString();
			this.LevelText = GameTexts.FindText("str_level", null).ToString();
			this.ExperienceText = new TextObject("{=SwSaXwQg}exp", null).ToString();
			this.PointsText = new TextObject("{=4dRTWSN3}Points", null).ToString();
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00028D98 File Offset: 0x00026F98
		public void OpenWith(int oldExperience, int newExperience, List<string> badgesEarned, int lootGained, RankBarInfo oldRankBarInfo, RankBarInfo newRankBarInfo)
		{
			Func<string> getExitText = this._getExitText;
			this.ClickToContinueText = ((getExitText != null) ? getExitText() : null);
			this._oldExperience = oldExperience;
			this._newExperience = newExperience;
			this._earnedBadgeIDs = badgesEarned;
			this._lootGained = lootGained;
			this._oldRankBarInfo = oldRankBarInfo;
			this._newRankBarInfo = newRankBarInfo;
			this._hasRatingChanged = oldRankBarInfo != null && newRankBarInfo != null && !oldRankBarInfo.IsEvaluating && !newRankBarInfo.IsEvaluating;
			this._hasRankChanged = this._hasRatingChanged && oldRankBarInfo.RankId != newRankBarInfo.RankId;
			this._hasFinishedEvaluation = this._oldRankBarInfo != null && this._newRankBarInfo != null && this._oldRankBarInfo.IsEvaluating && !this._newRankBarInfo.IsEvaluating;
			this.AdvanceState();
			this.IsEnabled = true;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00028E74 File Offset: 0x00027074
		private void AdvanceState()
		{
			this.HideInfo();
			switch (this._currentState)
			{
			case MPAfterBattlePopupVM.AfterBattleState.None:
				this._currentState = MPAfterBattlePopupVM.AfterBattleState.GeneralProgression;
				this.ShowGeneralProgression();
				return;
			case MPAfterBattlePopupVM.AfterBattleState.GeneralProgression:
				if (this._hasLeveledUp)
				{
					this._currentState = MPAfterBattlePopupVM.AfterBattleState.LevelUp;
					this.ShowLevelUp();
					return;
				}
				if (this._hasRatingChanged)
				{
					this._currentState = MPAfterBattlePopupVM.AfterBattleState.RatingChange;
					this.ShowRankProgression();
					return;
				}
				if (this._hasFinishedEvaluation)
				{
					this._currentState = MPAfterBattlePopupVM.AfterBattleState.RankChange;
					this.ShowRankChange();
					return;
				}
				this.Disable();
				return;
			case MPAfterBattlePopupVM.AfterBattleState.LevelUp:
				if (this._hasRatingChanged)
				{
					this._currentState = MPAfterBattlePopupVM.AfterBattleState.RatingChange;
					this.ShowRankProgression();
					return;
				}
				if (this._hasFinishedEvaluation)
				{
					this._currentState = MPAfterBattlePopupVM.AfterBattleState.RankChange;
					this.ShowRankChange();
					return;
				}
				this.Disable();
				return;
			case MPAfterBattlePopupVM.AfterBattleState.RatingChange:
				if (this._hasRankChanged)
				{
					this._currentState = MPAfterBattlePopupVM.AfterBattleState.RankChange;
					this.ShowRankChange();
					return;
				}
				this.Disable();
				return;
			case MPAfterBattlePopupVM.AfterBattleState.RankChange:
				this.Disable();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00028F58 File Offset: 0x00027158
		private void ShowGeneralProgression()
		{
			this.TitleText = this._battleResultsTitleText;
			this.InitialRatio = 0;
			this.FinalRatio = 0;
			this.NumOfLevelUps = 0;
			PlayerDataExperience playerDataExperience = new PlayerDataExperience(this._oldExperience);
			PlayerDataExperience playerDataExperience2 = new PlayerDataExperience(this._newExperience);
			this.GainedExperience = this._newExperience - this._oldExperience;
			this.CurrentLevel = playerDataExperience.Level;
			this.NextLevel = this.CurrentLevel + 1;
			this.InitialRatio = (int)((float)playerDataExperience.ExperienceInCurrentLevel / (float)(playerDataExperience.ExperienceToNextLevel + playerDataExperience.ExperienceInCurrentLevel) * 100f);
			this.FinalRatio = (int)((float)playerDataExperience2.ExperienceInCurrentLevel / (float)(playerDataExperience2.ExperienceToNextLevel + playerDataExperience2.ExperienceInCurrentLevel) * 100f);
			this.NumOfLevelUps = playerDataExperience2.Level - playerDataExperience.Level;
			this._hasLeveledUp = this.NumOfLevelUps > 0;
			this.HasLostRating = this.GainedExperience < 0;
			float num = (float)this.NumOfLevelUps + (float)this.FinalRatio / 100f;
			this.LevelsExperienceRequirment = (int)((float)this._newExperience / num);
			this.RewardsEarned = new MBBindingList<MPAfterBattleRewardItemVM>();
			foreach (string text in this._earnedBadgeIDs)
			{
				Badge byId = BadgeManager.GetById(text);
				if (byId != null)
				{
					this.RewardsEarned.Add(new MPAfterBattleBadgeRewardItemVM(byId));
				}
			}
			if (this._lootGained > 0)
			{
				int num2 = this._lootGained - this._earnedBadgeIDs.Count * Parameters.LootRewardPerBadgeEarned;
				int num3 = this._lootGained - num2;
				this.RewardsEarned.Add(new MPAfterBattleLootRewardItemVM(num2, num3));
			}
			this.IsShowingGeneralProgression = true;
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00029124 File Offset: 0x00027324
		private void ShowLevelUp()
		{
			this.TitleText = this._levelUpTitleText;
			int level = new PlayerDataExperience(this._newExperience).Level;
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_level", null));
			GameTexts.SetVariable("STR2", level);
			this.ReachedLevelText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			SoundEvent.PlaySound2D("event:/ui/multiplayer/levelup");
			this.IsShowingNewLevel = true;
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0002919C File Offset: 0x0002739C
		private void ShowRankProgression()
		{
			this.TitleText = this._rankProgressTitleText;
			this.OldRankID = this._oldRankBarInfo.RankId;
			this.NewRankID = this._newRankBarInfo.RankId;
			this.OldRankName = MPLobbyVM.GetLocalizedRankName(this.OldRankID);
			this.NewRankName = MPLobbyVM.GetLocalizedRankName(this.NewRankID);
			this.HasLostRating = this._oldRankBarInfo.Rating > this._newRankBarInfo.Rating;
			this.ShownRating = this._newRankBarInfo.Rating;
			this.InitialRatio = (int)this._oldRankBarInfo.ProgressPercentage;
			this.FinalRatio = (int)this._newRankBarInfo.ProgressPercentage;
			this.NumOfLevelUps = Ranks.RankIds.IndexOf(this.NewRankID) - Ranks.RankIds.IndexOf(this.OldRankID);
			if (this.HasLostRating)
			{
				this._pointsLostTextObj.SetTextVariable("POINTS", this._oldRankBarInfo.Rating - this._newRankBarInfo.Rating);
				this.PointChangedText = this._pointsLostTextObj.ToString();
			}
			else
			{
				this._pointsGainedTextObj.SetTextVariable("POINTS", this._newRankBarInfo.Rating - this._oldRankBarInfo.Rating);
				this.PointChangedText = this._pointsGainedTextObj.ToString();
			}
			this.IsShowingRankProgression = true;
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x000292F8 File Offset: 0x000274F8
		private void ShowRankChange()
		{
			if (this.OldRankID != string.Empty && this.OldRankID != null)
			{
				this.TitleText = ((Ranks.RankIds.IndexOf(this.OldRankID) < Ranks.RankIds.IndexOf(this.NewRankID)) ? this._promotedTitleText : this._demotedTitleText);
				this.IsShowingNewRank = true;
				return;
			}
			if (this._hasFinishedEvaluation)
			{
				this.OldRankID = string.Empty;
				this.NewRankID = this._newRankBarInfo.RankId;
				this.OldRankName = MPLobbyVM.GetLocalizedRankName(this.OldRankID);
				this.NewRankName = MPLobbyVM.GetLocalizedRankName(this.NewRankID);
				this.TitleText = this._evaluationFinishedTitleText;
				this.IsShowingNewRank = true;
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x000293BA File Offset: 0x000275BA
		private void HideInfo()
		{
			this.IsShowingGeneralProgression = false;
			this.IsShowingNewLevel = false;
			this.IsShowingRankProgression = false;
			this.IsShowingNewRank = false;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x000293D8 File Offset: 0x000275D8
		private void Disable()
		{
			this.HideInfo();
			this.ShownRating = 0;
			this._currentState = MPAfterBattlePopupVM.AfterBattleState.None;
			this.IsEnabled = false;
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x000293F5 File Offset: 0x000275F5
		public void ExecuteClose()
		{
			if (this.IsEnabled)
			{
				this.AdvanceState();
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06000D42 RID: 3394 RVA: 0x00029405 File Offset: 0x00027605
		// (set) Token: 0x06000D43 RID: 3395 RVA: 0x0002940D File Offset: 0x0002760D
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

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x0002942B File Offset: 0x0002762B
		// (set) Token: 0x06000D45 RID: 3397 RVA: 0x00029433 File Offset: 0x00027633
		[DataSourceProperty]
		public bool IsShowingGeneralProgression
		{
			get
			{
				return this._isShowingGeneralProgression;
			}
			set
			{
				if (value != this._isShowingGeneralProgression)
				{
					this._isShowingGeneralProgression = value;
					base.OnPropertyChangedWithValue(value, "IsShowingGeneralProgression");
				}
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x00029451 File Offset: 0x00027651
		// (set) Token: 0x06000D47 RID: 3399 RVA: 0x00029459 File Offset: 0x00027659
		[DataSourceProperty]
		public bool IsShowingNewLevel
		{
			get
			{
				return this._isShowingNewLevel;
			}
			set
			{
				if (value != this._isShowingNewLevel)
				{
					this._isShowingNewLevel = value;
					base.OnPropertyChangedWithValue(value, "IsShowingNewLevel");
				}
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x00029477 File Offset: 0x00027677
		// (set) Token: 0x06000D49 RID: 3401 RVA: 0x0002947F File Offset: 0x0002767F
		[DataSourceProperty]
		public bool IsShowingRankProgression
		{
			get
			{
				return this._isShowingRankProgression;
			}
			set
			{
				if (value != this._isShowingRankProgression)
				{
					this._isShowingRankProgression = value;
					base.OnPropertyChangedWithValue(value, "IsShowingRankProgression");
				}
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x0002949D File Offset: 0x0002769D
		// (set) Token: 0x06000D4B RID: 3403 RVA: 0x000294A5 File Offset: 0x000276A5
		[DataSourceProperty]
		public bool IsShowingNewRank
		{
			get
			{
				return this._isShowingNewRank;
			}
			set
			{
				if (value != this._isShowingNewRank)
				{
					this._isShowingNewRank = value;
					base.OnPropertyChangedWithValue(value, "IsShowingNewRank");
				}
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06000D4C RID: 3404 RVA: 0x000294C3 File Offset: 0x000276C3
		// (set) Token: 0x06000D4D RID: 3405 RVA: 0x000294CB File Offset: 0x000276CB
		[DataSourceProperty]
		public bool HasLostRating
		{
			get
			{
				return this._hasLostRating;
			}
			set
			{
				if (value != this._hasLostRating)
				{
					this._hasLostRating = value;
					base.OnPropertyChangedWithValue(value, "HasLostRating");
				}
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06000D4E RID: 3406 RVA: 0x000294E9 File Offset: 0x000276E9
		// (set) Token: 0x06000D4F RID: 3407 RVA: 0x000294F1 File Offset: 0x000276F1
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06000D50 RID: 3408 RVA: 0x00029514 File Offset: 0x00027714
		// (set) Token: 0x06000D51 RID: 3409 RVA: 0x0002951C File Offset: 0x0002771C
		[DataSourceProperty]
		public string LevelText
		{
			get
			{
				return this._levelText;
			}
			set
			{
				if (value != this._levelText)
				{
					this._levelText = value;
					base.OnPropertyChangedWithValue<string>(value, "LevelText");
				}
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x0002953F File Offset: 0x0002773F
		// (set) Token: 0x06000D53 RID: 3411 RVA: 0x00029547 File Offset: 0x00027747
		[DataSourceProperty]
		public string ExperienceText
		{
			get
			{
				return this._experienceText;
			}
			set
			{
				if (value != this._experienceText)
				{
					this._experienceText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExperienceText");
				}
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x0002956A File Offset: 0x0002776A
		// (set) Token: 0x06000D55 RID: 3413 RVA: 0x00029572 File Offset: 0x00027772
		[DataSourceProperty]
		public string ClickToContinueText
		{
			get
			{
				return this._clickToContinueText;
			}
			set
			{
				if (value != this._clickToContinueText)
				{
					this._clickToContinueText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClickToContinueText");
				}
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x00029595 File Offset: 0x00027795
		// (set) Token: 0x06000D57 RID: 3415 RVA: 0x0002959D File Offset: 0x0002779D
		[DataSourceProperty]
		public string ReachedLevelText
		{
			get
			{
				return this._reachedLevelText;
			}
			set
			{
				if (value != this._reachedLevelText)
				{
					this._reachedLevelText = value;
					base.OnPropertyChangedWithValue<string>(value, "ReachedLevelText");
				}
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x000295C0 File Offset: 0x000277C0
		// (set) Token: 0x06000D59 RID: 3417 RVA: 0x000295C8 File Offset: 0x000277C8
		[DataSourceProperty]
		public string PointsText
		{
			get
			{
				return this._pointsText;
			}
			set
			{
				if (value != this._pointsText)
				{
					this._pointsText = value;
					base.OnPropertyChangedWithValue<string>(value, "PointsText");
				}
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x000295EB File Offset: 0x000277EB
		// (set) Token: 0x06000D5B RID: 3419 RVA: 0x000295F3 File Offset: 0x000277F3
		[DataSourceProperty]
		public string PointChangedText
		{
			get
			{
				return this._pointChangeText;
			}
			set
			{
				if (value != this._pointChangeText)
				{
					this._pointChangeText = value;
					base.OnPropertyChangedWithValue<string>(value, "PointChangedText");
				}
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00029616 File Offset: 0x00027816
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x0002961E File Offset: 0x0002781E
		[DataSourceProperty]
		public string OldRankID
		{
			get
			{
				return this._oldRankID;
			}
			set
			{
				if (value != this._oldRankID)
				{
					this._oldRankID = value;
					base.OnPropertyChangedWithValue<string>(value, "OldRankID");
				}
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00029641 File Offset: 0x00027841
		// (set) Token: 0x06000D5F RID: 3423 RVA: 0x00029649 File Offset: 0x00027849
		[DataSourceProperty]
		public string NewRankID
		{
			get
			{
				return this._newRankID;
			}
			set
			{
				if (value != this._newRankID)
				{
					this._newRankID = value;
					base.OnPropertyChangedWithValue<string>(value, "NewRankID");
				}
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x0002966C File Offset: 0x0002786C
		// (set) Token: 0x06000D61 RID: 3425 RVA: 0x00029674 File Offset: 0x00027874
		[DataSourceProperty]
		public string OldRankName
		{
			get
			{
				return this._oldRankName;
			}
			set
			{
				if (value != this._oldRankName)
				{
					this._oldRankName = value;
					base.OnPropertyChangedWithValue<string>(value, "OldRankName");
				}
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x00029697 File Offset: 0x00027897
		// (set) Token: 0x06000D63 RID: 3427 RVA: 0x0002969F File Offset: 0x0002789F
		[DataSourceProperty]
		public string NewRankName
		{
			get
			{
				return this._newRankName;
			}
			set
			{
				if (value != this._newRankName)
				{
					this._newRankName = value;
					base.OnPropertyChangedWithValue<string>(value, "NewRankName");
				}
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x000296C2 File Offset: 0x000278C2
		// (set) Token: 0x06000D65 RID: 3429 RVA: 0x000296CA File Offset: 0x000278CA
		[DataSourceProperty]
		public int FinalRatio
		{
			get
			{
				return this._finalRatio;
			}
			set
			{
				if (value != this._finalRatio)
				{
					this._finalRatio = value;
					base.OnPropertyChangedWithValue(value, "FinalRatio");
				}
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x000296E8 File Offset: 0x000278E8
		// (set) Token: 0x06000D67 RID: 3431 RVA: 0x000296F0 File Offset: 0x000278F0
		[DataSourceProperty]
		public int NumOfLevelUps
		{
			get
			{
				return this._numOfLevelUps;
			}
			set
			{
				if (value != this._numOfLevelUps)
				{
					this._numOfLevelUps = value;
					base.OnPropertyChangedWithValue(value, "NumOfLevelUps");
				}
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x0002970E File Offset: 0x0002790E
		// (set) Token: 0x06000D69 RID: 3433 RVA: 0x00029716 File Offset: 0x00027916
		[DataSourceProperty]
		public int InitialRatio
		{
			get
			{
				return this._initialRatio;
			}
			set
			{
				if (value != this._initialRatio)
				{
					this._initialRatio = value;
					base.OnPropertyChangedWithValue(value, "InitialRatio");
				}
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x00029734 File Offset: 0x00027934
		// (set) Token: 0x06000D6B RID: 3435 RVA: 0x0002973C File Offset: 0x0002793C
		[DataSourceProperty]
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
					base.OnPropertyChangedWithValue(value, "GainedExperience");
				}
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x0002975A File Offset: 0x0002795A
		// (set) Token: 0x06000D6D RID: 3437 RVA: 0x00029762 File Offset: 0x00027962
		[DataSourceProperty]
		public int LevelsExperienceRequirment
		{
			get
			{
				return this._levelsExperienceRequirment;
			}
			set
			{
				if (value != this._levelsExperienceRequirment)
				{
					this._levelsExperienceRequirment = value;
					base.OnPropertyChangedWithValue(value, "LevelsExperienceRequirment");
				}
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x00029780 File Offset: 0x00027980
		// (set) Token: 0x06000D6F RID: 3439 RVA: 0x00029788 File Offset: 0x00027988
		[DataSourceProperty]
		public int NextLevel
		{
			get
			{
				return this._nextLevel;
			}
			set
			{
				if (value != this._nextLevel)
				{
					this._nextLevel = value;
					base.OnPropertyChangedWithValue(value, "NextLevel");
				}
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x000297A6 File Offset: 0x000279A6
		// (set) Token: 0x06000D71 RID: 3441 RVA: 0x000297AE File Offset: 0x000279AE
		[DataSourceProperty]
		public int CurrentLevel
		{
			get
			{
				return this._currentLevel;
			}
			set
			{
				if (value != this._currentLevel)
				{
					this._currentLevel = value;
					base.OnPropertyChangedWithValue(value, "CurrentLevel");
				}
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x000297CC File Offset: 0x000279CC
		// (set) Token: 0x06000D73 RID: 3443 RVA: 0x000297D4 File Offset: 0x000279D4
		[DataSourceProperty]
		public int ShownRating
		{
			get
			{
				return this._shownRating;
			}
			set
			{
				if (value != this._shownRating)
				{
					this._shownRating = value;
					base.OnPropertyChangedWithValue(value, "ShownRating");
				}
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x000297F2 File Offset: 0x000279F2
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x000297FA File Offset: 0x000279FA
		[DataSourceProperty]
		public MBBindingList<MPAfterBattleRewardItemVM> RewardsEarned
		{
			get
			{
				return this._rewardsEarned;
			}
			set
			{
				if (value != this._rewardsEarned)
				{
					this._rewardsEarned = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPAfterBattleRewardItemVM>>(value, "RewardsEarned");
				}
			}
		}

		// Token: 0x040005F7 RID: 1527
		private MPAfterBattlePopupVM.AfterBattleState _currentState;

		// Token: 0x040005F8 RID: 1528
		private bool _hasLeveledUp;

		// Token: 0x040005F9 RID: 1529
		private int _oldExperience;

		// Token: 0x040005FA RID: 1530
		private int _newExperience;

		// Token: 0x040005FB RID: 1531
		private List<string> _earnedBadgeIDs;

		// Token: 0x040005FC RID: 1532
		private int _lootGained;

		// Token: 0x040005FD RID: 1533
		private bool _hasRatingChanged;

		// Token: 0x040005FE RID: 1534
		private bool _hasRankChanged;

		// Token: 0x040005FF RID: 1535
		private bool _hasFinishedEvaluation;

		// Token: 0x04000600 RID: 1536
		private RankBarInfo _oldRankBarInfo;

		// Token: 0x04000601 RID: 1537
		private RankBarInfo _newRankBarInfo;

		// Token: 0x04000602 RID: 1538
		private string _battleResultsTitleText;

		// Token: 0x04000603 RID: 1539
		private string _levelUpTitleText;

		// Token: 0x04000604 RID: 1540
		private string _rankProgressTitleText;

		// Token: 0x04000605 RID: 1541
		private string _promotedTitleText;

		// Token: 0x04000606 RID: 1542
		private string _demotedTitleText;

		// Token: 0x04000607 RID: 1543
		private string _evaluationFinishedTitleText;

		// Token: 0x04000608 RID: 1544
		private TextObject _pointsGainedTextObj = new TextObject("{=EFU3uo0y}You've gained {POINTS} points", null);

		// Token: 0x04000609 RID: 1545
		private TextObject _pointsLostTextObj = new TextObject("{=oMYz0PvL}You've lost {POINTS} points", null);

		// Token: 0x0400060A RID: 1546
		private readonly Func<string> _getExitText;

		// Token: 0x0400060B RID: 1547
		private bool _isEnabled;

		// Token: 0x0400060C RID: 1548
		private bool _isShowingGeneralProgression;

		// Token: 0x0400060D RID: 1549
		private bool _isShowingNewLevel;

		// Token: 0x0400060E RID: 1550
		private bool _isShowingRankProgression;

		// Token: 0x0400060F RID: 1551
		private bool _isShowingNewRank;

		// Token: 0x04000610 RID: 1552
		private bool _hasLostRating;

		// Token: 0x04000611 RID: 1553
		private string _titleText;

		// Token: 0x04000612 RID: 1554
		private string _levelText;

		// Token: 0x04000613 RID: 1555
		private string _experienceText;

		// Token: 0x04000614 RID: 1556
		private string _clickToContinueText;

		// Token: 0x04000615 RID: 1557
		private string _reachedLevelText;

		// Token: 0x04000616 RID: 1558
		private string _pointsText;

		// Token: 0x04000617 RID: 1559
		private string _pointChangeText;

		// Token: 0x04000618 RID: 1560
		private string _oldRankID;

		// Token: 0x04000619 RID: 1561
		private string _newRankID;

		// Token: 0x0400061A RID: 1562
		private string _oldRankName;

		// Token: 0x0400061B RID: 1563
		private string _newRankName;

		// Token: 0x0400061C RID: 1564
		private int _initialRatio;

		// Token: 0x0400061D RID: 1565
		private int _finalRatio;

		// Token: 0x0400061E RID: 1566
		private int _numOfLevelUps;

		// Token: 0x0400061F RID: 1567
		private int _gainedExperience;

		// Token: 0x04000620 RID: 1568
		private int _levelsExperienceRequirment;

		// Token: 0x04000621 RID: 1569
		private int _currentLevel;

		// Token: 0x04000622 RID: 1570
		private int _nextLevel;

		// Token: 0x04000623 RID: 1571
		private int _shownRating;

		// Token: 0x04000624 RID: 1572
		private MBBindingList<MPAfterBattleRewardItemVM> _rewardsEarned;

		// Token: 0x02000183 RID: 387
		private enum AfterBattleState
		{
			// Token: 0x04000A94 RID: 2708
			None,
			// Token: 0x04000A95 RID: 2709
			GeneralProgression,
			// Token: 0x04000A96 RID: 2710
			LevelUp,
			// Token: 0x04000A97 RID: 2711
			RatingChange,
			// Token: 0x04000A98 RID: 2712
			RankChange
		}
	}
}
