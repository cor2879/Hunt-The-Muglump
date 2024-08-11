#pragma warning disable CS0649
/**************************************************
 *  NetArrowBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(SpriteRenderer))]
    public class NetArrowBehaviour : ArrowBehaviour
    {
        public override ArrowType ArrowType { get => ArrowType.NetArrow; }

        public override void OnDestinationReached()
        {
            var prefab = this.Destination.GetMuglumpBehaviour() != null ? GameManager.Instance.creatureNetPrefab : GameManager.Instance.netPrefab;
            var net = Instantiate(prefab, this.transform.position, Quaternion.identity).GetComponent<NetBehaviour>();
            net.ContinuePath(this.transform.position, this.Direction, this.Destination, this.transform.rotation, this.Velocity * 0.5f, 0.5f, 1.0f);
            CameraManager.Follow(net.gameObject);

            base.OnDestinationReached();
        }

        public override void Fire(RoomBehaviour startingRoom, Direction direction, float velocity, Action onDestinationReached)
        {
            this.onFireComplete = onDestinationReached;
            base.Fire(startingRoom, direction, velocity, 0.5f);
        }
    }
}
