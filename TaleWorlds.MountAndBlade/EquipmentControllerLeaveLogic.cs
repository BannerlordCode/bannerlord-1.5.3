using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200028A RID: 650
	public class EquipmentControllerLeaveLogic : MissionLogic
	{
		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x0008321F File Offset: 0x0008141F
		// (set) Token: 0x0600246D RID: 9325 RVA: 0x00083227 File Offset: 0x00081427
		public bool IsEquipmentSelectionActive { get; private set; }

		// Token: 0x0600246E RID: 9326 RVA: 0x00083230 File Offset: 0x00081430
		public void SetIsEquipmentSelectionActive(bool isActive)
		{
			this.IsEquipmentSelectionActive = isActive;
			Debug.Print("IsEquipmentSelectionActive: " + isActive.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x0008325B File Offset: 0x0008145B
		public override InquiryData OnEndMissionRequest(out bool canLeave)
		{
			canLeave = !this.IsEquipmentSelectionActive;
			return null;
		}
	}
}
