using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200033A RID: 826
	public class FleePosition : ScriptComponentBehavior
	{
		// Token: 0x06002EC1 RID: 11969 RVA: 0x000B4D90 File Offset: 0x000B2F90
		protected internal override void OnInit()
		{
			this.CollectNodes();
			bool flag = false;
			if (this.Side == "both")
			{
				this._side = BattleSideEnum.None;
			}
			else if (this.Side == "attacker")
			{
				this._side = (flag ? BattleSideEnum.Defender : BattleSideEnum.Attacker);
			}
			else if (this.Side == "defender")
			{
				this._side = (flag ? BattleSideEnum.Attacker : BattleSideEnum.Defender);
			}
			else
			{
				this._side = BattleSideEnum.None;
			}
			if (base.GameEntity.HasTag("sally_out"))
			{
				if (Mission.Current.IsSallyOutBattle)
				{
					Mission.Current.AddFleePosition(this);
					return;
				}
			}
			else
			{
				Mission.Current.AddFleePosition(this);
			}
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x000B4E41 File Offset: 0x000B3041
		public BattleSideEnum GetSide()
		{
			return this._side;
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x000B4E49 File Offset: 0x000B3049
		protected internal override void OnEditorInit()
		{
			this.CollectNodes();
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x000B4E54 File Offset: 0x000B3054
		private void CollectNodes()
		{
			this._nodes.Clear();
			int childCount = base.GameEntity.ChildCount;
			for (int i = 0; i < childCount; i++)
			{
				this._nodes.Add(base.GameEntity.GetChild(i).GlobalPosition);
			}
			if (this._nodes.IsEmpty<Vec3>())
			{
				this._nodes.Add(base.GameEntity.GlobalPosition);
			}
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x000B4ED0 File Offset: 0x000B30D0
		protected internal override void OnEditorTick(float dt)
		{
			this.CollectNodes();
			bool flag = base.GameEntity.IsSelectedOnEditor();
			int childCount = base.GameEntity.ChildCount;
			int num = 0;
			while (!flag && num < childCount)
			{
				flag = base.GameEntity.GetChild(num).IsSelectedOnEditor();
				num++;
			}
			if (flag)
			{
				for (int i = 0; i < this._nodes.Count; i++)
				{
					int num2 = this._nodes.Count - 1;
				}
			}
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x000B4F58 File Offset: 0x000B3158
		public Vec3 GetClosestPointToEscape(Vec2 position)
		{
			if (this._nodes.Count == 1)
			{
				return this._nodes[0];
			}
			float num = float.MaxValue;
			Vec3 vec = this._nodes[0];
			for (int i = 0; i < this._nodes.Count - 1; i++)
			{
				Vec3 vec2 = this._nodes[i];
				Vec3 vec3 = this._nodes[i + 1];
				float num2 = vec2.DistanceSquared(vec3);
				if (num2 > 0f)
				{
					float num3 = MathF.Max(0f, MathF.Min(1f, Vec2.DotProduct(position - vec2.AsVec2, vec3.AsVec2 - vec2.AsVec2) / num2));
					Vec3 vec4 = vec2 + num3 * (vec3 - vec2);
					float num4 = vec4.AsVec2.DistanceSquared(position);
					if (num > num4)
					{
						num = num4;
						vec = vec4;
					}
				}
				else
				{
					num = 0f;
					vec = vec2;
				}
			}
			return vec;
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x000B5060 File Offset: 0x000B3260
		protected internal override bool MovesEntity()
		{
			return true;
		}

		// Token: 0x04001285 RID: 4741
		private List<Vec3> _nodes = new List<Vec3>();

		// Token: 0x04001286 RID: 4742
		private BattleSideEnum _side;

		// Token: 0x04001287 RID: 4743
		public string Side = "both";
	}
}
