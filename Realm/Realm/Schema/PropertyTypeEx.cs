////////////////////////////////////////////////////////////////////////////
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
using MongoDB.Bson;
using Realms.Helpers;

namespace Realms.Schema
{
    internal static class PropertyTypeEx
    {
        private static readonly HashSet<Type> _integerTypes =
            new HashSet<Type> { typeof(char), typeof(byte), typeof(short), typeof(int), typeof(long) };

        public static bool IsRealmInteger(this Type type)
        {
            if (type.IsClosedGeneric(typeof(RealmInteger<>), out var typeArguments))
            {
                return IsRealmInteger(typeArguments.Single());
            }

            return _integerTypes.Contains(type);
        }

        public static PropertyType ToPropertyType(this Type type, out Type objectType)
        {
            Argument.NotNull(type, nameof(type));

            objectType = null;
            PropertyType nullabilityModifier = default;

            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
            {
                type = nullableType;
                nullabilityModifier = PropertyType.Nullable;
            }

            switch (type)
            {
                case Type _ when type.IsRealmInteger():
                    return PropertyType.Int | nullabilityModifier;

                case Type _ when type == typeof(bool):
                    return PropertyType.Bool | nullabilityModifier;

                case Type _ when type == typeof(string):
                    return PropertyType.String | PropertyType.Nullable;

                case Type _ when type == typeof(byte[]):
                    return PropertyType.Data | PropertyType.Nullable;

                case Type _ when type == typeof(DateTimeOffset):
                    return PropertyType.Date | nullabilityModifier;

                case Type _ when type == typeof(float):
                    return PropertyType.Float | nullabilityModifier;

                case Type _ when type == typeof(double):
                    return PropertyType.Double | nullabilityModifier;

                case Type _ when type == typeof(decimal) || type == typeof(Decimal128):
                    return PropertyType.Decimal | nullabilityModifier;

                case Type _ when type == typeof(ObjectId):
                    return PropertyType.ObjectId | nullabilityModifier;

                case Type _ when type == typeof(Guid):
                    return PropertyType.Guid | nullabilityModifier;

                case Type _ when type == typeof(RealmValue):
                    return PropertyType.RealmValue | PropertyType.Nullable;

                case Type _ when type.IsRealmObject() || type.IsEmbeddedObject():
                    objectType = type;
                    return PropertyType.Object | PropertyType.Nullable;

                case Type _ when type.IsClosedGeneric(typeof(IList<>), out var typeArguments):
                    var listResult = PropertyType.Array | typeArguments.Single().ToPropertyType(out objectType);

                    if (listResult.HasFlag(PropertyType.Object))
                    {
                        // List<Object> can't contain nulls
                        listResult &= ~PropertyType.Nullable;
                    }

                    return listResult;

                case Type _ when type.IsClosedGeneric(typeof(ISet<>), out var typeArguments):
                    var setResult = PropertyType.Set | typeArguments.Single().ToPropertyType(out objectType);

                    if (setResult.HasFlag(PropertyType.Object))
                    {
                        // Set<Object> can't contain nulls
                        setResult &= ~PropertyType.Nullable;
                    }

                    return setResult;
                case Type _ when type.IsClosedGeneric(typeof(IDictionary<,>), out var typeArguments):
                    return PropertyType.Dictionary | typeArguments.Last().ToPropertyType(out objectType);
                case Type _ when type.IsClosedGeneric(typeof(KeyValuePair<,>), out var typeArguments):
                    return typeArguments.Last().ToPropertyType(out objectType);
                default:
                    throw new ArgumentException($"The property type {type.Name} cannot be expressed as a Realm schema type", nameof(type));
            }
        }

        public static Type ToType(this PropertyType type)
        {
            return type switch
            {
                PropertyType.Int => typeof(long),
                PropertyType.Bool => typeof(bool),
                PropertyType.String => typeof(string),
                PropertyType.Data => typeof(byte[]),
                PropertyType.Date => typeof(DateTimeOffset),
                PropertyType.Float => typeof(float),
                PropertyType.Double => typeof(double),
                PropertyType.Object => typeof(IRealmObjectBase),
                PropertyType.ObjectId => typeof(ObjectId),
                PropertyType.Decimal => typeof(Decimal128),
                PropertyType.Guid => typeof(Guid),
                PropertyType.NullableInt => typeof(long?),
                PropertyType.NullableBool => typeof(bool?),
                PropertyType.NullableString => typeof(string),
                PropertyType.NullableData => typeof(byte[]),
                PropertyType.NullableDate => typeof(DateTimeOffset?),
                PropertyType.NullableFloat => typeof(float?),
                PropertyType.NullableDouble => typeof(double?),
                PropertyType.NullableObjectId => typeof(ObjectId?),
                PropertyType.NullableDecimal => typeof(Decimal128?),
                PropertyType.NullableGuid => typeof(Guid?),
                PropertyType.RealmValue | PropertyType.Nullable => typeof(RealmValue),
                _ => throw new NotSupportedException($"Unexpected property type: {type}"),
            };
        }

