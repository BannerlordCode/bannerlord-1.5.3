using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200003B RID: 59
	internal class EmptyWidget : Widget
	{
		// Token: 0x060003F2 RID: 1010 RVA: 0x0000FFF0 File Offset: 0x0000E1F0
		public EmptyWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000FFF9 File Offset: 0x0000E1F9
		protected override void OnUpdate(float dt)
		{
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000FFFB File Offset: 0x0000E1FB
		protected override void OnParallelUpdate(float dt)
		{
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000FFFD File Offset: 0x0000E1FD
		protected override void OnLateUpdate(float dt)
		{
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000FFFF File Offset: 0x0000E1FF
		public override void UpdateBrushes(float dt)
		{
		}
	}
}
