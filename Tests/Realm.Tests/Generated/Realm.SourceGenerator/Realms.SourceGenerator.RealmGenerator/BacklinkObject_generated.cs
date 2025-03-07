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
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestAsymmetricObject = Realms.IAsymmetricObject;
using TestEmbeddedObject = Realms.IEmbeddedObject;
using TestRealmObject = Realms.IRealmObject;

namespace Realms.Tests.Database
{
    [Generated]
    [Woven(typeof(BacklinkObjectObjectHelper))]
    public partial class BacklinkObject : IRealmObject, INotifyPropertyChanged, IReflectableType
    {
        public static Realms.Schema.ObjectSchema RealmSchema = new Realms.Schema.ObjectSchema.Builder("BacklinkObject", ObjectSchema.ObjectType.RealmObject)
        {
            Realms.Schema.Property.Primitive("BeforeBacklinks", Realms.RealmValueType.String, isPrimaryKey: false, isIndexed: false, isNullable: true, managedName: "BeforeBacklinks"),
            Realms.Schema.Property.Backlinks("Links", "SomeClass", "BacklinkObject", managedName: "Links"),
            Realms.Schema.Property.Primitive("AfterBacklinks", Realms.RealmValueType.String, isPrimaryKey: false, isIndexed: false, isNullable: true, managedName: "AfterBacklinks"),
        }.Build();

        #region IRealmObject implementation

        private IBacklinkObjectAccessor? _accessor;

        Realms.IRealmAccessor Realms.IRealmObjectBase.Accessor => Accessor;

        internal IBacklinkObjectAccessor Accessor => _accessor ??= new BacklinkObjectUnmanagedAccessor(typeof(BacklinkObject));

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
            var newAccessor = (IBacklinkObjectAccessor)managedAccessor;
            var oldAccessor = _accessor;
            _accessor = newAccessor;

            if (helper != null && oldAccessor != null)
            {
                if(!skipDefaults || oldAccessor.BeforeBacklinks != default(string))
                {
                    newAccessor.BeforeBacklinks = oldAccessor.BeforeBacklinks;
                }
                if(!skipDefaults || oldAccessor.AfterBacklinks != default(string))
                {
                    newAccessor.AfterBacklinks = oldAccessor.AfterBacklinks;
                }
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

        public static explicit operator BacklinkObject(Realms.RealmValue val) => val.AsRealmObject<BacklinkObject>();

        public static implicit operator Realms.RealmValue(BacklinkObject? val) => val == null ? Realms.RealmValue.Null : Realms.RealmValue.Object(val);

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
        private class BacklinkObjectObjectHelper : Realms.Weaving.IRealmObjectHelper
        {
            public void CopyToRealm(Realms.IRealmObjectBase instance, bool update, bool skipDefaults)
            {
                throw new InvalidOperationException("This method should not be called for source generated classes.");
            }

            public Realms.ManagedAccessor CreateAccessor() => new BacklinkObjectManagedAccessor();

            public Realms.IRealmObjectBase CreateInstance() => new BacklinkObject();

            public bool TryGetPrimaryKeyValue(Realms.IRealmObjectBase instance, out object? value)
            {
                value = null;
                return false;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal interface IBacklinkObjectAccessor : Realms.IRealmAccessor
        {
            string? BeforeBacklinks { get; set; }

            System.Linq.IQueryable<Realms.Tests.Database.SomeClass> Links { get; }

            string? AfterBacklinks { get; set; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal class BacklinkObjectManagedAccessor : Realms.ManagedAccessor, IBacklinkObjectAccessor
        {
            public string? BeforeBacklinks
            {
                get => (string?)GetValue("BeforeBacklinks");
                set => SetValue("BeforeBacklinks", value);
            }

            private System.Linq.IQueryable<Realms.Tests.Database.SomeClass> _links = null!;
            public System.Linq.IQueryable<Realms.Tests.Database.SomeClass> Links
            {
                get
                {
                    if (_links == null)
                    {
                        _links = GetBacklinks<Realms.Tests.Database.SomeClass>("Links");
                    }

                    return _links;
                }
            }

            public string? AfterBacklinks
            {
                get => (string?)GetValue("AfterBacklinks");
                set => SetValue("AfterBacklinks", value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal class BacklinkObjectUnmanagedAccessor : Realms.UnmanagedAccessor, IBacklinkObjectAccessor
        {
            public override ObjectSchema ObjectSchema => BacklinkObject.RealmSchema;

            private string? _beforeBacklinks;
            public string? BeforeBacklinks
            {
                get => _beforeBacklinks;
                set
                {
                    _beforeBacklinks = value;
                    RaisePropertyChanged("BeforeBacklinks");
                }
            }

            public System.Linq.IQueryable<Realms.Tests.Database.SomeClass> Links => throw new NotSupportedException("Using backlinks is only possible for managed(persisted) objects.");

            private string? _afterBacklinks;
            public string? AfterBacklinks
            {
                get => _afterBacklinks;
                set
                {
                    _afterBacklinks = value;
                    RaisePropertyChanged("AfterBacklinks");
                }
            }

            public BacklinkObjectUnmanagedAccessor(Type objectType) : base(objectType)
            {
            }

            public override Realms.RealmValue GetValue(string propertyName)
            {
                return propertyName switch
                {
                    "BeforeBacklinks" => _beforeBacklinks,
                    "Links" => throw new NotSupportedException("Using backlinks is only possible for managed(persisted) objects."),
                    "AfterBacklinks" => _afterBacklinks,
                    _ => throw new MissingMemberException($"The object does not have a gettable Realm property with name {propertyName}"),
                };
            }

            public override void SetValue(string propertyName, Realms.RealmValue val)
            {
                switch (propertyName)
                {
                    case "BeforeBacklinks":
                        BeforeBacklinks = (string?)val;
                        return;
                    case "AfterBacklinks":
                        AfterBacklinks = (string?)val;
                        return;
                    default:
                        throw new MissingMemberException($"The object does not have a settable Realm property with name {propertyName}");
                }
            }

            public override void SetValueUnique(string propertyName, Realms.RealmValue val)
            {
                throw new InvalidOperationException("Cannot set the value of an non primary key property with SetValueUnique");
            }

            public override IList<T> GetListValue<T>(string propertyName)
            {
                throw new MissingMemberException($"The object does not have a Realm list property with name {propertyName}");
            }

            public override ISet<T> GetSetValue<T>(string propertyName)
            {
                throw new MissingMemberException($"The object does not have a Realm set property with name {propertyName}");
            }

            public override IDictionary<string, TValue> GetDictionaryValue<TValue>(string propertyName)
            {
                throw new MissingMemberException($"The object does not have a Realm dictionary property with name {propertyName}");
            }
        }
    }
}
