package alensia

/**
 * The network stack is a port of LiteNetLib library on Scala/Akka.
 * @see https://github.com/RevenantX/LiteNetLib
 */
package object network {

  val MaxUdpHeaderSize = 68

  val SupportedMtu = Seq(
    576 - MaxUdpHeaderSize, // Internet Path MTU for X.25 (RFC 879)
    1492 - MaxUdpHeaderSize, // Ethernet with LLC and SNAP, PPPoE (RFC 1042)
    1500 - MaxUdpHeaderSize, // Ethernet II (RFC 1191)
    4352 - MaxUdpHeaderSize, // FDDI
    4464 - MaxUdpHeaderSize, // Token ring
    7981 - MaxUdpHeaderSize // WLAN
  )
}
