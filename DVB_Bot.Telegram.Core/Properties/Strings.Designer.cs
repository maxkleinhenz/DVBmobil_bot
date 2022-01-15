﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DVB_Bot.Telegram.Core.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DVB_Bot.Telegram.Core.Properties.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Du kannst nicht mehr Favoriten speichern.
        /// </summary>
        internal static string FavoriteStopsCommand_MaxFavorites {
            get {
                return ResourceManager.GetString("FavoriteStopsCommand_MaxFavorites", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Favoritenliste aktualisiert.
        /// </summary>
        internal static string FavoriteStopsCommand_RefreshFavorties {
            get {
                return ResourceManager.GetString("FavoriteStopsCommand_RefreshFavorties", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Haltestelle von den Favoriten entfernt.
        /// </summary>
        internal static string FavoriteStopsCommand_RemovedFromFavorites {
            get {
                return ResourceManager.GetString("FavoriteStopsCommand_RemovedFromFavorites", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Haltestelle nicht gefunden.
        /// </summary>
        internal static string FavoriteStopsCommand_StopCouldNotFound {
            get {
                return ResourceManager.GetString("FavoriteStopsCommand_StopCouldNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Haltestelle zu deinen Favoriten hinzugefügt.
        /// </summary>
        internal static string FavoriteStopsCommand_StopHasBeenAddedToFavorites {
            get {
                return ResourceManager.GetString("FavoriteStopsCommand_StopHasBeenAddedToFavorites", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Die Haltestelle ist bereits in deiner Favoritenliste.
        /// </summary>
        internal static string FavoriteStopsCommand_StopIsAlreadyFavorites {
            get {
                return ResourceManager.GetString("FavoriteStopsCommand_StopIsAlreadyFavorites", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Die Haltestelle ist nicht in deiner Favoritenliste.
        /// </summary>
        internal static string FavoriteStopsCommand_StopIsNotFavorite {
            get {
                return ResourceManager.GetString("FavoriteStopsCommand_StopIsNotFavorite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Der Befehl ist ungültig.
        /// </summary>
        internal static string Program_InvalidCommand {
            get {
                return ResourceManager.GetString("Program_InvalidCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bitte gebe auch das Haltestellenkürzel an
        ///Zum Beispiel: {0} HBF.
        /// </summary>
        internal static string Program_SpecifyStop {
            get {
                return ResourceManager.GetString("Program_SpecifyStop", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Das kann ich:
        ///\- Schreibe die Haltestelle oder ihr Kürzel in den Chat und ich sende dir die Abfahrten
        ///\- Schreibe /add und das Haltestellenkürzel, um die Haltestelle in deine Favoritenliste aufzunehmen
        ///\- Schreibe /remove und das Haltestellenkürzel, um die Haltestelle aus deinen Favoriten zu entfernen
        ///\- Schreibe /favs, um deine Favoritenliste in der App wieder anzuzeigen
        ///
        ///Für Fragen, Anregungen und Kritik kann mir eine E\-Mail geschrieben werden: dvbmobil@outlook\.de
        ///
        ///Der Code des Bots ist Open\-So [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Programm_HelpMessage {
            get {
                return ResourceManager.GetString("Programm_HelpMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hallo,
        ///bei mir kannst du dich über die Abfahrten an den Haltestellen der DVB in Dresden und Umgebung informieren.
        /// </summary>
        internal static string Programm_StartMessage {
            get {
                return ResourceManager.GetString("Programm_StartMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Als Favorit hinzufügen.
        /// </summary>
        internal static string ShowDeparturesCommand_AddToFavorites {
            get {
                return ResourceManager.GetString("ShowDeparturesCommand_AddToFavorites", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Keine Abfahrten verfügbar.
        /// </summary>
        internal static string ShowDeparturesCommand_CouldNotFoundDepartures {
            get {
                return ResourceManager.GetString("ShowDeparturesCommand_CouldNotFoundDepartures", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Haltestelle nicht gefunden.
        /// </summary>
        internal static string ShowDeparturesCommand_CouldNotFoundStop {
            get {
                return ResourceManager.GetString("ShowDeparturesCommand_CouldNotFoundStop", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Meintest du?.
        /// </summary>
        internal static string ShowDeparturesCommand_DidYouMean {
            get {
                return ResourceManager.GetString("ShowDeparturesCommand_DidYouMean", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bei der Abfrage trat ein Fehler auf :(.
        /// </summary>
        internal static string ShowDeparturesCommand_Error {
            get {
                return ResourceManager.GetString("ShowDeparturesCommand_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mehr anzeigen.
        /// </summary>
        internal static string ShowDeparturesCommand_LoadMoreDepartures {
            get {
                return ResourceManager.GetString("ShowDeparturesCommand_LoadMoreDepartures", resourceCulture);
            }
        }
    }
}
