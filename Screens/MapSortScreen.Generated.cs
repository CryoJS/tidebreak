//Code for MapSortScreen
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
partial class MapSortScreen : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("MapSortScreen");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named MapSortScreen - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new MapSortScreen(visual);
            visual.Width = 0;
            visual.WidthUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = global::Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(MapSortScreen)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("MapSortScreen", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public NineSliceRuntime FrameBG1 { get; protected set; }
    public TextRuntime H1 { get; protected set; }
    public ButtonDeny CloseBtn { get; protected set; }
    public ComboBox SortCombo { get; protected set; }
    public NineSliceRuntime FrameBG { get; protected set; }
    public Icon IconInstance3 { get; protected set; }

    public MapSortScreen(InteractiveGue visual) : base(visual)
    {
    }
    public MapSortScreen()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        FrameBG1 = this.Visual?.GetGraphicalUiElementByName("FrameBG1") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        H1 = this.Visual?.GetGraphicalUiElementByName("H1") as global::MonoGameGum.GueDeriving.TextRuntime;
        CloseBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonDeny>(this.Visual,"CloseBtn");
        SortCombo = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ComboBox>(this.Visual,"SortCombo");
        FrameBG = this.Visual?.GetGraphicalUiElementByName("FrameBG") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        IconInstance3 = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconInstance3");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
