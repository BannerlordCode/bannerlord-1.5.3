using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200036A RID: 874
	public class WallSegment : SynchedMissionObject, IPointDefendable, ICastleKeyPosition
	{
		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x0600323C RID: 12860 RVA: 0x000CDB00 File Offset: 0x000CBD00
		// (set) Token: 0x0600323D RID: 12861 RVA: 0x000CDB08 File Offset: 0x000CBD08
		public TacticalPosition MiddlePosition { get; private set; }

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x0600323E RID: 12862 RVA: 0x000CDB11 File Offset: 0x000CBD11
		// (set) Token: 0x0600323F RID: 12863 RVA: 0x000CDB19 File Offset: 0x000CBD19
		public TacticalPosition WaitPosition { get; private set; }

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06003240 RID: 12864 RVA: 0x000CDB22 File Offset: 0x000CBD22
		// (set) Token: 0x06003241 RID: 12865 RVA: 0x000CDB2A File Offset: 0x000CBD2A
		public TacticalPosition AttackerWaitPosition { get; private set; }

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06003242 RID: 12866 RVA: 0x000CDB33 File Offset: 0x000CBD33
		// (set) Token: 0x06003243 RID: 12867 RVA: 0x000CDB3B File Offset: 0x000CBD3B
		public IPrimarySiegeWeapon AttackerSiegeWeapon { get; set; }

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06003244 RID: 12868 RVA: 0x000CDB44 File Offset: 0x000CBD44
		// (set) Token: 0x06003245 RID: 12869 RVA: 0x000CDB4C File Offset: 0x000CBD4C
		public IEnumerable<DefencePoint> DefencePoints { get; protected set; }

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06003246 RID: 12870 RVA: 0x000CDB55 File Offset: 0x000CBD55
		// (set) Token: 0x06003247 RID: 12871 RVA: 0x000CDB5D File Offset: 0x000CBD5D
		public bool IsBreachedWall { get; private set; }

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06003248 RID: 12872 RVA: 0x000CDB66 File Offset: 0x000CBD66
		// (set) Token: 0x06003249 RID: 12873 RVA: 0x000CDB6E File Offset: 0x000CBD6E
		public WorldFrame MiddleFrame { get; private set; }

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x0600324A RID: 12874 RVA: 0x000CDB77 File Offset: 0x000CBD77
		// (set) Token: 0x0600324B RID: 12875 RVA: 0x000CDB7F File Offset: 0x000CBD7F
		public WorldFrame DefenseWaitFrame { get; private set; }

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x0600324C RID: 12876 RVA: 0x000CDB88 File Offset: 0x000CBD88
		// (set) Token: 0x0600324D RID: 12877 RVA: 0x000CDB90 File Offset: 0x000CBD90
		public WorldFrame AttackerWaitFrame { get; private set; } = WorldFrame.Invalid;

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x0600324E RID: 12878 RVA: 0x000CDB99 File Offset: 0x000CBD99
		// (set) Token: 0x0600324F RID: 12879 RVA: 0x000CDBA1 File Offset: 0x000CBDA1
		public FormationAI.BehaviorSide DefenseSide { get; private set; }

		// Token: 0x06003250 RID: 12880 RVA: 0x000CDBAC File Offset: 0x000CBDAC
		public Vec3 GetPosition()
		{
			return base.GameEntity.GlobalPosition;
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x000CDBC8 File Offset: 0x000CBDC8
		public WallSegment()
		{
			this.AttackerSiegeWeapon = null;
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x000CDC2C File Offset: 0x000CBE2C
		protected internal override void OnInit()
		{
			base.OnInit();
			string sideTag = this.SideTag;
			if (!(sideTag == "left"))
			{
				if (!(sideTag == "middle"))
				{
					if (!(sideTag == "right"))
					{
						this.DefenseSide = FormationAI.BehaviorSide.BehaviorSideNotSet;
					}
					else
					{
						this.DefenseSide = FormationAI.BehaviorSide.Right;
					}
				}
				else
				{
					this.DefenseSide = FormationAI.BehaviorSide.Middle;
				}
			}
			else
			{
				this.DefenseSide = FormationAI.BehaviorSide.Left;
			}
			WeakGameEntity weakGameEntity = base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity ce) => ce.HasTag("solid_child"));
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			List<WeakGameEntity> list2 = new List<WeakGameEntity>();
			if (weakGameEntity.IsValid)
			{
				list = weakGameEntity.CollectChildrenEntitiesWithTag("middle_pos");
				list2 = weakGameEntity.CollectChildrenEntitiesWithTag("wait_pos");
			}
			else
			{
				list = base.GameEntity.CollectChildrenEntitiesWithTag("middle_pos");
				list2 = base.GameEntity.CollectChildrenEntitiesWithTag("wait_pos");
			}
			MatrixFrame matrixFrame;
			if (list.Count > 0)
			{
				WeakGameEntity weakGameEntity2 = list[0];
				this.MiddlePosition = weakGameEntity2.GetFirstScriptOfType<TacticalPosition>();
				matrixFrame = weakGameEntity2.GetGlobalFrame();
			}
			else
			{
				matrixFrame = base.GameEntity.GetGlobalFrame();
			}
			this.MiddleFrame = new WorldFrame(matrixFrame.rotation, matrixFrame.origin.ToWorldPosition());
			if (list2.Count > 0)
			{
				WeakGameEntity weakGameEntity3 = list2[0];
				this.WaitPosition = weakGameEntity3.GetFirstScriptOfType<TacticalPosition>();
				matrixFrame = weakGameEntity3.GetGlobalFrame();
				this.DefenseWaitFrame = new WorldFrame(matrixFrame.rotation, matrixFrame.origin.ToWorldPosition());
				return;
			}
			this.DefenseWaitFrame = this.MiddleFrame;
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x000CDDC9 File Offset: 0x000CBFC9
		protected internal override bool MovesEntity()
		{
			return false;
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x000CDDCC File Offset: 0x000CBFCC
		public void OnChooseUsedWallSegment(bool isBroken)
		{
			WeakGameEntity firstChildEntityWithTag = base.GameEntity.GetFirstChildEntityWithTag("solid_child");
			WeakGameEntity firstChildEntityWithTag2 = base.GameEntity.GetFirstChildEntityWithTag("broken_child");
			Scene scene = base.GameEntity.Scene;
			if (isBroken)
			{
				firstChildEntityWithTag.GetFirstScriptOfType<WallSegment>().SetDisabledSynched();
				firstChildEntityWithTag2.GetFirstScriptOfType<WallSegment>().SetVisibleSynched(true, false);
				if (!GameNetwork.IsClientOrReplay)
				{
					if (this._properGroundOutsideNavmeshID > 0 && this._underDebrisOutsideNavmeshID > 0)
					{
						scene.SeparateFacesWithId(this._properGroundOutsideNavmeshID, this._underDebrisOutsideNavmeshID);
					}
					if (this._properGroundInsideNavmeshID > 0 && this._underDebrisInsideNavmeshID > 0)
					{
						scene.SeparateFacesWithId(this._properGroundInsideNavmeshID, this._underDebrisInsideNavmeshID);
					}
					if (this._underDebrisOutsideNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._underDebrisOutsideNavmeshID, false);
					}
					if (this._underDebrisInsideNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._underDebrisInsideNavmeshID, false);
					}
					if (this._underDebrisGenericNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._underDebrisGenericNavmeshID, false);
					}
					if (this._overDebrisOutsideNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._overDebrisOutsideNavmeshID, true);
						if (this._properGroundOutsideNavmeshID > 0)
						{
							scene.MergeFacesWithId(this._overDebrisOutsideNavmeshID, this._properGroundOutsideNavmeshID, 0);
						}
					}
					if (this._overDebrisInsideNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._overDebrisInsideNavmeshID, true);
						if (this._properGroundInsideNavmeshID > 0)
						{
							scene.MergeFacesWithId(this._overDebrisInsideNavmeshID, this._properGroundInsideNavmeshID, 1);
						}
					}
					if (this._overDebrisGenericNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._overDebrisGenericNavmeshID, true);
					}
					if (this._onSolidWallGenericNavmeshID > 0)
					{
						scene.SetAbilityOfFacesWithId(this._onSolidWallGenericNavmeshID, false);
					}
					foreach (StrategicArea strategicArea in from c in firstChildEntityWithTag.GetChildren()
						where c.HasScriptOfType<StrategicArea>()
						select c.GetFirstScriptOfType<StrategicArea>())
					{
						strategicArea.OnParentGameEntityVisibilityChanged(false);
					}
					foreach (StrategicArea strategicArea2 in from c in firstChildEntityWithTag2.GetChildren()
						where c.HasScriptOfType<StrategicArea>()
						select c.GetFirstScriptOfType<StrategicArea>())
					{
						strategicArea2.OnParentGameEntityVisibilityChanged(true);
					}
				}
				this.IsBreachedWall = true;
				List<WeakGameEntity> list = firstChildEntityWithTag2.CollectChildrenEntitiesWithTag("middle_pos");
				if (list.Count > 0)
				{
					WeakGameEntity weakGameEntity = list.FirstOrDefault<WeakGameEntity>();
					this.MiddlePosition = weakGameEntity.GetFirstScriptOfType<TacticalPosition>();
					MatrixFrame globalFrame = weakGameEntity.GetGlobalFrame();
					this.MiddleFrame = new WorldFrame(globalFrame.rotation, globalFrame.origin.ToWorldPosition());
				}
				else
				{
					MBDebug.ShowWarning("Broken child of wall does not have middle position");
					MatrixFrame globalFrame2 = firstChildEntityWithTag2.GetGlobalFrame();
					this.MiddleFrame = new WorldFrame(globalFrame2.rotation, new WorldPosition(scene, UIntPtr.Zero, globalFrame2.origin, false));
				}
				List<WeakGameEntity> list2 = firstChildEntityWithTag2.CollectChildrenEntitiesWithTag("wait_pos");
				if (list2.Count > 0)
				{
					WeakGameEntity weakGameEntity2 = list2.FirstOrDefault<WeakGameEntity>();
					this.WaitPosition = weakGameEntity2.GetFirstScriptOfType<TacticalPosition>();
					MatrixFrame globalFrame3 = weakGameEntity2.GetGlobalFrame();
					this.DefenseWaitFrame = new WorldFrame(globalFrame3.rotation, globalFrame3.origin.ToWorldPosition());
				}
				else
				{
					this.DefenseWaitFrame = this.MiddleFrame;
				}
				WallSegment firstScriptOfType = firstChildEntityWithTag.GetFirstScriptOfType<WallSegment>();
				if (firstScriptOfType != null)
				{
					firstScriptOfType.SetDisabledAndMakeInvisible(true, false);
				}
				WeakGameEntity weakGameEntity3 = firstChildEntityWithTag2.CollectChildrenEntitiesWithTag("attacker_wait_pos").FirstOrDefault<WeakGameEntity>();
				if (weakGameEntity3.IsValid)
				{
					MatrixFrame globalFrame4 = weakGameEntity3.GetGlobalFrame();
					this.AttackerWaitFrame = new WorldFrame(globalFrame4.rotation, globalFrame4.origin.ToWorldPosition());
					this.AttackerWaitPosition = weakGameEntity3.GetFirstScriptOfType<TacticalPosition>();
					return;
				}
			}
			else if (!GameNetwork.IsClientOrReplay)
			{
				firstChildEntityWithTag.GetFirstScriptOfType<WallSegment>().SetVisibleSynched(true, false);
				firstChildEntityWithTag2.GetFirstScriptOfType<WallSegment>().SetDisabledSynched();
				if (this._overDebrisOutsideNavmeshID > 0)
				{
					scene.SetAbilityOfFacesWithId(this._overDebrisOutsideNavmeshID, false);
				}
				if (this._overDebrisInsideNavmeshID > 0)
				{
					scene.SetAbilityOfFacesWithId(this._overDebrisInsideNavmeshID, false);
				}
				if (this._overDebrisGenericNavmeshID > 0)
				{
					scene.SetAbilityOfFacesWithId(this._overDebrisGenericNavmeshID, false);
				}
				foreach (StrategicArea strategicArea3 in from c in firstChildEntityWithTag.GetChildren()
					where c.HasScriptOfType<StrategicArea>()
					select c.GetFirstScriptOfType<StrategicArea>())
				{
					strategicArea3.OnParentGameEntityVisibilityChanged(true);
				}
				foreach (StrategicArea strategicArea4 in from c in firstChildEntityWithTag2.GetChildren()
					where c.HasScriptOfType<StrategicArea>()
					select c.GetFirstScriptOfType<StrategicArea>())
				{
					strategicArea4.OnParentGameEntityVisibilityChanged(false);
				}
			}
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x000CE34C File Offset: 0x000CC54C
		protected internal override void OnEditorValidate()
		{
			base.OnEditorValidate();
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x000CE354 File Offset: 0x000CC554
		protected internal override bool OnCheckForProblems()
		{
			bool flag = base.OnCheckForProblems();
			if (!base.Scene.IsMultiplayerScene() && this.SideTag == "left")
			{
				List<WeakGameEntity> list = new List<WeakGameEntity>();
				base.Scene.GetEntitiesAsWeak(ref list);
				int num = 0;
				foreach (WeakGameEntity weakGameEntity in list)
				{
					if (base.GameEntity.GetUpgradeLevelOfEntity() == weakGameEntity.GetUpgradeLevelOfEntity() && weakGameEntity.GetFirstScriptOfType<SiegeLadderSpawner>() != null)
					{
						num++;
					}
				}
				if (num != 4)
				{
					MBEditor.AddEntityWarning(base.GameEntity, "The siege ladder count in the scene is not 4, for upgrade level " + base.GameEntity.GetUpgradeLevelOfEntity().ToString() + ". Current siege ladder count: " + num.ToString());
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0400154A RID: 5450
		private const string WaitPositionTag = "wait_pos";

		// Token: 0x0400154B RID: 5451
		private const string MiddlePositionTag = "middle_pos";

		// Token: 0x0400154C RID: 5452
		private const string AttackerWaitPositionTag = "attacker_wait_pos";

		// Token: 0x0400154D RID: 5453
		private const string SolidChildTag = "solid_child";

		// Token: 0x0400154E RID: 5454
		private const string BrokenChildTag = "broken_child";

		// Token: 0x0400154F RID: 5455
		[EditableScriptComponentVariable(true, "")]
		private int _properGroundOutsideNavmeshID = -1;

		// Token: 0x04001550 RID: 5456
		[EditableScriptComponentVariable(true, "")]
		private int _properGroundInsideNavmeshID = -1;

		// Token: 0x04001551 RID: 5457
		[EditableScriptComponentVariable(true, "")]
		private int _underDebrisOutsideNavmeshID = -1;

		// Token: 0x04001552 RID: 5458
		[EditableScriptComponentVariable(true, "")]
		private int _underDebrisInsideNavmeshID = -1;

		// Token: 0x04001553 RID: 5459
		[EditableScriptComponentVariable(true, "")]
		private int _overDebrisOutsideNavmeshID = -1;

		// Token: 0x04001554 RID: 5460
		[EditableScriptComponentVariable(true, "")]
		private int _overDebrisInsideNavmeshID = -1;

		// Token: 0x04001555 RID: 5461
		[EditableScriptComponentVariable(true, "")]
		private int _underDebrisGenericNavmeshID = -1;

		// Token: 0x04001556 RID: 5462
		[EditableScriptComponentVariable(true, "")]
		private int _overDebrisGenericNavmeshID = -1;

		// Token: 0x04001557 RID: 5463
		[EditableScriptComponentVariable(true, "")]
		private int _onSolidWallGenericNavmeshID = -1;

		// Token: 0x0400155D RID: 5469
		public string SideTag;
	}
}
