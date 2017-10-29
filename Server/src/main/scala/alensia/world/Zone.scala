package alensia.world

import com.badlogic.gdx.graphics.VertexAttributes.Usage
import com.badlogic.gdx.graphics.g3d.attributes.ColorAttribute
import com.badlogic.gdx.graphics.g3d.environment.DirectionalLight
import com.badlogic.gdx.graphics.g3d.utils.{ CameraInputController, ModelBuilder }
import com.badlogic.gdx.graphics.g3d.{ ModelBatch, ModelInstance, _ }
import com.badlogic.gdx.graphics.{ Color, GL20, PerspectiveCamera }
import com.badlogic.gdx.math.Vector3
import com.badlogic.gdx.physics.bullet.Bullet
import com.badlogic.gdx.physics.bullet.collision._
import com.badlogic.gdx.utils.{ Array => GArray }
import com.badlogic.gdx.{ ApplicationListener, Gdx }

class Zone extends ApplicationListener {

  var cam: PerspectiveCamera = _

  var camController: CameraInputController = _

  var modelBatch: ModelBatch = _

  var instances: GArray[ModelInstance] = _

  var environment: Environment = _

  var model: Model = _

  var ground: ModelInstance = _

  var ball: ModelInstance = _

  var collision: Boolean = false

  var groundShape: btCollisionShape = _

  var ballShape: btCollisionShape = _

  var groundObject: btCollisionObject = _

  var ballObject: btCollisionObject = _

  var collisionConfig: btCollisionConfiguration = _

  var dispatcher: btDispatcher = _

  override def create(): Unit = {
    this.modelBatch = new ModelBatch

    environment = new Environment
    environment.set(new ColorAttribute(ColorAttribute.AmbientLight, 0.4f, 0.4f, 0.4f, 1f))
    environment.add(new DirectionalLight().set(0.8f, 0.8f, 0.8f, -1f, -0.8f, -0.2f))

    this.cam = new PerspectiveCamera(67, Gdx.graphics.getWidth, Gdx.graphics.getHeight)

    cam.position.set(3f, 7f, 10f)
    cam.lookAt(0, 4f, 0)
    cam.update()

    this.camController = new CameraInputController(cam)

    Gdx.input.setInputProcessor(camController)

    val mb = new ModelBuilder

    mb.begin()

    mb.node.id = "ground"
    mb.part("box", GL20.GL_TRIANGLES, Usage.Position | Usage.Normal, new Material(ColorAttribute.createDiffuse(Color.RED))).box(5f, 1f, 5f)

    mb.node.id = "ball"
    mb.part("sphere", GL20.GL_TRIANGLES, Usage.Position | Usage.Normal, new Material(ColorAttribute.createDiffuse(Color.GREEN))).sphere(1f, 1f, 1f, 10, 10)

    this.model = mb.end()

    this.ground = new ModelInstance(model, "ground")
    this.ball = new ModelInstance(model, "ball")

    ball.transform.setToTranslation(0, 9f, 0)

    this.instances = new GArray()

    instances.add(ground)
    instances.add(ball)

    Bullet.init()

    ballShape = new btSphereShape(0.5f)
    groundShape = new btBoxShape(new Vector3(2.5f, 0.5f, 2.5f))

    groundObject = new btCollisionObject
    groundObject.setCollisionShape(groundShape)
    groundObject.setWorldTransform(ground.transform)

    ballObject = new btCollisionObject
    ballObject.setCollisionShape(ballShape)
    ballObject.setWorldTransform(ball.transform)

    collisionConfig = new btDefaultCollisionConfiguration
    dispatcher = new btCollisionDispatcher(collisionConfig)
  }

  override def render(): Unit = {
    camController.update()

    Gdx.gl.glClearColor(0.3f, 0.3f, 0.3f, 1f)
    Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT | GL20.GL_DEPTH_BUFFER_BIT)

    modelBatch.begin(cam)
    modelBatch.render(instances, environment)
    modelBatch.end()

    val delta = Math.min(1f / 30f, Gdx.graphics.getDeltaTime)

    if (!collision) {
      ball.transform.translate(0f, -delta, 0f)
      ballObject.setWorldTransform(ball.transform)

      collision = checkCollision()
    }
  }

  def checkCollision(): Boolean = {
    val co0 = new CollisionObjectWrapper(ballObject)
    val co1 = new CollisionObjectWrapper(groundObject)

    val ci = new btCollisionAlgorithmConstructionInfo
    ci.setDispatcher1(dispatcher)
    val algorithm = new btSphereBoxCollisionAlgorithm(null, ci, co0.wrapper, co1.wrapper, false)

    val info = new btDispatcherInfo
    val result = new btManifoldResult(co0.wrapper, co1.wrapper)

    algorithm.processCollision(co0.wrapper, co1.wrapper, info, result)

    val r = result.getPersistentManifold.getNumContacts > 0

    result.dispose()
    info.dispose()
    algorithm.dispose()
    ci.dispose()
    co1.dispose()
    co0.dispose()

    r
  }

  override def resize(width: Int, height: Int): Unit = Unit

  override def resume(): Unit = Unit

  override def pause(): Unit = Unit

  override def dispose(): Unit = {
    groundObject.dispose()
    groundShape.dispose()

    ballObject.dispose()
    ballShape.dispose()

    dispatcher.dispose()
    collisionConfig.dispose()

    modelBatch.dispose()
  }
}
