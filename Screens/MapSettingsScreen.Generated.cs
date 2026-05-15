//Code for MapSettingsScreen
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;
using GumRuntime;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;
using System.Linq;
using Tidebreak.Components.Controls;
using Tidebreak.Components.Elements;
namespace Tidebreak.Screens;
partial class MapSettingsScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("MapSettingsScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named MapSettingsScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new MapSettingsScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(MapSettingsScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("MapSettingsScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public TextRuntime SettingsHeader { get; protected set; }
    public TextBox InputName { get; protected set; }
    public TextBox InputAuthor { get; protected set; }
    public TextBox InputDifficulty { get; protected set; }
    public TextBox InputSizeX { get; protected set; }
    public TextBox InputSizeY { get; protected set; }
    public TextBox InputDrownSpeed { get; protected set; }
    public TextBox InputFloodSpeed { get; protected set; }
    public NineSliceRuntime DescriptionBg { get; protected set; }
    public ButtonConfirm SaveBtn { get; protected set; }
    public ButtonStandard EditBtn { get; protected set; }
    public NineSliceRuntime FrameBG1 { get; protected set; }
    public TextRuntime MapName { get; protected set; }
    public ButtonDeny CloseBtn { get; protected set; }
    public ContainerRuntime ContainerInstance { get; protected set; }
    public NineSliceRuntime FrameBG { get; protected set; }
    public Icon IconInstance2 { get; protected set; }
    public TextRuntime DescriptionHeader { get; protected set; }
    public TextBox InputDesc { get; protected set; }

    public MapSettingsScreen(InteractiveGue visual) : base(visual)
    {
    }
    public MapSettingsScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        SettingsHeader = this.Visual?.GetGraphicalUiElementByName("SettingsHeader") as global::MonoGameGum.GueDeriving.TextRuntime;
        InputName = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<TextBox>(this.Visual,"InputName");
        InputAuthor = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<TextBox>(this.Visual,"InputAuthor");
        InputDifficulty = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<TextBox>(this.Visual,"InputDifficulty");
        InputSizeX = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<TextBox>(this.Visual,"InputSizeX");
        InputSizeY = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<TextBox>(this.Visual,"InputSizeY");
        InputDrownSpeed = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<TextBox>(this.Visual,"InputDrownSpeed");
        InputFloodSpeed = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<TextBox>(this.Visual,"InputFloodSpeed");
        DescriptionBg = this.Visual?.GetGraphicalUiElementByName("DescriptionBg") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        SaveBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonConfirm>(this.Visual,"SaveBtn");
        EditBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"EditBtn");
        FrameBG1 = this.Visual?.GetGraphicalUiElementByName("FrameBG1") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        MapName = this.Visual?.GetGraphicalUiElementByName("MapName") as global::MonoGameGum.GueDeriving.TextRuntime;
        CloseBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonDeny>(this.Visual,"CloseBtn");
        ContainerInstance = this.Visual?.GetGraphicalUiElementByName("ContainerInstance") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        FrameBG = this.Visual?.GetGraphicalUiElementByName("FrameBG") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        IconInstance2 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance2");
        DescriptionHeader = this.Visual?.GetGraphicalUiElementByName("DescriptionHeader") as global::MonoGameGum.GueDeriving.TextRuntime;
        InputDesc = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<TextBox>(this.Visual,"InputDesc");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
