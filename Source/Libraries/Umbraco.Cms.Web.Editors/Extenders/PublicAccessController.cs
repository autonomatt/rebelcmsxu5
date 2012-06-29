﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Cms.Web.Context;
using Umbraco.Cms.Web.Model.BackOffice.Editors;
using Umbraco.Cms.Web.Mvc;
using Umbraco.Cms.Web.Mvc.ActionFilters;
using Umbraco.Cms.Web.Mvc.ViewEngines;
using Umbraco.Cms.Web.Security;
using Umbraco.Framework;
using Umbraco.Framework.Localization;
using Umbraco.Framework.Persistence;
using Umbraco.Framework.Persistence.Model;
using Umbraco.Framework.Persistence.Model.Associations;
using Umbraco.Framework.Persistence.Model.Constants;
using Umbraco.Framework.Persistence.Model.Constants.AttributeDefinitions;
using Umbraco.Framework.Persistence.Model.Constants.Entities;
using Umbraco.Framework.Security;
using Umbraco.Framework.Security.Model.Entities;
using Umbraco.Hive;
using Umbraco.Hive.RepositoryTypes;

namespace Umbraco.Cms.Web.Editors.Extenders
{

    public class PublicAccessController : ContentControllerExtenderBase
    {
        public PublicAccessController(IBackOfficeRequestContext backOfficeRequestContext)
            : base(backOfficeRequestContext)
        { }

        /// <summary>
        /// Permissionses the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [HttpGet]
        [UmbracoAuthorize(Permissions = new[] { FixedPermissionIds.PublicAccess })] 
        public virtual ActionResult PublicAccess(HiveId id)
        {
            var model = new PublicAccessModel { Id = id };

            using (var uow = BackOfficeRequestContext.Application.Hive.OpenReader<IContentStore>())
            using (var securityUow = BackOfficeRequestContext.Application.Hive.OpenReader<ISecurityStore>())
            {
                // Get all user groups
                var userGroups = securityUow.Repositories
                    .GetChildren<UserGroup>(FixedRelationTypes.DefaultRelationType, Framework.Security.Model.FixedHiveIds.MemberGroupVirtualRoot)
                    .OrderBy(x => x.Name);

                model.AvailableUserGroups = userGroups.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

                // Get links
                var currentPubclicAccessRelation = uow.Repositories.GetParentRelations(id, FixedRelationTypes.PublicAccessRelationType)
                    .SingleOrDefault();

                if (currentPubclicAccessRelation != null && currentPubclicAccessRelation.MetaData != null)
                {
                    var metaData = currentPubclicAccessRelation.MetaData;

                    if(metaData.Any(x => x.Key == "UserGroupIds"))
                        model.UserGroupIds = metaData.SingleOrDefault(x => x.Key == "UserGroupIds").Value.DeserializeJson<IEnumerable<HiveId>>();

                    if (metaData.Any(x => x.Key == "LoginPageId"))
                        model.LoginPageId = HiveId.Parse(currentPubclicAccessRelation.MetaData.SingleOrDefault(x => x.Key == "LoginPageId").Value);

                    if (metaData.Any(x => x.Key == "ErrorPageId"))
                        model.ErrorPageId = HiveId.Parse(currentPubclicAccessRelation.MetaData.SingleOrDefault(x => x.Key == "ErrorPageId").Value);
                }
            }

            return View(model);
        }

        /// <summary>
        /// Permissionses the form.
        /// </summary>
        /// <returns></returns>
        [ActionName("PublicAccess")]
        [HttpPost]
        //[UmbracoAuthorize(Permissions = new[] { FixedPermissionIds.Permissions })] 
        public virtual JsonResult PublicAccessForm(PublicAccessModel model)
        {
            using (var uow = BackOfficeRequestContext.Application.Hive.OpenWriter<IContentStore>())
            {
                var entity = uow.Repositories.Get<TypedEntity>(model.Id);
                if (entity == null)
                    throw new NullReferenceException("Could not find entity with id " + model.Id);

                // Store the redirect locations
                var currentPubclicAccessRelation = uow.Repositories.GetParentRelations(entity.Id, FixedRelationTypes.PublicAccessRelationType)
                    .SingleOrDefault();

                if (model.LoginPageId.IsNullValueOrEmpty() && 
                    !model.UserGroupIds.Any() && currentPubclicAccessRelation != null)
                {
                    uow.Repositories.RemoveRelation(currentPubclicAccessRelation);
                }
                else
                {
                    uow.Repositories.ChangeOrCreateRelationMetadata(FixedHiveIds.SystemRoot, entity.Id, FixedRelationTypes.PublicAccessRelationType,
                        new[] {
                            new RelationMetaDatum("UserGroupIds", model.UserGroupIds.ToJsonString()),
                            new RelationMetaDatum("LoginPageId", model.LoginPageId.ToString()),
                            new RelationMetaDatum("ErrorPageId", model.ErrorPageId.ToString())
                        });
                }

                uow.Complete();

                var successMsg = "PublicAccess.Success.Message".Localize(this, new
                    {
                        Name = entity.GetAttributeValue(NodeNameAttributeDefinition.AliasValue, "Name")
                    });

                Notifications.Add(new NotificationMessage(
                                      successMsg,
                                      "PublicAccess.Title".Localize(this), NotificationType.Success));

                return new CustomJsonResult(new
                    {
                        success = true,
                        notifications = Notifications,
                        msg = successMsg
                    }.ToJsonString);
            }
        }
    }
}