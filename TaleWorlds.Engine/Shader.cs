using System;

namespace TaleWorlds.Engine
{
	// Token: 0x0200008A RID: 138
	public sealed class Shader : Resource
	{
		// Token: 0x06000C78 RID: 3192 RVA: 0x0000DED0 File Offset: 0x0000C0D0
		internal Shader(UIntPtr ptr)
			: base(ptr)
		{
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0000DED9 File Offset: 0x0000C0D9
		public static Shader GetFromResource(string shaderName)
		{
			return EngineApplicationInterface.IShader.GetFromResource(shaderName);
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x0000DEE6 File Offset: 0x0000C0E6
		public string Name
		{
			get
			{
				return EngineApplicationInterface.IShader.GetName(base.Pointer);
			}
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0000DEF8 File Offset: 0x0000C0F8
		public ulong GetMaterialShaderFlagMask(string flagName, bool showErrors = true)
		{
			return EngineApplicationInterface.IShader.GetMaterialShaderFlagMask(base.Pointer, flagName, showErrors);
		}
	}
}
