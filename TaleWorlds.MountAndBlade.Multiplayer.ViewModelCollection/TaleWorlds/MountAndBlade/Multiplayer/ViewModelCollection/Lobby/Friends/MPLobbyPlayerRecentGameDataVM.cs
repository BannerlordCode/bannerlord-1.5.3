using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x0200005A RID: 90
	public class MPLobbyPlayerRecentGameDataVM : ViewModel
	{
		// Token: 0x06000893 RID: 2195 RVA: 0x0001BA45 File Offset: 0x00019C45
		public MPLobbyPlayerRecentGameDataVM(int result, string gameType, string map, string date)
		{
			this.Result = result;
			this.GameType = gameType;
			this.Map = map;
			this.Date = date;
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x0001BA6A File Offset: 0x00019C6A
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x0001BA72 File Offset: 0x00019C72
		[DataSourceProperty]
		public int Result
		{
			get
			{
				return this._result;
			}
			set
			{
				if (value != this._result)
				{
					this._result = value;
					base.OnPropertyChangedWithValue(value, "Result");
				}
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x0001BA90 File Offset: 0x00019C90
		// (set) Token: 0x06000897 RID: 2199 RVA: 0x0001BA98 File Offset: 0x00019C98
		[DataSourceProperty]
		public string GameType
		{
			get
			{
				return this._gameType;
			}
			set
			{
				if (value != this._gameType)
				{
					this._gameType = value;
					base.OnPropertyChangedWithValue<string>(value, "GameType");
				}
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x0001BABB File Offset: 0x00019CBB
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x0001BAC3 File Offset: 0x00019CC3
		[DataSourceProperty]
		public string Map
		{
			get
			{
				return this._map;
			}
			set
			{
				if (value != this._map)
				{
					this._map = value;
					base.OnPropertyChangedWithValue<string>(value, "Map");
				}
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x0001BAE6 File Offset: 0x00019CE6
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x0001BAEE File Offset: 0x00019CEE
		[DataSourceProperty]
		public string Date
		{
			get
			{
				return this._date;
			}
			set
			{
				if (value != this._date)
				{
					this._date = value;
					base.OnPropertyChangedWithValue<string>(value, "Date");
				}
			}
		}

		// Token: 0x040003FA RID: 1018
		private int _result;

		// Token: 0x040003FB RID: 1019
		private string _gameType;

		// Token: 0x040003FC RID: 1020
		private string _map;

		// Token: 0x040003FD RID: 1021
		private string _date;
	}
}
