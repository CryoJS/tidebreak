//Code for SettingsScreen
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
partial class SettingsScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("SettingsScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named SettingsScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new SettingsScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(SettingsScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("SettingsScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public NineSliceRuntime FrameBG1 { get; protected set; }
    public CheckBox Fullscreen { get; protected set; }
    public TextRuntime PreMusicVol { get; protected set; }
    public TextRuntime PreSfxVol { get; protected set; }
    public ButtonConfirm SaveBtn { get; protected set; }
    public Slider MusicVol { get; protected set; }
    public Slider SfxVol { get; protected set; }
    public TextRuntime H1 { get; protected set; }
    public ButtonStandard ReturnBtn { get; protected set; }
    public NineSliceRuntime FrameBG { get; protected set; }
    public Icon IconInstance2 { get; protected set; }
    public ContainerRuntime ContainerInstance { get; protected set; }

    public SettingsScreen(InteractiveGue visual) : base(visual)
    {
    }
    public SettingsScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        FrameBG1 = this.Visual?.GetGraphicalUiElementByName("FrameBG1") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        Fullscreen = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<CheckBox>(this.Visual,"Fullscreen");
        PreMusicVol = this.Visual?.GetGraphicalUiElementByName("PreMusicVol") as global::MonoGameGum.GueDeriving.TextRuntime;
        PreSfxVol = this.Visual?.GetGraphicalUiElementByName("PreSfxVol") as global::MonoGameGum.GueDeriving.TextRuntime;
        SaveBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonConfirm>(this.Visual,"SaveBtn");
        MusicVol = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Slider>(this.Visual,"MusicVol");
        SfxVol = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Slider>(this.Visual,"SfxVol");
        H1 = this.Visual?.GetGraphicalUiElementByName("H1") as global::MonoGameGum.GueDeriving.TextRuntime;
        ReturnBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"ReturnBtn");
        FrameBG = this.Visual?.GetGraphicalUiElementByName("FrameBG") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        IconInstance2 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance2");
        ContainerInstance = this.Visual?.GetGraphicalUiElementByName("ContainerInstance") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
