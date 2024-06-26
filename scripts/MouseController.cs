using Godot;
using TowerDefence.characters;
using TowerDefence.menu;
using TowerDefence.towers;

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
    
    [Export] public int SignLevel = 7;
    [Export] public int SignSourceId = 8;
    [Export] public Vector2I SignAtlasCord = new(1, 0);

    [Export] public int TowersSourceId = 19;

    [Export] public Godot.Collections.Dictionary<string, Vector2I> TowersAtlas = new()
    {
        { "Level1", new Vector2I(0, 1) },
        { "Level2", new Vector2I(1, 0) },
        { "Level3", new Vector2I(2, 0) }
    };

    private PopupMenu ContextMenu { get; set; }
    private TileMap GameTileMap { get; set; }
    private Hero Hero { get; set; }
    private NavigationRegion2D NavigationRegion2D { get; set; }
    private Vector2I PointerPosition { get; set; }
    private Vector2I BuildPosition { get; set; }
    private PauseMenu PauseMenu { get; set; }
    private Node2D TowersList { get; set; }
    public override void _Ready()
    {
        GetTree().Paused = false;
        ContextMenu = new BuildPopupMenu();
        AddChild(ContextMenu);
        ContextMenu.IdPressed += ContextAction;
        GameTileMap = GetNode<TileMap>("TileMap");
        Hero = GetNode<Hero>("Hero");
        Hero.AutoMoveModeChanged += OnAutoMovingComplete;
        NavigationRegion2D = GetNode<NavigationRegion2D>("NavigationRegion2D");
        PauseMenu = GetNode<PauseMenu>("CanvasLayer/PauseMenu");
        PauseMenu.PauseChanged += ChangePause;
        TowersList = GetNode<Node2D>("Towers");
    }

    private void ChangePause(object sender, bool newPause)
    {
        GetTree().Paused = newPause;
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

    private void ContextAction(long id)
    {
        switch (id)
        {
            case 0:
                GameTileMap.EraseCell(SignLevel, BuildPosition);
                GameTileMap.SetCell(NavigationLevel, BuildPosition, TowersSourceId, TowersAtlas["Level1"]);
                TowersList.AddChild(new Tower(GameTileMap.MapToLocal(BuildPosition)));
                Hero.Coins -= 25;
                break;
            case 2:
                var towerLevel = GameTileMap.GetCellTileData(NavigationLevel, BuildPosition).GetCustomData("towerLevel")
                    .AsInt16() + 1;
                var levelString = "Level" + towerLevel;
                GameTileMap.SetCell(NavigationLevel, BuildPosition, TowersSourceId, TowersAtlas[levelString]);
                foreach (var i in TowersList.GetChildren())
                {
                    var j = (Tower) i;
                    if (GameTileMap.MapToLocal(BuildPosition) == j.Position)
                    {
                        j.SetLevel(GameTileMap.GetCellTileData(NavigationLevel, BuildPosition).GetCustomData("towerLevel")
                            .AsInt16());
                    }
                }
                Hero.Coins -= 25 * (GameTileMap.GetCellTileData(NavigationLevel, BuildPosition).GetCustomData("towerLevel")
                    .AsInt16());
                break;
            case 4:
                GameTileMap.EraseCell(NavigationLevel, BuildPosition);
                GameTileMap.SetCell(SignLevel, BuildPosition, SignSourceId, SignAtlasCord);
                foreach (var i in TowersList.GetChildren())
                {
                    var j = (Tower) i;
                    if (GameTileMap.MapToLocal(BuildPosition) == j.Position)
                    {
                        TowersList.RemoveChild(i);
                    }
                }
                break;
        }
    }

    private void UpdateContextMenuButton()
    {
        if (IsCellEmpty(BuildPosition))
        {
            ContextMenu.SetItemDisabled(0, Hero.Coins < 25);
            ContextMenu.SetItemText(0, ContextMenu.GetItemText(0)[..5]);
            ContextMenu.SetItemText(0, ContextMenu.GetItemText(0) + " (25)");
            ContextMenu.SetItemDisabled(2, true);
            ContextMenu.SetItemText(2, ContextMenu.GetItemText(2)[..7]);
            ContextMenu.SetItemDisabled(4, true);
        }
        else if (GameTileMap.GetCellTileData(NavigationLevel, BuildPosition).GetCustomData("towerLevel").AsInt16() is > 0 and < 3)
        {
            ContextMenu.SetItemDisabled(0, true);
            ContextMenu.SetItemText(0, ContextMenu.GetItemText(0)[..5]);
            ContextMenu.SetItemDisabled(2, Hero.Coins < (25 *  (GameTileMap.GetCellTileData(NavigationLevel, BuildPosition).GetCustomData("towerLevel").AsInt16() + 1)));
            ContextMenu.SetItemText(2, ContextMenu.GetItemText(2)[..7]);
            ContextMenu.SetItemText(2, ContextMenu.GetItemText(2) + " (" + 25 * 
                (GameTileMap.GetCellTileData(NavigationLevel, BuildPosition).GetCustomData("towerLevel").AsInt16() + 1) + ")");
            ContextMenu.SetItemDisabled(4, false);
        }
        else
        {
            ContextMenu.SetItemDisabled(0, true);
            ContextMenu.SetItemText(0, ContextMenu.GetItemText(0)[..5]);
            ContextMenu.SetItemDisabled(2, true);
            ContextMenu.SetItemText(2, ContextMenu.GetItemText(2)[..7]);
            ContextMenu.SetItemDisabled(4, false);
        }
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
            UpdateContextMenuButton();
            var viewportPosition = GetViewport().GetMousePosition();
            var viewportCenter = GetViewportRect().Size / 2;
            var contextMenuDirection = new Vector2I(viewportPosition.X > viewportCenter.X ? 1 : 0,
                viewportPosition.Y > viewportCenter.Y ? 1 : 0);
            var contextMenuMargin = (ContextMenu.Size + new Vector2I(20, 20)) * contextMenuDirection;
            ContextMenu.Position = new Vector2I((int)viewportPosition.X + 20, (int)viewportPosition.Y + 20)
                                   - contextMenuMargin;
            ContextMenu.Show();
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
            GameTileMap.SetCell(UiLevel, position, 6, new(1, 0));
        }
    }
}