using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI.Cursor
{
    [Serializable]
    public class AnimatedCursor : CursorDefinition
    {
        public IReadOnlyList<Texture2D> Images => _images.ToList().AsReadOnly();

        public float FramesPerSecond => _framesPerSecond;

        [SerializeField] private Texture2D[] _images;

        [Range(1f, 60f)] [SerializeField] private float _framesPerSecond = 30;

        public override Vector2 Size
        {
            get
            {
                if (_images == null || _images.Length == 0) return Vector2.zero;

                var image = Images[0];

                return new Vector2(image.width, image.height);
            }
        }

        protected AnimatedCursor()
        {
        }

        public AnimatedCursor(
            string name,
            Vector2 hotspot,
            IList<Texture2D> images,
            float framePerSecond = 30) : base(name, hotspot)
        {
            Assert.IsNotNull(images, "images != null");
            Assert.IsTrue(images.Any(), "images.Any()");

            Assert.IsTrue(framePerSecond > 0, "framePerSecond > 0");

            _images = images.ToArray();
            _framesPerSecond = framePerSecond;
        }

        public override UniRx.IObservable<Texture2D> Create()
        {
            var interval = Observable
                .Interval(
                    TimeSpan.FromSeconds(1d / _framesPerSecond),
                    Scheduler.MainThreadFixedUpdate);

            return Observable
                .Range(0, _images.Length)
                .Select(i => _images[i])
                .Zip(interval, (image, _) => image)
                .RepeatSafe();
        }
    }
}