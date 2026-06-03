//Code for SongSelectScreen
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
partial class SongSelectScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("SongSelectScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named SongSelectScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new SongSelectScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(SongSelectScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("SongSelectScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public SongRow SongRowInstance { get; protected set; }
    public NineSliceRuntime FrameBG1 { get; protected set; }
    public ListBox SongList { get; protected set; }
    public TextRuntime H1 { get; protected set; }
    public ButtonStandard ReturnBtn { get; protected set; }
    public ButtonDenyLarger NoSongBtn { get; protected set; }
    public NineSliceRuntime FrameBG { get; protected set; }
    public Icon IconInstance3 { get; protected set; }
    public Icon IconInstance5 { get; protected set; }
    public Icon IconInstance6 { get; protected set; }

    public SongSelectScreen(InteractiveGue visual) : base(visual)
    {
    }
    public SongSelectScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        SongRowInstance = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<SongRow>(this.Visual,"SongRowInstance");
        FrameBG1 = this.Visual?.GetGraphicalUiElementByName("FrameBG1") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        SongList = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ListBox>(this.Visual,"SongList");
        H1 = this.Visual?.GetGraphicalUiElementByName("H1") as global::MonoGameGum.GueDeriving.TextRuntime;
        ReturnBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"ReturnBtn");
        NoSongBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonDenyLarger>(this.Visual,"NoSongBtn");
        FrameBG = this.Visual?.GetGraphicalUiElementByName("FrameBG") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        IconInstance3 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance3");
        IconInstance5 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance5");
        IconInstance6 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance6");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
