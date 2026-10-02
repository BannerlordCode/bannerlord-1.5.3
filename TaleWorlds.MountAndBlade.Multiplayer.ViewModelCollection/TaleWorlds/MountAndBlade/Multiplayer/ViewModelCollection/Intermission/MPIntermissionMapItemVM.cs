using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Intermission
{
	// Token: 0x02000090 RID: 144
	public class MPIntermissionMapItemVM : ViewModel
	{
		// Token: 0x06000DD1 RID: 3537 RVA: 0x0002A770 File Offset: 0x00028970
		public MPIntermissionMapItemVM(string mapID, Action<MPIntermissionMapItemVM> onPlayerVoted)
		{
			this.MapID = mapID;
			this._onPlayerVoted = onPlayerVoted;
			this.RefreshValues();
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0002A78C File Offset: 0x0002898C
		public override void RefreshValues()
		{
			TextObject textObject;
			if (GameTexts.TryGetText("str_multiplayer_scene_name", out textObject, this.MapID))
			{
				this.MapName = textObject.ToString();
				return;
			}
			this.MapName = this.MapID;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0002A7C6 File Offset: 0x000289C6
		public void ExecuteVote()
		{
			this._onPlayerVoted(this);
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x0002A7D4 File Offset: 0x000289D4
		// (set) Token: 0x06000DD5 RID: 3541 RVA: 0x0002A7DC File Offset: 0x000289DC
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x0002A7FA File Offset: 0x000289FA
		// (set) Token: 0x06000DD7 RID: 3543 RVA: 0x0002A802 File Offset: 0x00028A02
		[DataSourceProperty]
		public string MapID
		{
			get
			{
				return this._mapID;
			}
			set
			{
				if (value != this._mapID)
				{
					this._mapID = value;
					base.OnPropertyChangedWithValue<string>(value, "MapID");
				}
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x0002A825 File Offset: 0x00028A25
		// (set) Token: 0x06000DD9 RID: 3545 RVA: 0x0002A82D File Offset: 0x00028A2D
		[DataSourceProperty]
		public string MapName
		{
			get
			{
				return this._mapName;
			}
			set
			{
				if (value != this._mapName)
				{
					this._mapName = value;
					base.OnPropertyChangedWithValue<string>(value, "MapName");
				}
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x0002A850 File Offset: 0x00028A50
		// (set) Token: 0x06000DDB RID: 3547 RVA: 0x0002A858 File Offset: 0x00028A58
		[DataSourceProperty]
		public int Votes
		{
			get
			{
				return this._votes;
			}
			set
			{
				if (value != this._votes)
				{
					this._votes = value;
					base.OnPropertyChangedWithValue(value, "Votes");
				}
			}
		}

		// Token: 0x04000647 RID: 1607
		private readonly Action<MPIntermissionMapItemVM> _onPlayerVoted;

		// Token: 0x04000648 RID: 1608
		private bool _isSelected;

		// Token: 0x04000649 RID: 1609
		private string _mapID;

		// Token: 0x0400064A RID: 1610
		private string _mapName;

		// Token: 0x0400064B RID: 1611
		private int _votes;
	}
}
