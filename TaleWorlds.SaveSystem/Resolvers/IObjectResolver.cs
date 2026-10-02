using System;
using TaleWorlds.SaveSystem.Load;

namespace TaleWorlds.SaveSystem.Resolvers
{
	// Token: 0x02000034 RID: 52
	public interface IObjectResolver
	{
		// Token: 0x06000218 RID: 536
		bool CheckIfRequiresAdvancedResolving(object originalObject);

		// Token: 0x06000219 RID: 537
		object ResolveObject(object originalObject);

		// Token: 0x0600021A RID: 538
		object AdvancedResolveObject(object originalObject, MetaData metaData, ObjectLoadData objectLoadData);
	}
}
