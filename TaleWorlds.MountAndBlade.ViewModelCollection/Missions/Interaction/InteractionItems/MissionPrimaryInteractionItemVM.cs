using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems
{
	// Token: 0x02000044 RID: 68
	public class MissionPrimaryInteractionItemVM : MissionGenericInteractionItemVM
	{
		// Token: 0x060005C2 RID: 1474 RVA: 0x00015A3E File Offset: 0x00013C3E
		protected override void OnResetData()
		{
			base.OnResetData();
			this.FocusTypeString = string.Empty;
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x00015A51 File Offset: 0x00013C51
		// (set) Token: 0x060005C4 RID: 1476 RVA: 0x00015A59 File Offset: 0x00013C59
		[DataSourceProperty]
		public string FocusTypeString
		{
			get
			{
				return this._focusTypeString;
			}
			set
			{
				if (value != this._focusTypeString)
				{
					this._focusTypeString = value;
					base.OnPropertyChangedWithValue<string>(value, "FocusTypeString");
				}
			}
		}

		// Token: 0x04000297 RID: 663
		private string _focusTypeString;
	}
}
