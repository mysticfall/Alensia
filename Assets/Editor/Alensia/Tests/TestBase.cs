﻿using NUnit.Framework;
using UnityEngine;

namespace Alensia.Tests
{
    public abstract class TestBase : AssertionHelper
    {
        public const float Tolerance = 0.01f;

        [SetUp]
        public virtual void SetUp()
        {
            ClearScene();
            PrepareScene();
        }

        public void ClearScene()
        {
            foreach (var obj in Object.FindObjectsOfType<GameObject>())
            {
                Object.DestroyImmediate(obj);
            }
        }

        protected virtual void PrepareScene()
        {
        }
    }
}