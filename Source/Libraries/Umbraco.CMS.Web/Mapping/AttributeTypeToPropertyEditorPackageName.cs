using System;
using Umbraco.Cms.Web.Context;
using Umbraco.Framework.Persistence.Model.Attribution.MetaData;
using Umbraco.Framework.TypeMapping;

namespace Umbraco.Cms.Web.Mapping
{
    /// <summary>
    /// Resolves a property editor's package name from an attribute type if it is available
    /// </summary>
    internal class AttributeTypeToPropertyEditorPackageName : StandardMemberMapper<AttributeType, string>
    {
        public AttributeTypeToPropertyEditorPackageName(AbstractFluentMappingEngine currentEngine, MapResolverContext context)
            : base(currentEngine, context)
        {
        }

        public override string GetValue(AttributeType source)
        {
            Guid output;
            var guid = Guid.TryParse(source.RenderTypeProvider, out output) ? output : Guid.Empty;

            if (guid != Guid.Empty)
            {
                var editor = ResolverContext.PropertyEditorFactory.GetPropertyEditor(guid);
                if (editor != null)
                {
                    if (editor.Metadata.PluginDefinition != null)
                    {
                        return editor.Metadata.PluginDefinition.PackageName;   
                    }                    
                }
            }
            return string.Empty;
        }
    }
}