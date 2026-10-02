using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200027B RID: 635
	public class BasicLeaveMissionLogic : MissionLogic
	{
		// Token: 0x060023BF RID: 9151 RVA: 0x0007F3AB File Offset: 0x0007D5AB
		public BasicLeaveMissionLogic()
			: this(false)
		{
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x0007F3B4 File Offset: 0x0007D5B4
		public BasicLeaveMissionLogic(bool askBeforeLeave)
			: this(askBeforeLeave, 5)
		{
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x0007F3BE File Offset: 0x0007D5BE
		public BasicLeaveMissionLogic(bool askBeforeLeave, int minRetreatDistance)
		{
			this._askBeforeLeave = askBeforeLeave;
			this._minRetreatDistance = minRetreatDistance;
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x0007F3D4 File Offset: 0x0007D5D4
		public override bool MissionEnded(ref MissionResult missionResult)
		{
			return base.Mission.MainAgent != null && !base.Mission.MainAgent.IsActive();
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x0007F3F8 File Offset: 0x0007D5F8
		public override InquiryData OnEndMissionRequest(out bool canPlayerLeave)
		{
			canPlayerLeave = true;
			if (base.Mission.MainAgent != null && base.Mission.MainAgent.IsActive() && (float)this._minRetreatDistance > 0f && base.Mission.IsPlayerCloseToAnEnemy((float)this._minRetreatDistance))
			{
				canPlayerLeave = false;
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_can_not_retreat", null), 0, null, null, "");
			}
			else if (this._askBeforeLeave)
			{
				return new InquiryData("", GameTexts.FindText("str_give_up_fight", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(base.Mission.OnEndMissionResult), null, "", 0f, null, null, null);
			}
			return null;
		}

		// Token: 0x04000DB6 RID: 3510
		private readonly bool _askBeforeLeave;

		// Token: 0x04000DB7 RID: 3511
		private readonly int _minRetreatDistance;
	}
}
