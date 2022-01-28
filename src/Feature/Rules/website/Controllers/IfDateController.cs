using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System;
using Sitecore.Common;
using Sitecore.Diagnostics;
using Sitecore;

namespace BasicCompany.Feature.Rules.Controllers
{
    public class IfDateController<T> : WhenCondition<T> where T : RuleContext
    {
        public string Now { get; set; }

        protected override bool Execute(T ruleContext)
        {
            DateTime utcNow = DateTimeProvider.GetUtcNow();
            DateTime dateTime = DateUtil.ParseDateTime(this.Now, DateTime.MaxValue);
            if (dateTime.Kind != DateTimeKind.Utc)
                this.HandleInvalidDateKind();
            return utcNow > dateTime;
        }

        protected virtual void HandleInvalidDateKind() => Log.Warn("Sitecore.Rules.Conditions.DateTimeConditions.NowCondition<T> expects UTC date, but date with different kind has been assigned to Now property: " + this.Now, (object)this);
    }
}