using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.Library.CodeGeneration;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000069 RID: 105
	internal class SaveCodeGenerationContextAssembly
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600037D RID: 893 RVA: 0x0000F2DC File Offset: 0x0000D4DC
		// (set) Token: 0x0600037E RID: 894 RVA: 0x0000F2E4 File Offset: 0x0000D4E4
		public Assembly Assembly { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0000F2ED File Offset: 0x0000D4ED
		// (set) Token: 0x06000380 RID: 896 RVA: 0x0000F2F5 File Offset: 0x0000D4F5
		public string Location { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0000F2FE File Offset: 0x0000D4FE
		// (set) Token: 0x06000382 RID: 898 RVA: 0x0000F306 File Offset: 0x0000D506
		public string FileName { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000383 RID: 899 RVA: 0x0000F30F File Offset: 0x0000D50F
		// (set) Token: 0x06000384 RID: 900 RVA: 0x0000F317 File Offset: 0x0000D517
		public string DefaultNamespace { get; private set; }

		// Token: 0x06000385 RID: 901 RVA: 0x0000F320 File Offset: 0x0000D520
		public SaveCodeGenerationContextAssembly(DefinitionContext definitionContext, Assembly assembly, string defaultNamespace, string location, string fileName)
		{
			this.Assembly = assembly;
			this.Location = location;
			this.FileName = fileName;
			this.DefaultNamespace = defaultNamespace;
			this._definitionContext = definitionContext;
			this._definitions = new List<TypeDefinition>();
			this._structDefinitions = new List<TypeDefinition>();
			this._containerDefinitions = new List<ContainerDefinition>();
			this._codeGenerationContext = new CodeGenerationContext();
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000F384 File Offset: 0x0000D584
		public void AddClassDefinition(TypeDefinition classDefinition)
		{
			this._definitions.Add(classDefinition);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000F392 File Offset: 0x0000D592
		public void AddStructDefinition(TypeDefinition classDefinition)
		{
			this._structDefinitions.Add(classDefinition);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000F3A0 File Offset: 0x0000D5A0
		public bool CheckIfGotAnyNonPrimitiveMembers(TypeDefinition typeDefinition)
		{
			MemberDefinition[] array = typeDefinition.MemberDefinitions.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				Type memberType = array[i].GetMemberType();
				if (memberType.IsClass && memberType != typeof(string))
				{
					return true;
				}
				if (!(this._definitionContext.GetTypeDefinition(memberType) is BasicTypeDefinition))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000F402 File Offset: 0x0000D602
		private string[] GetNestedClasses(string fullClassName)
		{
			return fullClassName.Split(new char[] { '.' }, StringSplitOptions.None);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000F418 File Offset: 0x0000D618
		private bool CheckIfBaseTypeDefind(Type type)
		{
			Type type2 = type.BaseType;
			TypeDefinitionBase typeDefinitionBase = null;
			while (type2 != null && type2 != typeof(object))
			{
				Type type3 = type2;
				if (type2.IsGenericType && !type2.IsGenericTypeDefinition)
				{
					type3 = type2.GetGenericTypeDefinition();
				}
				typeDefinitionBase = this._definitionContext.GetTypeDefinition(type3);
				if (typeDefinitionBase != null)
				{
					break;
				}
				type2 = type2.BaseType;
			}
			return typeDefinitionBase != null && !(typeDefinitionBase is BasicTypeDefinition);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000F490 File Offset: 0x0000D690
		private bool CheckIfTypeDefined(Type type)
		{
			Type type2 = type;
			if (type.IsGenericType && !type.IsGenericTypeDefinition)
			{
				type2 = type.GetGenericTypeDefinition();
			}
			TypeDefinitionBase typeDefinition = this._definitionContext.GetTypeDefinition(type2);
			return typeDefinition != null && !(typeDefinition is BasicTypeDefinition);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000F4D7 File Offset: 0x0000D6D7
		private static bool IsBuitlinTypeByDotNet(Type type)
		{
			return type.FullName.StartsWith("System.");
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000F4F0 File Offset: 0x0000D6F0
		private bool CheckIfPrimitiveOrPrimiteHolderStruct(Type type)
		{
			bool flag = false;
			TypeDefinitionBase typeDefinition = this._definitionContext.GetTypeDefinition(type);
			if (typeDefinition is BasicTypeDefinition)
			{
				flag = true;
			}
			else if (typeDefinition is EnumDefinition)
			{
				flag = true;
			}
			if (!flag && typeDefinition is TypeDefinition)
			{
				TypeDefinition typeDefinition2 = (TypeDefinition)typeDefinition;
				if (!typeDefinition2.IsClassDefinition && !this.CheckIfGotAnyNonPrimitiveMembers(typeDefinition2))
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000F548 File Offset: 0x0000D748
		private int GetClassGenericInformation(string className)
		{
			if (className.Length > 2 && className[className.Length - 2] == '`')
			{
				return Convert.ToInt32(className[className.Length - 1].ToString());
			}
			return -1;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000F590 File Offset: 0x0000D790
		private void GenerateForClassOrStruct(TypeDefinition typeDefinition)
		{
			Type type = typeDefinition.Type;
			bool isClass = type.IsClass;
			bool flag = !isClass;
			bool flag2 = this.CheckIfBaseTypeDefind(type);
			string @namespace = type.Namespace;
			string text = type.FullName.Replace('+', '.');
			string text2 = text.Substring(@namespace.Length + 1, text.Length - @namespace.Length - 1);
			text2 = text2.Replace('+', '.');
			string[] nestedClasses = this.GetNestedClasses(text2);
			NamespaceCode namespaceCode = this._codeGenerationContext.FindOrCreateNamespace(@namespace);
			string text3 = nestedClasses[nestedClasses.Length - 1];
			ClassCode classCode = null;
			for (int i = 0; i < nestedClasses.Length; i++)
			{
				string text4 = nestedClasses[i];
				ClassCode classCode2 = new ClassCode();
				classCode2.IsPartial = true;
				if (i + 1 == nestedClasses.Length)
				{
					classCode2.IsClass = isClass;
				}
				classCode2.AccessModifier = ClassCodeAccessModifier.DoNotMention;
				int classGenericInformation = this.GetClassGenericInformation(text4);
				if (classGenericInformation >= 0)
				{
					classCode2.IsGeneric = true;
					classCode2.GenericTypeCount = classGenericInformation;
					text4 = text4.Substring(0, text4.Length - 2);
				}
				classCode2.Name = text4;
				if (classCode != null)
				{
					classCode.AddNestedClass(classCode2);
				}
				else
				{
					namespaceCode.AddClass(classCode2);
				}
				classCode = classCode2;
			}
			TypeSaveId typeSaveId = (TypeSaveId)typeDefinition.SaveId;
			int delegateCount = this._delegateCount;
			this._delegateCount++;
			this._managerMethod.AddLine(string.Concat(new object[] { "var typeDefinition", delegateCount, " =  (global::TaleWorlds.SaveSystem.Definition.TypeDefinition)definitionContext.TryGetTypeDefinition(new global::TaleWorlds.SaveSystem.Definition.TypeSaveId(", typeSaveId.Id, "));" }));
			if (!type.IsGenericTypeDefinition && !type.IsAbstract)
			{
				MethodCode methodCode = new MethodCode();
				methodCode.IsStatic = true;
				methodCode.AccessModifier = (flag ? MethodCodeAccessModifier.Public : MethodCodeAccessModifier.Internal);
				methodCode.Name = "AutoGeneratedStaticCollectObjects" + text3;
				methodCode.MethodSignature = "(object o, global::System.Collections.Generic.List<object> collectedObjects)";
				methodCode.AddLine("var target = (global::" + text + ")o;");
				methodCode.AddLine("target.AutoGeneratedInstanceCollectObjects(collectedObjects);");
				classCode.AddMethod(methodCode);
				this._managerMethod.AddLine(string.Concat(new object[] { "TaleWorlds.SaveSystem.Definition.CollectObjectsDelegate d", delegateCount, " = global::", @namespace, ".", text2, ".", methodCode.Name, ";" }));
				this._managerMethod.AddLine(string.Concat(new object[] { "typeDefinition", delegateCount, ".InitializeForAutoGeneration(d", delegateCount, ");" }));
			}
			this._managerMethod.AddLine("");
			MethodCode methodCode2 = new MethodCode();
			if (flag2)
			{
				methodCode2.PolymorphismInfo = MethodCodePolymorphismInfo.Override;
				methodCode2.AccessModifier = MethodCodeAccessModifier.Protected;
			}
			else if (!type.IsSealed)
			{
				methodCode2.PolymorphismInfo = MethodCodePolymorphismInfo.Virtual;
				methodCode2.AccessModifier = MethodCodeAccessModifier.Protected;
			}
			else
			{
				methodCode2.PolymorphismInfo = MethodCodePolymorphismInfo.None;
				methodCode2.AccessModifier = MethodCodeAccessModifier.Private;
			}
			methodCode2.Name = "AutoGeneratedInstanceCollectObjects";
			methodCode2.MethodSignature = "(global::System.Collections.Generic.List<object> collectedObjects)";
			if (flag2)
			{
				methodCode2.AddLine("base.AutoGeneratedInstanceCollectObjects(collectedObjects);");
				methodCode2.AddLine("");
			}
			foreach (MemberDefinition memberDefinition in typeDefinition.MemberDefinitions)
			{
				if (memberDefinition is FieldDefinition)
				{
					FieldInfo fieldInfo = (memberDefinition as FieldDefinition).FieldInfo;
					Type fieldType = fieldInfo.FieldType;
					string name = fieldInfo.Name;
					if (fieldType.IsClass || fieldType.IsInterface)
					{
						if (fieldType != typeof(string))
						{
							bool flag3 = false;
							Type declaringType = fieldInfo.DeclaringType;
							if (declaringType != type)
							{
								flag3 = this.CheckIfTypeDefined(declaringType);
							}
							string text5 = "";
							if (flag3)
							{
								text5 += "//";
							}
							text5 = text5 + "collectedObjects.Add(this." + name + ");";
							methodCode2.AddLine(text5);
						}
					}
					else if (!fieldType.IsClass && this._definitionContext.GetStructDefinition(fieldType) != null)
					{
						string text6 = "";
						bool flag4 = false;
						Type declaringType2 = fieldInfo.DeclaringType;
						if (declaringType2 != type)
						{
							flag4 = this.CheckIfTypeDefined(declaringType2);
						}
						if (flag4)
						{
							text6 += "//";
						}
						string fullTypeName = SaveCodeGenerationContextAssembly.GetFullTypeName(fieldType);
						string usableTypeName = SaveCodeGenerationContextAssembly.GetUsableTypeName(fieldType);
						text6 = string.Concat(new string[] { text6, "global::", fullTypeName, ".AutoGeneratedStaticCollectObjects", usableTypeName, "(this.", name, ", collectedObjects);" });
						methodCode2.AddLine(text6);
					}
				}
			}
			methodCode2.AddLine("");
			foreach (MemberDefinition memberDefinition2 in typeDefinition.MemberDefinitions)
			{
				if (memberDefinition2 is PropertyDefinition)
				{
					PropertyDefinition propertyDefinition = memberDefinition2 as PropertyDefinition;
					PropertyInfo propertyInfo = propertyDefinition.PropertyInfo;
					Type propertyType = propertyDefinition.PropertyInfo.PropertyType;
					string name2 = propertyInfo.Name;
					if (propertyType.IsClass || propertyType.IsInterface)
					{
						if (propertyType != typeof(string))
						{
							bool flag5 = false;
							Type declaringType3 = propertyInfo.DeclaringType;
							if (declaringType3 != type)
							{
								flag5 = this.CheckIfTypeDefined(declaringType3);
							}
							string text7 = "";
							if (flag5)
							{
								text7 += "//";
							}
							text7 = text7 + "collectedObjects.Add(this." + name2 + ");";
							methodCode2.AddLine(text7);
						}
					}
					else if (!propertyType.IsClass && this._definitionContext.GetStructDefinition(propertyType) != null)
					{
						bool flag6 = false;
						Type declaringType4 = propertyInfo.DeclaringType;
						if (declaringType4 != type)
						{
							flag6 = this.CheckIfTypeDefined(declaringType4);
						}
						string text8 = "";
						if (flag6)
						{
							text8 += "//";
						}
						string fullTypeName2 = SaveCodeGenerationContextAssembly.GetFullTypeName(propertyType);
						string usableTypeName2 = SaveCodeGenerationContextAssembly.GetUsableTypeName(propertyType);
						text8 = string.Concat(new string[] { text8, "global::", fullTypeName2, ".AutoGeneratedStaticCollectObjects", usableTypeName2, "(this.", name2, ", collectedObjects);" });
						methodCode2.AddLine(text8);
					}
				}
			}
			classCode.AddMethod(methodCode2);
			foreach (MemberDefinition memberDefinition3 in typeDefinition.MemberDefinitions)
			{
				if (!type.IsGenericTypeDefinition)
				{
					MethodCode methodCode3 = new MethodCode();
					string text9 = "";
					Type type2 = null;
					if (memberDefinition3 is PropertyDefinition)
					{
						PropertyDefinition propertyDefinition2 = memberDefinition3 as PropertyDefinition;
						text9 = propertyDefinition2.PropertyInfo.Name;
						type2 = propertyDefinition2.PropertyInfo.DeclaringType;
					}
					else if (memberDefinition3 is FieldDefinition)
					{
						FieldDefinition fieldDefinition = memberDefinition3 as FieldDefinition;
						text9 = fieldDefinition.FieldInfo.Name;
						type2 = fieldDefinition.FieldInfo.DeclaringType;
					}
					bool flag7 = false;
					if (type2 != type)
					{
						flag7 = this.CheckIfTypeDefined(type2);
					}
					if (!flag7)
					{
						methodCode3.Name = "AutoGeneratedGetMemberValue" + text9;
						methodCode3.MethodSignature = "(object o)";
						methodCode3.IsStatic = true;
						methodCode3.AccessModifier = MethodCodeAccessModifier.Internal;
						methodCode3.PolymorphismInfo = MethodCodePolymorphismInfo.None;
						methodCode3.ReturnParameter = "object";
						methodCode3.AddLine("var target = (global::" + text + ")o;");
						methodCode3.AddLine("return (object)target." + text9 + ";");
						classCode.AddMethod(methodCode3);
						string text10 = "GetPropertyDefinitionWithId";
						if (memberDefinition3 is FieldDefinition)
						{
							text10 = "GetFieldDefinitionWithId";
						}
						this._managerMethod.AddLine("{");
						this._managerMethod.AddLine(string.Concat(new object[]
						{
							"var memberDefinition = typeDefinition",
							delegateCount,
							".",
							text10,
							"(new global::TaleWorlds.SaveSystem.Definition.MemberTypeId(",
							memberDefinition3.Id.TypeLevel,
							",",
							memberDefinition3.Id.LocalSaveId,
							"));"
						}));
						string text11 = string.Concat(new string[] { "global::", @namespace, ".", text2, ".", methodCode3.Name });
						this._managerMethod.AddLine("memberDefinition.InitializeForAutoGeneration(" + text11 + ");");
						this._managerMethod.AddLine("}");
						this._managerMethod.AddLine("");
					}
				}
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000FEC8 File Offset: 0x0000E0C8
		private static string GetFullTypeName(Type type)
		{
			if (type.IsArray)
			{
				return SaveCodeGenerationContextAssembly.GetFullTypeName(type.GetElementType()) + "[]";
			}
			if (type.IsGenericType)
			{
				string[] array = new string[type.GenericTypeArguments.Length];
				string text = "<";
				for (int i = 0; i < type.GenericTypeArguments.Length; i++)
				{
					string fullTypeName = SaveCodeGenerationContextAssembly.GetFullTypeName(type.GenericTypeArguments[i]);
					text += fullTypeName;
					array[i] = fullTypeName;
					if (i + 1 < type.GenericTypeArguments.Length)
					{
						text += ",";
					}
				}
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				text += ">";
				string text2 = genericTypeDefinition.FullName.Replace('+', '.');
				text2 = text2.Substring(0, text2.Length - 2);
				return text2 + text;
			}
			return type.FullName.Replace('+', '.');
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000FFA5 File Offset: 0x0000E1A5
		private static string GetUsableTypeName(Type type)
		{
			return type.Name.Replace('+', '.');
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000FFB8 File Offset: 0x0000E1B8
		private void AddCustomCollectionByBuiltinTypes(CodeBlock codeBlock, Type elementType, string variableName)
		{
			codeBlock.AddLine("//custom code here - begins ");
			if (elementType.GetGenericTypeDefinition() == typeof(Tuple<, >))
			{
				Type type = elementType.GenericTypeArguments[0];
				Type type2 = elementType.GenericTypeArguments[1];
				codeBlock.AddLine("//Tuple here");
				codeBlock.AddLine("");
				codeBlock.AddLine("if (" + variableName + " != null)");
				codeBlock.AddLine("{");
				codeBlock.AddLine("collectedObjects.Add(" + variableName + ");");
				codeBlock.AddLine("");
				codeBlock.AddLine(string.Concat(new string[] { "var ", variableName, "_item1 = ", variableName, ".Item1;" }));
				codeBlock.AddLine(string.Concat(new string[] { "var ", variableName, "_item2 = ", variableName, ".Item2;" }));
				this.AddCodeForType(codeBlock, type, variableName + "_item1");
				this.AddCodeForType(codeBlock, type2, variableName + "_item2");
				codeBlock.AddLine("}");
				return;
			}
			if (elementType.GetGenericTypeDefinition() == typeof(KeyValuePair<, >))
			{
				Type type3 = elementType.GenericTypeArguments[0];
				Type type4 = elementType.GenericTypeArguments[1];
				codeBlock.AddLine("//KeyValuePair here");
				codeBlock.AddLine("");
				codeBlock.AddLine(string.Concat(new string[] { "var ", variableName, "_key = ", variableName, ".Key;" }));
				codeBlock.AddLine(string.Concat(new string[] { "var ", variableName, "_value = ", variableName, ".Value;" }));
				this.AddCodeForType(codeBlock, type3, variableName + "_key");
				this.AddCodeForType(codeBlock, type4, variableName + "_value");
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000101B4 File Offset: 0x0000E3B4
		private void AddCodeForType(CodeBlock codeBlock, Type elementType, string elementVariableName)
		{
			bool flag = this.CheckIfPrimitiveOrPrimiteHolderStruct(elementType);
			TypeDefinition structDefinition = this._definitionContext.GetStructDefinition(elementType);
			bool flag2 = SaveCodeGenerationContextAssembly.IsBuitlinTypeByDotNet(elementType);
			if (elementType != typeof(object) && !flag)
			{
				if (flag2)
				{
					codeBlock.AddLine("//Builtin type in dot net: " + elementType);
					this.AddCustomCollectionByBuiltinTypes(codeBlock, elementType, elementVariableName);
					return;
				}
				if (structDefinition != null)
				{
					string fullTypeName = SaveCodeGenerationContextAssembly.GetFullTypeName(elementType);
					string usableTypeName = SaveCodeGenerationContextAssembly.GetUsableTypeName(elementType);
					codeBlock.AddLine(string.Concat(new string[] { "global::", fullTypeName, ".AutoGeneratedStaticCollectObjects", usableTypeName, "(", elementVariableName, ", collectedObjects);" }));
					return;
				}
				codeBlock.AddLine("collectedObjects.Add(" + elementVariableName + ");");
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00010280 File Offset: 0x0000E480
		private void GenerateForList(ContainerDefinition containerDefinition)
		{
			Type type = containerDefinition.Type;
			Type type2 = type.GetGenericArguments()[0];
			bool flag = this.CheckIfPrimitiveOrPrimiteHolderStruct(type2);
			if (type2 != typeof(object))
			{
				MethodCode methodCode = new MethodCode();
				methodCode.IsStatic = true;
				methodCode.AccessModifier = MethodCodeAccessModifier.Private;
				methodCode.Comment = "//" + type.FullName;
				methodCode.Name = "AutoGeneratedStaticCollectObjectsForList" + this._containerNumber;
				methodCode.MethodSignature = "(object o, global::System.Collections.Generic.List<object> collectedObjects)";
				CodeBlock codeBlock = new CodeBlock();
				this.AddCodeForType(codeBlock, type2, "element");
				if (flag)
				{
					methodCode.AddLine("//Got no child, type: " + type.FullName);
				}
				else
				{
					string fullTypeName = SaveCodeGenerationContextAssembly.GetFullTypeName(type2);
					methodCode.AddLine("var target = (global::System.Collections.IList)o;");
					methodCode.AddLine("");
					methodCode.AddLine("for (int i = 0; i < target.Count; i++)");
					methodCode.AddLine("{");
					methodCode.AddLine("var element = (" + fullTypeName + ")target[i];");
					methodCode.AddCodeBlock(codeBlock);
					methodCode.AddLine("}");
				}
				SaveId saveId = containerDefinition.SaveId;
				StringWriter stringWriter = new StringWriter();
				saveId.WriteTo(stringWriter);
				string text = (flag ? "true" : "false");
				this._managerMethod.AddLine(string.Concat(new object[] { "var saveId", this._delegateCount, " = global::TaleWorlds.SaveSystem.Definition.SaveId.ReadSaveIdFrom(new global::TaleWorlds.Library.StringReader(\"", stringWriter.Data, "\"));" }));
				this._managerMethod.AddLine(string.Concat(new object[] { "var typeDefinition", this._delegateCount, " =  (global::TaleWorlds.SaveSystem.Definition.ContainerDefinition)definitionContext.TryGetTypeDefinition(saveId", this._delegateCount, ");" }));
				this._managerMethod.AddLine(string.Concat(new object[] { "TaleWorlds.SaveSystem.Definition.CollectObjectsDelegate d", this._delegateCount, " = ", methodCode.Name, ";" }));
				this._managerMethod.AddLine(string.Concat(new object[] { "typeDefinition", this._delegateCount, ".InitializeForAutoGeneration(d", this._delegateCount, ", ", text, ");" }));
				this._managerMethod.AddLine("");
				this._delegateCount++;
				this._managerClass.AddMethod(methodCode);
				this._containerNumber++;
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00010524 File Offset: 0x0000E724
		private void GenerateForArray(ContainerDefinition containerDefinition)
		{
			Type type = containerDefinition.Type;
			Type elementType = type.GetElementType();
			bool flag = this.CheckIfPrimitiveOrPrimiteHolderStruct(elementType);
			CodeBlock codeBlock = new CodeBlock();
			this.AddCodeForType(codeBlock, elementType, "element");
			if (elementType != typeof(object))
			{
				MethodCode methodCode = new MethodCode();
				methodCode.IsStatic = true;
				methodCode.AccessModifier = MethodCodeAccessModifier.Private;
				methodCode.Comment = "//" + type.FullName;
				methodCode.Name = "AutoGeneratedStaticCollectObjectsForArray" + this._containerNumber;
				methodCode.MethodSignature = "(object o, global::System.Collections.Generic.List<object> collectedObjects)";
				if (flag)
				{
					methodCode.AddLine("//Got no child, type: " + type.FullName);
				}
				else
				{
					string fullTypeName = SaveCodeGenerationContextAssembly.GetFullTypeName(type);
					methodCode.AddLine("//Builtin type in dot net: " + type.FullName);
					methodCode.AddLine("var target = (global::" + fullTypeName + ")o;");
					methodCode.AddLine("");
					methodCode.AddLine("for (int i = 0; i < target.Length; i++)");
					methodCode.AddLine("{");
					methodCode.AddLine("var element = target[i];");
					methodCode.AddLine("");
					methodCode.AddCodeBlock(codeBlock);
					methodCode.AddLine("}");
				}
				SaveId saveId = containerDefinition.SaveId;
				StringWriter stringWriter = new StringWriter();
				saveId.WriteTo(stringWriter);
				string text = (flag ? "true" : "false");
				this._managerMethod.AddLine(string.Concat(new object[] { "var saveId", this._delegateCount, " = global::TaleWorlds.SaveSystem.Definition.SaveId.ReadSaveIdFrom(new global::TaleWorlds.Library.StringReader(\"", stringWriter.Data, "\"));" }));
				this._managerMethod.AddLine(string.Concat(new object[] { "var typeDefinition", this._delegateCount, " =  (global::TaleWorlds.SaveSystem.Definition.ContainerDefinition)definitionContext.TryGetTypeDefinition(saveId", this._delegateCount, ");" }));
				this._managerMethod.AddLine(string.Concat(new object[] { "TaleWorlds.SaveSystem.Definition.CollectObjectsDelegate d", this._delegateCount, " = ", methodCode.Name, ";" }));
				this._managerMethod.AddLine(string.Concat(new object[] { "typeDefinition", this._delegateCount, ".InitializeForAutoGeneration(d", this._delegateCount, ", ", text, ");" }));
				this._managerMethod.AddLine("");
				this._delegateCount++;
				this._managerClass.AddMethod(methodCode);
				this._containerNumber++;
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000107F8 File Offset: 0x0000E9F8
		private void GenerateForQueue(ContainerDefinition containerDefinition)
		{
			Type type = containerDefinition.Type;
			Type type2 = type.GetGenericArguments()[0];
			if (this.CheckIfPrimitiveOrPrimiteHolderStruct(type2))
			{
				MethodCode methodCode = new MethodCode();
				methodCode.IsStatic = true;
				methodCode.AccessModifier = MethodCodeAccessModifier.Private;
				methodCode.Comment = "//" + type.FullName;
				methodCode.Name = "AutoGeneratedStaticCollectObjectsForQueue" + this._containerNumber;
				methodCode.MethodSignature = "(object o, global::System.Collections.Generic.List<object> collectedObjects)";
				methodCode.AddLine("//Got no child, type: " + type.FullName);
				SaveId saveId = containerDefinition.SaveId;
				StringWriter stringWriter = new StringWriter();
				saveId.WriteTo(stringWriter);
				this._managerMethod.AddLine(string.Concat(new object[] { "var saveId", this._delegateCount, " = global::TaleWorlds.SaveSystem.Definition.SaveId.ReadSaveIdFrom(new global::TaleWorlds.Library.StringReader(\"", stringWriter.Data, "\"));" }));
				this._managerMethod.AddLine(string.Concat(new object[] { "var typeDefinition", this._delegateCount, " =  (global::TaleWorlds.SaveSystem.Definition.ContainerDefinition)definitionContext.TryGetTypeDefinition(saveId", this._delegateCount, ");" }));
				this._managerMethod.AddLine(string.Concat(new object[] { "TaleWorlds.SaveSystem.Definition.CollectObjectsDelegate d", this._delegateCount, " = ", methodCode.Name, ";" }));
				this._managerMethod.AddLine(string.Concat(new object[] { "typeDefinition", this._delegateCount, ".InitializeForAutoGeneration(d", this._delegateCount, ", true);" }));
				this._managerMethod.AddLine("");
				this._delegateCount++;
				this._managerClass.AddMethod(methodCode);
				this._containerNumber++;
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000109F0 File Offset: 0x0000EBF0
		private void GenerateForDictionary(ContainerDefinition containerDefinition)
		{
			Type type = containerDefinition.Type;
			Type type2 = type.GetGenericArguments()[0];
			Type type3 = type.GetGenericArguments()[1];
			bool flag = this.CheckIfPrimitiveOrPrimiteHolderStruct(type2);
			bool flag2 = this.CheckIfPrimitiveOrPrimiteHolderStruct(type3);
			TypeDefinition structDefinition = this._definitionContext.GetStructDefinition(type2);
			TypeDefinition structDefinition2 = this._definitionContext.GetStructDefinition(type3);
			bool flag3 = flag && flag2;
			if ((flag || structDefinition != null) && (flag2 || structDefinition2 != null) && type2 != typeof(object) && type3 != typeof(object))
			{
				MethodCode methodCode = new MethodCode();
				methodCode.IsStatic = true;
				methodCode.AccessModifier = MethodCodeAccessModifier.Private;
				methodCode.Comment = "//" + type.FullName;
				methodCode.Name = "AutoGeneratedStaticCollectObjectsForDictionary" + this._containerNumber;
				methodCode.MethodSignature = "(object o, global::System.Collections.Generic.List<object> collectedObjects)";
				if (flag3)
				{
					methodCode.AddLine("//Got no child, type: " + type.FullName);
				}
				else
				{
					methodCode.AddLine("var target = (global::System.Collections.IDictionary)o;");
					methodCode.AddLine("");
					if (structDefinition != null)
					{
						string fullTypeName = SaveCodeGenerationContextAssembly.GetFullTypeName(type2);
						methodCode.AddLine("foreach (var key in target.Keys)");
						methodCode.AddLine("{");
						string usableTypeName = SaveCodeGenerationContextAssembly.GetUsableTypeName(type2);
						methodCode.AddLine(string.Concat(new string[] { "global::", fullTypeName, ".AutoGeneratedStaticCollectObjects", usableTypeName, "(key, collectedObjects);" }));
						methodCode.AddLine("}");
					}
					methodCode.AddLine("");
					if (structDefinition2 != null)
					{
						string fullTypeName2 = SaveCodeGenerationContextAssembly.GetFullTypeName(type3);
						methodCode.AddLine("foreach (var value in target.Values)");
						methodCode.AddLine("{");
						string usableTypeName2 = SaveCodeGenerationContextAssembly.GetUsableTypeName(type3);
						methodCode.AddLine(string.Concat(new string[] { "global::", fullTypeName2, ".AutoGeneratedStaticCollectObjects", usableTypeName2, "(value, collectedObjects);" }));
						methodCode.AddLine("}");
					}
				}
				SaveId saveId = containerDefinition.SaveId;
				StringWriter stringWriter = new StringWriter();
				saveId.WriteTo(stringWriter);
				string text = (flag3 ? "true" : "false");
				this._managerMethod.AddLine(string.Concat(new object[] { "var saveId", this._delegateCount, " = global::TaleWorlds.SaveSystem.Definition.SaveId.ReadSaveIdFrom(new global::TaleWorlds.Library.StringReader(\"", stringWriter.Data, "\"));" }));
				this._managerMethod.AddLine(string.Concat(new object[] { "var typeDefinition", this._delegateCount, " =  (global::TaleWorlds.SaveSystem.Definition.ContainerDefinition)definitionContext.TryGetTypeDefinition(saveId", this._delegateCount, ");" }));
				this._managerMethod.AddLine(string.Concat(new object[] { "TaleWorlds.SaveSystem.Definition.CollectObjectsDelegate d", this._delegateCount, " = ", methodCode.Name, ";" }));
				this._managerMethod.AddLine(string.Concat(new object[] { "typeDefinition", this._delegateCount, ".InitializeForAutoGeneration(d", this._delegateCount, ", ", text, ");" }));
				this._managerMethod.AddLine("");
				this._delegateCount++;
				this._managerClass.AddMethod(methodCode);
				this._containerNumber++;
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00010D84 File Offset: 0x0000EF84
		public void Generate()
		{
			NamespaceCode namespaceCode = this._codeGenerationContext.FindOrCreateNamespace(this.DefaultNamespace);
			ClassCode classCode = new ClassCode();
			classCode.AccessModifier = ClassCodeAccessModifier.Internal;
			classCode.Name = "AutoGeneratedSaveManager";
			classCode.AddInterface("global::TaleWorlds.SaveSystem.Definition.IAutoGeneratedSaveManager");
			MethodCode methodCode = new MethodCode();
			methodCode.IsStatic = false;
			methodCode.AccessModifier = MethodCodeAccessModifier.Public;
			methodCode.Name = "Initialize";
			methodCode.MethodSignature = "(global::TaleWorlds.SaveSystem.Definition.DefinitionContext definitionContext)";
			classCode.AddMethod(methodCode);
			this._managerMethod = methodCode;
			this._managerClass = classCode;
			namespaceCode.AddClass(classCode);
			foreach (TypeDefinition typeDefinition in this._definitions)
			{
				this.GenerateForClassOrStruct(typeDefinition);
			}
			foreach (TypeDefinition typeDefinition2 in this._structDefinitions)
			{
				this.GenerateForClassOrStruct(typeDefinition2);
			}
			foreach (ContainerDefinition containerDefinition in this._containerDefinitions)
			{
				ContainerType containerType;
				if (containerDefinition.Type.IsContainer(out containerType))
				{
					switch (containerType)
					{
					case ContainerType.List:
					case ContainerType.CustomList:
					case ContainerType.CustomReadOnlyList:
						this.GenerateForList(containerDefinition);
						break;
					case ContainerType.Dictionary:
						this.GenerateForDictionary(containerDefinition);
						break;
					case ContainerType.Array:
						this.GenerateForArray(containerDefinition);
						break;
					case ContainerType.Queue:
						this.GenerateForQueue(containerDefinition);
						break;
					}
				}
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00010F2C File Offset: 0x0000F12C
		public string GenerateText()
		{
			CodeGenerationFile codeGenerationFile = new CodeGenerationFile(null);
			this._codeGenerationContext.GenerateInto(codeGenerationFile);
			return codeGenerationFile.GenerateText();
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00010F52 File Offset: 0x0000F152
		internal void AddContainerDefinition(ContainerDefinition containerDefinition)
		{
			this._containerDefinitions.Add(containerDefinition);
		}

		// Token: 0x0400010F RID: 271
		private List<TypeDefinition> _definitions;

		// Token: 0x04000110 RID: 272
		private List<TypeDefinition> _structDefinitions;

		// Token: 0x04000111 RID: 273
		private List<ContainerDefinition> _containerDefinitions;

		// Token: 0x04000112 RID: 274
		private CodeGenerationContext _codeGenerationContext;

		// Token: 0x04000113 RID: 275
		private DefinitionContext _definitionContext;

		// Token: 0x04000114 RID: 276
		private ClassCode _managerClass;

		// Token: 0x04000115 RID: 277
		private MethodCode _managerMethod;

		// Token: 0x04000116 RID: 278
		private int _delegateCount;

		// Token: 0x04000117 RID: 279
		private int _containerNumber;
	}
}
