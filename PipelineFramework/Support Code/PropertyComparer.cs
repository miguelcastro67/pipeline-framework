using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using Pipeline.Configuration;

namespace Pipeline
{
    public class PropertyComparer : IComparer<PropertyInfo>
    {
        int IComparer<PropertyInfo>.Compare(PropertyInfo x, PropertyInfo y)
        {
            int ret = 0;

            int orderX = 0;
            int orderY = 0;

            object[] attrX = x.GetCustomAttributes(typeof(PipelineEventAttribute), true);
            object[] attrY = y.GetCustomAttributes(typeof(PipelineEventAttribute), true);

            if (attrX.Length > 0)
            {
                PipelineEventAttribute attr = (PipelineEventAttribute)attrX[0];
                orderX = attr.Order;
            }

            if (attrY.Length > 0)
            {
                PipelineEventAttribute attr = (PipelineEventAttribute)attrY[0];
                orderY = attr.Order;
            }

            if (orderX < orderY)
                ret = -1;
            else if (orderX > orderY)
                ret = 1;

            return ret;
        }
    }
}
