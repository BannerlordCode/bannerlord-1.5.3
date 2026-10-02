using System;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200006E RID: 110
	public class TypeDefinitionBase
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x000115B4 File Offset: 0x0000F7B4
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x000115BC File Offset: 0x0000F7BC
		public SaveId SaveId { get; private set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x000115C5 File Offset: 0x0000F7C5
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x000115CD File Offset: 0x0000F7CD
		public Type Type { get; private set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x000115D6 File Offset: 0x0000F7D6
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x000115DE File Offset: 0x0000F7DE
		public byte TypeLevel { get; private set; }

		// Token: 0x060003C8 RID: 968 RVA: 0x000115E7 File Offset: 0x0000F7E7
		protected TypeDefinitionBase(Type type, SaveId saveId)
		{
			this.Type = type;
			this.SaveId = saveId;
			this.TypeLevel = TypeDefinitionBase.GetClassLevel(type);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001160C File Offset: 0x0000F80C
		public static byte GetClassLevel(Type type)
		{
			byte b = 1;
			if (type.IsClass)
			{
				Type type2 = type;
				while (type2 != typeof(object))
				{
					b += 1;
					type2 = type2.BaseType;
				}
			}
			return b;
		}
	}
}
