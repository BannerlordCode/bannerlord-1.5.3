using System;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x02000035 RID: 53
	public class ContainerHeaderLoadData
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000AF32 File Offset: 0x00009132
		// (set) Token: 0x0600021C RID: 540 RVA: 0x0000AF3A File Offset: 0x0000913A
		public int Id { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000AF43 File Offset: 0x00009143
		// (set) Token: 0x0600021E RID: 542 RVA: 0x0000AF4B File Offset: 0x0000914B
		public object Target { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0000AF54 File Offset: 0x00009154
		// (set) Token: 0x06000220 RID: 544 RVA: 0x0000AF5C File Offset: 0x0000915C
		public LoadContext Context { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000AF65 File Offset: 0x00009165
		// (set) Token: 0x06000222 RID: 546 RVA: 0x0000AF6D File Offset: 0x0000916D
		public ContainerDefinition TypeDefinition { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000AF76 File Offset: 0x00009176
		// (set) Token: 0x06000224 RID: 548 RVA: 0x0000AF7E File Offset: 0x0000917E
		public SaveId SaveId { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000AF87 File Offset: 0x00009187
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000AF8F File Offset: 0x0000918F
		public int ElementCount { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000AF98 File Offset: 0x00009198
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0000AFA0 File Offset: 0x000091A0
		public ContainerType ContainerType { get; private set; }

		// Token: 0x06000229 RID: 553 RVA: 0x0000AFA9 File Offset: 0x000091A9
		public ContainerHeaderLoadData(LoadContext context, int id)
		{
			this.Context = context;
			this.Id = id;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000AFBF File Offset: 0x000091BF
		public bool GetObjectTypeDefinition()
		{
			this.TypeDefinition = this.Context.DefinitionContext.TryGetTypeDefinition(this.SaveId) as ContainerDefinition;
			return this.TypeDefinition != null;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000AFEC File Offset: 0x000091EC
		public void CreateObject()
		{
			Type type = this.TypeDefinition.Type;
			if (this.ContainerType == ContainerType.Array)
			{
				this.Target = Activator.CreateInstance(type, new object[] { this.ElementCount });
				return;
			}
			if (this.ContainerType == ContainerType.List)
			{
				this.Target = Activator.CreateInstance(typeof(MBList<>).MakeGenericType(type.GetGenericArguments()));
				return;
			}
			this.Target = Activator.CreateInstance(type);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000B068 File Offset: 0x00009268
		public void InitialieReaders(SaveEntryFolder saveEntryFolder)
		{
			BinaryReader binaryReader = saveEntryFolder.GetEntry(new EntryId(-1, SaveEntryExtension.Object)).GetBinaryReader();
			this.SaveId = SaveId.ReadSaveIdFrom(binaryReader);
			this.ContainerType = (ContainerType)binaryReader.ReadByte();
			this.ElementCount = binaryReader.ReadInt();
		}
	}
}
