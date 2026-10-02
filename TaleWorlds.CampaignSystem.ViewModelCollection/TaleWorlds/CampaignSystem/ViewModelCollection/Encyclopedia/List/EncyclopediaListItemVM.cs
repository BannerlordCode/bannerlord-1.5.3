using System;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List
{
	// Token: 0x020000E3 RID: 227
	public class EncyclopediaListItemVM : ViewModel
	{
		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x0600155F RID: 5471 RVA: 0x00054EF6 File Offset: 0x000530F6
		// (set) Token: 0x06001560 RID: 5472 RVA: 0x00054EFE File Offset: 0x000530FE
		public object Object { get; private set; }

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06001561 RID: 5473 RVA: 0x00054F07 File Offset: 0x00053107
		public EncyclopediaListItem ListItem { get; }

		// Token: 0x06001562 RID: 5474 RVA: 0x00054F10 File Offset: 0x00053110
		public EncyclopediaListItemVM(EncyclopediaListItem listItem)
		{
			this.Object = listItem.Object;
			this.Id = listItem.Id;
			this._type = listItem.TypeName;
			this.ListItem = listItem;
			this.PlayerCanSeeValues = listItem.PlayerCanSeeValues;
			this._onShowTooltip = listItem.OnShowTooltip;
			this.RefreshValues();
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x00054F6C File Offset: 0x0005316C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.ListItem.Name;
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x00054F85 File Offset: 0x00053185
		public void Execute()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this._type, this.Id);
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x00054FA2 File Offset: 0x000531A2
		public void SetComparedValue(EncyclopediaListItemComparerBase comparer)
		{
			this.ComparedValue = comparer.GetComparedValueText(this.ListItem);
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x00054FB6 File Offset: 0x000531B6
		public void ExecuteBeginTooltip()
		{
			Action onShowTooltip = this._onShowTooltip;
			if (onShowTooltip == null)
			{
				return;
			}
			onShowTooltip();
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x00054FC8 File Offset: 0x000531C8
		public void ExecuteEndTooltip()
		{
			if (this._onShowTooltip != null)
			{
				MBInformationManager.HideInformations();
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06001568 RID: 5480 RVA: 0x00054FD7 File Offset: 0x000531D7
		// (set) Token: 0x06001569 RID: 5481 RVA: 0x00054FDF File Offset: 0x000531DF
		[DataSourceProperty]
		public bool IsFiltered
		{
			get
			{
				return this._isFiltered;
			}
			set
			{
				if (value != this._isFiltered)
				{
					this._isFiltered = value;
					base.OnPropertyChangedWithValue(value, "IsFiltered");
				}
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x0600156A RID: 5482 RVA: 0x00054FFD File Offset: 0x000531FD
		// (set) Token: 0x0600156B RID: 5483 RVA: 0x00055005 File Offset: 0x00053205
		[DataSourceProperty]
		public bool PlayerCanSeeValues
		{
			get
			{
				return this._playerCanSeeValues;
			}
			set
			{
				if (value != this._playerCanSeeValues)
				{
					this._playerCanSeeValues = value;
					base.OnPropertyChangedWithValue(value, "PlayerCanSeeValues");
				}
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x0600156C RID: 5484 RVA: 0x00055023 File Offset: 0x00053223
		// (set) Token: 0x0600156D RID: 5485 RVA: 0x0005502B File Offset: 0x0005322B
		[DataSourceProperty]
		public string Id
		{
			get
			{
				return this._id;
			}
			set
			{
				if (value != this._id)
				{
					this._id = value;
					base.OnPropertyChangedWithValue<string>(value, "Id");
				}
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x0600156E RID: 5486 RVA: 0x0005504E File Offset: 0x0005324E
		// (set) Token: 0x0600156F RID: 5487 RVA: 0x00055056 File Offset: 0x00053256
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

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06001570 RID: 5488 RVA: 0x00055079 File Offset: 0x00053279
		// (set) Token: 0x06001571 RID: 5489 RVA: 0x00055081 File Offset: 0x00053281
		[DataSourceProperty]
		public string ComparedValue
		{
			get
			{
				return this._comparedValue;
			}
			set
			{
				if (value != this._comparedValue)
				{
					this._comparedValue = value;
					base.OnPropertyChangedWithValue<string>(value, "ComparedValue");
				}
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x000550A4 File Offset: 0x000532A4
		// (set) Token: 0x06001573 RID: 5491 RVA: 0x000550AC File Offset: 0x000532AC
		[DataSourceProperty]
		public bool IsBookmarked
		{
			get
			{
				return this._isBookmarked;
			}
			set
			{
				if (value != this._isBookmarked)
				{
					this._isBookmarked = value;
					base.OnPropertyChangedWithValue(value, "IsBookmarked");
				}
			}
		}

		// Token: 0x040009B4 RID: 2484
		private readonly string _type;

		// Token: 0x040009B5 RID: 2485
		private readonly Action _onShowTooltip;

		// Token: 0x040009B6 RID: 2486
		private string _id;

		// Token: 0x040009B7 RID: 2487
		private string _name;

		// Token: 0x040009B8 RID: 2488
		private string _comparedValue;

		// Token: 0x040009B9 RID: 2489
		private bool _isFiltered;

		// Token: 0x040009BA RID: 2490
		private bool _isBookmarked;

		// Token: 0x040009BB RID: 2491
		private bool _playerCanSeeValues;
	}
}
