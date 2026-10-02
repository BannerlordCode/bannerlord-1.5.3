using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000036 RID: 54
	public class TwoDimensionContext
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000966E File Offset: 0x0000786E
		public float Width
		{
			get
			{
				return this.Platform.Width;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000967B File Offset: 0x0000787B
		public float Height
		{
			get
			{
				return this.Platform.Height;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600026F RID: 623 RVA: 0x00009688 File Offset: 0x00007888
		// (set) Token: 0x06000270 RID: 624 RVA: 0x00009690 File Offset: 0x00007890
		public ITwoDimensionPlatform Platform { get; private set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000271 RID: 625 RVA: 0x00009699 File Offset: 0x00007899
		// (set) Token: 0x06000272 RID: 626 RVA: 0x000096A1 File Offset: 0x000078A1
		public ITwoDimensionResourceContext ResourceContext { get; private set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000273 RID: 627 RVA: 0x000096AA File Offset: 0x000078AA
		// (set) Token: 0x06000274 RID: 628 RVA: 0x000096B2 File Offset: 0x000078B2
		public ResourceDepot ResourceDepot { get; private set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000275 RID: 629 RVA: 0x000096BB File Offset: 0x000078BB
		public bool IsDebugModeEnabled
		{
			get
			{
				return this.Platform.IsDebugModeEnabled();
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000096C8 File Offset: 0x000078C8
		public TwoDimensionContext(ITwoDimensionPlatform platform, ITwoDimensionResourceContext resourceContext, ResourceDepot resourceDepot)
		{
			this.ResourceDepot = resourceDepot;
			this.Platform = platform;
			this.ResourceContext = resourceContext;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000096E5 File Offset: 0x000078E5
		public void PlaySound(string soundName)
		{
			this.Platform.PlaySound(soundName);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x000096F3 File Offset: 0x000078F3
		public void CreateSoundEvent(string soundName)
		{
			this.Platform.CreateSoundEvent(soundName);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00009701 File Offset: 0x00007901
		public void StopAndRemoveSoundEvent(string soundName)
		{
			this.Platform.StopAndRemoveSoundEvent(soundName);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000970F File Offset: 0x0000790F
		public void PlaySoundEvent(string soundName)
		{
			this.Platform.PlaySoundEvent(soundName);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000971D File Offset: 0x0000791D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawImage(SimpleMaterial material, in ImageDrawObject drawObject2D, int layer = 0)
		{
			this.Platform.DrawImage(material, in drawObject2D, layer);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000972D File Offset: 0x0000792D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawText(TextMaterial material, in TextDrawObject drawObject2D, int layer = 0)
		{
			this.Platform.DrawText(material, in drawObject2D, layer);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000973D File Offset: 0x0000793D
		public void BeginDebugPanel(string panelTitle)
		{
			this.Platform.BeginDebugPanel(panelTitle);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000974B File Offset: 0x0000794B
		public void EndDebugPanel()
		{
			this.Platform.EndDebugPanel();
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00009758 File Offset: 0x00007958
		public void DrawDebugText(string text)
		{
			this.Platform.DrawDebugText(text);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00009766 File Offset: 0x00007966
		public bool DrawDebugTreeNode(string text)
		{
			return this.Platform.DrawDebugTreeNode(text);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00009774 File Offset: 0x00007974
		public void PopDebugTreeNode()
		{
			this.Platform.PopDebugTreeNode();
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00009781 File Offset: 0x00007981
		public void DrawCheckbox(string label, ref bool isChecked)
		{
			this.Platform.DrawCheckbox(label, ref isChecked);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00009790 File Offset: 0x00007990
		public bool IsDebugItemHovered()
		{
			return this.Platform.IsDebugItemHovered();
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000979D File Offset: 0x0000799D
		public Texture LoadTexture(string name)
		{
			return this.ResourceContext.LoadTexture(this.ResourceDepot, name);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x000097B1 File Offset: 0x000079B1
		public void SetScissor(ScissorTestInfo scissor)
		{
			this.Platform.SetScissor(scissor);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x000097BF File Offset: 0x000079BF
		public void ResetScissor()
		{
			this.Platform.ResetScissors();
		}
	}
}
