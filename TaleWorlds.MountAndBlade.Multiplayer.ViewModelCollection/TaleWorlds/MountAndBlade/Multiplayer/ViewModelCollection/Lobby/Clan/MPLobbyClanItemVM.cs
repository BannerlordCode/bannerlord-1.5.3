using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x0200006D RID: 109
	public class MPLobbyClanItemVM : ViewModel
	{
		// Token: 0x06000AAC RID: 2732 RVA: 0x00020DF4 File Offset: 0x0001EFF4
		public MPLobbyClanItemVM(string name, string tag, string sigilCode, int gamesWon, int gamesLost, int ranking, bool isOwnClan)
		{
			this._name = name;
			this._tag = tag;
			this._sigilCode = sigilCode;
			this.GamesWon = gamesWon;
			this.GamesLost = gamesLost;
			this.Ranking = ranking;
			this.IsOwnClan = isOwnClan;
			this.RefreshValues();
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00020E44 File Offset: 0x0001F044
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SigilImage = new BannerImageIdentifierVM(new Banner(this._sigilCode), false);
			GameTexts.SetVariable("STR", this._tag);
			string text = new TextObject("{=uTXYEAOg}[{STR}]", null).ToString();
			GameTexts.SetVariable("STR1", this._name);
			GameTexts.SetVariable("STR2", text);
			this.NameWithTag = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x00020EC0 File Offset: 0x0001F0C0
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x00020EC8 File Offset: 0x0001F0C8
		[DataSourceProperty]
		public string NameWithTag
		{
			get
			{
				return this._nameWithTag;
			}
			set
			{
				if (value != this._nameWithTag)
				{
					this._nameWithTag = value;
					base.OnPropertyChangedWithValue<string>(value, "NameWithTag");
				}
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x00020EEB File Offset: 0x0001F0EB
		// (set) Token: 0x06000AB1 RID: 2737 RVA: 0x00020EF3 File Offset: 0x0001F0F3
		[DataSourceProperty]
		public int MemberCount
		{
			get
			{
				return this._memberCount;
			}
			set
			{
				if (value != this._memberCount)
				{
					this._memberCount = value;
					base.OnPropertyChangedWithValue(value, "MemberCount");
				}
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x00020F11 File Offset: 0x0001F111
		// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x00020F19 File Offset: 0x0001F119
		[DataSourceProperty]
		public int GamesWon
		{
			get
			{
				return this._gamesWon;
			}
			set
			{
				if (value != this._gamesWon)
				{
					this._gamesWon = value;
					base.OnPropertyChangedWithValue(value, "GamesWon");
				}
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x00020F37 File Offset: 0x0001F137
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x00020F3F File Offset: 0x0001F13F
		[DataSourceProperty]
		public int GamesLost
		{
			get
			{
				return this._gamesLost;
			}
			set
			{
				if (value != this._gamesLost)
				{
					this._gamesLost = value;
					base.OnPropertyChangedWithValue(value, "GamesLost");
				}
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00020F5D File Offset: 0x0001F15D
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x00020F65 File Offset: 0x0001F165
		[DataSourceProperty]
		public int Ranking
		{
			get
			{
				return this._ranking;
			}
			set
			{
				if (value != this._ranking)
				{
					this._ranking = value;
					base.OnPropertyChangedWithValue(value, "Ranking");
				}
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00020F83 File Offset: 0x0001F183
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x00020F8B File Offset: 0x0001F18B
		[DataSourceProperty]
		public bool IsOwnClan
		{
			get
			{
				return this._isOwnClan;
			}
			set
			{
				if (value != this._isOwnClan)
				{
					this._isOwnClan = value;
					base.OnPropertyChangedWithValue(value, "IsOwnClan");
				}
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x00020FA9 File Offset: 0x0001F1A9
		// (set) Token: 0x06000ABB RID: 2747 RVA: 0x00020FB1 File Offset: 0x0001F1B1
		[DataSourceProperty]
		public BannerImageIdentifierVM SigilImage
		{
			get
			{
				return this._sigilImage;
			}
			set
			{
				if (value != this._sigilImage)
				{
					this._sigilImage = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "SigilImage");
				}
			}
		}

		// Token: 0x040004D8 RID: 1240
		private string _name;

		// Token: 0x040004D9 RID: 1241
		private string _tag;

		// Token: 0x040004DA RID: 1242
		private string _sigilCode;

		// Token: 0x040004DB RID: 1243
		private string _nameWithTag;

		// Token: 0x040004DC RID: 1244
		private int _memberCount;

		// Token: 0x040004DD RID: 1245
		private int _gamesWon;

		// Token: 0x040004DE RID: 1246
		private int _gamesLost;

		// Token: 0x040004DF RID: 1247
		private int _ranking;

		// Token: 0x040004E0 RID: 1248
		private bool _isOwnClan;

		// Token: 0x040004E1 RID: 1249
		private BannerImageIdentifierVM _sigilImage;
	}
}
