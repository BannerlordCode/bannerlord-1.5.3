using System;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace StoryMode.GameComponents
{
	// Token: 0x02000044 RID: 68
	public class StoryModeIncidentModel : IncidentModel
	{
		// Token: 0x0600044F RID: 1103 RVA: 0x00019457 File Offset: 0x00017657
		public override CampaignTime GetMinGlobalCooldownTime()
		{
			return base.BaseModel.GetMinGlobalCooldownTime();
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00019464 File Offset: 0x00017664
		public override CampaignTime GetMaxGlobalCooldownTime()
		{
			return base.BaseModel.GetMaxGlobalCooldownTime();
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00019471 File Offset: 0x00017671
		public override float GetIncidentTriggerGlobalProbability()
		{
			if (!TutorialPhase.Instance.IsCompleted)
			{
				return 0f;
			}
			return base.BaseModel.GetIncidentTriggerGlobalProbability();
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00019490 File Offset: 0x00017690
		public override float GetIncidentTriggerProbabilityDuringSiege()
		{
			if (!TutorialPhase.Instance.IsCompleted)
			{
				return 0f;
			}
			return base.BaseModel.GetIncidentTriggerProbabilityDuringSiege();
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000194AF File Offset: 0x000176AF
		public override float GetIncidentTriggerProbabilityDuringWait()
		{
			if (!TutorialPhase.Instance.IsCompleted)
			{
				return 0f;
			}
			return base.BaseModel.GetIncidentTriggerProbabilityDuringWait();
		}
	}
}
