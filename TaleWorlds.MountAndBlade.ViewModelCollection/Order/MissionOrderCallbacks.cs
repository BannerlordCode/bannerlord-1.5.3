using System;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x0200001B RID: 27
	public struct MissionOrderCallbacks
	{
		// Token: 0x0400010A RID: 266
		public MissionOrderCallbacks.OnToggleActivateOrderStateDelegate OnActivateToggleOrder;

		// Token: 0x0400010B RID: 267
		public MissionOrderCallbacks.OnToggleActivateOrderStateDelegate OnDeactivateToggleOrder;

		// Token: 0x0400010C RID: 268
		public MissionOrderCallbacks.OnTransferTroopsFinishedDelegate OnTransferTroopsFinished;

		// Token: 0x0400010D RID: 269
		public MissionOrderCallbacks.OnBeforeOrderDelegate OnBeforeOrder;

		// Token: 0x0400010E RID: 270
		public Action<bool> ToggleMissionInputs;

		// Token: 0x0400010F RID: 271
		public MissionOrderCallbacks.ToggleOrderPositionVisibilityDelegate SetSuspendTroopPlacer;

		// Token: 0x04000110 RID: 272
		public MissionOrderCallbacks.GetOrderExecutionParametersDelegate GetVisualOrderExecutionParameters;

		// Token: 0x020000B5 RID: 181
		// (Invoke) Token: 0x06000C0B RID: 3083
		public delegate void OnToggleActivateOrderStateDelegate();

		// Token: 0x020000B6 RID: 182
		// (Invoke) Token: 0x06000C0F RID: 3087
		public delegate void OnTransferTroopsFinishedDelegate();

		// Token: 0x020000B7 RID: 183
		// (Invoke) Token: 0x06000C13 RID: 3091
		public delegate void OnBeforeOrderDelegate();

		// Token: 0x020000B8 RID: 184
		// (Invoke) Token: 0x06000C17 RID: 3095
		public delegate void ToggleOrderPositionVisibilityDelegate(bool value);

		// Token: 0x020000B9 RID: 185
		// (Invoke) Token: 0x06000C1B RID: 3099
		public delegate VisualOrderExecutionParameters GetOrderExecutionParametersDelegate();
	}
}
