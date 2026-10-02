using System;
using TaleWorlds.Library;

namespace TaleWorlds.ScreenSystem
{
	// Token: 0x02000004 RID: 4
	public class InputRestrictions
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000020C7 File Offset: 0x000002C7
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000020CF File Offset: 0x000002CF
		public int Order { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000020D8 File Offset: 0x000002D8
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000020E0 File Offset: 0x000002E0
		public Guid Id { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000020E9 File Offset: 0x000002E9
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000020F1 File Offset: 0x000002F1
		public bool MouseVisibility { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000020FA File Offset: 0x000002FA
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002102 File Offset: 0x00000302
		public InputUsageMask InputUsageMask { get; private set; }

		// Token: 0x06000015 RID: 21 RVA: 0x0000210C File Offset: 0x0000030C
		public InputRestrictions(int order)
		{
			this.Id = default(Guid);
			this.InputUsageMask = InputUsageMask.Invalid;
			this.Order = order;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000213C File Offset: 0x0000033C
		public void SetMouseVisibility(bool isVisible)
		{
			this.MouseVisibility = isVisible;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002145 File Offset: 0x00000345
		public void SetInputRestrictions(bool isMouseVisible = true, InputUsageMask mask = InputUsageMask.All)
		{
			this.InputUsageMask = mask;
			this.SetMouseVisibility(isMouseVisible);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002155 File Offset: 0x00000355
		public void ResetInputRestrictions()
		{
			this.InputUsageMask = InputUsageMask.Invalid;
			this.SetMouseVisibility(false);
		}
	}
}
