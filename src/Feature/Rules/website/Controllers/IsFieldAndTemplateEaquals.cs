using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Data.Templates;
using Sitecore.Data.Managers;

namespace BasicCompany.Feature.Rules.Controllers
{
    public class IsFieldAndTemplateEaquals<T> : StringOperatorCondition<T> where T : RuleContext
    {
        public string Value { get; set; }

        public string FieldName { get; set; }

        private ID templateId;

        public IsFieldAndTemplateEaquals() => this.templateId = ID.Null;

        public IsFieldAndTemplateEaquals(ID templateId)
        {
            Assert.ArgumentNotNull((object)templateId, nameof(templateId));
            this.templateId = templateId;
        }

        public ID TemplateId
        {
            get => this.templateId;
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.templateId = value;
            }
        }

        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull((object)ruleContext, nameof(ruleContext));
            Item obj = ruleContext.Item;

            string str = this.Value ?? string.Empty;

            if (obj == null || string.IsNullOrEmpty(this.FieldName))
                return false;

            if (obj.TemplateID == this.TemplateId)
                return this.Compare(obj[this.FieldName], str) && true;

            Template template1 = TemplateManager.GetTemplate(obj);
            if (template1 == null)
                return false;

            Template template2 = TemplateManager.GetTemplate(this.TemplateId, obj.Database);
            if (template2 == null)
                return false;

            TemplateList baseTemplates = template1.GetBaseTemplates();

            return baseTemplates != null && baseTemplates.Contains(template2);
        }
    }
}