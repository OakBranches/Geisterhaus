using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    public struct Projectile
    {
        public Rigidbody2D rb;
        public GameObject instance;

        public Projectile(Rigidbody2D _rb, GameObject _instance)
        {
            rb = _rb;
            instance = _instance;
        }
    }
}
