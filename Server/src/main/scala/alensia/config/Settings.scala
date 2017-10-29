package alensia.config

import akka.actor.{ ActorSystem, ExtendedActorSystem, Extension, ExtensionId, ExtensionIdProvider }
import com.typesafe.config.Config

class Settings(config: Config) extends Extension {

  object network {

    val host: String = config.getString("alensia.network.host")

    val port: Int = config.getInt("alensia.network.port")

    val maxConnections: Int = config.getInt("alensia.network.max-connections")
  }
}

object Settings extends ExtensionId[Settings] with ExtensionIdProvider {

  override def lookup: Settings.type = Settings

  override def createExtension(system: ExtendedActorSystem) =
    new Settings(system.settings.config)

  /**
   * Java API: retrieve the Settings extension for the given system.
   */
  override def get(system: ActorSystem): Settings = super.get(system)
}
