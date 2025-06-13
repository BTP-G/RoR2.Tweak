using BTP.RoR2Plugin.Tweaks.ItemTweaks;
using R2API.Networking.Interfaces;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Messages {

    internal struct LunarWingsMessage(LunarWingsState state) : INetMessage {
        private byte _state = (byte)state;

        public void Deserialize(NetworkReader reader) {
            _state = reader.ReadByte();
        }

        public readonly void OnReceived() {
            LunarWingsTweak.UpdateLunarWingsState((LunarWingsState)_state);
        }

        public readonly void Serialize(NetworkWriter writer) {
            writer.Write(_state);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() {
            if (R2API.Networking.NetworkingAPI.RegisterMessageType<LunarWingsMessage>()) {
                "LunarWingsMessage Register Successd!".LogMessage();
            } else {
                "LunarWingsMessage Register Failed!".LogError();
            }
        }
    }
}