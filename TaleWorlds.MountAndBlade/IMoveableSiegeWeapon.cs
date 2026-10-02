using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200035C RID: 860
	public interface IMoveableSiegeWeapon
	{
		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06003185 RID: 12677
		SiegeWeaponMovementComponent MovementComponent { get; }

		// Token: 0x06003186 RID: 12678
		void HighlightPath();

		// Token: 0x06003187 RID: 12679
		void SwitchGhostEntityMovementMode(bool isGhostEnabled);

		// Token: 0x06003188 RID: 12680
		MatrixFrame GetInitialFrame();
	}
}
