using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory
{
	// Token: 0x0200007C RID: 124
	public class MPArmoryHeroPreviewVM : ViewModel
	{
		// Token: 0x06000C6A RID: 3178 RVA: 0x000266C0 File Offset: 0x000248C0
		public MPArmoryHeroPreviewVM()
		{
			this.HeroVisual = new CharacterViewModel();
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x000266D3 File Offset: 0x000248D3
		public override void RefreshValues()
		{
			base.RefreshValues();
			BasicCharacterObject character = this._character;
			this.ClassName = ((character != null) ? character.Name.ToString() : null) ?? "";
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00026704 File Offset: 0x00024904
		public void SetCharacter(BasicCharacterObject character, DynamicBodyProperties dynamicBodyProperties, int race, bool isFemale)
		{
			this._character = character;
			this.HeroVisual.FillFrom(character, -1, null);
			this.HeroVisual.BodyProperties = new BodyProperties(dynamicBodyProperties, character.BodyPropertyRange.BodyPropertyMin.StaticProperties).ToString();
			this.HeroVisual.IsFemale = isFemale;
			this.HeroVisual.Race = race;
			this.ClassName = character.Name.ToString();
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00026784 File Offset: 0x00024984
		public void SetCharacterClass(BasicCharacterObject classCharacter)
		{
			this._character = classCharacter;
			this._orgEquipmentWithoutPerks = classCharacter.Equipment;
			this.HeroVisual.SetEquipment(this._orgEquipmentWithoutPerks);
			this.HeroVisual.ArmorColor1 = classCharacter.Culture.Color;
			this.HeroVisual.ArmorColor2 = classCharacter.Culture.Color2;
			if (NetworkMain.GameClient.PlayerData != null)
			{
				string text = NetworkMain.GameClient.PlayerData.Sigil;
				if (NetworkMain.GameClient.PlayerData.IsUsingClanSigil && NetworkMain.GameClient.ClanInfo != null)
				{
					text = NetworkMain.GameClient.ClanInfo.Sigil;
				}
				Banner banner = new Banner(text, classCharacter.Culture.BackgroundColor1, classCharacter.Culture.ForegroundColor1);
				this.HeroVisual.BannerCodeText = banner.BannerCode;
			}
			this.ClassName = classCharacter.Name.ToString();
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x0002686C File Offset: 0x00024A6C
		public void SetCharacterPerks(List<IReadOnlyPerkObject> selectedPerks)
		{
			Equipment equipment = this._orgEquipmentWithoutPerks.Clone(false);
			MPArmoryVM.ApplyPerkEffectsToEquipment(ref equipment, selectedPerks);
			this.HeroVisual.SetEquipment(equipment);
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0002689A File Offset: 0x00024A9A
		// (set) Token: 0x06000C70 RID: 3184 RVA: 0x000268A2 File Offset: 0x00024AA2
		[DataSourceProperty]
		public CharacterViewModel HeroVisual
		{
			get
			{
				return this._heroVisual;
			}
			set
			{
				if (value != this._heroVisual)
				{
					this._heroVisual = value;
					base.OnPropertyChangedWithValue<CharacterViewModel>(value, "HeroVisual");
				}
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x000268C0 File Offset: 0x00024AC0
		// (set) Token: 0x06000C72 RID: 3186 RVA: 0x000268C8 File Offset: 0x00024AC8
		[DataSourceProperty]
		public string ClassName
		{
			get
			{
				return this._className;
			}
			set
			{
				if (value != this._className)
				{
					this._className = value;
					base.OnPropertyChangedWithValue<string>(value, "ClassName");
				}
			}
		}

		// Token: 0x040005A2 RID: 1442
		private BasicCharacterObject _character;

		// Token: 0x040005A3 RID: 1443
		private Equipment _orgEquipmentWithoutPerks;

		// Token: 0x040005A4 RID: 1444
		private CharacterViewModel _heroVisual;

		// Token: 0x040005A5 RID: 1445
		private string _className;
	}
}
