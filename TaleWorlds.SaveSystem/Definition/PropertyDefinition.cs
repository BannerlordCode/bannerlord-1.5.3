using System;
using System.Reflection;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000067 RID: 103
	public class PropertyDefinition : MemberDefinition
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0000EEFD File Offset: 0x0000D0FD
		// (set) Token: 0x0600036C RID: 876 RVA: 0x0000EF05 File Offset: 0x0000D105
		public PropertyInfo PropertyInfo { get; private set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0000EF0E File Offset: 0x0000D10E
		// (set) Token: 0x0600036E RID: 878 RVA: 0x0000EF16 File Offset: 0x0000D116
		public SaveablePropertyAttribute SaveablePropertyAttribute { get; private set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0000EF1F File Offset: 0x0000D11F
		// (set) Token: 0x06000370 RID: 880 RVA: 0x0000EF27 File Offset: 0x0000D127
		public MethodInfo GetMethod { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000371 RID: 881 RVA: 0x0000EF30 File Offset: 0x0000D130
		// (set) Token: 0x06000372 RID: 882 RVA: 0x0000EF38 File Offset: 0x0000D138
		public MethodInfo SetMethod { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000373 RID: 883 RVA: 0x0000EF41 File Offset: 0x0000D141
		// (set) Token: 0x06000374 RID: 884 RVA: 0x0000EF49 File Offset: 0x0000D149
		public GetPropertyValueDelegate GetPropertyValueMethod { get; private set; }

		// Token: 0x06000375 RID: 885 RVA: 0x0000EF54 File Offset: 0x0000D154
		public PropertyDefinition(PropertyInfo propertyInfo, MemberTypeId id)
			: base(propertyInfo, id)
		{
			this.PropertyInfo = propertyInfo;
			this.SaveablePropertyAttribute = propertyInfo.GetCustomAttribute<SaveablePropertyAttribute>();
			this.SetMethod = this.PropertyInfo.GetSetMethod(true);
			if (this.SetMethod == null && this.PropertyInfo.DeclaringType != null)
			{
				PropertyInfo property = this.PropertyInfo.DeclaringType.GetProperty(this.PropertyInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null)
				{
					this.SetMethod = property.GetSetMethod(true);
				}
			}
			if (this.SetMethod == null)
			{
				Debug.FailedAssert(string.Concat(new string[]
				{
					"Property ",
					this.PropertyInfo.Name,
					" at Type ",
					this.PropertyInfo.DeclaringType.FullName,
					" does not have setter method."
				}), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Definition\\PropertyDefinition.cs", ".ctor", 39);
				throw new Exception(string.Concat(new string[]
				{
					"Property ",
					this.PropertyInfo.Name,
					" at Type ",
					this.PropertyInfo.DeclaringType.FullName,
					" does not have setter method."
				}));
			}
			this.GetMethod = this.PropertyInfo.GetGetMethod(true);
			if (this.GetMethod == null && this.PropertyInfo.DeclaringType != null)
			{
				PropertyInfo property2 = this.PropertyInfo.DeclaringType.GetProperty(this.PropertyInfo.Name);
				if (property2 != null)
				{
					this.GetMethod = property2.GetGetMethod(true);
				}
			}
			if (this.GetMethod == null)
			{
				throw new Exception(string.Concat(new string[]
				{
					"Property ",
					this.PropertyInfo.Name,
					" at Type ",
					this.PropertyInfo.DeclaringType.FullName,
					" does not have getter method."
				}));
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000F150 File Offset: 0x0000D350
		public override Type GetMemberType()
		{
			return this.PropertyInfo.PropertyType;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000F160 File Offset: 0x0000D360
		public override object GetValue(object target)
		{
			object obj;
			if (this.GetPropertyValueMethod != null)
			{
				obj = this.GetPropertyValueMethod(target);
			}
			else
			{
				obj = this.GetMethod.Invoke(target, new object[0]);
			}
			return obj;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000F198 File Offset: 0x0000D398
		public void InitializeForAutoGeneration(GetPropertyValueDelegate getPropertyValueMethod)
		{
			this.GetPropertyValueMethod = getPropertyValueMethod;
		}
	}
}
