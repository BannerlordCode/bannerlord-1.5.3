using System;
using SandBox.AI;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects.Usables
{
	// Token: 0x0200004D RID: 77
	public class Passage : UsableMachine
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x00010438 File Offset: 0x0000E638
		public Location ToLocation
		{
			get
			{
				PassageUsePoint passageUsePoint;
				if ((passageUsePoint = base.PilotStandingPoint as PassageUsePoint) == null)
				{
					return null;
				}
				return passageUsePoint.ToLocation;
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0001045C File Offset: 0x0000E65C
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			StandingPoint pilotStandingPoint = base.PilotStandingPoint;
			return ((pilotStandingPoint != null) ? pilotStandingPoint.DescriptionMessage : null) ?? TextObject.GetEmpty();
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00010479 File Offset: 0x0000E679
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			StandingPoint pilotStandingPoint = base.PilotStandingPoint;
			if (pilotStandingPoint == null)
			{
				return null;
			}
			return pilotStandingPoint.ActionMessage;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0001048C File Offset: 0x0000E68C
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new PassageAI(this);
		}
	}
}
