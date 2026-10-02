using System;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x020003AE RID: 942
	public class StealthBox : ScriptComponentBehavior
	{
		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x060035D2 RID: 13778 RVA: 0x000DE100 File Offset: 0x000DC300
		// (remove) Token: 0x060035D3 RID: 13779 RVA: 0x000DE134 File Offset: 0x000DC334
		public static event Action<StealthBox> OnBoxInitialized;

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x060035D4 RID: 13780 RVA: 0x000DE168 File Offset: 0x000DC368
		// (remove) Token: 0x060035D5 RID: 13781 RVA: 0x000DE19C File Offset: 0x000DC39C
		public static event Action<StealthBox> OnBoxRemoved;

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x060035D6 RID: 13782 RVA: 0x000DE1CF File Offset: 0x000DC3CF
		public bool CoversStandingAgents
		{
			get
			{
				return this._coversStandingAgents;
			}
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x000DE1D8 File Offset: 0x000DC3D8
		protected internal override void OnInit()
		{
			base.OnInit();
			MetaMesh metaMesh = base.GameEntity.GetMetaMesh(0);
			if (metaMesh != null)
			{
				base.GameEntity.RemoveMultiMesh(metaMesh);
			}
			Action<StealthBox> onBoxInitialized = StealthBox.OnBoxInitialized;
			if (onBoxInitialized == null)
			{
				return;
			}
			onBoxInitialized(this);
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x000DE224 File Offset: 0x000DC424
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			Action<StealthBox> onBoxRemoved = StealthBox.OnBoxRemoved;
			if (onBoxRemoved == null)
			{
				return;
			}
			onBoxRemoved(this);
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x000DE240 File Offset: 0x000DC440
		public bool IsPointInside(Vec3 point)
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			Vec3 scaleVector = globalFrame.rotation.GetScaleVector();
			if (globalFrame.origin.DistanceSquared(point) > scaleVector.LengthSquared)
			{
				return false;
			}
			Vec3 vec = new Vec3(1f / scaleVector.x, 1f / scaleVector.y, 1f / scaleVector.z, -1f);
			globalFrame.rotation.ApplyScaleLocal(in vec);
			point = globalFrame.TransformToLocal(in point);
			return MathF.Abs(point.x) <= scaleVector.x / 2f && MathF.Abs(point.y) <= scaleVector.y / 2f && point.z >= 0f && point.z <= scaleVector.z;
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x000DE31B File Offset: 0x000DC51B
		public bool IsAgentInside(Agent agent)
		{
			return this.IsPointInside(agent.Position);
		}

		// Token: 0x040016F5 RID: 5877
		[EditableScriptComponentVariable(true, "")]
		private bool _coversStandingAgents;
	}
}
