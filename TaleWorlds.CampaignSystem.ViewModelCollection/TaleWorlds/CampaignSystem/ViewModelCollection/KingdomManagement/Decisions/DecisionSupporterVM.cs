using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions
{
	// Token: 0x0200007D RID: 125
	public class DecisionSupporterVM : ViewModel
	{
		// Token: 0x060009E1 RID: 2529 RVA: 0x0002B4CC File Offset: 0x000296CC
		public DecisionSupporterVM(TextObject name, string imagePath, Clan clan, Supporter.SupportWeights weight)
		{
			this._nameObj = name;
			this._clan = clan;
			this._weight = weight;
			this.SupportWeightImagePath = DecisionSupporterVM.GetSupporterWeightImagePath(weight);
			this.RefreshValues();
			this._hero = Hero.FindFirst((Hero H) => H.Name == name);
			if (this._hero != null)
			{
				this.Visual = new CharacterImageIdentifierVM(CampaignUIHelper.GetCharacterCode(this._hero.CharacterObject, false));
				return;
			}
			this.Visual = new CharacterImageIdentifierVM(null);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0002B562 File Offset: 0x00029762
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._nameObj.ToString();
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0002B57B File Offset: 0x0002977B
		private void ExecuteBeginHint()
		{
			if (this._hero != null)
			{
				InformationManager.ShowTooltip(typeof(Hero), new object[] { this._hero, false });
			}
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0002B5AC File Offset: 0x000297AC
		private void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0002B5B3 File Offset: 0x000297B3
		internal static string GetSupporterWeightImagePath(Supporter.SupportWeights weight)
		{
			switch (weight)
			{
			case Supporter.SupportWeights.SlightlyFavor:
				return "SPKingdom\\voter_strength1";
			case Supporter.SupportWeights.StronglyFavor:
				return "SPKingdom\\voter_strength2";
			case Supporter.SupportWeights.FullyPush:
				return "SPKingdom\\voter_strength3";
			}
			return string.Empty;
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0002B5E8 File Offset: 0x000297E8
		// (set) Token: 0x060009E7 RID: 2535 RVA: 0x0002B5F0 File Offset: 0x000297F0
		[DataSourceProperty]
		public CharacterImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0002B60E File Offset: 0x0002980E
		// (set) Token: 0x060009E9 RID: 2537 RVA: 0x0002B616 File Offset: 0x00029816
		[DataSourceProperty]
		public int SupportStrength
		{
			get
			{
				return this._supportStrength;
			}
			set
			{
				if (value != this._supportStrength)
				{
					this._supportStrength = value;
					base.OnPropertyChangedWithValue(value, "SupportStrength");
				}
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0002B634 File Offset: 0x00029834
		// (set) Token: 0x060009EB RID: 2539 RVA: 0x0002B63C File Offset: 0x0002983C
		[DataSourceProperty]
		public string SupportWeightImagePath
		{
			get
			{
				return this._supportWeightImagePath;
			}
			set
			{
				if (value != this._supportWeightImagePath)
				{
					this._supportWeightImagePath = value;
					base.OnPropertyChangedWithValue<string>(value, "SupportWeightImagePath");
				}
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x0002B65F File Offset: 0x0002985F
		// (set) Token: 0x060009ED RID: 2541 RVA: 0x0002B667 File Offset: 0x00029867
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChanged("string");
				}
			}
		}

		// Token: 0x04000458 RID: 1112
		private Supporter.SupportWeights _weight;

		// Token: 0x04000459 RID: 1113
		private Clan _clan;

		// Token: 0x0400045A RID: 1114
		private TextObject _nameObj;

		// Token: 0x0400045B RID: 1115
		private Hero _hero;

		// Token: 0x0400045C RID: 1116
		private CharacterImageIdentifierVM _visual;

		// Token: 0x0400045D RID: 1117
		private string _name;

		// Token: 0x0400045E RID: 1118
		private int _supportStrength;

		// Token: 0x0400045F RID: 1119
		private string _supportWeightImagePath;
	}
}
