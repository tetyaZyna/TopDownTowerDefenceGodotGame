using Godot;
namespace TowerDefence.menu;

public partial class BuildPopupMenu: PopupMenu
{
    public BuildPopupMenu()
    {
        AllowSearch = false;
        Visible = false;
        Size = new Vector2I(159, 97);
        
        var atlasTextureBuild = new AtlasTexture();
        atlasTextureBuild.Atlas = (Texture2D)GD.Load("res://assets/user interface/UiIcons.png");
        atlasTextureBuild.Region = new Rect2(0, 0, 16, 16);

        var atlasTextureUpgrade = new AtlasTexture();
        atlasTextureUpgrade.Atlas = (Texture2D)GD.Load("res://assets/user interface/UiIcons.png");
        atlasTextureUpgrade.Region = new Rect2(32, 64, 16, 16);

        var atlasTextureDestroy = new AtlasTexture();
        atlasTextureDestroy.Atlas = (Texture2D)GD.Load("res://assets/user interface/UiIcons.png");
        atlasTextureDestroy.Region = new Rect2(0, 112, 16, 16);
        
        AddIconItem(atlasTextureBuild, "Build", 0);
        AddSeparator();
        AddIconItem(atlasTextureUpgrade, "Upgrade", 2);
        AddSeparator();
        AddIconItem(atlasTextureDestroy, "Destroy", 4);
    }

    public override void _Input(InputEvent @event)
    {
        if (!Input.IsActionPressed("hide_context_menu") || !Visible) return;
        GD.Print("IsAnythingPressed");
        Hide();
    }
}