using System;

namespace SandBox.View.Map
{
	// Token: 0x0200004E RID: 78
	public class MapEncyclopediaView : MapView
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00017C49 File Offset: 0x00015E49
		// (set) Token: 0x06000291 RID: 657 RVA: 0x00017C51 File Offset: 0x00015E51
		public bool IsEncyclopediaOpen { get; protected set; }

		// Token: 0x06000292 RID: 658 RVA: 0x00017C5A File Offset: 0x00015E5A
		public virtual void CloseEncyclopedia()
		{
		}
	}
}
