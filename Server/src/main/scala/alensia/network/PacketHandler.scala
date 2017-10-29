package alensia.network

import akka.util.ByteString

sealed abstract class PacketHandler(val id: Int) {

  def headerSize: Int = 1

  def header(data: ByteString): ByteString = data.take(headerSize)

  def payload(data: ByteString): ByteString = data.drop(headerSize)

  override def toString: String = getClass.getSimpleName.dropRight(1)
}

object PacketHandler {

  private val handlers: Map[Int, PacketHandler] = {
    val list = Seq(
      Unreliable,
      Reliable,
      Sequenced,
      ReliableOrdered,
      AckReliable,
      AckReliableOrdered,
      Ping,
      Pong,
      ConnectRequest,
      ConnectAccept,
      Disconnect,
      UnconnectedMessage,
      NatIntroductionRequest,
      NatIntroduction,
      NatPunchMessage,
      MtuCheck,
      MtuOk,
      DiscoveryRequest,
      DiscoveryResponse,
      Merged
    )

    list.map(i => i.id -> i).toMap
  }

  def apply(data: ByteString, offset: Int = 0): PacketHandler = {
    handlers.getOrElse(data(offset) & 0x7F, Unknown)
  }

  trait SequencedPacket extends PacketHandler {

    import SequencedPacket._

    def sequence(data: ByteString): Short = BitConverter.readShort(data, 1)

    def relativeSequence(number: Int, expected: Int): Int =
      (number - expected + MaxSequence + HalfMaxSequence) % MaxSequence - HalfMaxSequence

    override def headerSize: Int = 3
  }

  object SequencedPacket {

    val MaxSequence: Int = 32768

    val HalfMaxSequence: Int = MaxSequence / 2
  }

  trait FragmentedPacketHandler extends PacketHandler {

    def fragmentId: Short = ???

    def fragmentPart: Short = ???

    def fragmentTotal: Short = ???
  }

  trait ClientDataPacket extends PacketHandler

  object Unreliable extends PacketHandler(0) with ClientDataPacket
  object Reliable extends PacketHandler(1) with SequencedPacket with ClientDataPacket
  object Sequenced extends PacketHandler(2) with SequencedPacket with ClientDataPacket
  object ReliableOrdered extends PacketHandler(3) with SequencedPacket with ClientDataPacket

  object AckReliable extends PacketHandler(4) with SequencedPacket
  object AckReliableOrdered extends PacketHandler(5) with SequencedPacket

  object Ping extends PacketHandler(6) with SequencedPacket
  object Pong extends PacketHandler(7) with SequencedPacket {

    def apply(sequence: Short): ByteString = id.toByte +: BitConverter.writeShort(sequence)
  }

  object ConnectRequest extends PacketHandler(8) {

    def connectionKey(data: ByteString): String = data.drop(13).utf8String

    def connectionId(data: ByteString): Long = BitConverter.readLong(data.drop(5))
  }

  object ConnectAccept extends PacketHandler(9) {

    def apply(connectionId: Long): ByteString = id.toByte +: BitConverter.writeLong(connectionId)
  }

  object Disconnect extends PacketHandler(10)
  object UnconnectedMessage extends PacketHandler(11)

  object NatIntroductionRequest extends PacketHandler(12)
  object NatIntroduction extends PacketHandler(13)
  object NatPunchMessage extends PacketHandler(14)

  object MtuCheck extends PacketHandler(15)
  object MtuOk extends PacketHandler(16)

  object DiscoveryRequest extends PacketHandler(17)
  object DiscoveryResponse extends PacketHandler(18)

  object Merged extends PacketHandler(19)

  object Unknown extends PacketHandler(-1)
}
