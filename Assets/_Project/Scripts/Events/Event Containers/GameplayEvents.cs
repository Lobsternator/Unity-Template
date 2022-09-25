using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Gameplay;
using Template.Physics;

namespace Template.Events
{
    public class GameplayEvents : EventContainer
    {
        public Action<TestObjectA>   Test;
        public Action<TestObjectA2D> Test2D;
    }
}
