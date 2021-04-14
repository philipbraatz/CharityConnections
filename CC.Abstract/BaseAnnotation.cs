﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CC.Abstract
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BaseAttribute : Attribute
    {
        public dynamic GetDynamic<TEntity>(BaseModel<TEntity> model,MethodBase mb) where TEntity : class
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));
            if (mb is null)
                throw new ArgumentNullException(nameof(mb));

            return (string)model.getProperty(mb.Name.Substring(3));
        }
    }
}
