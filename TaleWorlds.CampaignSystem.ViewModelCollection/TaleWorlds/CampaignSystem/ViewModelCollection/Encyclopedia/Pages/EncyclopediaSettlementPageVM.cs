using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000DE RID: 222
	[EncyclopediaViewModel(typeof(Settlement))]
	public class EncyclopediaSettlementPageVM : EncyclopediaContentPageVM
	{
		// Token: 0x060014CC RID: 5324 RVA: 0x00052DF8 File Offset: 0x00050FF8
		public EncyclopediaSettlementPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this._settlement = base.Obj as Settlement;
			this.NotableCharacters = new MBBindingList<HeroVM>();
			this.Settlements = new MBBindingList<EncyclopediaSettlementVM>();
			this.History = new MBBindingList<EncyclopediaHistoryEventVM>();
			this._isVisualTrackerSelected = Campaign.Current.VisualTrackerManager.CheckTracked(this._settlement);
			this.IsFortification = this._settlement.IsFortification;
			this.SettlementImageID = this._settlement.SettlementComponent.WaitMeshName;
			base.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(this._settlement);
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			this.RefreshValues();
			TextObject textObject;
			if (CampaignUIHelper.IsSettlementInformationHidden(this._settlement, out textObject))
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.Settlement, true));
			}
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00052EE8 File Offset: 0x000510E8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SettlementName = this._settlement.Name.ToString();
			this.SettlementsText = GameTexts.FindText("str_villages", null).ToString();
			this.NotableCharactersText = GameTexts.FindText("str_notable_characters", null).ToString();
			this.OwnerText = GameTexts.FindText("str_owner", null).ToString();
			this.TrackText = GameTexts.FindText("str_settlement_track", null).ToString();
			this.ShowInMapHint = new HintViewModel(GameTexts.FindText("str_show_on_map", null), null);
			this.InformationText = this._settlement.EncyclopediaText.ToString();
			base.UpdateBookmarkHintText();
			this.Refresh();
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00052FA4 File Offset: 0x000511A4
		public override void Refresh()
		{
			base.IsLoadingOver = false;
			SettlementComponent settlementComponent = this._settlement.SettlementComponent;
			this.NotableCharacters.Clear();
			this.Settlements.Clear();
			this.History.Clear();
			this.IsFortification = this._settlement.IsFortification;
			if (this._settlement.IsFortification)
			{
				this.SettlementType = 0;
				EncyclopediaPage pageOf = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Settlement));
				using (List<Village>.Enumerator enumerator = this._settlement.BoundVillages.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Village village = enumerator.Current;
						if (pageOf.IsValidEncyclopediaItem(village.Owner.Settlement))
						{
							this.Settlements.Add(new EncyclopediaSettlementVM(village.Owner.Settlement));
						}
					}
					goto IL_00F2;
				}
			}
			if (this._settlement.IsVillage)
			{
				this.SettlementType = 1;
			}
			IL_00F2:
			if (!this._settlement.IsCastle)
			{
				EncyclopediaPage pageOf2 = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero));
				foreach (Hero hero in this._settlement.Notables)
				{
					if (pageOf2.IsValidEncyclopediaItem(hero))
					{
						this.NotableCharacters.Add(new HeroVM(hero, false));
					}
				}
			}
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_enc_sf_culture", null).ToString());
			GameTexts.SetVariable("STR2", this._settlement.Culture.Name.ToString());
			this.CultureText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			this.OwnerText = GameTexts.FindText("str_owner", null).ToString();
			this.Owner = new HeroVM(this._settlement.OwnerClan.Leader, false);
			this.OwnerBanner = new EncyclopediaFactionVM(this._settlement.OwnerClan);
			this.SettlementPath = settlementComponent.BackgroundMeshName;
			this.SettlementCropPosition = (double)settlementComponent.BackgroundCropPosition;
			this.HasBoundSettlement = this._settlement.IsVillage;
			this.BoundSettlement = (this.HasBoundSettlement ? new EncyclopediaSettlementVM(this._settlement.Village.Bound) : null);
			this.BoundSettlementText = "";
			if (this.HasBoundSettlement)
			{
				GameTexts.SetVariable("SETTLEMENT_LINK", this._settlement.Village.Bound.EncyclopediaLinkWithName);
				this.BoundSettlementText = GameTexts.FindText("str_bound_settlement_encyclopedia", null).ToString();
			}
			TextObject textObject;
			bool flag = CampaignUIHelper.IsSettlementInformationHidden(this._settlement, out textObject);
			string text = GameTexts.FindText("str_missing_info_indicator", null).ToString();
			string text2 = (flag ? text : ((int)this._settlement.Militia).ToString());
			if (this._settlement.IsFortification)
			{
				MBBindingList<EncyclopediaSettlementPageStatItemVM> mbbindingList = new MBBindingList<EncyclopediaSettlementPageStatItemVM>();
				mbbindingList.Add(new EncyclopediaSettlementPageStatItemVM(new BasicTooltipViewModel(() => CampaignUIHelper.GetTownWallsTooltip(this._settlement.Town)), EncyclopediaSettlementPageStatItemVM.DescriptionType.Wall, flag ? text : this._settlement.Town.GetWallLevel().ToString()));
				BasicTooltipViewModel basicTooltipViewModel = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownGarrisonTooltip(this._settlement.Town));
				EncyclopediaSettlementPageStatItemVM.DescriptionType descriptionType = EncyclopediaSettlementPageStatItemVM.DescriptionType.Garrison;
				string text3;
				if (!flag)
				{
					MobileParty garrisonParty = this._settlement.Town.GarrisonParty;
					text3 = ((garrisonParty != null) ? garrisonParty.Party.NumberOfAllMembers.ToString() : null);
				}
				else
				{
					text3 = text;
				}
				mbbindingList.Add(new EncyclopediaSettlementPageStatItemVM(basicTooltipViewModel, descriptionType, text3));
				mbbindingList.Add(new EncyclopediaSettlementPageStatItemVM(new BasicTooltipViewModel(() => CampaignUIHelper.GetTownMilitiaTooltip(this._settlement.Town)), EncyclopediaSettlementPageStatItemVM.DescriptionType.Militia, text2));
				mbbindingList.Add(new EncyclopediaSettlementPageStatItemVM(new BasicTooltipViewModel(() => CampaignUIHelper.GetTownFoodTooltip(this._settlement.Town)), EncyclopediaSettlementPageStatItemVM.DescriptionType.Food, flag ? text : ((int)this._settlement.Town.FoodStocks).ToString()));
				this.LeftSideProperties = mbbindingList;
				this.RightSideProperties = new MBBindingList<EncyclopediaSettlementPageStatItemVM>
				{
					new EncyclopediaSettlementPageStatItemVM(new BasicTooltipViewModel(() => CampaignUIHelper.GetTownProsperityTooltip(this._settlement.Town)), EncyclopediaSettlementPageStatItemVM.DescriptionType.Prosperity, flag ? text : ((int)this._settlement.Town.Prosperity).ToString()),
					new EncyclopediaSettlementPageStatItemVM(new BasicTooltipViewModel(() => CampaignUIHelper.GetTownLoyaltyTooltip(this._settlement.Town)), EncyclopediaSettlementPageStatItemVM.DescriptionType.Loyalty, flag ? text : ((int)this._settlement.Town.Loyalty).ToString()),
					new EncyclopediaSettlementPageStatItemVM(new BasicTooltipViewModel(() => CampaignUIHelper.GetTownSecurityTooltip(this._settlement.Town)), EncyclopediaSettlementPageStatItemVM.DescriptionType.Security, flag ? text : ((int)this._settlement.Town.Security).ToString())
				};
			}
			else
			{
				this.LeftSideProperties = new MBBindingList<EncyclopediaSettlementPageStatItemVM>
				{
					new EncyclopediaSettlementPageStatItemVM(new BasicTooltipViewModel(() => CampaignUIHelper.GetVillageMilitiaTooltip(this._settlement.Village)), EncyclopediaSettlementPageStatItemVM.DescriptionType.Militia, text2)
				};
				this.RightSideProperties = new MBBindingList<EncyclopediaSettlementPageStatItemVM>
				{
					new EncyclopediaSettlementPageStatItemVM(new BasicTooltipViewModel(() => CampaignUIHelper.GetVillageProsperityTooltip(this._settlement.Village)), EncyclopediaSettlementPageStatItemVM.DescriptionType.Prosperity, flag ? text : ((int)this._settlement.Village.Hearth).ToString())
				};
			}
			this.NameText = this._settlement.Name.ToString();
			MBObjectBase settlement = this._settlement;
			for (int i = Campaign.Current.LogEntryHistory.GameActionLogs.Count - 1; i >= 0; i--)
			{
				IEncyclopediaLog encyclopediaLog;
				if ((encyclopediaLog = Campaign.Current.LogEntryHistory.GameActionLogs[i] as IEncyclopediaLog) != null && encyclopediaLog.IsVisibleInEncyclopediaPageOf(settlement))
				{
					this.History.Add(new EncyclopediaHistoryEventVM(encyclopediaLog));
				}
			}
			this.IsVisualTrackerSelected = Campaign.Current.VisualTrackerManager.CheckTracked(this._settlement);
			base.IsLoadingOver = true;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0005357C File Offset: 0x0005177C
		public override string GetName()
		{
			return this._settlement.Name.ToString();
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x00053590 File Offset: 0x00051790
		public void ExecuteTrack()
		{
			if (!this.IsVisualTrackerSelected)
			{
				Campaign.Current.VisualTrackerManager.RegisterObject(this._settlement);
				this.IsVisualTrackerSelected = true;
			}
			else
			{
				Campaign.Current.VisualTrackerManager.RemoveTrackedObject(this._settlement, false);
				this.IsVisualTrackerSelected = false;
			}
			Game.Current.EventManager.TriggerEvent<PlayerToggleTrackSettlementFromEncyclopediaEvent>(new PlayerToggleTrackSettlementFromEncyclopediaEvent(this._settlement, this.IsVisualTrackerSelected));
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x00053600 File Offset: 0x00051800
		public override string GetNavigationBarURL()
		{
			return HyperlinkTexts.GetGenericHyperlinkText("Home", GameTexts.FindText("str_encyclopedia_home", null).ToString()) + " \\ " + HyperlinkTexts.GetGenericHyperlinkText("ListPage-Settlements", GameTexts.FindText("str_encyclopedia_settlements", null).ToString()) + " \\ " + this.GetName();
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x00053665 File Offset: 0x00051865
		public void ExecuteBoundSettlementLink()
		{
			if (this.HasBoundSettlement)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._settlement.Village.Bound.EncyclopediaLink);
			}
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x00053694 File Offset: 0x00051894
		public override void ExecuteSwitchBookmarkedState()
		{
			base.ExecuteSwitchBookmarkedState();
			if (base.IsBookmarked)
			{
				Campaign.Current.EncyclopediaManager.ViewDataTracker.AddEncyclopediaBookmarkToItem(this._settlement);
				return;
			}
			Campaign.Current.EncyclopediaManager.ViewDataTracker.RemoveEncyclopediaBookmarkFromItem(this._settlement);
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x000536E4 File Offset: 0x000518E4
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent evnt)
		{
			this.IsTrackerButtonHighlightEnabled = evnt.NewNotificationElementID == "EncyclopediaItemTrackButton";
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x000536FC File Offset: 0x000518FC
		public override void OnFinalize()
		{
			base.OnFinalize();
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x060014D6 RID: 5334 RVA: 0x0005371F File Offset: 0x0005191F
		// (set) Token: 0x060014D7 RID: 5335 RVA: 0x00053727 File Offset: 0x00051927
		[DataSourceProperty]
		public EncyclopediaFactionVM OwnerBanner
		{
			get
			{
				return this._ownerBanner;
			}
			set
			{
				if (value != this._ownerBanner)
				{
					this._ownerBanner = value;
					base.OnPropertyChangedWithValue<EncyclopediaFactionVM>(value, "OwnerBanner");
				}
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x060014D8 RID: 5336 RVA: 0x00053745 File Offset: 0x00051945
		// (set) Token: 0x060014D9 RID: 5337 RVA: 0x0005374D File Offset: 0x0005194D
		[DataSourceProperty]
		public EncyclopediaSettlementVM BoundSettlement
		{
			get
			{
				return this._boundSettlement;
			}
			set
			{
				if (value != this._boundSettlement)
				{
					this._boundSettlement = value;
					base.OnPropertyChangedWithValue<EncyclopediaSettlementVM>(value, "BoundSettlement");
				}
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x060014DA RID: 5338 RVA: 0x0005376B File Offset: 0x0005196B
		// (set) Token: 0x060014DB RID: 5339 RVA: 0x00053773 File Offset: 0x00051973
		[DataSourceProperty]
		public bool IsFortification
		{
			get
			{
				return this._isFortification;
			}
			set
			{
				if (value != this._isFortification)
				{
					this._isFortification = value;
					base.OnPropertyChangedWithValue(value, "IsFortification");
				}
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x060014DC RID: 5340 RVA: 0x00053791 File Offset: 0x00051991
		// (set) Token: 0x060014DD RID: 5341 RVA: 0x00053799 File Offset: 0x00051999
		[DataSourceProperty]
		public bool IsTrackerButtonHighlightEnabled
		{
			get
			{
				return this._isTrackerButtonHighlightEnabled;
			}
			set
			{
				if (value != this._isTrackerButtonHighlightEnabled)
				{
					this._isTrackerButtonHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsTrackerButtonHighlightEnabled");
				}
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x060014DE RID: 5342 RVA: 0x000537B7 File Offset: 0x000519B7
		// (set) Token: 0x060014DF RID: 5343 RVA: 0x000537BF File Offset: 0x000519BF
		[DataSourceProperty]
		public bool HasBoundSettlement
		{
			get
			{
				return this._hasBoundSettlement;
			}
			set
			{
				if (value != this._hasBoundSettlement)
				{
					this._hasBoundSettlement = value;
					base.OnPropertyChangedWithValue(value, "HasBoundSettlement");
				}
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x060014E0 RID: 5344 RVA: 0x000537DD File Offset: 0x000519DD
		// (set) Token: 0x060014E1 RID: 5345 RVA: 0x000537E5 File Offset: 0x000519E5
		[DataSourceProperty]
		public double SettlementCropPosition
		{
			get
			{
				return this._settlementCropPosition;
			}
			set
			{
				if (value != this._settlementCropPosition)
				{
					this._settlementCropPosition = value;
					base.OnPropertyChangedWithValue(value, "SettlementCropPosition");
				}
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x00053803 File Offset: 0x00051A03
		// (set) Token: 0x060014E3 RID: 5347 RVA: 0x0005380B File Offset: 0x00051A0B
		[DataSourceProperty]
		public string BoundSettlementText
		{
			get
			{
				return this._boundSettlementText;
			}
			set
			{
				if (value != this._boundSettlementText)
				{
					this._boundSettlementText = value;
					base.OnPropertyChangedWithValue<string>(value, "BoundSettlementText");
				}
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x060014E4 RID: 5348 RVA: 0x0005382E File Offset: 0x00051A2E
		// (set) Token: 0x060014E5 RID: 5349 RVA: 0x00053836 File Offset: 0x00051A36
		[DataSourceProperty]
		public string TrackText
		{
			get
			{
				return this._trackText;
			}
			set
			{
				if (value != this._trackText)
				{
					this._trackText = value;
					base.OnPropertyChangedWithValue<string>(value, "TrackText");
				}
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060014E6 RID: 5350 RVA: 0x00053859 File Offset: 0x00051A59
		// (set) Token: 0x060014E7 RID: 5351 RVA: 0x00053861 File Offset: 0x00051A61
		[DataSourceProperty]
		public string SettlementPath
		{
			get
			{
				return this._settlementPath;
			}
			set
			{
				if (value != this._settlementPath)
				{
					this._settlementPath = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementPath");
				}
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060014E8 RID: 5352 RVA: 0x00053884 File Offset: 0x00051A84
		// (set) Token: 0x060014E9 RID: 5353 RVA: 0x0005388C File Offset: 0x00051A8C
		[DataSourceProperty]
		public string SettlementName
		{
			get
			{
				return this._settlementName;
			}
			set
			{
				if (value != this._settlementName)
				{
					this._settlementName = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementName");
				}
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x060014EA RID: 5354 RVA: 0x000538AF File Offset: 0x00051AAF
		// (set) Token: 0x060014EB RID: 5355 RVA: 0x000538B7 File Offset: 0x00051AB7
		[DataSourceProperty]
		public string InformationText
		{
			get
			{
				return this._informationText;
			}
			set
			{
				if (value != this._informationText)
				{
					this._informationText = value;
					base.OnPropertyChangedWithValue<string>(value, "InformationText");
				}
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x000538DA File Offset: 0x00051ADA
		// (set) Token: 0x060014ED RID: 5357 RVA: 0x000538E2 File Offset: 0x00051AE2
		[DataSourceProperty]
		public HeroVM Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				if (value != this._owner)
				{
					this._owner = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Owner");
				}
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x00053900 File Offset: 0x00051B00
		// (set) Token: 0x060014EF RID: 5359 RVA: 0x00053908 File Offset: 0x00051B08
		[DataSourceProperty]
		public string SettlementsText
		{
			get
			{
				return this._villagesText;
			}
			set
			{
				if (value != this._villagesText)
				{
					this._villagesText = value;
					base.OnPropertyChanged("VillagesText");
				}
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060014F0 RID: 5360 RVA: 0x0005392A File Offset: 0x00051B2A
		// (set) Token: 0x060014F1 RID: 5361 RVA: 0x00053932 File Offset: 0x00051B32
		[DataSourceProperty]
		public string SettlementImageID
		{
			get
			{
				return this._settlementImageID;
			}
			set
			{
				if (value != this._settlementImageID)
				{
					this._settlementImageID = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementImageID");
				}
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x00053955 File Offset: 0x00051B55
		// (set) Token: 0x060014F3 RID: 5363 RVA: 0x0005395D File Offset: 0x00051B5D
		[DataSourceProperty]
		public string NotableCharactersText
		{
			get
			{
				return this._notableCharactersText;
			}
			set
			{
				if (value != this._notableCharactersText)
				{
					this._notableCharactersText = value;
					base.OnPropertyChangedWithValue<string>(value, "NotableCharactersText");
				}
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060014F4 RID: 5364 RVA: 0x00053980 File Offset: 0x00051B80
		// (set) Token: 0x060014F5 RID: 5365 RVA: 0x00053988 File Offset: 0x00051B88
		[DataSourceProperty]
		public int SettlementType
		{
			get
			{
				return this._settlementType;
			}
			set
			{
				if (value != this._settlementType)
				{
					this._settlementType = value;
					base.OnPropertyChangedWithValue(value, "SettlementType");
				}
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060014F6 RID: 5366 RVA: 0x000539A6 File Offset: 0x00051BA6
		// (set) Token: 0x060014F7 RID: 5367 RVA: 0x000539AE File Offset: 0x00051BAE
		[DataSourceProperty]
		public MBBindingList<EncyclopediaHistoryEventVM> History
		{
			get
			{
				return this._history;
			}
			set
			{
				if (value != this._history)
				{
					this._history = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaHistoryEventVM>>(value, "History");
				}
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x000539CC File Offset: 0x00051BCC
		// (set) Token: 0x060014F9 RID: 5369 RVA: 0x000539D4 File Offset: 0x00051BD4
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSettlementVM> Settlements
		{
			get
			{
				return this._settlements;
			}
			set
			{
				if (value != this._settlements)
				{
					this._settlements = value;
					base.OnPropertyChanged("Villages");
				}
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060014FA RID: 5370 RVA: 0x000539F1 File Offset: 0x00051BF1
		// (set) Token: 0x060014FB RID: 5371 RVA: 0x000539F9 File Offset: 0x00051BF9
		[DataSourceProperty]
		public MBBindingList<HeroVM> NotableCharacters
		{
			get
			{
				return this._notableCharacters;
			}
			set
			{
				if (value != this._notableCharacters)
				{
					this._notableCharacters = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "NotableCharacters");
				}
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060014FC RID: 5372 RVA: 0x00053A17 File Offset: 0x00051C17
		// (set) Token: 0x060014FD RID: 5373 RVA: 0x00053A1F File Offset: 0x00051C1F
		[DataSourceProperty]
		public HintViewModel ShowInMapHint
		{
			get
			{
				return this._showInMapHint;
			}
			set
			{
				if (value != this._showInMapHint)
				{
					this._showInMapHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ShowInMapHint");
				}
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060014FE RID: 5374 RVA: 0x00053A3D File Offset: 0x00051C3D
		// (set) Token: 0x060014FF RID: 5375 RVA: 0x00053A45 File Offset: 0x00051C45
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSettlementPageStatItemVM> LeftSideProperties
		{
			get
			{
				return this._leftSideProperties;
			}
			set
			{
				if (value != this._leftSideProperties)
				{
					this._leftSideProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSettlementPageStatItemVM>>(value, "LeftSideProperties");
				}
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06001500 RID: 5376 RVA: 0x00053A63 File Offset: 0x00051C63
		// (set) Token: 0x06001501 RID: 5377 RVA: 0x00053A6B File Offset: 0x00051C6B
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSettlementPageStatItemVM> RightSideProperties
		{
			get
			{
				return this._rightSideProperties;
			}
			set
			{
				if (value != this._rightSideProperties)
				{
					this._rightSideProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSettlementPageStatItemVM>>(value, "RightSideProperties");
				}
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x00053A89 File Offset: 0x00051C89
		// (set) Token: 0x06001503 RID: 5379 RVA: 0x00053A91 File Offset: 0x00051C91
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

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06001504 RID: 5380 RVA: 0x00053AB4 File Offset: 0x00051CB4
		// (set) Token: 0x06001505 RID: 5381 RVA: 0x00053ABC File Offset: 0x00051CBC
		[DataSourceProperty]
		public string CultureText
		{
			get
			{
				return this._cultureText;
			}
			set
			{
				if (value != this._cultureText)
				{
					this._cultureText = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureText");
				}
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x00053ADF File Offset: 0x00051CDF
		// (set) Token: 0x06001507 RID: 5383 RVA: 0x00053AE7 File Offset: 0x00051CE7
		[DataSourceProperty]
		public string OwnerText
		{
			get
			{
				return this._ownerText;
			}
			set
			{
				if (value != this._ownerText)
				{
					this._ownerText = value;
					base.OnPropertyChangedWithValue<string>(value, "OwnerText");
				}
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x00053B0A File Offset: 0x00051D0A
		// (set) Token: 0x06001509 RID: 5385 RVA: 0x00053B12 File Offset: 0x00051D12
		[DataSourceProperty]
		public bool IsVisualTrackerSelected
		{
			get
			{
				return this._isVisualTrackerSelected;
			}
			set
			{
				if (value != this._isVisualTrackerSelected)
				{
					this._isVisualTrackerSelected = value;
					base.OnPropertyChangedWithValue(value, "IsVisualTrackerSelected");
				}
			}
		}

		// Token: 0x04000978 RID: 2424
		protected readonly Settlement _settlement;

		// Token: 0x04000979 RID: 2425
		private int _settlementType;

		// Token: 0x0400097A RID: 2426
		private MBBindingList<EncyclopediaHistoryEventVM> _history;

		// Token: 0x0400097B RID: 2427
		private MBBindingList<EncyclopediaSettlementVM> _settlements;

		// Token: 0x0400097C RID: 2428
		private EncyclopediaSettlementVM _boundSettlement;

		// Token: 0x0400097D RID: 2429
		private MBBindingList<HeroVM> _notableCharacters;

		// Token: 0x0400097E RID: 2430
		private EncyclopediaFactionVM _ownerBanner;

		// Token: 0x0400097F RID: 2431
		private HintViewModel _showInMapHint;

		// Token: 0x04000980 RID: 2432
		private MBBindingList<EncyclopediaSettlementPageStatItemVM> _leftSideProperties;

		// Token: 0x04000981 RID: 2433
		private MBBindingList<EncyclopediaSettlementPageStatItemVM> _rightSideProperties;

		// Token: 0x04000982 RID: 2434
		private HeroVM _owner;

		// Token: 0x04000983 RID: 2435
		private string _ownerText;

		// Token: 0x04000984 RID: 2436
		private string _nameText;

		// Token: 0x04000985 RID: 2437
		private string _cultureText;

		// Token: 0x04000986 RID: 2438
		private string _villagesText;

		// Token: 0x04000987 RID: 2439
		private string _notableCharactersText;

		// Token: 0x04000988 RID: 2440
		private string _settlementPath;

		// Token: 0x04000989 RID: 2441
		private string _settlementName;

		// Token: 0x0400098A RID: 2442
		private string _informationText;

		// Token: 0x0400098B RID: 2443
		private string _settlementImageID;

		// Token: 0x0400098C RID: 2444
		private string _boundSettlementText;

		// Token: 0x0400098D RID: 2445
		private string _trackText;

		// Token: 0x0400098E RID: 2446
		private double _settlementCropPosition;

		// Token: 0x0400098F RID: 2447
		private bool _isFortification;

		// Token: 0x04000990 RID: 2448
		private bool _isVisualTrackerSelected;

		// Token: 0x04000991 RID: 2449
		private bool _hasBoundSettlement;

		// Token: 0x04000992 RID: 2450
		private bool _isTrackerButtonHighlightEnabled;

		// Token: 0x0200024C RID: 588
		private enum SettlementTypes
		{
			// Token: 0x040012A7 RID: 4775
			Town,
			// Token: 0x040012A8 RID: 4776
			LoneVillage,
			// Token: 0x040012A9 RID: 4777
			VillageWithCastle
		}
	}
}
