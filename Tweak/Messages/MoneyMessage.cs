using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Messages {

    internal struct MoneyMessage(CharacterMaster master, uint amount, bool changeToRemoveMoney = true) : INetMessage {
        private CharacterMaster _master = master;
        private uint _amount = amount;
        private bool _changeToRemoveMoney = changeToRemoveMoney;

        public void Deserialize(NetworkReader reader) {
            _master = reader.ReadGameObject().GetComponent<CharacterMaster>();
            _amount = reader.ReadUInt32();
            _changeToRemoveMoney = reader.ReadBoolean();
        }

        public readonly void OnReceived() {
            if (_changeToRemoveMoney) {
                _master.money -= _amount > _master.money ? _master.money : _amount;
            } else {
                _master.GiveMoney(_amount);
            }
        }

        public readonly void Serialize(NetworkWriter writer) {
            writer.Write(_master.gameObject);
            writer.Write(_amount);
            writer.Write(_changeToRemoveMoney);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() {
            if (R2API.Networking.NetworkingAPI.RegisterMessageType<MoneyMessage>()) {
                "MoneyMessage Register Successd!".LogMessage();
            } else {
                "MoneyMessage Register Failed!".LogError();
            }
        }
    }
}