using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x02000007 RID: 7
	public class ArmyCompositionGroupVM : ViewModel
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00004C3C File Offset: 0x00002E3C
		public ArmyCompositionGroupVM(TroopTypeSelectionPopUpVM troopTypeSelectionPopUp)
		{
			this.MinArmySize = 1;
			this.MaxArmySize = BannerlordConfig.MaxBattleSize;
			foreach (BasicCharacterObject basicCharacterObject in from c in Game.Current.ObjectManager.GetObjectTypeList<BasicCharacterObject>()
				where c.IsSoldier && !c.IsObsolete
				select c)
			{
				this._allCharacterObjects.Add(basicCharacterObject);
			}
			this.CompositionValues = new int[4];
			this.CompositionValues[0] = 25;
			this.CompositionValues[1] = 25;
			this.CompositionValues[2] = 25;
			this.CompositionValues[3] = 25;
			this.MeleeInfantryComposition = new ArmyCompositionItemVM(ArmyCompositionItemVM.CompositionType.MeleeInfantry, this._allCharacterObjects, this._allSkills, new Action<int, int>(this.UpdateSliders), troopTypeSelectionPopUp, this.CompositionValues);
			this.RangedInfantryComposition = new ArmyCompositionItemVM(ArmyCompositionItemVM.CompositionType.RangedInfantry, this._allCharacterObjects, this._allSkills, new Action<int, int>(this.UpdateSliders), troopTypeSelectionPopUp, this.CompositionValues);
			this.MeleeCavalryComposition = new ArmyCompositionItemVM(ArmyCompositionItemVM.CompositionType.MeleeCavalry, this._allCharacterObjects, this._allSkills, new Action<int, int>(this.UpdateSliders), troopTypeSelectionPopUp, this.CompositionValues);
			this.RangedCavalryComposition = new ArmyCompositionItemVM(ArmyCompositionItemVM.CompositionType.RangedCavalry, this._allCharacterObjects, this._allSkills, new Action<int, int>(this.UpdateSliders), troopTypeSelectionPopUp, this.CompositionValues);
			this.ArmySize = BannerlordConfig.GetRealBattleSize() / 5;
			this.RefreshValues();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00004DE8 File Offset: 0x00002FE8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ArmySizeTitle = GameTexts.FindText("str_army_size", null).ToString();
			this.MeleeInfantryComposition.RefreshValues();
			this.RangedInfantryComposition.RefreshValues();
			this.MeleeCavalryComposition.RefreshValues();
			this.RangedCavalryComposition.RefreshValues();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00004E40 File Offset: 0x00003040
		private static int SumOfValues(int[] array, bool[] enabledArray, int excludedIndex = -1)
		{
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (enabledArray[i] && excludedIndex != i)
				{
					num += array[i];
				}
			}
			return num;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00004E70 File Offset: 0x00003070
		public void SetCurrentSelectedCulture(BasicCultureObject selectedCulture)
		{
			if (this._selectedCulture != selectedCulture)
			{
				this.MeleeInfantryComposition.SetCurrentSelectedCulture(selectedCulture);
				this.RangedInfantryComposition.SetCurrentSelectedCulture(selectedCulture);
				this.MeleeCavalryComposition.SetCurrentSelectedCulture(selectedCulture);
				this.RangedCavalryComposition.SetCurrentSelectedCulture(selectedCulture);
				this._selectedCulture = selectedCulture;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00004EC0 File Offset: 0x000030C0
		private void UpdateSliders(int value, int changedSliderIndex)
		{
			if (this._updatingSliders)
			{
				return;
			}
			this._updatingSliders = true;
			bool[] array = new bool[]
			{
				!this.MeleeInfantryComposition.IsLocked,
				!this.RangedInfantryComposition.IsLocked,
				!this.MeleeCavalryComposition.IsLocked,
				!this.RangedCavalryComposition.IsLocked
			};
			int[] array2 = new int[]
			{
				this.CompositionValues[0],
				this.CompositionValues[1],
				this.CompositionValues[2],
				this.CompositionValues[3]
			};
			int[] array3 = new int[]
			{
				this.CompositionValues[0],
				this.CompositionValues[1],
				this.CompositionValues[2],
				this.CompositionValues[3]
			};
			int num = array.Count<bool>((bool s) => s);
			if (array[changedSliderIndex])
			{
				num--;
			}
			if (num > 0)
			{
				int num2 = ArmyCompositionGroupVM.SumOfValues(array2, array, -1);
				array[changedSliderIndex] = false;
				if (value >= num2)
				{
					value = num2;
				}
				int num3 = value - array2[changedSliderIndex];
				if (num3 != 0)
				{
					array3[changedSliderIndex] = value;
					int num4 = -num3;
					int num5 = num4 / num;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i])
						{
							array3[i] += num5;
							num4 -= num5;
						}
					}
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j] && array3[j] < 0)
						{
							num4 += array3[j];
							array3[j] = 0;
						}
					}
					if (num4 > 0)
					{
						while (num4 != 0)
						{
							int num6 = int.MaxValue;
							int num7 = -1;
							for (int k = 0; k < array.Length; k++)
							{
								if (array[k] && array3[k] < num6)
								{
									num6 = array3[k];
									num7 = k;
								}
							}
							array3[num7]++;
							num4--;
						}
					}
					else if (num4 < 0)
					{
						while (num4 != 0)
						{
							int num8 = int.MinValue;
							int num9 = -1;
							for (int l = 0; l < array.Length; l++)
							{
								if (array[l] && array3[l] > num8)
								{
									num8 = array3[l];
									num9 = l;
								}
							}
							array3[num9]--;
							num4++;
						}
					}
				}
			}
			this.SetArmyCompositionValue(0, array3[0], this.MeleeInfantryComposition);
			this.SetArmyCompositionValue(1, array3[1], this.RangedInfantryComposition);
			this.SetArmyCompositionValue(2, array3[2], this.MeleeCavalryComposition);
			this.SetArmyCompositionValue(3, array3[3], this.RangedCavalryComposition);
			this._updatingSliders = false;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000513F File Offset: 0x0000333F
		private void SetArmyCompositionValue(int index, int value, ArmyCompositionItemVM composition)
		{
			this.CompositionValues[index] = value;
			composition.RefreshCompositionValue();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00005150 File Offset: 0x00003350
		public void ExecuteRandomize(ArmyCompositionGroupVM oppositeSide = null)
		{
			if (oppositeSide == null)
			{
				this.ArmySize = MBRandom.RandomInt(this.MinArmySize, this.MaxArmySize);
			}
			else
			{
				float num = MBRandom.RandomFloatGaussian((float)(oppositeSide.ArmySize - oppositeSide.MinArmySize) / (float)(oppositeSide.MaxArmySize - oppositeSide.MinArmySize), 0.2f, 0f, 1f);
				this.ArmySize = MathF.Round(MathF.Lerp((float)this.MinArmySize, (float)this.MaxArmySize, num, 1E-05f));
			}
			int num2 = MBRandom.RandomInt(100);
			int num3 = MBRandom.RandomInt(100);
			int num4 = MBRandom.RandomInt(100);
			int num5 = MBRandom.RandomInt(100);
			int num6 = num2 + num3 + num4 + num5;
			int num7 = MathF.Round(100f * ((float)num2 / (float)num6));
			int num8 = MathF.Round(100f * ((float)num3 / (float)num6));
			int num9 = MathF.Round(100f * ((float)num4 / (float)num6));
			int num10 = 100 - (num7 + num8 + num9);
			this.MeleeInfantryComposition.ExecuteRandomize(num7);
			this.RangedInfantryComposition.ExecuteRandomize(num8);
			this.MeleeCavalryComposition.ExecuteRandomize(num9);
			this.RangedCavalryComposition.ExecuteRandomize(num10);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00005274 File Offset: 0x00003474
		public void OnPlayerTypeChange(CustomBattlePlayerType playerType)
		{
			this.MinArmySize = ((playerType == CustomBattlePlayerType.Commander) ? 1 : 2);
			this.ArmySize = (int)MathF.Clamp((float)this.ArmySize, (float)this.MinArmySize, (float)this.MaxArmySize);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000052A4 File Offset: 0x000034A4
		// (set) Token: 0x06000028 RID: 40 RVA: 0x000052AC File Offset: 0x000034AC
		[DataSourceProperty]
		public ArmyCompositionItemVM MeleeInfantryComposition
		{
			get
			{
				return this._meleeInfantryComposition;
			}
			set
			{
				if (value != this._meleeInfantryComposition)
				{
					this._meleeInfantryComposition = value;
					base.OnPropertyChangedWithValue<ArmyCompositionItemVM>(value, "MeleeInfantryComposition");
				}
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000052CA File Offset: 0x000034CA
		// (set) Token: 0x0600002A RID: 42 RVA: 0x000052D2 File Offset: 0x000034D2
		[DataSourceProperty]
		public ArmyCompositionItemVM RangedInfantryComposition
		{
			get
			{
				return this._rangedInfantryComposition;
			}
			set
			{
				if (value != this._rangedInfantryComposition)
				{
					this._rangedInfantryComposition = value;
					base.OnPropertyChangedWithValue<ArmyCompositionItemVM>(value, "RangedInfantryComposition");
				}
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000052F0 File Offset: 0x000034F0
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000052F8 File Offset: 0x000034F8
		[DataSourceProperty]
		public ArmyCompositionItemVM MeleeCavalryComposition
		{
			get
			{
				return this._meleeCavalryComposition;
			}
			set
			{
				if (value != this._meleeCavalryComposition)
				{
					this._meleeCavalryComposition = value;
					base.OnPropertyChangedWithValue<ArmyCompositionItemVM>(value, "MeleeCavalryComposition");
				}
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00005316 File Offset: 0x00003516
		// (set) Token: 0x0600002E RID: 46 RVA: 0x0000531E File Offset: 0x0000351E
		[DataSourceProperty]
		public ArmyCompositionItemVM RangedCavalryComposition
		{
			get
			{
				return this._rangedCavalryComposition;
			}
			set
			{
				if (value != this._rangedCavalryComposition)
				{
					this._rangedCavalryComposition = value;
					base.OnPropertyChangedWithValue<ArmyCompositionItemVM>(value, "RangedCavalryComposition");
				}
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002F RID: 47 RVA: 0x0000533C File Offset: 0x0000353C
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00005344 File Offset: 0x00003544
		[DataSourceProperty]
		public string ArmySizeTitle
		{
			get
			{
				return this._armySizeTitle;
			}
			set
			{
				if (value != this._armySizeTitle)
				{
					this._armySizeTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "ArmySizeTitle");
				}
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00005367 File Offset: 0x00003567
		// (set) Token: 0x06000032 RID: 50 RVA: 0x0000536F File Offset: 0x0000356F
		public int ArmySize
		{
			get
			{
				return this._armySize;
			}
			set
			{
				value = (int)MathF.Clamp((float)value, (float)this.MinArmySize, (float)this.MaxArmySize);
				if (this._armySize != value)
				{
					this._armySize = value;
					base.OnPropertyChangedWithValue(value, "ArmySize");
				}
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000053A5 File Offset: 0x000035A5
		// (set) Token: 0x06000034 RID: 52 RVA: 0x000053AD File Offset: 0x000035AD
		public int MaxArmySize
		{
			get
			{
				return this._maxArmySize;
			}
			set
			{
				if (this._maxArmySize != value)
				{
					this._maxArmySize = value;
					base.OnPropertyChangedWithValue(value, "MaxArmySize");
				}
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000053CB File Offset: 0x000035CB
		// (set) Token: 0x06000036 RID: 54 RVA: 0x000053D3 File Offset: 0x000035D3
		public int MinArmySize
		{
			get
			{
				return this._minArmySize;
			}
			set
			{
				if (this._minArmySize != value)
				{
					this._minArmySize = value;
					base.OnPropertyChangedWithValue(value, "MinArmySize");
				}
			}
		}

		// Token: 0x0400002D RID: 45
		public int[] CompositionValues;

		// Token: 0x0400002E RID: 46
		private bool _updatingSliders;

		// Token: 0x0400002F RID: 47
		private BasicCultureObject _selectedCulture;

		// Token: 0x04000030 RID: 48
		private readonly MBReadOnlyList<SkillObject> _allSkills = Game.Current.ObjectManager.GetObjectTypeList<SkillObject>();

		// Token: 0x04000031 RID: 49
		private readonly List<BasicCharacterObject> _allCharacterObjects = new List<BasicCharacterObject>();

		// Token: 0x04000032 RID: 50
		private ArmyCompositionItemVM _meleeInfantryComposition;

		// Token: 0x04000033 RID: 51
		private ArmyCompositionItemVM _rangedInfantryComposition;

		// Token: 0x04000034 RID: 52
		private ArmyCompositionItemVM _meleeCavalryComposition;

		// Token: 0x04000035 RID: 53
		private ArmyCompositionItemVM _rangedCavalryComposition;

		// Token: 0x04000036 RID: 54
		private int _armySize;

		// Token: 0x04000037 RID: 55
		private int _maxArmySize;

		// Token: 0x04000038 RID: 56
		private int _minArmySize;

		// Token: 0x04000039 RID: 57
		private string _armySizeTitle;
	}
}
