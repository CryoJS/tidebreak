//Code for DeathScreen
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
partial class DeathScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("DeathScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named DeathScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new DeathScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(DeathScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("DeathScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public ButtonStandard RestartBtn { get; protected set; }
    public ButtonStandard MapsBtn { get; protected set; }
    public ButtonStandard MenuBtn { get; protected set; }
    public TextRuntime H1 { get; protected set; }
    public TextRuntime H2 { get; protected set; }
    public RoundedRectangleRuntime RoundedRectangleInstance { get; protected set; }

    public DeathScreen(InteractiveGue visual) : base(visual)
    {
    }
    public DeathScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        RestartBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"RestartBtn");
        MapsBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"MapsBtn");
        MenuBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"MenuBtn");
        H1 = this.Visual?.GetGraphicalUiElementByName("H1") as global::MonoGameGum.GueDeriving.TextRuntime;
        H2 = this.Visual?.GetGraphicalUiElementByName("H2") as global::MonoGameGum.GueDeriving.TextRuntime;
        RoundedRectangleInstance = this.Visual?.GetGraphicalUiElementByName("RoundedRectangleInstance") as global::MonoGameGum.GueDeriving.RoundedRectangleRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
