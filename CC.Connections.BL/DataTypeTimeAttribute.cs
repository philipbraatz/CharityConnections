using System;
using System.ComponentModel.DataAnnotations;

namespace Doorfail.Connections.BL
{
    internal class DataTypeTimeAttribute : DataTypeAttribute
    {
        public DataTypeTimeAttribute(DataType dataType) : base(dataType)
        {
        }

        public override object TypeId => base.TypeId;

        public override bool RequiresValidationContext => base.RequiresValidationContext;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }

        public override string GetDataTypeName()
        {
            return base.GetDataTypeName();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute();
        }

        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }

        public override bool Match(object obj)
        {
            return base.Match(obj);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }
    }
}