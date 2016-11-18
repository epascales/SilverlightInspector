using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using SilverlightInspector.ViewModels;

namespace SilverlightInspector.Views
{
	public partial class InspectorView
	{
		public InspectorView(InspectorViewModel viewModel)
		{
			InitializeComponent();
			DataContext = ViewModel = viewModel;
			ViewModel.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "SelectedItem")
				{
					SelectItem(ViewModel.SelectedItem == null ? null : ViewModel.SelectedItem.Content);
				}

				if (e.PropertyName == "SelectedModelItem")
				{
					SelectItem(ViewModel.SelectedModelItem == null ? null : ViewModel.SelectedModelItem.VisualTreeItem.Content);
				}
			};
		}

		InspectorViewModel ViewModel { get; set; }

		private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
		{
			gridOverlay.Visibility = Visibility.Visible;
			borderMarker.Visibility = Visibility.Visible;

			{
				var openPopups = VisualTreeHelper.GetOpenPopups().Where(p => p.Child != this && p.Child.Visibility == Visibility.Visible);



				root = openPopups.Any() ? openPopups.First().Child : Application.Current.RootVisual;
			}
		}

		private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
		{
			gridOverlay.Visibility = Visibility.Collapsed;
			//borderMarker.Visibility = Visibility.Collapsed;
		}

		private void GridOverlay_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			buttonSelect.IsChecked = false;
		}

		FrameworkElement HoveredItem { get; set; }

		void SelectItem(FrameworkElement frameworkElement)
		{
			if (frameworkElement == null)
			{
				borderMarker.Visibility = Visibility.Collapsed;
				return;
			}

			borderMarker.Visibility = Visibility.Visible;

			var translation = frameworkElement.TransformToVisual(root).Transform(new Point(0, 0));
			borderMarker.Width = frameworkElement.ActualWidth;
			borderMarker.Height = frameworkElement.ActualHeight;
			borderMarkerTransform.TranslateX = translation.X;
			borderMarkerTransform.TranslateY = translation.Y;
		}

		private UIElement root;

		private void GridOverlay_OnMouseMove(object sender, MouseEventArgs e)
		{
			var position = e.GetPosition(this);
			var items = VisualTreeHelper.FindElementsInHostCoordinates(position, root);
			IEnumerable<UIElement> selectionPath;
			FrameworkElement selectedItem;
			if (items.Contains(this))
			{
				selectionPath = items.SkipWhile(item => item != this).Skip(1);
				selectedItem = selectionPath.Take(1).First() as FrameworkElement;
			}
			else
			{
				selectionPath = items;
				selectedItem = selectionPath.FirstOrDefault() as FrameworkElement;
			}

			ViewModel.SelectedItemPath = selectionPath.OfType<FrameworkElement>().Select(fe => new VisualTreeItem(fe)).ToList();

			if (selectedItem != null && selectedItem != HoveredItem)
			{
				HoveredItem = selectedItem;
				SelectItem(HoveredItem);
			}


		}
	}
}
