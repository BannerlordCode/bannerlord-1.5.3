using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TournamentLeaderboard
{
	// Token: 0x020000B5 RID: 181
	public class TournamentLeaderboardVM : ViewModel
	{
		// Token: 0x06001119 RID: 4377 RVA: 0x0004515C File Offset: 0x0004335C
		public TournamentLeaderboardVM()
		{
			this.Entries = new MBBindingList<TournamentLeaderboardEntryItemVM>();
			List<KeyValuePair<Hero, int>> leaderboard = Campaign.Current.TournamentManager.GetLeaderboard();
			for (int i = 0; i < leaderboard.Count; i++)
			{
				this.Entries.Add(new TournamentLeaderboardEntryItemVM(leaderboard[i].Key, leaderboard[i].Value, i + 1));
			}
			this.SortController = new TournamentLeaderboardSortControllerVM(ref this._entries);
			this.RefreshValues();
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x000451E4 File Offset: 0x000433E4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.Entries.ApplyActionOnAllItems(delegate(TournamentLeaderboardEntryItemVM x)
			{
				x.RefreshValues();
			});
			this.HeroText = GameTexts.FindText("str_hero", null).ToString();
			this.VictoriesText = GameTexts.FindText("str_leaderboard_victories", null).ToString();
			this.RankText = GameTexts.FindText("str_rank_sign", null).ToString();
			this.TitleText = GameTexts.FindText("str_leaderboard_title", null).ToString();
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0004528F File Offset: 0x0004348F
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.OnFinalize();
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x000452A7 File Offset: 0x000434A7
		public void ExecuteDone()
		{
			this.IsEnabled = false;
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x000452B0 File Offset: 0x000434B0
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600111E RID: 4382 RVA: 0x000452BF File Offset: 0x000434BF
		// (set) Token: 0x0600111F RID: 4383 RVA: 0x000452C7 File Offset: 0x000434C7
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001120 RID: 4384 RVA: 0x000452E5 File Offset: 0x000434E5
		// (set) Token: 0x06001121 RID: 4385 RVA: 0x000452ED File Offset: 0x000434ED
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

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001122 RID: 4386 RVA: 0x0004530B File Offset: 0x0004350B
		// (set) Token: 0x06001123 RID: 4387 RVA: 0x00045313 File Offset: 0x00043513
		[DataSourceProperty]
		public TournamentLeaderboardSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChangedWithValue<TournamentLeaderboardSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001124 RID: 4388 RVA: 0x00045331 File Offset: 0x00043531
		// (set) Token: 0x06001125 RID: 4389 RVA: 0x00045339 File Offset: 0x00043539
		[DataSourceProperty]
		public MBBindingList<TournamentLeaderboardEntryItemVM> Entries
		{
			get
			{
				return this._entries;
			}
			set
			{
				if (value != this._entries)
				{
					this._entries = value;
					base.OnPropertyChangedWithValue<MBBindingList<TournamentLeaderboardEntryItemVM>>(value, "Entries");
				}
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001126 RID: 4390 RVA: 0x00045357 File Offset: 0x00043557
		// (set) Token: 0x06001127 RID: 4391 RVA: 0x0004535F File Offset: 0x0004355F
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001128 RID: 4392 RVA: 0x00045382 File Offset: 0x00043582
		// (set) Token: 0x06001129 RID: 4393 RVA: 0x0004538A File Offset: 0x0004358A
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

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x0600112A RID: 4394 RVA: 0x000453AD File Offset: 0x000435AD
		// (set) Token: 0x0600112B RID: 4395 RVA: 0x000453B5 File Offset: 0x000435B5
		[DataSourceProperty]
		public string HeroText
		{
			get
			{
				return this._heroText;
			}
			set
			{
				if (value != this._heroText)
				{
					this._heroText = value;
					base.OnPropertyChangedWithValue<string>(value, "HeroText");
				}
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x000453D8 File Offset: 0x000435D8
		// (set) Token: 0x0600112D RID: 4397 RVA: 0x000453E0 File Offset: 0x000435E0
		[DataSourceProperty]
		public string VictoriesText
		{
			get
			{
				return this._victoriesText;
			}
			set
			{
				if (value != this._victoriesText)
				{
					this._victoriesText = value;
					base.OnPropertyChangedWithValue<string>(value, "VictoriesText");
				}
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x00045403 File Offset: 0x00043603
		// (set) Token: 0x0600112F RID: 4399 RVA: 0x0004540B File Offset: 0x0004360B
		[DataSourceProperty]
		public string RankText
		{
			get
			{
				return this._rankText;
			}
			set
			{
				if (value != this._rankText)
				{
					this._rankText = value;
					base.OnPropertyChangedWithValue<string>(value, "RankText");
				}
			}
		}

		// Token: 0x040007C5 RID: 1989
		private InputKeyItemVM _doneInputKey;

		// Token: 0x040007C6 RID: 1990
		private bool _isEnabled;

		// Token: 0x040007C7 RID: 1991
		private string _doneText;

		// Token: 0x040007C8 RID: 1992
		private string _heroText;

		// Token: 0x040007C9 RID: 1993
		private string _victoriesText;

		// Token: 0x040007CA RID: 1994
		private string _rankText;

		// Token: 0x040007CB RID: 1995
		private string _titleText;

		// Token: 0x040007CC RID: 1996
		private MBBindingList<TournamentLeaderboardEntryItemVM> _entries;

		// Token: 0x040007CD RID: 1997
		private TournamentLeaderboardSortControllerVM _sortController;
	}
}
