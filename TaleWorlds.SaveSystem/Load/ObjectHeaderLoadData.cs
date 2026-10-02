using System;
using System.Runtime.Serialization;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x0200003E RID: 62
	public class ObjectHeaderLoadData
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000C53F File Offset: 0x0000A73F
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000C547 File Offset: 0x0000A747
		public int Id { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000C550 File Offset: 0x0000A750
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000C558 File Offset: 0x0000A758
		public object LoadedObject { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000C561 File Offset: 0x0000A761
		// (set) Token: 0x06000270 RID: 624 RVA: 0x0000C569 File Offset: 0x0000A769
		public object Target { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000C572 File Offset: 0x0000A772
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000C57A File Offset: 0x0000A77A
		public short PropertyCount { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000C583 File Offset: 0x0000A783
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000C58B File Offset: 0x0000A78B
		public short ChildStructCount { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000C594 File Offset: 0x0000A794
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000C59C File Offset: 0x0000A79C
		public TypeDefinition TypeDefinition { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000C5A5 File Offset: 0x0000A7A5
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000C5AD File Offset: 0x0000A7AD
		public LoadContext Context { get; private set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000C5B6 File Offset: 0x0000A7B6
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000C5BE File Offset: 0x0000A7BE
		public SaveId SaveId { get; private set; }

		// Token: 0x0600027B RID: 635 RVA: 0x0000C5C7 File Offset: 0x0000A7C7
		public ObjectHeaderLoadData(LoadContext context, int id)
		{
			this.Context = context;
			this.Id = id;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000C5E0 File Offset: 0x0000A7E0
		public void InitialieReaders(SaveEntryFolder saveEntryFolder)
		{
			BinaryReader binaryReader = saveEntryFolder.GetEntry(new EntryId(-1, SaveEntryExtension.Basics)).GetBinaryReader();
			this.SaveId = SaveId.ReadSaveIdFrom(binaryReader);
			this.PropertyCount = binaryReader.ReadShort();
			this.ChildStructCount = binaryReader.ReadShort();
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000C624 File Offset: 0x0000A824
		public void CreateObject()
		{
			this.TypeDefinition = this.Context.DefinitionContext.TryGetTypeDefinition(this.SaveId) as TypeDefinition;
			if (this.TypeDefinition != null)
			{
				Type type = this.TypeDefinition.Type;
				this.LoadedObject = FormatterServices.GetUninitializedObject(type);
				this.Target = this.LoadedObject;
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000C67E File Offset: 0x0000A87E
		public void AdvancedResolveObject(MetaData metaData, ObjectLoadData objectLoadData)
		{
			this.Target = this.TypeDefinition.AdvancedResolveObject(this.LoadedObject, metaData, objectLoadData);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000C699 File Offset: 0x0000A899
		public void ResolveObject()
		{
			this.Target = this.TypeDefinition.ResolveObject(this.LoadedObject);
		}
	}
}
