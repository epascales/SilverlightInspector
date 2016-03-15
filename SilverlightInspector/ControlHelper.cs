using System.Windows;
using System.Windows.Media;

namespace SilverlightInspector
{
	/// <summary>
	/// Helpers for UI elements
	/// </summary>
	public class ControlHelper
	{
		/// <summary>
		/// This method searches all descendants recursively to return the first element of type 'T' it finds. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parentElement"></param>
		/// <returns></returns>
		public static T FindFirstDescendantElementInVisualTree<T>(DependencyObject parentElement) where T : DependencyObject
		{
			return FindFirstChildElementInVisualTree<T>(parentElement);
		}

		/// <summary>
		/// This method searches all descendants recursively to return the first element of type 'T' it finds. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parentElement"></param>
		/// <returns></returns>
		private static T FindFirstChildElementInVisualTree<T>(DependencyObject parentElement) where T : DependencyObject
		{
			var count = VisualTreeHelper.GetChildrenCount(parentElement);
			if (count == 0)
				return null;

			for (int i = 0; i < count; i++)
			{
				var child = VisualTreeHelper.GetChild(parentElement, i);

				if (child != null && child is T)
					return (T)child;

				var result = FindFirstChildElementInVisualTree<T>(child);
				if (result != null)
					return result;

			}

			return null;
		}
	}
}

