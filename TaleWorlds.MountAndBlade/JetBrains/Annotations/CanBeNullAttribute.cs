using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000E9 RID: 233
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = false, Inherited = true)]
	public sealed class CanBeNullAttribute : Attribute
	{
	}
}
