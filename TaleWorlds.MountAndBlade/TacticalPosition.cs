using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200036F RID: 879
	public class TacticalPosition : MissionObject
	{
		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x060032A6 RID: 12966 RVA: 0x000CF804 File Offset: 0x000CDA04
		// (set) Token: 0x060032A7 RID: 12967 RVA: 0x000CF80C File Offset: 0x000CDA0C
		public WorldPosition Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x060032A8 RID: 12968 RVA: 0x000CF815 File Offset: 0x000CDA15
		// (set) Token: 0x060032A9 RID: 12969 RVA: 0x000CF81D File Offset: 0x000CDA1D
		public Vec2 Direction
		{
			get
			{
				return this._direction;
			}
			set
			{
				this._direction = value;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x060032AA RID: 12970 RVA: 0x000CF826 File Offset: 0x000CDA26
		public float Width
		{
			get
			{
				return this._width;
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x060032AB RID: 12971 RVA: 0x000CF82E File Offset: 0x000CDA2E
		public float Slope
		{
			get
			{
				return this._slope;
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x060032AC RID: 12972 RVA: 0x000CF836 File Offset: 0x000CDA36
		public bool IsInsurmountable
		{
			get
			{
				return this._isInsurmountable;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x060032AD RID: 12973 RVA: 0x000CF83E File Offset: 0x000CDA3E
		public bool IsOuterEdge
		{
			get
			{
				return this._isOuterEdge;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x060032AE RID: 12974 RVA: 0x000CF846 File Offset: 0x000CDA46
		// (set) Token: 0x060032AF RID: 12975 RVA: 0x000CF84E File Offset: 0x000CDA4E
		public List<TacticalPosition> LinkedTacticalPositions
		{
			get
			{
				return this._linkedTacticalPositions;
			}
			set
			{
				this._linkedTacticalPositions = value;
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x060032B0 RID: 12976 RVA: 0x000CF857 File Offset: 0x000CDA57
		public TacticalPosition.TacticalPositionTypeEnum TacticalPositionType
		{
			get
			{
				return this._tacticalPositionType;
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x060032B1 RID: 12977 RVA: 0x000CF85F File Offset: 0x000CDA5F
		public TacticalRegion.TacticalRegionTypeEnum TacticalRegionMembership
		{
			get
			{
				return this._tacticalRegionMembership;
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x060032B2 RID: 12978 RVA: 0x000CF867 File Offset: 0x000CDA67
		public FormationAI.BehaviorSide TacticalPositionSide
		{
			get
			{
				return this._tacticalPositionSide;
			}
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x000CF86F File Offset: 0x000CDA6F
		public TacticalPosition()
		{
			this._width = 1f;
			this._slope = 0f;
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x000CF894 File Offset: 0x000CDA94
		public TacticalPosition(WorldPosition position, Vec2 direction, float width, float slope = 0f, bool isInsurmountable = false, TacticalPosition.TacticalPositionTypeEnum tacticalPositionType = TacticalPosition.TacticalPositionTypeEnum.Regional, TacticalRegion.TacticalRegionTypeEnum tacticalRegionMembership = TacticalRegion.TacticalRegionTypeEnum.Opening)
		{
			this._position = position;
			this._direction = direction;
			this._width = width;
			this._slope = slope;
			this._isInsurmountable = isInsurmountable;
			this._tacticalPositionType = tacticalPositionType;
			this._tacticalRegionMembership = tacticalRegionMembership;
			this._tacticalPositionSide = FormationAI.BehaviorSide.BehaviorSideNotSet;
			this._isOuterEdge = false;
			this._linkedTacticalPositions = new List<TacticalPosition>();
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x000CF8FC File Offset: 0x000CDAFC
		protected internal override void OnInit()
		{
			base.OnInit();
			this._position = new WorldPosition(base.GameEntity.GetScenePointer(), base.GameEntity.GlobalPosition);
			this._direction = base.GameEntity.GetGlobalFrame().rotation.f.AsVec2.Normalized();
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x000CF964 File Offset: 0x000CDB64
		public override void AfterMissionStart()
		{
			base.AfterMissionStart();
			this.UpdateLinkedTacticalPositions();
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x000CF972 File Offset: 0x000CDB72
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.ApplyChangesFromEditor();
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x000CF980 File Offset: 0x000CDB80
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.ApplyChangesFromEditor();
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x000CF990 File Offset: 0x000CDB90
		public void UpdateLinkedTacticalPositions()
		{
			this._linkedTacticalPositions = (from c in base.GameEntity.GetChildren()
				where c.HasScriptOfType<TacticalPosition>()
				select c.GetFirstScriptOfType<TacticalPosition>()).ToList<TacticalPosition>();
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x000CFA00 File Offset: 0x000CDC00
		private void ApplyChangesFromEditor()
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			if (this._width > 0f && this._width != this._oldWidth)
			{
				globalFrame.rotation.MakeUnit();
				Vec3 vec = new Vec3(this._width, 1f, 1f, -1f);
				globalFrame.rotation.ApplyScaleLocal(in vec);
				base.GameEntity.SetGlobalFrame(in globalFrame, true);
				base.GameEntity.UpdateTriadFrameForEditorForAllChildren();
				this._oldWidth = this._width;
			}
			this._direction = globalFrame.rotation.f.AsVec2.Normalized();
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x000CFAB7 File Offset: 0x000CDCB7
		public void SetWidth(float width)
		{
			this._width = width;
		}

		// Token: 0x04001581 RID: 5505
		private WorldPosition _position;

		// Token: 0x04001582 RID: 5506
		private Vec2 _direction;

		// Token: 0x04001583 RID: 5507
		private float _oldWidth;

		// Token: 0x04001584 RID: 5508
		[EditableScriptComponentVariable(true, "")]
		private float _width;

		// Token: 0x04001585 RID: 5509
		[EditableScriptComponentVariable(true, "")]
		private float _slope;

		// Token: 0x04001586 RID: 5510
		[EditableScriptComponentVariable(true, "")]
		private bool _isInsurmountable;

		// Token: 0x04001587 RID: 5511
		[EditableScriptComponentVariable(true, "")]
		private bool _isOuterEdge;

		// Token: 0x04001588 RID: 5512
		private List<TacticalPosition> _linkedTacticalPositions;

		// Token: 0x04001589 RID: 5513
		[EditableScriptComponentVariable(true, "")]
		private TacticalPosition.TacticalPositionTypeEnum _tacticalPositionType;

		// Token: 0x0400158A RID: 5514
		[EditableScriptComponentVariable(true, "")]
		private TacticalRegion.TacticalRegionTypeEnum _tacticalRegionMembership;

		// Token: 0x0400158B RID: 5515
		[EditableScriptComponentVariable(true, "")]
		private FormationAI.BehaviorSide _tacticalPositionSide = FormationAI.BehaviorSide.BehaviorSideNotSet;

		// Token: 0x0200064F RID: 1615
		public enum TacticalPositionTypeEnum
		{
			// Token: 0x040021AC RID: 8620
			Regional,
			// Token: 0x040021AD RID: 8621
			HighGround,
			// Token: 0x040021AE RID: 8622
			ChokePoint,
			// Token: 0x040021AF RID: 8623
			Cliff,
			// Token: 0x040021B0 RID: 8624
			SpecialMissionPosition
		}
	}
}
