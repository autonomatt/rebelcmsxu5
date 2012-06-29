﻿using Umbraco.Framework.Persistence;
using Umbraco.Framework.Persistence.Model.Attribution.MetaData;
using Umbraco.Framework.Persistence.Model.Constants.AttributeDefinitions;
using Umbraco.Framework.Persistence.Model.Constants.AttributeTypes;
using Umbraco.Framework.Persistence.Model.Constants.Schemas;

namespace Umbraco.Framework.Security.Model.Schemas
{
    public abstract class ProfileSchema : SystemSchema
    {
        public const string ProviderUserKeyAlias = "providerUserKey";

        protected abstract AttributeGroup DetailsGroup { get; }

        protected virtual void CreatedAttributeDefinitions()
        {
            var inBuiltNodeNameType = AttributeTypeRegistry.Current.GetAttributeType(NodeNameAttributeType.AliasValue);
            var inBuiltTextType = AttributeTypeRegistry.Current.GetAttributeType(StringAttributeType.AliasValue);

            this.AttributeDefinitions.Add(new AttributeDefinition(NodeNameAttributeDefinition.AliasValue, "Name")
            {
                AttributeType = inBuiltNodeNameType,
                AttributeGroup = DetailsGroup,
                Ordinal = 0
            });

            this.AttributeDefinitions.Add(new AttributeDefinition(ProviderUserKeyAlias, "Provider User Key")
            {
                AttributeType = inBuiltTextType,
                AttributeGroup = DetailsGroup,
                Ordinal = 1
            });
        }
    }
}
