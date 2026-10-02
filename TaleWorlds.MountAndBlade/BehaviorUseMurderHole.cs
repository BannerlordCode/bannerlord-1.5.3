using System;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200013B RID: 315
	public class BehaviorUseMurderHole : BehaviorComponent
	{
		// Token: 0x06000F37 RID: 3895 RVA: 0x000280D8 File Offset: 0x000262D8
		public BehaviorUseMurderHole(Formation formation)
			: base(formation)
		{
			this._behaviorSide = formation.AI.Side;
			WorldPosition worldPosition = new WorldPosition(base.Formation.Team.Mission.Scene, UIntPtr.Zero, (formation.Team.TeamAI as TeamAISiegeDefender).MurderHolePosition, false);
			this._outerGate = (formation.Team.TeamAI as TeamAISiegeDefender).OuterGate;
			this._innerGate = (formation.Team.TeamAI as TeamAISiegeDefender).InnerGate;
			this._batteringRam = base.Formation.Team.Mission.ActiveMissionObjects.FindAllWithType<BatteringRam>().FirstOrDefault<BatteringRam>();
			base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x000281A6 File Offset: 0x000263A6
		public override void TickOccasionally()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x000281BC File Offset: 0x000263BC
		public bool IsMurderHoleActive()
		{
			return (this._batteringRam != null && this._batteringRam.HasArrivedAtTarget && !this._innerGate.IsDestroyed) || (this._outerGate.IsDestroyed && !this._innerGate.IsDestroyed);
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0002820A File Offset: 0x0002640A
		protected override float GetAiWeight()
		{
			return 10f * (this.IsMurderHoleActive() ? 1f : 0f);
		}

		// Token: 0x040003B1 RID: 945
		private readonly CastleGate _outerGate;

		// Token: 0x040003B2 RID: 946
		private readonly CastleGate _innerGate;

		// Token: 0x040003B3 RID: 947
		private readonly BatteringRam _batteringRam;
	}
}
