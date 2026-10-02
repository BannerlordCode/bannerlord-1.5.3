using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Multiplayer
{
	// Token: 0x0200003A RID: 58
	public class MPChatLineVM : ViewModel
	{
		// Token: 0x060004CB RID: 1227 RVA: 0x00013030 File Offset: 0x00011230
		public MPChatLineVM(string chatLine, Color color, string category)
		{
			this.ChatLine = chatLine;
			this.Color = color;
			this.Alpha = 1f;
			this.Category = category;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00013058 File Offset: 0x00011258
		public void HandleFading(float dt)
		{
			this._timeSinceCreation += dt;
			this.RefreshAlpha();
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001306E File Offset: 0x0001126E
		private void RefreshAlpha()
		{
			if (this._forcedVisible)
			{
				this.Alpha = 1f;
				return;
			}
			this.Alpha = this.GetActualAlpha();
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00013090 File Offset: 0x00011290
		public void ForceInvisible()
		{
			this._timeSinceCreation = 10.5f;
			this.Alpha = 0f;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x000130A8 File Offset: 0x000112A8
		private float GetActualAlpha()
		{
			if (this._timeSinceCreation >= 10f)
			{
				return MBMath.ClampFloat(1f - (this._timeSinceCreation - 10f) / 0.5f, 0f, 1f);
			}
			return 1f;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x000130E4 File Offset: 0x000112E4
		public void ToggleForceVisible(bool visible)
		{
			this._forcedVisible = visible;
			this.RefreshAlpha();
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x000130F3 File Offset: 0x000112F3
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x000130FB File Offset: 0x000112FB
		[DataSourceProperty]
		public string ChatLine
		{
			get
			{
				return this._chatLine;
			}
			set
			{
				if (this._chatLine != value)
				{
					this._chatLine = value;
					base.OnPropertyChangedWithValue<string>(value, "ChatLine");
				}
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x0001311E File Offset: 0x0001131E
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00013126 File Offset: 0x00011326
		[DataSourceProperty]
		public Color Color
		{
			get
			{
				return this._color;
			}
			set
			{
				if (this._color != value)
				{
					this._color = value;
					base.OnPropertyChangedWithValue(value, "Color");
				}
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00013149 File Offset: 0x00011349
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00013151 File Offset: 0x00011351
		[DataSourceProperty]
		public float Alpha
		{
			get
			{
				return this._alpha;
			}
			set
			{
				if (this._alpha != value)
				{
					this._alpha = value;
					base.OnPropertyChangedWithValue(value, "Alpha");
				}
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0001316F File Offset: 0x0001136F
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x00013177 File Offset: 0x00011377
		[DataSourceProperty]
		public string Category
		{
			get
			{
				return this._category;
			}
			set
			{
				if (this._category != value)
				{
					this._category = value;
					base.OnPropertyChangedWithValue<string>(value, "Category");
				}
			}
		}

		// Token: 0x04000232 RID: 562
		private bool _forcedVisible;

		// Token: 0x04000233 RID: 563
		private string _category;

		// Token: 0x04000234 RID: 564
		private const float ChatVisibilityDuration = 10f;

		// Token: 0x04000235 RID: 565
		private const float ChatFadeOutDuration = 0.5f;

		// Token: 0x04000236 RID: 566
		private float _timeSinceCreation;

		// Token: 0x04000237 RID: 567
		private string _chatLine;

		// Token: 0x04000238 RID: 568
		private Color _color;

		// Token: 0x04000239 RID: 569
		private float _alpha;
	}
}
