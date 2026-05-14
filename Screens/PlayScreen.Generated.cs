//Code for PlayScreen
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
partial class PlayScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("PlayScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named PlayScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new PlayScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(PlayScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("PlayScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public TextRuntime TimeText { get; protected set; }
    public Icon TimeTextIcon { get; protected set; }
    public IconSmall BestTimeTextIcon { get; protected set; }
    public ButtonIcon PauseBtn { get; protected set; }
    public TextRuntime NameText { get; protected set; }
    public PercentBarIcon OxygenBar { get; protected set; }
    public TextRuntime BestTimeText { get; protected set; }
    public ContainerRuntime TopCenterContainer { get; protected set; }
    public RoundedRectangleRuntime Vignette { get; protected set; }

    public PlayScreen(InteractiveGue visual) : base(visual)
    {
    }
    public PlayScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        TimeText = this.Visual?.GetGraphicalUiElementByName("TimeText") as global::MonoGameGum.GueDeriving.TextRuntime;
        TimeTextIcon = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"TimeTextIcon");
        BestTimeTextIcon = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<IconSmall>(this.Visual,"BestTimeTextIcon");
        PauseBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonIcon>(this.Visual,"PauseBtn");
        NameText = this.Visual?.GetGraphicalUiElementByName("NameText") as global::MonoGameGum.GueDeriving.TextRuntime;
        OxygenBar = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<PercentBarIcon>(this.Visual,"OxygenBar");
        BestTimeText = this.Visual?.GetGraphicalUiElementByName("BestTimeText") as global::MonoGameGum.GueDeriving.TextRuntime;
        TopCenterContainer = this.Visual?.GetGraphicalUiElementByName("TopCenterContainer") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        Vignette = this.Visual?.GetGraphicalUiElementByName("Vignette") as global::MonoGameGum.GueDeriving.RoundedRectangleRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
