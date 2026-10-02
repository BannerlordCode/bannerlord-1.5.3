using System;
using System.Collections.Generic;

namespace TaleWorlds.ScreenSystem
{
	// Token: 0x02000003 RID: 3
	public class GlobalLayer : IComparable
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
		public ScreenLayer Layer { get; protected set; }

		// Token: 0x06000003 RID: 3 RVA: 0x00002059 File Offset: 0x00000259
		internal void EarlyTick(float dt)
		{
			this.OnEarlyTick(dt);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002062 File Offset: 0x00000262
		internal void Tick(float dt)
		{
			this.OnTick(dt);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000206B File Offset: 0x0000026B
		internal void LateTick(float dt)
		{
			this.OnLateTick(dt);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002074 File Offset: 0x00000274
		protected virtual void OnEarlyTick(float dt)
		{
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002076 File Offset: 0x00000276
		protected virtual void OnTick(float dt)
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002078 File Offset: 0x00000278
		protected virtual void OnLateTick(float dt)
		{
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000207A File Offset: 0x0000027A
		internal void Update(IReadOnlyList<int> lastKeysPressed)
		{
			this.Layer.Update(lastKeysPressed);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002088 File Offset: 0x00000288
		public int CompareTo(object obj)
		{
			GlobalLayer globalLayer = obj as GlobalLayer;
			if (globalLayer == null)
			{
				return -1;
			}
			return this.Layer.CompareTo(globalLayer.Layer);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000020B2 File Offset: 0x000002B2
		public virtual void UpdateLayout()
		{
			this.Layer.UpdateLayout();
		}
	}
}
