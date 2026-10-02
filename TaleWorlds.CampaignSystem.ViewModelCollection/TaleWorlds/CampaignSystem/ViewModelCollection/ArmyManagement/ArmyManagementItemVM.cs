using System;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement
{
	// Token: 0x02000162 RID: 354
	public class ArmyManagementItemVM : ViewModel
	{
		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06002242 RID: 8770 RVA: 0x0007A3CB File Offset: 0x000785CB
		public float DistInTime { get; }

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06002243 RID: 8771 RVA: 0x0007A3D3 File Offset: 0x000785D3
		public float _distance { get; }

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06002244 RID: 8772 RVA: 0x0007A3DB File Offset: 0x000785DB
		public Clan Clan { get; }

		// Token: 0x06002245 RID: 8773 RVA: 0x0007A3E4 File Offset: 0x000785E4
		public ArmyManagementItemVM(Action<ArmyManagementItemVM> onAddToCart, Action<ArmyManagementItemVM> onRemove, Action<ArmyManagementItemVM> onFocus, MobileParty mobileParty)
		{
			ArmyManagementCalculationModel armyManagementCalculationModel = Campaign.Current.Models.ArmyManagementCalculationModel;
			this._onAddToCart = onAddToCart;
			this._onRemove = onRemove;
			this._onFocus = onFocus;
			this.Party = mobileParty;
			this._eligibilityReason = TextObject.GetEmpty();
			this.ClanBanner = new BannerImageIdentifierVM(mobileParty.LeaderHero.ClanBanner, true);
			CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(mobileParty.LeaderHero.CharacterObject, false);
			this.LordFace = new CharacterImageIdentifierVM(characterCode);
			this.Relation = armyManagementCalculationModel.GetPartyRelation(mobileParty.LeaderHero);
			this.Strength = this.Party.Party.NumberOfHealthyMembers;
			this.ShipCount = this.Party.Ships.Count;
			this._distance = DistanceHelper.FindClosestDistanceFromMobilePartyToMobileParty(this.Party, MobileParty.MainParty, this.Party.NavigationCapability);
			if (MobileParty.MainParty.IsCurrentlyAtSea && !this.Party.HasNavalNavigationCapability)
			{
				this.DistInTime = 2.1474836E+09f;
			}
			else
			{
				this.DistInTime = (float)MathF.Ceiling(this._distance / this.Party.Speed);
				this.Cost = armyManagementCalculationModel.CalculatePartyInfluenceCost(MobileParty.MainParty, mobileParty);
			}
			this.Clan = mobileParty.LeaderHero.Clan;
			this.IsMainHero = mobileParty.IsMainParty;
			this.UpdateEligibility();
			this.IsTransferDisabled = this.IsMainHero || PlayerSiege.PlayerSiegeEvent != null;
			this.RefreshValues();
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x0007A580 File Offset: 0x00078780
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.InArmyText = GameTexts.FindText("str_in_army", null).ToString();
			this.LeaderNameText = this.Party.LeaderHero.Name.ToString();
			this.NameText = this.Party.Name.ToString();
			if (!this.Party.IsMainParty)
			{
				this.DistanceText = (((int)this._distance < 5) ? GameTexts.FindText("str_nearby", null).ToString() : CampaignUIHelper.GetPartyDistanceByTimeTextAbbreviated((float)((int)this._distance), this.Party.Speed));
			}
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x0007A621 File Offset: 0x00078821
		public void ExecuteAction()
		{
			if (this.IsInCart)
			{
				this.OnRemove();
				return;
			}
			this.OnAddToCart();
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x0007A638 File Offset: 0x00078838
		private void OnRemove()
		{
			if (!this.IsMainHero)
			{
				this._onRemove(this);
				this.UpdateEligibility();
			}
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x0007A654 File Offset: 0x00078854
		private void OnAddToCart()
		{
			this.UpdateEligibility();
			if (this.IsEligible)
			{
				this._onAddToCart(this);
			}
			this.UpdateEligibility();
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x0007A676 File Offset: 0x00078876
		public void ExecuteSetFocused()
		{
			this.IsFocused = true;
			Action<ArmyManagementItemVM> onFocus = this._onFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(this);
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x0007A690 File Offset: 0x00078890
		public void ExecuteSetUnfocused()
		{
			this.IsFocused = false;
			Action<ArmyManagementItemVM> onFocus = this._onFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(null);
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x0007A6AC File Offset: 0x000788AC
		public void UpdateEligibility()
		{
			GameModels models = Campaign.Current.Models;
			ArmyManagementCalculationModel armyManagementCalculationModel = ((models != null) ? models.ArmyManagementCalculationModel : null);
			bool flag = true;
			this._eligibilityReason = TextObject.GetEmpty();
			if (!this.CanJoinBackWithoutCost)
			{
				if (this.IsInCart && !this.IsAlreadyWithPlayer)
				{
					flag = false;
					this._eligibilityReason = new TextObject("{=idRXFzQ6}Already added to the army.", null);
				}
				else
				{
					flag = armyManagementCalculationModel.CheckPartyEligibility(this.Party, out this._eligibilityReason);
					if (flag)
					{
						flag = CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out this._eligibilityReason);
					}
				}
			}
			this.IsEligible = flag;
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x0007A733 File Offset: 0x00078933
		private void UpdateIsCostRelevant()
		{
			if (this.Cost == 0 && this.IsAlreadyWithPlayer && this.IsInCart)
			{
				this.IsCostRelevant = false;
				return;
			}
			this.IsCostRelevant = true;
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x0007A75C File Offset: 0x0007895C
		public void ExecuteBeginHint()
		{
			if (!this.IsEligible)
			{
				MBInformationManager.ShowHint(this._eligibilityReason.ToString());
				return;
			}
			InformationManager.ShowTooltip(typeof(MobileParty), new object[] { this.Party, true, true });
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x0007A7B2 File Offset: 0x000789B2
		public void ExecuteBeginClanHint()
		{
			Type typeFromHandle = typeof(Clan);
			object[] array = new object[3];
			int num = 0;
			MobileParty party = this.Party;
			array[num] = ((party != null) ? party.ActualClan : null);
			array[1] = true;
			array[2] = true;
			InformationManager.ShowTooltip(typeFromHandle, array);
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x0007A7F0 File Offset: 0x000789F0
		public void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x0007A7F7 File Offset: 0x000789F7
		public void ExecuteOpenEncyclopedia()
		{
			MobileParty party = this.Party;
			if (((party != null) ? party.LeaderHero : null) != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Party.LeaderHero.EncyclopediaLink);
			}
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x0007A82C File Offset: 0x00078A2C
		public void ExecuteOpenClanEncyclopedia()
		{
			MobileParty party = this.Party;
			if (((party != null) ? party.ActualClan : null) != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Party.ActualClan.EncyclopediaLink);
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06002253 RID: 8787 RVA: 0x0007A861 File Offset: 0x00078A61
		// (set) Token: 0x06002254 RID: 8788 RVA: 0x0007A869 File Offset: 0x00078A69
		[DataSourceProperty]
		public InputKeyItemVM RemoveInputKey
		{
			get
			{
				return this._removeInputKey;
			}
			set
			{
				if (value != this._removeInputKey)
				{
					this._removeInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "RemoveInputKey");
				}
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06002255 RID: 8789 RVA: 0x0007A887 File Offset: 0x00078A87
		// (set) Token: 0x06002256 RID: 8790 RVA: 0x0007A88F File Offset: 0x00078A8F
		[DataSourceProperty]
		public bool IsEligible
		{
			get
			{
				return this._isEligible;
			}
			set
			{
				if (value != this._isEligible)
				{
					this._isEligible = value;
					base.OnPropertyChangedWithValue(value, "IsEligible");
				}
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x0007A8AD File Offset: 0x00078AAD
		// (set) Token: 0x06002258 RID: 8792 RVA: 0x0007A8B5 File Offset: 0x00078AB5
		[DataSourceProperty]
		public bool IsInCart
		{
			get
			{
				return this._isInCart;
			}
			set
			{
				if (value != this._isInCart)
				{
					this._isInCart = value;
					base.OnPropertyChangedWithValue(value, "IsInCart");
					this.UpdateIsCostRelevant();
				}
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06002259 RID: 8793 RVA: 0x0007A8D9 File Offset: 0x00078AD9
		// (set) Token: 0x0600225A RID: 8794 RVA: 0x0007A8E1 File Offset: 0x00078AE1
		[DataSourceProperty]
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (value != this._isMainHero)
				{
					this._isMainHero = value;
					base.OnPropertyChangedWithValue(value, "IsMainHero");
				}
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x0600225B RID: 8795 RVA: 0x0007A8FF File Offset: 0x00078AFF
		// (set) Token: 0x0600225C RID: 8796 RVA: 0x0007A907 File Offset: 0x00078B07
		[DataSourceProperty]
		public int Strength
		{
			get
			{
				return this._strength;
			}
			set
			{
				if (value != this._strength)
				{
					this._strength = value;
					base.OnPropertyChangedWithValue(value, "Strength");
				}
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x0600225D RID: 8797 RVA: 0x0007A925 File Offset: 0x00078B25
		// (set) Token: 0x0600225E RID: 8798 RVA: 0x0007A92D File Offset: 0x00078B2D
		[DataSourceProperty]
		public int ShipCount
		{
			get
			{
				return this._shipCount;
			}
			set
			{
				if (value != this._shipCount)
				{
					this._shipCount = value;
					base.OnPropertyChangedWithValue(value, "ShipCount");
					this.HasShip = this._shipCount > 0;
				}
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x0600225F RID: 8799 RVA: 0x0007A95A File Offset: 0x00078B5A
		// (set) Token: 0x06002260 RID: 8800 RVA: 0x0007A962 File Offset: 0x00078B62
		[DataSourceProperty]
		public bool HasShip
		{
			get
			{
				return this._hasShip;
			}
			set
			{
				if (value != this._hasShip)
				{
					this._hasShip = value;
					base.OnPropertyChangedWithValue(value, "HasShip");
				}
			}
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06002261 RID: 8801 RVA: 0x0007A980 File Offset: 0x00078B80
		// (set) Token: 0x06002262 RID: 8802 RVA: 0x0007A988 File Offset: 0x00078B88
		[DataSourceProperty]
		public string DistanceText
		{
			get
			{
				return this._distanceText;
			}
			set
			{
				if (value != this._distanceText)
				{
					this._distanceText = value;
					base.OnPropertyChangedWithValue<string>(value, "DistanceText");
				}
			}
		}

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06002263 RID: 8803 RVA: 0x0007A9AB File Offset: 0x00078BAB
		// (set) Token: 0x06002264 RID: 8804 RVA: 0x0007A9B3 File Offset: 0x00078BB3
		[DataSourceProperty]
		public string InArmyText
		{
			get
			{
				return this._inArmyText;
			}
			set
			{
				if (value != this._inArmyText)
				{
					this._inArmyText = value;
					base.OnPropertyChangedWithValue<string>(value, "InArmyText");
				}
			}
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06002265 RID: 8805 RVA: 0x0007A9D6 File Offset: 0x00078BD6
		// (set) Token: 0x06002266 RID: 8806 RVA: 0x0007A9DE File Offset: 0x00078BDE
		[DataSourceProperty]
		public int Cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				if (value != this._cost)
				{
					this._cost = value;
					base.OnPropertyChangedWithValue(value, "Cost");
					this.UpdateIsCostRelevant();
				}
			}
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06002267 RID: 8807 RVA: 0x0007AA02 File Offset: 0x00078C02
		// (set) Token: 0x06002268 RID: 8808 RVA: 0x0007AA0A File Offset: 0x00078C0A
		[DataSourceProperty]
		public bool IsCostRelevant
		{
			get
			{
				return this._isCostRelevant;
			}
			set
			{
				if (value != this._isCostRelevant)
				{
					this._isCostRelevant = value;
					base.OnPropertyChangedWithValue(value, "IsCostRelevant");
				}
			}
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06002269 RID: 8809 RVA: 0x0007AA28 File Offset: 0x00078C28
		// (set) Token: 0x0600226A RID: 8810 RVA: 0x0007AA30 File Offset: 0x00078C30
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

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x0600226B RID: 8811 RVA: 0x0007AA4E File Offset: 0x00078C4E
		// (set) Token: 0x0600226C RID: 8812 RVA: 0x0007AA56 File Offset: 0x00078C56
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

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x0600226D RID: 8813 RVA: 0x0007AA74 File Offset: 0x00078C74
		// (set) Token: 0x0600226E RID: 8814 RVA: 0x0007AA7C File Offset: 0x00078C7C
		[DataSourceProperty]
		public CharacterImageIdentifierVM LordFace
		{
			get
			{
				return this._lordFace;
			}
			set
			{
				if (value != this._lordFace)
				{
					this._lordFace = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "LordFace");
				}
			}
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x0600226F RID: 8815 RVA: 0x0007AA9A File Offset: 0x00078C9A
		// (set) Token: 0x06002270 RID: 8816 RVA: 0x0007AAA2 File Offset: 0x00078CA2
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

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06002271 RID: 8817 RVA: 0x0007AAC5 File Offset: 0x00078CC5
		// (set) Token: 0x06002272 RID: 8818 RVA: 0x0007AACD File Offset: 0x00078CCD
		[DataSourceProperty]
		public bool IsAlreadyWithPlayer
		{
			get
			{
				return this._isAlreadyWithPlayer;
			}
			set
			{
				if (value != this._isAlreadyWithPlayer)
				{
					this._isAlreadyWithPlayer = value;
					base.OnPropertyChangedWithValue(value, "IsAlreadyWithPlayer");
					this.UpdateIsCostRelevant();
				}
			}
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06002273 RID: 8819 RVA: 0x0007AAF1 File Offset: 0x00078CF1
		// (set) Token: 0x06002274 RID: 8820 RVA: 0x0007AAF9 File Offset: 0x00078CF9
		[DataSourceProperty]
		public bool IsTransferDisabled
		{
			get
			{
				return this._isTransferDisabled;
			}
			set
			{
				if (value != this._isTransferDisabled)
				{
					this._isTransferDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsTransferDisabled");
				}
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x06002275 RID: 8821 RVA: 0x0007AB17 File Offset: 0x00078D17
		// (set) Token: 0x06002276 RID: 8822 RVA: 0x0007AB1F File Offset: 0x00078D1F
		[DataSourceProperty]
		public string LeaderNameText
		{
			get
			{
				return this._leaderNameText;
			}
			set
			{
				if (value != this._leaderNameText)
				{
					this._leaderNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaderNameText");
				}
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06002277 RID: 8823 RVA: 0x0007AB42 File Offset: 0x00078D42
		// (set) Token: 0x06002278 RID: 8824 RVA: 0x0007AB4A File Offset: 0x00078D4A
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

		// Token: 0x04000FA7 RID: 4007
		private readonly Action<ArmyManagementItemVM> _onAddToCart;

		// Token: 0x04000FA8 RID: 4008
		private readonly Action<ArmyManagementItemVM> _onRemove;

		// Token: 0x04000FA9 RID: 4009
		private readonly Action<ArmyManagementItemVM> _onFocus;

		// Token: 0x04000FAA RID: 4010
		public readonly MobileParty Party;

		// Token: 0x04000FAB RID: 4011
		private const float _minimumPartySizeScoreNeeded = 0.4f;

		// Token: 0x04000FAC RID: 4012
		public bool CanJoinBackWithoutCost;

		// Token: 0x04000FAD RID: 4013
		private TextObject _eligibilityReason;

		// Token: 0x04000FAE RID: 4014
		private InputKeyItemVM _removeInputKey;

		// Token: 0x04000FAF RID: 4015
		private BannerImageIdentifierVM _clanBanner;

		// Token: 0x04000FB0 RID: 4016
		private CharacterImageIdentifierVM _lordFace;

		// Token: 0x04000FB1 RID: 4017
		private string _nameText;

		// Token: 0x04000FB2 RID: 4018
		private string _inArmyText;

		// Token: 0x04000FB3 RID: 4019
		private string _leaderNameText;

		// Token: 0x04000FB4 RID: 4020
		private int _relation = -102;

		// Token: 0x04000FB5 RID: 4021
		private int _strength = -1;

		// Token: 0x04000FB6 RID: 4022
		private int _shipCount = -1;

		// Token: 0x04000FB7 RID: 4023
		private bool _hasShip;

		// Token: 0x04000FB8 RID: 4024
		private string _distanceText;

		// Token: 0x04000FB9 RID: 4025
		private int _cost = -1;

		// Token: 0x04000FBA RID: 4026
		private bool _isCostRelevant;

		// Token: 0x04000FBB RID: 4027
		private bool _isEligible;

		// Token: 0x04000FBC RID: 4028
		private bool _isMainHero;

		// Token: 0x04000FBD RID: 4029
		private bool _isInCart;

		// Token: 0x04000FBE RID: 4030
		private bool _isAlreadyWithPlayer;

		// Token: 0x04000FBF RID: 4031
		private bool _isTransferDisabled;

		// Token: 0x04000FC0 RID: 4032
		private bool _isFocused;
	}
}
