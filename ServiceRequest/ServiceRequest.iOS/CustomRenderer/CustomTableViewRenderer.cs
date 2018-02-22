using System;
using ServiceRequest.Custom;
using Xamarin.Forms.Platform.iOS;
using ServiceRequest.IOS;
using Xamarin.Forms;
using UIKit;
using System.Collections.Generic;
using System.Collections;
using Foundation;
using Idox.LGDP.Apps.ServiceRequest.Client;

[assembly: ExportRenderer(typeof(SrTableView), typeof(CustomTableViewRenderer))]
namespace ServiceRequest.IOS
	{
		public class CustomTableViewRenderer : TableViewRenderer, IUIGestureRecognizerDelegate
		{
			internal static Dictionary<string, Tuple<UITableView, IList>> ListMap = new Dictionary<string, Tuple<UITableView, IList>>();

			private class ReorderableTableViewSource : UITableViewSource
			{
				public WeakReference<TableView> View { get; set; }

				public UITableViewSource Source { get; set; }

				#region A replacement UITableViewSource which enables drag and drop to reorder rows

				public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
				{
					return Source.GetCell(tableView, indexPath);
				}

				public override nfloat GetHeightForHeader(UITableView tableView, nint section)
				{
					return Source.GetHeightForHeader(tableView, section);
				}

				public override UIView GetViewForHeader(UITableView tableView, nint section)
				{
					return Source.GetViewForHeader(tableView, section);
				}

				public override nint NumberOfSections(UITableView tableView)
				{
					return Source.NumberOfSections(tableView);
				}

				public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
				{
					Source.RowDeselected(tableView, indexPath);
				}

				public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
				{
					Source.RowSelected(tableView, indexPath);
				}

				public override nint RowsInSection(UITableView tableview, nint section)
				{
					return Source.RowsInSection(tableview, section);
				}

				public override string[] SectionIndexTitles(UITableView tableView)
				{
					return Source.SectionIndexTitles(tableView);
				}

				public override string TitleForHeader(UITableView tableView, nint section)
				{
					return Source.TitleForHeader(tableView, section);
				}

				#endregion

				public override void MoveRow(UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
				{
					// Don't call the base method, which is the key: the same method in
					// ListViewRenderer.ListViewDataSource throws which prevents the rows to become moveable

					// TODO: do things to actually reorder in data model, such as raise a reorder event, etc.
				}
			}


			public IList Items { get; set; }

			private new TableView Element => base.Element as TableView;

			protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
			{
				try
				{
					base.OnElementChanged(e);
				Items = AppData.PropertyModel.SelectedAction.Action.Paragraphs;
					if (Control == null)
						return;
					var tableView = Control as UITableView;
					tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

					//Control.Tag = (nint)Control.GetHashCode();
					ListMap.Clear();
					ListMap.Add(Control.Tag.ToString(), new Tuple<UITableView, IList>(Control, Items));

					Control.Source = new ReorderableTableViewSource { View = new WeakReference<TableView>(Element), Source = Control.Source };
				}
				catch (Exception ex)
				{

				}
			}

			protected override void Dispose(bool disposing)
			{
				if (Control != null)
					ListMap.Remove(Control.Tag.ToString());

				base.Dispose(disposing);
			}

		}
	}
