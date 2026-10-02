using System;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Nameplate
{
	// Token: 0x0200001D RID: 29
	public class SettlementNameplateEventItemVM : ViewModel
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x0000CBBD File Offset: 0x0000ADBD
		public SettlementNameplateEventItemVM(SettlementNameplateEventItemVM.SettlementEventType eventType)
		{
			this.EventType = eventType;
			this.Type = (int)eventType;
			this.AdditionalParameters = "";
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000CBDE File Offset: 0x0000ADDE
		public SettlementNameplateEventItemVM(string productionIconId = "")
		{
			this.EventType = SettlementNameplateEventItemVM.SettlementEventType.Production;
			this.Type = (int)this.EventType;
			this.AdditionalParameters = productionIconId;
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000CC00 File Offset: 0x0000AE00
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x0000CC08 File Offset: 0x0000AE08
		[DataSourceProperty]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != this._type)
				{
					this._type = value;
					base.OnPropertyChangedWithValue(value, "Type");
				}
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000CC26 File Offset: 0x0000AE26
		// (set) Token: 0x060002DA RID: 730 RVA: 0x0000CC2E File Offset: 0x0000AE2E
		[DataSourceProperty]
		public string AdditionalParameters
		{
			get
			{
				return this._additionalParameters;
			}
			set
			{
				if (value != this._additionalParameters)
				{
					this._additionalParameters = value;
					base.OnPropertyChangedWithValue<string>(value, "AdditionalParameters");
				}
			}
		}

		// Token: 0x04000166 RID: 358
		public readonly SettlementNameplateEventItemVM.SettlementEventType EventType;

		// Token: 0x04000167 RID: 359
		private int _type;

		// Token: 0x04000168 RID: 360
		private string _additionalParameters;

		// Token: 0x0200008C RID: 140
		public enum SettlementEventType
		{
			// Token: 0x040003BE RID: 958
			Tournament,
			// Token: 0x040003BF RID: 959
			AvailableIssue,
			// Token: 0x040003C0 RID: 960
			ActiveQuest,
			// Token: 0x040003C1 RID: 961
			ActiveStoryQuest,
			// Token: 0x040003C2 RID: 962
			TrackedIssue,
			// Token: 0x040003C3 RID: 963
			TrackedStoryQuest,
			// Token: 0x040003C4 RID: 964
			Production
		}
	}
}
