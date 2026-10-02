using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Conversation
{
	// Token: 0x02000174 RID: 372
	public class ConversationPersuasionProgressRichTextWidget : RichTextWidget
	{
		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x00035599 File Offset: 0x00033799
		// (set) Token: 0x06001392 RID: 5010 RVA: 0x000355A1 File Offset: 0x000337A1
		public float FadeInTime { get; set; } = 1f;

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06001393 RID: 5011 RVA: 0x000355AA File Offset: 0x000337AA
		// (set) Token: 0x06001394 RID: 5012 RVA: 0x000355B2 File Offset: 0x000337B2
		public float FadeOutTime { get; set; } = 1f;

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x000355BB File Offset: 0x000337BB
		// (set) Token: 0x06001396 RID: 5014 RVA: 0x000355C3 File Offset: 0x000337C3
		public float StayTime { get; set; } = 2.5f;

		// Token: 0x06001397 RID: 5015 RVA: 0x000355CC File Offset: 0x000337CC
		public ConversationPersuasionProgressRichTextWidget(UIContext context)
			: base(context)
		{
			base.PropertyChanged += this.OnSelfPropertyChanged;
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00035620 File Offset: 0x00033820
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._startTime == -1f)
			{
				this.SetGlobalAlphaRecursively(0f);
				return;
			}
			float num;
			if (base.EventManager.Time - this._startTime < this.FadeInTime)
			{
				num = Mathf.Lerp(0f, 1f, (base.EventManager.Time - this._startTime) / this.FadeInTime);
			}
			else if (base.EventManager.Time - this._startTime < this.StayTime + this.FadeInTime)
			{
				num = 1f;
			}
			else
			{
				num = Mathf.Lerp(base.ReadOnlyBrush.GlobalAlphaFactor, 0f, (base.EventManager.Time - (this._startTime + this.StayTime + this.FadeInTime)) / this.FadeOutTime);
				if (base.ReadOnlyBrush.GlobalAlphaFactor <= 0.001f)
				{
					this._startTime = -1f;
				}
			}
			this.SetGlobalAlphaRecursively(num);
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00035725 File Offset: 0x00033925
		private void OnSelfPropertyChanged(PropertyOwnerObject arg1, string propertyName, object newState)
		{
			if (propertyName == "Text" && !string.IsNullOrEmpty(newState as string))
			{
				this._startTime = base.EventManager.Time;
			}
		}

		// Token: 0x040008EB RID: 2283
		private float _startTime = -1f;
	}
}
