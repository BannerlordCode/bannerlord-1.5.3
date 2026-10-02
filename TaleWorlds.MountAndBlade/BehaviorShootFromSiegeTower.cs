using System;
using System.Linq;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000134 RID: 308
	public class BehaviorShootFromSiegeTower : BehaviorComponent
	{
		// Token: 0x06000F0A RID: 3850 RVA: 0x00025B6B File Offset: 0x00023D6B
		public BehaviorShootFromSiegeTower(Formation formation)
			: base(formation)
		{
			this._behaviorSide = formation.AI.Side;
			this._siegeTower = Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeTower>().FirstOrDefault<SiegeTower>((SiegeTower st) => st.WeaponSide == this._behaviorSide);
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x00025BAC File Offset: 0x00023DAC
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			if (base.Formation.AI.Side != this._behaviorSide)
			{
				this._behaviorSide = base.Formation.AI.Side;
				this._siegeTower = Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeTower>().FirstOrDefault<SiegeTower>((SiegeTower st) => st.WeaponSide == this._behaviorSide);
			}
			if (this._siegeTower == null || this._siegeTower.IsDestroyed)
			{
				return;
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x00025C3A File Offset: 0x00023E3A
		protected override float GetAiWeight()
		{
			return 0f;
		}

		// Token: 0x0400039B RID: 923
		private SiegeTower _siegeTower;
	}
}
