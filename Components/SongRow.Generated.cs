//Code for SongRow (Container)
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
namespace Tidebreak.Components;
partial class SongRow : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("SongRow");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named SongRow - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new SongRow(visual);
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(SongRow)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("SongRow", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public Icon IconInstance { get; protected set; }
    public Icon PlayBtnIcon { get; protected set; }
    public Icon IconInstance2 { get; protected set; }
    public Icon IconInstance3 { get; protected set; }
    public Icon IconInstance4 { get; protected set; }
    public ButtonConfirm SaveBtn { get; protected set; }
    public ButtonStandard PlayBtn { get; protected set; }
    public ButtonStandard FastForwardBtn { get; protected set; }
    public ButtonStandard FastBackBtn { get; protected set; }
    public ButtonStandard RestartBtn { get; protected set; }
    public NineSliceRuntime Background { get; protected set; }
    public NineSliceRuntime FocusedIndicator { get; protected set; }
    public TextRuntime TitleText { get; protected set; }
    public ContainerRuntime Options { get; protected set; }

    public SongRow(InteractiveGue visual) : base(visual)
    {
    }
    public SongRow()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        IconInstance = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance");
        PlayBtnIcon = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"PlayBtnIcon");
        IconInstance2 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance2");
        IconInstance3 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance3");
        IconInstance4 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance4");
        SaveBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonConfirm>(this.Visual,"SaveBtn");
        PlayBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"PlayBtn");
        FastForwardBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"FastForwardBtn");
        FastBackBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"FastBackBtn");
        RestartBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"RestartBtn");
        Background = this.Visual?.GetGraphicalUiElementByName("Background") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        FocusedIndicator = this.Visual?.GetGraphicalUiElementByName("FocusedIndicator") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        TitleText = this.Visual?.GetGraphicalUiElementByName("TitleText") as global::MonoGameGum.GueDeriving.TextRuntime;
        Options = this.Visual?.GetGraphicalUiElementByName("Options") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
