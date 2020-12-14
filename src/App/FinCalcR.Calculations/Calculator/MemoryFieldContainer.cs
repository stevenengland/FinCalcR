using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public class MemoryFieldContainer : IMemoryFieldContainer
    {
        private readonly Dictionary<string, object> container = new Dictionary<string, object>();

        public void Add<T>(IMemoryField<T> memoryField)
        {
            this.container.Add(memoryField.Id, memoryField);
        }

        public IMemoryField<T> Get<T>(string key)
        {
            if (this.container.ContainsKey(key))
            {
                var value = this.container[key];
                if (value is IMemoryField<T> field)
                {
                    return field;
                }

                throw new KeyNotFoundException("An item with the key was found but it is not of the correct type.");
            }

            throw new KeyNotFoundException($"An item with key {key} does not exist.");
        }

        public IMemoryFieldDescriptor Get(string key)
        {
            if (this.container.ContainsKey(key))
            {
                var value = this.container[key];
                if (value is IMemoryFieldDescriptor field)
                {
                    return field;
                }

                throw new KeyNotFoundException("An item with the key was found but it is not of the correct type.");
            }

            throw new KeyNotFoundException($"An item with key {key} does not exist.");
        }

        public void Reset(IList<string> categories = null)
        {
            if (categories == null || !categories.Any())
            {
                foreach (var memoryField in this.container.Values)
                {
                    var memoryFieldDescriptor = (IMemoryFieldDescriptor)memoryField;
                    memoryFieldDescriptor.Reset();
                }
            }
            else
            {
                foreach (var category in categories)
                {
                    foreach (var memoryField in this.container.Values)
                    {
                        var memoryFieldDescriptor = (IMemoryFieldDescriptor)memoryField;
                        if (memoryFieldDescriptor.Category == category)
                        {
                            memoryFieldDescriptor.Reset();
                        }
                    }
                }
            }
        }
    }
}
