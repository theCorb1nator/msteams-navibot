using System;

namespace NaviBot.Data.ExpandableQueries
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ExpansionExpressionAttribute : Attribute { }
}