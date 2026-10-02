using System;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000379 RID: 889
	public interface IFocusable
	{
		// Token: 0x0600331A RID: 13082
		void OnFocusGain(Agent userAgent);

		// Token: 0x0600331B RID: 13083
		void OnFocusLose(Agent userAgent);

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x0600331C RID: 13084
		FocusableObjectType FocusableObjectType { get; }

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x0600331D RID: 13085
		bool IsFocusable { get; }

		// Token: 0x0600331E RID: 13086
		TextObject GetInfoTextForBeingNotInteractable(Agent userAgent);

		// Token: 0x0600331F RID: 13087
		TextObject GetDescriptionText(WeakGameEntity gameEntity);
	}
}
