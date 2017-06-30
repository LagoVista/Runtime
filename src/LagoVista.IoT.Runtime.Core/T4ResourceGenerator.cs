using System.Globalization;
using System.Reflection;

//Resources:RuntimeCoreResources:Common_Description
namespace LagoVista.IoT.Runtime.Core.Resources
{
	public class RuntimeCoreResources
	{
        private static global::System.Resources.ResourceManager _resourceManager;
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private static global::System.Resources.ResourceManager ResourceManager 
		{
            get 
			{
                if (object.ReferenceEquals(_resourceManager, null)) 
				{
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LagoVista.IoT.Runtime.Core.Resources.RuntimeCoreResources", typeof(RuntimeCoreResources).GetTypeInfo().Assembly);
                    _resourceManager = temp;
                }
                return _resourceManager;
            }
        }
        
        /// <summary>
        ///   Returns the formatted resource string.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private static string GetResourceString(string key, params string[] tokens)
		{
			var culture = CultureInfo.CurrentCulture;;
            var str = ResourceManager.GetString(key, culture);

			for(int i = 0; i < tokens.Length; i += 2)
				str = str.Replace(tokens[i], tokens[i+1]);
										
            return str;
        }
        
        /// <summary>
        ///   Returns the formatted resource string.
        /// </summary>
		/*
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private static HtmlString GetResourceHtmlString(string key, params string[] tokens)
		{
			var str = GetResourceString(key, tokens);
							
			if(str.StartsWith("HTML:"))
				str = str.Substring(5);

			return new HtmlString(str);
        }*/
		
		public static string Common_Description { get { return GetResourceString("Common_Description"); } }
//Resources:RuntimeCoreResources:Common_IsPublic

		public static string Common_IsPublic { get { return GetResourceString("Common_IsPublic"); } }
//Resources:RuntimeCoreResources:Common_Key

		public static string Common_Key { get { return GetResourceString("Common_Key"); } }
//Resources:RuntimeCoreResources:Common_Key_Help

		public static string Common_Key_Help { get { return GetResourceString("Common_Key_Help"); } }
//Resources:RuntimeCoreResources:Common_Key_Validation

		public static string Common_Key_Validation { get { return GetResourceString("Common_Key_Validation"); } }
//Resources:RuntimeCoreResources:Common_Name

		public static string Common_Name { get { return GetResourceString("Common_Name"); } }
//Resources:RuntimeCoreResources:Status

		public static string Status { get { return GetResourceString("Status"); } }
//Resources:RuntimeCoreResources:Verifier_Aborted


		///<summary>
		///Will append number of iterations completed to messages
		///</summary>
		public static string Verifier_Aborted { get { return GetResourceString("Verifier_Aborted"); } }
//Resources:RuntimeCoreResources:Verifier_Actual

		public static string Verifier_Actual { get { return GetResourceString("Verifier_Actual"); } }
//Resources:RuntimeCoreResources:Verifier_Component

		public static string Verifier_Component { get { return GetResourceString("Verifier_Component"); } }
//Resources:RuntimeCoreResources:Verifier_Description

		public static string Verifier_Description { get { return GetResourceString("Verifier_Description"); } }
//Resources:RuntimeCoreResources:Verifier_Empty

		public static string Verifier_Empty { get { return GetResourceString("Verifier_Empty"); } }
//Resources:RuntimeCoreResources:Verifier_Expected

		public static string Verifier_Expected { get { return GetResourceString("Verifier_Expected"); } }
//Resources:RuntimeCoreResources:Verifier_Expected_NotMatch_Actual

		public static string Verifier_Expected_NotMatch_Actual { get { return GetResourceString("Verifier_Expected_NotMatch_Actual"); } }
//Resources:RuntimeCoreResources:Verifier_ExpectedOutput

		public static string Verifier_ExpectedOutput { get { return GetResourceString("Verifier_ExpectedOutput"); } }
//Resources:RuntimeCoreResources:Verifier_Header

