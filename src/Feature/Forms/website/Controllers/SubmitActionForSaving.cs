using System;
using System.Collections.Generic;
using System.Linq;
using BasicCompany.Feature.Forms.Models;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Data;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Data;
using Sitecore.ExperienceForms.Data.Entities;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using static System.FormattableString;


namespace BasicCompany.Feature.Forms.Controllers
{
    public class SubmitActionForSaving : SubmitActionBase<string>
    {
        private readonly IFormDataProvider _formDataProvider;

        protected virtual ITracker CurrentTracker => Tracker.Current;

        public SubmitActionForSaving(ISubmitActionData submitActionData) 
            : base(submitActionData)
        {
            _formDataProvider = (IFormDataProvider)ServiceLocator.ServiceProvider.GetService(typeof(IFormDataProvider));
        }

        public override void ExecuteAction(FormSubmitContext formSubmitContext, string parameters)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));
            
            if (TryParse(parameters, out string target))
            {
                try 
                {
                    if (Execute(target, formSubmitContext))
                        return;
                    
                }
                catch (Exception ex)
                {
                    Log.Error($"Sitecore Lead Submit Action Error. Message:[{ex.Message}]", ex, GetType());
                }
            }
            formSubmitContext.Errors.Add(new FormActionError()
            {
                ErrorMessage = SubmitActionData.ErrorMessage
            });
        }


        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));

            var sessionId = Guid.NewGuid();
            var formId = formSubmitContext.FormId;
            var formItem = Context.Database.GetItem(new ID(formId));
            var formFieldList = new List<FormFieldData>();

            if (formSubmitContext.HasErrors)
            {
                Logger.Warn(Invariant($"Form {formSubmitContext.FormId} submitted with errors: {string.Join(", ", formSubmitContext.Errors.Select(t => t.ErrorMessage))}."), this);
            }

            if (formItem != null)
            {
                var register = new Register()
                {
                    FullNameValue = GetValue(formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("FullName"))),
                    FullNameId = GetItemId(formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("FullName"))),
                    EmailValue = GetValue(formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Email"))),
                    EmailId = GetItemId(formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Email"))),
                    PasswordValue = GetValue(formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Password"))),
                    PasswordId = GetItemId(formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Passwor"))),
                    RoleValue = GetValue(formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Role"))),
                    RoleId = GetItemId(formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Role"))),
                };

                if (!string.IsNullOrEmpty(register.FullNameId))
                {
                    var formFullNameData = GetFormFieldData(register.FullNameId, register.FullNameValue);

                    formFieldList.Add(formFullNameData);
                }
                if (!string.IsNullOrEmpty(register.EmailId))
                {
                    var formEmailData = GetFormFieldData(register.EmailId, register.EmailValue);

                    formFieldList.Add(formEmailData);
                }
                if (!string.IsNullOrEmpty(register.PasswordId))
                {
                    var formPasswordData = GetFormFieldData(register.PasswordId, register.PasswordValue);

                    formFieldList.Add(formPasswordData);
                }
                if (!string.IsNullOrEmpty(register.RoleId))
                {
                    var formRoleData = GetFormFieldData(register.RoleId, register.RoleValue);

                    formFieldList.Add(formRoleData);
                }

                SaveFormsData(formId, formFieldList);

                Logger.Info(Invariant($"Form {formSubmitContext.FormId} submitted successfully with datas: FullName {register.FullNameValue}; Email {register.EmailValue}."), this);

                return true;
            }

            return false;
        }

        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        private static string GetValue(object field)
        {
            return field?.GetType().GetProperty("Value")?.GetValue(field, null)?.ToString() ?? string.Empty;
        }

        private static string GetItemId(object field)
        {
            return field?.GetType().GetProperty("ItemId")?.GetValue(field, null).ToString() ?? string.Empty;
        }

        private FormFieldData GetFormFieldData(string fieldId, string fieldValue)
        {
            if (fieldId == null)
                return new FormFieldData();

            var fieldItem = Context.Database.GetItem(fieldId);

            if (fieldItem == null)
                return new FormFieldData();
            
            var formData = new FormFieldData
            {
                FormFieldName = fieldItem.Name,
                FormFieldGuid = fieldItem.ID.ToGuid(),
                FormFieldValue = fieldValue
            };

            return formData;
        }

        public void SaveFormsData(Guid formId, List<FormFieldData> formFieldDataList)
        {
            Guid sessionId = Guid.NewGuid();

            FormEntry formEntry = new FormEntry()
            {
                Created = DateTime.Now,
                FormItemId = formId,
                FormEntryId = sessionId,
                Fields = new List<FieldData>()
            };

            foreach (var item in formFieldDataList)
            {
                AddFormFieldData(item, formEntry);
            }

            _formDataProvider.CreateEntry(formEntry);
        }

        protected static void AddFormFieldData(FormFieldData formFieldDataModel, FormEntry formEntry)
        {
            string str = "System.String";
            string fieldValue = formFieldDataModel.FormFieldValue;

            FieldData fieldData = new FieldData()
            {
                FieldDataId = Guid.NewGuid(),
                FieldItemId = formFieldDataModel.FormFieldGuid,
                FormEntryId = formEntry.FormEntryId,
                FieldName = formFieldDataModel.FormFieldName,
                Value = fieldValue,
                ValueType = str
            };

            formEntry.Fields.Add(fieldData);
        }
    }
}