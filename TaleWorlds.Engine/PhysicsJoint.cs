using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000077 RID: 119
	public sealed class PhysicsJoint
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x0000AEBC File Offset: 0x000090BC
		internal UIntPtr Pointer
		{
			get
			{
				return this._pointer;
			}
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0000AEC4 File Offset: 0x000090C4
		internal PhysicsJoint(UIntPtr ptr)
		{
			this._pointer = ptr;
		}

		// Token: 0x04000165 RID: 357
		private readonly UIntPtr _pointer;
	}
}
