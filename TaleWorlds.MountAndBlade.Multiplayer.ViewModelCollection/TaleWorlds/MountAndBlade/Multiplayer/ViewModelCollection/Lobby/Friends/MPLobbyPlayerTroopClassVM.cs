using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x0200005C RID: 92
	public class MPLobbyPlayerTroopClassVM : ViewModel
	{
		// Token: 0x060008A4 RID: 2212 RVA: 0x0001BBDA File Offset: 0x00019DDA
		public MPLobbyPlayerTroopClassVM()
		{
			this.Name = "Varangian Guard";
			this.Preview = new CharacterImageIdentifierVM(null);
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x0001BBF9 File Offset: 0x00019DF9
		// (set) Token: 0x060008A6 RID: 2214 RVA: 0x0001BC01 File Offset: 0x00019E01
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

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0001BC24 File Offset: 0x00019E24
		// (set) Token: 0x060008A8 RID: 2216 RVA: 0x0001BC2C File Offset: 0x00019E2C
		[DataSourceProperty]
		public CharacterImageIdentifierVM Preview
		{
			get
			{
				return this._preview;
			}
			set
			{
				if (value != this._preview)
				{
					this._preview = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Preview");
				}
			}
		}

		// Token: 0x04000402 RID: 1026
		private string _name;

		// Token: 0x04000403 RID: 1027
		private CharacterImageIdentifierVM _preview;
	}
}
