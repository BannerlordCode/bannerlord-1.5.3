using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000039 RID: 57
	[ApplicationInterfaceBase]
	internal interface IHighlights
	{
		// Token: 0x0600058D RID: 1421
		[EngineMethod("initialize", false, null, false)]
		void Initialize();

		// Token: 0x0600058E RID: 1422
		[EngineMethod("open_group", false, null, false)]
		void OpenGroup(string id);

		// Token: 0x0600058F RID: 1423
		[EngineMethod("close_group", false, null, false)]
		void CloseGroup(string id, bool destroy = false);

		// Token: 0x06000590 RID: 1424
		[EngineMethod("save_screenshot", false, null, false)]
		void SaveScreenshot(string highlightId, string groupId);

		// Token: 0x06000591 RID: 1425
		[EngineMethod("save_video", false, null, false)]
		void SaveVideo(string highlightId, string groupId, int startDelta, int endDelta);

		// Token: 0x06000592 RID: 1426
		[EngineMethod("open_summary", false, null, false)]
		void OpenSummary(string groups);

		// Token: 0x06000593 RID: 1427
		[EngineMethod("add_highlight", false, null, false)]
		void AddHighlight(string id, string name);

		// Token: 0x06000594 RID: 1428
		[EngineMethod("remove_highlight", false, null, false)]
		void RemoveHighlight(string id);
	}
}
