using Godot;
using TowerDefence.characters;

namespace TowerDefence.scripts;

public partial class MouseMovement : Node2D
{
    [Export] public int PointerLevel = 5;
    [Export] public int PointerSourceId = 5;
    [Export] public Vector2I PointerAtlasCord = new(0, 0);
        
    [Export] public int UiLevel = 6;
    [Export] public int UiSourceId = 19;
    [Export] public Vector2I UiAtlasCord = new(7, 7);
    private TileMap GameTileMap { get; set; }
    private Hero Hero { get; set; }
    private NavigationRegion2D NavigationRegion2D { get; set; }
    private Vector2I PointerPosition { get; set; }

    public override void _Ready()
    {
        GameTileMap = GetNode<TileMap>("TileMap");
        Hero = GetNode<Hero>("Hero");
        Hero.AutoMoveModeChanged += OnAutoMovingComplete;
        NavigationRegion2D = GetNode<NavigationRegion2D>("NavigationRegion2D");
    }

    private void OnAutoMovingComplete(object sender, bool newMode)
    {
        if (newMode) return;
        ClearPointerLayer();
    }
        
    private void UpdatePointerPosition()
    {
        PointerPosition = GameTileMap.LocalToMap(GetGlobalMousePosition());
    }

    private void ClearPointerLayer()
    {
        GameTileMap.ClearLayer(PointerLevel);
    }
        
    private bool IsCellEmpty(Vector2I cellCoords)
    {
        var cellValue = GameTileMap.GetCellTileData(PointerLevel, cellCoords);
        return cellValue == null;
    }

    public override void _Input(InputEvent @event)
    {
        if (!Input.IsActionPressed("set_move_point") || !IsCellEmpty(GameTileMap.LocalToMap(GetGlobalMousePosition()))) return;
        ClearPointerLayer();
        UpdatePointerPosition();
        GameTileMap.SetCell(PointerLevel, PointerPosition, PointerSourceId, PointerAtlasCord);
        Hero.MoveHeroTo(GetGlobalMousePosition());
    }

    public override void _Process(double delta)
    {
        GameTileMap.ClearLayer(UiLevel);
        if (Input.IsActionPressed("set_move_point")) return;
        var position = GameTileMap.LocalToMap(GetGlobalMousePosition());
        GameTileMap.SetCell(UiLevel, position, UiSourceId, UiAtlasCord);
    }
}