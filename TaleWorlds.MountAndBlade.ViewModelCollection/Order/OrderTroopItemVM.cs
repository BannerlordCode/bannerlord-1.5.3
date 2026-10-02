using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.FormationMarker;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x02000023 RID: 35
	public class OrderTroopItemVM : OrderSubjectVM
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060002F2 RID: 754 RVA: 0x0000B3D0 File Offset: 0x000095D0
		// (remove) Token: 0x060002F3 RID: 755 RVA: 0x0000B404 File Offset: 0x00009604
		public static event Action<OrderTroopItemVM, bool> OnSelectionChange;

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000B437 File Offset: 0x00009637
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000B43F File Offset: 0x0000963F
		public bool ContainsDeadTroop { get; private set; }

		// Token: 0x060002F6 RID: 758 RVA: 0x0000B448 File Offset: 0x00009648
		public OrderTroopItemVM(Formation formation, Action<OrderTroopItemVM> setSelected, Func<Formation, int> getMorale)
		{
			this.IsValid = true;
			this.ActiveFormationClasses = new MBBindingList<OrderTroopItemFormationClassVM>();
			this.ActiveFilters = new MBBindingList<OrderTroopItemFilterVM>();
			this.InitialFormationClass = formation.FormationIndex;
			this.SetFormationClassFromFormation(formation);
			this.Formation = formation;
			this.FormationIndex = formation.Index;
			this.FormationName = (this.FormationIndex + 1).ToString();
			this.SetSelected = setSelected;
			this.CurrentMemberCount = (formation.IsPlayerTroopInFormation ? (formation.CountOfUnits - 1) : formation.CountOfUnits);
			this.Morale = getMorale(formation);
			base.UnderAttackOfType = 0;
			base.BehaviorType = 0;
			this.UpdateSelectionKeyInfo();
			this.UpdateVisuals();
			this.Formation.OnUnitCountChanged += this.FormationOnOnUnitCountChanged;
			this.RefreshValues();
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000B51E File Offset: 0x0000971E
		public OrderTroopItemVM()
		{
			this.IsValid = false;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000B530 File Offset: 0x00009730
		public override void OnFinalize()
		{
			if (this.IsValid)
			{
				this.Formation.OnUnitCountChanged -= this.FormationOnOnUnitCountChanged;
			}
			InputKeyItemVM applySelectionKey = base.ApplySelectionKey;
			if (applySelectionKey != null)
			{
				applySelectionKey.OnFinalize();
			}
			InputKeyItemVM toggleSelectionKey = base.ToggleSelectionKey;
			if (toggleSelectionKey == null)
			{
				return;
			}
			toggleSelectionKey.OnFinalize();
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000B57D File Offset: 0x0000977D
		protected override void OnSelectionStateChanged(bool isSelected)
		{
			Action<OrderTroopItemVM, bool> onSelectionChange = OrderTroopItemVM.OnSelectionChange;
			if (onSelectionChange == null)
			{
				return;
			}
			onSelectionChange(this, isSelected);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000B590 File Offset: 0x00009790
		private void FormationOnOnUnitCountChanged(Formation formation)
		{
			this.CurrentMemberCount = (formation.IsPlayerTroopInFormation ? (formation.CountOfUnits - 1) : formation.CountOfUnits);
			this.UpdateVisuals();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000B5B6 File Offset: 0x000097B6
		public void OnFormationAgentRemoved(Agent agent)
		{
			if (!agent.IsActive())
			{
				this.ContainsDeadTroop = true;
			}
			this.UpdateVisuals();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000B5D0 File Offset: 0x000097D0
		public virtual void UpdateVisuals()
		{
			Formation formation = this.Formation;
			bool flag;
			if (formation == null)
			{
				flag = null != null;
			}
			else
			{
				Agent captain = formation.Captain;
				flag = ((captain != null) ? captain.Character : null) != null;
			}
			if (flag)
			{
				if (this.CaptainImageIdentifier == null || this.Formation.Captain.Character != this._cachedCaptain)
				{
					CharacterImageIdentifierVM captainImageIdentifier = this.CaptainImageIdentifier;
					if (captainImageIdentifier != null)
					{
						captainImageIdentifier.OnFinalize();
					}
					this.CaptainImageIdentifier = new CharacterImageIdentifierVM(CharacterCode.CreateFrom(this.Formation.Captain.Character));
					this.HasCaptain = true;
					this._cachedCaptain = this.Formation.Captain.Character;
					return;
				}
			}
			else
			{
				CharacterImageIdentifierVM captainImageIdentifier2 = this.CaptainImageIdentifier;
				if (captainImageIdentifier2 != null)
				{
					captainImageIdentifier2.OnFinalize();
				}
				this.CaptainImageIdentifier = null;
				this.HasCaptain = false;
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000B68B File Offset: 0x0000988B
		public virtual void Update()
		{
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000B690 File Offset: 0x00009890
		public void UpdateSelectionKeyInfo()
		{
			if (this.Formation == null)
			{
				return;
			}
			if (Input.IsGamepadActive)
			{
				GameKey gameKey = HotKeyManager.GetCategory("MissionOrderHotkeyCategory").GetGameKey(91);
				InputKeyItemVM toggleSelectionKey = base.ToggleSelectionKey;
				if (toggleSelectionKey != null)
				{
					toggleSelectionKey.OnFinalize();
				}
				base.ToggleSelectionKey = InputKeyItemVM.CreateFromGameKey(gameKey, true);
				gameKey = HotKeyManager.GetCategory("MissionOrderHotkeyCategory").GetGameKey(90);
				InputKeyItemVM applySelectionKey = base.ApplySelectionKey;
				if (applySelectionKey != null)
				{
					applySelectionKey.OnFinalize();
				}
				base.ApplySelectionKey = InputKeyItemVM.CreateFromGameKey(gameKey, true);
				return;
			}
			int num = -1;
			if (this.Formation.Index == 0)
			{
				num = 79;
			}
			else if (this.Formation.Index == 1)
			{
				num = 80;
			}
			else if (this.Formation.Index == 2)
			{
				num = 81;
			}
			else if (this.Formation.Index == 3)
			{
				num = 82;
			}
			else if (this.Formation.Index == 4)
			{
				num = 83;
			}
			else if (this.Formation.Index == 5)
			{
				num = 84;
			}
			else if (this.Formation.Index == 6)
			{
				num = 85;
			}
			else if (this.Formation.Index == 7)
			{
				num = 86;
			}
			if (num == -1)
			{
				return;
			}
			GameKey gameKey2 = HotKeyManager.GetCategory("MissionOrderHotkeyCategory").GetGameKey(num);
			InputKeyItemVM applySelectionKey2 = base.ApplySelectionKey;
			if (applySelectionKey2 != null)
			{
				applySelectionKey2.OnFinalize();
			}
			base.ApplySelectionKey = InputKeyItemVM.CreateFromGameKey(gameKey2, false);
			InputKeyItemVM toggleSelectionKey2 = base.ToggleSelectionKey;
			if (toggleSelectionKey2 != null)
			{
				toggleSelectionKey2.OnFinalize();
			}
			base.ToggleSelectionKey = null;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000B7F4 File Offset: 0x000099F4
		public bool SetFormationClassFromFormation(Formation formation)
		{
			bool flag = formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.Infantry) > 0;
			bool flag2 = formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.Ranged) > 0;
			bool flag3 = formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.Cavalry) > 0;
			bool flag4 = formation.GetCountOfUnitsBelongingToLogicalClass(FormationClass.HorseArcher) > 0;
			if (flag && this._cachedInfantryItem == null)
			{
				this._cachedInfantryItem = new OrderTroopItemFormationClassVM(formation, FormationClass.Infantry);
				this.ActiveFormationClasses.Add(this._cachedInfantryItem);
			}
			else if (!flag)
			{
				this.ActiveFormationClasses.Remove(this._cachedInfantryItem);
				this._cachedInfantryItem = null;
			}
			if (flag2 && this._cachedRangedItem == null)
			{
				this._cachedRangedItem = new OrderTroopItemFormationClassVM(formation, FormationClass.Ranged);
				this.ActiveFormationClasses.Add(this._cachedRangedItem);
			}
			else if (!flag2)
			{
				this.ActiveFormationClasses.Remove(this._cachedRangedItem);
				this._cachedRangedItem = null;
			}
			if (flag3 && this._cachedCavalryItem == null)
			{
				this._cachedCavalryItem = new OrderTroopItemFormationClassVM(formation, FormationClass.Cavalry);
				this.ActiveFormationClasses.Add(this._cachedCavalryItem);
			}
			else if (!flag3)
			{
				this.ActiveFormationClasses.Remove(this._cachedCavalryItem);
				this._cachedCavalryItem = null;
			}
			if (flag4 && this._cachedHorseArcherItem == null)
			{
				this._cachedHorseArcherItem = new OrderTroopItemFormationClassVM(formation, FormationClass.HorseArcher);
				this.ActiveFormationClasses.Add(this._cachedHorseArcherItem);
			}
			else if (!flag4)
			{
				this.ActiveFormationClasses.Remove(this._cachedHorseArcherItem);
				this._cachedHorseArcherItem = null;
			}
			foreach (OrderTroopItemFormationClassVM orderTroopItemFormationClassVM in this.ActiveFormationClasses)
			{
				orderTroopItemFormationClassVM.UpdateTroopCount();
			}
			this.UpdateVisuals();
			return false;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000B994 File Offset: 0x00009B94
		public void UpdateFilterData(List<FormationFilterType> usedFilters)
		{
			this.ActiveFilters.Clear();
			foreach (FormationFilterType formationFilterType in usedFilters)
			{
				this.ActiveFilters.Add(new OrderTroopItemFilterVM((int)formationFilterType));
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000B9F8 File Offset: 0x00009BF8
		public void ExecuteAction()
		{
			this.SetSelected(this);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000BA08 File Offset: 0x00009C08
		public virtual void RefreshTargetedOrderVisual()
		{
			bool flag = false;
			string text = null;
			string text2 = null;
			for (int i = 0; i < base.ActiveOrders.Count; i++)
			{
				OrderItemVM orderItemVM = base.ActiveOrders[i];
				if (orderItemVM.Order.IsTargeted())
				{
					Formation targetFormation = this.Formation.TargetFormation;
					if (targetFormation != null)
					{
						text2 = MissionFormationMarkerTargetVM.GetFormationType(targetFormation.PhysicalClass);
						flag = true;
					}
					text = orderItemVM.OrderIconId;
				}
			}
			this.HasTarget = flag;
			this.CurrentOrderIconId = text;
			this.CurrentTargetFormationType = text2;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000BA8A File Offset: 0x00009C8A
		public virtual TextObject GetVisibleNameOfFormationForMessage()
		{
			return GameTexts.FindText("str_formation_class_string", this.Formation.PhysicalClass.GetName());
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000BAA6 File Offset: 0x00009CA6
		// (set) Token: 0x06000305 RID: 773 RVA: 0x0000BAAE File Offset: 0x00009CAE
		[DataSourceProperty]
		public bool IsValid
		{
			get
			{
				return this._isValid;
			}
			set
			{
				if (value != this._isValid)
				{
					this._isValid = value;
					base.OnPropertyChangedWithValue(value, "IsValid");
				}
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000BACC File Offset: 0x00009CCC
		// (set) Token: 0x06000307 RID: 775 RVA: 0x0000BAD4 File Offset: 0x00009CD4
		[DataSourceProperty]
		public int FormationIndex
		{
			get
			{
				return this._formationIndex;
			}
			set
			{
				if (value != this._formationIndex)
				{
					this._formationIndex = value;
					base.OnPropertyChangedWithValue(value, "FormationIndex");
				}
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000BAF2 File Offset: 0x00009CF2
		// (set) Token: 0x06000309 RID: 777 RVA: 0x0000BAFA File Offset: 0x00009CFA
		[DataSourceProperty]
		public int CurrentMemberCount
		{
			get
			{
				return this._currentMemberCount;
			}
			set
			{
				if (value != this._currentMemberCount)
				{
					this._currentMemberCount = value;
					base.OnPropertyChangedWithValue(value, "CurrentMemberCount");
					this.HaveTroops = value > 0;
				}
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0000BB22 File Offset: 0x00009D22
		// (set) Token: 0x0600030B RID: 779 RVA: 0x0000BB2A File Offset: 0x00009D2A
		[DataSourceProperty]
		public int Morale
		{
			get
			{
				return this._morale;
			}
			set
			{
				if (value != this._morale)
				{
					this._morale = value;
					base.OnPropertyChangedWithValue(value, "Morale");
				}
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0000BB48 File Offset: 0x00009D48
		// (set) Token: 0x0600030D RID: 781 RVA: 0x0000BB50 File Offset: 0x00009D50
		[DataSourceProperty]
		public float AmmoPercentage
		{
			get
			{
				return this._ammoPercentage;
			}
			set
			{
				if (value != this._ammoPercentage)
				{
					this._ammoPercentage = value;
					base.OnPropertyChangedWithValue(value, "AmmoPercentage");
				}
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0000BB6E File Offset: 0x00009D6E
		// (set) Token: 0x0600030F RID: 783 RVA: 0x0000BB76 File Offset: 0x00009D76
		[DataSourceProperty]
		public bool IsAmmoAvailable
		{
			get
			{
				return this._isAmmoAvailable;
			}
			set
			{
				if (value != this._isAmmoAvailable)
				{
					this._isAmmoAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsAmmoAvailable");
				}
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0000BB94 File Offset: 0x00009D94
		// (set) Token: 0x06000311 RID: 785 RVA: 0x0000BB9C File Offset: 0x00009D9C
		[DataSourceProperty]
		public bool HaveTroops
		{
			get
			{
				return this._haveTroops;
			}
			set
			{
				if (value != this._haveTroops)
				{
					this._haveTroops = value;
					base.OnPropertyChangedWithValue(value, "HaveTroops");
				}
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0000BBBA File Offset: 0x00009DBA
		// (set) Token: 0x06000313 RID: 787 RVA: 0x0000BBC2 File Offset: 0x00009DC2
		[DataSourceProperty]
		public bool HasTarget
		{
			get
			{
				return this._hasTarget;
			}
			set
			{
				if (value != this._hasTarget)
				{
					this._hasTarget = value;
					base.OnPropertyChangedWithValue(value, "HasTarget");
				}
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000BBE0 File Offset: 0x00009DE0
		// (set) Token: 0x06000315 RID: 789 RVA: 0x0000BBE8 File Offset: 0x00009DE8
		[DataSourceProperty]
		public bool IsTargetRelevant
		{
			get
			{
				return this._isTargetRelevant;
			}
			set
			{
				if (value != this._isTargetRelevant)
				{
					this._isTargetRelevant = value;
					base.OnPropertyChangedWithValue(value, "IsTargetRelevant");
				}
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000316 RID: 790 RVA: 0x0000BC06 File Offset: 0x00009E06
		// (set) Token: 0x06000317 RID: 791 RVA: 0x0000BC0E File Offset: 0x00009E0E
		[DataSourceProperty]
		public bool HasCaptain
		{
			get
			{
				return this._hasCaptain;
			}
			set
			{
				if (value != this._hasCaptain)
				{
					this._hasCaptain = value;
					base.OnPropertyChangedWithValue(value, "HasCaptain");
				}
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0000BC2C File Offset: 0x00009E2C
		// (set) Token: 0x06000319 RID: 793 RVA: 0x0000BC34 File Offset: 0x00009E34
		[DataSourceProperty]
		public string CurrentOrderIconId
		{
			get
			{
				return this._currentOrderIconId;
			}
			set
			{
				if (value != this._currentOrderIconId)
				{
					this._currentOrderIconId = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentOrderIconId");
				}
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000BC57 File Offset: 0x00009E57
		// (set) Token: 0x0600031B RID: 795 RVA: 0x0000BC5F File Offset: 0x00009E5F
		[DataSourceProperty]
		public string CurrentTargetFormationType
		{
			get
			{
				return this._currentTargetFormationType;
			}
			set
			{
				if (value != this._currentTargetFormationType)
				{
					this._currentTargetFormationType = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentTargetFormationType");
				}
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000BC82 File Offset: 0x00009E82
		// (set) Token: 0x0600031D RID: 797 RVA: 0x0000BC8A File Offset: 0x00009E8A
		[DataSourceProperty]
		public string FormationName
		{
			get
			{
				return this._formationName;
			}
			set
			{
				if (value != this._formationName)
				{
					this._formationName = value;
					base.OnPropertyChangedWithValue<string>(value, "FormationName");
				}
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0000BCAD File Offset: 0x00009EAD
		// (set) Token: 0x0600031F RID: 799 RVA: 0x0000BCB5 File Offset: 0x00009EB5
		[DataSourceProperty]
		public CharacterImageIdentifierVM CaptainImageIdentifier
		{
			get
			{
				return this._captainImageIdentifier;
			}
			set
			{
				if (value != this._captainImageIdentifier)
				{
					this._captainImageIdentifier = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "CaptainImageIdentifier");
				}
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000BCD3 File Offset: 0x00009ED3
		// (set) Token: 0x06000321 RID: 801 RVA: 0x0000BCDB File Offset: 0x00009EDB
		[DataSourceProperty]
		public MBBindingList<OrderTroopItemFormationClassVM> ActiveFormationClasses
		{
			get
			{
				return this._activeFormationClasses;
			}
			set
			{
				if (value != this._activeFormationClasses)
				{
					this._activeFormationClasses = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderTroopItemFormationClassVM>>(value, "ActiveFormationClasses");
				}
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000BCF9 File Offset: 0x00009EF9
		// (set) Token: 0x06000323 RID: 803 RVA: 0x0000BD01 File Offset: 0x00009F01
		[DataSourceProperty]
		public MBBindingList<OrderTroopItemFilterVM> ActiveFilters
		{
			get
			{
				return this._activeFilters;
			}
			set
			{
				if (value != this._activeFilters)
				{
					this._activeFilters = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderTroopItemFilterVM>>(value, "ActiveFilters");
				}
			}
		}

		// Token: 0x0400014C RID: 332
		public FormationClass InitialFormationClass;

		// Token: 0x0400014D RID: 333
		public Formation Formation;

		// Token: 0x0400014E RID: 334
		public Type MachineType;

		// Token: 0x0400014F RID: 335
		public Action<OrderTroopItemVM> SetSelected;

		// Token: 0x04000151 RID: 337
		private OrderTroopItemFormationClassVM _cachedInfantryItem;

		// Token: 0x04000152 RID: 338
		private OrderTroopItemFormationClassVM _cachedRangedItem;

		// Token: 0x04000153 RID: 339
		private OrderTroopItemFormationClassVM _cachedCavalryItem;

		// Token: 0x04000154 RID: 340
		private OrderTroopItemFormationClassVM _cachedHorseArcherItem;

		// Token: 0x04000155 RID: 341
		private BasicCharacterObject _cachedCaptain;

		// Token: 0x04000156 RID: 342
		private bool _isValid;

		// Token: 0x04000157 RID: 343
		private int _formationIndex;

		// Token: 0x04000158 RID: 344
		private int _currentMemberCount;

		// Token: 0x04000159 RID: 345
		private int _morale;

		// Token: 0x0400015A RID: 346
		private float _ammoPercentage;

		// Token: 0x0400015B RID: 347
		private bool _isAmmoAvailable;

		// Token: 0x0400015C RID: 348
		private bool _haveTroops;

		// Token: 0x0400015D RID: 349
		private bool _hasTarget;

		// Token: 0x0400015E RID: 350
		private bool _isTargetRelevant;

		// Token: 0x0400015F RID: 351
		private bool _hasCaptain;

		// Token: 0x04000160 RID: 352
		private string _currentOrderIconId;

		// Token: 0x04000161 RID: 353
		private string _currentTargetFormationType;

		// Token: 0x04000162 RID: 354
		private string _formationName;

		// Token: 0x04000163 RID: 355
		private CharacterImageIdentifierVM _captainImageIdentifier;

		// Token: 0x04000164 RID: 356
		private MBBindingList<OrderTroopItemFormationClassVM> _activeFormationClasses;

		// Token: 0x04000165 RID: 357
		private MBBindingList<OrderTroopItemFilterVM> _activeFilters;
	}
}
