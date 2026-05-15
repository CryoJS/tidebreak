//Code for NewMapScreen
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
partial class NewMapScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("NewMapScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named NewMapScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new NewMapScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(NewMapScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("NewMapScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public TextBox MapName { get; protected set; }
    public TextRuntime Prompt { get; protected set; }
    public TextRuntime PreName { get; protected set; }
    public TextRuntime PreAuthor { get; protected set; }
    public NineSliceRuntime FrameBG1 { get; protected set; }
    public TextRuntime H1 { get; protected set; }
    public ButtonDeny CloseBtn { get; protected set; }
    public ButtonConfirm CreateBtn { get; protected set; }
    public ContainerRuntime ContainerInstance { get; protected set; }
    public NineSliceRuntime FrameBG { get; protected set; }
    public Icon IconInstance2 { get; protected set; }
    public TextBox Author { get; protected set; }

    public NewMapScreen(InteractiveGue visual) : base(visual)
    {
    }
    public NewMapScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        MapName = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<TextBox>(this.Visual,"MapName");
        Prompt = this.Visual?.GetGraphicalUiElementByName("Prompt") as global::MonoGameGum.GueDeriving.TextRuntime;
        PreName = this.Visual?.GetGraphicalUiElementByName("PreName") as global::MonoGameGum.GueDeriving.TextRuntime;
        PreAuthor = this.Visual?.GetGraphicalUiElementByName("PreAuthor") as global::MonoGameGum.GueDeriving.TextRuntime;
        FrameBG1 = this.Visual?.GetGraphicalUiElementByName("FrameBG1") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        H1 = this.Visual?.GetGraphicalUiElementByName("H1") as global::MonoGameGum.GueDeriving.TextRuntime;
        CloseBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonDeny>(this.Visual,"CloseBtn");
        CreateBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonConfirm>(this.Visual,"CreateBtn");
        ContainerInstance = this.Visual?.GetGraphicalUiElementByName("ContainerInstance") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        FrameBG = this.Visual?.GetGraphicalUiElementByName("FrameBG") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        IconInstance2 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance2");
        Author = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<TextBox>(this.Visual,"Author");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
