using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using SilverlightInspector.Annotations;

namespace SilverlightInspector.ViewModels
{
	public class InspectorViewModel : INotifyPropertyChanged
	{
		private List<VisualTreeItem> selectedItemPath;
		private VisualTreeItem selectedItem;
		private IList<PropertyInfoViewModel> properties;
		private List<ModelItem> selectedItemModelsPath;
		private ModelItem selectedModelItem;
		private int? hashCode;

		public InspectorViewModel()
		{
			RefreshCommand = new ActionCommand(() =>
			{
				if (SelectedItem == null)
					return;

				RetrieveProperties(SelectedObject);
			});
			ShowViewHierarchyCommand = new ActionCommand(() =>
			{
				ShowViewHierarchy(selectedVisualTreeItem);
			});
		}

		public List<VisualTreeItem> SelectedItemPath
		{
			get { return selectedItemPath; }
			set { selectedItemPath = value; OnPropertyChanged("SelectedItemPath"); OnSelectedItemChanged(value); }
		}

		private void OnSelectedItemChanged(IEnumerable<VisualTreeItem> items)
		{
			SelectedItemModelsPath = items.Reverse()
				.GroupBy(i => i.Content.DataContext)
				.Select(gi => new ModelItem { VisualTreeItem = gi.First() })
				.Reverse().ToList();
		}

		public List<ModelItem> SelectedItemModelsPath
		{
			get { return selectedItemModelsPath; }
			set
			{
				if (Equals(value, selectedItemModelsPath)) return;
				selectedItemModelsPath = value;
				OnPropertyChanged("SelectedItemModelsPath");
			}
		}

		public VisualTreeItem SelectedItem
		{
			get { return selectedItem; }
			set
			{
				selectedItem = value; OnPropertyChanged("SelectedItem");
				OnSelectedItemChanged(value);
			}
		}

		public ModelItem SelectedModelItem
		{
			get { return selectedModelItem; }
			set
			{
				selectedModelItem = value; OnPropertyChanged("SelectedModelItem");
				OnSelectedModelItemChanged(value);
			}
		}

		private void OnSelectedModelItemChanged(ModelItem value)
		{
			if (value == null)
			{
				selectedVisualTreeItem = null;
				Properties = null;
			}
			selectedVisualTreeItem = value.VisualTreeItem;

			RetrieveProperties(value.VisualTreeItem.Content.DataContext);
		}

		private void ShowViewHierarchy(VisualTreeItem visualTreeItem)
		{
			if (visualTreeItem == null || visualTreeItem.Content == null)
				return;

			var parentsHierarchy = ControlHelper.GetParentsHierarchy(visualTreeItem.Content);
			var path = new List<FrameworkElement>() { visualTreeItem.Content };
			path.AddRange(parentsHierarchy.OfType<FrameworkElement>().ToList());
			SelectedItemPath = path.Select(i => new VisualTreeItem(i)).ToList();
			SelectedItem = visualTreeItem;
		}

		private VisualTreeItem selectedVisualTreeItem;

		private void OnSelectedItemChanged(VisualTreeItem value)
		{
			selectedVisualTreeItem = value;
			if (value == null)
			{
				Properties = null;
				return;
			}

			RetrieveProperties(value.Content);
		}

		public int? HashCode
		{
			get { return hashCode; }
			set
			{
				if (value == hashCode) return;
				hashCode = value;
				OnPropertyChanged("HashCode");
			}
		}

		public object SelectedObject { get; set; }

		void RetrieveProperties(object obj)
		{
			SelectedObject = obj;
			if (obj == null || !obj.GetType().IsClass)
			{
				Properties = null;
				HashCode = null;
				return;
			}

			HashCode = obj.GetHashCode();
			Properties = obj.GetType()
					.GetProperties()
					.Where(p => p.GetIndexParameters().Length == 0)
					.OrderBy(p => p.Name)
					.Select(p =>
					{
						object val;
						try
						{
							val = p.GetValue(obj, null);
						}
						catch (Exception ex)
						{
							val = string.Format("<Exception: {0}>", ex.Message);
						}
						return new PropertyInfoViewModel { Name = p.Name, Type = p.PropertyType, Value = val };
					})
					.ToList();
		}

		public ICommand RefreshCommand { get; private set; }
		public ICommand ShowViewHierarchyCommand { get; private set; }

		public IList<PropertyInfoViewModel> Properties
		{
			get { return properties; }
			set { properties = value; OnPropertyChanged("Properties"); }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class ModelItem
	{
		public VisualTreeItem VisualTreeItem { get; set; }

		public override string ToString()
		{
			object vm = VisualTreeItem.Content.DataContext;

			if (vm == null)
			{
				vm = "<null>";
			}

			return string.Format("{0} [{1}]", vm, VisualTreeItem);
		}
	}
}
