using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000054 RID: 84
	[NullableContext(1)]
	[Nullable(0)]
	internal class NoThrowSetBinderMember : SetMemberBinder
	{
		// Token: 0x060004FB RID: 1275 RVA: 0x00014D8A File Offset: 0x00012F8A
		public NoThrowSetBinderMember(SetMemberBinder innerBinder)
			: base(innerBinder.Name, innerBinder.IgnoreCase)
		{
			this._innerBinder = innerBinder;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00014DA8 File Offset: 0x00012FA8
		public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, [Nullable(2)] DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, new DynamicMetaObject[] { value });
			return new DynamicMetaObject(new NoThrowExpressionVisitor().Visit(dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
		}

		// Token: 0x040001DA RID: 474
		private readonly SetMemberBinder _innerBinder;
	}
}
