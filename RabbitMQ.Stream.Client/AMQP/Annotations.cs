// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 2.0.
// Copyright (c) 2017-2023 Broadcom. All Rights Reserved. The term "Broadcom" refers to Broadcom Inc. and/or its subsidiaries.

using System;
using System.Buffers;

namespace RabbitMQ.Stream.Client.AMQP
{
    public class Annotations : Map<object>
    {
        internal static new Annotations Parse(ref SequenceReader<byte> reader, ref int byteRead)
        {
            return Parse<Annotations>(ref reader, ref byteRead);
        }

        public override int Size
        {
            get => DescribedFormatCode.Size + base.Size;
        }

        public override int Write(Span<byte> span)
        {
            var offset = DescribedFormatCode.Write(span, DescribedFormatCode.MessageAnnotations);
            offset += base.Write(span[offset..]);
            return offset;
        }
    }
}
