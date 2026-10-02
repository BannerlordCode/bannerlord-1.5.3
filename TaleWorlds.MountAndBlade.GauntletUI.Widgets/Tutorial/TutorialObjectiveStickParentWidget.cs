using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x0200004E RID: 78
	public class TutorialObjectiveStickParentWidget : TextWidget
	{
		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x0000D89D File Offset: 0x0000BA9D
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x0000D8A5 File Offset: 0x0000BAA5
		public Widget StickMiddle { get; set; }

		// Token: 0x0600044C RID: 1100 RVA: 0x0000D8B0 File Offset: 0x0000BAB0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._animQueue.Count > 0)
			{
				base.ParentWidget.ParentWidget.AlphaFactor = 0.5f;
				this._animQueue.Peek().ForEach(delegate(TutorialObjectiveStickParentWidget.StickAnimStage a)
				{
					a.Tick(dt);
				});
				if (this._animQueue.Peek().All<TutorialObjectiveStickParentWidget.StickAnimStage>((TutorialObjectiveStickParentWidget.StickAnimStage a) => a.IsCompleted))
				{
					this._animQueue.Dequeue();
					return;
				}
			}
			else
			{
				this.UpdateAnimQueue();
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000D959 File Offset: 0x0000BB59
		public TutorialObjectiveStickParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000D96D File Offset: 0x0000BB6D
		private void ResetAnim()
		{
			base.PositionXOffset = 0f;
			base.PositionYOffset = 0f;
			this.SetGlobalAlphaRecursively(0f);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000D990 File Offset: 0x0000BB90
		private void UpdateAnimQueue()
		{
			this.ResetAnim();
			switch (this.MovementType)
			{
			case 1:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(-20f, 0f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(1f) });
				return;
			case 2:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(20f, 0f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(1f) });
				return;
			case 3:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(0f, -20f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(1f) });
				return;
			case 4:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(0f, 20f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(1f) });
				return;
			case 5:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(-20f, 0f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(2f) });
				return;
			case 6:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(20f, 0f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(2f) });
				return;
			case 7:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(0f, -20f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(2f) });
				return;
			case 8:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(0f, 20f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(2f) });
				return;
			default:
				return;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0000DF02 File Offset: 0x0000C102
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x0000DF0A File Offset: 0x0000C10A
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
					base.OnPropertyChanged(value, "MovementType");
				}
			}
		}

		// Token: 0x040001CA RID: 458
		private const float LongStayTime = 1f;

		// Token: 0x040001CB RID: 459
		private const float ShortStayTime = 0.1f;

		// Token: 0x040001CC RID: 460
		private const float FadeInTime = 0.15f;

		// Token: 0x040001CD RID: 461
		private const float FadeOutTime = 0.15f;

		// Token: 0x040001CE RID: 462
		private const float SingleMovementDirection = 20f;

		// Token: 0x040001CF RID: 463
		private const float MovementTime = 0.15f;

		// Token: 0x040001D0 RID: 464
		private const float ParentActiveAlpha = 0.5f;

		// Token: 0x040001D2 RID: 466
		private Queue<List<TutorialObjectiveStickParentWidget.StickAnimStage>> _animQueue = new Queue<List<TutorialObjectiveStickParentWidget.StickAnimStage>>();

		// Token: 0x040001D3 RID: 467
		private int _movementType;

		// Token: 0x020001AE RID: 430
		public class StickAnimStage
		{
			// Token: 0x1700077F RID: 1919
			// (get) Token: 0x06001554 RID: 5460 RVA: 0x0003A65C File Offset: 0x0003885C
			// (set) Token: 0x06001555 RID: 5461 RVA: 0x0003A664 File Offset: 0x00038864
			public bool IsCompleted { get; private set; }

			// Token: 0x17000780 RID: 1920
			// (get) Token: 0x06001556 RID: 5462 RVA: 0x0003A66D File Offset: 0x0003886D
			// (set) Token: 0x06001557 RID: 5463 RVA: 0x0003A675 File Offset: 0x00038875
			public float AnimTime { get; private set; }

			// Token: 0x17000781 RID: 1921
			// (get) Token: 0x06001558 RID: 5464 RVA: 0x0003A67E File Offset: 0x0003887E
			// (set) Token: 0x06001559 RID: 5465 RVA: 0x0003A686 File Offset: 0x00038886
			public Vec2 Direction { get; private set; }

			// Token: 0x17000782 RID: 1922
			// (get) Token: 0x0600155A RID: 5466 RVA: 0x0003A68F File Offset: 0x0003888F
			// (set) Token: 0x0600155B RID: 5467 RVA: 0x0003A697 File Offset: 0x00038897
			public TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes AnimType { get; private set; }

			// Token: 0x17000783 RID: 1923
			// (get) Token: 0x0600155C RID: 5468 RVA: 0x0003A6A0 File Offset: 0x000388A0
			// (set) Token: 0x0600155D RID: 5469 RVA: 0x0003A6A8 File Offset: 0x000388A8
			public Widget WidgetToManipulate { get; private set; }

			// Token: 0x0600155E RID: 5470 RVA: 0x0003A6B1 File Offset: 0x000388B1
			private StickAnimStage()
			{
			}

			// Token: 0x0600155F RID: 5471 RVA: 0x0003A6B9 File Offset: 0x000388B9
			internal static TutorialObjectiveStickParentWidget.StickAnimStage CreateMovementStage(float movementTime, Vec2 direction, Widget widgetToManipulate)
			{
				return new TutorialObjectiveStickParentWidget.StickAnimStage
				{
					AnimTime = movementTime,
					Direction = direction,
					AnimType = TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.Movement,
					WidgetToManipulate = widgetToManipulate
				};
			}

			// Token: 0x06001560 RID: 5472 RVA: 0x0003A6DC File Offset: 0x000388DC
			internal static TutorialObjectiveStickParentWidget.StickAnimStage CreateFadeInStage(float fadeInTime, Widget widgetToManipulate, bool isGlobal)
			{
				return new TutorialObjectiveStickParentWidget.StickAnimStage
				{
					AnimTime = fadeInTime,
					AnimType = (isGlobal ? TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.FadeInGlobal : TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.FadeInLocal),
					WidgetToManipulate = widgetToManipulate
				};
			}

			// Token: 0x06001561 RID: 5473 RVA: 0x0003A6FE File Offset: 0x000388FE
			internal static TutorialObjectiveStickParentWidget.StickAnimStage CreateStayStage(float stayTime)
			{
				return new TutorialObjectiveStickParentWidget.StickAnimStage
				{
					AnimTime = stayTime,
					AnimType = TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.Stay,
					WidgetToManipulate = null
				};
			}

			// Token: 0x06001562 RID: 5474 RVA: 0x0003A71C File Offset: 0x0003891C
			public void Tick(float dt)
			{
				float num = MathF.Clamp(this._totalTime / this.AnimTime, 0f, 1f);
				switch (this.AnimType)
				{
				case TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.Movement:
					this.WidgetToManipulate.PositionXOffset = ((this.Direction.X != 0f) ? MathF.Lerp(0f, this.Direction.X, num, 1E-05f) : 0f);
					this.WidgetToManipulate.PositionYOffset = ((this.Direction.Y != 0f) ? MathF.Lerp(0f, this.Direction.Y, num, 1E-05f) : 0f);
					this.IsCompleted = this._totalTime > this.AnimTime;
					break;
				case TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.FadeInLocal:
					this.WidgetToManipulate.AlphaFactor = num;
					this.IsCompleted = this.WidgetToManipulate.AlphaFactor > 0.98f;
					break;
				case TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.FadeOutLocal:
					this.WidgetToManipulate.AlphaFactor = 1f - num;
					this.IsCompleted = this.WidgetToManipulate.AlphaFactor < 0.02f;
					break;
				case TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.FadeInGlobal:
					this.WidgetToManipulate.SetGlobalAlphaRecursively(num);
					this.IsCompleted = this.WidgetToManipulate.AlphaFactor > 0.98f;
					break;
				case TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.FadeOutGlobal:
					this.WidgetToManipulate.SetGlobalAlphaRecursively(1f - num);
					this.IsCompleted = this.WidgetToManipulate.AlphaFactor < 0.02f;
					break;
				case TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.Stay:
					this.IsCompleted = this._totalTime > this.AnimTime;
					break;
				}
				this._totalTime += dt;
			}

			// Token: 0x04000A04 RID: 2564
			private float _totalTime;

			// Token: 0x020001DD RID: 477
			public enum AnimTypes
			{
				// Token: 0x04000A99 RID: 2713
				Movement,
				// Token: 0x04000A9A RID: 2714
				FadeInLocal,
				// Token: 0x04000A9B RID: 2715
				FadeOutLocal,
				// Token: 0x04000A9C RID: 2716
				FadeInGlobal,
				// Token: 0x04000A9D RID: 2717
				FadeOutGlobal,
				// Token: 0x04000A9E RID: 2718
				Stay
			}
		}
	}
}
