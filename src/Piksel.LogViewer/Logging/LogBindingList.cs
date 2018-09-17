using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piksel.LogViewer.Logging
{
    public class LogBindingList : IBindingListView
    {
        List<LogMessage> allMessages = new List<LogMessage>();
        List<LogMessage> enabledMessages = new List<LogMessage>();

        public object this[int index] {
            get => enabledMessages[index];
            set => throw new NotSupportedException();
        }

        public bool AllowNew => false;

        public bool AllowEdit => false;

        public bool AllowRemove => false;

        public bool SupportsChangeNotification => true;

        public bool SupportsSearching => true;

        public bool SupportsSorting => true;

        public bool IsSorted => false;

        public PropertyDescriptor SortProperty => null;

        public ListSortDirection SortDirection => ListSortDirection.Ascending;

        public bool IsReadOnly => true;

        public bool IsFixedSize => false;

        public int Count => enabledMessages.Count;

        public object SyncRoot => this;

        public bool IsSynchronized => false;

        public LogLevel EnabledLevels { get; private set; } = LogLevel.All;

        private string filter;
        public string Filter {
            get => filter;
            set
            {
                if (filter == value) return;
                filter = value;
                ParseEnabledLevels(filter);
                FilterUpdated();
            }
        }

        private void ParseEnabledLevels(string filter)
        {
            if(string.IsNullOrEmpty(filter))
            {
                EnabledLevels = LogLevel.All;
                return;
            }

            if (Enum.TryParse(filter, out LogLevel enabledLevels))
            {
                EnabledLevels = enabledLevels;
            }
        }

        private void FilterUpdated()
        {
            RaiseListChangedEvents = false;

            var enabled = EnabledLevels;

            enabledMessages.Clear();
            foreach(var lm in allMessages)
            {
                if((lm.LogLevel & enabled) != 0)
                {
                    enabledMessages.Add(lm);
                }
            }
            RaiseListChangedEvents = true;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        public ListSortDescriptionCollection SortDescriptions => new ListSortDescriptionCollection();

        public bool SupportsAdvancedSorting => false;

        public bool SupportsFiltering => true;

        public bool RaiseListChangedEvents { get; private set; } = true;

        public event ListChangedEventHandler ListChanged;

        public int Add(object value)
        {
            if (value is LogMessage lm)
            {
                allMessages.Add(lm);
                if ((lm.LogLevel & EnabledLevels) != 0)
                {
                    enabledMessages.Add(lm);
                    var newIndex = enabledMessages.Count - 1;
                    OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, newIndex));
                    return newIndex;
                }
                else
                {
                    return -1;
                }
            }
            throw new ArgumentException(nameof(value));
        }

        private void OnListChanged(ListChangedEventArgs listChangedEventArgs)
        {
            if (!RaiseListChangedEvents) return;
            ListChanged?.Invoke(this, listChangedEventArgs);
        }

        public void AddIndex(PropertyDescriptor property)
        {
            throw new NotImplementedException();
        }

        public object AddNew()
        {
            throw new NotImplementedException();
        }

        public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            //throw new NotImplementedException();
        }

        public void ApplySort(ListSortDescriptionCollection sorts)
        {

        }

        public void Clear()
        {
            allMessages.Clear();
            enabledMessages.Clear();
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Find(PropertyDescriptor property, object key)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
            => enabledMessages.GetEnumerator();

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void RemoveFilter()
        {
            Filter = string.Empty;
        }

        public void RemoveIndex(PropertyDescriptor property)
        {
            throw new NotImplementedException();
        }

        public void RemoveSort()
        {
            throw new NotImplementedException();
        }
    }
}
