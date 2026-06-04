//Code for EditScreen
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
partial class EditScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("EditScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named EditScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new EditScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(EditScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("EditScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public CheckBox FgBtn { get; protected set; }
    public CheckBox BgBtn { get; protected set; }
    public CheckBox EditBgBtn { get; protected set; }
    public CheckBox GridBtn { get; protected set; }
    public ButtonStandard CloseBtn { get; protected set; }
    public ButtonConfirm SaveBtn { get; protected set; }
    public ButtonYellow RedoBtn { get; protected set; }
    public ButtonYellow UndoBtn { get; protected set; }
    public ButtonStandardMini FunctionalBtn { get; protected set; }
    public ButtonStandardMini PlatformBtn { get; protected set; }
    public ButtonStandardMini DecorativeBtn { get; protected set; }
    public ButtonStandardMini ColorsBtn { get; protected set; }
    public ButtonStandardMini UnselectBtn { get; protected set; }
    public NineSliceRuntime FrameTopBG { get; protected set; }
    public ContainerRuntime ContainerInstance { get; protected set; }
    public ScrollViewer TileList { get; protected set; }
    public NineSliceRuntime BarContainer { get; protected set; }
    public Icon IconInstance2 { get; protected set; }
    public ContainerRuntime TopRightContainer { get; protected set; }
    public ContainerRuntime TopLeftContainer { get; protected set; }
    public Icon IconInstance3 { get; protected set; }

    public EditScreen(InteractiveGue visual) : base(visual)
    {
    }
    public EditScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        FgBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<CheckBox>(this.Visual,"FgBtn");
        BgBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<CheckBox>(this.Visual,"BgBtn");
        EditBgBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<CheckBox>(this.Visual,"EditBgBtn");
        GridBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<CheckBox>(this.Visual,"GridBtn");
        CloseBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"CloseBtn");
        SaveBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonConfirm>(this.Visual,"SaveBtn");
        RedoBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonYellow>(this.Visual,"RedoBtn");
        UndoBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonYellow>(this.Visual,"UndoBtn");
        FunctionalBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandardMini>(this.Visual,"FunctionalBtn");
        PlatformBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandardMini>(this.Visual,"PlatformBtn");
        DecorativeBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandardMini>(this.Visual,"DecorativeBtn");
        ColorsBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandardMini>(this.Visual,"ColorsBtn");
        UnselectBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandardMini>(this.Visual,"UnselectBtn");
        FrameTopBG = this.Visual?.GetGraphicalUiElementByName("FrameTopBG") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        ContainerInstance = this.Visual?.GetGraphicalUiElementByName("ContainerInstance") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        TileList = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ScrollViewer>(this.Visual,"TileList");
        BarContainer = this.Visual?.GetGraphicalUiElementByName("BarContainer") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        IconInstance2 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance2");
        TopRightContainer = this.Visual?.GetGraphicalUiElementByName("TopRightContainer") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        TopLeftContainer = this.Visual?.GetGraphicalUiElementByName("TopLeftContainer") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        IconInstance3 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance3");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
