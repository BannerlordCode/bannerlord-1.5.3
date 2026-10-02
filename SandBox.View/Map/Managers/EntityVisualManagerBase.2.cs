using System;
using SandBox.View.Map.Visuals;

namespace SandBox.View.Map.Managers
{
	// Token: 0x02000076 RID: 118
	public abstract class EntityVisualManagerBase<TEntity> : EntityVisualManagerBase
	{
		// Token: 0x06000519 RID: 1305
		public abstract MapEntityVisual<TEntity> GetVisualOfEntity(TEntity entity);

		// Token: 0x0600051A RID: 1306 RVA: 0x00026E4A File Offset: 0x0002504A
		public static EntityVisualManagerBase<TEntity> GetEntityVisualManagerBase()
		{
			return SandBoxViewSubModule.SandBoxViewVisualManager.GetEntityComponent<EntityVisualManagerBase<TEntity>>();
		}
	}
}
