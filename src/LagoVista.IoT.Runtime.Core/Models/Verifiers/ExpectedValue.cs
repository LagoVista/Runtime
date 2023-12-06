﻿using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Runtime.Core.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models.Verifiers
{

    [EntityDescription(VerifierDomain.Verifiers, RuntimeCoreResources.Names.ExpectedValue_Title, RuntimeCoreResources.Names.ExpectedValue_Help,
        RuntimeCoreResources.Names.ExpectedValue_Help, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(RuntimeCoreResources), 
        FactoryUrl: "/api/verifier/expectedoutput/factory", Icon:"")]
    public class ExpectedValue : IFormDescriptor
    {        
        public string Key { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.ExpectedValue_Field, IsRequired: true, FieldType: FieldTypes.Picker, WaterMark:RuntimeCoreResources.Names.ExpectedValue_Field_Select, ResourceType: typeof(RuntimeCoreResources))]
        public EntityHeader Field { get; set; }

        [FormField(LabelResource: RuntimeCoreResources.Names.ExpectedValue_Value, IsRequired:true, FieldType: FieldTypes.Text, ResourceType: typeof(RuntimeCoreResources))]
        public string Value { get; set; }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Field),
                nameof(Value)
            };
        }
    }
}
