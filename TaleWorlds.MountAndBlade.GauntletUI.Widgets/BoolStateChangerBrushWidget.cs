using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000008 RID: 8
	public class BoolStateChangerBrushWidget : BrushWidget
	{
		// Token: 0x0600002C RID: 44 RVA: 0x0000268D File Offset: 0x0000088D
		public BoolStateChangerBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002696 File Offset: 0x00000896
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._isStateDirty)
			{
				this._isStateDirty = false;
				this.UpdateState();
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000026B4 File Offset: 0x000008B4
		private void AddState(Widget widget, string state, bool includeChildren)
		{
			widget.AddState(state);
			if (includeChildren)
			{
				for (int i = 0; i < widget.ChildCount; i++)
				{
					this.AddState(widget.GetChild(i), state, true);
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000026EC File Offset: 0x000008EC
		private void SetState(Widget widget, string state, bool includeChildren)
		{
			widget.SetState(state);
			if (includeChildren)
			{
				for (int i = 0; i < widget.ChildCount; i++)
				{
					this.SetState(widget.GetChild(i), state, true);
				}
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002724 File Offset: 0x00000924
		private void UpdateState()
		{
			string text = (this.BooleanCheck ? this.TrueState : this.FalseState);
			if (text == null)
			{
				Debug.FailedAssert("State is null for BoolStateChangerWidget", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\BoolStateChangerBrushWidget.cs", "UpdateState", 59);
				return;
			}
			Widget widget = this.TargetWidget ?? this;
			this.AddState(widget, text, this.IncludeChildren);
			this.SetState(widget, text, this.IncludeChildren);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000278A File Offset: 0x0000098A
		private void SetStateDirty()
		{
			this._isStateDirty = true;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002793 File Offset: 0x00000993
		// (set) Token: 0x06000033 RID: 51 RVA: 0x0000279B File Offset: 0x0000099B
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
					this.SetStateDirty();
				}
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000027BF File Offset: 0x000009BF
		// (set) Token: 0x06000035 RID: 53 RVA: 0x000027C7 File Offset: 0x000009C7
		[Editor(false)]
		public string TrueState
		{
			get
			{
				return this._trueState;
			}
			set
			{
				if (this._trueState != value)
				{
					this._trueState = value;
					base.OnPropertyChanged<string>(value, "TrueState");
					this.SetStateDirty();
				}
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000027F0 File Offset: 0x000009F0
		// (set) Token: 0x06000037 RID: 55 RVA: 0x000027F8 File Offset: 0x000009F8
		[Editor(false)]
		public string FalseState
		{
			get
			{
				return this._falseState;
			}
			set
			{
				if (this._falseState != value)
				{
					this._falseState = value;
					base.OnPropertyChanged<string>(value, "FalseState");
					this.SetStateDirty();
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002821 File Offset: 0x00000A21
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002829 File Offset: 0x00000A29
		[Editor(false)]
		public Widget TargetWidget
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
					base.OnPropertyChanged<Widget>(value, "TargetWidget");
					this.SetStateDirty();
				}
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003A RID: 58 RVA: 0x0000284D File Offset: 0x00000A4D
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002855 File Offset: 0x00000A55
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
					this.SetStateDirty();
				}
			}
		}

		// Token: 0x04000010 RID: 16
		private bool _isStateDirty;

		// Token: 0x04000011 RID: 17
		private bool _booleanCheck;

		// Token: 0x04000012 RID: 18
		private string _trueState;

		// Token: 0x04000013 RID: 19
		private string _falseState;

		// Token: 0x04000014 RID: 20
		private Widget _targetWidget;

		// Token: 0x04000015 RID: 21
		private bool _includeChildren;
	}
}
