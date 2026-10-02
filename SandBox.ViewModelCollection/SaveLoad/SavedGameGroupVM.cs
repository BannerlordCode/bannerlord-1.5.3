using System;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.SaveLoad
{
	// Token: 0x02000013 RID: 19
	public class SavedGameGroupVM : ViewModel
	{
		// Token: 0x0600019E RID: 414 RVA: 0x000081CA File Offset: 0x000063CA
		public SavedGameGroupVM()
		{
			this.SavedGamesList = new MBBindingList<SavedGameVM>();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000081DD File Offset: 0x000063DD
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SavedGamesList.ApplyActionOnAllItems(delegate(SavedGameVM s)
			{
				s.RefreshValues();
			});
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000820F File Offset: 0x0000640F
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00008217 File Offset: 0x00006417
		[DataSourceProperty]
		public bool IsFilteredOut
		{
			get
			{
				return this._isFilteredOut;
			}
			set
			{
				if (value != this._isFilteredOut)
				{
					this._isFilteredOut = value;
					base.OnPropertyChangedWithValue(value, "IsFilteredOut");
				}
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00008235 File Offset: 0x00006435
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x0000823D File Offset: 0x0000643D
		[DataSourceProperty]
		public MBBindingList<SavedGameVM> SavedGamesList
		{
			get
			{
				return this._savedGamesList;
			}
			set
			{
				if (value != this._savedGamesList)
				{
					this._savedGamesList = value;
					base.OnPropertyChangedWithValue<MBBindingList<SavedGameVM>>(value, "SavedGamesList");
				}
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000825B File Offset: 0x0000645B
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00008263 File Offset: 0x00006463
		[DataSourceProperty]
		public string IdentifierID
		{
			get
			{
				return this._identifierID;
			}
			set
			{
				if (value != this._identifierID)
				{
					this._identifierID = value;
					base.OnPropertyChangedWithValue<string>(value, "IdentifierID");
				}
			}
		}

		// Token: 0x040000B4 RID: 180
		private bool _isFilteredOut;

		// Token: 0x040000B5 RID: 181
		private MBBindingList<SavedGameVM> _savedGamesList;

		// Token: 0x040000B6 RID: 182
		private string _identifierID;
	}
}
