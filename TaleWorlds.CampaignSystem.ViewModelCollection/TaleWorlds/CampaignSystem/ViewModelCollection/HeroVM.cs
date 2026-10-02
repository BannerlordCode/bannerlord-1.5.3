using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection
{
	// Token: 0x0200001B RID: 27
	public class HeroVM : ViewModel
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000C593 File Offset: 0x0000A793
		public Hero Hero { get; }

		// Token: 0x0600019F RID: 415 RVA: 0x0000C59C File Offset: 0x0000A79C
		public HeroVM(Hero hero, bool useCivilian = false)
		{
			if (hero != null)
			{
				CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(hero.CharacterObject, useCivilian);
				this.ImageIdentifier = new CharacterImageIdentifierVM(characterCode);
				this.ClanBanner = new BannerImageIdentifierVM(hero.ClanBanner, false);
				this.ClanBanner_9 = new BannerImageIdentifierVM(hero.ClanBanner, true);
				this.Relation = HeroVM.GetRelation(hero);
				this.IsDead = !hero.IsAlive;
				TextObject textObject;
				this.IsChild = !CampaignUIHelper.IsHeroInformationHidden(hero, out textObject) && FaceGen.GetMaturityTypeWithAge(hero.Age) <= BodyMeshMaturityType.Child;
				this.IsKingdomLeader = hero.IsKingdomLeader;
			}
			else
			{
				this.ImageIdentifier = new CharacterImageIdentifierVM(null);
				this.ClanBanner = new BannerImageIdentifierVM(null, false);
				this.ClanBanner_9 = new BannerImageIdentifierVM(null, false);
				this.Relation = 0;
			}
			this.Hero = hero;
			this.RefreshValues();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000C688 File Offset: 0x0000A888
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.Hero != null)
			{
				this.NameText = this.Hero.Name.ToString();
				return;
			}
			this.NameText = "";
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000C6BA File Offset: 0x0000A8BA
		public void ExecuteLink()
		{
			if (this.Hero != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Hero.EncyclopediaLink);
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000C6DE File Offset: 0x0000A8DE
		public virtual void ExecuteBeginHint()
		{
			if (this.Hero != null)
			{
				InformationManager.ShowTooltip(typeof(Hero), new object[] { this.Hero, false });
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000C70F File Offset: 0x0000A90F
		public virtual void ExecuteEndHint()
		{
			if (this.Hero != null)
			{
				MBInformationManager.HideInformations();
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000C71E File Offset: 0x0000A91E
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x0000C726 File Offset: 0x0000A926
		[DataSourceProperty]
		public bool IsDead
		{
			get
			{
				return this._isDead;
			}
			set
			{
				if (value != this._isDead)
				{
					this._isDead = value;
					base.OnPropertyChangedWithValue(value, "IsDead");
				}
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000C744 File Offset: 0x0000A944
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x0000C74C File Offset: 0x0000A94C
		[DataSourceProperty]
		public bool IsChild
		{
			get
			{
				return this._isChild;
			}
			set
			{
				if (value != this._isChild)
				{
					this._isChild = value;
					base.OnPropertyChangedWithValue(value, "IsChild");
				}
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000C76A File Offset: 0x0000A96A
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x0000C772 File Offset: 0x0000A972
		[DataSourceProperty]
		public bool IsKingdomLeader
		{
			get
			{
				return this._isKingdomLeader;
			}
			set
			{
				if (value != this._isKingdomLeader)
				{
					this._isKingdomLeader = value;
					base.OnPropertyChangedWithValue(value, "IsKingdomLeader");
				}
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000C790 File Offset: 0x0000A990
		// (set) Token: 0x060001AB RID: 427 RVA: 0x0000C798 File Offset: 0x0000A998
		[DataSourceProperty]
		public int Relation
		{
			get
			{
				return this._relation;
			}
			set
			{
				if (value != this._relation)
				{
					this._relation = value;
					base.OnPropertyChangedWithValue(value, "Relation");
				}
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000C7B6 File Offset: 0x0000A9B6
		// (set) Token: 0x060001AD RID: 429 RVA: 0x0000C7BE File Offset: 0x0000A9BE
		[DataSourceProperty]
		public CharacterImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001AE RID: 430 RVA: 0x0000C7DC File Offset: 0x0000A9DC
		// (set) Token: 0x060001AF RID: 431 RVA: 0x0000C7E4 File Offset: 0x0000A9E4
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000C807 File Offset: 0x0000AA07
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x0000C80F File Offset: 0x0000AA0F
		[DataSourceProperty]
		public BannerImageIdentifierVM ClanBanner
		{
			get
			{
				return this._clanBanner;
			}
			set
			{
				if (value != this._clanBanner)
				{
					this._clanBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ClanBanner");
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000C82D File Offset: 0x0000AA2D
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x0000C835 File Offset: 0x0000AA35
		[DataSourceProperty]
		public BannerImageIdentifierVM ClanBanner_9
		{
			get
			{
				return this._clanBanner_9;
			}
			set
			{
				if (value != this._clanBanner_9)
				{
					this._clanBanner_9 = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ClanBanner_9");
				}
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000C853 File Offset: 0x0000AA53
		public static int GetRelation(Hero hero)
		{
			if (hero == null)
			{
				return -101;
			}
			if (hero == Hero.MainHero)
			{
				return 101;
			}
			if (ViewModel.UIDebugMode)
			{
				return MBRandom.RandomInt(-100, 100);
			}
			return Hero.MainHero.GetRelation(hero);
		}

		// Token: 0x040000C4 RID: 196
		private CharacterImageIdentifierVM _imageIdentifier;

		// Token: 0x040000C5 RID: 197
		private BannerImageIdentifierVM _clanBanner;

		// Token: 0x040000C6 RID: 198
		private BannerImageIdentifierVM _clanBanner_9;

		// Token: 0x040000C7 RID: 199
		private string _nameText;

		// Token: 0x040000C8 RID: 200
		private int _relation = -102;

		// Token: 0x040000C9 RID: 201
		private bool _isDead = true;

		// Token: 0x040000CA RID: 202
		private bool _isChild;

		// Token: 0x040000CB RID: 203
		private bool _isKingdomLeader;
	}
}
