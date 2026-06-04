//Code for SectionRow (Container)
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;
using GumRuntime;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;
using System.Linq;
namespace Tidebreak.Components;
partial class SectionRow : global::Gum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("SectionRow");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named SectionRow - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new SectionRow(visual);
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(SectionRow)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("SectionRow", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public TextRuntime Pg { get; protected set; }
    public NineSliceRuntime Background { get; protected set; }
    public TextRuntime Header { get; protected set; }

    public string HeaderText
    {
        get => Header.Text;
        set => Header.Text = value;
    }

    public string PgText
    {
        get => Pg.Text;
        set => Pg.Text = value;
    }

    public SectionRow(InteractiveGue visual) : base(visual)
    {
    }
    public SectionRow()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        Pg = this.Visual?.GetGraphicalUiElementByName("Pg") as global::MonoGameGum.GueDeriving.TextRuntime;
        Background = this.Visual?.GetGraphicalUiElementByName("Background") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        Header = this.Visual?.GetGraphicalUiElementByName("Header") as global::MonoGameGum.GueDeriving.TextRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
