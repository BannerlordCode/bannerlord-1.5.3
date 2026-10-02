using System;
using TaleWorlds.ScreenSystem;

namespace SandBox.View
{
	// Token: 0x02000009 RID: 9
	public abstract class SandboxView
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002A26 File Offset: 0x00000C26
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002A2E File Offset: 0x00000C2E
		public bool IsFinalized { get; protected set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002A37 File Offset: 0x00000C37
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002A3F File Offset: 0x00000C3F
		public ScreenLayer Layer { get; protected set; }

		// Token: 0x06000020 RID: 32 RVA: 0x00002A48 File Offset: 0x00000C48
		protected internal virtual void OnActivate()
		{
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002A4A File Offset: 0x00000C4A
		protected internal virtual void OnDeactivate()
		{
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002A4C File Offset: 0x00000C4C
		protected internal virtual void OnInitialize()
		{
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002A4E File Offset: 0x00000C4E
		protected internal virtual void OnFinalize()
		{
			this.IsFinalized = true;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002A57 File Offset: 0x00000C57
		protected internal virtual void OnFrameTick(float dt)
		{
		}
	}
}
