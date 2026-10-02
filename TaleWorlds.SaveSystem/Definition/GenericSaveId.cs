using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000062 RID: 98
	internal class GenericSaveId : SaveId
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0000EBBF File Offset: 0x0000CDBF
		// (set) Token: 0x06000350 RID: 848 RVA: 0x0000EBC7 File Offset: 0x0000CDC7
		public SaveId BaseId { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000EBD0 File Offset: 0x0000CDD0
		// (set) Token: 0x06000352 RID: 850 RVA: 0x0000EBD8 File Offset: 0x0000CDD8
		public SaveId[] GenericTypeIDs { get; set; }

		// Token: 0x06000353 RID: 851 RVA: 0x0000EBE1 File Offset: 0x0000CDE1
		public GenericSaveId(TypeSaveId baseId, SaveId[] saveIds)
		{
			this.BaseId = baseId;
			this.GenericTypeIDs = saveIds;
			this._stringId = this.CalculateStringId();
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000EC04 File Offset: 0x0000CE04
		private string CalculateStringId()
		{
			string text = "";
			for (int i = 0; i < this.GenericTypeIDs.Length; i++)
			{
				if (i != 0)
				{
					text += ",";
				}
				SaveId saveId = this.GenericTypeIDs[i];
				text += saveId.GetStringId();
			}
			return string.Concat(new string[]
			{
				"G(",
				this.BaseId.GetStringId(),
				")-(",
				text,
				")"
			});
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000EC84 File Offset: 0x0000CE84
		public override string GetStringId()
		{
			return this._stringId;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000EC8C File Offset: 0x0000CE8C
		public override void WriteTo(IWriter writer)
		{
			writer.WriteByte(1);
			this.BaseId.WriteTo(writer);
			writer.WriteByte((byte)this.GenericTypeIDs.Length);
			for (int i = 0; i < this.GenericTypeIDs.Length; i++)
			{
				this.GenericTypeIDs[i].WriteTo(writer);
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000ECDC File Offset: 0x0000CEDC
		public static GenericSaveId ReadFrom(IReader reader)
		{
			reader.ReadByte();
			TypeSaveId typeSaveId = TypeSaveId.ReadFrom(reader);
			byte b = reader.ReadByte();
			List<SaveId> list = new List<SaveId>();
			for (int i = 0; i < (int)b; i++)
			{
				SaveId saveId = null;
				byte b2 = reader.ReadByte();
				if (b2 == 0)
				{
					saveId = TypeSaveId.ReadFrom(reader);
				}
				else if (b2 == 1)
				{
					saveId = GenericSaveId.ReadFrom(reader);
				}
				else if (b2 == 2)
				{
					saveId = ContainerSaveId.ReadFrom(reader);
				}
				list.Add(saveId);
			}
			return new GenericSaveId(typeSaveId, list.ToArray());
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000ED5C File Offset: 0x0000CF5C
		public override int GetSizeInBytes()
		{
			int num = 2 + this.BaseId.GetSizeInBytes();
			for (int i = 0; i < this.GenericTypeIDs.Length; i++)
			{
				SaveId saveId = this.GenericTypeIDs[i];
				num += saveId.GetSizeInBytes();
			}
			return num;
		}

		// Token: 0x040000FF RID: 255
		private readonly string _stringId;
	}
}
