using Idox.LGDP.Apps.Common.OnSite;
using System.ComponentModel;
using ServiceRequest.ViewModels;
using System;
using System.Linq;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Pages;
using ServiceRequest.Views.ViewCells;
using Xamarin.Forms;
using ServiceRequest.AppContext;
using ServiceRequest;
using ServiceRequest.DependencyInterfaces;



namespace ServiceRequest.Views.PopUp
{
    public class TypedParagraph
    {
        public string Key;
        public string Type;
        public OnSiteConfigPara Paragraph;
    }
	public partial class ParagraphView
	{
		/// ------------------------------------------------------------------------------------------------

		#region public Variables

		/// ------------------------------------------------------------------------------------------------
		/// 
		public static ParagraphViewModel ParaViewModel { get; set; }

		public static Action<TypedParagraph> Delete { get; set; }

		public static ParagraphView CurrentInstance { get; private set; }

		private Action Refresh { get; }

		private View _tempView;
		public static PopupInstanceLayout CenterPopup { get; set; }
		public static PopupInstanceLayout RelativePopup { get; set; }
		private ScrollView PageScroll { get; set; }

		#endregion

		/// ------------------------------------------------------------------------------------------------

		/// ------------------------------------------------------------------------------------------------

		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		public ParagraphView(Action refreshlist)
		{
			try
			{
				InitializeComponent();
				Refresh = refreshlist;
				ParaViewModel = new ParagraphViewModel();
				CurrentInstance = this;
				TapGestures();
				AddNewCell();
				Lstvw_TypedParagraphs.ItemsSource = ParaViewModel.SelectedParagraphs;
				ParagraphViewCell.CellTextUpdated += AddNewCell;
				Delete = OnDelete;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}
		#endregion

		/// ------------------------------------------------------------------------------------------------

		#region public Functions

		/// ------------------------------------------------------------------------------------------------
		/// 

		/// <summary>
		/// Index of selected item in the TypedParagraph List 
		/// </summary>
		/// <returns></returns>
		public int SelectedIndex(TypedParagraph typedParagraph)
		{
			try
			{
				int index = -1;
				if (typedParagraph != null)
				{
					index = ParaViewModel.SelectedParagraphs.IndexOf(typedParagraph);
				}
				return index;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				System.Diagnostics.Debug.WriteLine(ex.Message);
				return -1;
			}

		}
		#endregion

		/// ------------------------------------------------------------------------------------------------

		#region Private Functions

		/// ------------------------------------------------------------------------------------------------
		/// 

		private void TapGestures()
		{
			try
			{
				TapGestureRecognizer tapCancel = new TapGestureRecognizer();
				tapCancel.Tapped += OnCancel;
				Lbl_Cancel.GestureRecognizers.Add(tapCancel);

				TapGestureRecognizer tapSave = new TapGestureRecognizer();
				tapSave.Tapped += OnSave;
				Lbl_Save.GestureRecognizers.Add(tapSave);
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		/// ------------------------------------------------------------------------------------------------
		/// Name		OnCancel
		/// 
		/// <summary>	Dismisses the popup on clicking Cancel button.
		/// </summary>
		/// <param name="sender"> </param>
		///   /// <param name="e"> event arguments</param>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		private void OnCancel(object sender, EventArgs e)
		{
			try
			{
				VisitActionDetailsPage.CenterPopup?.DismisPopup();
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		AddNewCell
		/// 
		/// <summary>
		/// Adds the new cell into the typePara list.
		/// </summary>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		///
		private void AddNewCell()
		{
			try
			{
				bool isPossible;
				if (ParaViewModel.SelectedParagraphs?.Count <= 0)
				{
					isPossible = true;
				}
				else
				{
					var laseCell = ParaViewModel.SelectedParagraphs.Last();

					if (laseCell != null)
					{
						if (laseCell.Type != "Type" || laseCell.Paragraph.ParagraphText != "Paragraph")
							isPossible = true;
						else
							isPossible = false;
					}
					else
					{
						isPossible = false;
					}
				}

				if (isPossible)
				{
					ParaViewModel.SelectedParagraphs?.Add(new TypedParagraph
					{
						Key = "1",
						Paragraph = new OnSiteConfigPara()
						{
							ParagraphText = "Paragraph"
						},
						Type = "Type"
					});
				}
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSave
		/// 
		/// <summary>	Saves the data and Dismisses the popup on clicking Save button.
		/// </summary>
		/// <param name="sender"> </param>
		///   /// <param name="e"> event arguments</param>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		private void OnSave(object sender, EventArgs e)
		{
			try
			{
				var pts = ParaViewModel;
				pts.SelectedParagraphs.RemoveAt(pts.SelectedParagraphs.Count - 1);
				// Check all paragraphs have a Type and paragraph set
				foreach (TypedParagraph pc in pts.SelectedParagraphs)
				{
					if (pc.Type == "Type" || pc.Paragraph.ParagraphText == "Paragraph")
					{
						if (Device.OS == TargetPlatform.iOS)
						{
							DependencyService.Get<IDisplayAlertPopup>().ShowAlertParagraph();
						}
						else
						{ 
							SplitView.DisplayAlert("Error", "All items must have both Type and Paragraph set.", "OK", null);
						}
						AddNewCell();
						return;
					}
				}
				//
				// Save paragraphs back to action
				foreach (TypedParagraph pc in pts.SelectedParagraphs)
				{
					var pi = new SRiActionParagraph
					{
						ParagraphType = pc.Type,
						Text = pc.Paragraph.ParagraphText
					};
					// Don't add the made up code from "Insert Custom Text" option
					if (pc.Paragraph.Code != ParagraphViewModel.CustomCode)
						pi.ParagraphCodeInserts = pc.Paragraph.Code;
					pi.ActionKeyVal = AppData.PropertyModel.SelectedAction.Action.KeyVal;

					// Insert new paragraph at the end of this TypeCode group
					var paras = AppData.PropertyModel.SelectedAction.Action.Paragraphs;
					var i = paras.Count - 1;
					while ((i >= 0) && (string.CompareOrdinal(paras[i].ParagraphType, pi.ParagraphType) > 0))
						i--;

					// Might as well set the sequance here. I'm also tempted to (re)set all sequence values it in VisitActionEditView.cmdSave_Click
					if ((i >= 0) && (paras[i].ParagraphType == pi.ParagraphType) && (paras[i].Sequence.HasValue))
						pi.Sequence = paras[i].Sequence.Value + 1;
					else
						pi.Sequence = 0;

					paras.Insert(i + 1, pi);

				}
				VisitActionDetailsPage.CenterPopup.DismisPopup();
				Refresh();
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		/// ------------------------------------------------------------------------------------------------
		/// Name		OnDelete
		/// 
		/// <summary>	Delete's the selected Paragraph.
		/// </summary>
		/// /// <param name="typedParagraph"> The selected para</param>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		private void OnDelete(TypedParagraph typedParagraph)
		{
			try
			{
				if (ParaViewModel.SelectedParagraphs.Count > 1)
					ParaViewModel.SelectedParagraphs.Remove(typedParagraph);
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		#endregion



		/// <summary>
		///   Note      Drawpopup
		/// </summary>

		public void Drawpopup()
		{
			_tempView = Content;
			CenterPopup = new PopupInstanceLayout(this.Content, this, PageScroll);
			Content = CenterPopup;

			RelativePopup = new PopupInstanceLayout(this.Content, this, PageScroll);
			Content = RelativePopup; ;
		}


		public void ClearScreen()
		{
			Content = _tempView;
		}


	}
}
