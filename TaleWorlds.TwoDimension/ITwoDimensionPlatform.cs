using System;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000021 RID: 33
	public interface ITwoDimensionPlatform
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000142 RID: 322
		float Width { get; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000143 RID: 323
		float Height { get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000144 RID: 324
		float ReferenceWidth { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000145 RID: 325
		float ReferenceHeight { get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000146 RID: 326
		float ApplicationTime { get; }

		// Token: 0x06000147 RID: 327
		void OnFrameBegin();

		// Token: 0x06000148 RID: 328
		void OnFrameEnd();

		// Token: 0x06000149 RID: 329
		void Clear();

		// Token: 0x0600014A RID: 330
		void DrawImage(SimpleMaterial material, in ImageDrawObject drawObject2D, int layer);

		// Token: 0x0600014B RID: 331
		void DrawText(TextMaterial material, in TextDrawObject drawObject2D, int layer);

		// Token: 0x0600014C RID: 332
		void SetScissor(ScissorTestInfo scissorTestInfo);

		// Token: 0x0600014D RID: 333
		void ResetScissors();

		// Token: 0x0600014E RID: 334
		void PlaySound(string soundName);

		// Token: 0x0600014F RID: 335
		void CreateSoundEvent(string soundName);

		// Token: 0x06000150 RID: 336
		void PlaySoundEvent(string soundName);

		// Token: 0x06000151 RID: 337
		void StopAndRemoveSoundEvent(string soundName);

		// Token: 0x06000152 RID: 338
		void OpenOnScreenKeyboard(string initialText, string descriptionText, int maxLength, int keyboardTypeEnum);

		// Token: 0x06000153 RID: 339
		void BeginDebugPanel(string panelTitle);

		// Token: 0x06000154 RID: 340
		void EndDebugPanel();

		// Token: 0x06000155 RID: 341
		void DrawDebugText(string text);

		// Token: 0x06000156 RID: 342
		bool DrawDebugTreeNode(string text);

		// Token: 0x06000157 RID: 343
		void PopDebugTreeNode();

		// Token: 0x06000158 RID: 344
		void DrawCheckbox(string label, ref bool isChecked);

		// Token: 0x06000159 RID: 345
		bool IsDebugItemHovered();

		// Token: 0x0600015A RID: 346
		bool IsDebugModeEnabled();
	}
}
