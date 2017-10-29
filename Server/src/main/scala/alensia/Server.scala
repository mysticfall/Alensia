package alensia

import akka.actor.{ ActorSystem, Props }
import alensia.network.Listener

object Server extends App {

  val system: ActorSystem = ActorSystem("alensia")

  val settings = config.Settings(system)

  system.actorOf(
    Props(classOf[Listener],
      settings.network.host,
      settings.network.port,
      settings.network.maxConnections), "listener")

  //system.actorOf(Props[World],"world")

  sys.addShutdownHook(system.terminate())
}
