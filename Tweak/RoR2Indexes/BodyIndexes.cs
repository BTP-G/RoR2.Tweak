using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace BTP.RoR2Plugin.RoR2Indexes {

    public static class BodyIndexes {

        internal enum BodyNameIndex : byte {
            None = 0,
            ArbiterBody,
            Bandit2Body,
            BrotherBody,
            BrotherHurtBody,
            CaptainBody,
            ChefBody,
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
            RobRavagerBody,
            ShopkeeperBody,
            SniperClassicBody,
            ToolbotBody,
            TreebotBody,
            VoidSurvivorBody,
            Count,
        }

        public static BodyIndex Arbiter { get; private set; }
        public static BodyIndex Bandit2 { get; private set; }
        public static BodyIndex Brother { get; private set; }
        public static BodyIndex BrotherHurt { get; private set; }
        public static BodyIndex Captain { get; private set; }
        public static BodyIndex Chef { get; private set; }
        public static BodyIndex Commando { get; private set; }
        public static BodyIndex Croco { get; private set; }
        public static BodyIndex Engi { get; private set; }
        public static BodyIndex EngiTurret { get; private set; }
        public static BodyIndex EngiWalkerTurret { get; private set; }
        public static BodyIndex EquipmentDrone { get; private set; }
        public static BodyIndex Heretic { get; private set; }
        public static BodyIndex Huntress { get; private set; }
        public static BodyIndex Loader { get; private set; }
        public static BodyIndex Mage { get; private set; }
        public static BodyIndex Merc { get; private set; }
        public static BodyIndex Pathfinder { get; private set; }
        public static BodyIndex Railgunner { get; private set; }
        public static BodyIndex RobRavager { get; private set; }
        public static BodyIndex RedMist { get; private set; }
        public static BodyIndex RobPaladin { get; private set; }
        public static BodyIndex SniperClassic { get; private set; }
        public static BodyIndex Toolbot { get; private set; }
        public static BodyIndex Treebot { get; private set; }
        public static BodyIndex VoidSurvivor { get; private set; }
        public static BodyIndex MiniVoidRaidCrabBodyPhase1 { get; private set; }
        public static BodyIndex MiniVoidRaidCrabBodyPhase2 { get; private set; }
        public static BodyIndex MiniVoidRaidCrabBodyPhase3 { get; private set; }
        internal static Dictionary<int, BodyNameIndex> BodyIndexToNameIndex { get; } = [];

        [RuntimeInitializeOnLoadMethod]
        private static void Init() {
            On.RoR2.BodyCatalog.SetBodyPrefabs += BodyCatalog_SetBodyPrefabs;
        }

        private static void BodyCatalog_SetBodyPrefabs(On.RoR2.BodyCatalog.orig_SetBodyPrefabs orig, GameObject[] newBodyPrefabs) {
            orig(newBodyPrefabs);
            for (BodyNameIndex bodyNameIndex = BodyNameIndex.None + 1; bodyNameIndex < BodyNameIndex.Count; ++bodyNameIndex) {
                var bodyIndex = BodyCatalog.FindBodyIndex(bodyNameIndex.ToString());
                if (bodyIndex == BodyIndex.None) {
                    (bodyNameIndex.ToString() + " not found!").LogError();
                } else {
                    BodyIndexToNameIndex.Add((int)bodyIndex, bodyNameIndex);
                }
            }
            $"class {typeof(BodyIndexes).FullName} has been initialized. 已成功添加{BodyIndexToNameIndex.Count}个映射。".LogInfo();
            Arbiter = BodyCatalog.FindBodyIndex("ArbiterBody");
            Bandit2 = BodyCatalog.FindBodyIndex("Bandit2Body");
            Brother = BodyCatalog.FindBodyIndex("BrotherBody");
            BrotherHurt = BodyCatalog.FindBodyIndex("BrotherHurtBody");
            Captain = BodyCatalog.FindBodyIndex("CaptainBody");
            Chef = BodyCatalog.FindBodyIndex("ChefBody");
            Commando = BodyCatalog.FindBodyIndex("CommandoBody");
            Croco = BodyCatalog.FindBodyIndex("CrocoBody");
            Engi = BodyCatalog.FindBodyIndex("EngiBody");
            EngiTurret = BodyCatalog.FindBodyIndex("EngiTurretBody");
            EngiWalkerTurret = BodyCatalog.FindBodyIndex("EngiWalkerTurretBody");
            EquipmentDrone = BodyCatalog.FindBodyIndex("EquipmentDroneBody");
            Heretic = BodyCatalog.FindBodyIndex("HereticBody");
            Huntress = BodyCatalog.FindBodyIndex("HuntressBody");
            Loader = BodyCatalog.FindBodyIndex("LoaderBody");
            Mage = BodyCatalog.FindBodyIndex("MageBody");
            Merc = BodyCatalog.FindBodyIndex("MercBody");
            MiniVoidRaidCrabBodyPhase1 = BodyCatalog.FindBodyIndex("MiniVoidRaidCrabBodyPhase1");
            MiniVoidRaidCrabBodyPhase2 = BodyCatalog.FindBodyIndex("MiniVoidRaidCrabBodyPhase2");
            MiniVoidRaidCrabBodyPhase3 = BodyCatalog.FindBodyIndex("MiniVoidRaidCrabBodyPhase3");
            Pathfinder = BodyCatalog.FindBodyIndex("PathfinderBody");
            Railgunner = BodyCatalog.FindBodyIndex("RailgunnerBody");
            RedMist = BodyCatalog.FindBodyIndex("RedMistBody");
            RobPaladin = BodyCatalog.FindBodyIndex("RobPaladinBody");
            RobRavager = BodyCatalog.FindBodyIndex("RobRavagerBody");
            SniperClassic = BodyCatalog.FindBodyIndex("SniperClassicBody");
            Toolbot = BodyCatalog.FindBodyIndex("ToolbotBody");
            Treebot = BodyCatalog.FindBodyIndex("TreebotBody");
            VoidSurvivor = BodyCatalog.FindBodyIndex("VoidSurvivorBody");
        }
    }
}