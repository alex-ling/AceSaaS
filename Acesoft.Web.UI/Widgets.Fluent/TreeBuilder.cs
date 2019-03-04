using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;
using System.Text;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TreeBuilder : TreeBuilder<Tree, TreeNode, TreeBuilder>
	{
		public TreeBuilder(Tree component)
			: base(component)
		{
		}

		public virtual TreeBuilder Animate(bool animate = true)
		{
			base.Component.Animate = animate;
			return this;
		}

		public virtual TreeBuilder CheckBox(bool checkBox = true)
		{
			base.Component.Checkbox = checkBox;
			return this;
		}

		public virtual TreeBuilder CascadeCheck(bool cascadeCheck = true)
		{
			base.Component.CascadeCheck = cascadeCheck;
			return this;
		}

		public virtual TreeBuilder OnlyLeafCheck(bool onlyLeafCheck = true)
		{
			base.Component.OnlyLeafCheck = onlyLeafCheck;
			return this;
		}

		public virtual TreeBuilder Dnd(bool dnd = true)
		{
			base.Component.Dnd = dnd;
			return this;
		}

		public virtual TreeBuilder Lines(bool lines = true)
		{
			base.Component.Lines = lines;
			return this;
		}

		public virtual TreeBuilder Data(Action<StringBuilder> action)
		{
			action(base.Component.Data);
			return this;
		}

		public virtual TreeBuilder Edit(string url, int width = 0, int height = 0)
		{
			base.Component.EditUrl = url;
			if (width > 0)
			{
				base.Component.EditWidth = width;
			}
			if (height > 0)
			{
				base.Component.EditHeight = height;
			}
			return this;
		}

		public virtual TreeBuilder Delete(string api, string ds = null)
		{
			base.Component.DelApi = api;
			if (ds.HasValue())
			{
				base.Component.DelDs = ds;
			}
			return this;
		}

		public virtual TreeBuilder DataSource(string ds)
		{
			base.Component.DelDs = ds;
			base.Component.DataSource.RouteValues["ds"] = ds + "_tree";
			return this;
		}

		public TreeBuilder Root(string name, string icon = null, string value = null)
		{
			base.Component.DataSource.RouteValues["rootname"] = name;
			if (icon.HasValue())
			{
				base.Component.DataSource.RouteValues["rooticon"] = icon;
			}
			if (value.HasValue())
			{
				base.Component.DataSource.RouteValues["rootid"] = value;
			}
			return this;
		}

		public TreeBuilder Ajax(Action<DataSourceBuilder> ajaxAction)
		{
			ajaxAction(new DataSourceBuilder(base.Component.DataSource).Controller("crud").Action("tree"));
			return this;
		}

		public TreeBuilder Nodes(Action<ItemsBuilder<TreeNode, TreeNodeBuilder>> addAction)
		{
			return base.Nodes(addAction, () => new TreeNode(base.Component.Ace, base.Component), (TreeNode item) => new TreeNodeBuilder(item));
		}

		public TreeBuilder Events(Action<TreeEventBuilder> clientEventsAction)
		{
			clientEventsAction(new TreeEventBuilder(base.Component.Events));
			return this;
		}
	}
}
