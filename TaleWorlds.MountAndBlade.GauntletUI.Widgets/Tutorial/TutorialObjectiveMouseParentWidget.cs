using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x0200004D RID: 77
	public class TutorialObjectiveMouseParentWidget : Widget
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x0000D404 File Offset: 0x0000B604
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x0000D40C File Offset: 0x0000B60C
		public BrushWidget MouseBodyWidget { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x0000D415 File Offset: 0x0000B615
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x0000D41D File Offset: 0x0000B61D
		public BrushWidget MouseLeftClickWidget { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x0000D426 File Offset: 0x0000B626
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x0000D42E File Offset: 0x0000B62E
		public BrushWidget MouseRightClickWidget { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x0000D437 File Offset: 0x0000B637
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x0000D43F File Offset: 0x0000B63F
		public BrushWidget MouseMiddleClickWidget { get; set; }

		// Token: 0x06000442 RID: 1090 RVA: 0x0000D448 File Offset: 0x0000B648
		public TutorialObjectiveMouseParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000D454 File Offset: 0x0000B654
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._animationsSet)
			{
				if (this.MouseBodyWidget == null || this.MouseLeftClickWidget == null || this.MouseRightClickWidget == null || this.MouseMiddleClickWidget == null)
				{
					return;
				}
				this._animationsSet = true;
				BrushAnimation animation2 = this.MouseLeftClickWidget.Brush.GetAnimation("BlinkAnimation");
				using (IEnumerator<BrushAnimation> enumerator = this.MouseLeftClickWidget.Brush.GetAnimations().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BrushAnimation animation3 = enumerator.Current;
						if (animation3 != animation2)
						{
							Action<BrushAnimationProperty> <>9__0;
							foreach (BrushLayerAnimation brushLayerAnimation in animation2.GetLayerAnimations())
							{
								List<BrushAnimationProperty> list = brushLayerAnimation.Collections.ToList<BrushAnimationProperty>();
								Action<BrushAnimationProperty> action;
								if ((action = <>9__0) == null)
								{
									action = (<>9__0 = delegate(BrushAnimationProperty x)
									{
										animation3.AddAnimationProperty(x);
									});
								}
								list.ForEach(action);
							}
						}
					}
				}
				using (IEnumerator<BrushAnimation> enumerator = this.MouseRightClickWidget.Brush.GetAnimations().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BrushAnimation animation4 = enumerator.Current;
						if (animation4 != animation2)
						{
							Action<BrushAnimationProperty> <>9__1;
							foreach (BrushLayerAnimation brushLayerAnimation2 in animation2.GetLayerAnimations())
							{
								List<BrushAnimationProperty> list2 = brushLayerAnimation2.Collections.ToList<BrushAnimationProperty>();
								Action<BrushAnimationProperty> action2;
								if ((action2 = <>9__1) == null)
								{
									action2 = (<>9__1 = delegate(BrushAnimationProperty x)
									{
										animation4.AddAnimationProperty(x);
									});
								}
								list2.ForEach(action2);
							}
						}
					}
				}
				using (IEnumerator<BrushAnimation> enumerator = this.MouseMiddleClickWidget.Brush.GetAnimations().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BrushAnimation animation = enumerator.Current;
						if (animation != animation2)
						{
							Action<BrushAnimationProperty> <>9__2;
							foreach (BrushLayerAnimation brushLayerAnimation3 in animation2.GetLayerAnimations())
							{
								List<BrushAnimationProperty> list3 = brushLayerAnimation3.Collections.ToList<BrushAnimationProperty>();
								Action<BrushAnimationProperty> action3;
								if ((action3 = <>9__2) == null)
								{
									action3 = (<>9__2 = delegate(BrushAnimationProperty x)
									{
										animation.AddAnimationProperty(x);
									});
								}
								list3.ForEach(action3);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000D6EC File Offset: 0x0000B8EC
		private void DecideMovement()
		{
			switch (this.MovementType)
			{
			case 1:
				this.MouseBodyWidget.SetState("Left");
				return;
			case 2:
				this.MouseBodyWidget.SetState("Right");
				return;
			case 3:
				this.MouseBodyWidget.SetState("Up");
				return;
			case 4:
				this.MouseBodyWidget.SetState("Down");
				return;
			default:
				this.MouseBodyWidget.SetState("Default");
				return;
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000D770 File Offset: 0x0000B970
		private void DecideClick()
		{
			string keyId = this.KeyId;
			if (keyId == "mouse_left_click")
			{
				this.MouseLeftClickWidget.IsVisible = true;
				this.MouseMiddleClickWidget.IsVisible = false;
				this.MouseRightClickWidget.IsVisible = false;
				return;
			}
			if (keyId == "mouse_middle_click")
			{
				this.MouseLeftClickWidget.IsVisible = false;
				this.MouseMiddleClickWidget.IsVisible = true;
				this.MouseRightClickWidget.IsVisible = false;
				return;
			}
			if (!(keyId == "mouse_right_click"))
			{
				this.MouseLeftClickWidget.IsVisible = false;
				this.MouseMiddleClickWidget.IsVisible = false;
				this.MouseRightClickWidget.IsVisible = false;
				return;
			}
			this.MouseLeftClickWidget.IsVisible = false;
			this.MouseMiddleClickWidget.IsVisible = false;
			this.MouseRightClickWidget.IsVisible = true;
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x0000D840 File Offset: 0x0000BA40
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x0000D848 File Offset: 0x0000BA48
		[Editor(false)]
		public string KeyId
		{
			get
			{
				return this._keyId;
			}
			set
			{
				if (value != this._keyId)
				{
					this._keyId = value;
					this.DecideClick();
					base.OnPropertyChanged<string>(value, "KeyId");
				}
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0000D871 File Offset: 0x0000BA71
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x0000D879 File Offset: 0x0000BA79
		[Editor(false)]
		public int MovementType
		{
			get
			{
				return this._movementType;
			}
			set
			{
				if (value != this._movementType)
				{
					this._movementType = value;
					this.DecideMovement();
					base.OnPropertyChanged(value, "MovementType");
				}
			}
		}

		// Token: 0x040001C7 RID: 455
		private bool _animationsSet;

		// Token: 0x040001C8 RID: 456
		private string _keyId;

		// Token: 0x040001C9 RID: 457
		private int _movementType;

		// Token: 0x020001AA RID: 426
		public enum MovementTypes
		{
			// Token: 0x040009F4 RID: 2548
			None,
			// Token: 0x040009F5 RID: 2549
			MoveLeft,
			// Token: 0x040009F6 RID: 2550
			MoveRight,
			// Token: 0x040009F7 RID: 2551
			MoveUp,
			// Token: 0x040009F8 RID: 2552
			MoveDown
		}
	}
}
