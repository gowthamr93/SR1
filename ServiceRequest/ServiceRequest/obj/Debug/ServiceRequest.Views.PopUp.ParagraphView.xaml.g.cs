//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceRequest.Views.PopUp {
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    
    
    public partial class ParagraphView : global::Xamarin.Forms.ContentView {
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Grid GL_Title;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Label Lbl_Cancel;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Label Lbl_Title;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Label Lbl_Save;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.ListView Lstvw_TypedParagraphs;
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent() {
            this.LoadFromXaml(typeof(ParagraphView));
            GL_Title = this.FindByName<global::Xamarin.Forms.Grid>("GL_Title");
            Lbl_Cancel = this.FindByName<global::Xamarin.Forms.Label>("Lbl_Cancel");
            Lbl_Title = this.FindByName<global::Xamarin.Forms.Label>("Lbl_Title");
            Lbl_Save = this.FindByName<global::Xamarin.Forms.Label>("Lbl_Save");
            Lstvw_TypedParagraphs = this.FindByName<global::Xamarin.Forms.ListView>("Lstvw_TypedParagraphs");
        }
    }
}
