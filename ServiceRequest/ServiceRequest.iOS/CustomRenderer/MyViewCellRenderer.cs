using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ServiceRequest.Custom;
using CommercialPremises.IOS;
using ServiceRequest.Pages;
using ServiceRequest.Views;
using Foundation;
using Idox.LGDP.Apps.ServiceRequest.Client;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ServiceRequest.IOS;

[assembly: ExportRenderer(typeof(SrViewCell), typeof(MyViewCellRenderer))]
namespace CommercialPremises.IOS
{
	public class MyViewCellRenderer : ViewCellRenderer
	{
		private static NSIndexPath sourceIndexPath, destinationIndexPath;
		private static UITableView sourceTableView;

		public static List<SRiActionParagraph> Items { get; set; }

		private class LongPressGestureRecognizer : UILongPressGestureRecognizer
		{
			private static UIView dragAndDropView;

			private WeakReference<UITableView> TableView { get; set; }

			private WeakReference<ViewCell> ViewCell { get; set; }



			public static LongPressGestureRecognizer CreateGesture(UITableView tableView, ViewCell cell)
			{
				return new LongPressGestureRecognizer(OnRecognizing)
				{
					TableView = new WeakReference<UITableView>(tableView),
					ViewCell = new WeakReference<ViewCell>(cell),
				};
			}

			private static void OnRecognizing(UILongPressGestureRecognizer r)
			{
				LongPressGestureRecognizer recognizer = r as LongPressGestureRecognizer;
				UITableView tableView;
				recognizer.TableView.TryGetTarget(out tableView);
				ViewCell cell;
				recognizer.ViewCell.TryGetTarget(out cell);
				if (tableView == null || cell == null)
				{
					return;
				}
				OnRecognizing(recognizer, tableView, cell);
			}

			private static void OnRecognizing(LongPressGestureRecognizer recognizer, UITableView tableView, ViewCell cell)
			{
				try
				{

					NSIndexPath indexPath = tableView.IndexPathForRowAtPoint(recognizer.LocationInView(tableView));

					Items = AppData.PropertyModel.SelectedAction.Action.Paragraphs;
					if (indexPath != null)
					{
						if (VisitActionDetailsPage.CurrentInstance.IsEditable)
						{
							if (!VisitActionDetailsPage.CurrentInstance.SingleTypeCode.Contains(Items[(int)indexPath.LongRow].ParagraphType))
							{
								switch (recognizer.State)
								{
									case UIGestureRecognizerState.Began:
										tableView.ScrollEnabled = false;
										if (indexPath != null)
										{
											// Remember the source row
											sourceIndexPath = indexPath;
											destinationIndexPath = indexPath;
											var selectedCell = tableView.CellAt(indexPath);
											UIGraphics.BeginImageContext(selectedCell.ContentView.Bounds.Size);
											selectedCell.ContentView.Layer.RenderInContext(UIGraphics.GetCurrentContext());
											UIImage img = UIGraphics.GetImageFromCurrentImageContext();
											UIGraphics.EndImageContext();


											UIImageView iv = new UIImageView(img);
											dragAndDropView = new UIView();
											dragAndDropView.Frame = iv.Frame;
											dragAndDropView.Add(iv);
											//dragAndDropView.BackgroundColor = UIColor.Blue;
											sourceTableView = tableView;

											UIApplication.SharedApplication.KeyWindow.Add(dragAndDropView);

											dragAndDropView.Center = recognizer.LocationInView(UIApplication.SharedApplication.KeyWindow);

											dragAndDropView.AddGestureRecognizer(selectedCell.GestureRecognizers[0]);
										}

										break;
									case UIGestureRecognizerState.Changed:
										dragAndDropView.Center = recognizer.LocationInView(UIApplication.SharedApplication.KeyWindow);
										destinationIndexPath = indexPath;
										break;

									case UIGestureRecognizerState.Cancelled:
									case UIGestureRecognizerState.Failed:
										sourceIndexPath = null;
										cell.View.BackgroundColor = Color.Transparent;
										break;
									case UIGestureRecognizerState.Ended:

										if (dragAndDropView == null)
											return;

										dragAndDropView.RemoveFromSuperview();
										dragAndDropView = null;

										UIView view = UIApplication.SharedApplication.KeyWindow;

										UIView viewHit = view.HitTest(recognizer.LocationInView(view), null);

										int removeLocation = (int)sourceIndexPath.Item;
										int insertLocation = destinationIndexPath != null ? (int)destinationIndexPath.Item : -1;

										while (!(viewHit is UITableViewCell) && viewHit != null)
										{
											viewHit = viewHit.Superview;
										}

										UIView tempView = viewHit?.Superview;

										while (!(tempView is UITableView) && tempView != null)
										{
											tempView = tempView.Superview;
										}

										insertLocation = (int)tableView.IndexPathForCell((UITableViewCell)viewHit).Item;

										if (CustomTableViewRenderer.ListMap.ContainsKey(tableView.Tag.ToString()))
										{
											var sourceList = (IList)CustomTableViewRenderer.ListMap[tableView.Tag.ToString()].Item2;

											if (!tableView.Tag.Equals(tableView.Tag) || removeLocation != insertLocation)
											{


												if (Items[removeLocation].ParagraphType == Items[insertLocation].ParagraphType)
												{
													if (sourceList.Contains(cell.BindingContext))
													{
														sourceList.Remove(cell.BindingContext);

														if (insertLocation != -1)
															sourceList.Insert(insertLocation, cell.BindingContext);
														else
															sourceList.Add(cell.BindingContext);
													}
													tableView.ReloadData();
												}

											}
										}

										VisitActionDetailsPage.CurrentInstance.RefreshList();
										tableView.ScrollEnabled = true;

										break;
								}
							}
							else {

								dragAndDropView.RemoveFromSuperview();
								VisitActionDetailsPage.CurrentInstance.RefreshList();
							}
						}
					}
					else {
						dragAndDropView.RemoveFromSuperview();
						VisitActionDetailsPage.CurrentInstance.RefreshList();
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine("==={0}===", ex.Message);
				}
			}

			private LongPressGestureRecognizer(Action<UILongPressGestureRecognizer> action) : base(action)
			{
			}
		}
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusedCell, UITableView tableView)
		{
			UITableViewCell tableViewCell = base.GetCell(item, reusedCell, tableView);
			AddGestures(item as ViewCell, tableViewCell, tableView);
			return tableViewCell;
		}

		private void AddGestures(ViewCell cell, UITableViewCell tableViewCell, UITableView tableView)
		{
			if (VisitActionDetailsPage.CurrentInstance.IsEditable)
			{
				if (VisitActionDetailsPage.CurrentInstance.SingleTypeCode != null)
				{
					tableViewCell.AddGestureRecognizer(LongPressGestureRecognizer.CreateGesture(tableView, cell));
				}
			}
			else
			{
				tableViewCell.RemoveGestureRecognizer(LongPressGestureRecognizer.CreateGesture(tableView, cell));
			}
		}
	}
}
