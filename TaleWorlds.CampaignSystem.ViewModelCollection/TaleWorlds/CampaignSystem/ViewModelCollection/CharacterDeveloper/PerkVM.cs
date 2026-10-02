using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper
{
	// Token: 0x0200014A RID: 330
	public class PerkVM : ViewModel
	{
		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x00073E8D File Offset: 0x0007208D
		private bool _hasAlternativeAndSelected
		{
			get
			{
				return this.AlternativeType != 0 && this._getIsPerkSelected(this.Perk.AlternativePerk);
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x0600201E RID: 8222 RVA: 0x00073EAF File Offset: 0x000720AF
		// (set) Token: 0x0600201F RID: 8223 RVA: 0x00073EB7 File Offset: 0x000720B7
		public PerkVM.PerkStates CurrentState
		{
			get
			{
				return this._currentState;
			}
			private set
			{
				if (value != this._currentState)
				{
					this._currentState = value;
					this.PerkState = (int)value;
				}
			}
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x00073ED0 File Offset: 0x000720D0
		public PerkVM(PerkObject perk, bool isAvailable, PerkVM.PerkAlternativeType alternativeType, Action<PerkVM> onStartSelection, Action<PerkVM> onSelectionOver, Func<PerkObject, bool> getIsPerkSelected, Func<PerkObject, bool> getIsPreviousPerkSelected)
		{
			PerkVM <>4__this = this;
			this.AlternativeType = (int)alternativeType;
			this.Perk = perk;
			this._onStartSelection = onStartSelection;
			this._onSelectionOver = onSelectionOver;
			this._getIsPerkSelected = getIsPerkSelected;
			this._getIsPreviousPerkSelected = getIsPreviousPerkSelected;
			this._isAvailable = isAvailable;
			this.PerkId = "SPPerks\\" + perk.StringId;
			this.Level = (int)perk.RequiredSkillValue;
			this.LevelText = ((int)perk.RequiredSkillValue).ToString();
			this.Hint = new BasicTooltipViewModel(() => CampaignUIHelper.GetPerkEffectText(perk, <>4__this._getIsPerkSelected(<>4__this.Perk)));
			this._perkConceptObj = Concept.All.SingleOrDefault<Concept>((Concept c) => c.StringId == "str_game_objects_perks");
			this.RefreshState();
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x00073FD4 File Offset: 0x000721D4
		public void RefreshState()
		{
			bool flag = this._getIsPerkSelected(this.Perk);
			if (!this._isAvailable)
			{
				this.CurrentState = PerkVM.PerkStates.NotEarned;
				return;
			}
			if (flag)
			{
				this.CurrentState = PerkVM.PerkStates.EarnedAndActive;
				return;
			}
			if (this.Perk.AlternativePerk != null && this._getIsPerkSelected(this.Perk.AlternativePerk))
			{
				this.CurrentState = PerkVM.PerkStates.EarnedAndNotActive;
				return;
			}
			if (this._getIsPreviousPerkSelected(this.Perk))
			{
				this.CurrentState = PerkVM.PerkStates.EarnedButNotSelected;
				return;
			}
			this.CurrentState = PerkVM.PerkStates.EarnedPreviousPerkNotSelected;
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x0007405D File Offset: 0x0007225D
		public void ExecuteShowPerkConcept()
		{
			if (this._perkConceptObj != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._perkConceptObj.EncyclopediaLink);
				return;
			}
			Debug.FailedAssert("Couldn't find Perks encyclopedia page", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\CharacterDeveloper\\PerkVM.cs", "ExecuteShowPerkConcept", 127);
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x00074098 File Offset: 0x00072298
		public void ExecuteStartSelection()
		{
			if (this._isAvailable && !this._getIsPerkSelected(this.Perk) && !this._hasAlternativeAndSelected && this._getIsPreviousPerkSelected(this.Perk))
			{
				this._onStartSelection(this);
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06002024 RID: 8228 RVA: 0x000740E7 File Offset: 0x000722E7
		// (set) Token: 0x06002025 RID: 8229 RVA: 0x000740EF File Offset: 0x000722EF
		[DataSourceProperty]
		public bool IsTutorialHighlightEnabled
		{
			get
			{
				return this._isTutorialHighlightEnabled;
			}
			set
			{
				if (value != this._isTutorialHighlightEnabled)
				{
					this._isTutorialHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsTutorialHighlightEnabled");
				}
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x0007410D File Offset: 0x0007230D
		// (set) Token: 0x06002027 RID: 8231 RVA: 0x00074115 File Offset: 0x00072315
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06002028 RID: 8232 RVA: 0x00074133 File Offset: 0x00072333
		// (set) Token: 0x06002029 RID: 8233 RVA: 0x0007413B File Offset: 0x0007233B
		[DataSourceProperty]
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (value != this._level)
				{
					this._level = value;
					base.OnPropertyChangedWithValue(value, "Level");
				}
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x0600202A RID: 8234 RVA: 0x00074159 File Offset: 0x00072359
		// (set) Token: 0x0600202B RID: 8235 RVA: 0x00074161 File Offset: 0x00072361
		[DataSourceProperty]
		public int PerkState
		{
			get
			{
				return this._perkState;
			}
			set
			{
				if (value != this._perkState)
				{
					this._perkState = value;
					base.OnPropertyChangedWithValue(value, "PerkState");
				}
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x0600202C RID: 8236 RVA: 0x0007417F File Offset: 0x0007237F
		// (set) Token: 0x0600202D RID: 8237 RVA: 0x00074187 File Offset: 0x00072387
		[DataSourceProperty]
		public int AlternativeType
		{
			get
			{
				return this._alternativeType;
			}
			set
			{
				if (value != this._alternativeType)
				{
					this._alternativeType = value;
					base.OnPropertyChangedWithValue(value, "AlternativeType");
				}
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x0600202E RID: 8238 RVA: 0x000741A5 File Offset: 0x000723A5
		// (set) Token: 0x0600202F RID: 8239 RVA: 0x000741AD File Offset: 0x000723AD
		[DataSourceProperty]
		public string LevelText
		{
			get
			{
				return this._levelText;
			}
			set
			{
				if (value != this._levelText)
				{
					this._levelText = value;
					base.OnPropertyChangedWithValue<string>(value, "LevelText");
				}
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06002030 RID: 8240 RVA: 0x000741D0 File Offset: 0x000723D0
		// (set) Token: 0x06002031 RID: 8241 RVA: 0x000741D8 File Offset: 0x000723D8
		[DataSourceProperty]
		public string BackgroundImage
		{
			get
			{
				return this._backgroundImage;
			}
			set
			{
				if (value != this._backgroundImage)
				{
					this._backgroundImage = value;
					base.OnPropertyChangedWithValue<string>(value, "BackgroundImage");
				}
			}
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06002032 RID: 8242 RVA: 0x000741FB File Offset: 0x000723FB
		// (set) Token: 0x06002033 RID: 8243 RVA: 0x00074203 File Offset: 0x00072403
		[DataSourceProperty]
		public string PerkId
		{
			get
			{
				return this._perkId;
			}
			set
			{
				if (value != this._perkId)
				{
					this._perkId = value;
					base.OnPropertyChangedWithValue<string>(value, "PerkId");
				}
			}
		}

		// Token: 0x04000EB3 RID: 3763
		public readonly PerkObject Perk;

		// Token: 0x04000EB4 RID: 3764
		private readonly Action<PerkVM> _onStartSelection;

		// Token: 0x04000EB5 RID: 3765
		private readonly Action<PerkVM> _onSelectionOver;

		// Token: 0x04000EB6 RID: 3766
		private readonly Func<PerkObject, bool> _getIsPerkSelected;

		// Token: 0x04000EB7 RID: 3767
		private readonly Func<PerkObject, bool> _getIsPreviousPerkSelected;

		// Token: 0x04000EB8 RID: 3768
		private readonly bool _isAvailable;

		// Token: 0x04000EB9 RID: 3769
		private readonly Concept _perkConceptObj;

		// Token: 0x04000EBA RID: 3770
		private PerkVM.PerkStates _currentState = PerkVM.PerkStates.None;

		// Token: 0x04000EBB RID: 3771
		private string _levelText;

		// Token: 0x04000EBC RID: 3772
		private string _perkId;

		// Token: 0x04000EBD RID: 3773
		private string _backgroundImage;

		// Token: 0x04000EBE RID: 3774
		private BasicTooltipViewModel _hint;

		// Token: 0x04000EBF RID: 3775
		private int _level;

		// Token: 0x04000EC0 RID: 3776
		private int _alternativeType;

		// Token: 0x04000EC1 RID: 3777
		private int _perkState = -1;

		// Token: 0x04000EC2 RID: 3778
		private bool _isTutorialHighlightEnabled;

		// Token: 0x020002DA RID: 730
		public enum PerkStates
		{
			// Token: 0x04001402 RID: 5122
			None = -1,
			// Token: 0x04001403 RID: 5123
			NotEarned,
			// Token: 0x04001404 RID: 5124
			EarnedButNotSelected,
			// Token: 0x04001405 RID: 5125
			EarnedAndActive,
			// Token: 0x04001406 RID: 5126
			EarnedAndNotActive,
			// Token: 0x04001407 RID: 5127
			EarnedPreviousPerkNotSelected
		}

		// Token: 0x020002DB RID: 731
		public enum PerkAlternativeType
		{
			// Token: 0x04001409 RID: 5129
			NoAlternative,
			// Token: 0x0400140A RID: 5130
			FirstAlternative,
			// Token: 0x0400140B RID: 5131
			SecondAlternative
		}
	}
}
