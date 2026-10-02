using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000112 RID: 274
	[Serializable]
	public struct CustomBattleId
	{
		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x0000757E File Offset: 0x0000577E
		// (set) Token: 0x060005F5 RID: 1525 RVA: 0x00007586 File Offset: 0x00005786
		[JsonProperty]
		public Guid Guid { get; private set; }

		// Token: 0x060005F6 RID: 1526 RVA: 0x0000758F File Offset: 0x0000578F
		public CustomBattleId(Guid guid)
		{
			this.Guid = guid;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00007598 File Offset: 0x00005798
		public static CustomBattleId NewGuid()
		{
			return new CustomBattleId(Guid.NewGuid());
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000075A4 File Offset: 0x000057A4
		public override string ToString()
		{
			return this.Guid.ToString();
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x000075C8 File Offset: 0x000057C8
		public byte[] ToByteArray()
		{
			return this.Guid.ToByteArray();
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000075E3 File Offset: 0x000057E3
		public static bool operator ==(CustomBattleId a, CustomBattleId b)
		{
			return a.Guid == b.Guid;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000075F8 File Offset: 0x000057F8
		public static bool operator !=(CustomBattleId a, CustomBattleId b)
		{
			return a.Guid != b.Guid;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00007610 File Offset: 0x00005810
		public override bool Equals(object o)
		{
			if (o != null && o is CustomBattleId)
			{
				CustomBattleId customBattleId = (CustomBattleId)o;
				return this.Guid.Equals(customBattleId.Guid);
			}
			return false;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00007648 File Offset: 0x00005848
		public override int GetHashCode()
		{
			return this.Guid.GetHashCode();
		}

		// Token: 0x04000242 RID: 578
		[JsonIgnore]
		public static CustomBattleId Empty = new CustomBattleId(Guid.Empty);
	}
}
