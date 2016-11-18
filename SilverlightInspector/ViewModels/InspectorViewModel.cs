using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
				Properties = null;
			}

			RetrieveProperties(value.VisualTreeItem.Content.DataContext);
		}

		private void OnSelectedItemChanged(VisualTreeItem value)
		{
			if (value == null)
			{
				Properties = null;
				return;
			}

			RetrieveProperties(value.Content);
		}

		void RetrieveProperties(object obj)
		{

			if (obj == null || !obj.GetType().IsClass)
			{
				Properties = null;
				return;
			}

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
