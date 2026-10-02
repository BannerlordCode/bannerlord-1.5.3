using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x0200002F RID: 47
	public class OrderOfBattleFormationClassVM : ViewModel
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000C3C8 File Offset: 0x0000A5C8
		// (set) Token: 0x0600035A RID: 858 RVA: 0x0000C3D0 File Offset: 0x0000A5D0
		public FormationClass Class
		{
			get
			{
				return this._class;
			}
			set
			{
				if (value != this._class)
				{
					if (!this._isFormationClassPreset)
					{
						Action<OrderOfBattleFormationClassVM, FormationClass> onClassChanged = OrderOfBattleFormationClassVM.OnClassChanged;
						if (onClassChanged != null)
						{
							onClassChanged(this, value);
						}
					}
					this._class = value;
					this.IsUnset = this._class == FormationClass.NumberOfAllFormations;
					this.ShownFormationClass = (int)(this.IsUnset ? FormationClass.Infantry : (this._class + 1));
					this.UpdateTroopCountText();
					this._isFormationClassPreset = false;
				}
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000C43D File Offset: 0x0000A63D
		// (set) Token: 0x0600035C RID: 860 RVA: 0x0000C445 File Offset: 0x0000A645
		public int PreviousWeight { get; private set; }

		// Token: 0x0600035D RID: 861 RVA: 0x0000C44E File Offset: 0x0000A64E
		public OrderOfBattleFormationClassVM(OrderOfBattleFormationItemVM formationItem, FormationClass formationClass = FormationClass.NumberOfAllFormations)
		{
			this.BelongedFormationItem = formationItem;
			this._isFormationClassPreset = formationClass != FormationClass.NumberOfAllFormations;
			this.Class = formationClass;
			this.PreviousWeight = 0;
			this.OnWeightAdjusted();
			this.RefreshValues();
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000C485 File Offset: 0x0000A685
		public override void RefreshValues()
		{
			this.LockWeightHint = new HintViewModel(new TextObject("{=mPCrz4rs}Lock troop percentage from relative changes.", null), null);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000C49E File Offset: 0x0000A69E
		private void OnWeightAdjusted()
		{
			if (!this._isLockedOfWeightAdjustments)
			{
				Action<OrderOfBattleFormationClassVM> onWeightAdjustedCallback = OrderOfBattleFormationClassVM.OnWeightAdjustedCallback;
				if (onWeightAdjustedCallback != null)
				{
					onWeightAdjustedCallback(this);
				}
			}
			this.UpdateTroopCountText();
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000C4C0 File Offset: 0x0000A6C0
		public void UpdateTroopCountText()
		{
			if (this.Class != FormationClass.NumberOfAllFormations && OrderOfBattleFormationClassVM.GetTotalCountOfTroopType != null)
			{
				this.TroopCountText = GameTexts.FindText("str_LEFT_over_RIGHT", null).SetTextVariable("LEFT", OrderOfBattleUIHelper.GetCountOfRealUnitsInClass(this)).SetTextVariable("RIGHT", OrderOfBattleFormationClassVM.GetTotalCountOfTroopType(this.Class))
					.ToString();
				return;
			}
			this.TroopCountText = string.Empty;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000C52A File Offset: 0x0000A72A
		public void SetWeightAdjustmentLock(bool isLocked)
		{
			this._isLockedOfWeightAdjustments = isLocked;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000C533 File Offset: 0x0000A733
		public void UpdateWeightAdjustable()
		{
			bool flag;
			if (this.Class != FormationClass.NumberOfAllFormations)
			{
				Func<OrderOfBattleFormationClassVM, bool> canAdjustWeight = OrderOfBattleFormationClassVM.CanAdjustWeight;
				flag = canAdjustWeight != null && canAdjustWeight(this);
			}
			else
			{
				flag = false;
			}
			this.IsAdjustable = flag;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000C55A File Offset: 0x0000A75A
		// (set) Token: 0x06000364 RID: 868 RVA: 0x0000C562 File Offset: 0x0000A762
		[DataSourceProperty]
		public bool IsAdjustable
		{
			get
			{
				return this._isAdjustable;
			}
			set
			{
				if (value != this._isAdjustable)
				{
					this._isAdjustable = value && Mission.Current.PlayerTeam.IsPlayerGeneral;
					base.OnPropertyChangedWithValue(this._isAdjustable, "IsAdjustable");
				}
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0000C599 File Offset: 0x0000A799
		// (set) Token: 0x06000366 RID: 870 RVA: 0x0000C5A1 File Offset: 0x0000A7A1
		[DataSourceProperty]
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
			set
			{
				if (value != this._isLocked)
				{
					this._isLocked = value;
					base.OnPropertyChangedWithValue(value, "IsLocked");
				}
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000C5BF File Offset: 0x0000A7BF
		// (set) Token: 0x06000368 RID: 872 RVA: 0x0000C5C7 File Offset: 0x0000A7C7
		[DataSourceProperty]
		public bool IsUnset
		{
			get
			{
				return this._isUnset;
			}
			set
			{
				if (value != this._isUnset)
				{
					this._isUnset = value;
					base.OnPropertyChangedWithValue(value, "IsUnset");
				}
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000369 RID: 873 RVA: 0x0000C5E5 File Offset: 0x0000A7E5
		// (set) Token: 0x0600036A RID: 874 RVA: 0x0000C5ED File Offset: 0x0000A7ED
		[DataSourceProperty]
		public int Weight
		{
			get
			{
				return this._weight;
			}
			set
			{
				if (value != this._weight)
				{
					this.PreviousWeight = this._weight;
					this._weight = value;
					base.OnPropertyChangedWithValue(value, "Weight");
					this.OnWeightAdjusted();
				}
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0000C61D File Offset: 0x0000A81D
		// (set) Token: 0x0600036C RID: 876 RVA: 0x0000C625 File Offset: 0x0000A825
		[DataSourceProperty]
		public int ShownFormationClass
		{
			get
			{
				return this._shownFormationClass;
			}
			set
			{
				if (value != this._shownFormationClass)
				{
					this._shownFormationClass = value;
					base.OnPropertyChangedWithValue(value, "ShownFormationClass");
				}
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0000C643 File Offset: 0x0000A843
		// (set) Token: 0x0600036E RID: 878 RVA: 0x0000C64B File Offset: 0x0000A84B
		[DataSourceProperty]
		public string TroopCountText
		{
			get
			{
				return this._troopCountText;
			}
			set
			{
				if (value != this._troopCountText)
				{
					this._troopCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "TroopCountText");
				}
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0000C66E File Offset: 0x0000A86E
		// (set) Token: 0x06000370 RID: 880 RVA: 0x0000C676 File Offset: 0x0000A876
		[DataSourceProperty]
		public HintViewModel LockWeightHint
		{
			get
			{
				return this._lockWeightHint;
			}
			set
			{
				if (value != this._lockWeightHint)
				{
					this._lockWeightHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LockWeightHint");
				}
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000371 RID: 881 RVA: 0x0000C694 File Offset: 0x0000A894
		// (set) Token: 0x06000372 RID: 882 RVA: 0x0000C69C File Offset: 0x0000A89C
		[DataSourceProperty]
		public bool IsWeightHighlightActive
		{
			get
			{
				return this._isWeightHighlightActive;
			}
			set
			{
				if (value != this._isWeightHighlightActive)
				{
					this._isWeightHighlightActive = value;
					base.OnPropertyChangedWithValue(value, "IsWeightHighlightActive");
				}
			}
		}

		// Token: 0x0400017A RID: 378
		private FormationClass _class;

		// Token: 0x0400017B RID: 379
		private bool _isLockedOfWeightAdjustments;

		// Token: 0x0400017D RID: 381
		public readonly OrderOfBattleFormationItemVM BelongedFormationItem;

		// Token: 0x0400017E RID: 382
		public static Action<OrderOfBattleFormationClassVM> OnWeightAdjustedCallback;

		// Token: 0x0400017F RID: 383
		public static Action<OrderOfBattleFormationClassVM, FormationClass> OnClassChanged;

		// Token: 0x04000180 RID: 384
		public static Func<OrderOfBattleFormationClassVM, bool> CanAdjustWeight;

		// Token: 0x04000181 RID: 385
		public static Func<FormationClass, int> GetTotalCountOfTroopType;

		// Token: 0x04000182 RID: 386
		private bool _isFormationClassPreset;

		// Token: 0x04000183 RID: 387
		private bool _isAdjustable;

		// Token: 0x04000184 RID: 388
		private bool _isLocked;

		// Token: 0x04000185 RID: 389
		private bool _isUnset;

		// Token: 0x04000186 RID: 390
		private int _weight;

		// Token: 0x04000187 RID: 391
		private int _shownFormationClass;

		// Token: 0x04000188 RID: 392
		private string _troopCountText;

		// Token: 0x04000189 RID: 393
		private HintViewModel _lockWeightHint;

		// Token: 0x0400018A RID: 394
		private bool _isWeightHighlightActive;
	}
}
