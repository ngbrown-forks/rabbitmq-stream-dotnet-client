// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 2.0.
// Copyright (c) 2017-2023 Broadcom. All Rights Reserved. The term "Broadcom" refers to Broadcom Inc. and/or its subsidiaries.

using System.Buffers;

namespace RabbitMQ.Stream.Client.AMQP
{
    public class ApplicationProperties : Map<string>
    {
        internal static ApplicationProperties Parse(ref SequenceReader<byte> reader, ref int byteRead)
        {
            return Parse<ApplicationProperties>(ref reader, ref byteRead);
        }

        public ApplicationProperties()
        {
            MapDataCode = DescribedFormatCode.ApplicationProperties;
        }
    }
}
