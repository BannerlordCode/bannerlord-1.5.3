using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000385 RID: 901
	public class UsableGameObjectGroup : ScriptComponentBehavior, IVisible
	{
		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06003386 RID: 13190 RVA: 0x000D4CB4 File Offset: 0x000D2EB4
		// (set) Token: 0x06003387 RID: 13191 RVA: 0x000D4CD0 File Offset: 0x000D2ED0
		public bool IsVisible
		{
			get
			{
				return base.GameEntity.IsVisibleIncludeParents();
			}
			set
			{
				base.GameEntity.SetVisibilityExcludeParents(value);
			}
		}
	}
}
