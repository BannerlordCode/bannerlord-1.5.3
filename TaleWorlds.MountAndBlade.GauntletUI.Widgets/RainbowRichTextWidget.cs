using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000039 RID: 57
	public class RainbowRichTextWidget : RichTextWidget
	{
		// Token: 0x06000355 RID: 853 RVA: 0x0000AB26 File Offset: 0x00008D26
		public RainbowRichTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000AB3C File Offset: 0x00008D3C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.Brush.FontColor = Color.Lerp(base.ReadOnlyBrush.FontColor, this.targetColor, dt);
			if (base.Brush.FontColor.ToVec3().Distance(this.targetColor.ToVec3()) < 1f)
			{
				Random random = new Random();
				this.targetColor = Color.FromVector3(new Vector3((float)random.Next(255), (float)random.Next(255), (float)random.Next(255)));
			}
		}

		// Token: 0x04000156 RID: 342
		private Color targetColor = Color.White;
	}
}
