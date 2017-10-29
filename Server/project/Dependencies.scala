import sbt._

//noinspection TypeAnnotation
object Dependencies {

  object akka {

    val Organization = "com.typesafe.akka"
    val Version = "2.5.6"

    object actor {

      val api = Organization %% "akka-actor" % Version
      val slf4j = Organization %% "akka-slf4j" % Version
      val test = Organization %% "akka-testkit" % Version % "test"
    }
  }

  object gdx {

    val Organization = "com.badlogicgames.gdx"
    val Version = "1.9.7"
    val Classifier = "natives-desktop"

    object core {

      val api = Organization % "gdx" % Version
      val platform = Organization % "gdx-platform" % Version classifier Classifier
    }

    object backend {

      val lwjgl= Organization % "gdx-backend-lwjgl" % Version
      val headless = Organization % "gdx-backend-headless" % Version
    }

    object bullet {

      val api = Organization % "gdx-bullet" % Version
      val platform = Organization % "gdx-bullet-platform" % Version classifier Classifier
    }
  }

  object logback {

    val api = "ch.qos.logback" % "logback-classic" % "1.2.3"
  }

  object scalatest {

    val Version = "3.0.4"

    val api = "org.scalatest" %% "scalatest" % Version % "test"
    val scalatic = "org.scalactic" %% "scalactic" % Version % "test"
  }
}
