﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Cms.Web.Context;
using Umbraco.Cms.Web.Model.BackOffice.Editors;
using Umbraco.Framework.Persistence.Model;
using Umbraco.Framework.Persistence.Model.Constants.Entities;
using Umbraco.Framework.TypeMapping;

namespace Umbraco.Cms.Web.Mapping
{
    //internal class MembershipUserToEditorModel<TUser, TEditorModel> : TypedEntityToContentEditorModel<TUser, TEditorModel>
    //    where TUser : TypedEntity, IMembershipUser
    //    where TEditorModel : MembershipUserEditorModel
    //{
    //    public MembershipUserToEditorModel(AbstractFluentMappingEngine engine, MapResolverContext resolverContext, Action<TUser, TEditorModel> additionalAfterMap) 
    //        : base(engine, resolverContext, additionalAfterMap)
    //    {
    //        MappingContext
    //            .MapMemberFrom(x => x.LinkedProfileHiveId, x => x.LinkedProfileHiveId)
    //            //ignore all custom properties as these need to be mapped by the underlying attributes
    //            .ForMember(x => x.Name, opt => opt.Ignore())
    //            .ForMember(x => x.Username, opt => opt.Ignore())
    //            .ForMember(x => x.Password, opt => opt.Ignore())
    //            .ForMember(x => x.ConfirmPassword, opt => opt.Ignore())
    //            .ForMember(x => x.Email, opt => opt.Ignore())
    //            .ForMember(x => x.IsApproved, opt => opt.Ignore())
    //            .ForMember(x => x.LastLoginDate, opt => opt.Ignore())
    //            .ForMember(x => x.LastActivityDate, opt => opt.Ignore())
    //            .ForMember(x => x.LastPasswordChangeDate, opt => opt.Ignore());
    //    }
    //}
}
