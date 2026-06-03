//Code for TitleScreen
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
partial class TitleScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("TitleScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named TitleScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new TitleScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(TitleScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("TitleScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public ButtonStandard PlayBtn { get; protected set; }
    public ButtonStandard AboutBtn { get; protected set; }
    public ButtonStandard SettingsBtn { get; protected set; }
    public ButtonStandard ExitBtn { get; protected set; }
    public SpriteRuntime tidebreakbg { get; protected set; }
    public TextRuntime TitleText { get; protected set; }
    public ContainerRuntime Options { get; protected set; }

    public TitleScreen(InteractiveGue visual) : base(visual)
    {
    }
    public TitleScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        PlayBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"PlayBtn");
        AboutBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"AboutBtn");
        SettingsBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"SettingsBtn");
        ExitBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"ExitBtn");
        tidebreakbg = this.Visual?.GetGraphicalUiElementByName("tidebreakbg") as global::MonoGameGum.GueDeriving.SpriteRuntime;
        TitleText = this.Visual?.GetGraphicalUiElementByName("TitleText") as global::MonoGameGum.GueDeriving.TextRuntime;
        Options = this.Visual?.GetGraphicalUiElementByName("Options") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
