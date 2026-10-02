using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000D5 RID: 213
	public class AgentAlarmStateWidget : Widget
	{
		// Token: 0x06000AF8 RID: 2808 RVA: 0x0001EC54 File Offset: 0x0001CE54
		public AgentAlarmStateWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0001EC5D File Offset: 0x0001CE5D
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.UpdatePosition();
			this.UpdateVisuals();
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0001EC72 File Offset: 0x0001CE72
		private void UpdateVisuals()
		{
			if (this.AlarmState != null)
			{
				this.SetState(this.AlarmState);
			}
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0001EC88 File Offset: 0x0001CE88
		private void UpdatePosition()
		{
			float num = this.Position.X - base.Size.X / 2f;
			float num2 = this.Position.X + base.Size.X / 2f;
			float num3 = this.Position.Y - base.Size.Y / 2f;
			float num4 = this.Position.Y + base.Size.Y / 2f;
			bool flag = this.WSign > 0 && num > 0f && num2 < base.Context.EventManager.PageSize.X && num3 > 0f && num4 < base.Context.EventManager.PageSize.Y;
			bool flag2 = this.WSign > 0 && (num2 > 0f || num < base.Context.EventManager.PageSize.X) && (num4 > 0f || num3 < base.Context.EventManager.PageSize.Y);
			if (!flag)
			{
				Vec2 vec = new Vec2(num, num3);
				Vector2 vector = base.Context.EventManager.PageSize - base.Size;
				Vec2 vec2 = vector / 2f;
				vec -= vec2;
				if (this.WSign < 0)
				{
					vec *= -1f;
				}
				float num5 = Mathf.Atan2(vec.y, vec.x) - 1.5707964f;
				float num6 = Mathf.Cos(num5);
				float num7 = Mathf.Sin(num5);
				float num8 = num6 / num7;
				Vec2 vec3 = vec2 * 1f;
				vec = ((num6 > 0f) ? new Vec2(-vec3.y / num8, vec2.y) : new Vec2(vec3.y / num8, -vec2.y));
				if (vec.x > vec3.x)
				{
					vec = new Vec2(vec3.x, -vec3.x * num8);
				}
				else if (vec.x < -vec3.x)
				{
					vec = new Vec2(-vec3.x, vec3.x * num8);
				}
				vec += vec2;
				base.ScaledPositionXOffset = Mathf.Clamp(vec.x, 0f, vector.X);
				base.ScaledPositionYOffset = Mathf.Clamp(vec.y, 0f, vector.Y);
				return;
			}
			if (flag || flag2)
			{
				base.ScaledPositionXOffset = num;
				base.ScaledPositionYOffset = num3;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x0001EF48 File Offset: 0x0001D148
		// (set) Token: 0x06000AFD RID: 2813 RVA: 0x0001EF50 File Offset: 0x0001D150
		public string AlarmState
		{
			get
			{
				return this._alarmState;
			}
			set
			{
				if (this._alarmState != value)
				{
					this._alarmState = value;
					base.OnPropertyChanged<string>(value, "AlarmState");
				}
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x0001EF73 File Offset: 0x0001D173
		// (set) Token: 0x06000AFF RID: 2815 RVA: 0x0001EF7B File Offset: 0x0001D17B
		public int WSign
		{
			get
			{
				return this._wSign;
			}
			set
			{
				if (value != this._wSign)
				{
					this._wSign = value;
					base.OnPropertyChanged(value, "WSign");
				}
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x0001EF99 File Offset: 0x0001D199
		// (set) Token: 0x06000B01 RID: 2817 RVA: 0x0001EFA1 File Offset: 0x0001D1A1
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (value != this._position)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x040004F9 RID: 1273
		private string _alarmState;

		// Token: 0x040004FA RID: 1274
		private int _wSign;

		// Token: 0x040004FB RID: 1275
		private Vec2 _position;
	}
}
