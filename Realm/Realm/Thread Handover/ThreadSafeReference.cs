﻿////////////////////////////////////////////////////////////////////////////
//
// Copyright 2017 Realm Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Realms.Exceptions;
using Realms.Helpers;

namespace Realms
{
    /// <summary>
    /// An object intended to be passed between threads containing a thread-safe reference to its
    /// thread-confined object.
    /// <para/>
    /// To resolve a thread-safe reference on a target <see cref="Realm"/> on a different thread, pass it to
    /// <c>Realm.ResolveReference</c>.
    /// </summary>
    /// <remarks>
    /// A <see cref="ThreadSafeReference"/> object must be resolved at most once.
    /// <para/>
    /// Failing to resolve a <see cref="ThreadSafeReference"/> will result in the source version of the
    /// Realm being pinned until the reference is deallocated.
    /// <para/>
    /// Prefer short-lived <see cref="ThreadSafeReference"/>s as the data for the version of the source Realm
    /// will be retained until all references have been resolved or deallocated.
    /// </remarks>
    public abstract class ThreadSafeReference
    {
        internal readonly ThreadSafeReferenceHandle Handle;

        internal readonly Metadata Metadata;

        internal readonly Type ReferenceType;

        internal ThreadSafeReference(IThreadConfined value, Type type)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!value.IsManaged)
            {
                throw new RealmException("Cannot construct reference to unmanaged object, which can be passed across threads directly.");
            }

            if (!value.IsValid)
            {
                throw new RealmException("Cannot construct reference to invalidated object.");
            }

