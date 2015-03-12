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

		public List<VisualTreeItem> SelectedItemPath
		{
			get { return selectedItemPath; }
			set { selectedItemPath = value; OnPropertyChanged("SelectedItemPath"); }
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

		private void OnSelectedItemChanged(VisualTreeItem value)
		{
			if (value == null)
			{
				Properties = null;
				return;
			}

			Properties = value.Content.GetType()
					.GetProperties()
					.OrderBy(p => p.Name)
					.Select(p => new PropertyInfoViewModel { Name = p.Name, Type = p.PropertyType, Value = p.GetValue(value.Content, null) })
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
}