        public static RealmValueType ToRealmValueType(this PropertyType type)
        {
            return type.UnderlyingType() switch
            {
                PropertyType.Int => RealmValueType.Int,
                PropertyType.Bool => RealmValueType.Bool,
                PropertyType.String => RealmValueType.String,
                PropertyType.Data => RealmValueType.Data,
                PropertyType.Date => RealmValueType.Date,
                PropertyType.Float => RealmValueType.Float,
                PropertyType.Double => RealmValueType.Double,
                PropertyType.Object => RealmValueType.Object,
                PropertyType.ObjectId => RealmValueType.ObjectId,
                PropertyType.Decimal => RealmValueType.Decimal128,
                PropertyType.Guid => RealmValueType.Guid,
                _ => throw new NotSupportedException($"The type {type} can't be mapped to RealmValueType."),
            };
        }

        public static PropertyType ToPropertyType(this RealmValueType type, bool isNullable)
        {
            var nullabilityModifier = isNullable ? PropertyType.Nullable : default;

            return nullabilityModifier | type switch
            {
                RealmValueType.Int => PropertyType.Int,
                RealmValueType.Bool => PropertyType.Bool,
                RealmValueType.String => PropertyType.String,
                RealmValueType.Data => PropertyType.Data,
                RealmValueType.Date => PropertyType.Date,
                RealmValueType.Float => PropertyType.Float,
                RealmValueType.Double => PropertyType.Double,
                RealmValueType.Object => PropertyType.Object,
                RealmValueType.ObjectId => PropertyType.ObjectId,
                RealmValueType.Decimal128 => PropertyType.Decimal,
                RealmValueType.Guid => PropertyType.Guid,
                _ => throw new NotSupportedException($"Type {type} can't be converted to {nameof(PropertyType)}"),
            };
        }

        public static bool IsComputed(this PropertyType propertyType) => propertyType == (PropertyType.LinkingObjects | PropertyType.Array);

        public static bool IsNullable(this PropertyType propertyType) => propertyType.HasFlag(PropertyType.Nullable);

        public static bool IsList(this PropertyType propertyType) => propertyType.HasFlag(PropertyType.Array) && !propertyType.IsComputed();

        public static bool IsSet(this PropertyType propertyType) => propertyType.HasFlag(PropertyType.Set);

        public static bool IsDictionary(this PropertyType propertyType) => propertyType.HasFlag(PropertyType.Dictionary);

        public static bool IsCollection(this PropertyType propertyType, out PropertyType collection)
        {
            if (propertyType.IsList())
            {
                collection = PropertyType.Array;
            }
            else if (propertyType.IsSet())
            {
                collection = PropertyType.Set;
            }
            else if (propertyType.IsDictionary())
            {
                collection = PropertyType.Dictionary;
            }
            else
            {
                collection = default;
            }

            return collection != default;
        }

        public static bool IsRealmValue(this PropertyType propertyType) => propertyType == (PropertyType.RealmValue | PropertyType.Nullable);

        public static string GetDotnetTypeName(this Property property)
        {
            _ = property.Type.IsCollection(out var collection);
            var format = collection switch
            {
                PropertyType.Array => "IList<{0}>",
                PropertyType.Set => "ISet<{0}>",
                PropertyType.Dictionary => "IDictionary<string, {0}>",
                _ => "{0}"
            };

            if (property.Type.IsComputed())
            {
                format = "IQueryable<{0}>";
            }

            var elementType = property.Type & ~(PropertyType.Array | PropertyType.Set | PropertyType.Dictionary);
            return string.Format(format, property.ObjectType ?? elementType.ToType().Name);
        }

        public static PropertyType UnderlyingType(this PropertyType propertyType) => propertyType & ~PropertyType.Flags;
    }
}
