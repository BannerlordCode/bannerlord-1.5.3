using System;

namespace SandBox.View.Map.Visuals
{
	// Token: 0x02000064 RID: 100
	public abstract class MapEntityVisual<T> : MapEntityVisual
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x0001F145 File Offset: 0x0001D345
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x0001F14D File Offset: 0x0001D34D
		public T MapEntity { get; private set; }

		// Token: 0x06000401 RID: 1025 RVA: 0x0001F156 File Offset: 0x0001D356
		public MapEntityVisual(T entity)
		{
			this.MapEntity = entity;
		}
	}
}
