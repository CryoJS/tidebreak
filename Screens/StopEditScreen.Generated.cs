//Code for StopEditScreen
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
partial class StopEditScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("StopEditScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named StopEditScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new StopEditScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(StopEditScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("StopEditScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public ButtonConfirm SaveBtn { get; protected set; }
    public ButtonDenyLarger ExitBtn { get; protected set; }
    public NineSliceRuntime FrameBG1 { get; protected set; }
    public TextRuntime H1 { get; protected set; }
    public ButtonDeny CloseBtn { get; protected set; }
    public ContainerRuntime ContainerInstance { get; protected set; }
    public NineSliceRuntime FrameBG { get; protected set; }
    public Icon IconInstance2 { get; protected set; }

    public StopEditScreen(InteractiveGue visual) : base(visual)
    {
    }
    public StopEditScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        SaveBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonConfirm>(this.Visual,"SaveBtn");
        ExitBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonDenyLarger>(this.Visual,"ExitBtn");
        FrameBG1 = this.Visual?.GetGraphicalUiElementByName("FrameBG1") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        H1 = this.Visual?.GetGraphicalUiElementByName("H1") as global::MonoGameGum.GueDeriving.TextRuntime;
        CloseBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonDeny>(this.Visual,"CloseBtn");
        ContainerInstance = this.Visual?.GetGraphicalUiElementByName("ContainerInstance") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        FrameBG = this.Visual?.GetGraphicalUiElementByName("FrameBG") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        IconInstance2 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance2");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
