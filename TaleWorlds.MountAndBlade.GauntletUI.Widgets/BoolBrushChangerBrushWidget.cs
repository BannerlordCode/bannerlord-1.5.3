using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000007 RID: 7
	public class BoolBrushChangerBrushWidget : BrushWidget
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00002512 File Offset: 0x00000712
		public BoolBrushChangerBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000251B File Offset: 0x0000071B
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialUpdateHandled)
			{
				this.OnBooleanUpdated();
				this._initialUpdateHandled = true;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000253C File Offset: 0x0000073C
		private void OnBooleanUpdated()
		{
			string text = (this.BooleanCheck ? this.TrueBrush : this.FalseBrush);
			Brush brush = base.Context.GetBrush(text);
			BrushWidget brushWidget = this.TargetWidget ?? this;
			brushWidget.Brush = brush;
			if (this.IncludeChildren)
			{
				List<Widget> allChildrenRecursive = brushWidget.GetAllChildrenRecursive(null);
				for (int i = 0; i < allChildrenRecursive.Count; i++)
				{
					BrushWidget brushWidget2;
					if ((brushWidget2 = allChildrenRecursive[i] as BrushWidget) != null)
					{
						brushWidget2.Brush = brush;
					}
				}
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000025BF File Offset: 0x000007BF
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000025C7 File Offset: 0x000007C7
		[Editor(false)]
		public bool BooleanCheck
		{
			get
			{
				return this._booleanCheck;
			}
			set
			{
				if (this._booleanCheck != value)
				{
					this._booleanCheck = value;
					base.OnPropertyChanged(value, "BooleanCheck");
					this.OnBooleanUpdated();
				}
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000025EB File Offset: 0x000007EB
		// (set) Token: 0x06000025 RID: 37 RVA: 0x000025F3 File Offset: 0x000007F3
		[Editor(false)]
		public string TrueBrush
		{
			get
			{
				return this._trueBrush;
			}
			set
			{
				if (this._trueBrush != value)
				{
					this._trueBrush = value;
					base.OnPropertyChanged<string>(value, "TrueBrush");
				}
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002616 File Offset: 0x00000816
		// (set) Token: 0x06000027 RID: 39 RVA: 0x0000261E File Offset: 0x0000081E
		[Editor(false)]
		public string FalseBrush
		{
			get
			{
				return this._falseBrush;
			}
			set
			{
				if (this._falseBrush != value)
				{
					this._falseBrush = value;
					base.OnPropertyChanged<string>(value, "FalseBrush");
				}
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002641 File Offset: 0x00000841
		// (set) Token: 0x06000029 RID: 41 RVA: 0x00002649 File Offset: 0x00000849
		[Editor(false)]
		public BrushWidget TargetWidget
		{
			get
			{
				return this._targetWidget;
			}
			set
			{
				if (this._targetWidget != value)
				{
					this._targetWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "TargetWidget");
				}
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002667 File Offset: 0x00000867
		// (set) Token: 0x0600002B RID: 43 RVA: 0x0000266F File Offset: 0x0000086F
		[Editor(false)]
		public bool IncludeChildren
		{
			get
			{
				return this._includeChildren;
			}
			set
			{
				if (this._includeChildren != value)
				{
					this._includeChildren = value;
					base.OnPropertyChanged(value, "IncludeChildren");
				}
			}
		}

		// Token: 0x0400000A RID: 10
		private bool _initialUpdateHandled;

		// Token: 0x0400000B RID: 11
		private bool _booleanCheck;

		// Token: 0x0400000C RID: 12
		private string _trueBrush;

		// Token: 0x0400000D RID: 13
		private string _falseBrush;

		// Token: 0x0400000E RID: 14
		private BrushWidget _targetWidget;

		// Token: 0x0400000F RID: 15
		private bool _includeChildren;
	}
}
