using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party.PartyTroopManagerPopUp
{
	// Token: 0x02000033 RID: 51
	public class PartyTroopManagerItemVM : ViewModel
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x0001C9C2 File Offset: 0x0001ABC2
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x0001C9CA File Offset: 0x0001ABCA
		public Action<PartyTroopManagerItemVM> SetFocused { get; private set; }

		// Token: 0x06000507 RID: 1287 RVA: 0x0001C9D3 File Offset: 0x0001ABD3
		public PartyTroopManagerItemVM(PartyCharacterVM baseTroop, Action<PartyTroopManagerItemVM> setFocused)
		{
			this.PartyCharacter = baseTroop;
			this.SetFocused = setFocused;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001C9E9 File Offset: 0x0001ABE9
		public void ExecuteSetFocused()
		{
			if (this.PartyCharacter.Character != null)
			{
				Action<PartyTroopManagerItemVM> setFocused = this.SetFocused;
				if (setFocused != null)
				{
					setFocused(this);
				}
				this.IsFocused = true;
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001CA11 File Offset: 0x0001AC11
		public void ExecuteSetUnfocused()
		{
			Action<PartyTroopManagerItemVM> setFocused = this.SetFocused;
			if (setFocused != null)
			{
				setFocused(null);
			}
			this.IsFocused = false;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001CA2C File Offset: 0x0001AC2C
		public void ExecuteOpenTroopEncyclopedia()
		{
			this.PartyCharacter.ExecuteOpenTroopEncyclopedia();
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x0001CA39 File Offset: 0x0001AC39
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x0001CA41 File Offset: 0x0001AC41
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
				}
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x0001CA5F File Offset: 0x0001AC5F
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x0001CA67 File Offset: 0x0001AC67
		[DataSourceProperty]
		public PartyCharacterVM PartyCharacter
		{
			get
			{
				return this._partyCharacter;
			}
			set
			{
				if (value != this._partyCharacter)
				{
					this._partyCharacter = value;
					base.OnPropertyChangedWithValue<PartyCharacterVM>(value, "PartyCharacter");
				}
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x0001CA85 File Offset: 0x0001AC85
		// (set) Token: 0x06000510 RID: 1296 RVA: 0x0001CA92 File Offset: 0x0001AC92
		[DataSourceProperty]
		public bool IsTroopUpgradable
		{
			get
			{
				return this.PartyCharacter.IsTroopUpgradable;
			}
			set
			{
				if (value != this.PartyCharacter.IsTroopUpgradable)
				{
					this.PartyCharacter.IsTroopUpgradable = value;
					base.OnPropertyChangedWithValue(value, "IsTroopUpgradable");
				}
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0001CABA File Offset: 0x0001ACBA
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x0001CAC7 File Offset: 0x0001ACC7
		[DataSourceProperty]
		public bool IsTroopRecruitable
		{
			get
			{
				return this.PartyCharacter.IsTroopRecruitable;
			}
			set
			{
				if (value != this.PartyCharacter.IsTroopRecruitable)
				{
					this.PartyCharacter.IsTroopRecruitable = value;
					base.OnPropertyChangedWithValue(value, "IsTroopRecruitable");
				}
			}
		}

		// Token: 0x0400022B RID: 555
		private bool _isFocused;

		// Token: 0x0400022C RID: 556
		private PartyCharacterVM _partyCharacter;
	}
}
