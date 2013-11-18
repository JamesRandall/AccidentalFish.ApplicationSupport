using System;
using System.Collections.Generic;
using System.Reflection;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using CuttingEdge.Conditions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.NoSql
{
    /// <summary>
    /// Wrapping entities for processing in this way incurs a performance penalty but it stops Azure bleeding out at the API level.
    /// I can't, yet, make my mind up as to if this is misguided or not! Need to plug in an alternative like Mongo and feel where
    /// the edges truly are.
    /// </summary>
    internal class AzureNoSqlEntityWrapper<T> : ITableEntity where T : NoSqlEntity
    {
        private T _wrappedEntity;

        public AzureNoSqlEntityWrapper()
        {
            
        }

        public AzureNoSqlEntityWrapper(T wrappedEntity)
        {
            Condition.Requires(wrappedEntity).IsNotNull();
            _wrappedEntity = wrappedEntity;
        }

        public T WrappedEntity { get { return _wrappedEntity; } }

        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            if (_wrappedEntity == null)
            {
                _wrappedEntity = Activator.CreateInstance<T>();
            }
            // TODO: Optimisation - look at the type of _wrappedEntity and compile expressions and place them in a cache
            Type entityType = _wrappedEntity.GetType();
            foreach (KeyValuePair<string, EntityProperty> propertyValuePair in properties)
            {
                PropertyInfo property = entityType.GetProperty(propertyValuePair.Key);
                EntityProperty ep = propertyValuePair.Value;
                if (ep.PropertyType == EdmType.Binary)
                {
                    property.SetValue(_wrappedEntity, ep.BinaryValue);
                }
                else if (ep.PropertyType == EdmType.Boolean)
                {
                    property.SetValue(_wrappedEntity, ep.BooleanValue);
                }
                else if (ep.PropertyType == EdmType.DateTime)
                {
                    property.SetValue(_wrappedEntity, ep.DateTimeOffsetValue);
                }
                else if (ep.PropertyType == EdmType.Double)                
                {
                    property.SetValue(_wrappedEntity, ep.DoubleValue);
                }
                else if (ep.PropertyType == EdmType.Guid)
                {
                    property.SetValue(_wrappedEntity, ep.GuidValue);
                }
                else if (ep.PropertyType == EdmType.Int32)
                {
                    property.SetValue(_wrappedEntity, ep.Int32Value);
                }
                else if (ep.PropertyType == EdmType.Int64)
                {
                    property.SetValue(_wrappedEntity, ep.Int64Value);
                }
                else if (ep.PropertyType == EdmType.String)
                {
                    property.SetValue(_wrappedEntity, ep.StringValue);
                }
                else
                {
                    throw new InvalidOperationException("Unexpected property type");
                }
            }
        }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            if (_wrappedEntity == null)
            {
                throw new InvalidOperationException("No entity to write");
            }

            Dictionary<string, EntityProperty> entityBag = new Dictionary<string, EntityProperty>();
            PropertyInfo[] properties = typeof (T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                Type propertyType = property.PropertyType;
                object value = property.GetValue(_wrappedEntity);
                EntityProperty entityProperty;
                if (propertyType == typeof(Byte[]))
                {
                    entityProperty = new EntityProperty((byte[])value);
                }
                else if (propertyType == typeof(bool))
                {
                    entityProperty = new EntityProperty((bool)value);
                }
                else if (propertyType == typeof(DateTimeOffset))
                {
                    entityProperty = new EntityProperty((DateTimeOffset?)value);
                }
                else if (propertyType == typeof(DateTime))
                {
                    entityProperty = new EntityProperty((DateTime?)value);
                }
                else if (propertyType == typeof(Double))
                {
                    entityProperty = new EntityProperty((double?)value);
                }
                else if (propertyType == typeof(Guid))
                {
                    entityProperty = new EntityProperty((Guid?)value);
                }
                else if (propertyType == typeof(int))
                {
                    entityProperty = new EntityProperty((int?)value);
                }
                else if (propertyType == typeof(long))
                {
                    entityProperty = new EntityProperty((long?)value);
                }
                else if (propertyType == typeof(string))
                {
                    entityProperty = new EntityProperty((string) value);
                }
                else if (propertyType.IsEnum)
                {
                    entityProperty = new EntityProperty((int) value);
                }
                else
                {
                    throw new InvalidOperationException(String.Format("Property type {0} cannot be wrapped", propertyType));
                }
                entityBag.Add(property.Name ,entityProperty);
            }
            return entityBag;
        }

        public string PartitionKey
        {
            get { return _wrappedEntity.PartitionKey; }
            set { _wrappedEntity.PartitionKey = value; }
        }
        
        public string RowKey
        {
            get { return _wrappedEntity.RowKey; }
            set { _wrappedEntity.RowKey = value; }
        }

        public DateTimeOffset Timestamp
        {
            get { return _wrappedEntity.Timestamp; }
            set { _wrappedEntity.Timestamp = value; }
        }
        
        public string ETag { get; set; }
    }
}
