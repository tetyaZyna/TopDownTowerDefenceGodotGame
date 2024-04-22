using Godot;
using TowerDefence.characters;

namespace TowerDefence.levels
{
    public partial class MousePlacer : Node2D
    {
        [Export] public int CurrentLevel;
        [Export] public int SourceId;
        [Export] public Vector2I AtlasCord = new Vector2I(0, 0);
        private TileMap GameTileMap { get; set; }
        private Hero Hero { get; set; }
        private Vector2I PointerPosition { get; set; }
        private Vector2I PreviousPointerPosition { get; set; }
        private bool IsBuild { get; set; }
        private bool IsDestroy { get; set; }
        

        public override void _Ready()
        {
            GameTileMap = GetNode<TileMap>("TileMap");
            Hero = GetNode<Hero>("Hero");
        }

        public override void _Input(InputEvent @event)
        {
            if (Input.IsActionJustPressed("enter_build_mode"))
            {
                if (IsDestroy) IsDestroy = false;
                IsBuild = !IsBuild;
            }
            if (Input.IsActionJustPressed("enter_destroy_mode"))
            {
                if (IsBuild) IsBuild = false;
                IsDestroy = !IsDestroy;
            }
        }

        private void UpdatePointerPosition()
        {
            Vector2 heroDirection = Hero.GetHeroDirection();
            Vector2I localHeroPosition = GameTileMap.LocalToMap(Hero.Position);
            PointerPosition = new Vector2I(localHeroPosition.X + (int)heroDirection.X,
                localHeroPosition.Y + (int)heroDirection.Y);
        }

        public override void _PhysicsProcess(double delta)
        {
            UpdatePointerPosition();
            GameTileMap.EraseCell(2, PreviousPointerPosition);
            TileData tileData = GameTileMap.GetCellTileData(CurrentLevel, PointerPosition);
            if (tileData.GetCustomData("isTowerCell").AsBool())
            {
                if (IsBuild)
                {
                    GameTileMap.SetCell(2, PointerPosition, 6, new Vector2I(1, 0));
                    PreviousPointerPosition = PointerPosition;
                }
                if (IsDestroy)
                {
                    GameTileMap.SetCell(2, PointerPosition, 6, new Vector2I(3, 0));
                    PreviousPointerPosition = PointerPosition;
                } 
            }
            else
            {
                if (IsDestroy || IsBuild)
                {
                    GameTileMap.SetCell(2, PointerPosition, 6, new Vector2I(0, 0));
                    PreviousPointerPosition = PointerPosition;  
                }
                
            }
            
        }

        public override void _Process(double delta)
        {
            if (Input.IsActionPressed("set_block"))
            {
                if (IsBuild)
                {
                    GameTileMap.SetCell(CurrentLevel, PointerPosition, SourceId, AtlasCord);
                }
                if (IsDestroy)
                {
                    GameTileMap.EraseCell(1, PointerPosition);
                }
            }
        }
    }
}
