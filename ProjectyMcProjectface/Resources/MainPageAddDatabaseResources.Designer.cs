﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class MainPageAddDatabaseResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MainPageAddDatabaseResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.MainPageAddDatabaseResources", typeof(MainPageAddDatabaseResources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Test Connection.
        /// </summary>
        public static string button_TestConnection {
            get {
                return ResourceManager.GetString("button_TestConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have already registered this Database.
        /// </summary>
        public static string ErrorConnStringAlreadyRegistered {
            get {
                return ResourceManager.GetString("ErrorConnStringAlreadyRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Couldn&apos;t establish valid connection, check the connection string!.
        /// </summary>
        public static string ErrorConnStringInvalid {
            get {
                return ResourceManager.GetString("ErrorConnStringInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connection String.
        /// </summary>
        public static string label_ConString {
            get {
                return ResourceManager.GetString("label_ConString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database Name.
        /// </summary>
        public static string label_Databasename {
            get {
                return ResourceManager.GetString("label_Databasename", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database was successfully added! Now you can view it&apos;s content! Press the following button to refresh the page..
        /// </summary>
        public static string RegistrationWasASuccess {
            get {
                return ResourceManager.GetString("RegistrationWasASuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connection string test was a success! Now you can add it!.
        /// </summary>
        public static string TestSuccess {
            get {
                return ResourceManager.GetString("TestSuccess", resourceCulture);
            }
        }
    }
}
