﻿////////////////////////////////////////////////////////////////////////////
//
// Copyright 2016 Realm Inc.
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
using System.Threading;
using System.Threading.Tasks;
using Realms.Native;
using Realms.Schema;

namespace Realms
{
    /// <summary>
    /// A Realm configuration specifying settings for an in-memory Realm. When all in-memory instances with the
    /// same identifier are disposed or go out of scope, all data in that Realm is deleted.
    /// </summary>
    public class InMemoryConfiguration : RealmConfigurationBase
    {
        /// <summary>
        /// Gets a value indicating the identifier of the Realm that will be opened with this <see cref="InMemoryConfiguration"/>.
        /// </summary>
        /// <value>The identifier for this configuration.</value>
        public string Identifier { get; }

        /// <inheritdoc/>
        [Obsolete("Encryption is not supported for in-memory realms. This property will be removed in a future version.")]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override byte[] EncryptionKey
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            get => base.EncryptionKey;
            set => throw new NotSupportedException("Encryption is not supported for in-memory realms");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryConfiguration"/> class with a specified identifier.
        /// </summary>
        /// <param name="identifier">A string that will uniquely identify this in-memory Realm.</param>
        /// <remarks>
        /// Different instances with the same identifier will see the same data.
        /// When all instances with a particular identifier have been removed, the data will be deleted and no longer accessible.
        /// The identifier must not be the same as the file name of a persisted Realm.
        /// </remarks>
        public InMemoryConfiguration(string identifier) : base(identifier)
        {
            Identifier = identifier;
        }

        internal override SharedRealmHandle CreateHandle(RealmSchema schema) => SharedRealmHandle.Open(CreateNativeConfiguration(), schema, EncryptionKey);

        internal override Configuration CreateNativeConfiguration()
        {
            var result = base.CreateNativeConfiguration();
            result.in_memory = true;
            return result;
        }

        internal override Task<SharedRealmHandle> CreateHandleAsync(RealmSchema schema, CancellationToken cancellationToken)
            => Task.FromResult(CreateHandle(schema));
    }
}
