﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhysicalPersonDirectory.Application.Resources {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources_en_US {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources_en_US() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("PhysicalPersonDirectory.Application.Resources.Resources_en_US", typeof(Resources_en_US).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string CityNotFoundException {
            get {
                return ResourceManager.GetString("CityNotFoundException", resourceCulture);
            }
        }
        
        internal static string CreatePhysicalPersonValidator_FirstName {
            get {
                return ResourceManager.GetString("CreatePhysicalPersonValidator_FirstName", resourceCulture);
            }
        }
        
        internal static string CreatePhysicalPersonValidator_FirstName_Regex {
            get {
                return ResourceManager.GetString("CreatePhysicalPersonValidator_FirstName_Regex", resourceCulture);
            }
        }
        
        internal static string CreatePhysicalPersonValidator_IdentificationNumber {
            get {
                return ResourceManager.GetString("CreatePhysicalPersonValidator_IdentificationNumber", resourceCulture);
            }
        }
        
        internal static string ImageCannotBeUploadedException {
            get {
                return ResourceManager.GetString("ImageCannotBeUploadedException", resourceCulture);
            }
        }
        
        internal static string PhoneNumberValidator_PhoneNumber {
            get {
                return ResourceManager.GetString("PhoneNumberValidator_PhoneNumber", resourceCulture);
            }
        }
        
        internal static string PhysicalPersonNotFoundException {
            get {
                return ResourceManager.GetString("PhysicalPersonNotFoundException", resourceCulture);
            }
        }
        
        internal static string RelatedPersonAlreadyExistsException {
            get {
                return ResourceManager.GetString("RelatedPersonAlreadyExistsException", resourceCulture);
            }
        }
        
        internal static string UpdatePhysicalPersonValidator_FirstName {
            get {
                return ResourceManager.GetString("UpdatePhysicalPersonValidator_FirstName", resourceCulture);
            }
        }
        
        internal static string RelatedPersonNotFoundException {
            get {
                return ResourceManager.GetString("RelatedPersonNotFoundException", resourceCulture);
            }
        }
        
        internal static string UpdatePhysicalPersonValidator_IdentificationNumber {
            get {
                return ResourceManager.GetString("UpdatePhysicalPersonValidator_IdentificationNumber", resourceCulture);
            }
        }
        
        internal static string UpdatePhysicalPersonValidator_FirstName_Regex {
            get {
                return ResourceManager.GetString("UpdatePhysicalPersonValidator_FirstName_Regex", resourceCulture);
            }
        }
    }
}
