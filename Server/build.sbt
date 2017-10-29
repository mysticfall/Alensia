name := "alensia-server"
version := "0.1-SNAPSHOT"
scalaVersion := "2.12.4"
organization := "alensia"

resolvers += "Artima Maven Repository" at "http://repo.artima.com/releases"

import Dependencies._

libraryDependencies ++= Seq(
  akka.actor.api,
  akka.actor.slf4j,
  gdx.core.api,
  gdx.core.platform,
  gdx.backend.lwjgl,
  gdx.backend.headless,
  gdx.bullet.api,
  gdx.bullet.platform,
  logback.api)

libraryDependencies in Test ++= Seq(
  scalatest.api,
  scalatest.scalatic,
  akka.actor.test)
