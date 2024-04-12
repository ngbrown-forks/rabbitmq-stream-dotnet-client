// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 2.0.
// Copyright (c) 2017-2023 Broadcom. All Rights Reserved. The term "Broadcom" refers to Broadcom Inc. and/or its subsidiaries.

using System;
using System.Buffers;

namespace RabbitMQ.Stream.Client.AMQP
{
    public class ApplicationProperties : Map<string>
    {
        internal static new ApplicationProperties Parse(ref SequenceReader<byte> reader, ref int byteRead)
        {
            return Parse<ApplicationProperties>(ref reader, ref byteRead);
        }

        public override int Size
        {
            get => DescribedFormatCode.Size + base.Size;
        }

        public override int Write(Span<byte> span)
        {
            var offset = DescribedFormatCode.Write(span, DescribedFormatCode.ApplicationProperties);
            offset += base.Write(span[offset..]);
            return offset;
        }
    }
}
