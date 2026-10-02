using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200005A RID: 90
	public class ContainerSaveId : SaveId
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000D6BE File Offset: 0x0000B8BE
		// (set) Token: 0x06000308 RID: 776 RVA: 0x0000D6C6 File Offset: 0x0000B8C6
		public ContainerType ContainerType { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000D6CF File Offset: 0x0000B8CF
		// (set) Token: 0x0600030A RID: 778 RVA: 0x0000D6D7 File Offset: 0x0000B8D7
		public SaveId KeyId { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600030B RID: 779 RVA: 0x0000D6E0 File Offset: 0x0000B8E0
		// (set) Token: 0x0600030C RID: 780 RVA: 0x0000D6E8 File Offset: 0x0000B8E8
		public SaveId ValueId { get; set; }

		// Token: 0x0600030D RID: 781 RVA: 0x0000D6F1 File Offset: 0x0000B8F1
		public ContainerSaveId(ContainerType containerType, SaveId elementId)
		{
			this.ContainerType = containerType;
			this.KeyId = elementId;
			this._stringId = this.CalculateStringId();
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000D713 File Offset: 0x0000B913
		public ContainerSaveId(ContainerType containerType, SaveId keyId, SaveId valueId)
		{
			this.ContainerType = containerType;
			this.KeyId = keyId;
			this.ValueId = valueId;
			this._stringId = this.CalculateStringId();
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000D73C File Offset: 0x0000B93C
		private string CalculateStringId()
		{
			string text;
			if (this.ContainerType == ContainerType.Dictionary)
			{
				string stringId = this.KeyId.GetStringId();
				string stringId2 = this.ValueId.GetStringId();
				text = string.Concat(new object[]
				{
					"C(",
					(int)this.ContainerType,
					")-(",
					stringId,
					",",
					stringId2,
					")"
				});
			}
			else
			{
				string stringId3 = this.KeyId.GetStringId();
				text = string.Concat(new object[]
				{
					"C(",
					(int)this.ContainerType,
					")-(",
					stringId3,
					")"
				});
			}
			return text;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000D7F7 File Offset: 0x0000B9F7
		public override string GetStringId()
		{
			return this._stringId;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000D7FF File Offset: 0x0000B9FF
		public override void WriteTo(IWriter writer)
		{
			writer.WriteByte(2);
			writer.WriteByte((byte)this.ContainerType);
			this.KeyId.WriteTo(writer);
			if (this.ContainerType == ContainerType.Dictionary)
			{
				this.ValueId.WriteTo(writer);
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000D838 File Offset: 0x0000BA38
		public static ContainerSaveId ReadFrom(IReader reader)
		{
			ContainerType containerType = (ContainerType)reader.ReadByte();
			int num = ((containerType == ContainerType.Dictionary) ? 2 : 1);
			List<SaveId> list = new List<SaveId>();
			for (int i = 0; i < num; i++)
			{
				SaveId saveId = null;
				byte b = reader.ReadByte();
				if (b == 0)
				{
					saveId = TypeSaveId.ReadFrom(reader);
				}
				else if (b == 1)
				{
					saveId = GenericSaveId.ReadFrom(reader);
				}
				else if (b == 2)
				{
					saveId = ContainerSaveId.ReadFrom(reader);
				}
				list.Add(saveId);
			}
			SaveId saveId2 = list[0];
			SaveId saveId3 = ((list.Count > 1) ? list[1] : null);
			return new ContainerSaveId(containerType, saveId2, saveId3);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000D8D0 File Offset: 0x0000BAD0
		public override int GetSizeInBytes()
		{
			int num = 2 + this.KeyId.GetSizeInBytes();
			if (this.ContainerType == ContainerType.Dictionary)
			{
				num += this.ValueId.GetSizeInBytes();
			}
			return num;
		}

		// Token: 0x040000E0 RID: 224
		private readonly string _stringId;
	}
}
