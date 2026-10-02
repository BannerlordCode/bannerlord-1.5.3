using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.OfficialGame
{
	// Token: 0x02000045 RID: 69
	public class MPMatchmakingSelectionInfoVM : ViewModel
	{
		// Token: 0x06000667 RID: 1639 RVA: 0x00014BCE File Offset: 0x00012DCE
		public MPMatchmakingSelectionInfoVM()
		{
			this.Name = "";
			this.Description = "";
			this.ExtraInfos = new MBBindingList<StringPairItemVM>();
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00014BF8 File Offset: 0x00012DF8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._playersDescription = new TextObject("{=RfXJdNye}Players", null).ToString();
			this._averagePlaytimeDescription = new TextObject("{=YAaAlbkX}Avg. Playtime", null).ToString();
			this._roundsDescription = new TextObject("{=iKtIhlbo}Rounds", null).ToString();
			this._roundTimeDescription = new TextObject("{=r5WzivPb}Round Time", null).ToString();
			this._objectivesDescription = new TextObject("{=gqNxq11A}Objectives", null).ToString();
			this._troopsDescription = new TextObject("{=5k4dxUEJ}Troops", null).ToString();
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00014C90 File Offset: 0x00012E90
		public void UpdateForGameType(string gameTypeStr)
		{
			this.Name = GameTexts.FindText("str_multiplayer_official_game_type_name", gameTypeStr).ToString();
			MBTextManager.SetTextVariable("newline", "\n", false);
			this.Description = GameTexts.FindText("str_multiplayer_official_game_type_description", gameTypeStr).ToString();
			this.ExtraInfos.Clear();
			int num = MultiplayerOptions.Instance.GetNumberOfPlayersForGameMode(gameTypeStr) / 2;
			int roundCountForGameMode = MultiplayerOptions.Instance.GetRoundCountForGameMode(gameTypeStr);
			int roundTimeLimitInMinutesForGameMode = MultiplayerOptions.Instance.GetRoundTimeLimitInMinutesForGameMode(gameTypeStr);
			int num2 = ((roundCountForGameMode == 1) ? 1 : (roundCountForGameMode / 2 + 1));
			int num3 = num2 * roundTimeLimitInMinutesForGameMode;
			MBTextManager.SetTextVariable("PLAYER_COUNT", num.ToString(), false);
			string text = GameTexts.FindText("str_multiplayer_official_game_type_player_info_for_versus", null).ToString();
			MBTextManager.SetTextVariable("PLAY_TIME", num3.ToString(), false);
			string text2 = GameTexts.FindText("str_multiplayer_official_game_type_playtime_info_in_minutes", null).ToString();
			MBTextManager.SetTextVariable("ROUND_COUNT", num2.ToString(), false);
			string text3 = GameTexts.FindText("str_multiplayer_official_game_type_rounds_info_for_best_of", null).ToString();
			MBTextManager.SetTextVariable("PLAY_TIME", roundTimeLimitInMinutesForGameMode.ToString(), false);
			string text4 = GameTexts.FindText("str_multiplayer_official_game_type_playtime_info_in_minutes", null).ToString();
			string text5 = GameTexts.FindText("str_multiplayer_official_game_type_objective_info", gameTypeStr).ToString();
			string text6 = GameTexts.FindText("str_multiplayer_official_game_type_troops_info", gameTypeStr).ToString();
			this.ExtraInfos.Add(new StringPairItemVM(this._playersDescription, text, null));
			this.ExtraInfos.Add(new StringPairItemVM(this._averagePlaytimeDescription, text2, null));
			this.ExtraInfos.Add(new StringPairItemVM(this._roundsDescription, text3, null));
			this.ExtraInfos.Add(new StringPairItemVM(this._roundTimeDescription, text4, null));
			this.ExtraInfos.Add(new StringPairItemVM(this._objectivesDescription, text5, null));
			this.ExtraInfos.Add(new StringPairItemVM(this._troopsDescription, text6, null));
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00014E66 File Offset: 0x00013066
		public void SetEnabled(bool isEnabled)
		{
			this.IsEnabled = isEnabled;
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00014E6F File Offset: 0x0001306F
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x00014E77 File Offset: 0x00013077
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x00014E9A File Offset: 0x0001309A
		// (set) Token: 0x0600066E RID: 1646 RVA: 0x00014EA2 File Offset: 0x000130A2
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

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x00014EC0 File Offset: 0x000130C0
		// (set) Token: 0x06000670 RID: 1648 RVA: 0x00014EC8 File Offset: 0x000130C8
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00014EEB File Offset: 0x000130EB
		// (set) Token: 0x06000672 RID: 1650 RVA: 0x00014EF3 File Offset: 0x000130F3
		[DataSourceProperty]
		public MBBindingList<StringPairItemVM> ExtraInfos
		{
			get
			{
				return this._extraInfos;
			}
			set
			{
				if (value != this._extraInfos)
				{
					this._extraInfos = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemVM>>(value, "ExtraInfos");
				}
			}
		}

		// Token: 0x04000303 RID: 771
		private string _playersDescription;

		// Token: 0x04000304 RID: 772
		private string _averagePlaytimeDescription;

		// Token: 0x04000305 RID: 773
		private string _roundsDescription;

		// Token: 0x04000306 RID: 774
		private string _roundTimeDescription;

		// Token: 0x04000307 RID: 775
		private string _objectivesDescription;

		// Token: 0x04000308 RID: 776
		private string _troopsDescription;

		// Token: 0x04000309 RID: 777
		private string _name;

		// Token: 0x0400030A RID: 778
		private string _description;

		// Token: 0x0400030B RID: 779
		private bool _isEnabled;

		// Token: 0x0400030C RID: 780
		private MBBindingList<StringPairItemVM> _extraInfos;
	}
}
