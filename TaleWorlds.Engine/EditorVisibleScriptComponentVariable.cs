using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000088 RID: 136
	public class EditorVisibleScriptComponentVariable : Attribute
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0000D96F File Offset: 0x0000BB6F
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x0000D977 File Offset: 0x0000BB77
		public bool Visible { get; set; }

		// Token: 0x06000C40 RID: 3136 RVA: 0x0000D980 File Offset: 0x0000BB80
		public EditorVisibleScriptComponentVariable(bool visible)
		{
			this.Visible = visible;
		}
	}
}
