using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem
{
	// Token: 0x02000080 RID: 128
	public class MPArmoryCosmeticTauntItemVM : MPArmoryCosmeticItemBaseVM
	{
		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x00027D4A File Offset: 0x00025F4A
		public MPArmoryCosmeticsVM.TauntCategoryFlag TauntCategory { get; }

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x00027D52 File Offset: 0x00025F52
		public TauntCosmeticElement TauntCosmeticElement { get; }

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00027D5C File Offset: 0x00025F5C
		public MPArmoryCosmeticTauntItemVM(string tauntId, CosmeticElement cosmetic, string cosmeticID)
			: base(cosmetic, cosmeticID, CosmeticsManager.CosmeticType.Taunt)
		{
			this.TauntID = tauntId;
			this.TauntCosmeticElement = cosmetic as TauntCosmeticElement;
			this.TauntCategory = this.GetCategoryOfTaunt();
			this.TauntUsages = new MBBindingList<StringItemWithEnabledAndHintVM>();
			this.RefreshTauntUsages();
			this.BlocksMovementOnUsageHint = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00027DB4 File Offset: 0x00025FB4
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.BlocksMovementOnUsageHint != null)
			{
				this.BlocksMovementOnUsageHint.HintText = new TextObject("{=BUQsaZMg}Blocks Movement on Usage", null);
			}
			this.SelectSlotText = new TextObject("{=4gfAb1ar}Select a Slot", null).ToString();
			this.CancelEquipText = new TextObject("{=avYRbfHA}Cancel Equip", null).ToString();
			TauntCosmeticElement tauntCosmeticElement = this.TauntCosmeticElement;
			string text;
			if (tauntCosmeticElement == null)
			{
				text = null;
			}
			else
			{
				TextObject name = tauntCosmeticElement.Name;
				text = ((name != null) ? name.ToString() : null);
			}
			base.Name = text;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00027E38 File Offset: 0x00026038
		private void RefreshTauntUsages()
		{
			this.TauntUsages.Clear();
			TauntUsageManager.TauntUsageSet usageSet = TauntUsageManager.Instance.GetUsageSet(this.TauntCosmeticElement.Id);
			TauntUsageManager.TauntUsage.TauntUsageFlag? tauntUsageFlag;
			if (usageSet == null)
			{
				tauntUsageFlag = null;
			}
			else
			{
				MBReadOnlyList<TauntUsageManager.TauntUsage> usages = usageSet.GetUsages();
				tauntUsageFlag = ((usages != null) ? new TauntUsageManager.TauntUsage.TauntUsageFlag?(usages.FirstOrDefault<TauntUsageManager.TauntUsage>().UsageFlag) : null);
			}
			TauntUsageManager.TauntUsage.TauntUsageFlag tauntUsageFlag2 = tauntUsageFlag ?? TauntUsageManager.TauntUsage.TauntUsageFlag.None;
			TextObject textObject = new TextObject("{=aeDp7IEK}Usable with {USAGE}", null);
			if ((tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForOneHanded) == TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				TextObject textObject2 = new TextObject("{=PiHpR4QL}One Handed", null);
				TextObject textObject3 = textObject.CopyTextObject().SetTextVariable("USAGE", textObject2);
				this.TauntUsages.Add(new StringItemWithEnabledAndHintVM(null, "UsableWithOneHanded", true, null, textObject3));
			}
			if ((tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForTwoHanded) == TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				TextObject textObject4 = new TextObject("{=t78atYqH}Two Handed", null);
				TextObject textObject5 = textObject.CopyTextObject().SetTextVariable("USAGE", textObject4);
				this.TauntUsages.Add(new StringItemWithEnabledAndHintVM(null, "UsableWithTwoHanded", true, null, textObject5));
			}
			if ((tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForBow) == TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				TextObject textObject6 = new TextObject("{=5rj7xQE4}Bow", null);
				TextObject textObject7 = textObject.CopyTextObject().SetTextVariable("USAGE", textObject6);
				this.TauntUsages.Add(new StringItemWithEnabledAndHintVM(null, "UsableWithBow", true, null, textObject7));
			}
			if ((tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForCrossbow) == TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				TextObject textObject8 = new TextObject("{=TTWL7RLe}Crossbow", null);
				TextObject textObject9 = textObject.CopyTextObject().SetTextVariable("USAGE", textObject8);
				this.TauntUsages.Add(new StringItemWithEnabledAndHintVM(null, "UsableWithCrossbow", true, null, textObject9));
			}
			if ((tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForShield) == TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				TextObject textObject10 = new TextObject("{=Jd0Kq9lD}Shield", null);
				TextObject textObject11 = textObject.CopyTextObject().SetTextVariable("USAGE", textObject10);
				this.TauntUsages.Add(new StringItemWithEnabledAndHintVM(null, "UsableWithShield", true, null, textObject11));
			}
			if ((tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresOnFoot) == TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				TextObject textObject12 = new TextObject("{=uGM8DWrm}Mount", null);
				TextObject textObject13 = textObject.CopyTextObject().SetTextVariable("USAGE", textObject12);
				this.TauntUsages.Add(new StringItemWithEnabledAndHintVM(null, "UsableWithMount", true, null, textObject13));
			}
			this.RequiresOnFoot = (tauntUsageFlag2 & TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresOnFoot) > TauntUsageManager.TauntUsage.TauntUsageFlag.None;
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00028050 File Offset: 0x00026250
		private MPArmoryCosmeticsVM.TauntCategoryFlag GetCategoryOfTaunt()
		{
			TauntUsageManager.TauntUsageSet usageSet = TauntUsageManager.Instance.GetUsageSet(this.TauntCosmeticElement.Id);
			MBReadOnlyList<TauntUsageManager.TauntUsage> mbreadOnlyList = ((usageSet != null) ? usageSet.GetUsages() : null);
			MPArmoryCosmeticsVM.TauntCategoryFlag tauntCategoryFlag = MPArmoryCosmeticsVM.TauntCategoryFlag.None;
			if (mbreadOnlyList == null || mbreadOnlyList.Count <= 0)
			{
				return tauntCategoryFlag;
			}
			if (this.AnyUsageNotHaveFlag(mbreadOnlyList, TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresOnFoot))
			{
				tauntCategoryFlag |= MPArmoryCosmeticsVM.TauntCategoryFlag.UsableWithMount;
			}
			if (this.AnyUsageNotHaveFlag(mbreadOnlyList, TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForShield))
			{
				tauntCategoryFlag |= MPArmoryCosmeticsVM.TauntCategoryFlag.UsableWithShield;
			}
			if (this.AnyUsageNotHaveFlag(mbreadOnlyList, TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForOneHanded))
			{
				tauntCategoryFlag |= MPArmoryCosmeticsVM.TauntCategoryFlag.UsableWithOneHanded;
			}
			if (this.AnyUsageNotHaveFlag(mbreadOnlyList, TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForTwoHanded))
			{
				tauntCategoryFlag |= MPArmoryCosmeticsVM.TauntCategoryFlag.UsableWithTwoHanded;
			}
			if (this.AnyUsageNotHaveFlag(mbreadOnlyList, TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForBow))
			{
				tauntCategoryFlag |= MPArmoryCosmeticsVM.TauntCategoryFlag.UsableWithBow;
			}
			if (this.AnyUsageNotHaveFlag(mbreadOnlyList, TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForCrossbow))
			{
				tauntCategoryFlag |= MPArmoryCosmeticsVM.TauntCategoryFlag.UsableWithCrossbow;
			}
			return tauntCategoryFlag;
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x000280F4 File Offset: 0x000262F4
		private bool AllUsagesHaveFlag(MBReadOnlyList<TauntUsageManager.TauntUsage> list, TauntUsageManager.TauntUsage.TauntUsageFlag flag)
		{
			return list.All<TauntUsageManager.TauntUsage>((TauntUsageManager.TauntUsage u) => (u.UsageFlag & flag) > TauntUsageManager.TauntUsage.TauntUsageFlag.None);
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00028120 File Offset: 0x00026320
		private bool AnyUsageHaveFlag(MBReadOnlyList<TauntUsageManager.TauntUsage> list, TauntUsageManager.TauntUsage.TauntUsageFlag flag)
		{
			return list.Any<TauntUsageManager.TauntUsage>((TauntUsageManager.TauntUsage u) => (u.UsageFlag & flag) > TauntUsageManager.TauntUsage.TauntUsageFlag.None);
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0002814C File Offset: 0x0002634C
		private bool AnyUsageNotHaveFlag(MBReadOnlyList<TauntUsageManager.TauntUsage> list, TauntUsageManager.TauntUsage.TauntUsageFlag flag)
		{
			return list.Any<TauntUsageManager.TauntUsage>((TauntUsageManager.TauntUsage u) => (u.UsageFlag & flag) == TauntUsageManager.TauntUsage.TauntUsageFlag.None);
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00028178 File Offset: 0x00026378
		// (set) Token: 0x06000CE8 RID: 3304 RVA: 0x00028180 File Offset: 0x00026380
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
					base.UpdatePreviewAndActionTexts();
				}
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x000281A4 File Offset: 0x000263A4
		// (set) Token: 0x06000CEA RID: 3306 RVA: 0x000281AC File Offset: 0x000263AC
		[DataSourceProperty]
		public bool RequiresOnFoot
		{
			get
			{
				return this._requiresOnFoot;
			}
			set
			{
				if (value != this._requiresOnFoot)
				{
					this._requiresOnFoot = value;
					base.OnPropertyChangedWithValue(value, "RequiresOnFoot");
				}
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x000281CA File Offset: 0x000263CA
		// (set) Token: 0x06000CEC RID: 3308 RVA: 0x000281D2 File Offset: 0x000263D2
		[DataSourceProperty]
		public float PreviewAnimationRatio
		{
			get
			{
				return this._previewAnimationRatio;
			}
			set
			{
				if (value != this._previewAnimationRatio)
				{
					this._previewAnimationRatio = value;
					base.OnPropertyChangedWithValue(value, "PreviewAnimationRatio");
				}
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x000281F0 File Offset: 0x000263F0
		// (set) Token: 0x06000CEE RID: 3310 RVA: 0x000281F8 File Offset: 0x000263F8
		[DataSourceProperty]
		public string SelectSlotText
		{
			get
			{
				return this._selectSlotText;
			}
			set
			{
				if (value != this._selectSlotText)
				{
					this._selectSlotText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectSlotText");
					base.UpdatePreviewAndActionTexts();
				}
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x00028221 File Offset: 0x00026421
		// (set) Token: 0x06000CF0 RID: 3312 RVA: 0x00028229 File Offset: 0x00026429
		[DataSourceProperty]
		public string CancelEquipText
		{
			get
			{
				return this._cancelEquipText;
			}
			set
			{
				if (value != this._cancelEquipText)
				{
					this._cancelEquipText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelEquipText");
					base.UpdatePreviewAndActionTexts();
				}
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x00028252 File Offset: 0x00026452
		// (set) Token: 0x06000CF2 RID: 3314 RVA: 0x0002825A File Offset: 0x0002645A
		[DataSourceProperty]
		public string TauntID
		{
			get
			{
				return this._tauntId;
			}
			set
			{
				if (value != this._tauntId)
				{
					this._tauntId = value;
					base.OnPropertyChangedWithValue<string>(value, "TauntID");
				}
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x0002827D File Offset: 0x0002647D
		// (set) Token: 0x06000CF4 RID: 3316 RVA: 0x00028285 File Offset: 0x00026485
		[DataSourceProperty]
		public MBBindingList<StringItemWithEnabledAndHintVM> TauntUsages
		{
			get
			{
				return this._tauntUsages;
			}
			set
			{
				if (value != this._tauntUsages)
				{
					this._tauntUsages = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringItemWithEnabledAndHintVM>>(value, "TauntUsages");
				}
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x000282A3 File Offset: 0x000264A3
		// (set) Token: 0x06000CF6 RID: 3318 RVA: 0x000282AB File Offset: 0x000264AB
		[DataSourceProperty]
		public HintViewModel BlocksMovementOnUsageHint
		{
			get
			{
				return this._blocksMovementOnUsageHint;
			}
			set
			{
				if (value != this._blocksMovementOnUsageHint)
				{
					this._blocksMovementOnUsageHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "BlocksMovementOnUsageHint");
				}
			}
		}

		// Token: 0x040005D7 RID: 1495
		private bool _isSelected;

		// Token: 0x040005D8 RID: 1496
		private bool _requiresOnFoot;

		// Token: 0x040005D9 RID: 1497
		private float _previewAnimationRatio;

		// Token: 0x040005DA RID: 1498
		private string _selectSlotText;

		// Token: 0x040005DB RID: 1499
		private string _cancelEquipText;

		// Token: 0x040005DC RID: 1500
		private string _tauntId;

		// Token: 0x040005DD RID: 1501
		private HintViewModel _blocksMovementOnUsageHint;

		// Token: 0x040005DE RID: 1502
		private MBBindingList<StringItemWithEnabledAndHintVM> _tauntUsages;
	}
}
