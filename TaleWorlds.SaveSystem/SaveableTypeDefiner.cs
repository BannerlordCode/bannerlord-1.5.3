using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.SaveSystem
{
	// Token: 0x02000015 RID: 21
	public abstract class SaveableTypeDefiner
	{
		// Token: 0x06000066 RID: 102 RVA: 0x00003E21 File Offset: 0x00002021
		protected SaveableTypeDefiner(int saveBaseId)
		{
			this._saveBaseId = saveBaseId;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003E30 File Offset: 0x00002030
		internal void Initialize(DefinitionContext definitionContext)
		{
			this._definitionContext = definitionContext;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003E39 File Offset: 0x00002039
		protected internal virtual void DefineBasicTypes()
		{
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003E3B File Offset: 0x0000203B
		protected internal virtual void DefineClassTypes()
		{
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003E3D File Offset: 0x0000203D
		protected internal virtual void DefineConflictResolvers()
		{
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003E3F File Offset: 0x0000203F
		protected internal virtual void DefineStructTypes()
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003E41 File Offset: 0x00002041
		protected internal virtual void DefineInterfaceTypes()
		{
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003E43 File Offset: 0x00002043
		protected internal virtual void DefineEnumTypes()
		{
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003E45 File Offset: 0x00002045
		protected internal virtual void DefineRootClassTypes()
		{
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003E47 File Offset: 0x00002047
		protected internal virtual void DefineGenericClassDefinitions()
		{
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003E49 File Offset: 0x00002049
		protected internal virtual void DefineGenericStructDefinitions()
		{
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003E4B File Offset: 0x0000204B
		protected internal virtual void DefineContainerDefinitions()
		{
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003E4D File Offset: 0x0000204D
		protected void ConstructGenericClassDefinition(Type type)
		{
			this._definitionContext.ConstructGenericClassDefinition(type);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003E5C File Offset: 0x0000205C
		protected void ConstructGenericStructDefinition(Type type)
		{
			this._definitionContext.ConstructGenericStructDefinition(type);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003E6C File Offset: 0x0000206C
		protected void AddBasicTypeDefinition(Type type, int saveId, IBasicTypeSerializer serializer)
		{
			BasicTypeDefinition basicTypeDefinition = new BasicTypeDefinition(type, this._saveBaseId + saveId, serializer);
			this._definitionContext.AddBasicTypeDefinition(basicTypeDefinition);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003E95 File Offset: 0x00002095
		protected void AddConflictResolver(int saveId, IConflictResolver conflictResolver)
		{
			this._definitionContext.AddConflictResolver(new TypeSaveId(this._saveBaseId + saveId), conflictResolver);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003EB0 File Offset: 0x000020B0
		protected void AddClassDefinition(Type type, int saveId, IObjectResolver resolver = null)
		{
			TypeDefinition typeDefinition = new TypeDefinition(type, this._saveBaseId + saveId, resolver);
			this._definitionContext.AddClassDefinition(typeDefinition);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003EDC File Offset: 0x000020DC
		protected void AddClassDefinitionWithCustomFields(Type type, int saveId, IEnumerable<Tuple<string, short>> fields, IObjectResolver resolver = null)
		{
			TypeDefinition typeDefinition = new TypeDefinition(type, this._saveBaseId + saveId, resolver);
			this._definitionContext.AddClassDefinition(typeDefinition);
			foreach (Tuple<string, short> tuple in fields)
			{
				typeDefinition.AddCustomField(tuple.Item1, tuple.Item2);
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003F4C File Offset: 0x0000214C
		protected void AddStructDefinitionWithCustomFields(Type type, int saveId, IEnumerable<Tuple<string, short>> fields, IObjectResolver resolver = null)
		{
			StructDefinition structDefinition = new StructDefinition(type, this._saveBaseId + saveId, resolver);
			this._definitionContext.AddStructDefinition(structDefinition);
			foreach (Tuple<string, short> tuple in fields)
			{
				structDefinition.AddCustomField(tuple.Item1, tuple.Item2);
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003FBC File Offset: 0x000021BC
		protected void AddRootClassDefinition(Type type, int saveId, IObjectResolver resolver = null)
		{
			TypeDefinition typeDefinition = new TypeDefinition(type, this._saveBaseId + saveId, resolver);
			this._definitionContext.AddRootClassDefinition(typeDefinition);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003FE8 File Offset: 0x000021E8
		protected void AddStructDefinition(Type type, int saveId, IObjectResolver resolver = null)
		{
			StructDefinition structDefinition = new StructDefinition(type, this._saveBaseId + saveId, resolver);
			this._definitionContext.AddStructDefinition(structDefinition);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004014 File Offset: 0x00002214
		protected void AddInterfaceDefinition(Type type, int saveId)
		{
			InterfaceDefinition interfaceDefinition = new InterfaceDefinition(type, this._saveBaseId + saveId);
			this._definitionContext.AddInterfaceDefinition(interfaceDefinition);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000403C File Offset: 0x0000223C
		protected void AddEnumDefinition(Type type, int saveId, IEnumResolver enumResolver = null)
		{
			EnumDefinition enumDefinition = new EnumDefinition(type, this._saveBaseId + saveId, enumResolver);
			this._definitionContext.AddEnumDefinition(enumDefinition);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004068 File Offset: 0x00002268
		protected void ConstructContainerDefinition(Type type)
		{
			if (!this._definitionContext.HasDefinition(type))
			{
				Assembly assembly = base.GetType().Assembly;
				this._definitionContext.ConstructContainerDefinition(type, assembly);
				return;
			}
			Debug.FailedAssert(string.Format("There is duplicate definition for {0}", type), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\SaveableTypeDefiner.cs", "ConstructContainerDefinition", 176);
		}

		// Token: 0x0400001A RID: 26
		private DefinitionContext _definitionContext;

		// Token: 0x0400001B RID: 27
		private readonly int _saveBaseId;
	}
}
