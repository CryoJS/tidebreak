//Code for MapDetailsScreen
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
partial class MapDetailsScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("MapDetailsScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named MapDetailsScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new MapDetailsScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(MapDetailsScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("MapDetailsScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public TextRuntime DescriptionHeader { get; protected set; }
    public TextRuntime Description { get; protected set; }
    public TextRuntime BestTime { get; protected set; }
    public TextRuntime Author { get; protected set; }
    public TextRuntime CreationDate { get; protected set; }
    public TextRuntime ModifiedDate { get; protected set; }
    public TextRuntime Difficulty { get; protected set; }
    public TextRuntime Locked { get; protected set; }
    public TextRuntime Size { get; protected set; }
    public TextRuntime DetailsHeader { get; protected set; }
    public TextRuntime DatesHeader { get; protected set; }
    public TextRuntime BestTimeHeader { get; protected set; }
    public TextRuntime PreAuthor { get; protected set; }
    public TextRuntime PreDifficulty { get; protected set; }
    public TextRuntime PreLocked { get; protected set; }
    public TextRuntime PreSized { get; protected set; }
    public TextRuntime PreCreationDate { get; protected set; }
    public TextRuntime PreModifiedDate { get; protected set; }
    public NineSliceRuntime FrameBG1 { get; protected set; }
    public TextRuntime MapName { get; protected set; }
    public ButtonDeny CloseBtn { get; protected set; }
    public NineSliceRuntime FrameBG { get; protected set; }
    public Icon IconInstance2 { get; protected set; }
    public ContainerRuntime ContainerInstance { get; protected set; }
    public NineSliceRuntime DescriptionBg { get; protected set; }

    public MapDetailsScreen(InteractiveGue visual) : base(visual)
    {
    }
    public MapDetailsScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        DescriptionHeader = this.Visual?.GetGraphicalUiElementByName("DescriptionHeader") as global::MonoGameGum.GueDeriving.TextRuntime;
        Description = this.Visual?.GetGraphicalUiElementByName("Description") as global::MonoGameGum.GueDeriving.TextRuntime;
        BestTime = this.Visual?.GetGraphicalUiElementByName("BestTime") as global::MonoGameGum.GueDeriving.TextRuntime;
        Author = this.Visual?.GetGraphicalUiElementByName("Author") as global::MonoGameGum.GueDeriving.TextRuntime;
        CreationDate = this.Visual?.GetGraphicalUiElementByName("CreationDate") as global::MonoGameGum.GueDeriving.TextRuntime;
        ModifiedDate = this.Visual?.GetGraphicalUiElementByName("ModifiedDate") as global::MonoGameGum.GueDeriving.TextRuntime;
        Difficulty = this.Visual?.GetGraphicalUiElementByName("Difficulty") as global::MonoGameGum.GueDeriving.TextRuntime;
        Locked = this.Visual?.GetGraphicalUiElementByName("Locked") as global::MonoGameGum.GueDeriving.TextRuntime;
        Size = this.Visual?.GetGraphicalUiElementByName("Size") as global::MonoGameGum.GueDeriving.TextRuntime;
        DetailsHeader = this.Visual?.GetGraphicalUiElementByName("DetailsHeader") as global::MonoGameGum.GueDeriving.TextRuntime;
        DatesHeader = this.Visual?.GetGraphicalUiElementByName("DatesHeader") as global::MonoGameGum.GueDeriving.TextRuntime;
        BestTimeHeader = this.Visual?.GetGraphicalUiElementByName("BestTimeHeader") as global::MonoGameGum.GueDeriving.TextRuntime;
        PreAuthor = this.Visual?.GetGraphicalUiElementByName("PreAuthor") as global::MonoGameGum.GueDeriving.TextRuntime;
        PreDifficulty = this.Visual?.GetGraphicalUiElementByName("PreDifficulty") as global::MonoGameGum.GueDeriving.TextRuntime;
        PreLocked = this.Visual?.GetGraphicalUiElementByName("PreLocked") as global::MonoGameGum.GueDeriving.TextRuntime;
        PreSized = this.Visual?.GetGraphicalUiElementByName("PreSized") as global::MonoGameGum.GueDeriving.TextRuntime;
        PreCreationDate = this.Visual?.GetGraphicalUiElementByName("PreCreationDate") as global::MonoGameGum.GueDeriving.TextRuntime;
        PreModifiedDate = this.Visual?.GetGraphicalUiElementByName("PreModifiedDate") as global::MonoGameGum.GueDeriving.TextRuntime;
        FrameBG1 = this.Visual?.GetGraphicalUiElementByName("FrameBG1") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        MapName = this.Visual?.GetGraphicalUiElementByName("MapName") as global::MonoGameGum.GueDeriving.TextRuntime;
        CloseBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonDeny>(this.Visual,"CloseBtn");
        FrameBG = this.Visual?.GetGraphicalUiElementByName("FrameBG") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        IconInstance2 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance2");
        ContainerInstance = this.Visual?.GetGraphicalUiElementByName("ContainerInstance") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        DescriptionBg = this.Visual?.GetGraphicalUiElementByName("DescriptionBg") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
