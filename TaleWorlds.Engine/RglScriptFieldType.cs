using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000058 RID: 88
	[EngineStruct("rglScript_field_type", false, null)]
	public enum RglScriptFieldType
	{
		// Token: 0x040000BD RID: 189
		RglSftInvalid = -1,
		// Token: 0x040000BE RID: 190
		RglSftString,
		// Token: 0x040000BF RID: 191
		RglSftDouble,
		// Token: 0x040000C0 RID: 192
		RglSftFloat,
		// Token: 0x040000C1 RID: 193
		RglSftBool,
		// Token: 0x040000C2 RID: 194
		RglSftInt,
		// Token: 0x040000C3 RID: 195
		[CustomEngineStructMemberData("Rgl_sft_vec3")]
		RglSftVec3,
		// Token: 0x040000C4 RID: 196
		RglSftEntity,
		// Token: 0x040000C5 RID: 197
		RglSftTexture,
		// Token: 0x040000C6 RID: 198
		RglSftMesh,
		// Token: 0x040000C7 RID: 199
		RglSftEnum,
		// Token: 0x040000C8 RID: 200
		RglSftMaterial,
		// Token: 0x040000C9 RID: 201
		RglSftButton,
		// Token: 0x040000CA RID: 202
		RglSftColor,
		// Token: 0x040000CB RID: 203
		RglSftMatrixFrame
	}
}
