//Code for MapRow (Container)
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
partial class MapRow : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("MapRow");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named MapRow - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new MapRow(visual);
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(MapRow)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("MapRow", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public TextRuntime DifficultyText { get; protected set; }
    public IconLarge IconLargeInstance2 { get; protected set; }
    public IconLarge IconLargeInstance4 { get; protected set; }
    public IconLarge IconLargeInstance3 { get; protected set; }
    public IconLarge IconLargeInstance5 { get; protected set; }
    public IconLarge IconLargeInstance1 { get; protected set; }
    public IconLarge IconLargeInstance6 { get; protected set; }
    public ButtonConfirm PlayBtn { get; protected set; }
    public ButtonStandard DetailsBtn { get; protected set; }
    public ButtonStandard EditBtn { get; protected set; }
    public ButtonConfirm PlayBtn1 { get; protected set; }
    public ButtonStandard DetailsBtn1 { get; protected set; }
    public NineSliceRuntime Background { get; protected set; }
    public TextRuntime TitleText { get; protected set; }
    public NineSliceRuntime DifficultyBG { get; protected set; }
    public TextRuntime AuthorText { get; protected set; }
    public ContainerRuntime Options { get; protected set; }
    public NineSliceRuntime FocusedIndicator { get; protected set; }

    public MapRow(InteractiveGue visual) : base(visual)
    {
    }
    public MapRow()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        DifficultyText = this.Visual?.GetGraphicalUiElementByName("DifficultyText") as global::MonoGameGum.GueDeriving.TextRuntime;
        IconLargeInstance2 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<IconLarge>(this.Visual,"IconLargeInstance2");
        IconLargeInstance4 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<IconLarge>(this.Visual,"IconLargeInstance4");
        IconLargeInstance3 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<IconLarge>(this.Visual,"IconLargeInstance3");
        IconLargeInstance5 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<IconLarge>(this.Visual,"IconLargeInstance5");
        IconLargeInstance1 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<IconLarge>(this.Visual,"IconLargeInstance1");
        IconLargeInstance6 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<IconLarge>(this.Visual,"IconLargeInstance6");
        PlayBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonConfirm>(this.Visual,"PlayBtn");
        DetailsBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"DetailsBtn");
        EditBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"EditBtn");
        PlayBtn1 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonConfirm>(this.Visual,"PlayBtn1");
        DetailsBtn1 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"DetailsBtn1");
        Background = this.Visual?.GetGraphicalUiElementByName("Background") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        TitleText = this.Visual?.GetGraphicalUiElementByName("TitleText") as global::MonoGameGum.GueDeriving.TextRuntime;
        DifficultyBG = this.Visual?.GetGraphicalUiElementByName("DifficultyBG") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        AuthorText = this.Visual?.GetGraphicalUiElementByName("AuthorText") as global::MonoGameGum.GueDeriving.TextRuntime;
        Options = this.Visual?.GetGraphicalUiElementByName("Options") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        FocusedIndicator = this.Visual?.GetGraphicalUiElementByName("FocusedIndicator") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
