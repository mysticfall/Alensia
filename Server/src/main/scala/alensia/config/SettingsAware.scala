package alensia.config

import akka.actor.Actor

trait SettingsAware {
  this: Actor =>

  val settings: Settings = Settings(context.system)
}
