using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using SilverlightInspector.ViewModels;
using SilverlightInspector.Views;

namespace SilverlightInspector
{
	public class Inspector
	{
		private Inspector()
		{
			Initialize();
		}

		private void Initialize()
		{
			var root = Application.Current.RootVisual as UserControl;

			if (root == null)
				throw new InvalidOperationException("RootVisual is not set.");
			
			var grid = new Grid();
			grid.Children.Add(root);
			Application.Current.RootVisual = grid;
		
			var popup = new Popup { IsOpen = true };
			Action updatePopupSize = () =>
			{
				popup.Height = Application.Current.Host.Content.ActualHeight;
				popup.Width = Application.Current.Host.Content.ActualWidth;
			};

			Application.Current.Host.Content.Resized += (s, e) =>
			{
				updatePopupSize();
			};

			var inspectorView = new InspectorView(new InspectorViewModel());
			popup.Child = inspectorView;
			grid.Children.Add(popup);
			popup.SizeChanged += (s, e) =>
			{
				inspectorView.Width = popup.ActualWidth;
				inspectorView.Height = popup.ActualHeight;
			};

			checkPopupsTimer.Tick += (s, e) =>
			{
				var openPopups = VisualTreeHelper.GetOpenPopups();
				if (openPopups.First() != popup)
				{
					popup.IsOpen = false;
					popup.IsOpen = true;
					inspectorView.IsEnabled = true;
				}
			};

			checkPopupsTimer.Start();
		}

		public static void Run()
		{
			Current = new Inspector();
		}

		public static Inspector Current { get; private set; }

		DispatcherTimer checkPopupsTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
	}
}
