using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000053 RID: 83
	[NullableContext(1)]
	[Nullable(0)]
	internal class NoThrowGetBinderMember : GetMemberBinder
	{
		// Token: 0x060004F9 RID: 1273 RVA: 0x00014D32 File Offset: 0x00012F32
		public NoThrowGetBinderMember(GetMemberBinder innerBinder)
			: base(innerBinder.Name, innerBinder.IgnoreCase)
		{
			this._innerBinder = innerBinder;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00014D50 File Offset: 0x00012F50
		public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, [Nullable(2)] DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, CollectionUtils.ArrayEmpty<DynamicMetaObject>());
			return new DynamicMetaObject(new NoThrowExpressionVisitor().Visit(dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
		}

		// Token: 0x040001D9 RID: 473
		private readonly GetMemberBinder _innerBinder;
	}
}
