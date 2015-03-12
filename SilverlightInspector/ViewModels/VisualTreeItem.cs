using System.Windows;

namespace SilverlightInspector.ViewModels
{
	public class VisualTreeItem
	{
		public VisualTreeItem(FrameworkElement content)
		{
			Content = content;
		}

		public FrameworkElement Content;

		public string Text { get { return Content.ToString(); } }

		public override string ToString()
		{
			return Text + (string.IsNullOrEmpty(Content.Name) ?null: string.Format(" [{0}]", Content.Name));
		}
	}
}
