using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Load;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200006C RID: 108
	public class TypeDefinition : TypeDefinitionBase
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00010FF9 File Offset: 0x0000F1F9
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x00011001 File Offset: 0x0000F201
		public List<MemberDefinition> MemberDefinitions { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0001100A File Offset: 0x0000F20A
		public IEnumerable<MethodInfo> InitializationCallbacks
		{
			get
			{
				return this._initializationCallbacks;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00011012 File Offset: 0x0000F212
		public IEnumerable<MethodInfo> LateInitializationCallbacks
		{
			get
			{
				return this._lateInitializationCallbacks;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0001101A File Offset: 0x0000F21A
		public IEnumerable<string> Errors
		{
			get
			{
				return this._errors.AsReadOnly();
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x00011027 File Offset: 0x0000F227
		public bool IsClassDefinition
		{
			get
			{
				return this._isClass;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060003AA RID: 938 RVA: 0x0001102F File Offset: 0x0000F22F
		// (set) Token: 0x060003AB RID: 939 RVA: 0x00011037 File Offset: 0x0000F237
		public List<CustomField> CustomFields { get; private set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060003AC RID: 940 RVA: 0x00011040 File Offset: 0x0000F240
		// (set) Token: 0x060003AD RID: 941 RVA: 0x00011048 File Offset: 0x0000F248
		public CollectObjectsDelegate CollectObjectsMethod { get; private set; }

		// Token: 0x060003AE RID: 942 RVA: 0x00011054 File Offset: 0x0000F254
		public TypeDefinition(Type type, SaveId saveId, IObjectResolver objectResolver)
			: base(type, saveId)
		{
			this._isClass = base.Type.IsClass;
			this._errors = new List<string>();
			this._properties = new Dictionary<MemberTypeId, PropertyDefinition>();
			this._fields = new Dictionary<MemberTypeId, FieldDefinition>();
			this.MemberDefinitions = new List<MemberDefinition>();
			this.CustomFields = new List<CustomField>();
			this._initializationCallbacks = new List<MethodInfo>();
			this._lateInitializationCallbacks = new List<MethodInfo>();
			this._objectResolver = objectResolver;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x000110CE File Offset: 0x0000F2CE
		public TypeDefinition(Type type, int saveId, IObjectResolver objectResolver)
			: this(type, new TypeSaveId(saveId), objectResolver)
		{
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x000110DE File Offset: 0x0000F2DE
		public bool CheckIfRequiresAdvancedResolving(object originalObject)
		{
			return this._objectResolver != null && this._objectResolver.CheckIfRequiresAdvancedResolving(originalObject);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x000110F6 File Offset: 0x0000F2F6
		public object ResolveObject(object originalObject)
		{
			if (this._objectResolver != null)
			{
				return this._objectResolver.ResolveObject(originalObject);
			}
			return originalObject;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0001110E File Offset: 0x0000F30E
		public object AdvancedResolveObject(object originalObject, MetaData metaData, ObjectLoadData objectLoadData)
		{
			if (this._objectResolver != null)
			{
				return this._objectResolver.AdvancedResolveObject(originalObject, metaData, objectLoadData);
			}
			return originalObject;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00011128 File Offset: 0x0000F328
		public void CollectInitializationCallbacks()
		{
			Type type = base.Type;
			while (type != typeof(object))
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (methodInfo.DeclaringType == type)
					{
						if (methodInfo.GetCustomAttributesSafe(typeof(LoadInitializationCallback)).ToArray<Attribute>().Length != 0 && !this._initializationCallbacks.Contains(methodInfo))
						{
							this._initializationCallbacks.Insert(0, methodInfo);
						}
						if (methodInfo.GetCustomAttributesSafe(typeof(LateLoadInitializationCallback)).ToArray<Attribute>().Length != 0 && !this._lateInitializationCallbacks.Contains(methodInfo))
						{
							this._lateInitializationCallbacks.Insert(0, methodInfo);
						}
					}
				}
				type = type.BaseType;
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x000111F0 File Offset: 0x0000F3F0
		public void CollectProperties()
		{
			foreach (PropertyInfo propertyInfo in base.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				Attribute[] array = propertyInfo.GetCustomAttributesSafe(typeof(SaveablePropertyAttribute)).ToArray<Attribute>();
				if (array.Length != 0)
				{
					SaveablePropertyAttribute saveablePropertyAttribute = (SaveablePropertyAttribute)array[0];
					byte classLevel = TypeDefinitionBase.GetClassLevel(propertyInfo.DeclaringType);
					MemberTypeId memberTypeId = new MemberTypeId(classLevel, saveablePropertyAttribute.LocalSaveId);
					PropertyDefinition propertyDefinition = new PropertyDefinition(propertyInfo, memberTypeId);
					if (this._properties.ContainsKey(memberTypeId))
					{
						this._errors.Add(string.Concat(new object[]
						{
							"SaveId ",
							memberTypeId,
							" of property ",
							propertyDefinition.PropertyInfo.Name,
							" is already defined in type ",
							base.Type.FullName
						}));
					}
					else
					{
						this._properties.Add(memberTypeId, propertyDefinition);
						this.MemberDefinitions.Add(propertyDefinition);
					}
				}
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x000112F3 File Offset: 0x0000F4F3
		private static IEnumerable<FieldInfo> GetFieldsOfType(Type type)
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!fieldInfo.IsPrivate)
				{
					yield return fieldInfo;
				}
			}
			FieldInfo[] array = null;
			Type typeToCheck = type;
			while (typeToCheck != typeof(object))
			{
				FieldInfo[] fields2 = typeToCheck.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
				foreach (FieldInfo fieldInfo2 in fields2)
				{
					if (fieldInfo2.IsPrivate)
					{
						yield return fieldInfo2;
					}
				}
				array = null;
				typeToCheck = typeToCheck.BaseType;
			}
			yield break;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00011304 File Offset: 0x0000F504
		public void CollectFields()
		{
			foreach (FieldInfo fieldInfo in TypeDefinition.GetFieldsOfType(base.Type).ToArray<FieldInfo>())
			{
				Attribute[] array2 = fieldInfo.GetCustomAttributesSafe(typeof(SaveableFieldAttribute)).ToArray<Attribute>();
				if (array2.Length != 0)
				{
					SaveableFieldAttribute saveableFieldAttribute = (SaveableFieldAttribute)array2[0];
					byte classLevel = TypeDefinitionBase.GetClassLevel(fieldInfo.DeclaringType);
					MemberTypeId memberTypeId = new MemberTypeId(classLevel, saveableFieldAttribute.LocalSaveId);
					FieldDefinition fieldDefinition = new FieldDefinition(fieldInfo, memberTypeId);
					if (this._fields.ContainsKey(memberTypeId))
					{
						this._errors.Add(string.Concat(new object[]
						{
							"SaveId ",
							memberTypeId,
							" of field ",
							fieldDefinition.FieldInfo,
							" is already defined in type ",
							base.Type.FullName
						}));
					}
					else
					{
						this._fields.Add(memberTypeId, fieldDefinition);
						this.MemberDefinitions.Add(fieldDefinition);
					}
				}
			}
			foreach (CustomField customField in this.CustomFields)
			{
				string name = customField.Name;
				short saveId = customField.SaveId;
				FieldInfo field = base.Type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				byte classLevel2 = TypeDefinitionBase.GetClassLevel(field.DeclaringType);
				MemberTypeId memberTypeId2 = new MemberTypeId(classLevel2, saveId);
				FieldDefinition fieldDefinition2 = new FieldDefinition(field, memberTypeId2);
				if (this._fields.ContainsKey(memberTypeId2))
				{
					this._errors.Add(string.Concat(new object[]
					{
						"SaveId ",
						memberTypeId2,
						" of field ",
						fieldDefinition2.FieldInfo,
						" is already defined in type ",
						base.Type.FullName
					}));
				}
				else
				{
					this._fields.Add(memberTypeId2, fieldDefinition2);
					this.MemberDefinitions.Add(fieldDefinition2);
				}
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00011508 File Offset: 0x0000F708
		public void AddCustomField(string fieldName, short saveId)
		{
			this.CustomFields.Add(new CustomField(fieldName, saveId));
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0001151C File Offset: 0x0000F71C
		public PropertyDefinition GetPropertyDefinitionWithId(MemberTypeId id)
		{
			PropertyDefinition propertyDefinition;
			this._properties.TryGetValue(id, out propertyDefinition);
			return propertyDefinition;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001153C File Offset: 0x0000F73C
		public FieldDefinition GetFieldDefinitionWithId(MemberTypeId id)
		{
			FieldDefinition fieldDefinition;
			this._fields.TryGetValue(id, out fieldDefinition);
			return fieldDefinition;
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00011559 File Offset: 0x0000F759
		public Dictionary<MemberTypeId, PropertyDefinition>.ValueCollection PropertyDefinitions
		{
			get
			{
				return this._properties.Values;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00011566 File Offset: 0x0000F766
		public Dictionary<MemberTypeId, FieldDefinition>.ValueCollection FieldDefinitions
		{
			get
			{
				return this._fields.Values;
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00011573 File Offset: 0x0000F773
		public void InitializeForAutoGeneration(CollectObjectsDelegate collectObjectsDelegate)
		{
			this.CollectObjectsMethod = collectObjectsDelegate;
		}

		// Token: 0x04000118 RID: 280
		private Dictionary<MemberTypeId, PropertyDefinition> _properties;

		// Token: 0x04000119 RID: 281
		private Dictionary<MemberTypeId, FieldDefinition> _fields;

		// Token: 0x0400011B RID: 283
		private List<string> _errors;

		// Token: 0x0400011C RID: 284
		private List<MethodInfo> _initializationCallbacks;

		// Token: 0x0400011D RID: 285
		private List<MethodInfo> _lateInitializationCallbacks;

		// Token: 0x0400011E RID: 286
		private bool _isClass;

		// Token: 0x0400011F RID: 287
		private readonly IObjectResolver _objectResolver;
	}
}
