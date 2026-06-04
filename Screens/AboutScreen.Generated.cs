//Code for AboutScreen
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;
using GumRuntime;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;
using System.Linq;
using Tidebreak.Components;
using Tidebreak.Components.Controls;
using Tidebreak.Components.Elements;
namespace Tidebreak.Screens;
partial class AboutScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("AboutScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named AboutScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new AboutScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(AboutScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("AboutScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public SectionRow Welcome { get; protected set; }
    public SectionRow Controls { get; protected set; }
    public SectionRow Editor { get; protected set; }
    public SectionRow Diffs { get; protected set; }
    public SectionRow Credits { get; protected set; }
    public ListBox SectionList { get; protected set; }
    public TextRuntime H1 { get; protected set; }
    public ButtonStandard ReturnBtn { get; protected set; }
    public NineSliceRuntime FrameBG { get; protected set; }
    public Icon IconInstance2 { get; protected set; }

    public AboutScreen(InteractiveGue visual) : base(visual)
    {
    }
    public AboutScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        Welcome = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<SectionRow>(this.Visual,"Welcome");
        Controls = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<SectionRow>(this.Visual,"Controls");
        Editor = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<SectionRow>(this.Visual,"Editor");
        Diffs = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<SectionRow>(this.Visual,"Diffs");
        Credits = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<SectionRow>(this.Visual,"Credits");
        SectionList = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ListBox>(this.Visual,"SectionList");
        H1 = this.Visual?.GetGraphicalUiElementByName("H1") as global::MonoGameGum.GueDeriving.TextRuntime;
        ReturnBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"ReturnBtn");
        FrameBG = this.Visual?.GetGraphicalUiElementByName("FrameBG") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        IconInstance2 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance2");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
