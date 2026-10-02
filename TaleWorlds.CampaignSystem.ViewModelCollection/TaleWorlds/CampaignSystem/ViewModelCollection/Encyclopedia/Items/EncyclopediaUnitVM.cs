using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000F5 RID: 245
	public class EncyclopediaUnitVM : ViewModel
	{
		// Token: 0x06001618 RID: 5656 RVA: 0x00056C40 File Offset: 0x00054E40
		public EncyclopediaUnitVM(CharacterObject character, bool isActive)
		{
			if (character != null)
			{
				CharacterCode characterCode = CharacterCode.CreateFrom(character);
				this.ImageIdentifier = new CharacterImageIdentifierVM(characterCode);
				this._character = character;
				this.IsActiveUnit = isActive;
				this.TierIconData = CampaignUIHelper.GetCharacterTierData(character, true);
				this.TypeIconData = CampaignUIHelper.GetCharacterTypeData(character, true);
			}
			else
			{
				this.IsActiveUnit = false;
			}
			this.RefreshValues();
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x00056CA0 File Offset: 0x00054EA0
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._character != null)
			{
				this.NameText = this._character.Name.ToString();
			}
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x00056CC6 File Offset: 0x00054EC6
		public void ExecuteLink()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this._character.EncyclopediaLink);
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x00056CE2 File Offset: 0x00054EE2
		public virtual void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(typeof(CharacterObject), new object[] { this._character });
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x00056D02 File Offset: 0x00054F02
		public virtual void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x0600161D RID: 5661 RVA: 0x00056D09 File Offset: 0x00054F09
		// (set) Token: 0x0600161E RID: 5662 RVA: 0x00056D11 File Offset: 0x00054F11
		[DataSourceProperty]
		public bool IsActiveUnit
		{
			get
			{
				return this._isActiveUnit;
			}
			set
			{
				if (value != this._isActiveUnit)
				{
					this._isActiveUnit = value;
					base.OnPropertyChangedWithValue(value, "IsActiveUnit");
				}
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x0600161F RID: 5663 RVA: 0x00056D2F File Offset: 0x00054F2F
		// (set) Token: 0x06001620 RID: 5664 RVA: 0x00056D37 File Offset: 0x00054F37
		[DataSourceProperty]
		public CharacterImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06001621 RID: 5665 RVA: 0x00056D55 File Offset: 0x00054F55
		// (set) Token: 0x06001622 RID: 5666 RVA: 0x00056D5D File Offset: 0x00054F5D
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

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06001623 RID: 5667 RVA: 0x00056D80 File Offset: 0x00054F80
		// (set) Token: 0x06001624 RID: 5668 RVA: 0x00056D88 File Offset: 0x00054F88
		[DataSourceProperty]
		public StringItemWithHintVM TierIconData
		{
			get
			{
				return this._tierIconData;
			}
			set
			{
				if (value != this._tierIconData)
				{
					this._tierIconData = value;
					base.OnPropertyChangedWithValue<StringItemWithHintVM>(value, "TierIconData");
				}
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06001625 RID: 5669 RVA: 0x00056DA6 File Offset: 0x00054FA6
		// (set) Token: 0x06001626 RID: 5670 RVA: 0x00056DAE File Offset: 0x00054FAE
		[DataSourceProperty]
		public StringItemWithHintVM TypeIconData
		{
			get
			{
				return this._typeIconData;
			}
			set
			{
				if (value != this._typeIconData)
				{
					this._typeIconData = value;
					base.OnPropertyChangedWithValue<StringItemWithHintVM>(value, "TypeIconData");
				}
			}
		}

		// Token: 0x040009FE RID: 2558
		private CharacterObject _character;

		// Token: 0x040009FF RID: 2559
		private CharacterImageIdentifierVM _imageIdentifier;

		// Token: 0x04000A00 RID: 2560
		private string _nameText;

		// Token: 0x04000A01 RID: 2561
		private bool _isActiveUnit;

		// Token: 0x04000A02 RID: 2562
		private StringItemWithHintVM _tierIconData;

		// Token: 0x04000A03 RID: 2563
		private StringItemWithHintVM _typeIconData;
	}
}
