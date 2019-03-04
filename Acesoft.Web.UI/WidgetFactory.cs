using Acesoft.Data;
using Acesoft.Rbac;
using Acesoft.Web.UI.Charts;
using Acesoft.Web.UI.Widgets;
using Acesoft.Web.UI.Widgets.Fluent;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Web.UI
{
	public class WidgetFactory
	{
        public IApplicationContext AppCtx { get; }
		public HttpContext Context { get; }
        public Acesoft.Data.ISession Session => AppCtx.Session;
        public IAccessControl AC => AppCtx.AccessControl;

		public RazorPageBase Page { get; }
        public string AppName { get; }
        public string Path { get; }
        public bool RenderAuthOptions { get; set; }

        public WidgetFactory(RazorPageBase page)
		{
			Page = page;

            Context = App.Context;
            AppCtx = Context.RequestServices.GetService<IApplicationContext>();

            string text = page.Path.Substring(6);
			Path = text.Substring(0, text.LastIndexOf('/') + 1);

			//string appName = App.Context.Request.GetAppName();
			//AppName = (appName.HasValue() ? appName : App.DefaultApplication);
		}

		public virtual AccordionBuilder Accordion()
		{
			return new AccordionBuilder(new Accordion(this));
		}

		public virtual LinkButtonBuilder Button()
		{
			return new LinkButtonBuilder(new LinkButton(this)).Plain(true).Css("aceui");
		}

		public virtual CalendarBuilder Calendar()
		{
			return new CalendarBuilder(new Calendar(this));
		}

		public virtual CheckBoxBuilder CheckBox()
		{
			return new CheckBoxBuilder(new CheckBox(this));
		}

		public virtual CheckBoxListBuilder CheckBoxList()
		{
			return new CheckBoxListBuilder(new CheckBoxList(this));
		}

		public virtual ComboBuilder Combo()
		{
			return new ComboBuilder(new Combo(this));
		}

		public virtual ComboBoxBuilder ComboBox()
		{
			return new ComboBoxBuilder(new ComboBox(this)).Editable(false);
		}

		public virtual ComboGridBuilder ComboGrid()
		{
			return new ComboGridBuilder(new ComboGrid(this));
		}

		public virtual ComboTreeBuilder ComboTree()
		{
			return new ComboTreeBuilder(new ComboTree(this));
		}

		public virtual DataGridBuilder DataGrid()
		{
			return new DataGridBuilder(new DataGrid(this)).IdField("id").FitColumns(true).MultiSort(true)
				.CheckBox(true)
				.Fit(true);
		}

		public virtual DataListBuilder DataList()
		{
			return new DataListBuilder(new DataList(this));
		}

		public virtual DataViewBuilder DataView()
		{
			return new DataViewBuilder(new DataView(this));
		}

		public virtual DateBoxBuilder DateBox()
		{
			return new DateBoxBuilder(new DateBox(this));
		}

		public virtual DateTimeBoxBuilder DateTimeBox()
		{
			return new DateTimeBoxBuilder(new DatetimeBox(this));
		}

		public virtual DateTimeSpinnerBuilder DateTimeSpinner()
		{
			return new DateTimeSpinnerBuilder(new DatetimeSpinner(this));
		}

		public virtual EChartBuilder EChart()
		{
			return new EChartBuilder(new EChart(this));
		}

		public virtual FormBuilder Form()
		{
			return new FormBuilder(new Form(this));
		}

		public virtual HiddenBoxBuilder HiddenBox()
		{
			return new HiddenBoxBuilder(new HiddenBox(this));
		}

		public virtual HtmlerBuilder Htmler()
		{
			return new HtmlerBuilder(new Htmler(this));
		}

		public virtual KindEditorBuilder KindEditor()
		{
			return new KindEditorBuilder(new KindEditor(this));
		}

		public virtual LayoutBuilder Layout()
		{
			return new LayoutBuilder(new Layout(this));
		}

		public virtual LinkButtonBuilder LinkButton()
		{
			return new LinkButtonBuilder(new LinkButton(this));
		}

		public virtual MenuBuilder Menu()
		{
			return new MenuBuilder(new Menu(this));
		}

		public virtual MenuButtonBuilder MenuButton()
		{
			return new MenuButtonBuilder(new MenuButton(this));
		}

		public virtual ComboBoxBuilder MonthBox()
		{
			return new ComboBoxBuilder(new MonthBox(this));
		}

		public virtual NumberBoxBuilder NumberBox()
		{
			return new NumberBoxBuilder(new NumberBox(this));
		}

		public virtual NumberSpinnerBuilder NumberSpinner()
		{
			return new NumberSpinnerBuilder(new NumberSpinner(this));
		}

		public virtual PaginationBuilder Pagination()
		{
			return new PaginationBuilder(new Pagination(this));
		}

		public virtual PanelBuilder Panel()
		{
			return new PanelBuilder(new Panel(this));
		}

		public virtual PasswordBoxBuilder PasswordBox()
		{
			return new PasswordBoxBuilder(new PasswordBox(this));
		}

		public virtual UploadBoxBuilder PictureBox()
		{
			return new UploadBoxBuilder(new UploadBox(this)).PicView(true);
		}

		public virtual ProgressBarBuilder ProgressBar()
		{
			return new ProgressBarBuilder(new ProgressBar(this));
		}

		public virtual PropertyGridBuilder PropertyGrid()
		{
			return new PropertyGridBuilder(new PropertyGrid(this));
		}

		public virtual CheckBoxBuilder RadioBox()
		{
			return new CheckBoxBuilder(new RadioBox(this));
		}

		public virtual SearchBuilder Search()
		{
			return new SearchBuilder(new Search(this)).Height("42px");
		}

		public virtual SplitButtonBuilder SplitButton()
		{
			return new SplitButtonBuilder(new SplitButton(this));
		}

		public virtual SwitchButtonBuilder SwitchButton()
		{
			return new SwitchButtonBuilder(new SwitchButton(this));
		}

		public virtual TabsBuilder Tabs()
		{
			return new TabsBuilder(new Tabs(this));
		}

		public virtual TagBoxBuilder TagBox()
		{
			return new TagBoxBuilder(new TagBox(this));
		}

		public virtual TextBoxBuilder TextArea()
		{
			return new TextBoxBuilder(new TextBox(this)).Multiline(true).Height(50);
		}

		public virtual TextBoxBuilder TextBox()
		{
			return new TextBoxBuilder(new TextBox(this));
		}

		public virtual TimeSpinnerBuilder TimeSpinner()
		{
			return new TimeSpinnerBuilder(new TimeSpinner(this));
		}

		public virtual ToolbarBuilder Toolbar()
		{
			return new ToolbarBuilder(new Toolbar(this));
		}

		public virtual TreeBuilder Tree()
		{
			return new TreeBuilder(new Tree(this)).Lines(true);
		}

		public virtual TreeGridBuilder TreeGrid()
		{
			return new TreeGridBuilder(new TreeGrid(this)).FitColumns(true).MultiSort(true).CheckBox(true)
				.Fit(true)
				.IdField("id");
		}

		public virtual UploadBoxBuilder UploadBox()
		{
			return new UploadBoxBuilder(new UploadBox(this));
		}

		public virtual ValidateBoxBuilder ValidateBox()
		{
			return new ValidateBoxBuilder(new ValidateBox(this));
		}

		public virtual FormBuilder View()
		{
			return new FormBuilder(new View(this));
		}

		public virtual YearBoxBuilder YearBox()
		{
			return new YearBoxBuilder(new YearBox(this));
		}
	}
}
