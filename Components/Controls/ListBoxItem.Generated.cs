//Code for Controls/ListBoxItem (Container)
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
namespace Tidebreak.Components.Controls;
partial class ListBoxItem : global::Gum.Forms.Controls.ListBoxItem
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new global::Gum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new global::MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("Controls/ListBoxItem");
#if DEBUG
if(element == null) throw new System.InvalidOperationException("Could not find an element named Controls/ListBoxItem - did you forget to load a Gum project?");
#endif
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new ListBoxItem(visual);
            return visual;
        });
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(ListBoxItem)] = template;
        global::Gum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(global::Gum.Forms.Controls.ListBoxItem)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("Controls/ListBoxItem", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public enum ListBoxItemCategory
    {
        Enabled,
        Highlighted,
        Selected,
        Focused,
    }

    ListBoxItemCategory? _listBoxItemCategoryState;
    public ListBoxItemCategory? ListBoxItemCategoryState
    {
        get => _listBoxItemCategoryState;
        set
        {
            _listBoxItemCategoryState = value;
            if(value != null)
            {
                if(Visual.Categories.ContainsKey("ListBoxItemCategory"))
                {
                    var category = Visual.Categories["ListBoxItemCategory"];
                    var state = category.States.Find(item => item.Name == value.ToString());
                    this.Visual.ApplyState(state);
                }
                else
                {
                    var category = ((global::Gum.DataTypes.ElementSave)this.Visual.Tag).Categories.FirstOrDefault(item => item.Name == "ListBoxItemCategory");
                    var state = category.States.Find(item => item.Name == value.ToString());
                    this.Visual.ApplyState(state);
                }
            }
        }
    }
    public ButtonConfirm PlayBtn { get; protected set; }
    public ButtonStandard EditBtn { get; protected set; }
    public NineSliceRuntime Background { get; protected set; }
    public TextRuntime Title { get; protected set; }
    public TextRuntime Author { get; protected set; }
    public TextRuntime Rating { get; protected set; }
    public ContainerRuntime Options { get; protected set; }
    public NineSliceRuntime FocusedIndicator { get; protected set; }

    public string ListItemDisplayText
    {
        get => Title.Text;
        set => Title.Text = value;
    }

    public ListBoxItem(InteractiveGue visual) : base(visual)
    {
    }
    public ListBoxItem()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        PlayBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonConfirm>(this.Visual,"PlayBtn");
        EditBtn = global::Gum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<ButtonStandard>(this.Visual,"EditBtn");
        Background = this.Visual?.GetGraphicalUiElementByName("Background") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        Title = this.Visual?.GetGraphicalUiElementByName("Title") as global::MonoGameGum.GueDeriving.TextRuntime;
        Author = this.Visual?.GetGraphicalUiElementByName("Author") as global::MonoGameGum.GueDeriving.TextRuntime;
        Rating = this.Visual?.GetGraphicalUiElementByName("Rating") as global::MonoGameGum.GueDeriving.TextRuntime;
        Options = this.Visual?.GetGraphicalUiElementByName("Options") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        FocusedIndicator = this.Visual?.GetGraphicalUiElementByName("FocusedIndicator") as global::MonoGameGum.GueDeriving.NineSliceRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
