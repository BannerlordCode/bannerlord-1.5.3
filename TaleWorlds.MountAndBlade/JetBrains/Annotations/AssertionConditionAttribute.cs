using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000E6 RID: 230
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public sealed class AssertionConditionAttribute : Attribute
	{
		// Token: 0x0600093A RID: 2362 RVA: 0x0000FA67 File Offset: 0x0000DC67
		public AssertionConditionAttribute(AssertionConditionType conditionType)
		{
			this.ConditionType = conditionType;
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x0000FA76 File Offset: 0x0000DC76
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x0000FA7E File Offset: 0x0000DC7E
		public AssertionConditionType ConditionType { get; private set; }
	}
}
