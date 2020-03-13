
using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Crm.Sdk.Messages;
using System.Collections.Generic;
namespace SummerUpper
{
    public sealed partial class SummerUpper : CodeActivity
    {
        [Input("Entity Sum field")]
        public InArgument<string> entitySumField { get; set; }
        [Input("Entities lookup field name to Parent Entity")]
        public InArgument<string> parentEntityLookupName { get; set; }

        [Output("Sum")]
        public OutArgument<int> Sum { get; set; }

        [Output("Money Sum")]
        public OutArgument<decimal> moneySum { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory =
            executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            int summerUpper = 0;
            decimal summerUppermoney = 0;
            Entity thing = new Entity();
            thing.Id = context.PrimaryEntityId;
            thing.LogicalName = context.PrimaryEntityName;               
            string EntityLookup = this.parentEntityLookupName.Get(executionContext);
            string EntitySumField = this.entitySumField.Get(executionContext);
            EntityReference mummyId = null;
            string fetch1 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                            <entity name='" + context.PrimaryEntityName + @"'>
                                            <attribute name='" + EntityLookup + @"' />                                              
                                            <filter type='and'>
                                                <condition attribute='" + context.PrimaryEntityName + @"id' operator='eq' value='{" + context.PrimaryEntityId.ToString() + @"}' />                                                 
                                            </filter>
                                            </entity>
                                        </fetch>";
            EntityCollection ec1 = service.RetrieveMultiple(new FetchExpression(fetch1));
            List<Entity> Entities1 = new List<Entity>();
            if (ec1 != null && ec1.Entities != null && ec1.Entities.Count > 0)
            {
                if (ec1.Entities.Count >= 0)
                {
                    Entities1.AddRange(ec1.Entities);
                }
            }
            if (ec1 != null && ec1.Entities != null && ec1.Entities.Count > 0)
            {

                foreach (var c in Entities1)
                {
                    if (c.Attributes.Contains(EntityLookup))
                    {
                            
                        mummyId= ((c[EntityLookup]) as EntityReference);
                    }
                }
            }
             string fetch2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                            <entity name='"+ context.PrimaryEntityName + @"'>
                                            <attribute name='"+EntitySumField+@"' />                                              
                                            <filter type='and'>
                                                <condition attribute='"+EntityLookup+@"' operator='eq' value='{" + mummyId.Id.ToString() + @"}' />                                                 
                                            </filter>
                                            </entity>
                                        </fetch>";
            EntityCollection ec = service.RetrieveMultiple(new FetchExpression(fetch2));
            List<Entity> Entities = new List<Entity>();
            if (ec != null && ec.Entities != null && ec.Entities.Count > 0)
            {
                if (ec.Entities.Count >= 0)
                {
                    Entities.AddRange(ec.Entities);
                }
            }
            if (ec != null && ec.Entities != null && ec.Entities.Count > 0)
            {                      
                foreach (var c in Entities)
                {
                    if (c.Attributes.Contains(EntitySumField))
                    {
                        var att = (c[EntitySumField]);

                        var atttype = att.GetType();
                        if (atttype.Name == "Money" || atttype.Name == "Decimal")
                        {
                            Money money = ((c.GetAttributeValue<Money>(EntitySumField)));
                            int moneymoneymoneyMoneeeh = Convert.ToInt32(money.Value);
                            decimal Mahneeh = money.Value;
                            summerUppermoney = summerUppermoney + Mahneeh;
                        }
                        else
                        {
                            int number = ((c.GetAttributeValue<int>(EntitySumField)));
                            summerUpper = summerUpper + number;
                        }
                    }
                }
             }
         Sum.Set(executionContext, summerUpper);
         moneySum.Set(executionContext, summerUppermoney);
        }
    }
 }

  
