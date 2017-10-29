package alensia.network

import java.net.InetSocketAddress

import akka.actor.{ Actor, ActorLogging, ActorRef, Props }
import akka.io.{ IO, Udp }
import alensia.network.PacketHandler.{ ConnectAccept, ConnectRequest }

class Listener(
  val host: String,
  val port: Int,
  val maxConnections: Int) extends Actor with ActorLogging {

  import context.system

  override def preStart() {
    super.preStart()

    log.info("Listening for UDP connections on {}:{}.", host, port)

    IO(Udp) ! Udp.Bind(self, new InetSocketAddress(host, port))
  }

  def receive: Receive = {
    case Udp.Bound(_) => context become {
      ready {
        sender()
      }
    }
  }

  def ready(socket: ActorRef): Receive = {
    case Udp.Received(data, remote) =>
      val peerId = remote.toString.dropWhile(_ == '/')

      PacketHandler(data) match {
        case ConnectRequest =>
          val id = ConnectRequest.connectionId(data)
          val key = ConnectRequest.connectionKey(data)

          log.debug("Connection request from '{}' (id: {}, key: '{}').", remote, id, key)

          println(s" - id = $id")
          println(s" - key = $key")

          context.actorOf(Props(classOf[Peer], remote, socket), peerId)

          socket ! Udp.Send(ConnectAccept(id), remote)
        case _ =>
          context.actorSelection(self.path / peerId) ! data
      }
    case Udp.Unbind => socket ! Udp.Unbind
    case Udp.Unbound => context.stop(self)
  }

  override def postStop() {
    super.postStop()

    log.info("Stopping UDP listener.")
  }
}