		public static string Verifier_Header { get { return GetResourceString("Verifier_Header"); } }
//Resources:RuntimeCoreResources:Verifier_Header_Help

		public static string Verifier_Header_Help { get { return GetResourceString("Verifier_Header_Help"); } }
//Resources:RuntimeCoreResources:Verifier_Help

		public static string Verifier_Help { get { return GetResourceString("Verifier_Help"); } }
//Resources:RuntimeCoreResources:Verifier_InputType

		public static string Verifier_InputType { get { return GetResourceString("Verifier_InputType"); } }
//Resources:RuntimeCoreResources:Verifier_InputType_Binary

		public static string Verifier_InputType_Binary { get { return GetResourceString("Verifier_InputType_Binary"); } }
//Resources:RuntimeCoreResources:Verifier_InputType_Help

		public static string Verifier_InputType_Help { get { return GetResourceString("Verifier_InputType_Help"); } }
//Resources:RuntimeCoreResources:Verifier_InputType_Select

		public static string Verifier_InputType_Select { get { return GetResourceString("Verifier_InputType_Select"); } }
//Resources:RuntimeCoreResources:Verifier_InputType_Text

		public static string Verifier_InputType_Text { get { return GetResourceString("Verifier_InputType_Text"); } }
//Resources:RuntimeCoreResources:Verifier_IterationCountZero

		public static string Verifier_IterationCountZero { get { return GetResourceString("Verifier_IterationCountZero"); } }
//Resources:RuntimeCoreResources:Verifier_MissingExpectedOutput

		public static string Verifier_MissingExpectedOutput { get { return GetResourceString("Verifier_MissingExpectedOutput"); } }
//Resources:RuntimeCoreResources:Verifier_MissingExpectedOutputs

		public static string Verifier_MissingExpectedOutputs { get { return GetResourceString("Verifier_MissingExpectedOutputs"); } }
//Resources:RuntimeCoreResources:Verifier_MissingInput

		public static string Verifier_MissingInput { get { return GetResourceString("Verifier_MissingInput"); } }
//Resources:RuntimeCoreResources:Verifier_MissingInputType

		public static string Verifier_MissingInputType { get { return GetResourceString("Verifier_MissingInputType"); } }
//Resources:RuntimeCoreResources:Verifier_ParserFailed

		public static string Verifier_ParserFailed { get { return GetResourceString("Verifier_ParserFailed"); } }
//Resources:RuntimeCoreResources:Verifier_PathAndQueryString

		public static string Verifier_PathAndQueryString { get { return GetResourceString("Verifier_PathAndQueryString"); } }
//Resources:RuntimeCoreResources:Verifier_PathAndQueryString_Help

		public static string Verifier_PathAndQueryString_Help { get { return GetResourceString("Verifier_PathAndQueryString_Help"); } }
//Resources:RuntimeCoreResources:Verifier_Payload

		public static string Verifier_Payload { get { return GetResourceString("Verifier_Payload"); } }
//Resources:RuntimeCoreResources:Verifier_ResultDoesNotContainKey


		///<summary>
		///Will append some additional text to message
		///</summary>
		public static string Verifier_ResultDoesNotContainKey { get { return GetResourceString("Verifier_ResultDoesNotContainKey"); } }
//Resources:RuntimeCoreResources:Verifier_ShouldSucceed

		public static string Verifier_ShouldSucceed { get { return GetResourceString("Verifier_ShouldSucceed"); } }
//Resources:RuntimeCoreResources:Verifier_ShouldSucceed_Help

		public static string Verifier_ShouldSucceed_Help { get { return GetResourceString("Verifier_ShouldSucceed_Help"); } }
//Resources:RuntimeCoreResources:Verifier_Title

		public static string Verifier_Title { get { return GetResourceString("Verifier_Title"); } }
//Resources:RuntimeCoreResources:Verifier_Topic

		public static string Verifier_Topic { get { return GetResourceString("Verifier_Topic"); } }
//Resources:RuntimeCoreResources:Verifier_VerifierType

