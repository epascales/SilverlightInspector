using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace SilverlightInspector.Extensions
{
	public class SelectedItemTreeViewBehavior : Behavior<TreeView>
	{
		protected override void OnAttached()
		{
			AssociatedObject.SelectedItemChanged += (s, e) =>
			{
				SelectedItem = e.NewValue;
			};
		}

		public static DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem",
			typeof(object), typeof(SelectedItemTreeViewBehavior), new PropertyMetadata(OnSelectedItemChanged));



		private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == null)
				return;
			
		}

		public object SelectedItem
		{
			get { return GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}
	}
}
