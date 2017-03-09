using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SilverlightInspector.ViewModels
{
	public class VisualTreeItem
	{
		public VisualTreeItem(FrameworkElement content)
		{
			Content = content;
		}

		public FrameworkElement Content;
		private IList<VisualTreeItem> children;

		public string Text { get { return Content.ToString(); } }

		public override string ToString()
		{
			return Text + (string.IsNullOrEmpty(Content.Name) ? null : string.Format(" [{0}]", Content.Name));
		}

		public IList<VisualTreeItem> Children
		{
			get
			{
				if (Content == null)
					return null;

				var childrenCount = VisualTreeHelper.GetChildrenCount(Content);

				return
					Enumerable.Range(0, childrenCount)
						.Select(i => VisualTreeHelper.GetChild(Content, i))
						.OfType<FrameworkElement>()
						.Select(i => new VisualTreeItem(i))
						.ToList();

				//var panel = Content as Panel;

				//if (panel != null)
				//	return panel.Children.OfType<FrameworkElement>().Select(i => new VisualTreeItem(i)).ToList();

				//var contentControl = Content as ContentControl;

				//if (contentControl != null && contentControl.Content != null)
				//	return new List<VisualTreeItem>() { new VisualTreeItem(VisualTreeHelper.GetChild(Content, 0) as FrameworkElement) };

				//var itemsControl = Content as ItemsControl;

				//if(itemsControl!=null)
				//	return itemsControl.Items.OfType<FrameworkElement>().Select(i => new VisualTreeItem(i)).ToList();

				//return null;
			}
		}
	}
}
