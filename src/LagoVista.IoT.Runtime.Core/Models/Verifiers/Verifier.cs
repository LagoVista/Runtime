using LagoVista.Core.Attributes;
using LagoVista.Core.Models;
using LagoVista.IoT.DeviceAdmin.Models;
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
        [EnumLabel(Verifier.VerifierType_MessageFieldParser, RuntimeCoreResources.Names.Verifier_VerifierType_MessageFieldParser, typeof(RuntimeCoreResources))]
        MessageFieldParser,
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

    [EntityDescription(VerifierDomain.Verifiers, RuntimeCoreResources.Names.Verifier_Title, RuntimeCoreResources.Names.Verifier_Help, RuntimeCoreResources.Names.Verifier_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(RuntimeCoreResources))]
    public class Verifier : IoTModelBase, IVerifier
    {
        public const string InputType_Binary = "binary";
        public const string InputType_Text = "text";

        public const string VerifierType_MessageFieldParser = "messagefieldparser";
        public const string VerifierType_Planner = "planner";

        public string DatabaseName { get; set; }
        public string EntityType { get; set; }

        public Verifier()
        {
            Headers = new ObservableCollection<Header>();
            ExpectedOutputs = new ObservableCollection<ExpectedValue>();
        }

        [FormField(LabelResource: RuntimeCoreResources.Names.Common_Key, HelpResource: RuntimeCoreResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: RuntimeCoreResources.Names.Common_Key_Validation, ResourceType: typeof(RuntimeCoreResources), IsRequired: true)]
        public String Key { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_InputType, EnumType: (typeof(InputTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(RuntimeCoreResources), WaterMark: RuntimeCoreResources.Names.Verifier_InputType_Select, HelpResource: RuntimeCoreResources.Names.Verifier_InputType_Help, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<InputTypes> InputType { get; set; }

        public EntityHeader<VerifierTypes> VerifierType { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_ShouldSucceed, HelpResource: RuntimeCoreResources.Names.Verifier_ShouldSucceed_Help, ResourceType: typeof(RuntimeCoreResources), FieldType: FieldTypes.CheckBox)]
        public bool ShouldSucceed { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_Header, HelpResource: RuntimeCoreResources.Names.Verifier_Header_Help, ResourceType: typeof(RuntimeCoreResources), FieldType: FieldTypes.ChildList)]
        public ObservableCollection<Header> Headers { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_PathAndQueryString, FieldType: FieldTypes.Text, HelpResource: RuntimeCoreResources.Names.Verifier_PathAndQueryString_Help, ResourceType: typeof(RuntimeCoreResources))]
        public String PathAndQueryString { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_Component, FieldType: FieldTypes.EntityHeaderPicker, IsRequired: true, ResourceType: typeof(RuntimeCoreResources))]
        public EntityHeader Component { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_Input, FieldType: FieldTypes.MultiLineText, IsRequired: true, ResourceType: typeof(RuntimeCoreResources))]
        public string Input { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_ExpectedOutput, FieldType: FieldTypes.MultiLineText, IsRequired: true, ResourceType: typeof(RuntimeCoreResources))]
        public string ExpectedOutput { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.Verifier_ExpectedOutput, FieldType: FieldTypes.MultiLineText, IsRequired: true, ResourceType: typeof(RuntimeCoreResources))]
        public ObservableCollection<ExpectedValue> ExpectedOutputs { get; set; }


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


        [FormField(LabelResource: RuntimeCoreResources.Names.Common_IsPublic, FieldType: FieldTypes.Bool, ResourceType: typeof(RuntimeCoreResources))]
        public bool IsPublic { get; set; }
        public EntityHeader OwnerOrganization { get; set; }
        public EntityHeader OwnerUser { get; set; }

        public VerifierSummary CreateSummary()
        {
            return new VerifierSummary()
            {
                Id = Id,
                Name = Name,
                IsPublic = IsPublic,
                Description = Description,
                Key = Key
            };
        }
    }

    public class VerifierSummary : LagoVista.Core.Models.SummaryData
    {

    }
}
