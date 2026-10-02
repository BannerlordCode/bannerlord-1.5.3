using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x0200005B RID: 91
	public class MPLobbyPlayerStatItemVM : ViewModel
	{
		// Token: 0x0600089C RID: 2204 RVA: 0x0001BB11 File Offset: 0x00019D11
		public MPLobbyPlayerStatItemVM(string gameMode, TextObject description, string value)
		{
			this.GameMode = gameMode;
			this._descriptionText = description;
			this.Value = value;
			this.RefreshValues();
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0001BB34 File Offset: 0x00019D34
		public MPLobbyPlayerStatItemVM(string gameMode, TextObject description, float value)
			: this(gameMode, description, value.ToString("0.00"))
		{
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001BB4A File Offset: 0x00019D4A
		public MPLobbyPlayerStatItemVM(string gameMode, TextObject description, int value)
			: this(gameMode, description, value.ToString())
		{
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0001BB5B File Offset: 0x00019D5B
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject descriptionText = this._descriptionText;
			this.Description = ((descriptionText != null) ? descriptionText.ToString() : null) ?? "";
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x0001BB84 File Offset: 0x00019D84
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x0001BB8C File Offset: 0x00019D8C
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

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x0001BBAF File Offset: 0x00019DAF
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x0001BBB7 File Offset: 0x00019DB7
		[DataSourceProperty]
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue<string>(value, "Value");
				}
			}
		}

		// Token: 0x040003FE RID: 1022
		public readonly string GameMode;

		// Token: 0x040003FF RID: 1023
		private readonly TextObject _descriptionText;

		// Token: 0x04000400 RID: 1024
		private string _description;

		// Token: 0x04000401 RID: 1025
		private string _value;
	}
}
