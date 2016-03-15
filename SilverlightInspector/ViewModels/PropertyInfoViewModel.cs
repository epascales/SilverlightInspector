using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using SilverlightInspector.Annotations;

namespace SilverlightInspector.ViewModels
{
	public class BasePropertyInfoViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class EmptyPropertyInfoViewModel : BasePropertyInfoViewModel
	{

	}

	public class PropertyInfoViewModel : BasePropertyInfoViewModel
	{
		public string Name { get; set; }

		public bool IsExpanded
		{
			get { return isExpanded; }
			set
			{
				isExpanded = value; OnPropertyChanged("IsExpanded");
				if (value) OnExpanded();
			}
		}

		private void OnExpanded()
		{
			Properties = Value.GetType()
					.GetProperties()
					.Where(p => !new[] { "Chars", "Item" }.Contains(p.Name))
					.OrderBy(p => p.Name)
					.Select(p => new PropertyInfoViewModel { Name = p.Name, Type = p.PropertyType, Value = p.GetValue(Value, null) })
					.ToList<BasePropertyInfoViewModel>();
		}

		private IList<BasePropertyInfoViewModel> properties = new BasePropertyInfoViewModel[] { new EmptyPropertyInfoViewModel() };
		private bool isExpanded;

		public IList<BasePropertyInfoViewModel> Properties
		{
			get
			{
				if (Value == null)
					return null;
				return properties;
			}
			set { properties = value; OnPropertyChanged("Properties"); }
		}

		public object Value { get; set; }
		public Type Type { get; set; }



		public override string ToString()
		{
			return Name + string.Format(" ({0}): {1}", Type.FullName, Value);
		}
	}
}
