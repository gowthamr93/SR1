//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceRequest.Views {
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    
    
    public partial class ContactDetailsView : global::Xamarin.Forms.ContentView {
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.ResourceDictionary AppDictionary;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Grid GL_Title;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::ServiceRequest.Custom.SrPicker Pkr_CustomerType;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Entry Lbl_Name;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::ServiceRequest.Custom.SrEditor EdAddress;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Entry Lbl_Phone;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Entry Lbl_Moblie;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Entry Lbl_Email;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent() {
            this.LoadFromXaml(typeof(ContactDetailsView));
            AppDictionary = this.FindByName<global::Xamarin.Forms.ResourceDictionary>("AppDictionary");
            GL_Title = this.FindByName<global::Xamarin.Forms.Grid>("GL_Title");
            Pkr_CustomerType = this.FindByName<global::ServiceRequest.Custom.SrPicker>("Pkr_CustomerType");
            Lbl_Name = this.FindByName<global::Xamarin.Forms.Entry>("Lbl_Name");
            EdAddress = this.FindByName<global::ServiceRequest.Custom.SrEditor>("EdAddress");
            Lbl_Phone = this.FindByName<global::Xamarin.Forms.Entry>("Lbl_Phone");
            Lbl_Moblie = this.FindByName<global::Xamarin.Forms.Entry>("Lbl_Moblie");
            Lbl_Email = this.FindByName<global::Xamarin.Forms.Entry>("Lbl_Email");
        }
    }
}
