using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000297 RID: 663
	public abstract class MissionLogic : MissionBehavior
	{
		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06002502 RID: 9474 RVA: 0x00086F66 File Offset: 0x00085166
		public override MissionBehaviorType BehaviorType
		{
			get
			{
				return MissionBehaviorType.Logic;
			}
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x00086F69 File Offset: 0x00085169
		public virtual InquiryData OnEndMissionRequest(out bool canLeave)
		{
			canLeave = true;
			return null;
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x00086F6F File Offset: 0x0008516F
		public virtual bool MissionEnded(ref MissionResult missionResult)
		{
			return false;
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x00086F72 File Offset: 0x00085172
		public virtual void OnBattleEnded()
		{
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x00086F74 File Offset: 0x00085174
		public virtual void ShowBattleResults()
		{
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x00086F76 File Offset: 0x00085176
		public virtual void OnRetreatMission()
		{
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x00086F78 File Offset: 0x00085178
		public virtual void OnSurrenderMission()
		{
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x00086F7A File Offset: 0x0008517A
		public virtual void OnAutoDeployTeam(Team team)
		{
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x00086F7C File Offset: 0x0008517C
		public virtual List<EquipmentElement> GetExtraEquipmentElementsForCharacter(BasicCharacterObject character, bool getAllEquipments = false)
		{
			return null;
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x00086F7F File Offset: 0x0008517F
		public virtual void OnMissionResultReady(MissionResult missionResult)
		{
		}
	}
}
