using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement
{
	// Token: 0x02000161 RID: 353
	public class ArmyManagementBoostEventVM : ViewModel
	{
		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06002232 RID: 8754 RVA: 0x0007A22F File Offset: 0x0007842F
		public ArmyManagementBoostEventVM.BoostCurrency CurrencyToPayForCohesion { get; }

		// Token: 0x06002233 RID: 8755 RVA: 0x0007A237 File Offset: 0x00078437
		public ArmyManagementBoostEventVM(ArmyManagementBoostEventVM.BoostCurrency currencyToPayForCohesion, int amountToPay, int amountOfCohesionToGain, Action<ArmyManagementBoostEventVM> onExecuteEvent)
		{
			this.IsEnabled = true;
			this._onExecuteEvent = onExecuteEvent;
			this.AmountToPay = amountToPay;
			this.AmountOfCohesionToGain = amountOfCohesionToGain;
			this.CurrencyToPayForCohesion = currencyToPayForCohesion;
			this.CurrencyType = (int)currencyToPayForCohesion;
			this.RefreshValues();
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x0007A270 File Offset: 0x00078470
		public override void RefreshValues()
		{
			base.RefreshValues();
			GameTexts.SetVariable("AMOUNT", this.AmountToPay);
			this.SpendText = GameTexts.FindText("str_cohesion_boost_spend", null).ToString();
			GameTexts.SetVariable("GAIN_AMOUNT", this.AmountOfCohesionToGain);
			this.GainText = GameTexts.FindText("str_cohesion_boost_gain", null).ToString();
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x0007A2CF File Offset: 0x000784CF
		private void ExecuteEvent()
		{
			this._onExecuteEvent(this);
		}

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06002236 RID: 8758 RVA: 0x0007A2DD File Offset: 0x000784DD
		// (set) Token: 0x06002237 RID: 8759 RVA: 0x0007A2E5 File Offset: 0x000784E5
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

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06002238 RID: 8760 RVA: 0x0007A303 File Offset: 0x00078503
		// (set) Token: 0x06002239 RID: 8761 RVA: 0x0007A30B File Offset: 0x0007850B
		[DataSourceProperty]
		public int AmountToPay
		{
			get
			{
				return this._amountToPay;
			}
			set
			{
				if (value != this._amountToPay)
				{
					this._amountToPay = value;
					base.OnPropertyChangedWithValue(value, "AmountToPay");
				}
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x0600223A RID: 8762 RVA: 0x0007A329 File Offset: 0x00078529
		// (set) Token: 0x0600223B RID: 8763 RVA: 0x0007A331 File Offset: 0x00078531
		[DataSourceProperty]
		public int CurrencyType
		{
			get
			{
				return this._currencyType;
			}
			set
			{
				if (value != this._currencyType)
				{
					this._currencyType = value;
					base.OnPropertyChangedWithValue(value, "CurrencyType");
				}
			}
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x0600223C RID: 8764 RVA: 0x0007A34F File Offset: 0x0007854F
		// (set) Token: 0x0600223D RID: 8765 RVA: 0x0007A357 File Offset: 0x00078557
		[DataSourceProperty]
		public int AmountOfCohesionToGain
		{
			get
			{
				return this._amountOfCohesionToGain;
			}
			set
			{
				if (value != this._amountOfCohesionToGain)
				{
					this._amountOfCohesionToGain = value;
					base.OnPropertyChangedWithValue(value, "AmountOfCohesionToGain");
				}
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x0600223E RID: 8766 RVA: 0x0007A375 File Offset: 0x00078575
		// (set) Token: 0x0600223F RID: 8767 RVA: 0x0007A37D File Offset: 0x0007857D
		[DataSourceProperty]
		public string SpendText
		{
			get
			{
				return this._spendText;
			}
			set
			{
				if (value != this._spendText)
				{
					this._spendText = value;
					base.OnPropertyChangedWithValue<string>(value, "SpendText");
				}
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06002240 RID: 8768 RVA: 0x0007A3A0 File Offset: 0x000785A0
		// (set) Token: 0x06002241 RID: 8769 RVA: 0x0007A3A8 File Offset: 0x000785A8
		[DataSourceProperty]
		public string GainText
		{
			get
			{
				return this._gainText;
			}
			set
			{
				if (value != this._gainText)
				{
					this._gainText = value;
					base.OnPropertyChangedWithValue<string>(value, "GainText");
				}
			}
		}

		// Token: 0x04000F9D RID: 3997
		private readonly Action<ArmyManagementBoostEventVM> _onExecuteEvent;

		// Token: 0x04000F9E RID: 3998
		private int _amountToPay;

		// Token: 0x04000F9F RID: 3999
		private int _amountOfCohesionToGain;

		// Token: 0x04000FA0 RID: 4000
		private int _currencyType;

		// Token: 0x04000FA1 RID: 4001
		private string _spendText;

		// Token: 0x04000FA2 RID: 4002
		private string _gainText;

		// Token: 0x04000FA3 RID: 4003
		private bool _isEnabled;

		// Token: 0x020002F3 RID: 755
		public enum BoostCurrency
		{
			// Token: 0x04001450 RID: 5200
			Gold,
			// Token: 0x04001451 RID: 5201
			Influence
		}
	}
}
