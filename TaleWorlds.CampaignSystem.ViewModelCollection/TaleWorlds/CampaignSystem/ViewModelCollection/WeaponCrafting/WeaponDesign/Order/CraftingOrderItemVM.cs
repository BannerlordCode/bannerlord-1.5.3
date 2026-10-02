using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign.Order
{
	// Token: 0x02000115 RID: 277
	public class CraftingOrderItemVM : ViewModel
	{
		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06001904 RID: 6404 RVA: 0x0005FE6A File Offset: 0x0005E06A
		public CraftingOrder CraftingOrder { get; }

		// Token: 0x06001905 RID: 6405 RVA: 0x0005FE74 File Offset: 0x0005E074
		public CraftingOrderItemVM(CraftingOrder order, Action<CraftingOrderItemVM> onSelection, Func<CraftingAvailableHeroItemVM> getCurrentCraftingHero, List<CraftingStatData> orderStatDatas, CampaignUIHelper.IssueQuestFlags questFlags = CampaignUIHelper.IssueQuestFlags.None)
		{
			this.CraftingOrder = order;
			this._orderOwner = order.OrderOwner;
			this._getCurrentCraftingHero = getCurrentCraftingHero;
			this._orderStatDatas = orderStatDatas;
			this._onSelection = onSelection;
			this.WeaponAttributes = new MBBindingList<WeaponAttributeVM>();
			this.OrderOwnerData = new HeroVM(this._orderOwner, false);
			this._weaponTemplate = order.PreCraftedWeaponDesignItem.WeaponDesign.Template;
			this.OrderWeaponTypeCode = this._weaponTemplate.StringId;
			this.Quests = this.GetQuestMarkers(questFlags);
			this.IsQuestOrder = this.Quests.Count > 0;
			this.RefreshValues();
			this.RefreshStats();
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x0005FF34 File Offset: 0x0005E134
		private MBBindingList<QuestMarkerVM> GetQuestMarkers(CampaignUIHelper.IssueQuestFlags flags)
		{
			MBBindingList<QuestMarkerVM> mbbindingList = new MBBindingList<QuestMarkerVM>();
			if ((flags & CampaignUIHelper.IssueQuestFlags.ActiveIssue) != CampaignUIHelper.IssueQuestFlags.None)
			{
				mbbindingList.Add(new QuestMarkerVM(CampaignUIHelper.IssueQuestFlags.ActiveIssue, null, null));
			}
			if ((flags & CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest) != CampaignUIHelper.IssueQuestFlags.None)
			{
				mbbindingList.Add(new QuestMarkerVM(CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest, null, null));
			}
			return mbbindingList;
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x0005FF70 File Offset: 0x0005E170
		public void RefreshStats()
		{
			this.WeaponAttributes.Clear();
			ItemObject preCraftedWeaponDesignItem = this.CraftingOrder.PreCraftedWeaponDesignItem;
			if (((preCraftedWeaponDesignItem != null) ? preCraftedWeaponDesignItem.Weapons : null) == null)
			{
				Debug.FailedAssert("Crafting order does not contain any valid weapons", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Crafting\\WeaponDesign\\Order\\CraftingOrderItemVM.cs", "RefreshStats", 71);
				return;
			}
			this.CraftingOrder.GetStatWeapon();
			foreach (CraftingStatData craftingStatData in this._orderStatDatas)
			{
				if (craftingStatData.IsValid)
				{
					this.WeaponAttributes.Add(new WeaponAttributeVM(craftingStatData.Type, craftingStatData.DamageType, craftingStatData.DescriptionText.ToString(), craftingStatData.CurValue));
				}
			}
			IEnumerable<Hero> enumerable = from x in CraftingHelper.GetAvailableHeroesForCrafting()
				where this.CraftingOrder.IsOrderAvailableForHero(x)
				select x;
			this.HasAvailableHeroes = enumerable.Any<Hero>();
			this.OrderPrice = this.CraftingOrder.BaseGoldReward;
			this.RefreshDifficulty();
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x00060074 File Offset: 0x0005E274
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.OrderNumberText = GameTexts.FindText("str_crafting_order_header", null).ToString();
			this.OrderWeaponType = this._weaponTemplate.TemplateName.ToString();
			this.OrderDifficultyLabelText = this._difficultyText.ToString();
			this.OrderDifficultyValueText = MathF.Round(this.CraftingOrder.OrderDifficulty).ToString();
			this.DisabledReasonHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetCraftingOrderDisabledReasonTooltip(this._getCurrentCraftingHero().Hero, this.CraftingOrder));
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x000600FC File Offset: 0x0005E2FC
		private void RefreshDifficulty()
		{
			Hero hero = this._getCurrentCraftingHero().Hero;
			int skillValue = hero.GetSkillValue(DefaultSkills.Crafting);
			this.IsEnabled = this.CraftingOrder.IsOrderAvailableForHero(hero);
			this.IsDifficultySuitableForHero = this.CraftingOrder.OrderDifficulty < (float)skillValue;
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x0006014D File Offset: 0x0005E34D
		public void ExecuteSelectOrder()
		{
			Action<CraftingOrderItemVM> onSelection = this._onSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this);
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x0600190B RID: 6411 RVA: 0x00060160 File Offset: 0x0005E360
		// (set) Token: 0x0600190C RID: 6412 RVA: 0x00060168 File Offset: 0x0005E368
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x0600190D RID: 6413 RVA: 0x00060186 File Offset: 0x0005E386
		// (set) Token: 0x0600190E RID: 6414 RVA: 0x0006018E File Offset: 0x0005E38E
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

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x0600190F RID: 6415 RVA: 0x000601AC File Offset: 0x0005E3AC
		// (set) Token: 0x06001910 RID: 6416 RVA: 0x000601B4 File Offset: 0x0005E3B4
		[DataSourceProperty]
		public bool HasAvailableHeroes
		{
			get
			{
				return this._hasAvailableHeroes;
			}
			set
			{
				if (value != this._hasAvailableHeroes)
				{
					this._hasAvailableHeroes = value;
					base.OnPropertyChangedWithValue(value, "HasAvailableHeroes");
				}
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06001911 RID: 6417 RVA: 0x000601D2 File Offset: 0x0005E3D2
		// (set) Token: 0x06001912 RID: 6418 RVA: 0x000601DA File Offset: 0x0005E3DA
		[DataSourceProperty]
		public bool IsDifficultySuitableForHero
		{
			get
			{
				return this._isDifficultySuitableForHero;
			}
			set
			{
				if (value != this._isDifficultySuitableForHero)
				{
					this._isDifficultySuitableForHero = value;
					base.OnPropertyChangedWithValue(value, "IsDifficultySuitableForHero");
				}
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06001913 RID: 6419 RVA: 0x000601F8 File Offset: 0x0005E3F8
		// (set) Token: 0x06001914 RID: 6420 RVA: 0x00060200 File Offset: 0x0005E400
		[DataSourceProperty]
		public bool IsQuestOrder
		{
			get
			{
				return this._isQuestOrder;
			}
			set
			{
				if (value != this._isQuestOrder)
				{
					this._isQuestOrder = value;
					base.OnPropertyChangedWithValue(value, "IsQuestOrder");
				}
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06001915 RID: 6421 RVA: 0x0006021E File Offset: 0x0005E41E
		// (set) Token: 0x06001916 RID: 6422 RVA: 0x00060226 File Offset: 0x0005E426
		[DataSourceProperty]
		public int OrderPrice
		{
			get
			{
				return this._orderPrice;
			}
			set
			{
				if (value != this._orderPrice)
				{
					this._orderPrice = value;
					base.OnPropertyChangedWithValue(value, "OrderPrice");
				}
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06001917 RID: 6423 RVA: 0x00060244 File Offset: 0x0005E444
		// (set) Token: 0x06001918 RID: 6424 RVA: 0x0006024C File Offset: 0x0005E44C
		[DataSourceProperty]
		public string OrderDifficultyLabelText
		{
			get
			{
				return this._orderDifficultyLabelText;
			}
			set
			{
				if (value != this._orderDifficultyLabelText)
				{
					this._orderDifficultyLabelText = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderDifficultyLabelText");
				}
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06001919 RID: 6425 RVA: 0x0006026F File Offset: 0x0005E46F
		// (set) Token: 0x0600191A RID: 6426 RVA: 0x00060277 File Offset: 0x0005E477
		[DataSourceProperty]
		public string OrderDifficultyValueText
		{
			get
			{
				return this._orderDifficultyValueText;
			}
			set
			{
				if (value != this._orderDifficultyValueText)
				{
					this._orderDifficultyValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderDifficultyValueText");
				}
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x0600191B RID: 6427 RVA: 0x0006029A File Offset: 0x0005E49A
		// (set) Token: 0x0600191C RID: 6428 RVA: 0x000602A2 File Offset: 0x0005E4A2
		[DataSourceProperty]
		public string OrderNumberText
		{
			get
			{
				return this._orderNumberText;
			}
			set
			{
				if (value != this._orderNumberText)
				{
					this._orderNumberText = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderNumberText");
				}
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x0600191D RID: 6429 RVA: 0x000602C5 File Offset: 0x0005E4C5
		// (set) Token: 0x0600191E RID: 6430 RVA: 0x000602CD File Offset: 0x0005E4CD
		[DataSourceProperty]
		public string OrderWeaponType
		{
			get
			{
				return this._orderWeaponType;
			}
			set
			{
				if (value != this._orderWeaponType)
				{
					this._orderWeaponType = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderWeaponType");
				}
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x0600191F RID: 6431 RVA: 0x000602F0 File Offset: 0x0005E4F0
		// (set) Token: 0x06001920 RID: 6432 RVA: 0x000602F8 File Offset: 0x0005E4F8
		[DataSourceProperty]
		public string OrderWeaponTypeCode
		{
			get
			{
				return this._orderWeaponTypeCode;
			}
			set
			{
				if (value != this._orderWeaponTypeCode)
				{
					this._orderWeaponTypeCode = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderWeaponTypeCode");
				}
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06001921 RID: 6433 RVA: 0x0006031B File Offset: 0x0005E51B
		// (set) Token: 0x06001922 RID: 6434 RVA: 0x00060323 File Offset: 0x0005E523
		[DataSourceProperty]
		public HeroVM OrderOwnerData
		{
			get
			{
				return this._orderOwnerData;
			}
			set
			{
				if (value != this._orderOwnerData)
				{
					this._orderOwnerData = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "OrderOwnerData");
				}
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06001923 RID: 6435 RVA: 0x00060341 File Offset: 0x0005E541
		// (set) Token: 0x06001924 RID: 6436 RVA: 0x00060349 File Offset: 0x0005E549
		[DataSourceProperty]
		public BasicTooltipViewModel DisabledReasonHint
		{
			get
			{
				return this._disabledReasonHint;
			}
			set
			{
				if (value != this._disabledReasonHint)
				{
					this._disabledReasonHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "DisabledReasonHint");
				}
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06001925 RID: 6437 RVA: 0x00060367 File Offset: 0x0005E567
		// (set) Token: 0x06001926 RID: 6438 RVA: 0x0006036F File Offset: 0x0005E56F
		[DataSourceProperty]
		public MBBindingList<QuestMarkerVM> Quests
		{
			get
			{
				return this._quests;
			}
			set
			{
				if (value != this._quests)
				{
					this._quests = value;
					base.OnPropertyChangedWithValue<MBBindingList<QuestMarkerVM>>(value, "Quests");
				}
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06001927 RID: 6439 RVA: 0x0006038D File Offset: 0x0005E58D
		// (set) Token: 0x06001928 RID: 6440 RVA: 0x00060395 File Offset: 0x0005E595
		[DataSourceProperty]
		public MBBindingList<WeaponAttributeVM> WeaponAttributes
		{
			get
			{
				return this._weaponAttributes;
			}
			set
			{
				if (value != this._weaponAttributes)
				{
					this._weaponAttributes = value;
					base.OnPropertyChangedWithValue<MBBindingList<WeaponAttributeVM>>(value, "WeaponAttributes");
				}
			}
		}

		// Token: 0x04000B6E RID: 2926
		private Hero _orderOwner;

		// Token: 0x04000B6F RID: 2927
		private Action<CraftingOrderItemVM> _onSelection;

		// Token: 0x04000B70 RID: 2928
		private Func<CraftingAvailableHeroItemVM> _getCurrentCraftingHero;

		// Token: 0x04000B71 RID: 2929
		private CraftingTemplate _weaponTemplate;

		// Token: 0x04000B72 RID: 2930
		private TextObject _difficultyText = new TextObject("{=udPWHmOm}Difficulty:", null);

		// Token: 0x04000B73 RID: 2931
		private List<CraftingStatData> _orderStatDatas;

		// Token: 0x04000B74 RID: 2932
		private bool _isEnabled;

		// Token: 0x04000B75 RID: 2933
		private bool _isSelected;

		// Token: 0x04000B76 RID: 2934
		private bool _hasAvailableHeroes;

		// Token: 0x04000B77 RID: 2935
		private bool _isDifficultySuitableForHero;

		// Token: 0x04000B78 RID: 2936
		private bool _isQuestOrder;

		// Token: 0x04000B79 RID: 2937
		private int _orderPrice;

		// Token: 0x04000B7A RID: 2938
		private string _orderDifficultyLabelText;

		// Token: 0x04000B7B RID: 2939
		private string _orderDifficultyValueText;

		// Token: 0x04000B7C RID: 2940
		private string _orderNumberText;

		// Token: 0x04000B7D RID: 2941
		private string _orderWeaponType;

		// Token: 0x04000B7E RID: 2942
		private string _orderWeaponTypeCode;

		// Token: 0x04000B7F RID: 2943
		private HeroVM _orderOwnerData;

		// Token: 0x04000B80 RID: 2944
		private BasicTooltipViewModel _disabledReasonHint;

		// Token: 0x04000B81 RID: 2945
		private MBBindingList<QuestMarkerVM> _quests;

		// Token: 0x04000B82 RID: 2946
		private MBBindingList<WeaponAttributeVM> _weaponAttributes;
	}
}
