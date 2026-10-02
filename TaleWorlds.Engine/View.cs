using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200009D RID: 157
	[EngineClass("rglView")]
	public abstract class View : NativeObject
	{
		// Token: 0x06000DFA RID: 3578 RVA: 0x0000FD68 File Offset: 0x0000DF68
		internal View(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0000FD77 File Offset: 0x0000DF77
		public void SetScale(Vec2 scale)
		{
			EngineApplicationInterface.IView.SetScale(base.Pointer, scale.x, scale.y);
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0000FD95 File Offset: 0x0000DF95
		public void SetOffset(Vec2 offset)
		{
			EngineApplicationInterface.IView.SetOffset(base.Pointer, offset.x, offset.y);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0000FDB3 File Offset: 0x0000DFB3
		public void SetRenderOrder(int value)
		{
			EngineApplicationInterface.IView.SetRenderOrder(base.Pointer, value);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0000FDC6 File Offset: 0x0000DFC6
		public void SetRenderOption(View.ViewRenderOptions optionEnum, bool value)
		{
			EngineApplicationInterface.IView.SetRenderOption(base.Pointer, (int)optionEnum, value);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0000FDDA File Offset: 0x0000DFDA
		public void SetRenderTarget(Texture texture)
		{
			EngineApplicationInterface.IView.SetRenderTarget(base.Pointer, texture.Pointer);
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0000FDF2 File Offset: 0x0000DFF2
		public void SetDepthTarget(Texture texture)
		{
			EngineApplicationInterface.IView.SetDepthTarget(base.Pointer, texture.Pointer);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0000FE0A File Offset: 0x0000E00A
		public void DontClearBackground()
		{
			this.SetRenderOption(View.ViewRenderOptions.ClearColor, false);
			this.SetRenderOption(View.ViewRenderOptions.ClearDepth, false);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0000FE1C File Offset: 0x0000E01C
		public void SetClearColor(uint rgba)
		{
			EngineApplicationInterface.IView.SetClearColor(base.Pointer, rgba);
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0000FE2F File Offset: 0x0000E02F
		public void SetEnable(bool value)
		{
			EngineApplicationInterface.IView.SetEnable(base.Pointer, value);
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0000FE42 File Offset: 0x0000E042
		public void SetRenderOnDemand(bool value)
		{
			EngineApplicationInterface.IView.SetRenderOnDemand(base.Pointer, value);
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0000FE55 File Offset: 0x0000E055
		public void SetAutoDepthTargetCreation(bool value)
		{
			EngineApplicationInterface.IView.SetAutoDepthTargetCreation(base.Pointer, value);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0000FE68 File Offset: 0x0000E068
		public void SetSaveFinalResultToDisk(bool value)
		{
			EngineApplicationInterface.IView.SetSaveFinalResultToDisk(base.Pointer, value);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0000FE7B File Offset: 0x0000E07B
		public void SetFileNameToSaveResult(string name)
		{
			EngineApplicationInterface.IView.SetFileNameToSaveResult(base.Pointer, name);
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0000FE8E File Offset: 0x0000E08E
		public void SetFileTypeToSave(View.TextureSaveFormat format)
		{
			EngineApplicationInterface.IView.SetFileTypeToSave(base.Pointer, (int)format);
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0000FEA1 File Offset: 0x0000E0A1
		public void SetFilePathToSaveResult(string name)
		{
			EngineApplicationInterface.IView.SetFilePathToSaveResult(base.Pointer, name);
		}

		// Token: 0x020000DB RID: 219
		public enum TextureSaveFormat
		{
			// Token: 0x04000497 RID: 1175
			TextureTypeUnknown,
			// Token: 0x04000498 RID: 1176
			TextureTypeBmp,
			// Token: 0x04000499 RID: 1177
			TextureTypeJpg,
			// Token: 0x0400049A RID: 1178
			TextureTypePng,
			// Token: 0x0400049B RID: 1179
			TextureTypeDds,
			// Token: 0x0400049C RID: 1180
			TextureTypeTif,
			// Token: 0x0400049D RID: 1181
			TextureTypePsd,
			// Token: 0x0400049E RID: 1182
			TextureTypeRaw
		}

		// Token: 0x020000DC RID: 220
		public enum PostfxConfig : uint
		{
			// Token: 0x040004A0 RID: 1184
			pfx_config_bloom = 1U,
			// Token: 0x040004A1 RID: 1185
			pfx_config_sunshafts,
			// Token: 0x040004A2 RID: 1186
			pfx_config_motionblur = 4U,
			// Token: 0x040004A3 RID: 1187
			pfx_config_dof = 8U,
			// Token: 0x040004A4 RID: 1188
			pfx_config_tsao = 16U,
			// Token: 0x040004A5 RID: 1189
			pfx_config_fxaa = 64U,
			// Token: 0x040004A6 RID: 1190
			pfx_config_smaa = 128U,
			// Token: 0x040004A7 RID: 1191
			pfx_config_temporal_smaa = 256U,
			// Token: 0x040004A8 RID: 1192
			pfx_config_temporal_resolve = 512U,
			// Token: 0x040004A9 RID: 1193
			pfx_config_temporal_filter = 1024U,
			// Token: 0x040004AA RID: 1194
			pfx_config_contour = 2048U,
			// Token: 0x040004AB RID: 1195
			pfx_config_ssr = 4096U,
			// Token: 0x040004AC RID: 1196
			pfx_config_sssss = 8192U,
			// Token: 0x040004AD RID: 1197
			pfx_config_streaks = 16384U,
			// Token: 0x040004AE RID: 1198
			pfx_config_lens_flares = 32768U,
			// Token: 0x040004AF RID: 1199
			pfx_config_chromatic_aberration = 65536U,
			// Token: 0x040004B0 RID: 1200
			pfx_config_vignette = 131072U,
			// Token: 0x040004B1 RID: 1201
			pfx_config_sharpen = 262144U,
			// Token: 0x040004B2 RID: 1202
			pfx_config_grain = 524288U,
			// Token: 0x040004B3 RID: 1203
			pfx_config_temporal_shadow = 1048576U,
			// Token: 0x040004B4 RID: 1204
			pfx_config_editor_scene = 2097152U,
			// Token: 0x040004B5 RID: 1205
			pfx_config_custom1 = 16777216U,
			// Token: 0x040004B6 RID: 1206
			pfx_config_custom2 = 33554432U,
			// Token: 0x040004B7 RID: 1207
			pfx_config_custom3 = 67108864U,
			// Token: 0x040004B8 RID: 1208
			pfx_config_custom4 = 134217728U,
			// Token: 0x040004B9 RID: 1209
			pfx_config_hexagon_vignette = 268435456U,
			// Token: 0x040004BA RID: 1210
			pfx_config_screen_rt_injection = 536870912U,
			// Token: 0x040004BB RID: 1211
			pfx_config_high_dof = 1073741824U,
			// Token: 0x040004BC RID: 1212
			pfx_lower_bound = 1U,
			// Token: 0x040004BD RID: 1213
			pfx_upper_bound = 536870912U
		}

		// Token: 0x020000DD RID: 221
		public enum ViewRenderOptions
		{
			// Token: 0x040004BF RID: 1215
			ClearColor,
			// Token: 0x040004C0 RID: 1216
			ClearDepth
		}
	}
}
