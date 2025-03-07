﻿// <auto-generated />
#nullable enable

using NUnit.Framework;
using Realms;
using Realms.Schema;
using Realms.Tests.Database;
using Realms.Weaving;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using TestAsymmetricObject = Realms.IAsymmetricObject;
using TestEmbeddedObject = Realms.IEmbeddedObject;
using TestRealmObject = Realms.IRealmObject;

namespace Realms.Tests.Database
{
    [Generated]
    [Woven(typeof(DynamicTaskObjectHelper))]
    public partial class DynamicTask : IRealmObject, INotifyPropertyChanged, IReflectableType
    {
        public static Realms.Schema.ObjectSchema RealmSchema = new Realms.Schema.ObjectSchema.Builder("DynamicTask", ObjectSchema.ObjectType.RealmObject)
        {
            Realms.Schema.Property.Primitive("Id", Realms.RealmValueType.String, isPrimaryKey: true, isIndexed: false, isNullable: true, managedName: "Id"),
            Realms.Schema.Property.Primitive("Summary", Realms.RealmValueType.String, isPrimaryKey: false, isIndexed: false, isNullable: true, managedName: "Summary"),
            Realms.Schema.Property.Object("CompletionReport", "CompletionReport", managedName: "CompletionReport"),
            Realms.Schema.Property.ObjectList("SubTasks", "DynamicSubTask", managedName: "SubTasks"),
            Realms.Schema.Property.ObjectList("SubSubTasks", "DynamicSubSubTask", managedName: "SubSubTasks"),
            Realms.Schema.Property.ObjectDictionary("SubTasksDictionary", "DynamicSubTask", managedName: "SubTasksDictionary"),
        }.Build();

        #region IRealmObject implementation

        private IDynamicTaskAccessor? _accessor;

        Realms.IRealmAccessor Realms.IRealmObjectBase.Accessor => Accessor;

        internal IDynamicTaskAccessor Accessor => _accessor ??= new DynamicTaskUnmanagedAccessor(typeof(DynamicTask));

        [IgnoreDataMember, XmlIgnore]
        public bool IsManaged => Accessor.IsManaged;

        [IgnoreDataMember, XmlIgnore]
        public bool IsValid => Accessor.IsValid;

        [IgnoreDataMember, XmlIgnore]
        public bool IsFrozen => Accessor.IsFrozen;

        [IgnoreDataMember, XmlIgnore]
        public Realms.Realm Realm => Accessor.Realm;

        [IgnoreDataMember, XmlIgnore]
        public Realms.Schema.ObjectSchema ObjectSchema => Accessor.ObjectSchema;

        [IgnoreDataMember, XmlIgnore]
        public Realms.DynamicObjectApi DynamicApi => Accessor.DynamicApi;

        [IgnoreDataMember, XmlIgnore]
        public int BacklinksCount => Accessor.BacklinksCount;

        public void SetManagedAccessor(Realms.IRealmAccessor managedAccessor, Realms.Weaving.IRealmObjectHelper? helper = null, bool update = false, bool skipDefaults = false)
        {
            var newAccessor = (IDynamicTaskAccessor)managedAccessor;
            var oldAccessor = _accessor;
            _accessor = newAccessor;

            if (helper != null && oldAccessor != null)
            {
                if (!skipDefaults)
                {
                    newAccessor.SubTasks.Clear();
                    newAccessor.SubSubTasks.Clear();
                    newAccessor.SubTasksDictionary.Clear();
                }

                if(!skipDefaults || oldAccessor.Id != default(string))
                {
                    newAccessor.Id = oldAccessor.Id;
                }
                if(!skipDefaults || oldAccessor.Summary != default(string))
                {
                    newAccessor.Summary = oldAccessor.Summary;
                }
                newAccessor.CompletionReport = oldAccessor.CompletionReport;
                Realms.CollectionExtensions.PopulateCollection(oldAccessor.SubTasks, newAccessor.SubTasks, update, skipDefaults);
                Realms.CollectionExtensions.PopulateCollection(oldAccessor.SubSubTasks, newAccessor.SubSubTasks, update, skipDefaults);
                Realms.CollectionExtensions.PopulateCollection(oldAccessor.SubTasksDictionary, newAccessor.SubTasksDictionary, update, skipDefaults);
            }

            if (_propertyChanged != null)
            {
                SubscribeForNotifications();
            }

            OnManaged();
        }

        #endregion

        /// <summary>
        /// Called when the object has been managed by a Realm.
        /// </summary>
        /// <remarks>
        /// This method will be called either when a managed object is materialized or when an unmanaged object has been
        /// added to the Realm. It can be useful for providing some initialization logic as when the constructor is invoked,
        /// it is not yet clear whether the object is managed or not.
        /// </remarks>
        partial void OnManaged();

