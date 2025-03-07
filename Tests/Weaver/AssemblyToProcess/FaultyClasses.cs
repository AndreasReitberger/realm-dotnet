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
using System.Collections.Generic;
using System.Linq;
using Realms;

namespace AssemblyToProcess
{
    public class LambdaPropertyObject : RealmObject
    {
        public IList<Person> ListProperty { get; }

        public Person FirstPropertyObject => ListProperty.First();

        public int IntProperty => ListProperty.Count();
    }

    public class RealmCollectionsWithCounter : RealmObject
    {
        public int Id { get; set; }

        public IList<RealmInteger<int>> CounterList { get; set; }

        public ISet<RealmInteger<int>> CounterSet { get; set; }

        public IDictionary<string, RealmInteger<int>> CounterDict { get; set; }

    }

    public class RealmListWithSetter : RealmObject
    {
        public IList<Person> People { get; set; }

        public int PropertyToEnsureOtherwiseHealthyClass { get; set; }
    }

    public class RealmSetWithSetter : RealmObject
    {
        public ISet<Person> People { get; set; }

        public int PropertyToEnsureOtherwiseHealthyClass { get; set; }
    }

    public class RealmDictionaryWithSetter : RealmObject
    {
        public IDictionary<string, Person> People { get; set; }

        public int PropertyToEnsureOtherwiseHealthyClass { get; set; }
    }

    public class RealmDictionaryWithNonStringKey : RealmObject
    {
        public IDictionary<int, Person> People { get; }

        public int PropertyToEnsureOtherwiseHealthyClass { get; set; }
    }

    public class EmbeddedWithPrimaryKey : EmbeddedObject
    {
        [PrimaryKey]
        public int NotAllowed { get; set; }
    }

    public class IndexedProperties : RealmObject
    {
        // These should be allowed:

        [Indexed]
        public int IntProperty { get; set; }

        [Indexed]
        public string StringProperty { get; set; }

        [Indexed]
        public bool BooleanProperty { get; set; }

        [Indexed]
        public Guid GuidProperty { get; set; }

        [Indexed]
        public DateTimeOffset DateTimeOffsetProperty { get; set; }

        // This should cause an error:

        [Indexed]
        public float SingleProperty { get; set; }
    }

    public class RequiredProperties : RealmObject
    {
        // These should be allowed:

        [Required]
        public string StringProperty { get; set; }

        [Required]
        public byte[] ByteArrayProperty { get; set; }

        [Required]
        public char? NullableCharProperty { get; set; }

        [Required]
        public byte? NullableByteProperty { get; set; }

        [Required]
        public short? NullableInt16Property { get; set; }

        [Required]
        public int? NullableInt32Property { get; set; }

        [Required]
        public long? NullableInt64Property { get; set; }

        [Required]
        public float? NullableSingleProperty { get; set; }

        [Required]
        public double? NullableDoubleProperty { get; set; }

        [Required]
        public bool? NullableBooleanProperty { get; set; }

        [Required]
        public DateTimeOffset? NullableDateTimeOffsetProperty { get; set; }

        // These should cause an error:

        [Required]
        public char CharProperty { get; set; }

        [Required]
        public byte ByteProperty { get; set; }

        [Required]
        public short Int16Property { get; set; }

        [Required]
        public int Int32Property { get; set; }

        [Required]
        public long Int64Property { get; set; }

        [Required]
        public float SingleProperty { get; set; }

        [Required]
        public double DoubleProperty { get; set; }

        [Required]
        public bool BooleanProperty { get; set; }

        [Required]
        public DateTimeOffset DateTimeOffsetProperty { get; set; }

        [Required]
        public Person ObjectProperty { get; set; }

        [Required]
        public IList<Person> ListProperty { get; }
    }

    public class PrimaryKeyProperties : RealmObject
    {
        // These should be allowed:

        [PrimaryKey]
        public int IntProperty { get; set; }

        [PrimaryKey]
        public string StringProperty { get; set; }

        // These should cause errors:

        [PrimaryKey]
        public bool BooleanProperty { get; set; }

        [PrimaryKey]
        public DateTimeOffset DateTimeOffsetProperty { get; set; }

        [PrimaryKey]
        public float SingleProperty { get; set; }
    }

    // This class has no default constructor which is necessary for Realm.CreateObject<>()
    public class DefaultConstructorMissing : RealmObject
    {
        public DefaultConstructorMissing(int parameter)
        {
        }

        public int PropertyToEnsureOtherwiseHealthyClass { get; set; }
    }

    // This class has no persisted properties. 
    public class NoPersistedProperties : RealmObject
    {
        public int PublicField;

        [Ignored]
        public int IgnoredProperty { get; set; }
    }

    // This class has realm attributes applied on non-persisted properties
    public class IncorrectAttributes : RealmObject
    {
        [PrimaryKey]
        public int AutomaticId { get; }

        [Indexed]
        public DateTimeOffset AutomaticDate { get; }

        [MapTo("Email")]
        public string Email_ { get; }

        [Indexed]
        [MapTo("Date")]
        public DateTimeOffset Date_ { get; }

        public string PersistedProperty { get; set; }
    }

    public class NotSupportedProperties : RealmObject
    {
        public DateTime DateTimeProperty { get; set; }

        public DateTime? NullableDateTimeProperty { get; set; }

        public MyEnum EnumProperty { get; set; }

        public List<Person> People { get; }

        public string PersistedProperty { get; set; }

        public enum MyEnum
        {
            Value1,
            Value2
        }
    }

    public class InvalidBacklinkRelationships : RealmObject
    {
        public int Id { get; set; }

        [Backlink(nameof(ChildRelationShips))]
        public InvalidBacklinkRelationships ParentRelationship { get; set; }

        [Backlink(nameof(ParentRelationship))]
        public IList<InvalidBacklinkRelationships> ChildRelationShips { get; }

        [Backlink(nameof(ParentRelationship))]
        public IQueryable<InvalidBacklinkRelationships> WritableBacklinksProperty { get; set; }

        [Backlink(nameof(Person.PhoneNumbers))]
        public IQueryable<Person> NoSuchRelationshipProperty { get; }

        public IQueryable<Person> BacklinkNotAppliedProperty { get; set; }
    }

    public class AccessorTestObject : RealmObject
    {
        // One normal property to avoid triggering "object has no properties" error
        public int IntProperty { get; set; }

        public AccessorTestObject GetterLessObject { set { } }

        public AccessorTestObject SetterLessObject { get; }

        public string GetterLessString { set { } }

        public string SetterLessString { get; }
    }

    public class Sensor : AsymmetricObject
    {
        [PrimaryKey, MapTo("_id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Backlink(nameof(Measurement.Sensor))]
        public IQueryable<Measurement> Measurements { get; }

        public Measurement FirstMeasurement => Measurements.First();
    }

    public class Coordinates : EmbeddedObject
    {
        public double X { get; set; }

        public double Y { get; set; }

        public Sensor Sensor { get; set; }
    }

    public class Department : EmbeddedObject
    {
        public IList<Sensor> SensorsList { get; }

        public ISet<Sensor> SensorsSet { get; }

        public string Name { get; set; }
    }
}
