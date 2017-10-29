package alensia.world

import akka.actor.{ Actor, ActorLogging }
import com.badlogic.gdx.Application
import com.badlogic.gdx.backends.lwjgl.{ LwjglApplication, LwjglApplicationConfiguration }

class World extends Actor with ActorLogging {

  var application: Application = _

  override def preStart(): Unit = {
    super.preStart()

    log.info("Initializing a world instance.")

    val config = new LwjglApplicationConfiguration

    config.forceExit = false

    application = new LwjglApplication(new Zone, config)
  }

  override def receive: Receive = {
    case _ =>
  }

  override def postStop(): Unit = {
    super.postStop()

    log.info("Stopping the world instance.")

    application.exit()
  }
}
