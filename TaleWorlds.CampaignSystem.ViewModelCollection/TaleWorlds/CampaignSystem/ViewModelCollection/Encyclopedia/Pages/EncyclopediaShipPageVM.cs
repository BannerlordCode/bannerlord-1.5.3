using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000DF RID: 223
	[EncyclopediaViewModel(typeof(ShipHull))]
	public class EncyclopediaShipPageVM : EncyclopediaContentPageVM
	{
		// Token: 0x06001513 RID: 5395 RVA: 0x00053BD4 File Offset: 0x00051DD4
		public EncyclopediaShipPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this._shipHull = base.Obj as ShipHull;
			this._missionShip = MBObjectManager.Instance.GetObject<MissionShipObject>(this._shipHull.MissionShipObjectId);
			this.StatList = new MBBindingList<EncyclopediaShipStatVM>();
			this.AllShipSlots = new MBBindingList<EncyclopediaShipSlotVM>();
			base.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(this._shipHull);
			this.SailType = this.GetSailType();
			this.RefreshValues();
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x00053C5C File Offset: 0x00051E5C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameText = this.GetName();
			this.PrefabId = EncyclopediaShipPageVM.GetPrefabIdOfShipHull(this._shipHull);
			TextObject description = this._shipHull.Description;
			this.DescriptionText = ((description != null) ? description.ToString() : null) ?? "";
			this.AvailableUpgradesText = new TextObject("{=0xN2FaYa}Available Upgrades", null).ToString();
			this.RefreshShipSlots();
			this.RefreshStats();
			base.UpdateBookmarkHintText();
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x00053CDC File Offset: 0x00051EDC
		private void RefreshShipSlots()
		{
			this.AllShipSlots.Clear();
			using (List<ShipSlot>.Enumerator enumerator = MBObjectManager.Instance.GetObjectTypeList<ShipSlot>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ShipSlot shipSlot = enumerator.Current;
					this.AllShipSlots.Add(new EncyclopediaShipSlotVM(shipSlot.TypeId, this._shipHull.AvailableSlots.Values.Any<ShipSlot>((ShipSlot x) => x.TypeId == shipSlot.TypeId)));
				}
			}
			this.AllShipSlots.Add(new EncyclopediaShipSlotVM("figurehead", this._shipHull.CanEquipFigurehead));
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x00053DA0 File Offset: 0x00051FA0
		private void RefreshStats()
		{
			this.StatList.Clear();
			this.StatsText = new TextObject("{=ffjTMejn}Stats", null).ToString();
			this.StatList.Add(new EncyclopediaShipStatVM("hull", new TextObject("{=wEmx6fZi}Hull", null), this._shipHull.Name.ToString(), null));
			this.StatList.Add(new EncyclopediaShipStatVM("class", new TextObject("{=sqdzHOPe}Class", null), GameTexts.FindText("str_ship_type", this._shipHull.Type.ToString().ToLowerInvariant()).ToString(), null));
			this.StatList.Add(new EncyclopediaShipStatVM("crew", new TextObject("{=wXCM8BnW}Crew", null), this.GetCrewCapacityStr(), new Func<List<TooltipProperty>>(this.GetCrewCapacityTooltip)));
			this.StatList.Add(new EncyclopediaShipStatVM("cargo_capacity", new TextObject("{=IE1KbkaH}Cargo Capacity", null), this._shipHull.InventoryCapacity.ToString(), null));
			this.StatList.Add(new EncyclopediaShipStatVM("weight", new TextObject("{=4Dd2xgPm}Weight", null), this._missionShip.Mass.ToString("0"), null));
			this.StatList.Add(new EncyclopediaShipStatVM("travel_speed", new TextObject("{=DbERaPfF}Travel Speed", null), this._shipHull.BaseSpeed.ToString("0.##"), null));
			this.SailTypeStat = new EncyclopediaShipStatVM("sail_type", new TextObject("{=PJyFY05L}Sail", null), this.GetSailTypeDescription(), null);
			this.StatList.Add(this.SailTypeStat);
			this.StatList.Add(new EncyclopediaShipStatVM("draft_type", new TextObject("{=I4bu7cLr}Draft", null), this.GetDraftTypeStr(), null));
			this.StatList.Add(new EncyclopediaShipStatVM("sea_worthiness", new TextObject("{=yCzuXN3O}Seaworthiness", null), this._shipHull.SeaWorthiness.ToString(), null));
			this.StatList.Add(new EncyclopediaShipStatVM("hit_points", new TextObject("{=oBbiVeKE}Hit Points", null), this._shipHull.MaxHitPoints.ToString(), null));
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x00053FE8 File Offset: 0x000521E8
		private string GetSailType()
		{
			if (this._missionShip.HasSails)
			{
				bool flag = this._missionShip.Sails.Any<ShipSail>((ShipSail x) => x.Type == TaleWorlds.Core.SailType.Lateen);
				bool flag2 = this._missionShip.Sails.Any<ShipSail>((ShipSail x) => x.Type == TaleWorlds.Core.SailType.Square);
				if (flag && flag2)
				{
					return "Hybrid";
				}
				if (flag)
				{
					return "Lateen";
				}
				if (flag2)
				{
					return "Square";
				}
			}
			return "None";
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00054084 File Offset: 0x00052284
		private string GetSailTypeDescription()
		{
			if (this._missionShip.HasSails)
			{
				bool flag = this._missionShip.Sails.Any<ShipSail>((ShipSail x) => x.Type == TaleWorlds.Core.SailType.Lateen);
				bool flag2 = this._missionShip.Sails.Any<ShipSail>((ShipSail x) => x.Type == TaleWorlds.Core.SailType.Square);
				if (flag && flag2)
				{
					return new TextObject("{=bXJLb0BE}Hybrid", null).ToString();
				}
				if (flag)
				{
					return new TextObject("{=kNxD2oer}Lateen", null).ToString();
				}
				if (flag2)
				{
					return new TextObject("{=squareSail}Square", null).ToString();
				}
			}
			return new TextObject("{=koX9okuG}None", null).ToString();
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0005414F File Offset: 0x0005234F
		private string GetDraftTypeStr()
		{
			if (this._shipHull.CanNavigateShallowWater)
			{
				return new TextObject("{=ShipDraftTypeShallow}Shallow", null).ToString();
			}
			return new TextObject("{=ShipDraftTypeDeep}Deep", null).ToString();
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x00054180 File Offset: 0x00052380
		private string GetCrewCapacityStr()
		{
			int skeletalCrewCapacity = this._shipHull.SkeletalCrewCapacity;
			int mainDeckCrewCapacity = this._shipHull.MainDeckCrewCapacity;
			int num = this._shipHull.TotalCrewCapacity - this._shipHull.MainDeckCrewCapacity;
			TextObject textObject;
			if (num > 0)
			{
				textObject = new TextObject("{=!}{SKELETAL} • {DECK} + {RESERVE}", null);
			}
			else
			{
				textObject = new TextObject("{=!}{SKELETAL} • {DECK}", null);
			}
			return textObject.SetTextVariable("SKELETAL", skeletalCrewCapacity).SetTextVariable("DECK", mainDeckCrewCapacity).SetTextVariable("RESERVE", num)
				.ToString();
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x00054204 File Offset: 0x00052404
		private List<TooltipProperty> GetCrewCapacityTooltip()
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			int skeletalCrewCapacity = this._shipHull.SkeletalCrewCapacity;
			int mainDeckCrewCapacity = this._shipHull.MainDeckCrewCapacity;
			int totalCrewCapacity = this._shipHull.TotalCrewCapacity;
			int num = totalCrewCapacity - mainDeckCrewCapacity;
			list.Add(new TooltipProperty(new TextObject("{=kalMphFt}Skeletal Capacity", null).ToString(), skeletalCrewCapacity.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty(string.Empty, GameTexts.FindText("str_ship_stat_explanation", "crewskeletal").ToString(), -1, false, TooltipProperty.TooltipPropertyFlags.MultiLine));
			list.Add(new TooltipProperty(string.Empty, string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.DefaultSeperator));
			list.Add(new TooltipProperty(new TextObject("{=Bt82dbKu}Deck Capacity", null).ToString(), mainDeckCrewCapacity.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty(string.Empty, GameTexts.FindText("str_ship_stat_explanation", "crewdeck").ToString(), -1, false, TooltipProperty.TooltipPropertyFlags.MultiLine));
			list.Add(new TooltipProperty(string.Empty, string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty(new TextObject("{=HThruy9f}Reserve Capacity", null).ToString(), num.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty(string.Empty, GameTexts.FindText("str_ship_stat_explanation", "crewreserve").ToString(), -1, false, TooltipProperty.TooltipPropertyFlags.MultiLine));
			list.Add(new TooltipProperty(string.Empty, string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.RundownSeperator));
			list.Add(new TooltipProperty(new TextObject("{=kLvWPxIK}Total Capacity", null).ToString(), totalCrewCapacity.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty(string.Empty, GameTexts.FindText("str_ship_stat_explanation", "crewtotal").ToString(), -1, false, TooltipProperty.TooltipPropertyFlags.MultiLine));
			return list;
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x000543BE File Offset: 0x000525BE
		public override string GetName()
		{
			return this._shipHull.Name.ToString();
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x000543D0 File Offset: 0x000525D0
		private static string GetPrefabIdOfShipHull(ShipHull shipHull)
		{
			MissionShipObject @object = MBObjectManager.Instance.GetObject<MissionShipObject>(shipHull.MissionShipObjectId);
			return ((@object != null) ? @object.Prefab : null) ?? string.Empty;
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x000543F8 File Offset: 0x000525F8
		public override string GetNavigationBarURL()
		{
			return HyperlinkTexts.GetGenericHyperlinkText("Home", GameTexts.FindText("str_encyclopedia_home", null).ToString()) + " \\ " + HyperlinkTexts.GetGenericHyperlinkText("ListPage-Ships", GameTexts.FindText("str_encyclopedia_ships", null).ToString()) + " \\ " + this.GetName();
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0005445D File Offset: 0x0005265D
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x00054470 File Offset: 0x00052670
		public override void ExecuteSwitchBookmarkedState()
		{
			base.ExecuteSwitchBookmarkedState();
			if (base.IsBookmarked)
			{
				Campaign.Current.EncyclopediaManager.ViewDataTracker.AddEncyclopediaBookmarkToItem(this._shipHull);
				return;
			}
			Campaign.Current.EncyclopediaManager.ViewDataTracker.RemoveEncyclopediaBookmarkFromItem(this._shipHull);
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x000544C0 File Offset: 0x000526C0
		// (set) Token: 0x06001522 RID: 5410 RVA: 0x000544C8 File Offset: 0x000526C8
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

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06001523 RID: 5411 RVA: 0x000544EB File Offset: 0x000526EB
		// (set) Token: 0x06001524 RID: 5412 RVA: 0x000544F3 File Offset: 0x000526F3
		[DataSourceProperty]
		public string AvailableUpgradesText
		{
			get
			{
				return this._availableUpgradesText;
			}
			set
			{
				if (value != this._availableUpgradesText)
				{
					this._availableUpgradesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AvailableUpgradesText");
				}
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x00054516 File Offset: 0x00052716
		// (set) Token: 0x06001526 RID: 5414 RVA: 0x0005451E File Offset: 0x0005271E
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x00054541 File Offset: 0x00052741
		// (set) Token: 0x06001528 RID: 5416 RVA: 0x00054549 File Offset: 0x00052749
		[DataSourceProperty]
		public string PrefabId
		{
			get
			{
				return this._prefabId;
			}
			set
			{
				if (value != this._prefabId)
				{
					this._prefabId = value;
					base.OnPropertyChangedWithValue<string>(value, "PrefabId");
				}
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x0005456C File Offset: 0x0005276C
		// (set) Token: 0x0600152A RID: 5418 RVA: 0x00054574 File Offset: 0x00052774
		[DataSourceProperty]
		public string StatsText
		{
			get
			{
				return this._statsText;
			}
			set
			{
				if (value != this._statsText)
				{
					this._statsText = value;
					base.OnPropertyChangedWithValue<string>(value, "StatsText");
				}
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x0600152B RID: 5419 RVA: 0x00054597 File Offset: 0x00052797
		// (set) Token: 0x0600152C RID: 5420 RVA: 0x0005459F File Offset: 0x0005279F
		[DataSourceProperty]
		public string SailType
		{
			get
			{
				return this._sailType;
			}
			set
			{
				if (value != this._sailType)
				{
					this._sailType = value;
					base.OnPropertyChangedWithValue<string>(value, "SailType");
				}
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x0600152D RID: 5421 RVA: 0x000545C2 File Offset: 0x000527C2
		// (set) Token: 0x0600152E RID: 5422 RVA: 0x000545CA File Offset: 0x000527CA
		[DataSourceProperty]
		public EncyclopediaShipStatVM SailTypeStat
		{
			get
			{
				return this._sailTypeStat;
			}
			set
			{
				if (value != this._sailTypeStat)
				{
					this._sailTypeStat = value;
					base.OnPropertyChangedWithValue<EncyclopediaShipStatVM>(value, "SailTypeStat");
				}
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x0600152F RID: 5423 RVA: 0x000545E8 File Offset: 0x000527E8
		// (set) Token: 0x06001530 RID: 5424 RVA: 0x000545F0 File Offset: 0x000527F0
		[DataSourceProperty]
		public MBBindingList<EncyclopediaShipStatVM> StatList
		{
			get
			{
				return this._statList;
			}
			set
			{
				if (value != this._statList)
				{
					this._statList = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaShipStatVM>>(value, "StatList");
				}
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06001531 RID: 5425 RVA: 0x0005460E File Offset: 0x0005280E
		// (set) Token: 0x06001532 RID: 5426 RVA: 0x00054616 File Offset: 0x00052816
		[DataSourceProperty]
		public MBBindingList<EncyclopediaShipSlotVM> AllShipSlots
		{
			get
			{
				return this._allShipSlots;
			}
			set
			{
				if (value != this._allShipSlots)
				{
					this._allShipSlots = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaShipSlotVM>>(value, "AllShipSlots");
				}
			}
		}

		// Token: 0x04000993 RID: 2451
		private readonly ShipHull _shipHull;

		// Token: 0x04000994 RID: 2452
		private readonly MissionShipObject _missionShip;

		// Token: 0x04000995 RID: 2453
		private string _descriptionText;

		// Token: 0x04000996 RID: 2454
		private string _prefabId;

		// Token: 0x04000997 RID: 2455
		private string _nameText;

		// Token: 0x04000998 RID: 2456
		private string _availableUpgradesText;

		// Token: 0x04000999 RID: 2457
		private string _statsText;

		// Token: 0x0400099A RID: 2458
		private string _sailType;

		// Token: 0x0400099B RID: 2459
		private EncyclopediaShipStatVM _sailTypeStat;

		// Token: 0x0400099C RID: 2460
		private MBBindingList<EncyclopediaShipStatVM> _statList;

		// Token: 0x0400099D RID: 2461
		private MBBindingList<EncyclopediaShipSlotVM> _allShipSlots;
	}
}
