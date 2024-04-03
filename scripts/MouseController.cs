using Godot;
using Godot.Collections;
using TowerDefence.characters;
using TowerDefence.menu;

namespace TowerDefence.scripts;

public partial class MouseController : Node2D
{
    [Export] public int NavigationLevel;
    
    [Export] public int GroundLayer = 1;
    
    [Export] public int PointerLevel = 5;
    [Export] public int PointerSourceId = 5;
    [Export] public Vector2I PointerAtlasCord = new(0, 0);
        
    [Export] public int UiLevel = 6;
    [Export] public int UiSourceId = 19;
    [Export] public Vector2I UiAtlasCord = new(7, 7);
    
    [Export] public int TowersSourceId = 19;
    [Export] public Dictionary<string, Vector2I> TowersAtlas = new()
    {
        { "Level1", new Vector2I(0,1) },
        { "Level2", new Vector2I(1,0) },
        {"Level3", new Vector2I(2,0)}
    };

    private PopupMenu ContextMenu { get; set; }
    private TileMap GameTileMap { get; set; }
    private Hero Hero { get; set; }
    private NavigationRegion2D NavigationRegion2D { get; set; }
    private Vector2I PointerPosition { get; set; }
    private Vector2I BuildPosition { get; set; }

    public override void _Ready()
    {
        ContextMenu = new BuildPopupMenu();
        AddChild(ContextMenu);
        ContextMenu.IdPressed += ContextAction;
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
        var cellValue = GameTileMap.GetCellTileData(NavigationLevel, cellCoords);
        return cellValue == null;
    }

    public override void _Input(InputEvent @event)
    {
        var position = GameTileMap.LocalToMap(GetGlobalMousePosition());
        if (Input.IsActionPressed("set_move_point") && IsCellEmpty(position))
        {
            ClearPointerLayer();
            UpdatePointerPosition();
            GameTileMap.SetCell(PointerLevel, PointerPosition, PointerSourceId, PointerAtlasCord);
            Hero.MoveHeroTo(GetGlobalMousePosition());  
        }
        if (Input.IsActionPressed("action")
            && GameTileMap.GetCellTileData(GroundLayer, position).GetCustomData("isTowerCell").AsBool())
        {
            BuildPosition = position;
            if (IsCellEmpty(position))
            {
                ContextMenu.SetItemDisabled(0, false);
                ContextMenu.SetItemDisabled(2, true);
                ContextMenu.SetItemDisabled(4, true);
            }
            else if (GameTileMap.GetCellTileData(NavigationLevel, position).GetCustomData("towerLevel").AsInt16() > 0 
                     && GameTileMap.GetCellTileData(NavigationLevel, position).GetCustomData("towerLevel").AsInt16() < 3)
            {
                ContextMenu.SetItemDisabled(0, true);
                ContextMenu.SetItemDisabled(2, false);
                ContextMenu.SetItemDisabled(4, false);
            }
            else
            {
                ContextMenu.SetItemDisabled(0, true);
                ContextMenu.SetItemDisabled(2, true); 
                ContextMenu.SetItemDisabled(4, false);
            }
            var viewportPosition = GetViewport().GetMousePosition();
            ContextMenu.Position = new Vector2I((int)viewportPosition.X, (int)viewportPosition.Y);
            ContextMenu.Show();
        }
    }
    
    private void ContextAction(long id)
    {
        switch (id)
        {
            case 0:
                GameTileMap.SetCell(NavigationLevel, BuildPosition, TowersSourceId, TowersAtlas["Level1"]);
                break;
            case 2:
                var towerLevel = GameTileMap.GetCellTileData(NavigationLevel, BuildPosition).GetCustomData("towerLevel")
                    .AsInt16() + 1;
                var levelString = "Level" + towerLevel;
                GameTileMap.SetCell(NavigationLevel, BuildPosition, TowersSourceId, TowersAtlas[levelString]);
                break;
            case 4:
                GameTileMap.EraseCell(NavigationLevel, BuildPosition);
                break;
        }
    }
    
    public override void _Process(double delta)
    {
        GameTileMap.ClearLayer(UiLevel);
        if (Input.IsActionPressed("set_move_point")) return;
        var position = GameTileMap.LocalToMap(GetGlobalMousePosition());
        GameTileMap.SetCell(UiLevel, position, UiSourceId, UiAtlasCord);
        if (GameTileMap.GetCellTileData(GroundLayer, position).GetCustomData("isTowerCell").AsBool())
        {
            GameTileMap.SetCell(UiLevel, position, 6, new(1,0));
        }
    }
}