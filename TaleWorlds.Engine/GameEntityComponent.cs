using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200004C RID: 76
	[EngineClass("rglEntity_component")]
	public abstract class GameEntityComponent : NativeObject
	{
		// Token: 0x060007F6 RID: 2038 RVA: 0x00005D1E File Offset: 0x00003F1E
		internal GameEntityComponent(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00005D2D File Offset: 0x00003F2D
		public WeakGameEntity GetEntity()
		{
			return new WeakGameEntity(EngineApplicationInterface.IGameEntityComponent.GetEntityPointer(base.Pointer));
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00005D44 File Offset: 0x00003F44
		public virtual MetaMesh GetFirstMetaMesh()
		{
			return EngineApplicationInterface.IGameEntityComponent.GetFirstMetaMesh(this);
		}
	}
}
