using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace SilverlightInspector.Behaviors
{
	public class BindIsExpandedBehavior : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			AssociatedObject.Loaded += (s, e) =>
			{
				var treeViewItem = GetParent<TreeViewItem>(AssociatedObject);
				treeViewItem.SetBinding(TreeViewItem.IsExpandedProperty, new Binding("IsExpanded") { Mode = BindingMode.TwoWay });
			};
		}

		public static T GetParent<T>(DependencyObject control) where T : DependencyObject
		{
			var parent = VisualTreeHelper.GetParent(control);
			return parent is T || parent == null ? parent as T : GetParent<T>(parent);
		}
	}
}
