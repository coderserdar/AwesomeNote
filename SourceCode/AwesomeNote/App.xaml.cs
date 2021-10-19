using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AwesomeNote.Resources;
using System.Collections.Generic;
using Microsoft.Phone.Marketplace;

namespace AwesomeNote
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        private static LicenseInformation _licenseInfo = new LicenseInformation();
        public static bool IsTrial
        {
            get;
            // setting the IsTrial property from outside is not allowed
            private set;
        }

        private void DetermineIsTrail()
        {
#if TRIAL
            // return true if debugging with trial enabled (DebugTrial configuration is active)
            IsTrial = true;
#else
            var license = new Microsoft.Phone.Marketplace.LicenseInformation();
            IsTrial = license.IsTrial();
#endif
        }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            ThemeManager.ToDarkTheme();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            //DetermineIsTrail();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            //DetermineIsTrail();
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            int noteFolderId;
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                if (!context.DatabaseExists())
                {
                    context.CreateDatabase();
                    DilAyariOlustur(context);
                }
                else
                {
                    using (var context2 = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                    {
                        // burada dil ayarlarından ötürü bilgilendirici not klasörlerini siliyoruz
                        var noteFolders = context2.NoteFolders.Where(j => j.NoteFolderDescription.Equals("AwesomeNote")).ToList() as List<NoteFolder>;
                        for (int i = 0; i < noteFolders.Count; i++)
                        {
                            var notes = context2.Notes.Where(j => j.NoteFolderId.Equals(noteFolders[i].NoteFolderId)).ToList() as List<Note>;
                            context2.Notes.DeleteAllOnSubmit(notes);
                            context2.SubmitChanges();
                        }
                        context2.NoteFolders.DeleteAllOnSubmit(noteFolders);
                        context2.SubmitChanges();

                        AppSettings lang =
                            context2.AppSettings.First() as AppSettings;
                        string culture = "";
                        switch (lang.AppLangName)
                        {
                            case "TR":
                                culture = "tr";
                                break;
                            case "EN":
                                culture = "en";
                                break;
                            case "DE":
                                culture = "de";
                                break;
                            case "ES":
                                culture = "es";
                                break;
                            case "FR":
                                culture = "fr";
                                break;
                            case "IT":
                                culture = "it";
                                break;
                            case "AR":
                                culture = "ar";
                                break;
                            case "FA":
                                culture = "fa-IR";
                                break;
                            case "ZH":
                                culture = "zh";
                                break;
                            case "PT":
                                culture = "pt";
                                break;
                            case "RU":
                                culture = "ru";
                                break;
                            case "SA":
                                culture = "sa";
                                break;
                            case "TH":
                                culture = "th";
                                break;
                            default:
                                culture = "tr-TR";
                                break;
                        }
                        CultureInfo newCulture = new CultureInfo(culture);
                        Thread.CurrentThread.CurrentCulture = newCulture;
                        Thread.CurrentThread.CurrentUICulture = newCulture;
                    }
                }

                // kullanıcının programla ilgili bilgilendirici notları kendi dilinde görebilmesi için burada ekliyoruz.
                WelcomeNotKlasoruOlustur(context);
                ProgramHakkindaBilgiKlasoruOlustur(context);
            }
        }

        private static void DilAyariOlustur(NoteFolderDataContext context)
        {
            var appLangSettings = new AppSettings()
            {
                //AppLangId = 42,
                AppLangName = "EN",
                AppBackgroundColor = "BLA",
                FolderOrderBy = "CDATE",
                FolderOrderStyle = "A",
                AppBackgroundImage = null
            };

            context.AppSettings.InsertOnSubmit(appLangSettings);
            context.SubmitChanges();

            CultureInfo newCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
        }

        private static void WelcomeNotKlasoruOlustur(NoteFolderDataContext context)
        {
            int noteFolderId;
            var noteFolder = new NoteFolder()
            {
                NoteFolderName = AppResources.Welcome,
                NoteFolderDescription = "AwesomeNote",
                NoteFolderPassword = "",
                IsPasswordProtected = false,
                FontFamily = "Verdana",
                FontSize = "26",
                NoteOrderBy = "NAME",
                NoteOrderStyle = "A",
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                NoteCount = 1,
                NameCount = AppResources.Welcome + " (1)",
                FolderBackground = null
                //FontWeight = "Normal",
                //FontStyle = "Normal"
            };
            context.NoteFolders.InsertOnSubmit(noteFolder);
            // commit the changes to the database
            context.SubmitChanges();
            noteFolderId = context.NoteFolders.First().NoteFolderId;
            var note = new Note()
            {
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                NoteFolderId = noteFolderId,
                NoteName = AppResources.Welcome,
                NameCreation = AppResources.Welcome + " (" + DateTime.Now.ToString() + ")",
                NoteDescription =
                    AppResources.WelcomeNote,
                NameDescription = AppResources.Welcome + " " + AppResources.WelcomeNote
                //NameDescriptionWithoutNewline = "Note " + j.ToString() + " " + j.ToString() + ". note of folder and " + (i * 4 + j).ToString() + ". note of general"
            };
            // place the object in pending insert state
            context.Notes.InsertOnSubmit(note);
            // commit the changes to the database
            context.SubmitChanges();
            //return noteFolderId;
        }

        private static void ProgramHakkindaBilgiKlasoruOlustur(NoteFolderDataContext context)
        {
            int noteFolderId;
            var noteFolder = new NoteFolder()
            {
                NoteFolderName = AppResources.Tips,
                NoteFolderDescription = "AwesomeNote",
                NoteFolderPassword = "",
                IsPasswordProtected = false,
                FontFamily = "Verdana",
                FontSize = "26",
                NoteOrderBy = "CDATE",
                NoteOrderStyle = "A",
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                NoteCount = 10,
                NameCount = AppResources.Tips + " (10)",
                FolderBackground = null
                //FontWeight = "Normal",
                //FontStyle = "Normal"
            };
            context.NoteFolders.InsertOnSubmit(noteFolder);
            // commit the changes to the database
            context.SubmitChanges();
            var noteFolder2 = context.NoteFolders.Where(j => j.NoteCount.Equals(10)).Single() as NoteFolder;
            noteFolderId = noteFolder2.NoteFolderId;


            var noteFeature = new Note()
            {
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                NoteFolderId = noteFolderId,
                NoteName = AppResources.ProgramFeatures,
                NameCreation = AppResources.ProgramFeatures + " (" + DateTime.Now.ToString() + ")",
                NoteDescription =
                    AppResources.ProgramFeaturesNote,
                NameDescription = AppResources.ProgramFeatures + " " + AppResources.ProgramFeaturesNote
            };
            context.Notes.InsertOnSubmit(noteFeature);

            var notePassword = new Note()
            {
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                NoteFolderId = noteFolderId,
                NoteName = AppResources.PasswordFeature,
                NameCreation = AppResources.PasswordFeature + " (" + DateTime.Now.ToString() + ")",
                NoteDescription =
                    AppResources.PasswordFeatureNote,
                NameDescription = AppResources.PasswordFeature + " " + AppResources.PasswordFeatureNote
            };
            context.Notes.InsertOnSubmit(notePassword);

            var noteLanguage = new Note()
            {
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                NoteFolderId = noteFolderId,
                NoteName = AppResources.LanguageFeature,
                NameCreation = AppResources.LanguageFeature + " (" + DateTime.Now.ToString() + ")",
                NoteDescription =
                    AppResources.LanguageFeatureNote,
                NameDescription = AppResources.LanguageFeature + " " + AppResources.LanguageFeatureNote
            };
            context.Notes.InsertOnSubmit(noteLanguage);

            var noteBackground = new Note()
            {
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                NoteFolderId = noteFolderId,
                NoteName = AppResources.BackgroundFeature,
                NameCreation = AppResources.BackgroundFeature + " (" + DateTime.Now.ToString() + ")",
                NoteDescription =
                    AppResources.BackgroundFeatureNote,
                NameDescription = AppResources.BackgroundFeature + " " + AppResources.BackgroundFeatureNote
            };
            context.Notes.InsertOnSubmit(noteBackground);

            var noteSearch = new Note()
            {
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                NoteFolderId = noteFolderId,
                NoteName = AppResources.SearchFeature,
                NameCreation = AppResources.SearchFeature + " (" + DateTime.Now.ToString() + ")",
                NoteDescription =
                    AppResources.SearchFeatureNote,
                NameDescription = AppResources.SearchFeature + " " + AppResources.SearchFeatureNote
            };
            context.Notes.InsertOnSubmit(noteSearch);

            var noteSync = new Note()
            {
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                NoteFolderId = noteFolderId,
                NoteName = AppResources.SyncFeature,
                NameCreation = AppResources.SyncFeature + " (" + DateTime.Now.ToString() + ")",
                NoteDescription =
                    AppResources.SyncFeatureNote,
                NameDescription = AppResources.SyncFeature + " " + AppResources.SyncFeatureNote
            };
            context.Notes.InsertOnSubmit(noteSync);

            var noteFont = new Note()
            {
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                NoteFolderId = noteFolderId,
                NoteName = AppResources.FontFeature,
                NameCreation = AppResources.FontFeature + " (" + DateTime.Now.ToString() + ")",
                NoteDescription =
                    AppResources.FontFeatureNote,
                NameDescription = AppResources.FontFeature + " " + AppResources.FontFeatureNote
            };
            context.Notes.InsertOnSubmit(noteFont);

            var noteOrder = new Note()
            {
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                NoteFolderId = noteFolderId,
                NoteName = AppResources.OrderFeature,
                NameCreation = AppResources.OrderFeature + " (" + DateTime.Now.ToString() + ")",
                NoteDescription =
                    AppResources.OrderFeatureNote,
                NameDescription = AppResources.OrderFeature + " " + AppResources.OrderFeatureNote
            };
            context.Notes.InsertOnSubmit(noteOrder);

            var noteNote = new Note()
            {
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                NoteFolderId = noteFolderId,
                NoteName = AppResources.NoteFeature,
                NameCreation = AppResources.NoteFeature + " (" + DateTime.Now.ToString() + ")",
                NoteDescription =
                    AppResources.NoteFeatureNote,
                NameDescription = AppResources.NoteFeature + " " + AppResources.NoteFeatureNote
            };
            context.Notes.InsertOnSubmit(noteNote);

            var noteNoteFolder = new Note()
            {
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                NoteFolderId = noteFolderId,
                NoteName = AppResources.NoteFolderFeature,
                NameCreation = AppResources.NoteFolderFeature + " (" + DateTime.Now.ToString() + ")",
                NoteDescription =
                    AppResources.NoteFeatureNote,
                NameDescription = AppResources.NoteFolderFeature + " " + AppResources.NoteFolderFeatureNote
            };
            context.Notes.InsertOnSubmit(noteNoteFolder);
            context.SubmitChanges();
        }

    }
}