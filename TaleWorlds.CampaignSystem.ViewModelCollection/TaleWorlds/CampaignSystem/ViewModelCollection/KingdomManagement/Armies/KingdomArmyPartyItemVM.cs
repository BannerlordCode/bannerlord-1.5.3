using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Armies
{
	// Token: 0x02000090 RID: 144
	public class KingdomArmyPartyItemVM : ViewModel
	{
		// Token: 0x06000BF0 RID: 3056 RVA: 0x00031E23 File Offset: 0x00030023
		public KingdomArmyPartyItemVM(MobileParty party)
		{
			this._party = party;
			Hero leaderHero = party.LeaderHero;
			this.Visual = new CharacterImageIdentifierVM(CampaignUIHelper.GetCharacterCode((leaderHero != null) ? leaderHero.CharacterObject : null, false));
			this.RefreshValues();
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00031E5B File Offset: 0x0003005B
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._party.Name.ToString();
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00031E79 File Offset: 0x00030079
		private void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(typeof(MobileParty), new object[] { this._party, true, false });
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00031EAB File Offset: 0x000300AB
		private void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00031EB2 File Offset: 0x000300B2
		public void ExecuteLink()
		{
			if (this._party != null && this._party.LeaderHero != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._party.LeaderHero.EncyclopediaLink);
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x00031EE8 File Offset: 0x000300E8
		// (set) Token: 0x06000BF6 RID: 3062 RVA: 0x00031EF0 File Offset: 0x000300F0
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

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x00031F0E File Offset: 0x0003010E
		// (set) Token: 0x06000BF8 RID: 3064 RVA: 0x00031F16 File Offset: 0x00030116
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
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x04000548 RID: 1352
		private MobileParty _party;

		// Token: 0x04000549 RID: 1353
		private CharacterImageIdentifierVM _visual;

		// Token: 0x0400054A RID: 1354
		private string _name;
	}
}
