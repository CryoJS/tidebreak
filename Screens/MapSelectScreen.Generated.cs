//Code for MapSelectScreen
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
namespace Tidebreak.Screens;
partial class MapSelectScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("MapSelectScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named MapSelectScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new MapSelectScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(MapSelectScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("MapSelectScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public SpriteRuntime tidebreakbg { get; protected set; }
    public ListBox MapList { get; protected set; }
    public TextRuntime H1 { get; protected set; }
    public NineSliceRuntime FrameBG { get; protected set; }
    public ButtonStandard ReturnBtn { get; protected set; }

    public MapSelectScreen(InteractiveGue visual) : base(visual)
    {
    }
    public MapSelectScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        tidebreakbg = this.Visual?.GetGraphicalUiElementByName("tidebreakbg") as global::MonoGameGum.GueDeriving.SpriteRuntime;
        MapList = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ListBox>(this.Visual,"MapList");
        H1 = this.Visual?.GetGraphicalUiElementByName("H1") as global::MonoGameGum.GueDeriving.TextRuntime;
        FrameBG = this.Visual?.GetGraphicalUiElementByName("FrameBG") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        ReturnBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"ReturnBtn");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
