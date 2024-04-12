// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 2.0.
// Copyright (c) 2017-2023 Broadcom. All Rights Reserved. The term "Broadcom" refers to Broadcom Inc. and/or its subsidiaries.

using System.Buffers;

namespace RabbitMQ.Stream.Client.AMQP
{
    public class Annotations : Map<object>
    {
        internal static Annotations Parse(ref SequenceReader<byte> reader, ref int byteRead)
        {
            return Parse<Annotations>(ref reader, ref byteRead);
        }

        public Annotations()
        {
            MapDataCode = DescribedFormatCode.MessageAnnotations;
        }
    }
}