        private event PropertyChangedEventHandler? _propertyChanged;

        public event PropertyChangedEventHandler? PropertyChanged
        {
            add
            {
                if (_propertyChanged == null)
                {
                    SubscribeForNotifications();
                }

                _propertyChanged += value;
            }

            remove
            {
                _propertyChanged -= value;

                if (_propertyChanged == null)
                {
                    UnsubscribeFromNotifications();
                }
            }
        }

        /// <summary>
        /// Called when a property has changed on this class.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <remarks>
        /// For this method to be called, you need to have first subscribed to <see cref="PropertyChanged"/>.
        /// This can be used to react to changes to the current object, e.g. raising <see cref="PropertyChanged"/> for computed properties.
        /// </remarks>
        /// <example>
        /// <code>
        /// class MyClass : IRealmObject
        /// {
        ///     public int StatusCodeRaw { get; set; }
        ///     public StatusCodeEnum StatusCode => (StatusCodeEnum)StatusCodeRaw;
        ///     partial void OnPropertyChanged(string propertyName)
        ///     {
        ///         if (propertyName == nameof(StatusCodeRaw))
        ///         {
        ///             RaisePropertyChanged(nameof(StatusCode));
        ///         }
        ///     }
        /// }
        /// </code>
        /// Here, we have a computed property that depends on a persisted one. In order to notify any <see cref="PropertyChanged"/>
        /// subscribers that <c>StatusCode</c> has changed, we implement <see cref="OnPropertyChanged"/> and
        /// raise <see cref="PropertyChanged"/> manually by calling <see cref="RaisePropertyChanged"/>.
        /// </example>
        partial void OnPropertyChanged(string? propertyName);

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            OnPropertyChanged(propertyName);
        }

        private void SubscribeForNotifications()
        {
            Accessor.SubscribeForNotifications(RaisePropertyChanged);
        }

        private void UnsubscribeFromNotifications()
        {
            Accessor.UnsubscribeFromNotifications();
        }

        public static explicit operator DynamicTask(Realms.RealmValue val) => val.AsRealmObject<DynamicTask>();

