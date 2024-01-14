using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Logging.Exceptions;
using LagoVista.IoT.Runtime.Core.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models.Verifiers
{
    public enum VerifierTypes
    {
        [EnumLabel(Verifier.VerifierType_NotSpecified, RuntimeCoreResources.Names.VerifierType_NotSpecified, typeof(RuntimeCoreResources))]
        NotSpecified,
        [EnumLabel(Verifier.VerifierType_MessageFieldParser, RuntimeCoreResources.Names.Verifier_VerifierType_MessageFieldParser, typeof(RuntimeCoreResources))]
        MessageFieldParser,
        [EnumLabel(Verifier.VerifierType_MessageParser, RuntimeCoreResources.Names.Verifier_VerifierType_MessageParser, typeof(RuntimeCoreResources))]
        MessageParser,
        [EnumLabel(Verifier.VerifierType_Planner, RuntimeCoreResources.Names.Verifier_VerifierType_Planner, typeof(RuntimeCoreResources))]
        Planner
    }

    public enum InputTypes
    {
        [EnumLabel(Verifier.InputType_Binary, RuntimeCoreResources.Names.Verifier_InputType_Binary, typeof(RuntimeCoreResources))]
        Binary,
        [EnumLabel(Verifier.InputType_Text, RuntimeCoreResources.Names.Verifier_InputType_Text, typeof(RuntimeCoreResources))]
        Text
    }

    [EntityDescription(VerifierDomain.Verifiers, RuntimeCoreResources.Names.Verifier_Title, RuntimeCoreResources.Names.Verifier_Help,
        RuntimeCoreResources.Names.Verifier_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(RuntimeCoreResources), Icon: "icon-ae-coding-proaction",
        GetUrl: "/api/verifier/{id}", SaveUrl: "/api/verifier", DeleteUrl: "/api/verifier/{id}", FactoryUrl: "/api/verifier/factory/{type}")]
    public class Verifier : IoTModelBase, IVerifier, IFormDescriptor, IFormConditionalFields, IFormAdditionalActions
    {
        public const string InputType_Binary = "binary";
        public const string InputType_Text = "text";

        public const string VerifierType_NotSpecified = "notspecified";
        public const string VerifierType_MessageFieldParser = "messagefieldparser";
        public const string VerifierType_MessageParser = "message";
        public const string VerifierType_Planner = "planner";


        public Verifier()
        {
            Headers = new ObservableCollection<Header>();
            ExpectedOutputs = new ObservableCollection<ExpectedValue>();
            VerifierType = EntityHeader<VerifierTypes>.Create(VerifierTypes.NotSpecified);
        }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_InputType, EnumType: (typeof(InputTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(RuntimeCoreResources), WaterMark: RuntimeCoreResources.Names.Verifier_InputType_Select, HelpResource: RuntimeCoreResources.Names.Verifier_InputType_Help, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<InputTypes> InputType { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_VerifierType, EnumType: (typeof(VerifierTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(RuntimeCoreResources), IsRequired: true, IsUserEditable: false)]
        public EntityHeader<VerifierTypes> VerifierType { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_ShouldSucceed, HelpResource: RuntimeCoreResources.Names.Verifier_ShouldSucceed_Help, ResourceType: typeof(RuntimeCoreResources), FieldType: FieldTypes.CheckBox)]
        public bool ShouldSucceed { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_Header, HelpResource: RuntimeCoreResources.Names.Verifier_Header_Help, FactoryUrl: "/api/devicemessagetype/header/factory", ResourceType: typeof(RuntimeCoreResources), FieldType: FieldTypes.ChildListInline)]
        public ObservableCollection<Header> Headers { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_PathAndQueryString, FieldType: FieldTypes.Text, HelpResource: RuntimeCoreResources.Names.Verifier_PathAndQueryString_Help, ResourceType: typeof(RuntimeCoreResources))]
        public String PathAndQueryString { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_Topic, FieldType: FieldTypes.Text, ResourceType: typeof(RuntimeCoreResources))]
        public String Topic { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_Component, FieldType: FieldTypes.EntityHeaderPicker, IsRequired: true, ResourceType: typeof(RuntimeCoreResources))]
        public EntityHeader Component { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_PopulateFromSampleMessage, FieldType: FieldTypes.Action, ResourceType: typeof(RuntimeCoreResources))]
        public bool PopulateFromSampleMessageAction { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_Payload, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(RuntimeCoreResources))]
        public string Input { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_ExpectedOutput, FieldType: FieldTypes.Text, ResourceType: typeof(RuntimeCoreResources))]
        public string ExpectedOutput { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_ExpectedOutput, FieldType: FieldTypes.ChildList, ChildListDisplayMember: nameof(ExpectedValue.Key), FactoryUrl: "/api/verifier/expectedoutput/factory", ResourceType: typeof(RuntimeCoreResources))]
        public ObservableCollection<ExpectedValue> ExpectedOutputs { get; set; }

        [CustomValidator]
        public void Validate(ValidationResult result, Actions action)
        {
            switch (VerifierType.Value)
            {
                case VerifierTypes.Planner:

                    break;
                case VerifierTypes.NotSpecified:
                    result.Errors.Add(new ErrorMessage() { Message = "Verifier Type Not Specified." });
                    break;
                case VerifierTypes.MessageParser:
                    if (String.IsNullOrEmpty(Input) && String.IsNullOrEmpty(PathAndQueryString) && String.IsNullOrEmpty(Topic) && Headers.Count == 0)
                    {
                        result.Errors.Add(new ErrorMessage() { Message = "Must Supply at a minimum an Input, Path/Query String, Topic or Header." });
                    }

                    if (ExpectedOutputs.Count == 0)
                    {
                        //TODO: Add translation and error code
                        result.Errors.Add(new ErrorMessage() { Message = "Must Supply at least one Expected Output." });
                    }

                    foreach (var expectedOutput in ExpectedOutputs)
                    {
                        if (String.IsNullOrEmpty(expectedOutput.Key))
                        {
                            result.Errors.Add(new ErrorMessage() { Message = "Key is Required for All Specified Expected Outputs, please fix or remove any incomplete expected values." });
                        }

                        if (String.IsNullOrEmpty(expectedOutput.Value))
                        {
                            result.Errors.Add(new ErrorMessage() { Message = "Value is Required for All Specified Expected Outputs, please fix or remove any incomplete expected values." });
                        }
                    }

                    foreach (var expectedOutput in Headers)
                    {
                        if (String.IsNullOrEmpty(expectedOutput.Name))
                        {
                            result.Errors.Add(new ErrorMessage() { Message = "Name is Required for All Available Headers, please fix or remove any incomplete headers." });
                        }

                        if (String.IsNullOrEmpty(expectedOutput.Value))
                        {
                            result.Errors.Add(new ErrorMessage() { Message = "Value is Required for All Available Headers, please fix or remove any incomplete headers." });
                        }
                    }

                    break;
                case VerifierTypes.MessageFieldParser:
                    if (String.IsNullOrEmpty(Input) && String.IsNullOrEmpty(PathAndQueryString) && String.IsNullOrEmpty(Topic) && Headers.Count == 0)
                    {
                        result.Errors.Add(new ErrorMessage() { Message = "Must Supply at a minimum an Input, Path/Query String, Topic or Header." });
                    }

                    if (String.IsNullOrEmpty(ExpectedOutput))
                    {
                        //TODO: Add translation and error code
                        result.Errors.Add(new ErrorMessage() { Message = "Expected Output is a Required Field." });
                    }
                    break;
            }
        }


        public byte[] GetBinaryPayload()
        {
            if (String.IsNullOrEmpty(Input))
            {
                throw InvalidConfigurationException.FromErrorCode(ErrorCodes.Verifiers.MissingBinaryInput);
            }

            try
            {
                var bytes = new List<Byte>();

                var bytesList = Input.Split(' ');
                foreach (var byteStr in bytesList)
                {
                    var lowerByteStr = byteStr.ToLower();
                    if (lowerByteStr.Contains("soh"))
                    {
                        bytes.Add(0x01);

                    }
                    else if (lowerByteStr.Contains("stx"))
                    {
                        bytes.Add(0x02);
                    }
                    else if (lowerByteStr.Contains("etx"))
                    {
                        bytes.Add(0x03);
                    }
                    else if (lowerByteStr.Contains("eot"))
                    {
                        bytes.Add(0x04);
                    }
                    else if (lowerByteStr.Contains("ack"))
                    {
                        bytes.Add(0x06);
                    }
                    else if (lowerByteStr.Contains("cr"))
                    {
                        bytes.Add(0x0d);
                    }
                    else if (lowerByteStr.Contains("lf"))
                    {
                        bytes.Add(0x0a);
                    }
                    else if (lowerByteStr.Contains("nak"))
                    {
                        bytes.Add(0x15);
                    }
                    else if (lowerByteStr.Contains("esc"))
                    {
                        bytes.Add(0x1b);
                    }
                    else if (lowerByteStr.Contains("del"))
                    {
                        bytes.Add(0x1b);
                    }
                    else if (lowerByteStr.StartsWith("0x"))
                    {
                        bytes.Add(Byte.Parse(byteStr.Substring(2), System.Globalization.NumberStyles.HexNumber));
                    }
                    else
                    {
                        bytes.Add(Byte.Parse(byteStr, System.Globalization.NumberStyles.HexNumber));
                    }
                }

                return bytes.ToArray();
            }
            catch (Exception ex)
            {
                throw InvalidConfigurationException.FromErrorCode(ErrorCodes.Verifiers.CouldNotConvertInputToBytes, ex.Message);
            }
        }

        public VerifierSummary CreateSummary()
        {
            return new VerifierSummary()
            {
                Id = Id,
                Name = Name,
                IsPublic = IsPublic,
                Description = Description,
                Icon = "icon-ae-coding-proaction",
                Key = Key
            };
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(VerifierType),
                nameof(PopulateFromSampleMessageAction),
                nameof(InputType),
                nameof(PathAndQueryString),
                nameof(Topic),
                nameof(Input),
                nameof(Headers),
                nameof(ExpectedOutput),
            };
        }

        public FormConditionals GetConditionalFields()
        {
            return new FormConditionals()
            {
                ConditionalFields = new List<string>() { nameof(ExpectedOutput), nameof(ExpectedOutputs), nameof(VerifierType), nameof(PopulateFromSampleMessageAction) },
                Conditionals = new List<FormConditional>()
                 {
                     new FormConditional()
                     {
                         Field = nameof(VerifierType),
                         Value = Verifier.VerifierType_MessageFieldParser,
                         VisibleFields = new List<string>() {nameof(ExpectedOutput)},
                         RequiredFields = new List<string>() {nameof(ExpectedOutput)}

                     },
                     new FormConditional()
                     {
                         Field = nameof(VerifierType),
                         Value = Verifier.VerifierType_MessageParser,
                         VisibleFields = new List<string>() {nameof(ExpectedOutputs), nameof(PopulateFromSampleMessageAction)},
                         RequiredFields = new List<string>() {nameof(ExpectedOutputs)}
                     },
                 }
            };
        }

        public List<FormAdditionalAction> GetAdditionalActions()
        {
            return new List<FormAdditionalAction>()
            {
                new FormAdditionalAction()
                {
                     ForCreate = false,
                     Key = "run",
                     Title = "Run",
                     Icon = "fa fa-play"
                }
            };
        }
    }

    [EntityDescription(VerifierDomain.Verifiers, RuntimeCoreResources.Names.Verifiers_Title, RuntimeCoreResources.Names.Verifier_Help,
        RuntimeCoreResources.Names.Verifier_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(RuntimeCoreResources), Icon: "icon-ae-coding-proaction",
        GetUrl: "/api/verifier/{id}", SaveUrl: "/api/verifier", DeleteUrl: "/api/verifier/{id}", FactoryUrl: "/api/verifier/factory/{type}")]
    public class VerifierSummary : LagoVista.Core.Models.SummaryData
    {

    }
}
