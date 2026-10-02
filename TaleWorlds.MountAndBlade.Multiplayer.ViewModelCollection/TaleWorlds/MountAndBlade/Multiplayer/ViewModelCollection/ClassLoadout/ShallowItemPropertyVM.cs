using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000AE RID: 174
	public class ShallowItemPropertyVM : ViewModel
	{
		// Token: 0x060010CA RID: 4298 RVA: 0x0003487F File Offset: 0x00032A7F
		public ShallowItemPropertyVM(TextObject propertyName, int permille, int value)
		{
			this._propertyName = propertyName;
			this.Permille = permille;
			this.Value = value;
			this.RefreshValues();
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x000348A2 File Offset: 0x00032AA2
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameText = this._propertyName.ToString();
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x000348BB File Offset: 0x00032ABB
		// (set) Token: 0x060010CD RID: 4301 RVA: 0x000348C3 File Offset: 0x00032AC3
		[DataSourceProperty]
		public int Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue(value, "Value");
				}
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x060010CE RID: 4302 RVA: 0x000348E1 File Offset: 0x00032AE1
		// (set) Token: 0x060010CF RID: 4303 RVA: 0x000348E9 File Offset: 0x00032AE9
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

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x0003490C File Offset: 0x00032B0C
		// (set) Token: 0x060010D1 RID: 4305 RVA: 0x00034914 File Offset: 0x00032B14
		[DataSourceProperty]
		public int Permille
		{
			get
			{
				return this._permille;
			}
			set
			{
				if (value != this._permille)
				{
					this._permille = value;
					base.OnPropertyChangedWithValue(value, "Permille");
				}
			}
		}

		// Token: 0x040007E5 RID: 2021
		private readonly TextObject _propertyName;

		// Token: 0x040007E6 RID: 2022
		private string _nameText;

		// Token: 0x040007E7 RID: 2023
		private int _permille;

		// Token: 0x040007E8 RID: 2024
		private int _value;
	}
}
