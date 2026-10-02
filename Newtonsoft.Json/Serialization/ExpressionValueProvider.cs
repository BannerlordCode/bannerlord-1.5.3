using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200007A RID: 122
	[NullableContext(1)]
	[Nullable(0)]
	public class ExpressionValueProvider : IValueProvider
	{
		// Token: 0x06000665 RID: 1637 RVA: 0x0001B5C5 File Offset: 0x000197C5
		public ExpressionValueProvider(MemberInfo memberInfo)
		{
			ValidationUtils.ArgumentNotNull(memberInfo, "memberInfo");
			this._memberInfo = memberInfo;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0001B5E0 File Offset: 0x000197E0
		public void SetValue(object target, [Nullable(2)] object value)
		{
			try
			{
				if (this._setter == null)
				{
					this._setter = ExpressionReflectionDelegateFactory.Instance.CreateSet<object>(this._memberInfo);
				}
				this._setter(target, value);
			}
			catch (Exception ex)
			{
				throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, this._memberInfo.Name, target.GetType()), ex);
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001B654 File Offset: 0x00019854
		[return: Nullable(2)]
		public object GetValue(object target)
		{
			object obj;
			try
			{
				if (this._getter == null)
				{
					this._getter = ExpressionReflectionDelegateFactory.Instance.CreateGet<object>(this._memberInfo);
				}
				obj = this._getter(target);
			}
			catch (Exception ex)
			{
				throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, this._memberInfo.Name, target.GetType()), ex);
			}
			return obj;
		}

		// Token: 0x04000245 RID: 581
		private readonly MemberInfo _memberInfo;

		// Token: 0x04000246 RID: 582
		[Nullable(new byte[] { 2, 1, 2 })]
		private Func<object, object> _getter;

		// Token: 0x04000247 RID: 583
		[Nullable(new byte[] { 2, 1, 2 })]
		private Action<object, object> _setter;
	}
}
