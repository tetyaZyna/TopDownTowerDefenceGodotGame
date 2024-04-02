/*
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using TowerDefence.characters;

namespace TowerDefence.scripts
{
    public partial class AStarGridMoving : Node2D
    {
        [Export] public int CurrentLevel = 2;
        [Export] public int SourceId = 5;
        [Export] public Vector2I AtlasCord = new Vector2I(0, 0);
        private TileMap TileMap { get; set; }
        private Hero Hero { get; set; }
        private AStarGrid2D AstarGrid { get; set; }
        private Array<Vector2> CurrentIdPath { get; set; }
        private Vector2I PointerPosition { get; set; }
        private Vector2I PreviousPointerPosition { get; set; }
        public override void _Ready()
        {
            TileMap = GetNode<TileMap>("TileMap");
            Hero = GetNode<Hero>("Hero");
            AstarGrid = new AStarGrid2D();
            AstarGrid.Region = TileMap.GetUsedRect();
            AstarGrid.CellSize = new Vector2I(16, 16);
            AstarGrid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.AtLeastOneWalkable;
            AstarGrid.Update();
        }
        
        private void UpdatePointerPosition()
        {
            PreviousPointerPosition = PointerPosition;
            PointerPosition = TileMap.LocalToMap(GetGlobalMousePosition());
        }

        public override void _Input(InputEvent @event)
        {
            if (!@event.IsActionPressed("set_move_point")) return;
            UpdatePointerPosition();
            TileMap.EraseCell(CurrentLevel, PreviousPointerPosition);
            TileMap.SetCell(CurrentLevel, PointerPosition, SourceId, AtlasCord);
            
            var idPath = AstarGrid.GetIdPath(
                TileMap.LocalToMap(Hero.Position),
                PointerPosition
            ).Slice(1);

            if (idPath.Count > 0)
            {
                CurrentIdPath = new Array<Vector2>();
                foreach (var path in idPath)
                {
                    CurrentIdPath.Add(TileMap.MapToLocal(path));
                }
                
            }
            
            Hero.MoveHeroTo(CurrentIdPath);
        }
    }
}
*/