		public static string Verifier_VerifierType { get { return GetResourceString("Verifier_VerifierType"); } }
//Resources:RuntimeCoreResources:Verifier_VerifierType_MessageFieldParser

		public static string Verifier_VerifierType_MessageFieldParser { get { return GetResourceString("Verifier_VerifierType_MessageFieldParser"); } }
//Resources:RuntimeCoreResources:Verifier_VerifierType_MessageParser

		public static string Verifier_VerifierType_MessageParser { get { return GetResourceString("Verifier_VerifierType_MessageParser"); } }
//Resources:RuntimeCoreResources:Verifier_VerifierType_Planner

		public static string Verifier_VerifierType_Planner { get { return GetResourceString("Verifier_VerifierType_Planner"); } }
//Resources:RuntimeCoreResources:VerifierType_NotSpecified

		public static string VerifierType_NotSpecified { get { return GetResourceString("VerifierType_NotSpecified"); } }

		public static class Names
		{
			public const string Common_Description = "Common_Description";
			public const string Common_IsPublic = "Common_IsPublic";
			public const string Common_Key = "Common_Key";
			public const string Common_Key_Help = "Common_Key_Help";
			public const string Common_Key_Validation = "Common_Key_Validation";
			public const string Common_Name = "Common_Name";
			public const string Status = "Status";
			public const string Verifier_Aborted = "Verifier_Aborted";
			public const string Verifier_Actual = "Verifier_Actual";
			public const string Verifier_Component = "Verifier_Component";
			public const string Verifier_Description = "Verifier_Description";
			public const string Verifier_Empty = "Verifier_Empty";
			public const string Verifier_Expected = "Verifier_Expected";
			public const string Verifier_Expected_NotMatch_Actual = "Verifier_Expected_NotMatch_Actual";
			public const string Verifier_ExpectedOutput = "Verifier_ExpectedOutput";
			public const string Verifier_Header = "Verifier_Header";
			public const string Verifier_Header_Help = "Verifier_Header_Help";
			public const string Verifier_Help = "Verifier_Help";
			public const string Verifier_InputType = "Verifier_InputType";
			public const string Verifier_InputType_Binary = "Verifier_InputType_Binary";
			public const string Verifier_InputType_Help = "Verifier_InputType_Help";
			public const string Verifier_InputType_Select = "Verifier_InputType_Select";
			public const string Verifier_InputType_Text = "Verifier_InputType_Text";
			public const string Verifier_IterationCountZero = "Verifier_IterationCountZero";
			public const string Verifier_MissingExpectedOutput = "Verifier_MissingExpectedOutput";
			public const string Verifier_MissingExpectedOutputs = "Verifier_MissingExpectedOutputs";
			public const string Verifier_MissingInput = "Verifier_MissingInput";
			public const string Verifier_MissingInputType = "Verifier_MissingInputType";
			public const string Verifier_ParserFailed = "Verifier_ParserFailed";
			public const string Verifier_PathAndQueryString = "Verifier_PathAndQueryString";
			public const string Verifier_PathAndQueryString_Help = "Verifier_PathAndQueryString_Help";
			public const string Verifier_Payload = "Verifier_Payload";
			public const string Verifier_ResultDoesNotContainKey = "Verifier_ResultDoesNotContainKey";
			public const string Verifier_ShouldSucceed = "Verifier_ShouldSucceed";
			public const string Verifier_ShouldSucceed_Help = "Verifier_ShouldSucceed_Help";
			public const string Verifier_Title = "Verifier_Title";
			public const string Verifier_Topic = "Verifier_Topic";
			public const string Verifier_VerifierType = "Verifier_VerifierType";
			public const string Verifier_VerifierType_MessageFieldParser = "Verifier_VerifierType_MessageFieldParser";
			public const string Verifier_VerifierType_MessageParser = "Verifier_VerifierType_MessageParser";
			public const string Verifier_VerifierType_Planner = "Verifier_VerifierType_Planner";
			public const string VerifierType_NotSpecified = "VerifierType_NotSpecified";
		}
	}
}