        public static implicit operator Realms.RealmValue(DynamicTask? val) => val == null ? Realms.RealmValue.Null : Realms.RealmValue.Object(val);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public TypeInfo GetTypeInfo() => Accessor.GetTypeInfo(this);

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is InvalidObject)
            {
                return !IsValid;
            }

            if (obj is not Realms.IRealmObjectBase iro)
            {
                return false;
            }

            return Accessor.Equals(iro.Accessor);
        }

        public override int GetHashCode() => IsManaged ? Accessor.GetHashCode() : base.GetHashCode();

        public override string? ToString() => Accessor.ToString();

        [EditorBrowsable(EditorBrowsableState.Never)]
        private class DynamicTaskObjectHelper : Realms.Weaving.IRealmObjectHelper
        {
            public void CopyToRealm(Realms.IRealmObjectBase instance, bool update, bool skipDefaults)
            {
                throw new InvalidOperationException("This method should not be called for source generated classes.");
            }

            public Realms.ManagedAccessor CreateAccessor() => new DynamicTaskManagedAccessor();

            public Realms.IRealmObjectBase CreateInstance() => new DynamicTask();

            public bool TryGetPrimaryKeyValue(Realms.IRealmObjectBase instance, out object? value)
            {
                value = ((IDynamicTaskAccessor)instance.Accessor).Id;
                return true;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal interface IDynamicTaskAccessor : Realms.IRealmAccessor
        {
            string? Id { get; set; }

            string? Summary { get; set; }

            Realms.Tests.Database.CompletionReport? CompletionReport { get; set; }

            System.Collections.Generic.IList<Realms.Tests.Database.DynamicSubTask> SubTasks { get; }

            System.Collections.Generic.IList<Realms.Tests.Database.DynamicSubSubTask> SubSubTasks { get; }

            System.Collections.Generic.IDictionary<string, Realms.Tests.Database.DynamicSubTask?> SubTasksDictionary { get; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal class DynamicTaskManagedAccessor : Realms.ManagedAccessor, IDynamicTaskAccessor
        {
            public string? Id
            {
                get => (string?)GetValue("Id");
                set => SetValueUnique("Id", value);
            }

            public string? Summary
            {
                get => (string?)GetValue("Summary");
                set => SetValue("Summary", value);
            }

            public Realms.Tests.Database.CompletionReport? CompletionReport
            {
                get => (Realms.Tests.Database.CompletionReport?)GetValue("CompletionReport");
                set => SetValue("CompletionReport", value);
            }

            private System.Collections.Generic.IList<Realms.Tests.Database.DynamicSubTask> _subTasks = null!;
            public System.Collections.Generic.IList<Realms.Tests.Database.DynamicSubTask> SubTasks
            {
                get
                {
                    if (_subTasks == null)
                    {
                        _subTasks = GetListValue<Realms.Tests.Database.DynamicSubTask>("SubTasks");
                    }

                    return _subTasks;
                }
            }

            private System.Collections.Generic.IList<Realms.Tests.Database.DynamicSubSubTask> _subSubTasks = null!;
            public System.Collections.Generic.IList<Realms.Tests.Database.DynamicSubSubTask> SubSubTasks
            {
                get
                {
                    if (_subSubTasks == null)
                    {
                        _subSubTasks = GetListValue<Realms.Tests.Database.DynamicSubSubTask>("SubSubTasks");
                    }

                    return _subSubTasks;
                }
            }

            private System.Collections.Generic.IDictionary<string, Realms.Tests.Database.DynamicSubTask?> _subTasksDictionary = null!;
            public System.Collections.Generic.IDictionary<string, Realms.Tests.Database.DynamicSubTask?> SubTasksDictionary
            {
                get
                {
                    if (_subTasksDictionary == null)
                    {
                        _subTasksDictionary = GetDictionaryValue<Realms.Tests.Database.DynamicSubTask?>("SubTasksDictionary");
                    }

                    return _subTasksDictionary;
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal class DynamicTaskUnmanagedAccessor : Realms.UnmanagedAccessor, IDynamicTaskAccessor
        {
            public override ObjectSchema ObjectSchema => DynamicTask.RealmSchema;

            private string? _id;
            public string? Id
            {
                get => _id;
                set
                {
                    _id = value;
                    RaisePropertyChanged("Id");
                }
            }

            private string? _summary;
            public string? Summary
            {
                get => _summary;
                set
                {
                    _summary = value;
                    RaisePropertyChanged("Summary");
                }
            }

            private Realms.Tests.Database.CompletionReport? _completionReport;
            public Realms.Tests.Database.CompletionReport? CompletionReport
            {
                get => _completionReport;
                set
                {
                    _completionReport = value;
                    RaisePropertyChanged("CompletionReport");
                }
            }

            public System.Collections.Generic.IList<Realms.Tests.Database.DynamicSubTask> SubTasks { get; } = new List<Realms.Tests.Database.DynamicSubTask>();

            public System.Collections.Generic.IList<Realms.Tests.Database.DynamicSubSubTask> SubSubTasks { get; } = new List<Realms.Tests.Database.DynamicSubSubTask>();

            public System.Collections.Generic.IDictionary<string, Realms.Tests.Database.DynamicSubTask?> SubTasksDictionary { get; } = new Dictionary<string, Realms.Tests.Database.DynamicSubTask?>();

            public DynamicTaskUnmanagedAccessor(Type objectType) : base(objectType)
            {
            }

            public override Realms.RealmValue GetValue(string propertyName)
            {
                return propertyName switch
                {
                    "Id" => _id,
                    "Summary" => _summary,
                    "CompletionReport" => _completionReport,
                    _ => throw new MissingMemberException($"The object does not have a gettable Realm property with name {propertyName}"),
                };
            }

            public override void SetValue(string propertyName, Realms.RealmValue val)
            {
                switch (propertyName)
                {
                    case "Id":
                        throw new InvalidOperationException("Cannot set the value of a primary key property with SetValue. You need to use SetValueUnique");
                    case "Summary":
                        Summary = (string?)val;
                        return;
                    case "CompletionReport":
                        CompletionReport = (Realms.Tests.Database.CompletionReport?)val;
                        return;
                    default:
                        throw new MissingMemberException($"The object does not have a settable Realm property with name {propertyName}");
                }
            }

            public override void SetValueUnique(string propertyName, Realms.RealmValue val)
            {
                if (propertyName != "Id")
                {
                    throw new InvalidOperationException($"Cannot set the value of non primary key property ({propertyName}) with SetValueUnique");
                }

                Id = (string?)val;
            }

            public override IList<T> GetListValue<T>(string propertyName)
            {
                return propertyName switch
                            {
                "SubTasks" => (IList<T>)SubTasks,
                "SubSubTasks" => (IList<T>)SubSubTasks,

                                _ => throw new MissingMemberException($"The object does not have a Realm list property with name {propertyName}"),
                            };
            }

            public override ISet<T> GetSetValue<T>(string propertyName)
            {
                throw new MissingMemberException($"The object does not have a Realm set property with name {propertyName}");
            }

            public override IDictionary<string, TValue> GetDictionaryValue<TValue>(string propertyName)
            {
                return propertyName switch
                {
                    "SubTasksDictionary" => (IDictionary<string, TValue>)SubTasksDictionary,
                    _ => throw new MissingMemberException($"The object does not have a Realm dictionary property with name {propertyName}"),
                };
            }
        }
    }
}
