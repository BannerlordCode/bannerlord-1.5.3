using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000EC RID: 236
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	[BaseTypeRequired(typeof(Attribute))]
	public sealed class BaseTypeRequiredAttribute : Attribute
	{
		// Token: 0x06000941 RID: 2369 RVA: 0x0000FAA7 File Offset: 0x0000DCA7
		public BaseTypeRequiredAttribute(Type baseType)
		{
			this.BaseTypes = new Type[] { baseType };
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x0000FABF File Offset: 0x0000DCBF
		// (set) Token: 0x06000943 RID: 2371 RVA: 0x0000FAC7 File Offset: 0x0000DCC7
		public Type[] BaseTypes { get; private set; }
	}
}
