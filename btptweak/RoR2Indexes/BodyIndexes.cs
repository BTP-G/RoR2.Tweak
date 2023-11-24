using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak.RoR2Indexes {

    public static class BodyIndexes {

        static BodyIndexes() {
            for (BodyNameIndex bodyNameIndex = BodyNameIndex.None + 1; bodyNameIndex < BodyNameIndex.Count; ++bodyNameIndex) {
                var bodyIndex = BodyCatalog.FindBodyIndex(bodyNameIndex.ToString());
                if (bodyIndex == BodyIndex.None) {
                    Debug.LogWarning(bodyNameIndex.ToString() + " not found!");
                } else {
                    BodyIndexToNameIndex.Add((int)bodyIndex, bodyNameIndex);
                }
            }
            Debug.Log($"class {typeof(BodyIndexes).FullName} has been initialized. 已成功添加{BodyIndexToNameIndex.Count}个映射。");
        }

        internal enum BodyNameIndex : byte {
            None = 0,
            ArbiterBody,
            Bandit2Body,
            BrotherBody,
            BrotherHurtBody,
            CaptainBody,
            CHEF,
            CommandoBody,
            CrocoBody,
            EngiBody,
            EngiTurretBody,
            EngiWalkerTurretBody,
            HereticBody,
            HuntressBody,
            LoaderBody,
            MageBody,
            MercBody,
            MiniVoidRaidCrabBodyPhase1,
            MiniVoidRaidCrabBodyPhase2,
            MiniVoidRaidCrabBodyPhase3,
            PathfinderBody,
            RailgunnerBody,
            RedMistBody,
            RobPaladinBody,
            SniperClassicBody,
            ToolbotBody,
            TreebotBody,
            VoidSurvivorBody,
            Count,
        }

        public static BodyIndex ArbiterBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.ArbiterBody.ToString());
        public static BodyIndex Bandit2Body { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.Bandit2Body.ToString());
        public static BodyIndex BrotherBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.BrotherBody.ToString());
        public static BodyIndex BrotherHurtBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.BrotherHurtBody.ToString());
        public static BodyIndex CaptainBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.CaptainBody.ToString());
        public static BodyIndex Chef { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.CHEF.ToString());
        public static BodyIndex CommandoBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.CommandoBody.ToString());
        public static BodyIndex CrocoBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.CrocoBody.ToString());
        public static BodyIndex EngiBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.EngiBody.ToString());
        public static BodyIndex EngiTurretBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.EngiTurretBody.ToString());
        public static BodyIndex EngiWalkerTurretBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.EngiWalkerTurretBody.ToString());
        public static BodyIndex EquipmentDroneBody { get; } = BodyCatalog.FindBodyIndex("EquipmentDroneBody");
        public static BodyIndex HereticBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.HereticBody.ToString());
        public static BodyIndex HuntressBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.HuntressBody.ToString());
        public static BodyIndex LoaderBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.LoaderBody.ToString());
        public static BodyIndex MageBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.MageBody.ToString());
        public static BodyIndex MercBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.MercBody.ToString());
        public static BodyIndex PathfinderBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.PathfinderBody.ToString());
        public static BodyIndex RailgunnerBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.RailgunnerBody.ToString());
        public static BodyIndex RedMistBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.RedMistBody.ToString());
        public static BodyIndex RobPaladinBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.RobPaladinBody.ToString());
        public static BodyIndex SniperClassicBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.SniperClassicBody.ToString());
        public static BodyIndex ToolbotBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.ToolbotBody.ToString());
        public static BodyIndex TreebotBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.TreebotBody.ToString());
        public static BodyIndex VoidSurvivorBody { get; } = BodyCatalog.FindBodyIndex(BodyNameIndex.VoidSurvivorBody.ToString());
        internal static Dictionary<int, BodyNameIndex> BodyIndexToNameIndex { get; } = new();
    }
}