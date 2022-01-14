using System;
using System.Linq;
using System.Web;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using static System.FormattableString;

namespace BasicCompany.Feature.Forms.Controllers
{
    public class SubmitActionForRedirecting : SubmitActionBase<string>
    {
        public SubmitActionForRedirecting(ISubmitActionData submitActionData) :
             base(submitActionData)
        { }

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
            Assert.ArgumentNotNull((object)formSubmitContext, nameof(formSubmitContext));

            formSubmitContext.RedirectUrl = "https://helix-basic-unicorn.dev.local/en/";
            formSubmitContext.RedirectOnSuccess = true;
            formSubmitContext.Abort();

            if (!formSubmitContext.HasErrors)
            {
                Logger.Info(Invariant($"Form {formSubmitContext.FormId} redirected to {formSubmitContext.RedirectUrl} successfully."), this);
            }
            else
            {
                Logger.Warn(Invariant($"Form {formSubmitContext.FormId} redirected with errors: {string.Join(", ", formSubmitContext.Errors.Select(t => t.ErrorMessage))}."), this);
            }

            return true;
        }

        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }
    }
}
