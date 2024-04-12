﻿// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 2.0.
// Copyright (c) 2017-2023 Broadcom. All Rights Reserved. The term "Broadcom" refers to Broadcom Inc. and/or its subsidiaries.

using System;
using System.Buffers;
using System.Collections.Generic;

namespace RabbitMQ.Stream.Client.AMQP
{
    public class Map<TKey> : Dictionary<TKey, object>, IWritable where TKey : class
    {
        internal static T Parse<T>(ref SequenceReader<byte> reader, ref int byteRead) where T : Map<TKey>, new()
        {
            var offset = AmqpWireFormatting.ReadMapHeader(ref reader, out var count);
            var amqpMap = new T();
            var values = (count / 2);
            for (var i = 0; i < values; i++)
            {
                offset += AmqpWireFormatting.ReadAny(ref reader, out var anyTypeKey);
                offset += AmqpWireFormatting.ReadAny(ref reader, out var value);
                var key = anyTypeKey as TKey;
                if (!IsNullOrEmptyString(key)) // this should never occur because we never write null keys
                {
                    amqpMap[key!] = value;
                }
            }

            byteRead += offset;
            return amqpMap;
        }

        internal static Map<TKey> Parse(ref SequenceReader<byte> reader, ref int byteRead)
        {
            return Parse<Map<TKey>>(ref reader, ref byteRead);
        }

        private int MapSize()
        {
            var size = 0;
            foreach (var (key, value) in this)
            {
                if (!IsNullOrEmptyString(key))
                {
                    size += AmqpWireFormatting.GetAnySize(key);
                    size += AmqpWireFormatting.GetAnySize(value);
                }
            }

            return size;
        }

        private static bool IsNullOrEmptyString([System.Diagnostics.CodeAnalysis.NotNullWhen(false)] object value)
        {
            return value switch
            {
                null => true,
                string s => string.IsNullOrEmpty(s),
                _ => false
            };
        }

        public virtual int Size
        {
            get
            {
                var size = sizeof(byte); //FormatCode.List32
                size += sizeof(uint); // field numbers
                size += sizeof(uint); // PropertySize
                size += MapSize();
                return size;
            }
        }

        public virtual int Write(Span<byte> span)
        {
            var offset = WireFormatting.WriteByte(span, FormatCode.Map32);
            offset += WireFormatting.WriteUInt32(span[offset..],
                (uint)MapSize() + DescribedFormatCode.Size + sizeof(byte)); // MapSize  + DescribedFormatCode + FormatCode
            offset += WireFormatting.WriteUInt32(span[offset..], (uint)Count * 2); // pair values
            foreach (var (key, value) in this)
            {
                if (!IsNullOrEmptyString(key))
                {
                    offset += AmqpWireFormatting.WriteAny(span[offset..], key);
                    offset += AmqpWireFormatting.WriteAny(span[offset..], value);
                }
            }

            return offset;
        }
    }
}
