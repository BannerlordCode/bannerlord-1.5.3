using System;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200002A RID: 42
	public interface IDropContainer
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600033C RID: 828
		// (set) Token: 0x0600033D RID: 829
		Predicate<Widget> AcceptDropPredicate { get; set; }

		// Token: 0x0600033E RID: 830
		Vector2 GetDropGizmoPosition(Vector2 draggedWidgetPosition);
	}
}
