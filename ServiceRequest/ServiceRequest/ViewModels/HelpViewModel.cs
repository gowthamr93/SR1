using System.Collections.Generic;
using System.Collections.ObjectModel;
using ServiceRequest.Models;
using ServiceRequest.AppContext;
using System;

namespace ServiceRequest.ViewModels
{
    public class HelpViewModel
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Properties
        ///--------------------------------------------------------------------------------------------------
        ///

        public static IList<HelpContent> All { get; private set; }

        #endregion
        ///--------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region static Constructor
        ///--------------------------------------------------------------------------------------------------
        ///

        static HelpViewModel()
        {
            try
            {
                All = new ObservableCollection<HelpContent>
            {
               new HelpContent()
         {
             Title = "Welcome to Idox On-Site",
             Text = "Welcome to the Idox On-Site SR App for officers completing Service Requests.\nSwipe this screen to get some useful tips on how to use this app, or if you already know what you’re doing click ‘done’ to skip this and get started, you can get back to this screen at any time by clicking on help in the left hand menu.",
             ImagePath = "help1.png",
             PageIndicator="1/11"
         },
        new HelpContent()
        {
             Title = "Offline Working",
             Text = "With this app you do not have to rely on always having an internet connection, just download the cases and documents you need before you start and while you have access to the internet, and then you can access everything where ever you are.\nJust remember to sync all your changes when you have a connection to keep your Uniform system updated.",
             ImagePath = "help2.png",
             PageIndicator="2/11"
        },
        new HelpContent()
        {
             Title = "Getting your Cases",
             Text = "To download your cases automatically click the ‘Action’ button then ‘Sync’. This will download all the cases that you have allocated. This feature only functions if you have Enterprise enabled in Uniform.\nIf you want to search for a case manually (perhaps it is allocated to a colleague) just click ‘Action’ and then ‘Search’ where you will be prompted to search via the case id.",
             ImagePath = "help3.png",
             PageIndicator="3/11"
        },
        new HelpContent()
        {
             Title = "Viewing a Case",
             Text = "To view a case just click on the name in the list on the left of the screen. You can then see everything you need to know, from the property notes and history, to contact details and an overview of commercial premise ratings.",
             ImagePath = "help4.png",
             PageIndicator="4/11"
        },
        new HelpContent()
        {
             Title = "Download Documents",
             Text = "You can view and download all the documents associated with the case providing they are stored in your Idox DMS.\nOnce you have a case open you can see a list of all the documents and have the option to download as many as you want for use offline.",
             ImagePath = "help5.png",
             PageIndicator="5/11"
        },
        new HelpContent()
        {
             Title = "Visits",
             Text = "After having clicked on the Service Request you can see more information specific to that service request such as the visits and customer information for that case.\nTo complete a visit click on the visit you want to work on, fill in all the details and click save in the top right.\nYou can partly fill in the details and come back to it. Just click ‘Save’ on the top right hand corner and the app will remember what you have done for later.",
             ImagePath = "help6.png",
             PageIndicator="6/11"
        },
        new HelpContent()
        {
             Title = "Adding Actions",
             Text = "In the add action screen you can add all the information you need to build an action such as a letter.\nYou can add custom and pre made paragraphs from Uniform and even drag and drop them into the correct order for when Uniform creates the document.",
             ImagePath = "help7.png",
             PageIndicator="7/11"
        },
        new HelpContent()
        {
             Title = "Creating a new Visit",
             Text = "You can add a new visit to the inspection by clicking on the ‘+ Add Visit’ button next to the visits. This could be for now or if you want to book one for the future.\nDid you know that you can also delete a new visit by swiping on the new entry and clicking delete. Note - you cannot delete a visit after syncing with the back office.",
             ImagePath = "help8.png",
             PageIndicator="8/11"
        },
        new HelpContent()
        {
             Title = "Adding photos or audio",
             Text = "You can also take photos with the inbuilt camera and have them automatically upload to the case on your Idox DMS.\nFor photos click the ‘+’ icon followed by the ‘Add New Image’ option which will allow you to attach photos in seconds. Alternatively click ‘Add Audio’ to record and attach a recording.",
             ImagePath = "help9.png",
             PageIndicator="9/11"
        },
        new HelpContent()
        {
             Title = "Update the office",
             Text = "Don’t forget to update the back office with all of your changes.\nWhen you have made changes to an inspection you will notice the upload icon. Note - this means you need to sync with the office. \nTo sync just click the ‘Action’ button then ‘Sync’ in the same way as you do to download your cases.",
             ImagePath = "help3.png",
             PageIndicator="10/11"
        },
        new HelpContent()
        {
             Title = "Get Started!",
             Text = "Thank you for your time.\nClick done and get started with Idox Onsite Service Requests.\nYou can get back to this guide at any time by clicking ‘Help’ from the left menu.",
             ImagePath = "help11.jpg",
             PageIndicator="11/11"
        }
            };
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
