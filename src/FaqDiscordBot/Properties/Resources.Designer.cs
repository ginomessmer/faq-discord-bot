//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FaqDiscordBot.Properties {
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
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FaqDiscordBot.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        public static byte[] quickstart {
            get {
                object obj = ResourceManager.GetObject("quickstart", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://raw.githubusercontent.com/tinf-network/faqs/main/assets/quickstart.gif.
        /// </summary>
        public static string UnansweredQuestionInstructionsImage_Url {
            get {
                return ResourceManager.GetString("UnansweredQuestionInstructionsImage_Url", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Post your question on the TINF Network Server and reply to your message here as soon as you have an answer so that we can add it to the knowledge base.
        ///
        ///Not sure? Reply with `tutorial` to be guided..
        /// </summary>
        public static string UnansweredQuestionInstructionsText_Desc_Pro {
            get {
                return ResourceManager.GetString("UnansweredQuestionInstructionsText_Desc_Pro", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Help add your question to the knowledge base.
        ///
        ///**Here&apos;s how it works**:
        ///**1.** Post your question on the TINF Network Server (e.g. in the #dhbw-overflow channel).
        ///**2.** Once you have received an answer, reply here to your original message (see GIF) as detailed as possible.
        ///**3.** Your response will be reviewed and then added to the knowledge base so that it is available to other students.
        ///
        ///Contact the Orga team if you have trouble doing this. Your participation will help other and future students..
        /// </summary>
        public static string UnansweredQuestionInstructionsText_Desc_Rookie {
            get {
                return ResourceManager.GetString("UnansweredQuestionInstructionsText_Desc_Rookie", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can contribute your answer within {0}..
        /// </summary>
        public static string UnansweredQuestionInstructionsText_Footnote {
            get {
                return ResourceManager.GetString("UnansweredQuestionInstructionsText_Footnote", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I could not find an answer.
        /// </summary>
        public static string UnansweredQuestionInstructionsText_Title_Pro {
            get {
                return ResourceManager.GetString("UnansweredQuestionInstructionsText_Title_Pro", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I could not find an answer. However, you can still find what you are looking for:.
        /// </summary>
        public static string UnansweredQuestionInstructionsText_Title_Rookie {
            get {
                return ResourceManager.GetString("UnansweredQuestionInstructionsText_Title_Rookie", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If so, it would be great if you could add your answer to the knowledge base. Reply with `tutorial` if you need help..
        /// </summary>
        public static string UnansweredQuestionReminderEmbed_Desc {
            get {
                return ResourceManager.GetString("UnansweredQuestionReminderEmbed_Desc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hey! Have you already received an answer to your question?.
        /// </summary>
        public static string UnansweredQuestionReminderEmbed_Title {
            get {
                return ResourceManager.GetString("UnansweredQuestionReminderEmbed_Title", resourceCulture);
            }
        }
    }
}
