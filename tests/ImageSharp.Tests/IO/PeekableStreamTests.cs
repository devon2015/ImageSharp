// <copyright file="PeekableStreamTests.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests.IO
{
    using ImageSharp.IO;
    using System.IO;
    using Xunit;

    /// <summary>
    /// The <see cref="PeekableStream"/> tests.
    /// </summary>
    public class PeekableStreamTests
    {
        private MemoryStream CreateStream(int length)
        {
            var ms = new MemoryStream();
            for (var i = 0; i < length; i++)
            {
                ms.WriteByte((byte)(i % byte.MaxValue));
            }
            ms.Position = 0;
            return ms;
        }

        [Fact]
        public void PeekedBytesAreCanBeRead()
        {
            var srcStream = CreateStream(100);
            var stream = new PeekableStream(srcStream, true);

            //peek 3 bytes indiviually
            var peek1 = stream.PeekByte();
            var peek2 = stream.PeekByte();
            var peek3 = stream.PeekByte();

            // position still at zero
            Assert.Equal(0, stream.Position);

            var read1 = stream.ReadByte();
            var read2 = stream.ReadByte();
            var read3 = stream.ReadByte();

            Assert.Equal(0, read1);
            Assert.Equal(1, read2);
            Assert.Equal(2, read3);

            Assert.Equal(3, stream.Position);
        }

        [Fact]
        public void ReadMoreThanPeekedBytes()
        {
            var srcStream = CreateStream(100);
            var stream = new PeekableStream(srcStream, true);

            var peekbuffer = new byte[10];

            //peek 3 bytes indiviually
            var peekAmount = stream.Peek(peekbuffer, 0, 10);

            // position still at zero
            Assert.Equal(0, stream.Position);

            var readbuffer = new byte[20];
            var readAmount = stream.Read(readbuffer, 0, 20);

            Assert.Equal(10, peekAmount);
            Assert.Equal(20, readAmount);
            Assert.Equal(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 }, readbuffer);
            Assert.Equal(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, peekbuffer);

            Assert.Equal(20, stream.Position);
        }

        [Fact]
        public void PeekedBytesReadBeforeRealBytes()
        {
            var srcStream = CreateStream(100);
            var stream = new PeekableStream(srcStream, true);

            //peek 3 bytes indiviually
            var peek1 = stream.PeekByte();
            var peek2 = stream.PeekByte();
            var peek3 = stream.PeekByte();

            // position still at zero
            Assert.Equal(3, stream.PeekPosition);
            //Assert.Equal(0, stream.Position);

            var read1 = stream.ReadByte();
            var read2 = stream.ReadByte();
            //Assert.Equal(3, stream.PeekPosition);
            //Assert.Equal(2, stream.Position);

            var read3 = stream.ReadByte();
            var read4 = stream.ReadByte();

            //Assert.Equal(4, stream.PeekPosition);
            //Assert.Equal(4, stream.Position);
        }

        [Fact]
        public void ReadPeekRead()
        {
            var srcStream = CreateStream(100);
            var stream = new PeekableStream(srcStream, true);

            var buffer = new byte[3];
            stream.Read(buffer, 0, 3);
            Assert.Equal(new byte[] { 0, 1, 2 }, buffer);
            stream.Peek(buffer, 0, 3);
            Assert.Equal(new byte[] { 3, 4, 5 }, buffer);

            stream.Read(buffer, 0, 3);
            Assert.Equal(new byte[] { 3, 4, 5 }, buffer);
        }
    }
}