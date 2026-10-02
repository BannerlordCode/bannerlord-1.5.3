using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200008F RID: 143
	[EngineStruct("Managed_sound_event_parameter", false, null)]
	public struct SoundEventParameter
	{
		// Token: 0x06000CD8 RID: 3288 RVA: 0x0000E6FD File Offset: 0x0000C8FD
		public SoundEventParameter(string paramName, float value)
		{
			this.ParamName = paramName;
			this.Value = value;
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0000E70D File Offset: 0x0000C90D
		public void Update(string paramName, float value)
		{
			this.ParamName = paramName;
			this.Value = value;
		}

		// Token: 0x040001CA RID: 458
		[CustomEngineStructMemberData("str_id")]
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		internal string ParamName;

		// Token: 0x040001CB RID: 459
		internal float Value;
	}
}
