//Code for WinScreen
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
partial class WinScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("WinScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named WinScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new WinScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(WinScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("WinScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public ContainerRuntime NewBestEffect { get; protected set; }
    public RoundedRectangleRuntime NewBestVignette { get; protected set; }
    public RoundedRectangleRuntime WinVignette { get; protected set; }
    public TextRuntime NewBestTimeText { get; protected set; }
    public ButtonStandard RestartBtn { get; protected set; }
    public ButtonStandard MapsBtn { get; protected set; }
    public ButtonStandard MenuBtn { get; protected set; }
    public TextRuntime H1 { get; protected set; }
    public TextRuntime H2 { get; protected set; }
    public NineSliceRuntime NineSliceInstance { get; protected set; }

    public WinScreen(InteractiveGue visual) : base(visual)
    {
    }
    public WinScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        NewBestEffect = this.Visual?.GetGraphicalUiElementByName("NewBestEffect") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        NewBestVignette = this.Visual?.GetGraphicalUiElementByName("NewBestVignette") as global::MonoGameGum.GueDeriving.RoundedRectangleRuntime;
        WinVignette = this.Visual?.GetGraphicalUiElementByName("WinVignette") as global::MonoGameGum.GueDeriving.RoundedRectangleRuntime;
        NewBestTimeText = this.Visual?.GetGraphicalUiElementByName("NewBestTimeText") as global::MonoGameGum.GueDeriving.TextRuntime;
        RestartBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"RestartBtn");
        MapsBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"MapsBtn");
        MenuBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"MenuBtn");
        H1 = this.Visual?.GetGraphicalUiElementByName("H1") as global::MonoGameGum.GueDeriving.TextRuntime;
        H2 = this.Visual?.GetGraphicalUiElementByName("H2") as global::MonoGameGum.GueDeriving.TextRuntime;
        NineSliceInstance = this.Visual?.GetGraphicalUiElementByName("NineSliceInstance") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
