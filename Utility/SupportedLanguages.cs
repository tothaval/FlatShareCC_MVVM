/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  SupportedLanguages 
 * 
 *  enum holds all supported languages,
 *  probably obsolete and better if
 *  this is replaced with a list
 *  of files found in the language folder,
 *  if they can be deserialized into
 *  a LanguageResourceStrings class instance,
 *  the DynamicResource bindings in the Views
 *  will show that data.
 */

namespace SharedLivingCostCalculator.Utility
{
    public enum SupportedLanguages
    {
        English,
        Deutsch
    }
}
// EOF