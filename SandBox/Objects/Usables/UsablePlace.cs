using System;
using SandBox.AI;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects.Usables
{
	// Token: 0x02000051 RID: 81
	public class UsablePlace : UsableMachine
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x00010FAD File Offset: 0x0000F1AD
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			StandingPoint pilotStandingPoint = base.PilotStandingPoint;
			return ((pilotStandingPoint != null) ? pilotStandingPoint.DescriptionMessage : null) ?? TextObject.GetEmpty();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00010FCA File Offset: 0x0000F1CA
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			StandingPoint pilotStandingPoint = base.PilotStandingPoint;
			if (pilotStandingPoint == null)
			{
				return null;
			}
			return pilotStandingPoint.ActionMessage;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00010FDD File Offset: 0x0000F1DD
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new UsablePlaceAI(this);
		}
	}
}
