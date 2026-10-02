using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000010 RID: 16
	public class TwoDimensionPlatform : ITwoDimensionPlatform, ITwoDimensionResourceContext
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00005F84 File Offset: 0x00004184
		float ITwoDimensionPlatform.Width
		{
			get
			{
				return (float)this._form.Width;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00005F92 File Offset: 0x00004192
		float ITwoDimensionPlatform.Height
		{
			get
			{
				return (float)this._form.Height;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005FA0 File Offset: 0x000041A0
		float ITwoDimensionPlatform.ReferenceWidth
		{
			get
			{
				return 1154f;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00005FA7 File Offset: 0x000041A7
		float ITwoDimensionPlatform.ReferenceHeight
		{
			get
			{
				return 701f;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005FAE File Offset: 0x000041AE
		float ITwoDimensionPlatform.ApplicationTime
		{
			get
			{
				return (float)Environment.TickCount;
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005FB6 File Offset: 0x000041B6
		public TwoDimensionPlatform(GraphicsForm form, bool isAssetsUnderDefaultFolders)
		{
			this._form = form;
			this._isAssetsUnderDefaultFolders = isAssetsUnderDefaultFolders;
			this._graphicsContext = this._form.GraphicsContext;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005FDD File Offset: 0x000041DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ITwoDimensionPlatform.DrawImage(SimpleMaterial material, in ImageDrawObject drawObject2D, int layer)
		{
			this._graphicsContext.DrawImage(material, in drawObject2D);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005FEC File Offset: 0x000041EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ITwoDimensionPlatform.DrawText(TextMaterial material, in TextDrawObject drawObject2D, int layer)
		{
			this._graphicsContext.DrawText(material, in drawObject2D);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005FFB File Offset: 0x000041FB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ITwoDimensionPlatform.OnFrameBegin()
		{
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005FFD File Offset: 0x000041FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ITwoDimensionPlatform.OnFrameEnd()
		{
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005FFF File Offset: 0x000041FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ITwoDimensionPlatform.Clear()
		{
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006004 File Offset: 0x00004204
		Texture ITwoDimensionResourceContext.LoadTexture(ResourceDepot resourceDepot, string name)
		{
			string text = name;
			if (!this._isAssetsUnderDefaultFolders)
			{
				string[] array = name.Split(new char[] { '\\' });
				text = array[array.Length - 1];
			}
			DirectXTexture directXTexture = new DirectXTexture();
			this._graphicsContext.LoadTextureUsing(directXTexture, resourceDepot, text);
			return new Texture(directXTexture);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000604D File Offset: 0x0000424D
		void ITwoDimensionPlatform.PlaySound(string soundName)
		{
			Debug.Print("Playing sound: " + soundName, 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000606B File Offset: 0x0000426B
		void ITwoDimensionPlatform.SetScissor(ScissorTestInfo scissorTestInfo)
		{
			this._graphicsContext.SetScissor(scissorTestInfo);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00006079 File Offset: 0x00004279
		void ITwoDimensionPlatform.ResetScissors()
		{
			this._graphicsContext.ResetScissor();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006086 File Offset: 0x00004286
		void ITwoDimensionPlatform.CreateSoundEvent(string soundName)
		{
			Debug.Print("Created sound event: " + soundName, 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000060A4 File Offset: 0x000042A4
		void ITwoDimensionPlatform.StopAndRemoveSoundEvent(string soundName)
		{
			Debug.Print("Stopped sound event: " + soundName, 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000060C2 File Offset: 0x000042C2
		void ITwoDimensionPlatform.PlaySoundEvent(string soundName)
		{
			Debug.Print("Played sound event: " + soundName, 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000060E0 File Offset: 0x000042E0
		void ITwoDimensionPlatform.OpenOnScreenKeyboard(string initialText, string descriptionText, int maxLength, int keyboardTypeEnum)
		{
			Debug.Print("Opened on-screen keyboard", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000060F8 File Offset: 0x000042F8
		void ITwoDimensionPlatform.BeginDebugPanel(string panelTitle)
		{
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000060FA File Offset: 0x000042FA
		void ITwoDimensionPlatform.EndDebugPanel()
		{
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000060FC File Offset: 0x000042FC
		void ITwoDimensionPlatform.DrawDebugText(string text)
		{
			Debug.Print(text, 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006110 File Offset: 0x00004310
		bool ITwoDimensionPlatform.IsDebugModeEnabled()
		{
			return false;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006113 File Offset: 0x00004313
		bool ITwoDimensionPlatform.DrawDebugTreeNode(string text)
		{
			return false;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006116 File Offset: 0x00004316
		void ITwoDimensionPlatform.DrawCheckbox(string label, ref bool isChecked)
		{
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006118 File Offset: 0x00004318
		bool ITwoDimensionPlatform.IsDebugItemHovered()
		{
			return false;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000611B File Offset: 0x0000431B
		void ITwoDimensionPlatform.PopDebugTreeNode()
		{
		}

		// Token: 0x04000058 RID: 88
		private DirectXGraphicsContext _graphicsContext;

		// Token: 0x04000059 RID: 89
		private GraphicsForm _form;

		// Token: 0x0400005A RID: 90
		private bool _isAssetsUnderDefaultFolders;
	}
}
