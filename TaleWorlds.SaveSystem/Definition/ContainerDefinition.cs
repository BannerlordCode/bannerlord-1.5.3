using System;
using System.Reflection;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000059 RID: 89
	public class ContainerDefinition : TypeDefinitionBase
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000D66A File Offset: 0x0000B86A
		// (set) Token: 0x06000300 RID: 768 RVA: 0x0000D672 File Offset: 0x0000B872
		public Assembly DefinedAssembly { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000D67B File Offset: 0x0000B87B
		// (set) Token: 0x06000302 RID: 770 RVA: 0x0000D683 File Offset: 0x0000B883
		public CollectObjectsDelegate CollectObjectsMethod { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000D68C File Offset: 0x0000B88C
		// (set) Token: 0x06000304 RID: 772 RVA: 0x0000D694 File Offset: 0x0000B894
		public bool HasNoChildObject { get; private set; }

		// Token: 0x06000305 RID: 773 RVA: 0x0000D69D File Offset: 0x0000B89D
		public ContainerDefinition(Type type, ContainerSaveId saveId, Assembly definedAssembly)
			: base(type, saveId)
		{
			this.DefinedAssembly = definedAssembly;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000D6AE File Offset: 0x0000B8AE
		public void InitializeForAutoGeneration(CollectObjectsDelegate collectObjectsDelegate, bool hasNoChildObject)
		{
			this.CollectObjectsMethod = collectObjectsDelegate;
			this.HasNoChildObject = hasNoChildObject;
		}
	}
}
