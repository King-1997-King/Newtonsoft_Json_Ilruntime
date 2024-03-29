﻿#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;
using System.Globalization;
using Newtonsoft.Json.Shims;

namespace Newtonsoft.Json.Linq
{
    /// <summary>
    /// Represents a value in JSON (string, integer, date, etc).
    /// </summary>
    [Preserve]
    public class JValue : JToken, IFormattable, IComparable
        , IConvertible
    {
        private JTokenType _valueType;
        private object _value;

        internal JValue(object value, JTokenType type)
        {
            _value = value;
            _valueType = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class from another <see cref="JValue"/> object.
        /// </summary>
        /// <param name="other">A <see cref="JValue"/> object to copy from.</param>
        public JValue(JValue other)
            : this(other.Value, other.Type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public JValue(long value)
            : this(value, JTokenType.Integer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public JValue(decimal value)
            : this(value, JTokenType.Float)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public JValue(char value)
            : this(value, JTokenType.String)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [CLSCompliant(false)]
        public JValue(ulong value)
            : this(value, JTokenType.Integer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public JValue(double value)
            : this(value, JTokenType.Float)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public JValue(float value)
            : this(value, JTokenType.Float)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public JValue(DateTime value)
            : this(value, JTokenType.Date)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public JValue(DateTimeOffset value)
            : this(value, JTokenType.Date)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public JValue(bool value)
            : this(value, JTokenType.Boolean)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public JValue(string value)
            : this(value, JTokenType.String)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public JValue(Guid value)
            : this(value, JTokenType.Guid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public JValue(Uri value)
            : this(value, (value != null) ? JTokenType.Uri : JTokenType.Null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public JValue(TimeSpan value)
            : this(value, JTokenType.TimeSpan)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JValue"/> class with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public JValue(object value)
            : this(value, GetValueType(null, value))
        {
        }

        internal override bool DeepEquals(JToken node)
        {
            JValue other = node as JValue;
            if (other == null)
            {
                return false;
            }
            if (other == this)
            {
                return true;
            }

            return ValuesEquals(this, other);
        }

        /// <summary>
        /// Gets a value indicating whether this token has child tokens.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this token has child values; otherwise, <c>false</c>.
        /// </value>
        public override bool HasValues
        {
            get { return false; }
        }


        internal static int Compare(JTokenType valueType, object objA, object objB)
        {
            if (objA == null && objB == null)
            {
                return 0;
            }
            if (objA != null && objB == null)
            {
                return 1;
            }
            if (objA == null && objB != null)
            {
                return -1;
            }

            switch (valueType)
            {
                case JTokenType.Integer:
                    if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
                    {
                        return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
                    }
                    else if (objA is float || objB is float || objA is double || objB is double)
                    {
                        return CompareFloat(objA, objB);
                    }
                    else
                    {
                        return Convert.ToInt64(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToInt64(objB, CultureInfo.InvariantCulture));
                    }
                case JTokenType.Float:
                    return CompareFloat(objA, objB);
                case JTokenType.Comment:
                case JTokenType.String:
                case JTokenType.Raw:
                    string s1 = Convert.ToString(objA, CultureInfo.InvariantCulture);
                    string s2 = Convert.ToString(objB, CultureInfo.InvariantCulture);

                    return string.CompareOrdinal(s1, s2);
                case JTokenType.Boolean:
                    bool b1 = Convert.ToBoolean(objA, CultureInfo.InvariantCulture);
                    bool b2 = Convert.ToBoolean(objB, CultureInfo.InvariantCulture);

                    return b1.CompareTo(b2);
                case JTokenType.Date:
                    if (objA is DateTime)
                    {
                        DateTime date1 = (DateTime)objA;
                        DateTime date2;

                        if (objB is DateTimeOffset)
                        {
                            date2 = ((DateTimeOffset)objB).DateTime;
                        }
                        else
                        {
                            date2 = Convert.ToDateTime(objB, CultureInfo.InvariantCulture);
                        }

                        return date1.CompareTo(date2);
                    }
                    else
                    {
                        DateTimeOffset date1 = (DateTimeOffset)objA;
                        DateTimeOffset date2;

                        if (objB is DateTimeOffset)
                        {
                            date2 = (DateTimeOffset)objB;
                        }
                        else
                        {
                            date2 = new DateTimeOffset(Convert.ToDateTime(objB, CultureInfo.InvariantCulture));
                        }

                        return date1.CompareTo(date2);
                    }
                case JTokenType.Bytes:
                    if (!(objB is byte[]))
                    {
                        throw new ArgumentException("Object must be of type byte[].");
                    }

                    byte[] bytes1 = objA as byte[];
                    byte[] bytes2 = objB as byte[];
                    if (bytes1 == null)
                    {
                        return -1;
                    }
                    if (bytes2 == null)
                    {
                        return 1;
                    }

                    return MiscellaneousUtils.ByteArrayCompare(bytes1, bytes2);
                case JTokenType.Guid:
                    if (!(objB is Guid))
                    {
                        throw new ArgumentException("Object must be of type Guid.");
                    }

                    Guid guid1 = (Guid)objA;
                    Guid guid2 = (Guid)objB;

                    return guid1.CompareTo(guid2);
                case JTokenType.Uri:
                    if (!(objB is Uri))
                    {
                        throw new ArgumentException("Object must be of type Uri.");
                    }

                    Uri uri1 = (Uri)objA;
                    Uri uri2 = (Uri)objB;

                    return Comparer<string>.Default.Compare(uri1.ToString(), uri2.ToString());
                case JTokenType.TimeSpan:
                    if (!(objB is TimeSpan))
                    {
                        throw new ArgumentException("Object must be of type TimeSpan.");
                    }

                    TimeSpan ts1 = (TimeSpan)objA;
                    TimeSpan ts2 = (TimeSpan)objB;

                    return ts1.CompareTo(ts2);
                default:
                    throw MiscellaneousUtils.CreateArgumentOutOfRangeException("valueType", valueType, "Unexpected value type: {0}".FormatWith(CultureInfo.InvariantCulture, valueType));
            }
        }

        private static int CompareFloat(object objA, object objB)
        {
            double d1 = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
            double d2 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);

            // take into account possible floating point errors
            if (MathUtils.ApproxEquals(d1, d2))
            {
                return 0;
            }

            return d1.CompareTo(d2);
        }

        internal override JToken CloneToken()
        {
            return new JValue(this);
        }

        /// <summary>
        /// Creates a <see cref="JValue"/> comment with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="JValue"/> comment with the given value.</returns>
        public static JValue CreateComment(string value)
        {
            return new JValue(value, JTokenType.Comment);
        }

        /// <summary>
        /// Creates a <see cref="JValue"/> string with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="JValue"/> string with the given value.</returns>
        public static JValue CreateString(string value)
        {
            return new JValue(value, JTokenType.String);
        }

        /// <summary>
        /// Creates a <see cref="JValue"/> null value.
        /// </summary>
        /// <returns>A <see cref="JValue"/> null value.</returns>
        public static JValue CreateNull()
        {
            return new JValue(null, JTokenType.Null);
        }

        /// <summary>
        /// Creates a <see cref="JValue"/> undefined value.
        /// </summary>
        /// <returns>A <see cref="JValue"/> undefined value.</returns>
        public static JValue CreateUndefined()
        {
            return new JValue(null, JTokenType.Undefined);
        }

        private static JTokenType GetValueType(JTokenType? current, object value)
        {
            if (value == null)
            {
                return JTokenType.Null;
            }
            else if (value == DBNull.Value)
            {
                return JTokenType.Null;
            }
            else if (value is string)
            {
                return GetStringValueType(current);
            }
            else if (value is long || value is int || value is short || value is sbyte
                     || value is ulong || value is uint || value is ushort || value is byte)
            {
                return JTokenType.Integer;
            }
            else if (value is Enum)
            {
                return JTokenType.Integer;
            }
            else if (value is double || value is float || value is decimal)
            {
                return JTokenType.Float;
            }
            else if (value is DateTime)
            {
                return JTokenType.Date;
            }
            else if (value is DateTimeOffset)
            {
                return JTokenType.Date;
            }
            else if (value is byte[])
            {
                return JTokenType.Bytes;
            }
            else if (value is bool)
            {
                return JTokenType.Boolean;
            }
            else if (value is Guid)
            {
                return JTokenType.Guid;
            }
            else if (value is Uri)
            {
                return JTokenType.Uri;
            }
            else if (value is TimeSpan)
            {
                return JTokenType.TimeSpan;
            }

            throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
        }

        private static JTokenType GetStringValueType(JTokenType? current)
        {
            if (current == null)
            {
                return JTokenType.String;
            }

            switch (current.GetValueOrDefault())
            {
                case JTokenType.Comment:
                case JTokenType.String:
                case JTokenType.Raw:
                    return current.GetValueOrDefault();
                default:
                    return JTokenType.String;
            }
        }

        /// <summary>
        /// Gets the node type for this <see cref="JToken"/>.
        /// </summary>
        /// <value>The type.</value>
        public override JTokenType Type
        {
            get { return _valueType; }
        }

        /// <summary>
        /// Gets or sets the underlying token value.
        /// </summary>
        /// <value>The underlying token value.</value>
        public object Value
        {
            get { return _value; }
            set
            {
                Type currentType = (_value != null) ? _value.GetType() : null;
                Type newType = (value != null) ? value.GetType() : null;

                if (currentType != newType)
                {
                    _valueType = GetValueType(_valueType, value);
                }

                _value = value;
            }
        }

        /// <summary>
        /// Writes this token to a <see cref="JsonWriter"/>.
        /// </summary>
        /// <param name="writer">A <see cref="JsonWriter"/> into which this method will write.</param>
        /// <param name="converters">A collection of <see cref="JsonConverter"/> which will be used when writing the token.</param>
        public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
        {
            if (converters != null && converters.Length > 0 && _value != null)
            {
                JsonConverter matchingConverter = JsonSerializer.GetMatchingConverter(converters, _value.GetType());
                if (matchingConverter != null && matchingConverter.CanWrite)
                {
                    matchingConverter.WriteJson(writer, _value, JsonSerializer.CreateDefault());
                    return;
                }
            }

            switch (_valueType)
            {
                case JTokenType.Comment:
                    writer.WriteComment((_value != null) ? _value.ToString() : null);
                    return;
                case JTokenType.Raw:
                    writer.WriteRawValue((_value != null) ? _value.ToString() : null);
                    return;
                case JTokenType.Null:
                    writer.WriteNull();
                    return;
                case JTokenType.Undefined:
                    writer.WriteUndefined();
                    return;
                case JTokenType.Integer:
                    if (_value is int)
                    {
                        writer.WriteValue((int)_value);
                    }
                    else if (_value is long)
                    {
                        writer.WriteValue((long)_value);
                    }
                    else if (_value is ulong)
                    {
                        writer.WriteValue((ulong)_value);
                    }
                    else
                    {
                        writer.WriteValue(Convert.ToInt64(_value, CultureInfo.InvariantCulture));
                    }
                    return;
                case JTokenType.Float:
                    if (_value is decimal)
                    {
                        writer.WriteValue((decimal)_value);
                    }
                    else if (_value is double)
                    {
                        writer.WriteValue((double)_value);
                    }
                    else if (_value is float)
                    {
                        writer.WriteValue((float)_value);
                    }
                    else
                    {
                        writer.WriteValue(Convert.ToDouble(_value, CultureInfo.InvariantCulture));
                    }
                    return;
                case JTokenType.String:
                    writer.WriteValue((_value != null) ? _value.ToString() : null);
                    return;
                case JTokenType.Boolean:
                    writer.WriteValue(Convert.ToBoolean(_value, CultureInfo.InvariantCulture));
                    return;
                case JTokenType.Date:
                    if (_value is DateTimeOffset)
                    {
                        writer.WriteValue((DateTimeOffset)_value);
                    }
                    else
                    {
                        writer.WriteValue(Convert.ToDateTime(_value, CultureInfo.InvariantCulture));
                    }
                    return;
                case JTokenType.Bytes:
                    writer.WriteValue((byte[])_value);
                    return;
                case JTokenType.Guid:
                    writer.WriteValue((_value != null) ? (Guid?)_value : null);
                    return;
                case JTokenType.TimeSpan:
                    writer.WriteValue((_value != null) ? (TimeSpan?)_value : null);
                    return;
                case JTokenType.Uri:
                    writer.WriteValue((Uri)_value);
                    return;
            }

            throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", _valueType, "Unexpected token type.");
        }

        internal override int GetDeepHashCode()
        {
            int valueHashCode = (_value != null) ? _value.GetHashCode() : 0;

            // GetHashCode on an enum boxes so cast to int
            return ((int)_valueType).GetHashCode() ^ valueHashCode;
        }

        private static bool ValuesEquals(JValue v1, JValue v2)
        {
            return (v1 == v2 || (v1._valueType == v2._valueType && Compare(v1._valueType, v1._value, v2._value) == 0));
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(JValue other)
        {
            if (other == null)
            {
                return false;
            }

            return ValuesEquals(this, other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            JValue otherValue = obj as JValue;
            if (otherValue != null)
            {
                return Equals(otherValue);
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            if (_value == null)
            {
                return 0;
            }

            return _value.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (_value == null)
            {
                return string.Empty;
            }

            return _value.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (_value == null)
            {
                return string.Empty;
            }

            IFormattable formattable = _value as IFormattable;
            if (formattable != null)
            {
                return formattable.ToString(format, formatProvider);
            }
            else
            {
                return _value.ToString();
            }
        }


        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            object otherValue = (obj is JValue) ? ((JValue)obj).Value : obj;

            return Compare(_valueType, _value, otherValue);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This instance is less than <paramref name="obj"/>.
        /// Zero
        /// This instance is equal to <paramref name="obj"/>.
        /// Greater than zero
        /// This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="obj"/> is not the same type as this instance.
        /// </exception>
        public int CompareTo(JValue obj)
        {
            if (obj == null)
            {
                return 1;
            }

            return Compare(_valueType, _value, obj._value);
        }

        TypeCode IConvertible.GetTypeCode()
        {
            if (_value == null)
            {
                return TypeCode.Empty;
            }

            IConvertible convertable = _value as IConvertible;

            if (convertable == null)
            {
                return TypeCode.Object;
            }

            return convertable.GetTypeCode();
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return (bool)this;
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return (char)this;
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return (sbyte)this;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return (byte)this;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (short)this;
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return (ushort)this;
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return (int)this;
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return (uint)this;
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return (long)this;
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return (ulong)this;
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return (float)this;
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return (double)this;
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return (decimal)this;
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return (DateTime)this;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return ToObject(conversionType);
        }
    }
}
