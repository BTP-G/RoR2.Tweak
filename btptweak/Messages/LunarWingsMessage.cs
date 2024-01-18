using BtpTweak.Tweaks.ItemTweaks;
using R2API.Networking.Interfaces;
using UnityEngine.Networking;

namespace BtpTweak.Messages {

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
    }
}