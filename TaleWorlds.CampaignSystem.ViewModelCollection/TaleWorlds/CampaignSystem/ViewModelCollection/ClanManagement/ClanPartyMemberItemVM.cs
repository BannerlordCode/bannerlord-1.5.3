using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000130 RID: 304
	public class ClanPartyMemberItemVM : ViewModel
	{
		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06001C54 RID: 7252 RVA: 0x000677B8 File Offset: 0x000659B8
		// (set) Token: 0x06001C55 RID: 7253 RVA: 0x000677C0 File Offset: 0x000659C0
		public Hero HeroObject { get; private set; }

		// Token: 0x06001C56 RID: 7254 RVA: 0x000677CC File Offset: 0x000659CC
		public ClanPartyMemberItemVM(Hero hero, MobileParty party)
		{
			this.HeroObject = hero;
			this.IsLeader = party == null || hero == party.LeaderHero;
			CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(hero.CharacterObject, false);
			this.Visual = new CharacterImageIdentifierVM(characterCode);
			this.HeroModel = new HeroViewModel(CharacterViewModel.StanceTypes.None);
			this.HeroModel.FillFrom(this.HeroObject, -1, false, false);
			this.RefreshValues();
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x0006783A File Offset: 0x00065A3A
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.HeroObject.Name.ToString();
			this.UpdateProperties();
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x0006785E File Offset: 0x00065A5E
		private void ExecuteLocationLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x00067870 File Offset: 0x00065A70
		public void UpdateProperties()
		{
			this.HeroModel = new HeroViewModel(CharacterViewModel.StanceTypes.None);
			this.HeroModel.FillFrom(this.HeroObject, -1, false, false);
			this.Banner_9 = new BannerImageIdentifierVM(this.HeroObject.ClanBanner, true);
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x000678A9 File Offset: 0x00065AA9
		public void ExecuteLink()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this.HeroObject.EncyclopediaLink);
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x000678C5 File Offset: 0x00065AC5
		public virtual void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(typeof(Hero), new object[] { this.HeroObject, true });
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x000678EE File Offset: 0x00065AEE
		public virtual void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x000678F5 File Offset: 0x00065AF5
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.HeroModel.OnFinalize();
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06001C5E RID: 7262 RVA: 0x00067908 File Offset: 0x00065B08
		// (set) Token: 0x06001C5F RID: 7263 RVA: 0x00067910 File Offset: 0x00065B10
		[DataSourceProperty]
		public HeroViewModel HeroModel
		{
			get
			{
				return this._heroModel;
			}
			set
			{
				if (value != this._heroModel)
				{
					this._heroModel = value;
					base.OnPropertyChangedWithValue<HeroViewModel>(value, "HeroModel");
				}
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06001C60 RID: 7264 RVA: 0x0006792E File Offset: 0x00065B2E
		// (set) Token: 0x06001C61 RID: 7265 RVA: 0x00067936 File Offset: 0x00065B36
		[DataSourceProperty]
		public CharacterImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06001C62 RID: 7266 RVA: 0x00067954 File Offset: 0x00065B54
		// (set) Token: 0x06001C63 RID: 7267 RVA: 0x0006795C File Offset: 0x00065B5C
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner_9
		{
			get
			{
				return this._banner_9;
			}
			set
			{
				if (value != this._banner_9)
				{
					this._banner_9 = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner_9");
				}
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06001C64 RID: 7268 RVA: 0x0006797A File Offset: 0x00065B7A
		// (set) Token: 0x06001C65 RID: 7269 RVA: 0x00067982 File Offset: 0x00065B82
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

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x06001C66 RID: 7270 RVA: 0x000679A5 File Offset: 0x00065BA5
		// (set) Token: 0x06001C67 RID: 7271 RVA: 0x000679AD File Offset: 0x00065BAD
		[DataSourceProperty]
		public bool IsLeader
		{
			get
			{
				return this._isLeader;
			}
			set
			{
				if (value != this._isLeader)
				{
					this._isLeader = value;
					base.OnPropertyChangedWithValue(value, "IsLeader");
				}
			}
		}

		// Token: 0x04000CF5 RID: 3317
		private CharacterImageIdentifierVM _visual;

		// Token: 0x04000CF6 RID: 3318
		private BannerImageIdentifierVM _banner_9;

		// Token: 0x04000CF7 RID: 3319
		private string _name;

		// Token: 0x04000CF8 RID: 3320
		private bool _isLeader;

		// Token: 0x04000CF9 RID: 3321
		private HeroViewModel _heroModel;
	}
}
