using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000E0 RID: 224
	[EncyclopediaViewModel(typeof(CharacterObject))]
	public class EncyclopediaUnitPageVM : EncyclopediaContentPageVM
	{
		// Token: 0x06001533 RID: 5427 RVA: 0x00054634 File Offset: 0x00052834
		public EncyclopediaUnitPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this._character = base.Obj as CharacterObject;
			this.UnitCharacter = new CharacterViewModel(CharacterViewModel.StanceTypes.OnMount);
			this.UnitCharacter.FillFrom(this._character, -1, null);
			this.HasErrors = this.DoesCharacterHaveCircularUpgradePaths(this._character, null);
			if (!this.HasErrors)
			{
				CharacterObject characterObject = CharacterHelper.FindUpgradeRootOf(this._character);
				this.Tree = new EncyclopediaTroopTreeNodeVM(characterObject, this._character, false, null);
			}
			this.PropertiesList = new MBBindingList<StringItemWithHintVM>();
			this.EquipmentSetSelector = new SelectorVM<EncyclopediaUnitEquipmentSetSelectorItemVM>(0, new Action<SelectorVM<EncyclopediaUnitEquipmentSetSelectorItemVM>>(this.OnEquipmentSetChange));
			base.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(this._character);
			this.RefreshValues();
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x000546FC File Offset: 0x000528FC
		private bool DoesCharacterHaveCircularUpgradePaths(CharacterObject baseCharacter, CharacterObject character = null)
		{
			bool flag = false;
			if (character == null)
			{
				character = baseCharacter;
			}
			for (int i = 0; i < character.UpgradeTargets.Length; i++)
			{
				if (character.UpgradeTargets[i] == baseCharacter)
				{
					Debug.FailedAssert(string.Format("Circular dependency on troop upgrade paths: {0} --> {1}", character.Name, baseCharacter.Name), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Encyclopedia\\Pages\\EncyclopediaUnitPageVM.cs", "DoesCharacterHaveCircularUpgradePaths", 56);
					flag = true;
					break;
				}
				flag = this.DoesCharacterHaveCircularUpgradePaths(baseCharacter, character.UpgradeTargets[i]);
			}
			return flag;
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0005476C File Offset: 0x0005296C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._equipmentSetTextObj = new TextObject("{=vggt7exj}Set {CURINDEX}/{COUNT}", null);
			this.PropertiesList.Clear();
			this.PropertiesList.Add(CampaignUIHelper.GetCharacterTierData(this._character, true));
			this.PropertiesList.Add(CampaignUIHelper.GetCharacterTypeData(this._character, true));
			this.EquipmentSetSelector.ItemList.Clear();
			using (IEnumerator<Equipment> enumerator = this._character.BattleEquipments.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Equipment equipment = enumerator.Current;
					if (!this.EquipmentSetSelector.ItemList.Any<EncyclopediaUnitEquipmentSetSelectorItemVM>((EncyclopediaUnitEquipmentSetSelectorItemVM x) => x.EquipmentSet.IsEquipmentEqualTo(equipment)))
					{
						this.EquipmentSetSelector.AddItem(new EncyclopediaUnitEquipmentSetSelectorItemVM(equipment, ""));
					}
				}
			}
			if (this.EquipmentSetSelector.ItemList.Count > 0)
			{
				this.EquipmentSetSelector.SelectedIndex = 0;
			}
			this._equipmentSetTextObj.SetTextVariable("CURINDEX", this.EquipmentSetSelector.SelectedIndex + 1);
			this._equipmentSetTextObj.SetTextVariable("COUNT", this.EquipmentSetSelector.ItemList.Count);
			this.EquipmentSetText = this._equipmentSetTextObj.ToString();
			this.TreeDisplayErrorText = new TextObject("{=BkDycbdq}Error while displaying the troop tree", null).ToString();
			this.Skills = new MBBindingList<EncyclopediaSkillVM>();
			List<SkillObject> list = TaleWorlds.CampaignSystem.Extensions.Skills.All.ToList<SkillObject>();
			list.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			foreach (SkillObject skillObject in list)
			{
				if (this._character.GetSkillValue(skillObject) > 0)
				{
					this.Skills.Add(new EncyclopediaSkillVM(skillObject, this._character.GetSkillValue(skillObject)));
				}
			}
			this.DescriptionText = GameTexts.FindText("str_encyclopedia_unit_description", this._character.StringId).ToString();
			this.NameText = this._character.Name.ToString();
			EncyclopediaTroopTreeNodeVM tree = this.Tree;
			if (tree != null)
			{
				tree.RefreshValues();
			}
			CharacterViewModel unitCharacter = this.UnitCharacter;
			if (unitCharacter != null)
			{
				unitCharacter.RefreshValues();
			}
			base.UpdateBookmarkHintText();
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x000549C0 File Offset: 0x00052BC0
		private void OnEquipmentSetChange(SelectorVM<EncyclopediaUnitEquipmentSetSelectorItemVM> selector)
		{
			this.CurrentSelectedEquipmentSet = selector.SelectedItem;
			this.UnitCharacter.SetEquipment(this.CurrentSelectedEquipmentSet.EquipmentSet);
			this._equipmentSetTextObj.SetTextVariable("CURINDEX", selector.SelectedIndex + 1);
			this._equipmentSetTextObj.SetTextVariable("COUNT", selector.ItemList.Count);
			this.EquipmentSetText = this._equipmentSetTextObj.ToString();
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x00054A35 File Offset: 0x00052C35
		public override string GetName()
		{
			return this._character.Name.ToString();
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x00054A48 File Offset: 0x00052C48
		public override string GetNavigationBarURL()
		{
			return HyperlinkTexts.GetGenericHyperlinkText("Home", GameTexts.FindText("str_encyclopedia_home", null).ToString()) + " \\ " + HyperlinkTexts.GetGenericHyperlinkText("ListPage-Units", GameTexts.FindText("str_encyclopedia_troops", null).ToString()) + " \\ " + this.GetName();
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x00054AB0 File Offset: 0x00052CB0
		public override void ExecuteSwitchBookmarkedState()
		{
			base.ExecuteSwitchBookmarkedState();
			if (base.IsBookmarked)
			{
				Campaign.Current.EncyclopediaManager.ViewDataTracker.AddEncyclopediaBookmarkToItem(this._character);
				return;
			}
			Campaign.Current.EncyclopediaManager.ViewDataTracker.RemoveEncyclopediaBookmarkFromItem(this._character);
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x0600153A RID: 5434 RVA: 0x00054B00 File Offset: 0x00052D00
		// (set) Token: 0x0600153B RID: 5435 RVA: 0x00054B08 File Offset: 0x00052D08
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSkillVM> Skills
		{
			get
			{
				return this._skills;
			}
			set
			{
				if (value != this._skills)
				{
					this._skills = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSkillVM>>(value, "Skills");
				}
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x0600153C RID: 5436 RVA: 0x00054B26 File Offset: 0x00052D26
		// (set) Token: 0x0600153D RID: 5437 RVA: 0x00054B2E File Offset: 0x00052D2E
		[DataSourceProperty]
		public MBBindingList<StringItemWithHintVM> PropertiesList
		{
			get
			{
				return this._propertiesList;
			}
			set
			{
				if (value != this._propertiesList)
				{
					this._propertiesList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringItemWithHintVM>>(value, "PropertiesList");
				}
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x0600153E RID: 5438 RVA: 0x00054B4C File Offset: 0x00052D4C
		// (set) Token: 0x0600153F RID: 5439 RVA: 0x00054B54 File Offset: 0x00052D54
		[DataSourceProperty]
		public SelectorVM<EncyclopediaUnitEquipmentSetSelectorItemVM> EquipmentSetSelector
		{
			get
			{
				return this._equipmentSetSelector;
			}
			set
			{
				if (value != this._equipmentSetSelector)
				{
					this._equipmentSetSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<EncyclopediaUnitEquipmentSetSelectorItemVM>>(value, "EquipmentSetSelector");
				}
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x00054B72 File Offset: 0x00052D72
		// (set) Token: 0x06001541 RID: 5441 RVA: 0x00054B7A File Offset: 0x00052D7A
		[DataSourceProperty]
		public EncyclopediaUnitEquipmentSetSelectorItemVM CurrentSelectedEquipmentSet
		{
			get
			{
				return this._currentSelectedEquipmentSet;
			}
			set
			{
				if (value != this._currentSelectedEquipmentSet)
				{
					this._currentSelectedEquipmentSet = value;
					base.OnPropertyChangedWithValue<EncyclopediaUnitEquipmentSetSelectorItemVM>(value, "CurrentSelectedEquipmentSet");
				}
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x00054B98 File Offset: 0x00052D98
		// (set) Token: 0x06001543 RID: 5443 RVA: 0x00054BA0 File Offset: 0x00052DA0
		[DataSourceProperty]
		public CharacterViewModel UnitCharacter
		{
			get
			{
				return this._unitCharacter;
			}
			set
			{
				if (value != this._unitCharacter)
				{
					this._unitCharacter = value;
					base.OnPropertyChangedWithValue<CharacterViewModel>(value, "UnitCharacter");
				}
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06001544 RID: 5444 RVA: 0x00054BBE File Offset: 0x00052DBE
		// (set) Token: 0x06001545 RID: 5445 RVA: 0x00054BC6 File Offset: 0x00052DC6
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

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x00054BE9 File Offset: 0x00052DE9
		// (set) Token: 0x06001547 RID: 5447 RVA: 0x00054BF1 File Offset: 0x00052DF1
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

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06001548 RID: 5448 RVA: 0x00054C14 File Offset: 0x00052E14
		// (set) Token: 0x06001549 RID: 5449 RVA: 0x00054C1C File Offset: 0x00052E1C
		[DataSourceProperty]
		public EncyclopediaTroopTreeNodeVM Tree
		{
			get
			{
				return this._tree;
			}
			set
			{
				if (value != this._tree)
				{
					this._tree = value;
					base.OnPropertyChangedWithValue<EncyclopediaTroopTreeNodeVM>(value, "Tree");
				}
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x0600154A RID: 5450 RVA: 0x00054C3A File Offset: 0x00052E3A
		// (set) Token: 0x0600154B RID: 5451 RVA: 0x00054C42 File Offset: 0x00052E42
		[DataSourceProperty]
		public string TreeDisplayErrorText
		{
			get
			{
				return this._treeDisplayErrorText;
			}
			set
			{
				if (value != this._treeDisplayErrorText)
				{
					this._treeDisplayErrorText = value;
					base.OnPropertyChangedWithValue<string>(value, "TreeDisplayErrorText");
				}
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x00054C65 File Offset: 0x00052E65
		// (set) Token: 0x0600154D RID: 5453 RVA: 0x00054C6D File Offset: 0x00052E6D
		[DataSourceProperty]
		public string EquipmentSetText
		{
			get
			{
				return this._equipmentSetText;
			}
			set
			{
				if (value != this._equipmentSetText)
				{
					this._equipmentSetText = value;
					base.OnPropertyChangedWithValue<string>(value, "EquipmentSetText");
				}
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x00054C90 File Offset: 0x00052E90
		// (set) Token: 0x0600154F RID: 5455 RVA: 0x00054C98 File Offset: 0x00052E98
		[DataSourceProperty]
		public bool HasErrors
		{
			get
			{
				return this._hasErrors;
			}
			set
			{
				if (value != this._hasErrors)
				{
					this._hasErrors = value;
					base.OnPropertyChangedWithValue(value, "HasErrors");
				}
			}
		}

		// Token: 0x0400099E RID: 2462
		private readonly CharacterObject _character;

		// Token: 0x0400099F RID: 2463
		private TextObject _equipmentSetTextObj;

		// Token: 0x040009A0 RID: 2464
		private MBBindingList<EncyclopediaSkillVM> _skills;

		// Token: 0x040009A1 RID: 2465
		private MBBindingList<StringItemWithHintVM> _propertiesList;

		// Token: 0x040009A2 RID: 2466
		private SelectorVM<EncyclopediaUnitEquipmentSetSelectorItemVM> _equipmentSetSelector;

		// Token: 0x040009A3 RID: 2467
		private EncyclopediaUnitEquipmentSetSelectorItemVM _currentSelectedEquipmentSet;

		// Token: 0x040009A4 RID: 2468
		private EncyclopediaTroopTreeNodeVM _tree;

		// Token: 0x040009A5 RID: 2469
		private string _descriptionText;

		// Token: 0x040009A6 RID: 2470
		private CharacterViewModel _unitCharacter;

		// Token: 0x040009A7 RID: 2471
		private string _nameText;

		// Token: 0x040009A8 RID: 2472
		private string _treeDisplayErrorText;

		// Token: 0x040009A9 RID: 2473
		private string _equipmentSetText;

		// Token: 0x040009AA RID: 2474
		private bool _hasErrors;
	}
}
