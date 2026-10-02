using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008A RID: 138
	[NullableContext(1)]
	[Nullable(0)]
	public class JsonDynamicContract : JsonContainerContract
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x0001C6E2 File Offset: 0x0001A8E2
		public JsonPropertyCollection Properties { get; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x0001C6EA File Offset: 0x0001A8EA
		// (set) Token: 0x060006CD RID: 1741 RVA: 0x0001C6F2 File Offset: 0x0001A8F2
		[Nullable(new byte[] { 2, 1, 1 })]
		public Func<string, string> PropertyNameResolver
		{
			[return: Nullable(new byte[] { 2, 1, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 1 })]
			set;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001C6FB File Offset: 0x0001A8FB
		private static CallSite<Func<CallSite, object, object>> CreateCallSiteGetter(string name)
		{
			return CallSite<Func<CallSite, object, object>>.Create(new NoThrowGetBinderMember((GetMemberBinder)DynamicUtils.BinderWrapper.GetMember(name, typeof(DynamicUtils))));
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0001C71C File Offset: 0x0001A91C
		[return: Nullable(new byte[] { 1, 1, 1, 1, 2, 1 })]
		private static CallSite<Func<CallSite, object, object, object>> CreateCallSiteSetter(string name)
		{
			return CallSite<Func<CallSite, object, object, object>>.Create(new NoThrowSetBinderMember((SetMemberBinder)DynamicUtils.BinderWrapper.SetMember(name, typeof(DynamicUtils))));
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001C740 File Offset: 0x0001A940
		public JsonDynamicContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Dynamic;
			this.Properties = new JsonPropertyCollection(base.UnderlyingType);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001C79C File Offset: 0x0001A99C
		internal bool TryGetMember(IDynamicMetaObjectProvider dynamicProvider, string name, [Nullable(2)] out object value)
		{
			ValidationUtils.ArgumentNotNull(dynamicProvider, "dynamicProvider");
			CallSite<Func<CallSite, object, object>> callSite = this._callSiteGetters.Get(name);
			object obj = callSite.Target(callSite, dynamicProvider);
			if (obj != NoThrowExpressionVisitor.ErrorResult)
			{
				value = obj;
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001C7E0 File Offset: 0x0001A9E0
		internal bool TrySetMember(IDynamicMetaObjectProvider dynamicProvider, string name, [Nullable(2)] object value)
		{
			ValidationUtils.ArgumentNotNull(dynamicProvider, "dynamicProvider");
			CallSite<Func<CallSite, object, object, object>> callSite = this._callSiteSetters.Get(name);
			return callSite.Target(callSite, dynamicProvider, value) != NoThrowExpressionVisitor.ErrorResult;
		}

		// Token: 0x04000289 RID: 649
		private readonly ThreadSafeStore<string, CallSite<Func<CallSite, object, object>>> _callSiteGetters = new ThreadSafeStore<string, CallSite<Func<CallSite, object, object>>>(new Func<string, CallSite<Func<CallSite, object, object>>>(JsonDynamicContract.CreateCallSiteGetter));

		// Token: 0x0400028A RID: 650
		[Nullable(new byte[] { 1, 1, 1, 1, 1, 1, 2, 1 })]
		private readonly ThreadSafeStore<string, CallSite<Func<CallSite, object, object, object>>> _callSiteSetters = new ThreadSafeStore<string, CallSite<Func<CallSite, object, object, object>>>(new Func<string, CallSite<Func<CallSite, object, object, object>>>(JsonDynamicContract.CreateCallSiteSetter));
	}
}
