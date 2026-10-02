using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000EA RID: 234
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = false, Inherited = true)]
	public sealed class NotNullAttribute : Attribute
	{
	}
}
