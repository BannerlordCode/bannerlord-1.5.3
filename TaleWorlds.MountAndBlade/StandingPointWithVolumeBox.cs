using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000365 RID: 869
	public class StandingPointWithVolumeBox : StandingPointWithWeaponRequirement
	{
		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x0600320A RID: 12810 RVA: 0x000CC090 File Offset: 0x000CA290
		public override Agent.AIScriptedFrameFlags DisableScriptedFrameFlags
		{
			get
			{
				return Agent.AIScriptedFrameFlags.NoAttack;
			}
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x000CC094 File Offset: 0x000CA294
		public override bool IsDisabledForAgent(Agent agent)
		{
			return base.IsDisabledForAgent(agent) || MathF.Abs(agent.Position.z - base.GameEntity.GlobalPosition.z) > 2f || agent.Position.DistanceSquared(base.GameEntity.GlobalPosition) > 100f;
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x000CC0FA File Offset: 0x000CA2FA
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			MBEditor.IsEntitySelected(base.GameEntity);
		}

		// Token: 0x04001518 RID: 5400
		private const float MaxUserAgentDistance = 10f;

		// Token: 0x04001519 RID: 5401
		private const float MaxUserAgentElevation = 2f;

		// Token: 0x0400151A RID: 5402
		public string VolumeBoxTag = "volumebox";
	}
}
