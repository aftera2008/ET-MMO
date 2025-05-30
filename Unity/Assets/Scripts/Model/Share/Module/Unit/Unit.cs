﻿using System.Diagnostics;
using MongoDB.Bson.Serialization.Attributes;
using Unity.Mathematics;

namespace ET
{
    [ChildOf(typeof (UnitComponent))]
    [DebuggerDisplay("ViewName,nq")]
    public partial class Unit: Entity, IAwake<int>
    {
        public int ConfigId { get; set; } //配置表id

        [BsonElement]
        private float3 position; //坐标

        [BsonIgnore]
        public float3 Position
        {
            get => this.position;
            set
            {
                float3 oldPos = this.position;
                this.position = value;
                EventSystem.Instance.Publish(this.Scene(), new ChangePosition() { Unit = this, OldPos = oldPos });
            }
        }

        [BsonIgnore]
        public float3 Forward
        {
            get => math.mul(this.Rotation, math.forward());
            set => this.Rotation = quaternion.LookRotation(value, math.up());
        }

        [BsonElement]
        private quaternion rotation;

        [BsonIgnore]
        public quaternion Rotation
        {
            get => this.rotation;
            set
            {
                this.rotation = value;
                EventSystem.Instance.Publish(this.Scene(), new ChangeRotation() { Unit = this });
            }
        }

        /// <summary>
        /// 当前所在地图
        /// </summary>
        public int MapId { get; set; }

        /// <summary>
        /// 上次离开的地图
        /// </summary>
        public int LastMapId { get; set; }

        public int MapUid { get; set; }

        private CampType camp;

        /// <summary>
        /// 玩家阵营
        /// </summary>
        [BsonIgnore]
        public CampType Camp
        {
            get => this.camp;
            set
            {
                var old = this.camp;
                this.camp = value;
                EventSystem.Instance.Publish(this.Scene(), new ChangeCamp() { OldCamp = old, Unit = this });
            }
        }

        protected override string ViewName
        {
            get
            {
                return $"{this.GetType().FullName} ({this.Id})";
            }
        }
    }
}