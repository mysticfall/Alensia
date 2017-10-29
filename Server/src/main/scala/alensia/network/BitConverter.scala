package alensia.network

import java.nio.{ ByteBuffer, ByteOrder }

import akka.util.ByteString

object BitConverter {

  def readShort(buffer: ByteString, index: Int = 0): Short = read(buffer, index)(2, _.getShort)

  def readInt(buffer: ByteString, index: Int = 0): Int = read(buffer, index)(4, _.getInt)

  def readLong(buffer: ByteString, index: Int = 0): Long = read(buffer, index)(8, _.getLong)

  private def read[A](buffer: ByteString, index: Int = 0)(size: Int, reader: ByteBuffer => A): A = {
    reader(buffer.drop(index).take(size).asByteBuffer.order(ByteOrder.LITTLE_ENDIAN))
  }

  def writeShort(data: Short): ByteString = write(data)(2, _.putShort)

  def writeShort(buffer: ByteString, data: Short, index: Int = 0): ByteString =
    write(buffer, data, index)(2, _.putShort)

  def writeInt(data: Int): ByteString = write(data)(4, _.putInt)

  def writeInt(buffer: ByteString, data: Int, index: Int = 0): ByteString =
    write(buffer, data, index)(4, _.putInt)

  def writeLong(data: Long): ByteString = write(data)(8, _.putLong)

  def writeLong(buffer: ByteString, data: Long, index: Int = 0): ByteString =
    write(buffer, data, index)(8, _.putLong)

  private def write[A](data: A)(size: Int, writer: ByteBuffer => A => ByteBuffer): ByteString = {
    val buffer = writer(ByteBuffer.allocate(size).order(ByteOrder.LITTLE_ENDIAN))(data)

    buffer.rewind()

    ByteString(buffer)
  }

  private def write[A](buffer: ByteString, data: A, index: Int)(
    size: Int, writer: ByteBuffer => A => ByteBuffer): ByteString = {

    val head = buffer.take(index)
    val tail = buffer.drop(index + size)

    head ++ write(data)(size, writer) ++ tail
  }
}
