//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceRequest.Pages {
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    
    
    public partial class VisitActionDetailsPage : global::Xamarin.Forms.ContentPage {
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Grid Gl_Main;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.BoxView Boxvw_Cancel;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Label Lbl_Cancel;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Label Lbl_Title;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.BoxView Boxvw_Save;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Label Lbl_Save;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.StackLayout Sl_View;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Label Lbl_ScheduledDate;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::ServiceRequest.Custom.SrDatePicker Dtp_ScheduledDate;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::ServiceRequest.Custom.SrTimePicker Tmp_ScheduledDate;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::ServiceRequest.Custom.SrPicker Pkr_TimeTakenHours;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::ServiceRequest.Custom.SrPicker Pkr_TimeTakenMinutes;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.BoxView BX_Officer;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Label Lbl_Officer;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Label Lbl_Status;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::ServiceRequest.Custom.SrPicker Pkr_Status;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::ServiceRequest.Custom.SrDatePicker Dtp_CompletedDate;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::ServiceRequest.Custom.SrTimePicker Tmp_CompletedDate;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.StackLayout Sl_Notes;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.ListView Lst_Comments;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Label Lbl_Comment;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.StackLayout Sl_Main;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::ServiceRequest.Custom.SrTableView Tblvw_StandardParagraph;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.TableSection TblSec_ParagraphList;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Label Lbl_AddParagraph;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Label Lbl_Edit;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent() {
            this.LoadFromXaml(typeof(VisitActionDetailsPage));
            Gl_Main = this.FindByName<global::Xamarin.Forms.Grid>("Gl_Main");
            Boxvw_Cancel = this.FindByName<global::Xamarin.Forms.BoxView>("Boxvw_Cancel");
            Lbl_Cancel = this.FindByName<global::Xamarin.Forms.Label>("Lbl_Cancel");
            Lbl_Title = this.FindByName<global::Xamarin.Forms.Label>("Lbl_Title");
            Boxvw_Save = this.FindByName<global::Xamarin.Forms.BoxView>("Boxvw_Save");
            Lbl_Save = this.FindByName<global::Xamarin.Forms.Label>("Lbl_Save");
            Sl_View = this.FindByName<global::Xamarin.Forms.StackLayout>("Sl_View");
            Lbl_ScheduledDate = this.FindByName<global::Xamarin.Forms.Label>("Lbl_ScheduledDate");
            Dtp_ScheduledDate = this.FindByName<global::ServiceRequest.Custom.SrDatePicker>("Dtp_ScheduledDate");
            Tmp_ScheduledDate = this.FindByName<global::ServiceRequest.Custom.SrTimePicker>("Tmp_ScheduledDate");
            Pkr_TimeTakenHours = this.FindByName<global::ServiceRequest.Custom.SrPicker>("Pkr_TimeTakenHours");
            Pkr_TimeTakenMinutes = this.FindByName<global::ServiceRequest.Custom.SrPicker>("Pkr_TimeTakenMinutes");
            BX_Officer = this.FindByName<global::Xamarin.Forms.BoxView>("BX_Officer");
            Lbl_Officer = this.FindByName<global::Xamarin.Forms.Label>("Lbl_Officer");
            Lbl_Status = this.FindByName<global::Xamarin.Forms.Label>("Lbl_Status");
            Pkr_Status = this.FindByName<global::ServiceRequest.Custom.SrPicker>("Pkr_Status");
            Dtp_CompletedDate = this.FindByName<global::ServiceRequest.Custom.SrDatePicker>("Dtp_CompletedDate");
            Tmp_CompletedDate = this.FindByName<global::ServiceRequest.Custom.SrTimePicker>("Tmp_CompletedDate");
            Sl_Notes = this.FindByName<global::Xamarin.Forms.StackLayout>("Sl_Notes");
            Lst_Comments = this.FindByName<global::Xamarin.Forms.ListView>("Lst_Comments");
            Lbl_Comment = this.FindByName<global::Xamarin.Forms.Label>("Lbl_Comment");
            Sl_Main = this.FindByName<global::Xamarin.Forms.StackLayout>("Sl_Main");
            Tblvw_StandardParagraph = this.FindByName<global::ServiceRequest.Custom.SrTableView>("Tblvw_StandardParagraph");
            TblSec_ParagraphList = this.FindByName<global::Xamarin.Forms.TableSection>("TblSec_ParagraphList");
            Lbl_AddParagraph = this.FindByName<global::Xamarin.Forms.Label>("Lbl_AddParagraph");
            Lbl_Edit = this.FindByName<global::Xamarin.Forms.Label>("Lbl_Edit");
        }
    }
}