            Handle = value.Handle.GetThreadSafeReference();
            Metadata = value.Metadata;
            ReferenceType = type;
        }

        #region Factory

        /// <summary>
        /// Initializes a new instance of the <see cref="Query{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The thread-confined <see cref="IQueryable{T}"/> to create a thread-safe reference to. It must be a collection,
        /// obtained by calling <see cref="Realm.All"/> or a subsequent LINQ query.
        /// </param>
        /// <typeparam name="T">The type of the <see cref="RealmObject"/> or <see cref="EmbeddedObject"/> contained in the query.</typeparam>
        /// <returns>A <see cref="ThreadSafeReference"/> that can be passed to <see cref="Realm.ResolveReference{T}(Query{T})"/> on a different thread.</returns>
        public static Query<T> Create<T>(IQueryable<T> value)
            => new Query<T>(Argument.EnsureType<RealmResults<T>>(value, "value must be a managed Query (i.e. the result of realm.All<T>).", nameof(value)));

        /// <summary>
        /// Initializes a new instance of the <see cref="Object{T}"/> class.
        /// </summary>
        /// <param name="value">The thread-confined <see cref="RealmObject"/> or <see cref="EmbeddedObject"/> to create a thread-safe reference to.</param>
        /// <typeparam name="T">The type of the <see cref="RealmObject"/>/<see cref="EmbeddedObject"/>.</typeparam>
        /// <returns>A <see cref="ThreadSafeReference"/> that can be passed to <see cref="Realm.ResolveReference{T}(Object{T})"/> on a different thread.</returns>
        public static Object<T> Create<T>(T value)
            where T : IRealmObjectBase
        {
            return new Object<T>(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="List{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The thread-confined <see cref="IList{T}"/> to create a thread-safe reference to. It must be a collection
        /// that is a managed property of a <see cref="RealmObject"/> or an <see cref="EmbeddedObject"/>.
        /// </param>
        /// <typeparam name="T">The type of the objects contained in the list.</typeparam>
        /// <returns>A <see cref="ThreadSafeReference"/> that can be passed to <see cref="Realm.ResolveReference{T}(List{T})"/> on a different thread.</returns>
        public static List<T> Create<T>(IList<T> value)
            => new List<T>(Argument.EnsureType<RealmList<T>>(value, "value must be a managed List (i.e. property of a RealmObject).", nameof(value)));

        /// <summary>
        /// Initializes a new instance of the <see cref="Set{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The thread-confined <see cref="ISet{T}"/> to create a thread-safe reference to. It must be a collection
        /// that is a managed property of a <see cref="RealmObject"/> or an <see cref="EmbeddedObject"/>.
        /// </param>
        /// <typeparam name="T">The type of the objects contained in the set.</typeparam>
        /// <returns>A <see cref="ThreadSafeReference"/> that can be passed to <see cref="Realm.ResolveReference{T}(Set{T})"/> on a different thread.</returns>
        public static Set<T> Create<T>(ISet<T> value)
            => new Set<T>(Argument.EnsureType<RealmSet<T>>(value, "value must be a managed Set (i.e. property of a RealmObject).", nameof(value)));

        /// <summary>
        /// Initializes a new instance of the <see cref="Dictionary{TValue}"/> class.
        /// </summary>
        /// <param name="value">
        /// The thread-confined <see cref="IDictionary{String, TValue}"/> to create a thread-safe reference to. It must be a collection
        /// that is a managed property of a <see cref="RealmObject"/> or an <see cref="EmbeddedObject"/>.
        /// </param>
        /// <typeparam name="TValue">The type of the values contained in the dictionary.</typeparam>
        /// <returns>A <see cref="ThreadSafeReference"/> that can be passed to <see cref="Realm.ResolveReference{TValue}(Dictionary{TValue})"/> on a different thread.</returns>
        public static Dictionary<TValue> Create<TValue>(IDictionary<string, TValue> value)
            => new Dictionary<TValue>(Argument.EnsureType<RealmDictionary<TValue>>(value, "value must be a managed dictionary (i.e. property of a RealmObject).", nameof(value)));

        #endregion

        #region Implementations

        /// <summary>
        /// A reference to a <see cref="IQueryable{T}"/> intended to be passed between threads.
        /// <para/>
        /// To resolve a thread-safe reference on a target <see cref="Realm"/> on a different thread, pass it to
        /// <see cref="Realm.ResolveReference{T}(Query{T})"/>.
        /// </summary>
        /// <remarks>
        /// A <see cref="ThreadSafeReference"/> object must be resolved at most once.
        /// <para/>
        /// Failing to resolve a <see cref="ThreadSafeReference"/> will result in the source version of the
        /// Realm being pinned until the reference is deallocated.
        /// <para/>
        /// Prefer short-lived <see cref="ThreadSafeReference"/>s as the data for the version of the source Realm
        /// will be retained until all references have been resolved or deallocated.
        /// </remarks>
        /// <typeparam name="T">The type of the <see cref="RealmObject"/>/<see cref="EmbeddedObject"/> contained in the query.</typeparam>
        public class Query<T> : ThreadSafeReference
        {
            internal Query(RealmResults<T> value) : base(value, Type.Query)
            {
            }
        }

        /// <summary>
        /// A reference to a <see cref="RealmObject"/> or an <see cref="EmbeddedObject"/> intended to be passed between threads.
        /// <para/>
        /// To resolve a thread-safe reference on a target <see cref="Realm"/> on a different thread, pass it to
        /// <see cref="Realm.ResolveReference{T}(Object{T})"/>.
        /// </summary>
        /// <remarks>
        /// A <see cref="ThreadSafeReference"/> object must be resolved at most once.
        /// <para/>
        /// Failing to resolve a <see cref="ThreadSafeReference"/> will result in the source version of the
        /// Realm being pinned until the reference is deallocated.
        /// <para/>
        /// Prefer short-lived <see cref="ThreadSafeReference"/>s as the data for the version of the source Realm
        /// will be retained until all references have been resolved or deallocated.
        /// </remarks>
        /// <typeparam name="T">The type of the <see cref="RealmObject"/>/<see cref="EmbeddedObject"/>.</typeparam>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "A nested class with generic argument is unlikely to be confused with System.Object.")]
        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is intentional as ThreadSafeReference.Object represents an object.")]
        public class Object<T> : ThreadSafeReference
            where T : IRealmObjectBase
        {
            internal Object(T value) : base(GetThreadConfined(value), Type.Object)
            {
            }

            private static IThreadConfined GetThreadConfined(IRealmObjectBase ro)
            {
                if (!ro.IsManaged)
                {
                    throw new RealmException("Cannot construct reference to unmanaged object, which can be passed across threads directly.");
                }

                return (ManagedAccessor)ro.Accessor;
            }
        }

        /// <summary>
        /// A reference to a <see cref="IList{T}"/> intended to be passed between threads.
        /// <para/>
        /// To resolve a thread-safe reference on a target <see cref="Realm"/> on a different thread, pass it to
        /// <see cref="Realm.ResolveReference{T}(List{T})"/>.
        /// </summary>
        /// <remarks>
        /// A <see cref="ThreadSafeReference"/> object must be resolved at most once.
        /// <para/>
        /// Failing to resolve a <see cref="ThreadSafeReference"/> will result in the source version of the
        /// Realm being pinned until the reference is deallocated.
        /// <para/>
        /// Prefer short-lived <see cref="ThreadSafeReference"/>s as the data for the version of the source Realm
        /// will be retained until all references have been resolved or deallocated.
        /// </remarks>
        /// <typeparam name="T">The type of the objects contained in the list.</typeparam>
        public class List<T> : ThreadSafeReference
        {
            internal List(RealmList<T> value) : base(value, Type.List)
            {
            }
        }

        /// <summary>
        /// A reference to a <see cref="ISet{T}"/> intended to be passed between threads.
        /// <para/>
        /// To resolve a thread-safe reference on a target <see cref="Realm"/> on a different thread, pass it to
        /// <see cref="Realm.ResolveReference{T}(Set{T})"/>.
        /// </summary>
        /// <remarks>
        /// A <see cref="ThreadSafeReference"/> object must be resolved at most once.
        /// <para/>
        /// Failing to resolve a <see cref="ThreadSafeReference"/> will result in the source version of the
        /// Realm being pinned until the reference is deallocated.
        /// <para/>
        /// Prefer short-lived <see cref="ThreadSafeReference"/>s as the data for the version of the source Realm
        /// will be retained until all references have been resolved or deallocated.
        /// </remarks>
        /// <typeparam name="T">The type of the objects contained in the set.</typeparam>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "A nested class with generic argument is unlikely to be confused with a property setter.")]
        public class Set<T> : ThreadSafeReference
        {
            internal Set(RealmSet<T> value) : base(value, Type.Set)
            {
            }
        }

        /// <summary>
        /// A reference to a <see cref="IDictionary{String, TValue}"/> intended to be passed between threads.
        /// <para/>
        /// To resolve a thread-safe reference on a target <see cref="Realm"/> on a different thread, pass it to
        /// <see cref="Realm.ResolveReference{TValue}(Dictionary{TValue})"/>.
        /// </summary>
        /// <remarks>
        /// A <see cref="ThreadSafeReference"/> object must be resolved at most once.
        /// <para/>
        /// Failing to resolve a <see cref="ThreadSafeReference"/> will result in the source version of the
        /// Realm being pinned until the reference is deallocated.
        /// <para/>
        /// Prefer short-lived <see cref="ThreadSafeReference"/>s as the data for the version of the source Realm
        /// will be retained until all references have been resolved or deallocated.
        /// </remarks>
        /// <typeparam name="TValue">The type of the dictionary values.</typeparam>
        public class Dictionary<TValue> : ThreadSafeReference
        {
            internal Dictionary(RealmDictionary<TValue> value) : base(value, Type.Dictionary)
            {
            }
        }

        #endregion

        internal enum Type : byte
        {
            Object,
            List,
            Query,
            Set,
            Dictionary,
        }
    }
}
