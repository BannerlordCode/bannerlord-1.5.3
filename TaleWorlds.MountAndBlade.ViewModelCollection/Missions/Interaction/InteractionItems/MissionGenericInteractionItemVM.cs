using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems
{
	// Token: 0x02000041 RID: 65
	public class MissionGenericInteractionItemVM : MissionInteractionItemBaseVM
	{
		// Token: 0x060005B3 RID: 1459 RVA: 0x0001592F File Offset: 0x00013B2F
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject messageTextObj = this._messageTextObj;
			base.Message = ((messageTextObj != null) ? messageTextObj.ToString() : null);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001594F File Offset: 0x00013B4F
		public void SetData(TextObject message, bool isDisabled = false)
		{
			this._messageTextObj = message;
			base.IsDisabled = isDisabled;
			this.RefreshValues();
			this.OnSetData(message, isDisabled);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001596D File Offset: 0x00013B6D
		public void ResetData()
		{
			this._messageTextObj = null;
			base.IsDisabled = false;
			base.Message = string.Empty;
			base.IsDisplayed = false;
			this.OnResetData();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00015995 File Offset: 0x00013B95
		protected virtual void OnSetData(TextObject message, bool isDisabled)
		{
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00015997 File Offset: 0x00013B97
		protected virtual void OnResetData()
		{
		}

		// Token: 0x04000292 RID: 658
		private TextObject _messageTextObj;
	}
}
