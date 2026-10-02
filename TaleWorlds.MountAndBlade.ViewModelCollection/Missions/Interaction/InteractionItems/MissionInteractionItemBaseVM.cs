using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems
{
	// Token: 0x02000043 RID: 67
	public abstract class MissionInteractionItemBaseVM : ViewModel
	{
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x000159CC File Offset: 0x00013BCC
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x000159D4 File Offset: 0x00013BD4
		public bool IsDisplayed { get; internal set; }

		// Token: 0x060005BC RID: 1468 RVA: 0x000159DD File Offset: 0x00013BDD
		public MissionInteractionItemBaseVM()
		{
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x000159E5 File Offset: 0x00013BE5
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x000159ED File Offset: 0x00013BED
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x00015A0B File Offset: 0x00013C0B
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x00015A13 File Offset: 0x00013C13
		[DataSourceProperty]
		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				if (value != this._message)
				{
					this._message = value;
					base.OnPropertyChangedWithValue<string>(value, "Message");
				}
			}
		}

		// Token: 0x04000295 RID: 661
		private bool _isDisabled;

		// Token: 0x04000296 RID: 662
		private string _message;
	}
}
